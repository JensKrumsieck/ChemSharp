using ChemSharp.Math;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ChemSharp.Molecules.Math
{
    public static class ChemMathUtil
    {
        // <summary>
        /// Wrapper for Vector3.Centroid()
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Vector3 Centroid(this IEnumerable<Atom> input) => (from atom in input select atom.Location).Centroid();
    }
}
