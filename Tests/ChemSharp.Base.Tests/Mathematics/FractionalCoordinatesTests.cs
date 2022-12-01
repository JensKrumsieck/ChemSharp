using System.Numerics;
using ChemSharp.Mathematics;
using FluentAssertions;
using Xunit;

namespace ChemSharp.Base.Tests.Mathematics;

public class FractionalCoordinatesTests
{
	[Theory, InlineData(0.4236f, 0.43592f, 0.38263f, 3.983062f, 9.077097f, 5.775253f),
	 InlineData(0.3542f, 0.51241f, 0.40267f, 3.468567f, 10.365696f, 6.077729f)]
	public void FractionalCoordinates_AreValid(float xfrac, float yfrac, float zfrac, float xcart, float ycart,
	                                           float zcart)
	{
		const float a = 8.1707f;
		const float b = 15.1621f;
		const float c = 16.4378f;
		const float al = 66.783f;
		const float be = 87.110f;
		const float ga = 88.224f;
		var matrix = FractionalCoordinates.ConversionMatrix(a, b, c, al, be, ga);
		var frac = new Vector3(xfrac, yfrac, zfrac);
		var cart = FractionalCoordinates.FractionalToCartesian(frac, matrix);
		var test = new Vector3(xcart, ycart, zcart);
		Vector3.DistanceSquared(cart, test).Should().BeApproximately(0f, 1e-10f);
	}
}
