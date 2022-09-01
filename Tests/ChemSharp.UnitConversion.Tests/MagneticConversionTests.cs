using Xunit;

namespace ChemSharp.UnitConversion.Tests;

public class MagneticConversionTests
{
	[Fact]
	public void Gauss_To_Millitesla()
	{
		var converter = new MagneticUnitConverter("G", "mT");
		Assert.Equal(.1, converter.Convert(1));
		Assert.Equal(10, converter.ConvertInverted(1));
	}
}
