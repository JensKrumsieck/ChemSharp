using ChemSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace ChemSharp.Rendering.Primitives.SVG
{
    [XmlRoot("svg", Namespace = "http://www.w3.org/2000/svg")]
    public class SvgRoot
    {
        [XmlIgnore] public List<ISvgItem> Elements { get; set; } = new();

        [XmlElement("text", typeof(SvgText))]
        [XmlElement("path", typeof(SvgPath))]
        [XmlElement("circle", typeof(SvgCircle))]
        public object[] ElementsProperty
        {
            get => Elements.ToArray();
            set => Elements = value.Cast<ISvgItem>().ToList();
        }

        [XmlAttribute("width")]
        public string Width
        {
            get => ActualWidth.ToInvariantString() + "px";
            set => Convert.ToDouble(value.Replace("px", ""), CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public double ActualWidth { get; set; }

        [XmlAttribute("height")]
        public string Height
        {
            get => ActualHeight.ToInvariantString() + "px";
            set => Convert.ToDouble(value.Replace("px", ""), CultureInfo.InvariantCulture);
        }

        [XmlIgnore]
        public double ActualHeight { get; set; }

        [XmlAttribute("version")]
        public readonly string Version = "1.1";

        [XmlAttribute("viewBox")]
        public string ViewBox
        {
            get => $"{-ActualWidth / 2} {-ActualHeight / 2} {ActualWidth} {ActualHeight}";
            set { }
        }
    }
}
