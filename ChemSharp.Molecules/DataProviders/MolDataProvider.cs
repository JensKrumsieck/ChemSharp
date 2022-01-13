using ChemSharp.Extensions;
using ChemSharp.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace ChemSharp.Molecules.DataProviders
{
    public class MolDataProvider : AbstractAtomDataProvider, IBondDataProvider
    {
        /// <summary>
        /// import recipes
        /// </summary>
        static MolDataProvider()
        {
            if (!FileHandler.RecipeDictionary.ContainsKey("mol"))
                FileHandler.RecipeDictionary.Add("mol", s => new PlainFile<string>(s));
        }

        public MolDataProvider(string path) : base(path) => ReadData();
        public MolDataProvider(Stream stream) : base(stream) => ReadData();
        public IEnumerable<Bond> Bonds { get; set; }
        public sealed override void ReadData()
        {
            var index = Array.FindIndex(Content, e => e.Contains("V2000")); //Support Mol V2000
            if (index == -1) throw new FileLoadException("File not in correct format!");
            var body = Content.Skip(index + 1);
            var firstRow = Content[index].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (!int.TryParse(firstRow[0], out var noAtoms)) throw new FileLoadException("File not in correct format!");
            if (!int.TryParse(firstRow[1], out var noBonds)) throw new FileLoadException("File not in correct format!");
            var atomBlock = body.Take(noAtoms);
            var bondBlock = body.Skip(noAtoms).Take(noBonds);
            Atoms = ReadAtoms(atomBlock).ToList();
            Bonds = ReadBonds(bondBlock).ToList();
        }

        private IEnumerable<Atom> ReadAtoms(IEnumerable<string> atomBlock)
        {
            foreach (var col in atomBlock)
            {
                var split = col.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var loc = new Vector3(
                    split[0].ToSingle(),
                    split[1].ToSingle(),
                    split[2].ToSingle());
                yield return new Atom(split[3])
                {
                    Location = loc
                };
            }
        }

        /// <summary>
        /// Reads Bonds from Block
        /// Please not, that implicit hydrogens in 2D mol files are not generated!
        /// </summary>
        /// <param name="bondBlock"></param>
        /// <returns></returns>
        private IEnumerable<Bond> ReadBonds(IEnumerable<string> bondBlock)
        {
            foreach (var col in bondBlock)
            {
                var split = col.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var atom1 = split[0].ToInt() - 1;
                var atom2 = split[1].ToInt() - 1;
                var order = split[2].ToInt();
                var bond = new Bond(Atoms.ElementAt(atom1), Atoms.ElementAt(atom2));
                if (order != 4) 
                    bond.Order = order;
                else bond.IsAromatic = true;
                yield return bond;
            }
        }

    }
}
