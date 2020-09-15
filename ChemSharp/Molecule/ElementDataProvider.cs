using ChemSharp.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChemSharp.Molecule
{
    public class ElementDataProvider
    {
        public static HashSet<Element> Elements { get; }

        /// <summary>
        /// Enter either the default resource path "ChemSharp.Resources.colorData.txt" or any path on harddisk
        /// </summary>
        public static string ColorDataPath = "ChemSharp.Resources.colorData.txt";

        static ElementDataProvider()
        {
            Elements ??= LoadElements().Result;
            ColorData ??= LoadColorData();
        }

        private static async Task<HashSet<Element>> LoadElements()
        {
            //Read Data from https://github.com/JensKrumsieck/periodic-table 
            //fetched from http://en.wikipedia.org
            var stream = LoadResource("ChemSharp.Resources.elements.json");
            return await JsonSerializer.DeserializeAsync<HashSet<Element>>(stream);
        }

        /// <summary>
        /// contains color data
        /// </summary>
        public static ReadOnlyDictionary<string, string> ColorData { get; }

        /// <summary>
        /// Loads Color Data from CólorDataPath
        /// </summary>
        /// <returns></returns>
        private static ReadOnlyDictionary<string, string> LoadColorData()
        {
            var data = ReadResourceString(ColorDataPath);
            var dic = data.LineSplit()
                .Select(line => line.Split(','))
                .ToDictionary(columns => columns[0], columns => columns[1]);
            return new ReadOnlyDictionary<string, string>(dic);
        }

        private static Stream LoadResource(string resourceName) =>
            Assembly.GetAssembly(typeof(Element)).GetManifestResourceStream(resourceName);

        private static string ReadResourceString(string path){
            StreamReader sr;
            if (path.Contains("ChemSharp.Resources")) //resource loading
            {
                var stream = LoadResource(path);
                sr = new StreamReader(stream);
            }
            else sr = new StreamReader(path);
            var data = sr.ReadToEnd();
            sr.Close();
            return data;
        }
    }
}
