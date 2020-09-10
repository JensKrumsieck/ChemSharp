using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ChemSharp.Extensions
{
    public static class Fourier
    {
        /// <summary>
        /// Shift zero-frequency component to center of spectrum
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IEnumerable<int> FftShift(this int[] input)
        {
            var n = (int)Math.Ceiling(input.Length / 2.0);
            for (var i = n; i < input.Length; i++) yield return input[i];
            for (var i = 0; i < n; i++) yield return input[i];
        }

        /// <summary>
        /// Radix 2 FFT Algorithm (inspired by Math.NET implementation)
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

            Radix2Reorder(x);
            for (var levelSize = 1; levelSize < x.Length; levelSize *= 2)
                for (var k = 0; k < levelSize; k++)
                    Radix2Step(x, -1, levelSize, k);
            return FullRescale(x);
        }

        private static void Radix2Step(IList<Complex> x, int exponentSign, int levelSize, int k)
        {
            var exponent = (exponentSign * k) * Math.PI / levelSize;
            var w = new Complex(Math.Cos(exponent), Math.Sin(exponent));
            var step = levelSize << 1;
            for (var i = k; i < x.Count; i += step)
            {
                var ai = x[i];
                var t = w * x[i + levelSize];
                x[i] = ai + t;
                x[i + levelSize] = ai - t;
            }
        }

        private static void Radix2Reorder<T>(IList<T> input)
        {
            var j = 0;
            for (var i = 0; i < input.Count - 1; i++)
            {
                if (i < j)
                {
                    var temp = input[i];
                    input[i] = input[j];
                    input[j] = temp;
                }

                var m = input.Count;
                do
                {
                    m >>= 1;
                    j ^= m;
                } while ((j & m) == 0);
            }
        }


        /// <summary>
        /// Rescales Fourier Data
        /// </summary>
        /// <param name="samples"></param>
        /// <returns></returns>
        private static Complex[] FullRescale(Complex[] samples)
        {
            var scalingFactor = 1.0 / samples.Length;
            for (var i = 0; i < samples.Length; i++) samples[i] *= scalingFactor;
            return samples;
        }
    }
}
