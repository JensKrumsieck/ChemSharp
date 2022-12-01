namespace ChemSharp.DataProviders;

/// <summary>
///     Provides XYData
/// </summary>
public interface IXYDataProvider
{
	/// <summary>
	///     DataPoints to use in Spectrum
	/// </summary>
	DataPoint[] XYData { get; set; }

	/// <summary>
	///     Path to provide data from
	/// </summary>
	string Path { get; set; }
}
