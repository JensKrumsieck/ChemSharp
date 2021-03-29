using ChemSharp.Files;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace ChemSharp.Molecules.DataProviders
{
    public class XYZDataProvider : AbstractAtomDataProvider
    {
        /// <summary>
        /// import recipes
        /// </summary>
        static XYZDataProvider()
        {
            if (!FileHandler.RecipeDictionary.ContainsKey("xyz"))
                FileHandler.RecipeDictionary.Add("xyz", s => new PlainFile<string>(s));
        }

        public XYZDataProvider(string path) : base(path) => ReadData();
        public XYZDataProvider(Stream stream) : base(stream) => ReadData();

        public sealed override void ReadData()
        {
            Atoms = ReadAtoms(Content).ToList();
        }

        internal const string Pattern = @"([A-Z][a-z]{0,1}){1}\s*(-*\d*[,.]?\d*)\s*(-*\d*[,.]?\d*)\s*(-*\d*[,.]?\d*)";

        /// <summary>
        /// Read Atom data from CIF File
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Atom> ReadAtoms(IEnumerable<string> data) =>
            from line in data
            select Regex.Match(line, Pattern)
            into atomMatch
            where atomMatch.Groups.Count == 5
                  && !string.IsNullOrEmpty(atomMatch.Groups[4].Value)
                  && !string.IsNullOrEmpty(atomMatch.Groups[3].Value)
                  && !string.IsNullOrEmpty(atomMatch.Groups[2].Value)
            select new Atom(atomMatch.Groups[1].Value)
            {
                Location = new Vector3(
                    float.Parse(atomMatch.Groups[2].Value, CultureInfo.InvariantCulture),
                    float.Parse(atomMatch.Groups[3].Value, CultureInfo.InvariantCulture),
                    float.Parse(atomMatch.Groups[4].Value, CultureInfo.InvariantCulture)
                )
            };
    }
}
