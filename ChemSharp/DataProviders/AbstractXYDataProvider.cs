namespace ChemSharp.DataProviders;

public abstract class AbstractXYDataProvider : AbstractDataProvider, IXYDataProvider
{
	protected AbstractXYDataProvider(string path) : base(path) { }
	public DataPoint[] XYData { get; set; }
}
