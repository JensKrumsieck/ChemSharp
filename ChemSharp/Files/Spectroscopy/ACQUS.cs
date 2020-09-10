﻿using ChemSharp.Extensions;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
            Type = Regex.Replace(Parameters.TryAndGet("$NUC1"), "<|>", "");

            //get seconds
            SecondsData = MathUtil.LinearRange(0, Count - 1 / SweepWidth, Count).ToArray();
            //get frequency
            FrequencyData = MathUtil.LinearRange(-SweepWidth / 2, SweepWidth / 2, Count).ToArray();


            FFTSize = MathUtil.PowerOf2(Count) ? Count : MathUtil.NextPowerOf2(Count);
            var procsPath = Path.Replace("acqus", "") + "/pdata/1/procs";
            if (File.Exists(procsPath)) PPMOffset = new PROCS(procsPath).Offset;

            //get ppm
            PPMData = FFTSize == Count 
                ? FrequencyData.Select(s => s / Frequency * 1e6f).ToArray() 
                : MathUtil.LinearRange(-SweepWidth / 2, SweepWidth / 2, FFTSize).Select(s => s / Frequency * 1e6f).ToArray();

            //if processing offset is not 0, correct ppm scale
            PPMData = PPMData.Select(s => s + PPMOffset - PPMData.Max()).ToArray();

            //set PPM to X by defaults
            XData = PPMData;
        }

        public float[] XData { get; set; }

        public float[] FrequencyData { get; set; }

        public float[] SecondsData { get; set; }

        public float[] PPMData { get; set; }
    }
}