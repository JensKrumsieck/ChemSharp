namespace ChemSharp.DataProviders;

public abstract class AbstractXYDataProvider : AbstractDataProvider, IXYDataProvider
{
    public DataPoint[] XYData { get; set; }

    protected AbstractXYDataProvider(string path) : base(path) { }
}
