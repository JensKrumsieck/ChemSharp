using System.Numerics;

namespace ChemSharp.Extensions;

public static class ComplexUtil
{
	/// <summary>
	///     Converts int collection to complex numbers
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static IEnumerable<Complex> ToComplexes(this IEnumerable<int> input)
	{
		var inputArr = input.ToArray();
		for (var i = 0; i < inputArr.Length / 2; i++) yield return new Complex(inputArr[2 * i], inputArr[2 * i + 1]);
	}

	/// <summary>
	///     Returns complex data to int by magnitude
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static int[] ToInts(this IEnumerable<Complex> input) => input.Select(s => (int)s.Magnitude).ToArray();
}
