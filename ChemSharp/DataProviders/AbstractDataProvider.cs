namespace ChemSharp.DataProviders;

public abstract class AbstractDataProvider
{
	protected AbstractDataProvider(string path) => Path = path;

	public string Path { get; set; }
}
