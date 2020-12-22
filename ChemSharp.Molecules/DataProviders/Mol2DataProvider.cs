using ChemSharp.Extensions;
using ChemSharp.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace ChemSharp.Molecules.DataProviders
{
    public class Mol2DataProvider : IAtomDataProvider, IBondDataProvider
    {
        /// <summary>
        /// import recipes
        /// </summary>
        static Mol2DataProvider()
        {
            if (!FileHandler.RecipeDictionary.ContainsKey("mol2"))
                FileHandler.RecipeDictionary.Add("mol2", s => new PlainFile<string>(s));
        }

        public Mol2DataProvider(string path)
        {
            var file = (PlainFile<string>)FileHandler.Handle(path);
            var blocks = TriposBlocks(file.Content).ToArray();
            var atomBlock = Array.Find(blocks, s => s.Contains("ATOM")).DefaultSplit();
            var bondBlock = Array.Find(blocks, s => s.Contains("BOND")).DefaultSplit();
            Atoms = Read<Atom>(atomBlock);
            Bonds = Read<Bond>(bondBlock);
        }

        public IEnumerable<Atom> Atoms { get; set; }
        public IEnumerable<Bond> Bonds { get; set; }

        /// <summary>
        /// Reads Data
        /// </summary>
        /// <typeparam name="T">Atom or Bond!</typeparam>
        /// <param name="block"></param>
        /// <returns></returns>
        private IEnumerable<T> Read<T>(IEnumerable<string> block) where T : class
        {
            if ((typeof(T)) != typeof(Atom) && (typeof(T)) != typeof(Bond)) yield break;
            foreach (var line in block)
            {
                if (string.IsNullOrEmpty(line) || (line == "ATOM" || line == "BOND")) continue;
                var cols = line.WhiteSpaceSplit();
                if (typeof(T) == typeof(Atom)) yield return ReadAtom(cols) as T;
                else yield return ReadBond(cols) as T;
            }
        }

        /// <summary>
        /// Reads Atom
        /// </summary>
        /// <param name="cols"></param>
        /// <returns></returns>
        private Atom ReadAtom(IReadOnlyList<string> cols)
        {
            var id = cols[1];
            var loc = new Vector3(
                cols[2].ToSingle(),
                cols[3].ToSingle(),
                cols[4].ToSingle()
            );
            var sym = Regex.Match(id, @"[A-Za-z]*").Value;
            return new Atom(sym)
            {
                Title = id,
                Location = loc
            };
        }

        /// <summary>
        /// Reads Bond
        /// </summary>
        /// <param name="cols"></param>
        /// <returns></returns>
        private Bond ReadBond(IReadOnlyList<string> cols)
        {
            //subtract 1 as mol2 starts counting at 1
            var a1 = cols[1].ToInt() - 1;
            var a2 = cols[2].ToInt() - 1;
            return new Bond(Atoms.ElementAt(a1), Atoms.ElementAt(a2));
        }


        /// <summary>
        /// reorders text into tripos blocks
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static IEnumerable<string> TriposBlocks(string[] data)
        {
            var text = string.Join('\n', data);
            return text.Split(new[] { "@<TRIPOS>" }, StringSplitOptions.None);
        }
    }
}
