using System;
using ChemSharp.Extensions;
using ChemSharp.Spectroscopy;
using ChemSharp.Spectroscopy.Extension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests.Spectroscopy;

[TestClass]
public class EPRSpectrumTest
{
    private Spectrum epr;
    [TestInitialize]
    public void Init()
    {
        const string path = "files/epr.par";
        epr = SpectrumFactory.Create(path);
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
