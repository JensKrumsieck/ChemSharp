using ChemSharp.Molecules;
using ChemSharp.Rendering.Svg;
using System;
using System.Drawing;
using System.Numerics;

namespace ChemSharp.Rendering.Extensions
{
    [Obsolete("Package will be removed soon")]
    public static class ConvertUtil
    {
        /// <summary>
        /// Converts Molecule to Svg Class
        /// </summary>
        /// <param name="mol"></param>
        /// <returns></returns>
        public static SvgMolecule ToSvg(this Molecule mol) => new(mol);

        /// <summary>
        /// Converts Atom to Svg Class
        /// </summary>
        /// <param name="atom"></param>
        /// <returns></returns>
        public static SvgAtom ToSvg(this Atom atom) => new(atom);

        /// <summary>
        /// Converts Atom to Svg Class
        /// </summary>
        /// <param name="bond"></param>
        /// <returns></returns>
        public static SvgBond ToSvg(this Bond bond) => new(bond);

        public static Vector3 HexColorToVector(this string hexColor)
        {
            var col = ColorTranslator.FromHtml(hexColor);
            return new Vector3(col.R / 255f, col.G / 255f, col.B / 255f);
        }
    }
}
