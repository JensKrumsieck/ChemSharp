using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChemSharp.Molecule
{
    public class ElementDataProvider
    {
        public static HashSet<Element> Elements { get; }

        static ElementDataProvider() => Elements ??= LoadElements().Result;

        private static async Task<HashSet<Element>> LoadElements()
        {
            //Read Data from https://github.com/JensKrumsieck/periodic-table 
            //fetched from http://en.wikipedia.org
            var assembly = Assembly.GetAssembly(typeof(Element));
            var stream = assembly.GetManifestResourceStream("ChemSharp.Resources.elements.json");
            return await JsonSerializer.DeserializeAsync<HashSet<Element>>(stream);
        }
    }
}
