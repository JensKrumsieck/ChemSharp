using System;
using System.Collections.Generic;
using ChemSharp.Memory;

namespace ChemSharp.Molecules.Formats;

public partial class XYZFormat : FileFormat, IAtomFileFormat
{
	private int _idx;
	public XYZFormat(string path) : base(path) { }

	public List<Atom> Atoms { get; } = new();

	public Atom ParseAtom(ReadOnlySpan<char> line)
	{
		var cols = line.WhiteSpaceSplit();
		var symbol = line.Slice(cols[0].start, cols[0].length).ToString();
		var x = line.Slice(cols[1].start, cols[1].length).ToSingle();
		var y = line.Slice(cols[2].start, cols[2].length).ToSingle();
		var z = line.Slice(cols[3].start, cols[3].length).ToSingle();
		return new Atom(symbol, x, y, z);
	}

	protected override void ParseLine(ReadOnlySpan<char> line)
	{
		if (_idx > 1)
			Atoms.Add(ParseAtom(line));
		_idx++;
	}
}
