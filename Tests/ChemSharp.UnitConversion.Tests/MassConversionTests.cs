using FluentAssertions;
using Xunit;

namespace ChemSharp.UnitConversion.Tests;

public class MassConversionTests
{
	[Fact]
	public void SolarMass_To_EarthMass()
	{
		var converter = new MassUnitConverter("Solar Mass", "Earth Mass");
		converter.Convert(1).Should().BeApproximately(332946.0487, 1.5);
	}

	[Fact]
	public void Lbs_to_g()
	{
		var converter = new MassUnitConverter("lbs", "g");
		converter.Convert(1).Should().Be(453.59237);
	}
}
