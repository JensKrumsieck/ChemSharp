using System;
using ChemSharp.Files;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Xml;

namespace ChemSharp.Molecules.DataProviders
{
    /// <summary>
    /// This class does not precisely render ChemDraw Files, but reads atom positions to get a 2D Molecule in 3D Space
    /// </summary>
    public class CDXMLDataProvider : IAtomDataProvider, IBondDataProvider
    {
        private Dictionary<int, Atom> idToAtoms = new Dictionary<int, Atom>();

        /// <summary>
        /// import recipes
        /// </summary>
        static CDXMLDataProvider()
        {
            if (!FileHandler.RecipeDictionary.ContainsKey("cdxml"))
                FileHandler.RecipeDictionary.Add("cdxml", s => new PlainFile<string>(s));
        }

        public CDXMLDataProvider(string path)
        {
            var file = (PlainFile<string>)FileHandler.Handle(path);
            var data = string.Join('\n', file.Content);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(data);
            var pages = xmlDoc.SelectNodes("CDXML/page");
            if (pages == null) return;
            foreach(XmlNode page in pages)
                foreach (XmlNode fragment in page) 
                    AnalyzeFragment(fragment);
        }

        /// <summary>
        /// Analyzes page fragments
        /// </summary>
        /// <param name="fragment"></param>
        public void AnalyzeFragment(XmlNode fragment)
        {
            var atoms = fragment.Cast<XmlNode>().Where(node => node.Name == "n");
            var bonds = fragment.Cast<XmlNode>().Where(node => node.Name == "b");
            Atoms = ReadAtoms(atoms);
            Bonds = ReadBonds(bonds);
        }

        /// <summary>
        /// Reads Atom Properties
        /// </summary>
        /// <param name="atoms"></param>
        public IEnumerable<Atom> ReadAtoms(IEnumerable<XmlNode> atoms)
        {
            foreach (var n in atoms)
            {
                if (!(n is XmlElement element)) yield break;
                var id = element.GetAttribute("id");
                var pos = element.GetAttribute("p").Split(" "); //position in pixels...
                var loc = new Vector3(Convert.ToSingle(pos[0], CultureInfo.InvariantCulture), Convert.ToSingle(pos[1], CultureInfo.InvariantCulture), 0f);
                var atomSymbol = "C";
                var elementNumber = element.GetAttribute("Element");
                if (!string.IsNullOrEmpty(elementNumber) && int.TryParse(elementNumber, out var numberResult))
                    atomSymbol = ElementDataProvider.ElementData.FirstOrDefault(s => s.AtomicNumber == numberResult)?.Symbol;
                var atom = new Atom(atomSymbol)
                {
                    Location = loc
                };
                idToAtoms.Add(int.Parse(id), atom);
                yield return atom;
            }
        }

        /// <summary>
        /// Reads Bond Properties
        /// </summary>
        /// <param name="bonds"></param>
        public IEnumerable<Bond> ReadBonds(IEnumerable<XmlNode> bonds)
        {
            foreach (var b in bonds)
            {
                if (!(b is XmlElement bond)) yield break;
                var begin = bond.GetAttribute("B");
                var end = bond.GetAttribute("E");
                int.TryParse(begin, out var firstId);
                int.TryParse(end, out var lastId);
                yield return new Bond(idToAtoms[firstId], idToAtoms[lastId]);
            }
        }

        public IEnumerable<Atom> Atoms { get; set; }
        public IEnumerable<Bond> Bonds { get; set; }
    }
}
