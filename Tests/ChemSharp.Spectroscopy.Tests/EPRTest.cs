using ChemSharp.Spectroscopy.Formats;

namespace ChemSharp.Spectroscopy.Tests;

public class EPRTest
{
	[Fact]
	public void Test1() =>
		BrukerEPRFormat.Load(@"C:\Users\jenso\PowerFolders\Forschung\EPR\PyOMeIPCoCl\5_55k\Spectrum6.par");
}
