using System;
using System.Linq;
using ChemSharp.Files.Spectroscopy;
using ChemSharp.Spectrum;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests.Spectroscopy
{
    [TestClass]
    public class VarianUVVisTest
    {
        public const string path = "files/uvvis.dsw";
        public readonly DSW dsw = new DSW(path);

        [TestMethod]
        public void TestDSW()
        {
            Assert.AreEqual(901, dsw.XYData.Length);
        }

        [TestMethod]
        public void TestUVVis()
        {
            var uvvis = SpectrumFactory.Create<UVVisSpectrum>(path);
            Assert.AreEqual(901, uvvis.Data.Length);
            //check if file information is saved correctly
            Assert.AreEqual(uvvis.Files[0].Path, path);
        }

        [TestMethod]
        public void TestSecondaryAxis()
        {
            var uvvis = SpectrumFactory.Create<UVVisSpectrum>(path);
            Assert.AreEqual(uvvis.Data.Length, uvvis.SecondaryXAxis.Length);
            var index = Array.IndexOf(uvvis.Data, uvvis.Data.FirstOrDefault(s => s.X > 499.9 && s.X < 500.1));
            Assert.AreEqual(20000, uvvis.SecondaryXAxis[index], 5);
        }
    }
}