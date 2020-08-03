using ChemSharp.Files;
using ChemSharp.Spectrum;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests
{
    [TestClass]
    public class VarianUVVisTest
    {
        private string path = "files/uvvis.dsw";

        [TestMethod]
        public void TestDSW()
        {
            var dsw = new DSW(path);
            Assert.AreEqual(901,dsw.XYData.Length);
        }

        [TestMethod]
        public void TestUVVis()
        {
            var uvvis = SpectrumFactory.Create<UVVisSpectrum, DSW>(path);
            Assert.AreEqual(901, uvvis.Data.Length);
        }
    }
}