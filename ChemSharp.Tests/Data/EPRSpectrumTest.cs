using ChemSharp.Extensions;
using ChemSharp.Spectroscopy;
using ChemSharp.Spectroscopy.DataProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests.Data
{
    [TestClass]
    public class EPRSpectrumTest
    {
        [TestMethod]
        public void TestEPRCreation()
        {
            const string par = "files/epr.par";
            var prov = new BrukerEPRProvider(par);
            var epr = new Spectrum() { DataProvider = prov };
            Assert.AreEqual(2048, epr.XYData.Count);
            Assert.AreEqual(2048, epr.Derivative.Count);
            Assert.AreEqual(2048, epr.Integral.Count);
            Assert.AreEqual(2048, epr["RES"].ToInt());
        }
    }
}
