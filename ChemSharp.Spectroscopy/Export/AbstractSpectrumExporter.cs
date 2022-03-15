using System;
using System.IO;
using ChemSharp.Export;

namespace ChemSharp.Spectroscopy.Export;

[Flags]
public enum SpectrumExportFlags
{
    Experimental,
    Derivative,
    Integral
}

public abstract class AbstractSpectrumExporter
{
    public SpectrumExportFlags Flags = SpectrumExportFlags.Experimental;

    public Spectrum Spectrum { get; protected set; }

    public virtual void Export(IExportable exportable, Stream stream)
    {
        Spectrum = exportable as Spectrum ?? throw new NotSupportedException("Please use Spectrum Type");
    }
}
