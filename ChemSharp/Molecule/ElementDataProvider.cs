using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChemSharp.Molecule
{
    public sealed class ElementDataProvider
    {
        private static readonly ElementDataProvider _instance = new ElementDataProvider();
        public List<Element> Elements;

        public static ElementDataProvider Instance => _instance;

        private ElementDataProvider()
        {
            Elements = Init().Result;
        }

        private static async Task<List<Element>> Init()
        {
            //Read Data from https://github.com/JensKrumsieck/periodic-table 
            //fetched from http://en.wikipedia.org
            var assembly = typeof(ChemSharp.Molecule.Element).GetTypeInfo().Assembly;
            await using var stream = assembly.GetManifestResourceStream("ChemSharp.Resources.elements.json");
            return await JsonSerializer.DeserializeAsync<List<Element>>(stream);
        }
    }
}
