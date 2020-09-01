using ChemSharp.Files.Spectroscopy;
using ChemSharp.Spectrum;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests
{
    [TestClass]
    public class VarianUVVisTest
    {
        private readonly string path = "files/uvvis.dsw";

        [TestMethod]
        public void TestDSW()
        {
            var dsw = new DSW(path);
            Assert.AreEqual(901, dsw.XYData.Length);
        }

        [TestMethod]
        public void TestUVVis()
        {
            var uvvis = SpectrumFactory.Create<UVVisSpectrum, DSW>(path);
            Assert.AreEqual(901, uvvis.Data.Length);
            //check if file information is saved correctly
            Assert.AreEqual(uvvis.Files[0].Path, path);
        }
    }
}