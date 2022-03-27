using System;

namespace ChemSharp.Molecules.Formats;

public abstract class FileFormat
{
	protected readonly string Path;
	protected FileFormat(string path) => Path = path;
	protected abstract Atom ParseAtom(ReadOnlySpan<char> line);
}
