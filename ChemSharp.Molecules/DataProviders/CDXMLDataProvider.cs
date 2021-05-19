using ChemSharp.Files;
using ChemSharp.Molecules.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Xml;

namespace ChemSharp.Molecules.DataProviders
{
    /// <summary>
    /// This class does not precisely render ChemDraw Files, but reads atom positions to get a 2D Molecule in 3D Space
    /// </summary>
    public class CDXMLDataProvider : AbstractAtomDataProvider, IBondDataProvider
    {
        private readonly Dictionary<int, Atom> _idToAtoms = new();

        /// <summary>
        /// import recipes
        /// </summary>
        static CDXMLDataProvider()
        {
            if (!FileHandler.RecipeDictionary.ContainsKey("cdxml"))
                FileHandler.RecipeDictionary.Add("cdxml", s => new PlainFile<string>(s));
        }


        public CDXMLDataProvider(string path) : base(path) => ReadData();
        public CDXMLDataProvider(Stream stream) : base(stream) => ReadData();

        public sealed override void ReadData()
        {
            var data = string.Join("\n", Content);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            var pages = xmlDoc.SelectNodes("CDXML/page");
            if (pages == null) return;
            foreach (XmlNode page in pages)
                foreach (XmlNode fragment in page)
                    AnalyzeFragment(fragment);

            AddImplicitHydrogens();
        }

        /// <summary>
        /// Analyzes page fragments
        /// </summary>
        /// <param name="fragment"></param>
        public void AnalyzeFragment(XmlNode fragment)
        {
            var atoms = fragment.Cast<XmlNode>().Where(node => node.Name == "n");
            var bonds = fragment.Cast<XmlNode>().Where(node => node.Name == "b");
            //cast to list to prevent multi enumeration
            Atoms = ReadAtoms(atoms).ToList();
            Bonds = ReadBonds(bonds).ToList();
        }


        /// <summary>
        /// Reads Atom Properties
        /// </summary>
        /// <param name="atoms"></param>
        private IEnumerable<Atom> ReadAtoms(IEnumerable<XmlNode> atoms)
        {
            foreach (var n in atoms)
            {
                if (n is not XmlElement element) yield break;
                var id = element.GetAttribute("id");
                var pos = element.GetAttribute("p").Split(' '); //position in pixels...
                var loc = new Vector3(Convert.ToSingle(pos[0], CultureInfo.InvariantCulture), Convert.ToSingle(pos[1], CultureInfo.InvariantCulture), 0f);
                var atomSymbol = "C";
                var elementNumber = element.GetAttribute("Element");
                if (!string.IsNullOrEmpty(elementNumber) && int.TryParse(elementNumber, out var numberResult))
                    atomSymbol = ElementDataProvider.ElementData.FirstOrDefault(s => s.AtomicNumber == numberResult)?.Symbol;
                var atom = new Atom(atomSymbol)
                {
                    Location = loc / Constants.AngstromToPixels
                };
                _idToAtoms.Add(int.Parse(id), atom);
                yield return atom;
            }
        }

        /// <summary>
        /// Reads Bond Properties
        /// </summary>
        /// <param name="bonds"></param>
        private IEnumerable<Bond> ReadBonds(IEnumerable<XmlNode> bonds)
        {
            foreach (var b in bonds)
            {
                if (b is not XmlElement bond) yield break;
                var begin = bond.GetAttribute("B");
                var end = bond.GetAttribute("E");
                var firstId = int.Parse(begin);
                var lastId = int.Parse(end);
                var bondObj = new Bond(_idToAtoms[firstId], _idToAtoms[lastId]);

                var order = bond.GetAttribute("Order");
                if (!string.IsNullOrEmpty(order) && int.TryParse(order, out var bondOrder))
                    bondObj.Order = bondOrder;

                yield return bondObj;
            }
        }

        /// <summary>
        /// Adds implicit hydrogens
        /// </summary>
        private void AddImplicitHydrogens()
        {
            var atomList = Atoms.ToList();
            var bondList = Bonds.ToList();
            foreach (var atom in Atoms)
            {
                var order = Bonds.Saturation(atom);
                var maxOrder = Element.DesiredSaturation.ContainsKey(atom.Symbol) ? Element.DesiredSaturation[atom.Symbol] : 4;
                for (var i = order; i < maxOrder; i++)
                {
                    var h = new Atom("H") { Title = $"implicit hydrogen at {atom}" };
                    atomList.Add(h);
                    bondList.Add(new Bond(atom, h));
                }
            }
            Atoms = atomList;
            Bonds = bondList;
        }

        public IEnumerable<Bond> Bonds { get; set; }
    }
}
