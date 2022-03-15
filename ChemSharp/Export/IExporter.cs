using System.IO;

namespace ChemSharp.Export;

/// <summary>
/// Defines an Exporter
/// </summary>
public interface IExporter
{
    /// <summary>
    /// Exports Data to Stream
    /// </summary>
    /// <param name="exportable"></param>
    /// <param name="stream"></param>
    void Export(IExportable exportable, Stream stream);
}
