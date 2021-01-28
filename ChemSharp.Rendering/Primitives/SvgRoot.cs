using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace ChemSharp.Rendering.Primitives
{
    [XmlRoot("svg", Namespace = "http://www.w3.org/2000/svg")]
    public class SvgRoot
    {
        [XmlIgnore] public List<ISvgItem> Elements { get; set; } = new List<ISvgItem>();

        [XmlElement("text", typeof(SvgText))]
        [XmlElement("path", typeof(SvgPath))]
        public object[] ElementsProperty
        {
            get => Elements.ToArray();
            set => Elements = value.Cast<ISvgItem>().ToList();
        }

        [XmlAttribute("width")]
        public string Width
        {
            get => ActualWidth.ToString(CultureInfo.InvariantCulture)+"px"; 
            set => Convert.ToDouble(value.Replace("px", ""), CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public double ActualWidth { get; set; }

        [XmlAttribute("height")]
        public string Height
        {
            get => ActualHeight.ToString(CultureInfo.InvariantCulture) + "px";
            set => Convert.ToDouble(value.Replace("px", ""), CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public double ActualHeight { get; set; }

        [XmlAttribute("version")]
        public readonly string Version = "1.1";

        [XmlAttribute("viewBox")]
        public string ViewBox
        {
            get => $"0 0 {ActualWidth} {ActualHeight}";
            set { }
        }
    }
}
