using ChemSharp.Molecules;
using ChemSharp.Molecules.Extensions;
using ChemSharp.Rendering.Extensions;
using ChemSharp.Rendering.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace ChemSharp.Rendering
{
    public class SvgMolecule
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
            _tags = AtomTags.Concat(BondTags);
        }

        private IEnumerable<ISvgItem> _tags;

        public IEnumerable<ISvgItem> Tags => _tags;

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

        /// <summary>
        /// Returns Molecule as Serialized SVG String
        /// </summary>
        /// <returns></returns>
        public string GetSerializedDocument()
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("xlink", "http://www.w3.org/1999/xlink");
            var doc = GetDocument();
            var ser = new XmlSerializer(typeof(SvgRoot));
            using var sw = new UTF8Writer();
            using var xw = XmlWriter.Create(sw, new XmlWriterSettings
            {
                Indent = true
            });
            xw.WriteDocType("svg", "-//W3C//DTD SVG 1.1//EN", "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd", null);
            ser.Serialize(xw, doc, ns);
            return sw.ToString();
        }

        /// <summary>
        /// Returns Document Object
        /// </summary>
        /// <returns></returns>
        public SvgRoot GetDocument()
        {
            var doc = new SvgRoot { ActualWidth = 700, ActualHeight = 700 };
            doc.Elements.AddRange(Tags);
            return doc;
        }
    }
}
