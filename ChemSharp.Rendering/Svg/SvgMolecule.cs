using ChemSharp.Export;
using ChemSharp.Molecules;
using ChemSharp.Molecules.Extensions;
using ChemSharp.Rendering.Extensions;
using ChemSharp.Rendering.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace ChemSharp.Rendering.Svg
{
    public class SvgMolecule : IExportable
    {
        public readonly Molecule Molecule;
        public List<SvgAtom> Atoms;
        public List<SvgBond> Bonds;

        public SvgMolecule(Molecule molecule)
        {
            Molecule = molecule;
            Atoms = Molecule.Atoms.Select(atom => atom.ToSvg()).ToList();
            Bonds = Molecule.Bonds.Select(bond => bond.ToSvg()).ToList();
            AddImplicitHydrogens();
            AddAromaticBonds();
            Tags = AtomTags.Concat(BondTags);
        }

        public IEnumerable<ISvgItem> Tags { get; }

        /// <summary>
        /// Returns AtomTags
        /// </summary>
        private IEnumerable<ISvgItem> AtomTags =>
            from atom in Atoms
            where (atom.Atom.Symbol != "C" && atom.Atom.Symbol != "H") || Molecule.Bonds == null || Molecule.Bonds.Count == 0
            select atom.Tag;

        /// <summary>
        /// Returns Bond Tags
        /// </summary>
        private IEnumerable<ISvgItem> BondTags =>
            from bond in Bonds
            where (bond.Bond.Atoms.All(s => s.Symbol != "H"))
            select bond.Tag;

        /// <summary>
        /// Adds HasImplicitHydrogen Flag to Atoms matching criteria
        /// </summary>
        private void AddImplicitHydrogens()
        {
            foreach (var atom in Atoms
                .Where(atom => !new List<string> { "H", "C" }
                    .Contains(atom.Atom.Symbol) && Molecule.Neighbors(atom.Atom).Count(s => s.Symbol == "H") == 1))
            {
                atom.HasImplicitHydrogen = true;
            }
        }

        /// <summary>
        /// Adds aromatic bonds as double bonds if fits saturation
        /// </summary>
        private void AddAromaticBonds()
        {
            foreach (var bond in Bonds.Where(s => s.Bond.IsAromatic))
            {
                var a1 = bond.Bond.Atom1;
                var a2 = bond.Bond.Atom2;
                if (Molecule.Saturation(a1) + 1 == Element.DesiredSaturation[a1.Symbol]
                    && Molecule.Saturation(a2) + 1 == Element.DesiredSaturation[a2.Symbol])
                    bond.Bond.Order = 2;
            }
        }
    }
}
