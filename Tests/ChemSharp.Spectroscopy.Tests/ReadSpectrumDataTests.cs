using ChemSharp.Spectroscopy.Formats;
using FluentAssertions;

namespace ChemSharp.Spectroscopy.Tests;

public class ReadSpectrumDataTests
{
	[Fact]
	public void EPRSpectrum_IsRead_Plausible()
	{
		//plausible length
		var spc = BrukerEPRFormat.Read("files/Spectrum6.par");
		spc.XYData.Should().HaveCount(2048);
		//see spectrum6.dat
		spc.XYData[107].X.Should().BeApproximately(813.629699707031, 1e-5);
		spc.XYData[107].Y.Should().BeApproximately(-4.63134765625, 1e-5);
		//correlate with csv later
	}

	[Fact]
	public void UVVisSpectrum_IsRead_Plausible()
	{
		var spc = VarianUVVisFormat.Read("files/uvvis.DSW");
		spc.XYData.Should().HaveCount(901);
		//read from csv
		spc.XYData[32].X.Should().BeApproximately(1067.955688, 1e-5);
		spc.XYData[32].Y.Should().BeApproximately(0.02218390256, 1e-5);
		//correlate with csv later
	}

	[Fact]
	public void NMRSpectrum_Is_Read_Plausible()
	{
		var spc = BrukerNMRFormat.Read("files/nmr/fid");
		spc.XYData.Should().HaveCount(32768);
	}

	[Fact]
	public void CSV_Is_Read_Plausible()
	{
		var spc = CSVFormat.Read("files/test_single.csv", ',');
		spc.XYData.Should().HaveCount(901);
	}

	[Fact]
	public void MultiCSV_Is_Read_Plausible()
	{
		var specs = CSVFormat.ReadAll("files/test_multiple.csv", ',');
		specs.Should().HaveCount(3);
		foreach (var spectrum in specs) spectrum.XYData.Should().HaveCount(901);
	}

	[Theory, InlineData("files/Spectrum6.par", 2048), InlineData("files/Spectrum6.spc", 2048),
	 InlineData("files/uvvis.DSW", 901), InlineData("files/nmr/fid", 32768), InlineData("files/nmr/acqus", 32768),
	 InlineData("files/nmr/pdata/1/1r", 32768), InlineData("files/nmr/pdata/1/1i", 32768),
	 InlineData("files/nmr/pdata/1/procs", 32768), InlineData("files/test_single.csv", 901)]
	public void Spectrum_CanDecide_Plausible(string file, int cnt)
	{
		var spc = Spectrum.FromFile(file);
		spc.XYData.Should().HaveCount(cnt);
	}
}
