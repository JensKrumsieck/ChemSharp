using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ChemSharp.Extensions
{
    public static class MathUtil
    {
        /// <summary>
        /// Converts a vector of fractional coordinates to cartesian
        /// </summary>
        /// <param name="fractional"></param>
        /// <param name="conversionMatrix">a 3x3 conversion matrix</param>
        /// <returns></returns>
        public static Vector3 FractionalToCartesian(Vector3 fractional, Vector3[] conversionMatrix)
        {
            var vector = new float[3];
            for (var i = 0; i <= 2; i++) vector[i] = fractional.X* conversionMatrix[i].X + fractional.Y * conversionMatrix[i].Y +
                    fractional.Z * conversionMatrix[i].Y;
            return vector.ToVector3();
        }

        /// <summary>
        /// Builds conversion matrix out of cell parameters
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <param name="gamma"></param>
        /// <returns></returns>
        public static Vector3[] ConversionMatrix(float a, float b, float c, float alpha, float beta, float gamma)
        {
            var line1 = new Vector3(a, b * MathF.Cos(gamma * MathF.PI / 180f), c * MathF.Cos(beta * MathF.PI / 180f));
            var line2 = new Vector3(
                0, 
                a * MathF.Sin(gamma * MathF.PI / 180f),
                c * (MathF.Cos(alpha * MathF.PI / 180f) -
                     MathF.Cos(beta * MathF.PI / 180f) * MathF.Cos(gamma * MathF.PI / 180f)) /
                MathF.Sin(gamma * MathF.PI / 180f));

            var line3 = new Vector3(
                0, 
                0,
                c * (MathF.Sqrt(1f - MathF.Pow(MathF.Cos(alpha * MathF.PI / 180f), 2f) -
                    MathF.Pow(MathF.Cos(beta * MathF.PI / 180f), 2f) -
                    MathF.Pow(MathF.Cos(gamma * MathF.PI / 180f), 2f) + 2f *
                    MathF.Cos(alpha * MathF.PI / 180f) * MathF.Cos(beta * MathF.PI / 180f) *
                    MathF.Cos(gamma * MathF.PI / 180f))) / MathF.Sin(gamma * MathF.PI / 180f));
            return new[] {line1, line2, line3};
        }

        /// <summary>
        /// Calculates the centroid of given vectors by 1/m sum_(i=0 to m) v_i
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Vector3 Centroid(this IEnumerable<Vector3> input)
        {
            var array = input.ToArray();
            return array.Sum() / array.Length;
        }

        /// <summary>
        /// Calculates the Sum of given Vectors
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Vector3 Sum(this IEnumerable<Vector3> input)
        {
            var array = input as Vector3[] ?? input.ToArray();
            var sumX = array.Sum(s => s.X);
            var sumY = array.Sum(s => s.Y);
            var sumZ = array.Sum(s => s.Z);
            return new Vector3(sumX, sumY, sumZ);
        }

        public static Complex[] FFT(this Complex[] x)
        {
           var N = x.Length;
           var y = new Complex[N];

           Complex[] d, D, e, E;

           if (N == 1) return x;
           int k;
           e = new Complex[N / 2];
           d= new Complex[N / 2];

           for (k = 0; k < N / 2; k++)
           {
               e[k] = x[2 * k];
               d[k] = x[2 * k + 1];
           }

            D = FFT(d);
            E = FFT(e);

            for (k = 0; k < N/2; k++)
            {
                var term = Complex.FromPolarCoordinates(1, -2 * Math.PI * k / N);
                D[k] *= term;
            }

            for (k = 0; k < N / 2; k++)
            {
                y[k] = E[k] + D[k];
                y[k + N / 2] = E[k] - D[k];
            }

            return y;
        }
    }
}
