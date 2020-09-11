using ChemSharp.Files;
using ChemSharp.Files.Spectroscopy;
using ChemSharp.Spectrum;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ChemSharp.Tests.Spectroscopy
{
    [TestClass]
    public class VarianUVVisTest
    {
        public const string path = "files/uvvis.dsw";
        public const string csvpath = "files/uvvis.csv";
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

        [TestMethod]
        public void TestCSV()
        {
            var csvGeneric = new CSV<float>(csvpath, ',');
            var csvFloat = new CSV(csvpath, ',');
            for (var i = 0; i < csvFloat.CsvTable.Count; i++)
            {
                for (var j = 0; j < csvFloat.CsvTable[i].Length; j++)
                    Assert.AreEqual(csvFloat.CsvTable[i][j], csvGeneric.CsvTable[i][j]);
            }
        }

        [TestMethod]
        public void TestCSVSpectrum()
        {
            var uvvis = SpectrumFactory.Create<UVVisSpectrum>(csvpath);
            Assert.AreEqual(901, uvvis.Data.Length);
        }
    }
}