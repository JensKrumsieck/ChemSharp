using System;
using ChemSharp.Molecule;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using ChemSharp.Extensions;

namespace ChemSharp.Files.Molecule
{
    /// <summary>
    /// FileWrapper for TRIPOS Mol2
    /// </summary>
    public class MOL2 : TextFile, IAtomFile, IBondFile
    {
        public MOL2(string path) : base(path)
        {
            Atoms = ReadAtoms();
            Bonds = ReadBonds();
        }

        /// <summary>
        /// IAtomFile:Atoms
        /// </summary>
        public IEnumerable<Atom> Atoms { get; set; }

        /// <summary>
        /// IBondFile:Bonds
        /// </summary>
        public IEnumerable<Bond> Bonds { get; set; }

        /// <summary>
        /// Reads in Atoms from Mol2 file
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Atom> ReadAtoms()
        {
            var atoms = Tripos("ATOM");
            var lines = atoms.LineSplit();
            foreach (var line in lines)
            {
                if(string.IsNullOrEmpty(line) || line == "ATOM") continue;
                var columns = line.WhiteSpaceSplit();
                var identifier = columns[1];
                var x = columns[2].ToFloat();
                var y = columns[3].ToFloat();
                var z = columns[4].ToFloat();
                var symbol = Regex.Match(identifier, @"[A-Za-z]*").Value;
                yield return new Atom(symbol)
                {
                    Title = identifier,
                    Location = new Vector3(x, y, z)
                };
            }
        }

        /// <summary>
        /// Reads in Bonds
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Bond> ReadBonds()
        {
            var bonds = Tripos("BOND");
            var lines = bonds.LineSplit();
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line) || line == "BOND") continue;
                var columns = line.WhiteSpaceSplit();
                //subtract 1 as mol2 starts counting at 1
                var a1 = columns[1].ToInt() - 1;
                var a2 = columns[2].ToInt() - 1;
                yield return new Bond(Atoms.ElementAt(a1), Atoms.ElementAt(a2));
            }
        }


        private string[] _tripos;

        /// <summary>
        /// Gets specified loop
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string Tripos(string name)
        {
            _tripos ??= TriposBlocks();
            return Array.Find(_tripos, s => s.Contains(name));
        }

        /// <summary>
        /// Create loop data
        /// </summary>
        /// <returns></returns>
        private string[] TriposBlocks()
        {
            var text = string.Join('\n', Data);
            return text.Split(new[] { "@<TRIPOS>" }, StringSplitOptions.None);
        }

    }
}
