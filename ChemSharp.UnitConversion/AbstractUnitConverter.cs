using System.Collections.Generic;
using ChemSharp.Extensions;

namespace ChemSharp.UnitConversion;

public abstract class AbstractUnitConverter
{
	internal string _fromKey;
	internal string _toKey;

	protected AbstractUnitConverter(string from, string to)
	{
		_fromKey = from;
		_toKey = to;
	}

	protected Dictionary<string, MappedFunction> SIUnitLookup { get; } = new();

	/// <summary>
	///     Add new Conversion to Lookup
	/// </summary>
	/// <param name="unit"></param>
	/// <param name="converter"></param>
	public void AddConversion(string unit, MappedFunction converter) => SIUnitLookup.Add(unit, converter);

	/// <summary>
	///     Addd new Conversion
	/// </summary>
	/// <param name="unit"></param>
	/// <param name="factor"></param>
	public void AddConversion(string unit, double factor) =>
		AddConversion(unit, new MappedFunction {Function = s => s * factor, Reciprocal = false});

	internal double Conversion(string from, string to, double val)
	{
		var baseUnit = SIUnitLookup.TryAndGet(from).Function(val);
		var mfunc = SIUnitLookup.TryAndGet(to);
		var toVal = baseUnit / mfunc.Function(1);
		return mfunc.Reciprocal ? 1 / toVal : toVal;
	}

	/// <summary>
	///     Converts from left to right (from -> to)
	/// </summary>
	/// <returns></returns>
	public double Convert(double value) => Conversion(_fromKey, _toKey, value);

	/// <summary>
	///     Converts inverted (right to left, to -> from)
	/// </summary>
	/// <returns></returns>
	public double ConvertInverted(double value) => Conversion(_toKey, _fromKey, value);
}
