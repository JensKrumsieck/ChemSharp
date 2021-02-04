namespace ChemSharp.DataProviders
{
    public abstract class AbstractDataProvider
    {
        public string Path { get; set; }
        protected AbstractDataProvider(string path)
        {
            Path = path;
        }
    }
}
