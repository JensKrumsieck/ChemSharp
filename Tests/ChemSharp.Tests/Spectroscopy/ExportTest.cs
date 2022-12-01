using System.IO;
using ChemSharp.Spectroscopy;
using ChemSharp.Spectroscopy.DataProviders;
using ChemSharp.Spectroscopy.Export;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests.Spectroscopy;

[TestClass]
public class ExportTest
{
	private const string dir = "tmp/";

	[TestInitialize]
	public void InitDir() => Directory.CreateDirectory(dir);

	[TestMethod]
	public void TestExportCSV()
	{
		const string par = "files/epr.par";
		var prov = new BrukerEPRProvider(par);
		var epr = new Spectrum(prov);
		var export = dir + "test.csv";
		CSVExporter.Export(epr, export,
		                   ';',
		                   SpectrumExportFlags.Experimental | SpectrumExportFlags.Derivative |
		                   SpectrumExportFlags.Integral);
		var provider = new MultiCSVProvider(export, ';');
		Assert.AreEqual(3, provider.MultiXYData.Count);
	}

	[TestCleanup]
	public void RemoveDir() => Directory.Delete(dir, true);
}
