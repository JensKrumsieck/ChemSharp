using ChemSharp.Export;
using System;
using System.IO;

namespace ChemSharp.Molecules.Export
{
    public abstract class AbstractMoleculeExporter : IExporter
    {
        public virtual void Export(IExportable exportable, Stream stream)
        {
            if (!(exportable is Molecule)) throw new NotSupportedException("Please use Molecule Type");
        }
    }
}
