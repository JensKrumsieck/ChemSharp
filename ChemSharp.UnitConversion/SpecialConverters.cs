namespace ChemSharp.UnitConversion;

public static class SpecialConverters
{
	#region EPRSpectroscopy

	/// <summary>
	///     Calculate g factor of given magnetic value and frequency
	/// </summary>
	/// <param name="xInput"></param>
	/// <param name="frequency"></param>
	/// <param name="unit"></param>
	/// <returns></returns>
	public static float GFromB(float bInput, float frequency, string unit = "G")
	{
		//convert input to Tesla unit
		var converter = new MagneticUnitConverter(unit, "T");
		return (float)(Constants.Planck * frequency * 1e9 / (Constants.BohrM * converter.Convert(bInput)));
	}

	/// <summary>
	///     Calculate magnetic value of given g factor and frequency
	/// </summary>
	/// <param name="gInput"></param>
	/// <param name="frequency"></param>
	/// <param name="unit"></param>
	/// <returns></returns>
	public static float BFromG(float gInput, float frequency, string unit = "G")
	{
		var value = Constants.Planck * frequency * 1e9 / (Constants.BohrM * gInput);
		var converter = new MagneticUnitConverter("T", unit);
		return (float)converter.Convert(value);
	}

	#endregion
}
