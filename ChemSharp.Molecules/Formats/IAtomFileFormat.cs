using System;
using System.Collections.Generic;

namespace ChemSharp.Molecules.Formats;

public interface IAtomFileFormat
{
	public List<Atom> Atoms { get; }
	public Atom? ParseAtom(ReadOnlySpan<char> line);
}
