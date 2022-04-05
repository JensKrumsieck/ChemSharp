using System;
using System.Buffers;
using System.Globalization;

namespace ChemSharp.Memory;

public static class MemoryExtensions
{
	public static Span<(int start, int length)> WhiteSpaceSplit(this ReadOnlySpan<char> input)
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
			{
				curEnd = i;
				//if last character is non whitespace
				if (i == input.Length - 1)
					col[pos] = (curStart + 1, curEnd - curStart);
			}
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

		//find first element with zero length
		var firstZero = Array.IndexOf(col, Array.Find(col, a => a.Item2 == 0));
		var segment = col.AsSpan(0, firstZero);
		ArrayPool<(int, int)>.Shared.Return(col);
		return segment;
	}

	/// <summary>
	///     Matches the point (.) character and returns first part of ROS
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static ReadOnlySpan<char> PointSplit(this ReadOnlySpan<char> input)
	{
		var pointLoc = input.IndexOf('.');
		return pointLoc != -1 ? input[..pointLoc] : input;
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

	/// <summary>
	///     Converts ReadOnlySpan<char> to int, uses ToString() if netstandard2.0
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static int ToInt(this ReadOnlySpan<char> input)
	{
#if NETSTANDARD2_0
		return int.Parse(input.ToString());
#else
		return int.Parse(input);
#endif
	}
}
