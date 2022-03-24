using System;
using System.Collections.Generic;
using System.Linq;

namespace ChemSharp.Extensions;

public static class DoubleUtil
{
	/// <summary>
	///     Converts T IEnumerable to double Array
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static double[] ToDoubles<T>(this IEnumerable<T> input) => input.Select(s => Convert.ToDouble(s)).ToArray();

	/// <summary>
	///     Converts float IEnumerable to double Array
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static double[] ToDoubles(this IEnumerable<float> input) => input.ToDoubles<float>();

	/// <summary>
	///     Converts int IEnumerable to double Array
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static double[] ToDoubles(this IEnumerable<int> input) => input.ToDoubles<int>();
}
