using System;
using ChemSharp.Spectroscopy;
using ChemSharp.Spectroscopy.Extension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests.Spectroscopy;

[TestClass]
public class NMRSpectrumTest
{

    private Spectrum _nmr;
    [TestInitialize]
    public void SetUp()
    {
        const string path = "files/nmr/pdata/1/1r";
        _nmr = SpectrumFactory.Create(path);
    }

    [TestMethod]
    public void TestNMRCreationRaw()
    {
        const string path = "files/nmr/fid";
        var nmr = SpectrumFactory.Create(path);
        //32768 Data points, taken from procs file
        Assert.AreEqual(32768, nmr.XYData.Count);
    }

    [TestMethod]
    public void TestNMRCreationProcessed()
    {
        //32768 Data points, taken from procs file
        Assert.AreEqual(32768, _nmr.XYData.Count);
    }

    [TestMethod]
    public void TestProperties()
    {
        Assert.AreEqual("ppm", _nmr.Unit());
        Assert.AreEqual(DateTime.Parse("03.Feb.2020 10:16:52 +1").ToUniversalTime(), _nmr.CreationDate().ToUniversalTime());
    }
}
