﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using ChemSharp.Files;

namespace ChemSharp.Molecules.DataProviders
{
    public class XYZDataProvider : IAtomDataProvider
    {
        /// <summary>
        /// import recipes
        /// </summary>
        static XYZDataProvider()
        {
            if (!FileHandler.RecipeDictionary.ContainsKey("xyz"))
                FileHandler.RecipeDictionary.Add("xyz", s => new PlainFile<string>(s));
        }

        public XYZDataProvider(string path)
        {
            var file = (PlainFile<string>)FileHandler.Handle(path);
            Atoms = ReadAtoms(file.Content);
        }

        internal const string Pattern = @"([A-Z][a-z]{0,1}){1}\s*(-*\d*[,.]?\d*)\s*(-*\d*[,.]?\d*)\s*(-*\d*[,.]?\d*)";

        public IEnumerable<Atom> Atoms { get; set; }

        /// <summary>
        /// Read Atom data from CIF File
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Atom> ReadAtoms(string[] data)
        {
            foreach (var line in data)
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