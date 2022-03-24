using System.Numerics;
#if NETSTANDARD2_1 || NET5_0_OR_GREATER
using MathF = System.MathF;
#endif

#if NETSTANDARD2_0
#endif

namespace ChemSharp.Mathematics;

public static class FractionalCoordinates
{
	/// <summary>
	///     Converts a vector of fractional coordinates to cartesian
	/// </summary>
	/// <param name="fractional"></param>
	/// <param name="conversionMatrix">a 3x3 conversion matrix</param>
	/// <returns></returns>
	public static Vector3 FractionalToCartesian(Vector3 fractional, Vector3[] conversionMatrix)
	{
		var vector = new float[3];
		for (var i = 0; i <= 2; i++)
			vector[i] = fractional.X * conversionMatrix[i].X + fractional.Y * conversionMatrix[i].Y +
			            fractional.Z * conversionMatrix[i].Z;

		return new Vector3(vector[0], vector[1], vector[2]);
	}

	/// <summary>
	///     Builds conversion matrix out of cell parameters
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <param name="c"></param>
	/// <param name="alpha"></param>
	/// <param name="beta"></param>
	/// <param name="gamma"></param>
	/// <returns></returns>
	public static Vector3[] ConversionMatrix(float a, float b, float c, float alpha, float beta, float gamma)
	{
		var line1 = new Vector3(a, b * MathF.Cos(gamma * MathF.PI / 180f), c * MathF.Cos(beta * MathF.PI / 180f));
		var line2 = new Vector3(
		                        0,
		                        b * MathF.Sin(gamma * MathF.PI / 180f),
		                        c * (MathF.Cos(alpha * MathF.PI / 180f) -
		                             MathF.Cos(beta * MathF.PI / 180f) * MathF.Cos(gamma * MathF.PI / 180f)) /
		                        MathF.Sin(gamma * MathF.PI / 180f));

		var line3 = new Vector3(
		                        0,
		                        0,
		                        c * MathF.Sqrt(1f - MathF.Pow(MathF.Cos(alpha * MathF.PI / 180f), 2f) -
		                                       MathF.Pow(MathF.Cos(beta * MathF.PI / 180f), 2f) -
		                                       MathF.Pow(MathF.Cos(gamma * MathF.PI / 180f), 2f) + 2f *
		                                       MathF.Cos(alpha * MathF.PI / 180f) *
		                                       MathF.Cos(beta * MathF.PI / 180f) *
		                                       MathF.Cos(gamma * MathF.PI / 180f)) /
		                        MathF.Sin(gamma * MathF.PI / 180f));
		return new[] {line1, line2, line3};
	}
}
