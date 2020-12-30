namespace ChemSharp.DataProviders
{
    public abstract class AbstractXYDataProvider : IXYDataProvider
    {
        public DataPoint[] XYData { get; set; }
        public string Path { get; set; }

        protected AbstractXYDataProvider(string path)
        {
            Path = path;
        }
    }
}
