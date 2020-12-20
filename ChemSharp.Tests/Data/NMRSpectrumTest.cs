using ChemSharp.Spectroscopy;
using ChemSharp.Spectroscopy.DataProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests.Data
{
    [TestClass]
    public class NMRSpectrumTest
    {
        [TestMethod]
        public void TestNMRCreationRaw()
        {
            const string path = "files/nmr/fid";
            var prov = new BrukerNMRProvider(path,true);
            var nmr = new Spectrum(){DataProvider = prov};
            //32768 Data points, taken from procs file
            Assert.AreEqual(32768, nmr.XYData.Count);
        }

        [TestMethod]
        public void TestNMRCreationProcessed()
        {
            const string path = "files/nmr/pdata/1/1r";
            var prov = new BrukerNMRProvider(path);
            var nmr = new Spectrum() { DataProvider = prov };
            //32768 Data points, taken from procs file
            Assert.AreEqual(32768, nmr.XYData.Count);
        }
    }
}
