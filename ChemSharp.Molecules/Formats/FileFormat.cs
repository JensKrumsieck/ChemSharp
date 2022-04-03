using System;
using System.IO;

namespace ChemSharp.Molecules.Formats;

public abstract class FileFormat
{
	protected readonly string Path;
	protected FileFormat(string path) => Path = path;

	protected abstract void ParseLine(ReadOnlySpan<char> line);

	protected void ReadInternal()
	{
		using var fs = File.OpenRead(Path);
		using var sr = new StreamReader(fs);
		while (sr.Peek() > 0)
		{
			var line = sr.ReadLine().AsSpan();
			ParseLine(line);
		}
	}
}
