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
	protected SpectrumExportFlags Flags = SpectrumExportFlags.Experimental;

	protected Spectrum Spectrum { get; set; }

	protected virtual void Export(IExportable exportable, Stream stream) => Spectrum =
		exportable as Spectrum ?? throw new NotSupportedException("Please use Spectrum Type");
}
