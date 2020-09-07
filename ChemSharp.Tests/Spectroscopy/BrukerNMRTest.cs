using System.Diagnostics.Tracing;
using ChemSharp.Files.Spectroscopy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ChemSharp.Tests.Spectroscopy
{
    [TestClass]
    public class BrukerNMRTest
    {
        private const string path = "files/nmr/";
        public  static readonly ACQUS ac = new ACQUS(path + "acqus");
        public static readonly PROCS procs = new PROCS(path + "pdata/1/procs");
        public static readonly FID fid = new FID(path + "fid");

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
    }
}
