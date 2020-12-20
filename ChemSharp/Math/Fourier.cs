using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ChemSharp.Math
{
    public static class Fourier
    {/// <summary>
        /// Shift zero-frequency component to center of spectrum
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IEnumerable<int> FftShift(this int[] input)
        {
            var n = (int)System.Math.Ceiling(input.Length / 2.0);
            for (var i = n; i < input.Length; i++) yield return input[i];
            for (var i = 0; i < n; i++) yield return input[i];
        }

        /// <summary>
        /// For compatibility with TopSpins Fourier Transformation force RadixFFT
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Complex[] Radix2FFT(this Complex[] x)
        {
            if (!MathUtil.PowerOf2(x.Length))
            {
                var padding = (MathUtil.NextPowerOf2(x.Length) - x.Length) / 2;
                var paddingSeq = new Complex[padding];
                x = paddingSeq.Concat(x).Concat(paddingSeq).ToArray();
            }

            MathNet.Numerics.IntegralTransforms.Fourier.Forward(x);
            return x;
        }
    }
}
