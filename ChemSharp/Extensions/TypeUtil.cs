using System;
using System.Collections.Generic;

namespace ChemSharp.Extensions;

public static class TypeUtil
{
	private static readonly HashSet<Type> NumericTypes = new()
	{
		typeof(decimal),
		typeof(int),
		typeof(byte),
		typeof(sbyte),
		typeof(short),
		typeof(ushort),
		typeof(uint),
		typeof(ulong),
		typeof(long),
		typeof(float),
		typeof(double)
	};

	/// <summary>
	///     Checks if given type is numeric
	/// </summary>
	/// <param name="t"></param>
	/// <returns></returns>
	public static bool IsNumeric(this Type t) =>
		NumericTypes.Contains(t) || NumericTypes.Contains(Nullable.GetUnderlyingType(t));
}
