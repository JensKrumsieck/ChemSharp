using ChemSharp.Rendering.Primitives.SVG;
using System.Xml.Serialization;

namespace ChemSharp.Rendering.Primitives.Svg
{
    [XmlRoot("circle")]
    public class SvgCircle : SvgColoredItem, ISvgItem, ISvgCoordinateObject
    {
        [XmlAttribute("r")]
        public double Radius { get; set; }

        [XmlAttribute("cx")]
        public double X { get; set; }
        [XmlAttribute("cy")]
        public double Y { get; set; }
    }
}
