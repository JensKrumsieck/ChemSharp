using ChemSharp.Spectroscopy;
using ChemSharp.Spectroscopy.DataProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests.Spectroscopy;

[TestClass]
public class UVVisSpectrumTest
{
	[TestMethod]
	public void TestUVVisCreation()
	{
		const string path = "files/uvvis.dsw";
		var uvvis = SpectrumFactory.Create(path);
		Assert.AreEqual(901, uvvis.XYData.Count);
		Assert.AreEqual(901, uvvis.Derivative.Count);
		Assert.AreEqual(901, uvvis.Integral.Count);
	}

	[TestMethod]
	public void TestUVVisFromCSV()
	{
		const string path = "files/uvvis.csv";
		var prov = new GenericCSVProvider(path);
		var uvvis = new Spectrum(prov);
		Assert.AreEqual(901, uvvis.XYData.Count);
		Assert.AreEqual(901, uvvis.Derivative.Count);
		Assert.AreEqual(901, uvvis.Integral.Count);
	}
}
