using ChemSharp.DataProviders;

namespace ChemSharp
{
    /// <summary>
    /// Marks an Object that has data
    /// </summary>
    public interface IDataObject
    {
        IXYDataProvider DataProvider { get; }
    }
}
