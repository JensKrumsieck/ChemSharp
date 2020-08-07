using System.Collections.Generic;
using System.Reflection;
using System.Text.Encodings.Web;
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
        static ElementDataProvider()
        {
            LoadElements().Wait();
        }
        private static async Task LoadElements()
        {
            //Read Data from https://github.com/JensKrumsieck/periodic-table 
            //fetched from http://en.wikipedia.org
            var assembly = typeof(Element).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("ChemSharp.Resources.elements.json");
            var json = await JsonSerializer.DeserializeAsync<List<Element>>(stream);
            Elements = json;
        }
    }
}
