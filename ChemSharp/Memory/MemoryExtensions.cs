using System;
using System.Buffers;
using System.Globalization;

namespace ChemSharp.Memory;

public static class MemoryExtensions
{
	public static Span<(int, int)> WhiteSpaceSplit(this ReadOnlySpan<char> input)
	{
		var col = ArrayPool<(int, int)>.Shared.Rent(input.Length);
		var pos = 0;
		var curStart = 0;
		var curEnd = 0;
		//step through span
		for (var i = 0; i < input.Length; i++)
		{
			var c = input[i];
			if (!char.IsWhiteSpace(c))
				curEnd = i;
			else
			{
				if (curEnd - curStart != 0)
				{
					col[pos] = (curStart + 1, curEnd - curStart);
					pos++;
				}

				curStart = curEnd = i;
			}
		}

		var firstZero = Array.IndexOf(col, Array.Find(col, a => a.Item1 - a.Item2 == 0));
		var segment = col.AsSpan(0, firstZero);
		return segment;
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
