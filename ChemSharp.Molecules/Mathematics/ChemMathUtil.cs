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
        /// See BondTo Method
        /// </summary>
        public const float Delta = 25f;

        /// <summary>
        /// Centers the molecule at point
        /// </summary>
        /// <param name="atoms"></param>
        public static Func<Vector3, Vector3> CenterMapping(this IEnumerable<Atom> atoms)
        {
            var centroid = MathV.Centroid(atoms.Select(s => s.Location));
            return s => s - centroid;
        }

        /// <summary>
        /// Tests if Atom is Bond to another based on distance!
        /// allow uncertainty of <see cref="Delta"/> overall
        /// </summary>
        /// <param name="atom"></param>
        /// <param name="test"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        public static bool BondToByCovalentRadii(this Atom atom, Atom test, float delta = Delta)
        {
            if (atom?.CovalentRadius is null || test?.CovalentRadius is null) return false;
            return atom.DistanceTo(test) < (atom.CovalentRadius + (float)test.CovalentRadius + delta) / 100f;
        }
    }
}

