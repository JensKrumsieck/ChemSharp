using System;

namespace ChemSharp.UnitConversion;

public struct MappedFunction
{
	public Func<double, double> Function;
	public bool Reciprocal;
}
