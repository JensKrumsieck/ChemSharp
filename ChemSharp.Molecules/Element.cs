using System;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using ChemSharp.Molecules.DataProviders;

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
            var props = typeof(Element).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var p in props.Where(s => !Attribute.IsDefined(s, typeof(JsonIgnoreAttribute))))
            {
                p.SetValue(this as Element, p.GetValue(shadow), null);
            }
        }

        /// <summary>
        /// Returns Element Color
        /// </summary>
        [JsonIgnore]
        public string Color => _color ??= ElementDataProvider.ColorData[Symbol];

        [JsonIgnore]
        private string _color { get; set; }
    }

}
