namespace ChemSharp.Mathematics;

public static class MathUtil
{
	/// <summary>
	///     Checks if number is power of 2
	/// </summary>
	/// <param name="x"></param>
	/// <returns></returns>
	public static bool PowerOf2(int x) => (x & (x - 1)) == 0;

	/// <summary>
	///     returns the next power of 2 of xs
	/// </summary>
	/// <param name="x"></param>
	/// <returns></returns>
	public static int NextPowerOf2(int x) => (int)Math.Pow(2, Math.Floor(Math.Log(x, 2)) + 1);

	/// <summary>
	///     Calculates the L2 Norm (Euclidean) of an array
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static double Length(this IEnumerable<double> input) => Math.Sqrt(input.Sum(s => Math.Pow(s, 2)));

	/// <summary>
	///     Normalizes Vector by dividing by L2Norm
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static double[] Normalize(this IEnumerable<double> input)
	{
		var inputArr = input as double[] ?? input.ToArray();
		var len = inputArr.Length();
		return inputArr.Select(s => s / len).ToArray();
	}
}
