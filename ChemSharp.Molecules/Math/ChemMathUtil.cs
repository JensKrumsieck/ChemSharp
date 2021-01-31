using ChemSharp.Math;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ChemSharp.Molecules.Math
{
    public static class ChemMathUtil
    {
        /// <summary>
        /// Wrapper for Vector3.Centroid()
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Vector3 Centroid(this IEnumerable<Atom> input) => (from atom in input select atom.Location).Centroid();

        /// <summary>
        /// Centers the molecule at point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="atoms"></param>
        public static Func<Vector3, Vector3> CenterMapping(this IEnumerable<Atom> atoms)
        {
            var centroid = Centroid(atoms);
            return s => s - centroid;
        }

        /// <summary>
        /// Gets the mean plane of a list of atoms
        /// </summary>
        /// <returns></returns>
        public static Plane MeanPlane(IList<Atom> atoms)
        {
            //calculate Centroid first
            //get the centroid
            var centroid = Centroid(atoms);

            //subtract centroid from each point... & build matrix of that
            var A = Matrix<float>.Build.Dense(3, atoms.Count);
            for (var x = 0; x < atoms.Count; x++)
            {
                A[0, x] = (atoms[x].Location - centroid).X;
                A[1, x] = (atoms[x].Location - centroid).Y;
                A[2, x] = (atoms[x].Location - centroid).Z;
            }

            //get svd
            var svd = A.Svd(true);

            //get plane unit vector
            var a = svd.U[0, 2];
            var b = svd.U[1, 2];
            var c = svd.U[2, 2];

            var d = -centroid.Dot(new Vector3(a, b, c));

            return new Plane(a, b, c, d);
        }
    }
}

