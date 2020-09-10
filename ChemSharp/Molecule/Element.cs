using System;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace ChemSharp.Molecule
{
    /// <summary>
    /// Represents an Element in the periodic table
    /// if your want to use an Element with 3D Properties use Atom class instead!
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
        private Element() { }

        /// <summary>
        /// Create Element by symbol
        /// </summary>
        /// <param name="symbol"></param>
        public Element(string symbol)
        {
            //get Data via API
            var shadow = ElementDataProvider.Elements.FirstOrDefault(s => s.Symbol == symbol);
            var props = typeof(Element).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var p in props.Where(s => !Attribute.IsDefined(s, typeof(JsonIgnoreAttribute))))
            {
                p.SetValue(this as Element, p.GetValue(shadow), null);
            }
        }


        /// <summary>
        /// retuns true if atom is considered as metal
        /// </summary>
        [JsonIgnore]
        public bool IsMetal => Category.Contains("metal", StringComparison.InvariantCultureIgnoreCase)
                               && !IsMetalloid
                               && !IsNonMetal;
        /// <summary>
        /// returns true if atom is considered as metalloid
        /// </summary>

        [JsonIgnore]
        public bool IsMetalloid => Category == "Metalloid";

        /// <summary>
        /// Returns true if atom is considered as gas
        /// </summary>
        [JsonIgnore]
        public bool IsGas => Category.Contains("gas", StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        /// Returns true if atom is considered nonmetal
        /// </summary>
        [JsonIgnore]
        public bool IsNonMetal => Category.Contains("nonmetal", StringComparison.InvariantCultureIgnoreCase);
    }
}
