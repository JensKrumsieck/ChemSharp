﻿using ChemSharp.Extensions;
using ChemSharp.Files;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;


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
            var tag = cols[3];
            var loc = new Vector3(
                cols[6].ToSingle(),
                cols[7].ToSingle(),
                cols[8].ToSingle()
            );
            var type = cols[11].UcFirst();
            return new Atom(type)
            {
                Location = loc,
                Title = title,
                Tag = tag
            };
        }

        public static readonly Dictionary<string, string> AminoAcids = new Dictionary<string, string>
        {
            ["ALA"] = "Alanine",
            ["ARG"] = "Arginine",
            ["ASN"] = "Asparagine",
            ["ASP"] = "Aspartate",
            ["ASX"] = "Aspartate or Asparagine",
            ["CYS"] = "Cysteine",
            ["GLU"] = "Glutamate",
            ["GLN"] = "Glutamine",
            ["GLY"] = "Glycine",
            ["GLX"] = "Glutamate or Glutamine",
            ["HIS"] = "Histidine",
            ["ILE"] = "Isoleucine",
            ["LEU"] = "Leucine",
            ["LYS"] = "Lysine",
            ["MET"] = "Methionine",
            ["PHE"] = "Phenylalanine",
            ["PRO"] = "Proline",
            ["SER"] = "Serine",
            ["THR"] = "Threonine",
            ["TRP"] = "Tryptophan",
            ["TYR"] = "Tyrosine",
            ["VAL"] = "Valine",
            ["XLE"] = "Leucine or Isoleucine"
        };
    }
}
