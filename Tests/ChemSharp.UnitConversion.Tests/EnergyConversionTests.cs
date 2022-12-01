using FluentAssertions;
using Xunit;

namespace ChemSharp.UnitConversion.Tests;

public class EnergyConversionTests
{
	[Fact]
	public void ElectronVolt_To_Wavenumbers()
	{
		var converter = new EnergyUnitConverter("eV", "cm^-1");
		converter.Convert(1).Should().BeApproximately(8065.544, 5e-4);
		converter.ConvertInverted(1).Should().BeApproximately(1 / 8065.544, 5e-4);
	}

	[Fact]
	public void Nanometers_To_Wavenumbers()
	{
		var converter = new EnergyUnitConverter("nm", "cm^-1");
		converter.Convert(500).Should().Be(20000);
		converter.ConvertInverted(20000).Should().Be(500);
	}
}
