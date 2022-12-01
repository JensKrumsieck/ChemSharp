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
	/// <param name="matrix">a 3x3 conversion matrix</param>
	/// <returns></returns>
	public static Vector3 FractionalToCartesian(Vector3 fractional, Matrix4x4 matrix) =>
		Vector3.TransformNormal(fractional, matrix);

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
	public static Matrix4x4 ConversionMatrix(float a, float b, float c, float alpha, float beta, float gamma)
		=> Matrix4x4.Transpose(new Matrix4x4(
		                                     a, b * MathF.Cos(gamma * MathF.PI / 180f),
		                                     c * MathF.Cos(beta * MathF.PI / 180f), 0,
		                                     0, b * MathF.Sin(gamma * MathF.PI / 180f), c *
		                                     (MathF.Cos(alpha * MathF.PI / 180f) -
		                                      MathF.Cos(beta * MathF.PI / 180f) *
		                                      MathF.Cos(gamma * MathF.PI / 180f)) /
		                                     MathF.Sin(gamma * MathF.PI / 180f), 0,
		                                     0, 0,
		                                     c * MathF.Sqrt(1f - MathF.Pow(MathF.Cos(alpha * MathF.PI / 180f), 2f) -
		                                                    MathF.Pow(MathF.Cos(beta * MathF.PI / 180f), 2f) -
		                                                    MathF.Pow(MathF.Cos(gamma * MathF.PI / 180f), 2f) + 2f *
		                                                    MathF.Cos(alpha * MathF.PI / 180f) *
		                                                    MathF.Cos(beta * MathF.PI / 180f) *
		                                                    MathF.Cos(gamma * MathF.PI / 180f)) /
		                                     MathF.Sin(gamma * MathF.PI / 180f), 0,
		                                     0, 0, 0, 1));
}
