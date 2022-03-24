using System;
using System.Collections;
using System.Collections.Generic;

namespace ChemSharp;

public readonly struct MatrixInt : IEquatable<MatrixInt>
{
	private readonly int[,] _values;

	public int Length => _values.GetLength(0);

	public MatrixInt(int length) => _values = new int[length, length];
	public MatrixInt(int[,] sourceArray) => _values = sourceArray;

	public int this[int i, int j]
	{
		get => _values[i, j];
		set => _values[i, j] = value;
	}

	public static implicit operator int[,](MatrixInt src) => src._values;
	public static implicit operator MatrixInt(int[,] src) => new(src);

	public bool Equals(MatrixInt other)
	{
		for (var i = 0; i < Length; i++)
		for (var j = 0; j < Length; j++)
			if (other[i, j] != this[i, j]) return false;

		return true;

	}


	public override bool Equals(object obj) => obj is MatrixInt other && Equals(other);

	public override int GetHashCode() => (_values != null ? _values.GetHashCode() : 0);
}
