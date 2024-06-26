﻿using System.Globalization;

namespace ChemSharp.Extensions;

public static class StringUtil
{
	public static readonly string[] NewLines = {"\n", "\r\n", "\r"};
	public static readonly char[] WhiteSpaces = {' ', '\t'};

	/// <summary>
	///     Provides string with n spaces
	/// </summary>
	/// <param name="n"></param>
	/// <returns></returns>
	public static string Spaces(int n) => new(' ', n);

	/// <summary>
	///     Shorthand for culture conversion
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static string ToInvariantString(this float input) => input.ToString(CultureInfo.InvariantCulture);

	/// <summary>
	///     Shorthand for culture conversion
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static string ToInvariantString(this double input) => input.ToString(CultureInfo.InvariantCulture);

	/// <summary>
	///     shorthand for splitting strings into line array
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static string[] DefaultSplit(this string input) =>
		input.Split(NewLines, StringSplitOptions.RemoveEmptyEntries);

	/// <summary>
	///     Converts String to Int in Invariant Culture
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static int ToInt(this string input) => Convert.ToInt32(input, CultureInfo.InvariantCulture);

	/// <summary>
	///     Converts String to float in Invariant Culture
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static float ToSingle(this string input) => Convert.ToSingle(input, CultureInfo.InvariantCulture);

	/// <summary>
	///     Converts String to double in Invariant Culture
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static double ToDouble(this string input) => Convert.ToDouble(input, CultureInfo.InvariantCulture);

	/// <summary>
	///     Remove brackets from number string
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static string RemoveUncertainty(this string input)
	{
		var span = input.AsSpan();
		var pos = span.IndexOf('(');
		return pos == -1 ? input : span[..pos].ToString();
	}

	/// <summary>
	///     Splits and Whitespace
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static string[] WhiteSpaceSplit(this string input) => input
		.Split(WhiteSpaces, StringSplitOptions.RemoveEmptyEntries);

	/// <summary>
	///     sets first letter to uppercase and others to lowercase
	/// </summary>
	/// <param name="s"></param>
	/// <returns></returns>
	public static string UcFirst(this string s) => char.ToUpper(s[0]) + s[1..].ToLower();
}
