﻿namespace ChemSharp.UnitConversion;

public class MagneticUnitConverter : AbstractUnitConverter
{
	public MagneticUnitConverter(string from, string to) : base(from, to)
	{
		AddConversion("T", 1);
		AddConversion("G", 1e-4);
		AddConversion("mT", 1e-3);
		AddConversion("Oe", 1e-4);
	}
}
