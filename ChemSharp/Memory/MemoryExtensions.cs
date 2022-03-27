using System;
using System.Collections.Generic;
using System.Globalization;

namespace ChemSharp.Memory;

public static class MemoryExtensions
{
	public static SpanSplitEnumerator Split(this ReadOnlySpan<char> input, char separator) => new(input, separator);

	public static SpanSplitEnumerator Split(this ReadOnlySpan<char> input, ReadOnlySpan<char> separators) =>
		new(input, separators);

	public static List<Range> ToList(this SpanSplitEnumerator enumerator, bool ignoreEmpty = false)
	{
		var list = new List<Range>();
		foreach (var element in enumerator)
		{
			if (element.End.Value == element.Start.Value && ignoreEmpty) continue;
			list.Add(element);
		}

		return list;
	}

	/// <summary>
	///     Converts ReadOnlySpan<char> to float, uses ToString() if netstandard2.0
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static float ToSingle(this ReadOnlySpan<char> input)
	{
#if NETSTANDARD2_0
		return float.Parse(input.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture);
#else
		return float.Parse(input, NumberStyles.Float, CultureInfo.InvariantCulture);
#endif
	}
}
