using ChemSharp.Files;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ChemSharp.Tests.IO
{
    [TestClass]
    public class FileTest
    {
        [TestMethod]
        public void TestCSV()
        {
            const string csv = "files/uvvis.csv";
            var csvFile = new PlainFile<string>(csv);
            csvFile.ReadFile();
            //901 data lines, headline + columns
            Assert.AreEqual(903, csvFile.Content.Length);
        }

        [TestMethod]
        public void TestSpcParFiles()
        {
            const string spc = "files/epr.spc";
            var spcFile = new PlainFile<int>(spc);
            spcFile.ReadFile();
            Assert.AreEqual(2048, spcFile.Content.Length);

            const string par = "files/epr.par";
            var parFile = new ParameterFile(par, ' ');
            parFile.ReadFile();

            //this file has 23 lines
            Assert.AreEqual(23, parFile.Content.Length);
        }

        [TestMethod]
        public void TestDswFile()
        {
            const string path = "files/uvvis.dsw";
            var file = new PlainFile<float>(path) { ByteOffset = 0x459 };
            const int dataLengthPtr = 0x6d;
            file.CutOffLength = BitConverter.ToInt32(file.Bytes, dataLengthPtr) * 8;
            file.ReadFile();

            //901 * 2 as X and Y are both in Content
            Assert.AreEqual(901 * 2, file.Content.Length);
        }

        [TestMethod]
        public void TestAcqusFidFiles()
        {
            const string fid = "files/nmr/fid";
            var fidFile = new PlainFile<float>(fid);
            fidFile.ReadFile();

            //lengths can be found in acqus: ##$TD=49152
            Assert.AreEqual(49152, fidFile.Content.Length);

            const string acqus = "files/nmr/acqus";
            var acqusFile = new ParameterFile(acqus, '=');
            acqusFile.ReadFile();

            //this file has 328 lines with string.Empty removed, tested against File.ReadAllLines
            Assert.AreEqual(328, acqusFile.Content.Length);
        }

        [TestMethod]
        public void TestXYZ()
        {
            const string xyz = "files/mescho.xyz";
            var xyzFile = new PlainFile<string>(xyz);
            xyzFile.ReadFile();

            //this file has 24 lines with string.Empty removed
            Assert.AreEqual(24, xyzFile.Content.Length);
        }
    }
}
