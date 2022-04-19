using System;

namespace ChemSharp.Memory;

public ref struct SpanSplitEnumerator
{
	private readonly ReadOnlySpan<char> _buffer;
	private readonly ReadOnlySpan<char> _separators;

	private int _start;
	private int _end;
	private int _next;

	public SpanSplitEnumerator GetEnumerator() => this;

	public (int start, int length) Current => (_start, _end - _start);

	internal SpanSplitEnumerator(ReadOnlySpan<char> span, ReadOnlySpan<char> separators)
	{
		_buffer = span;
		_separators = separators;
		_start = 0;
		_end = 0;
		_next = 0;
	}

	public bool MoveNext()
	{
		if (_next > _buffer.Length) return false;
		var slice = _buffer[_next..];
		_start = _next;
		var idx = slice.IndexOfAny(_separators);
		var len = idx != -1 ? idx : slice.Length;
		_end = _start + len;
		_next = _end + 1;
		return true;
	}
}
