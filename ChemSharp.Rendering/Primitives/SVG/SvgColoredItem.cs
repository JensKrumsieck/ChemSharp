using System.Drawing;
using System.Xml.Serialization;

namespace ChemSharp.Rendering.Primitives.SVG
{
    public abstract class SvgColoredItem : SvgTransformableItem
    {
        [XmlAttribute("stroke")]
        public string Stroke
        {
            get => ActualStroke != null ? ColorTranslator.ToHtml(ActualStroke.Value) : "none";
            set => ActualStroke = value != "none" ? (Color?)ColorTranslator.FromHtml(value) : null;
        }

        [XmlIgnore]
        public Color? ActualStroke { get; set; }

        [XmlAttribute("fill")]
        public string Fill
        {
            get => ActualFill != null ? ColorTranslator.ToHtml(ActualFill.Value) : "none";
            set => ActualFill = value != "none" ? (Color?)ColorTranslator.FromHtml(value) : null;
        }

        [XmlIgnore]
        public Color? ActualFill { get; set; }

        [XmlAttribute("stroke-width")]
        public double StrokeThickness { get; set; } = 1;
    }
}
