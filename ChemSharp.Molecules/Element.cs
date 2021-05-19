using ChemSharp.Molecules.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace ChemSharp.Molecules
{

    /// <summary>
    /// represents a chemical element
    /// data taken from https://github.com/JensKrumsieck/periodic-table api
    /// </summary>
    public class Element
    {
        #region API Properties
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Appearance { get; set; }
        public double AtomicWeight { get; set; }
        public int AtomicNumber { get; set; }
        public int Group { get; set; }
        public int Period { get; set; }
        public string Block { get; set; }
        public string Category { get; set; }
        public string ElectronConfiguration { get; set; }
        public double? Electronegativity { get; set; }
        public int? CovalentRadius { get; set; }
        public int? AtomicRadius { get; set; }
        public int? VdWRadius { get; set; }
        public string CAS { get; set; }
        #endregion API Properties

        /// <summary>
        /// Constructor for Json Serialization
        /// </summary>
        [JsonConstructor]
        public Element() { }

        /// <summary>
        /// Create Element by symbol
        /// </summary>
        /// <param name="symbol"></param>
        public Element(string symbol)
        {
            var elements = ElementDataProvider.ElementData.ToArray();
            var shadow = elements.FirstOrDefault(s => s.Symbol == symbol);
            if (shadow == null) return;
            var props = typeof(Element).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var p in props.Where(s => !Attribute.IsDefined(s, typeof(JsonIgnoreAttribute))))
            {
                p.SetValue(this, p.GetValue(shadow), null);
            }
        }

        /// <summary>
        /// Returns Element Color
        /// </summary>
        [JsonIgnore]
        public string Color => _color ??= ElementDataProvider.ColorData[Symbol];

        [JsonIgnore]
        private string _color;

        [JsonIgnore] public bool IsMetal => !IsMetalloid && !IsNonMetal;

        [JsonIgnore]
        public bool IsMetalloid => new[] { "B", "Si", "Ge", "As", "Sb", "Bi", "Se", "Te", "Po" }.Contains(Symbol);

        [JsonIgnore]
        public bool IsNonMetal =>
            new[] { "H", "C", "N", "O", "P", "S", "Se" }.Contains(Symbol) || Group == 18 ||
            Group == 17;

        static Element()
        {
            var transitionGroups = new List<int> { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            foreach (var element in ElementDataProvider.ElementData)
            {
                if (element.Group == 0) continue;
                var saturation = transitionGroups.Contains(element.Group) ? 0 : element.Group <= 3 ? element.Group : element.Group <= 14 ? 4 : 8 - (element.Group - 10);
                DesiredSaturation.Add(element.Symbol, saturation);
            }
        }
        public static Dictionary<string, int> DesiredSaturation = new();
    }

}
