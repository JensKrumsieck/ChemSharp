using System;
using System.IO;

namespace ChemSharp.Molecules.Formats;

public abstract class FileFormat
{
	protected readonly string Path;
	protected FileFormat(string path) => Path = path;

	protected abstract void ParseLine(ReadOnlySpan<char> line);

	protected void ReadFromFileInternal()
	{
		using var fs = File.OpenRead(Path);
		ReadFromStreamInternal(fs);
	}

	protected void ReadFromStreamInternal(Stream data)
	{
		using var sr = new StreamReader(data);
		ReadInternal(sr);
	}

	protected void ReadFromStringInternal(string file)
	{
		var sr = new StringReader(file);
		ReadInternal(sr);
	}

	private void ReadInternal(TextReader sr)
	{
		while (sr.Peek() > 0)
		{
			var line = sr.ReadLine().AsSpan();
			ParseLine(line);
		}
	}
}
