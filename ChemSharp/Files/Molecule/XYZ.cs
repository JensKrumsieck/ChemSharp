using ChemSharp.Molecule;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

namespace ChemSharp.Files.Molecule
{
    public class XYZ : TextFile, IAtomFile
    {
        internal readonly string Pattern = @"([A-Z][a-z]{0,1}){1}\s*(-*\d*[,.]?\d*)\s*(-*\d*[,.]?\d*)\s*(-*\d*[,.]?\d*)";

        public XYZ(string path) : base(path)
        {
            Atoms = ReadAtoms();
        }

        public IEnumerable<Atom> Atoms { get; set; }

        /// <summary>
        /// Read Atom data from CIF File
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Atom> ReadAtoms()
        {
            foreach (var line in Data)
            {
                var atomMatch = Regex.Match(line, Pattern);
                if (atomMatch.Groups.Count == 5)
                    yield return new Atom(atomMatch.Groups[1].Value)
                    {
                        Location = new Vector3(
                            float.Parse(atomMatch.Groups[2].Value, CultureInfo.InvariantCulture),
                            float.Parse(atomMatch.Groups[3].Value, CultureInfo.InvariantCulture),
                            float.Parse(atomMatch.Groups[4].Value, CultureInfo.InvariantCulture)
                        )
                    };
            }
        }
    }
}
