using ChemSharp.Files.Spectroscopy;
using ChemSharp.Spectrum;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ChemSharp.Tests.Spectroscopy
{
    [TestClass]
    public class BrukerEPRTest
    {
        public const string path = "files/epr";

        [TestMethod]
        public void TestSPC()
        {
            var spc = new SPC($"{path}.spc");
            //check length of loaded data
            Assert.AreEqual(2048, spc.YData.Length);
        }

        [TestMethod]
        public void TestPAR()
        {
            var par = new PAR($"{path}.par");
            //check length of loaded dat
            Assert.AreEqual(2048, par.XData.Length);
        }

        [TestMethod]
        public void TestEPRCreation()
        {
            var epr = SpectrumFactory.Create<EPRSpectrum>($"{path}.par", $"{path}.spc");
            //check length of created vector property
            Assert.AreEqual(2048, epr.Data.Count);
        }

        [TestMethod]
        public void TestGAxis()
        {
            var epr = SpectrumFactory.Create<EPRSpectrum>($"{path}.par", $"{path}.spc");
            var g = epr.GAxis.ToArray();
            Assert.AreEqual(2048, g.Length);
            CollectionAssert.AreEqual(g, epr.SecondaryXAxis);
        }
    }
}
