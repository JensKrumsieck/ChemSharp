using ChemSharp.Molecules;
using ChemSharp.Molecules.DataProviders;
using ChemSharp.Molecules.Math;
using ChemSharp.Rendering.Extensions;
using ChemSharp.Rendering.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var doc = new SvgRoot { ActualWidth = 700, ActualHeight = 700 };

            const string cdxmlPath = "files/porphin.cdxml";
            var porphin = new Molecule(new CDXMLDataProvider(cdxmlPath)).ToSvg();
            doc.Elements.AddRange(porphin.Tags);

            const string cifPath = "files/cif.cif";
            var cor = new Molecule(new CIFDataProvider(cifPath));
            cor.SetMapping(cor.Atoms.CenterMapping());
            var corrole = cor.ToSvg();
            doc.Elements.AddRange(corrole.Tags);

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
