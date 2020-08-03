using ChemSharp.Files;
using ChemSharp.Spectrum;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests
{
    [TestClass]
    public class BrukerEPRTest
    {
        private string path = "files/epr";

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
            var epr = SpectrumFactory.Create<EPRSpectrum, PAR, SPC>($"{path}.par", $"{path}.spc");
            //check length of created vector property
            Assert.AreEqual(2048, epr.Data.Length);
        }
    }
}
