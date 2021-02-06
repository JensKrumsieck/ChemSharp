using ChemSharp.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ChemSharp.Molecules.Mathematics
{
    public static class ChemMathUtil
    {

        /// <summary>
        /// Centers the molecule at point
        /// </summary>
        /// <param name="atoms"></param>
        public static Func<Vector3, Vector3> CenterMapping(this IEnumerable<Atom> atoms)
        {
            var centroid = MathV.Centroid(atoms.Select(s => s.Location));
            return s => s - centroid;
        }
    }
}

