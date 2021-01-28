using ChemSharp.Rendering.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ChemSharp.Tests.Rendering
{
    //TODO: Assertions
    [TestClass]
    public class SvgTest
    {
        [TestMethod]
        public void SerializationTest()
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("xlink", "http://www.w3.org/1999/xlink");
            var doc = new SvgRoot { ActualWidth = 1000, ActualHeight = 300 };
            var svg = new SvgText { Text = "Hello World", ActualFill = Color.Blue, ActualFontSize = 100, Y = 200 };
            doc.Elements.Add(svg);
            var ser = new XmlSerializer(typeof(SvgRoot));
            using var sw = new UTF8Writer();
            using var xw = XmlWriter.Create(sw, new XmlWriterSettings
            {
                Indent = true
            });
            xw.WriteDocType("svg", "-//W3C//DTD SVG 1.1//EN", "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd", null);
            ser.Serialize(xw, doc, ns);
            var result = sw.ToString();
            //TODO: Change to other path
            File.WriteAllText("D:/test.svg", result);
        }

        [TestMethod]
        public void DeserializationTest()
        {
            const string path = "files/porphin.svg";
            var ser = new XmlSerializer(typeof(SvgRoot));
            var xmlReader = new XmlTextReader(path);
            var data = ser.Deserialize(xmlReader);
        }
    }

    internal class UTF8Writer : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}
