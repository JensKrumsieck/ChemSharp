using FluentAssertions;
using Xunit;

namespace ChemSharp.UnitConversion.Tests;

public class SpecialConversionTests
{
	[Fact]
	public void Test_BFromG() => SpecialConverters.BFromG(2, 9.5f, "G").Should().BeApproximately(3394, 1);

	[Fact]
	public void Test_GFromB() => SpecialConverters.GFromB(500, 9.5f).Should().BeApproximately(14, 1);
}
