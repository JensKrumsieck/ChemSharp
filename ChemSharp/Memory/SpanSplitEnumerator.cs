using System;

namespace ChemSharp.Memory;

public ref struct SpanSplitEnumerator
{
	private readonly ReadOnlySpan<char> _input;
	private readonly ReadOnlySpan<char> _separators;
	private readonly char _separator;
	private readonly bool _single;
	private readonly int _separatorLength;

	private int _currentStart;
	private int _currentEnd;
	private int _nextStart;

	public SpanSplitEnumerator(ReadOnlySpan<char> input, ReadOnlySpan<char> separators)
	{
		_input = input;
		_separators = separators;
		_separatorLength = _separators.Length != 0 ? _separators.Length : 1;
		_separator = default;
		_single = false;
		_currentStart = 0;
		_currentEnd = 0;
		_nextStart = 0;
	}

	public SpanSplitEnumerator(ReadOnlySpan<char> input, char separator)
	{
		_input = input;
		_separator = separator;
		_separators = default;
		_separatorLength = 1;
		_single = true;
		_currentStart = 0;
		_currentEnd = 0;
		_nextStart = 0;
	}

	public Range Current => new(_currentStart, _currentEnd);

	public bool MoveNext()
	{
		if (_nextStart > _input.Length) return false;
		var slice = _input[_nextStart..];
		_currentStart = _nextStart;
		var sepI = _single ? slice.IndexOf(_separator) : slice.IndexOf(_separators);
		var len = sepI != -1 ? sepI : slice.Length;
		_currentEnd = _currentStart + len;
		_nextStart = _currentEnd + _separatorLength;
		return true;
	}

	public SpanSplitEnumerator GetEnumerator() => this;
}
