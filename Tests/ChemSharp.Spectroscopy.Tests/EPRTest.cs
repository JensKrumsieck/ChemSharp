using ChemSharp.Spectroscopy.Formats;

namespace ChemSharp.Spectroscopy.Tests;

public class EPRTest
{
	[Fact]
	public void Test1()
	{
		var spc = BrukerEPRFormat.Read(@"C:\Users\jenso\PowerFolders\Forschung\EPR\PyOMeIPCoCl\5_55k\Spectrum6.par");
		File.WriteAllText(@"C:\Users\jenso\Desktop\test.csv",
		                  string.Join("\n", spc.XYData.Select(d => $"{d.X};{d.Y}").ToArray()));
	}
}
