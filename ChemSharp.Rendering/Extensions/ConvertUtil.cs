using ChemSharp.Molecules;
using ChemSharp.Rendering.Svg;

namespace ChemSharp.Rendering.Extensions
{
    public static class ConvertUtil
    {
        /// <summary>
        /// Converts Molecule to Svg Class
        /// </summary>
        /// <param name="mol"></param>
        /// <returns></returns>
        public static SvgMolecule ToSvg(this Molecule mol) => new SvgMolecule(mol);

        /// <summary>
        /// Converts Atom to Svg Class
        /// </summary>
        /// <param name="atom"></param>
        /// <returns></returns>
        public static SvgAtom ToSvg(this Atom atom) => new SvgAtom(atom);

        /// <summary>
        /// Converts Atom to Svg Class
        /// </summary>
        /// <param name="bond"></param>
        /// <returns></returns>
        public static SvgBond ToSvg(this Bond bond) => new SvgBond(bond);
    }
}
