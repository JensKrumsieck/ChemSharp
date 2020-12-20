namespace ChemSharp.DataProviders
{
    /// <summary>
    /// Provides XYData
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// DataPoints to use in Spectrum
        /// </summary>
        DataPoint[] XYData { get; set; }
    }
}
