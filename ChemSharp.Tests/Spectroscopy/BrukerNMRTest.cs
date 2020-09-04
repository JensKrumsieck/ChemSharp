using ChemSharp.Files.Spectroscopy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ChemSharp.Extensions;

namespace ChemSharp.Tests.Spectroscopy
{
    [TestClass]
    public class BrukerNMRTest
    {
        private const string path = "files/nmr/";
        private static readonly ACQUS ac = new ACQUS(path + "acqus");
        private static readonly PROCS procs = new PROCS(path + "pdata/1/procs");
        public static readonly OneR oneR = new OneR(path + "pdata/1/1r");
        public static readonly OneR oneI = new OneR(path + "pdata/1/1i");
        public static readonly FID fid = new FID(path + "fid");

        [TestMethod]
        public void TestACQUS()
        {

            Assert.AreEqual(49152 / 2, ac.Count);
            Assert.AreEqual(49152 / 2, ac.XData.Length);
            Assert.AreEqual("1H", ac.Type);
            //check if processed correctly
            Assert.AreEqual(ac.PPMOffset, ac.XData.Last());
        }

        [TestMethod]
        public void Test1R()
        {
            Assert.AreEqual(procs.FTSize, oneR.YData.Length);
        }

        [TestMethod]
        public void TestFid()
        {
            Assert.AreEqual(ac.Count, fid.YData.Length);
            var x = fid.YData.FFT();
            var y = x.Select(s => s.Magnitude);
        }
    }
}
