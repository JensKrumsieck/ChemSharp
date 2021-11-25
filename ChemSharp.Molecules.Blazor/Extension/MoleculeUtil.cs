using ChemSharp.Molecules;
using System.Numerics;

namespace BlazorThreeJS.Extension
{
    public static class MoleculeUtil
    {
        /// <summary>
        /// Calculates Bond Rotation in 3D Space and returns midpoint and quaternion
        /// </summary>
        /// <param name="bond"></param>
        /// <returns></returns>
        public static (Vector3 position, Quaternion rotation) CalculateRotation(this Bond bond)
        {
            var start = bond.Atom1.Location;
            var end = bond.Atom2.Location;
            var loc = Vector3.Lerp(start, end, .5f);

            var up = new Vector3(0, 1, 0);
            var line = end - start;
            var axis = Vector3.Cross(up, line);
            var rad = MathF.Acos(Vector3.Dot(up, Vector3.Normalize(line)));
            var quat = Quaternion.CreateFromAxisAngle(Vector3.Normalize(axis), rad);

            return (loc, quat);
        }
    }
}
