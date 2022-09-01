namespace ChemSharp.DataProviders;

/// <summary>
///     Provides access to parameter dictionary from Parameter files
/// </summary>
public interface IParameterProvider
{
	/// <summary>
	///     Storage Dictionary from ParameterFile
	/// </summary>
	IDictionary<string, string> Storage { get; set; }

	/// <summary>
	///     Indexer
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	public string this[string index] { get; }
}
