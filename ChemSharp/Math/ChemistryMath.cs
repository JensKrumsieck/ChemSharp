using ChemSharp.Molecule;
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
    }
}
