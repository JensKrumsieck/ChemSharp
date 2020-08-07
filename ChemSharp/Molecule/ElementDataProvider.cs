using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChemSharp.Molecule
{
    /// <summary>
    /// Singleton!
    /// </summary>
    public class ElementDataProvider
    {
        public static List<Element> Elements { get; private set; }
        static ElementDataProvider() { Elements = LoadElements().Result; }

        private static async Task<List<Element>> LoadElements()
        {
            //Read Data from https://github.com/JensKrumsieck/periodic-table 
            //fetched from http://en.wikipedia.org
            var assembly = typeof(Element).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("ChemSharp.Resources.elements.json");
            return await JsonSerializer.DeserializeAsync<List<Element>>(stream);
        }
    }
}
