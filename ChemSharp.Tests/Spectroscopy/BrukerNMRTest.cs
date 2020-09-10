using ChemSharp.Files.Spectroscopy;
using ChemSharp.Spectrum;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ChemSharp.Tests.Spectroscopy
{
    [TestClass]
    public class BrukerNMRTest
    {
        private const string path = "files/nmr/";
        public static readonly ACQUS ac = new ACQUS(path + "acqus");
        public static readonly PROCS procs = new PROCS(path + "pdata/1/procs");
        public static readonly FID fid = new FID(path + "fid");
        public static readonly OneFile oneR = new OneFile(path + "pdata/1/1r");

        [TestMethod]
        public void TestACQUS()
        {
            Assert.AreEqual(49152 / 2, ac.Count);
            Assert.AreEqual("1H", ac.Type);
            //check if processed correctly
            Assert.AreEqual(ac.PPMOffset, ac.XData.Last());
        }

        [TestMethod]
        public void TestFid()
        {
            Assert.AreEqual(ac.Count, fid.FIDData.Length);
            Assert.AreEqual(ac.FFTSize, fid.YData.Length);
        }

        [TestMethod]
        public void TestOneR()
        {
            Assert.AreEqual(procs.FTSize, ac.FFTSize);
            Assert.AreEqual(ac.FFTSize, oneR.YData.Length);
            Assert.IsTrue(oneR.Is1R);
        }
        [TestMethod]
        public void TestCreationMethod()
        {
            //test self processing
            var nmr = SpectrumFactory.Create<NMRSpectrum>(path + "acqus", path + "fid");
            Assert.AreEqual(procs.FTSize, nmr.Data.Length);

            //test processed generation
            nmr = SpectrumFactory.Create<NMRSpectrum>(ac, oneR);
            Assert.AreEqual(procs.FTSize, nmr.Data.Length);
        }
    }
}
