using ChemSharp.Spectroscopy.DataProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests.Spectroscopy;

[TestClass]
public class ProviderTest
{
    [TestMethod]
    public void TestEPRProvider()
    {
        const string par = "files/epr.par";
        var provider = new BrukerEPRProvider(par);
        Assert.AreEqual(2048, provider.XYData.Length);
    }

    [TestMethod]
    public void TestUVVisProvider()
    {
        const string dsw = "files/uvvis.dsw";
        var provider = new VarianUVVisProvider(dsw);
        Assert.AreEqual(901, provider.XYData.Length);
    }

    [TestMethod]
    public void TestNMRProvider()
    {
        const string fid = "files/nmr/pdata/1/1i";
        var provider = new BrukerNMRProvider(fid);
        Assert.AreEqual(32768, provider.XYData.Length);
    }

    [TestMethod]
    public void TestNMRProviderRaw()
    {
        const string fid = "files/nmr/fid";
        var provider = new BrukerNMRProvider(fid, true);
        Assert.AreEqual(32768, provider.XYData.Length);
    }

    [TestMethod]
    public void TestCSVProvider()
    {
        const string file = "files/uvvis.csv";
        var provider = new GenericCSVProvider(file);
        Assert.AreEqual(901, provider.XYData.Length);
    }

    [TestMethod]
    public void TestCSVMultiProvider()
    {
        const string file = "files/multicsv.csv";
        var provider = new MultiCSVProvider(file);
        Assert.AreEqual(23, provider.MultiXYData.Count);
    }
}
