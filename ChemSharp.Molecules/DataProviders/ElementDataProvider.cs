using ChemSharp.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;

namespace ChemSharp.Molecules.DataProviders
{
    public static class ElementDataProvider
    {
        /// <summary>
        /// Where the colorData is located
        /// </summary>
        public static string ColorSource = "ChemSharp.Resources.colorData.txt";

        /// <summary>
        /// Where the element data is located
        /// </summary>
        public const string ApiSource = "ChemSharp.Resources.elements.json";

        static ElementDataProvider()
        {
            ReadColorData();
            ReadApiData();
        }
        
        /// <summary>
        /// contains color data
        /// </summary>
        public static ReadOnlyDictionary<string, string> ColorData { get; private set; }

        /// <summary>
        /// contains elemental data
        /// </summary>
        public static IEnumerable<Element> ElementData { get; private set; }

        /// <summary>
        /// Loads Color Data from ColorSource
        /// </summary>
        /// <returns></returns>
        private static void ReadColorData()
        {
            var data = ResourceUtil.ReadResourceString(ColorSource);
            var dic = data.DefaultSplit()
                .Select(line => line.Split(','))
                .ToDictionary(columns => columns[0], columns => columns[1]);
            ColorData = new ReadOnlyDictionary<string, string>(dic);
        }

        /// <summary>
        /// Loads Element Data from ApiSource
        /// </summary>
        /// <returns></returns>
        private static void ReadApiData()
        {
            //Read Data from https://github.com/JensKrumsieck/periodic-table 
            //fetched from http://en.wikipedia.org
            var raw = ResourceUtil.ReadResourceString(ApiSource);
            var data = JsonSerializer.Deserialize<IEnumerable<Element>>(raw);
            ElementData = data;
        }

    }
}
