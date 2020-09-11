using ChemSharp.Extensions;
using ChemSharp.Math;
using System.IO;
using System.Linq;

namespace ChemSharp.Files.Spectroscopy
{
    public class ACQUS : JCampFile, IXSpectrumFile
    {
        public float SweepWidth;
        /// <summary>
        /// Raw Datapoints count
        /// </summary>
        public int Count;
        /// <summary>
        /// As Bruker uses Radix FFT this needs to be the next power of 2 of count (or count if that is a power of 2)
        /// </summary>
        public int FFTSize;
        public float Frequency;
        public string Type;
        public float PPMOffset;

        public ACQUS(string path) : base(path)
        {
            Init();
        }

        private void Init()
        {
            SweepWidth = Parameters.TryAndGet("$SW_h").ToFloat();
            Count = Parameters.TryAndGet("$TD").ToInt() / 2;
            Frequency = Parameters.TryAndGet("$SFO1").ToFloat() * 1e6f;
            Type = Parameters.TryAndGet("$NUC1").Replace("<", "").Replace(">", "");

            //get seconds
            SecondsData = CollectionsUtil.LinearRange(0, Count - 1 / SweepWidth, Count).ToArray();
            //get frequency
            FrequencyData = CollectionsUtil.LinearRange(-SweepWidth / 2, SweepWidth / 2, Count).ToArray();


            FFTSize = MathUtil.PowerOf2(Count) ? Count : MathUtil.NextPowerOf2(Count);
            var procsPath = Path.Replace("acqus", "") + "/pdata/1/procs";
            if (File.Exists(procsPath)) PPMOffset = new PROCS(procsPath).Offset;

            //get ppm
            PPMData = FFTSize == Count
                ? FrequencyData.Select(s => s / Frequency * 1e6f).ToArray()
                : CollectionsUtil.LinearRange(-SweepWidth / 2, SweepWidth / 2, FFTSize).Select(s => s / Frequency * 1e6f).ToArray();

            //if processing offset is not 0, correct ppm scale
            var max = PPMData.Max();
            PPMData = PPMData.Select(s => s + PPMOffset - max).ToArray();

            //set PPM to X by defaults
            XData = PPMData;
        }

        public float[] XData { get; set; }

        public float[] FrequencyData { get; set; }

        public float[] SecondsData { get; set; }

        public float[] PPMData { get; set; }
    }
}
