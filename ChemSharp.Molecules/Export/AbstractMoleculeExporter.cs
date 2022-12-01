using ChemSharp.Export;

namespace ChemSharp.Molecules.Export;

public abstract class AbstractMoleculeExporter : IExporter
{
	protected Molecule? Molecule { get; private set; }

	public virtual void Export(IExportable exportable, Stream stream) => Molecule =
		exportable as Molecule ?? throw new NotSupportedException("Please use Molecule Type");
}
