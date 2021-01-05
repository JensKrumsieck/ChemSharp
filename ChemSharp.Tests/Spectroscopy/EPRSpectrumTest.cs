using ChemSharp.Extensions;
using ChemSharp.Spectroscopy;
using ChemSharp.Spectroscopy.DataProviders;
using ChemSharp.Spectroscopy.Extension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ChemSharp.Tests.Spectroscopy
{
    [TestClass]
    public class EPRSpectrumTest
    {
        private Spectrum epr;
        [TestInitialize]
        public void Init()
        {
            const string par = "files/epr.par";
            var prov = new BrukerEPRProvider(par);
            epr = new Spectrum() { DataProvider = prov };
        }

        [TestMethod]
        public void TestEPRCreation()
        {

            Assert.AreEqual(2048, epr.XYData.Count);
            Assert.AreEqual(2048, epr.Derivative.Count);
            Assert.AreEqual(2048, epr.Integral.Count);
            Assert.AreEqual(2048, epr["RES"].ToInt());
        }

        [TestMethod]
        public void TestProperties()
        {
            Assert.AreEqual("G", epr.Unit());
            Assert.AreEqual(DateTime.Parse("04.Aug.2017 15:19").ToUniversalTime(), epr.CreationDate().ToUniversalTime());
        }
    }
}
