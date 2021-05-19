using System.Xml.Serialization;

namespace ChemSharp.Rendering.Primitives
{
    /// <summary>
    /// TODO Transform handling!
    /// </summary>
    public abstract class SvgTransformableItem
    {
        [XmlAttribute("transform")]
        public string Transform { get; set; }

        public bool ShouldSerializeTransform() => !string.IsNullOrEmpty(Transform);
    }
}
