using ChemSharp.Extensions;
using ChemSharp.Rendering.Primitives.SVG;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace ChemSharp.Rendering.Primitives.Svg
{
    [Obsolete("Package will be removed soon")]
    [XmlRoot("text")]
    public class SvgText : SvgColoredItem, ISvgItem, ISvgCoordinateObject
    {
        [XmlIgnore]
        public readonly Dictionary<string, int> FontWeights = new()
        {
            { "lighter", 100 },
            { "normal", 300 },
            { "bold", 500 },
            { "bolder", 700 },
        };

        [XmlAttribute("font-family")]
        public string FontFamily { get; set; } = "Arial";

        [XmlAttribute("font-weight")]
        public string FontWeight
        {
            get => ActualFontWeight.ToString();
            set => ActualFontWeight = FontWeights.ContainsKey(value) ? FontWeights[value] : int.Parse(value);
        }

        [XmlIgnore]
        public int ActualFontWeight { get; set; }

        [XmlAttribute("font-size")]
        public string FontSize
        {
            get => ActualFontSize.ToInvariantString() + "px";
            set => Convert.ToDouble(value.Replace("px", ""), CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public double ActualFontSize { get; set; }

        [XmlAttribute("font-style")]
        public string FontStyle { get; set; } = "normal";

        [XmlText]
        public string Text { get; set; }

        [XmlAttribute("x")]
        public double X { get; set; }

        [XmlAttribute("y")]
        public double Y { get; set; }

        [XmlAttribute("text-anchor")]
        public string HorizontalAlign { get; set; }

        [XmlAttribute("alignment-baseline")]
        public string VerticalAlign { get; set; }
    }
}
