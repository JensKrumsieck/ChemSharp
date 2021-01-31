using ChemSharp.Molecules;
using ChemSharp.Rendering.Extensions;
using ChemSharp.Rendering.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace ChemSharp.Rendering
{
    public class SvgMolecule
    {
        protected Molecule Molecule;

        public SvgMolecule(Molecule molecule) => Molecule = molecule;

        public IEnumerable<ISvgItem> Tags => AtomTags.Concat(BondTags);

        private IEnumerable<ISvgItem> AtomTags =>
            from atom in Molecule.Atoms
            where (atom.Symbol != "C" && atom.Symbol != "H") || Molecule.Bonds == null || Molecule.Bonds.Count == 0
            select atom.ToSvg()
            into svgAtom
            select svgAtom.Tag;

        private IEnumerable<ISvgItem> BondTags =>
            Molecule.Bonds.
                Where(bond => bond.Atoms.All(s => s.Symbol != "H")).
                Select(bond => bond.ToSvg().Tag);

    }
}
