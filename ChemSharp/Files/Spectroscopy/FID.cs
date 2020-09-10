using ChemSharp.Math;
using System.Collections.Generic;
using System.Numerics;
using ChemSharp.Extensions;

namespace ChemSharp.Files.Spectroscopy
{
    public class FID : DataBinaryFile<int>, IYSpectrumFile
    {
        public float[] YData { get; set; }

        public Complex[] FIDData { get; set; }
        public FID(string path) : base(path)
        {
            FIDData = ReadData(ConvertedData);
            YData = FourierTransform(FIDData);
        }

        /// <summary>
        /// Read in raw data to complex
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static Complex[] ReadData(IReadOnlyList<int> data)
        {
            var complex = new Complex[data.Count / 2];

            for (var i = 0; i < data.Count / 2; i++)
                complex[i] = new Complex(
                    data[2 * i],
                    data[2 * i + 1]);

            return complex;
        }

        /// <summary>
        /// Fourier Transforms fid data and handles conversions
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        private static float[] FourierTransform(Complex[] fid) => fid.Radix2FFT().ToInt().FftShift().ToFloat();
    }
}
