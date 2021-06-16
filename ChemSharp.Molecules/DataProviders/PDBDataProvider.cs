using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using ChemSharp.Extensions;
using ChemSharp.Files;

namespace ChemSharp.Molecules.DataProviders
{
    public class PDBDataProvider : AbstractAtomDataProvider
    {
        /// <summary>
        /// import recipes
        /// </summary>
        static PDBDataProvider()
        {
            if (!FileHandler.RecipeDictionary.ContainsKey("pdb"))
                FileHandler.RecipeDictionary.Add("pdb", s => new PlainFile<string>(s));
        }

        public PDBDataProvider(string path) : base(path) => ReadData();

        public PDBDataProvider(Stream stream) : base(stream) => ReadData();

        public sealed override void ReadData()
        {
            Atoms = ReadAtoms();
        }

        public IEnumerable<Atom> ReadAtoms() =>
            //step through lines and assign Atoms/Bonds
            from line in Content.AsParallel() where line.StartsWith("ATOM") || line.StartsWith("HETATM") select ExtractAtom(line);

        private static Atom ExtractAtom(string line)
        {
            var cols = line.WhiteSpaceSplit();
            var title = cols[2];
            var loc = new Vector3(
                cols[6].ToSingle(),
                cols[7].ToSingle(),
                cols[8].ToSingle()
            );
            var type = cols[11].UcFirst();
            return new Atom(type)
            {
                Location = loc,
                Title = title
            };
        }
    }
}
