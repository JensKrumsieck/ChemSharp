using System.IO;
using System.Linq;
using ChemSharp.Extensions;
using System.Text.RegularExpressions;

namespace ChemSharp.Files.Spectroscopy
{
    public class ACQUS : JCampFile, IXSpectrumFile
    {
        public float SweepWidth;
        public int Count;
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
            Type = Regex.Replace(Parameters.TryAndGet("$NUC1"), "<|>", "");

            //calculate frequency domain
            FrequencyData = new float[Count];
            for (var i = 0; i < Count; i++) FrequencyData[i] = (SweepWidth / Count * i) - SweepWidth/2;
            
            //calculate ppm scale
            XData = FrequencyData.Select(s => s / Frequency * 1e6f).ToArray();

            var procsPath = Path.Replace("acqus", "") + "/pdata/1/procs";
            if (File.Exists(procsPath)) PPMOffset = LoadOffset(procsPath);

            //if processing offset is not 0, correct ppm scale
            XData = XData.Select(s => s + PPMOffset - XData.Max()).ToArray();
        }

        /// <summary>
        /// Loads Procs file
        /// </summary>
        /// <param name="path"></param>
        public float LoadOffset(string path) => new PROCS(path).Offset;


        /// <summary>
        /// Load PROCS File
        /// </summary>
        /// <param name="path"></param>

        public float[] XData { get; set; }

        public float[] FrequencyData { get; set; }
    }
}
