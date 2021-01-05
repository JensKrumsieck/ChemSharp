using ChemSharp.Spectroscopy;
using ChemSharp.Spectroscopy.DataProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests.Spectroscopy
{
    [TestClass]
    public class UVVisSpectrumTest
    {
        [TestMethod]
        public void TestUVVisCreation()
        {
            const string path = "files/uvvis.dsw";
            var prov = new VarianUVVisProvider(path);
            var uvvis = new Spectrum() { DataProvider = prov };
            Assert.AreEqual(901, uvvis.XYData.Count);
            Assert.AreEqual(901, uvvis.Derivative.Count);
            Assert.AreEqual(901, uvvis.Integral.Count);
        }

        [TestMethod]
        public void TestUVVisFromCSV()
        {
            const string path = "files/uvvis.csv";
            var prov = new GenericCSVProvider(path, ',', 1);
            var uvvis = new Spectrum() { DataProvider = prov };
            Assert.AreEqual(901, uvvis.XYData.Count);
            Assert.AreEqual(901, uvvis.Derivative.Count);
            Assert.AreEqual(901, uvvis.Integral.Count);
        }
    }
}
