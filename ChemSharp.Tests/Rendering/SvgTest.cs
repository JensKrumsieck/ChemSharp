using ChemSharp.Molecules;
using ChemSharp.Molecules.DataProviders;
using ChemSharp.Rendering.Export;
using ChemSharp.Rendering.Extensions;

/* Nicht gemergte Änderung aus Projekt "ChemSharp.Tests (net472)"
Vor:
using ChemSharp.Rendering.Primitives.SVG;
Nach:
using ChemSharp.Rendering.Primitives.Svg;
using ChemSharp.Rendering.Primitives.SVG;
*/

/* Nicht gemergte Änderung aus Projekt "ChemSharp.Tests (netcoreapp2.1)"
Vor:
using ChemSharp.Rendering.Primitives.SVG;
Nach:
using ChemSharp.Rendering.Primitives.Svg;
using ChemSharp.Rendering.Primitives.SVG;
*/

/* Nicht gemergte Änderung aus Projekt "ChemSharp.Tests (netcoreapp3.1)"
Vor:
using ChemSharp.Rendering.Primitives.SVG;
Nach:
using ChemSharp.Rendering.Primitives.Svg;
using ChemSharp.Rendering.Primitives.SVG;
*/
using ChemSharp.Rendering.Primitives.Svg;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.Xml.Serialization;

namespace ChemSharp.Tests.Rendering
{
    [TestClass]
    public class SvgTest
    {
        [TestMethod]
        public void SerializationTest1()
        {
            const string mol2 = "files/tep.mol2";
            var molecule = new Molecule(new Mol2DataProvider(mol2)).ToSvg();
            var text = new SvgExporter { Width = 1000, Height = 1000 }.ExportToString(molecule);
            Assert.IsInstanceOfType(text, typeof(string));
        }

        [TestMethod]
        public void SerializationTest2()
        {
            const string cif = "files/cif.cif";
            var molecule = new Molecule(new CIFDataProvider(cif)).ToSvg();
            var text = new SvgExporter { Width = 1000, Height = 1000 }.ExportToString(molecule);
            Assert.IsInstanceOfType(text, typeof(string));
        }

        [TestMethod]
        public void DeserializationTest()
        {
            const string path = "files/porphin.svg";
            var ser = new XmlSerializer(typeof(SvgRoot));
            var xmlReader = new XmlTextReader(path);
            var data = ser.Deserialize(xmlReader);
            Assert.IsInstanceOfType(data, typeof(SvgRoot));
        }
    }
}
