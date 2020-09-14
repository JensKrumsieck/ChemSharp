using ChemSharp.Molecule;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ChemSharp.Math
{
    public static class ChemistryMath
    {
        /// <summary>
        /// Wrapper for Vector3.Centroid()
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Vector3 Centroid(this IEnumerable<Atom> input) => (from atom in input select atom.Location).Centroid();

        /// <summary>
        /// Calculates Mean Plane of given atom enumerable
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Plane MeanPlane(this IEnumerable<Atom> input)
        {
            var data = input.ToArray();
            var count = data.Length;
            var centroid = data.Centroid();
            var matrix = Matrix<float>.Build.Dense(3, count);
            for (var i = 0; i < count; i++)
            {
                var vec = data[i].Location - centroid;
                matrix[0, i] = vec.X;
                matrix[1, i] = vec.Y;
                matrix[2, i] = vec.Z;
            }

            var svd = matrix.Svd();
            var a = svd.U[0, 2];
            var b = svd.U[1, 2];
            var c = svd.U[2, 2];
            return new Plane(a, b, c, -centroid.Dot(new Vector3(a, b, c)));
        }

        /// <summary>
        /// Wrapper for MathUtil method
        /// </summary>
        /// <param name="a"></param>
        /// <param name="p"></param>
        public static float DistanceToPlane(this Atom a, Plane p) => p.Distance(a.Location);

        /// <summary>
        /// Calculates mean plane distance (for huge number of atoms use separate methods)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="atoms"></param>
        /// <returns></returns>
        public static float DistanceToMeanPlane(this Atom a, IEnumerable<Atom> atoms) => atoms.MeanPlane().Distance(a.Location);
    }
}
