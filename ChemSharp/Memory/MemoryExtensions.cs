using System.Buffers;
using System.Globalization;

namespace ChemSharp.Memory;

public static class MemoryExtensions
{
	public static Span<(int start, int length)> WhiteSpaceSplit(this ReadOnlySpan<char> input)
		=> Split(input, " \t".AsSpan());

	public static Span<(int start, int length)> Split(this ReadOnlySpan<char> input, ReadOnlySpan<char> delimiters)
	{
		if (input.Length == 0) return Span<(int start, int length)>.Empty;
		var array = ArrayPool<(int, int)>.Shared.Rent(input.Length);
		Array.Clear(array, 0, input.Length);
		var i = 0;
		foreach (var (start, length) in new SpanSplitEnumerator(input, delimiters))
		{
			array[i] = (start, length);
			i++;
		}

		var segment = Array.FindAll(array, tuple => tuple.Item2 > 0).AsSpan();
		ArrayPool<(int, int)>.Shared.Return(array);
		return segment;
	}

	/// <summary>
	///     Returns position of first numeric character
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static int FirstNumeric(this ReadOnlySpan<char> input)
	{
		for (var i = 0; i < input.Length; i++)
			if (char.IsNumber(input[i]))
				return i;
		return -1;
	}

	/// <summary>
	///     Removes Uncertainty appended to line in commas e.g. 8.1707(5) returns 8.1707
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static ReadOnlySpan<char> RemoveUncertainty(this ReadOnlySpan<char> input)
	{
		var pos = input.IndexOf('(');
		return pos != -1 ? input[..pos] : input;
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
