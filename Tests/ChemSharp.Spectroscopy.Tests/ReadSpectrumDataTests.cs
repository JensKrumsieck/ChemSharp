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
	}
}
