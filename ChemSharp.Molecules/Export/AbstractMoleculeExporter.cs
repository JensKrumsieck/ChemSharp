using System;
using System.IO;
using ChemSharp.Export;

namespace ChemSharp.Molecules.Export;

public abstract class AbstractMoleculeExporter : IExporter
{
    public Molecule Molecule { get; protected set; }

    public virtual void Export(IExportable exportable, Stream stream)
    {
        Molecule = exportable as Molecule ?? throw new NotSupportedException("Please use Molecule Type");
    }
}
