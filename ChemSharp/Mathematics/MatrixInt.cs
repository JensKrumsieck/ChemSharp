using System;
using ChemSharp.Extensions;

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
				if (other[i, j] != this[i, j])
					return false;

		return true;
	}


	public override bool Equals(object obj) => obj is MatrixInt other && Equals(other);

	public override int GetHashCode() => (_values != null ? _values.GetHashCode() : 0);

	public static bool operator ==(MatrixInt a, MatrixInt b) => a.Equals(b);
	public static bool operator !=(MatrixInt a, MatrixInt b) => !a.Equals(b);

	/// <summary>
	///     Transposes Matrix
	/// </summary>
	/// <param name="src"></param>
	/// <returns></returns>
	public static MatrixInt Transpose(MatrixInt src)
	{
		var len = src.Length;
		var transposed = new MatrixInt(len);
		for (var i = 0; i < len; i++)
		{
			for (var j = 0; j < len; j++) transposed[j, i] = src[i, j];
		}

		return transposed;
	}


	/// <summary>
	///     Indicates whether Matrix is Symmetric
	/// </summary>
	/// <param name="src"></param>
	/// <returns></returns>
	public bool IsSymmetric() => this == Transpose(this);

	/// <summary>
	///     Returns neighbor vertices
	/// </summary>
	/// <param name="idx"></param>
	/// <returns></returns>
	public int[] Neighbors(int idx) => _values.GetRow(idx).FindAllIndicesOf(s => s == 1);
}
