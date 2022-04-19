using System;
using System.Collections.Generic;
using System.Linq;
using ChemSharp.Memory;
using FluentAssertions;
using Xunit;

namespace ChemSharp.Base.Tests.Memory;

public class MemoryExtensionsTests
{
	[Theory, InlineData("The quick brown fox", 4, new[] {"The", "quick", "brown", "fox"}),
	 InlineData("The	quick	brown	tab", 4, new[] {"The", "quick", "brown", "tab"}),
	 InlineData("Two  spaces  each  segment", 4, new[] {"Two", "spaces", "each", "segment"}),
	 InlineData("Two  spaces  each", 3, new[] {"Two", "spaces", "each"}),
	 InlineData("Space at the End ", 4, new[] {"Space", "at", "the", "End"}),
	 InlineData(" Space at the Start", 4, new[] {"Space", "at", "the", "Start"}),
	 InlineData("This is a looooooooong booooooi", 5, new[] {"This", "is", "a", "looooooooong", "booooooi"}),
	 InlineData("", 0, new string[0]),
	 InlineData("      1 C           3.5425    1.7594   -0.3301 C.2     1  UNL1        0.0819", 9,
	            new[] {"1", "C", "3.5425", "1.7594", "-0.3301", "C.2", "1", "UNL1", "0.0819"})]
	public void WhiteSpaceSplit_ShouldBePlausible(string input, int count, string[] results) =>
		CheckEquality(input, count, results);

	private static void CheckEquality(string input, int count, string[] results)
	{
		var cols = input.AsSpan().WhiteSpaceSplit().ToArray();
		var colsString = StringEquiv(input).Where(s => s.Item2 > 0).ToArray();
		colsString.Length.Should().Be(cols.Length);
		cols.Length.Should().Be(count);
		for (var i = 0; i < cols.Length; i++)
		{
			cols[i].start.Should().Be(colsString[i].Item1);
			cols[i].length.Should().Be(colsString[i].Item2);
			input.AsSpan().Slice(cols[i].start, cols[i].length).ToString().Should()
			     .Be(input.Substring(colsString[i].Item1, colsString[i].Item2));
			input.AsSpan().Slice(cols[i].start, cols[i].length).ToString().Should()
			     .Be(results[i]);
		}
	}

	private static IEnumerable<(int, int)> StringEquiv(string input)
	{
		var pos = 0;
		var list = new List<(int, int)>();
		for (var i = 0; i <= input.Length; i++)
			if (i == input.Length || char.IsWhiteSpace(input[i]))
			{
				list.Add((pos, i - pos));
				pos = i + 1;
			}

		return list;
	}
}
