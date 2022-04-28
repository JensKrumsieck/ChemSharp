using System;
using System.Collections.Generic;
using ChemSharp.Memory;

namespace ChemSharp.Molecules.Formats;

public partial class MolFormat : FileFormat, IAtomFileFormat, IBondFileFormat
{
	private const string Version = "V2000";
	private int _idx;
	private int _noAtoms;
	private int _noBonds;

	private bool _pickingAtoms;
	private bool _pickingBonds;

	public MolFormat(string path) : base(path) { }

	public List<Atom> Atoms { get; } = new();

	public Atom ParseAtom(ReadOnlySpan<char> line)
	{
		var cols = line.WhiteSpaceSplit();
		var x = line.Slice(cols[0].start, cols[0].length).ToSingle();
		var y = line.Slice(cols[1].start, cols[1].length).ToSingle();
		var z = line.Slice(cols[2].start, cols[2].length).ToSingle();
		var symbol = line.Slice(cols[3].start, cols[3].length).ToString();
		return new Atom(symbol, x, y, z);
	}

	public List<Bond> Bonds { get; } = new();

	public Bond ParseBond(ReadOnlySpan<char> line)
	{
		var cols = line.WhiteSpaceSplit();
		var iA1 = line.Slice(cols[0].start, cols[0].length).ToInt();
		var iA2 = line.Slice(cols[1].start, cols[1].length).ToInt();
		var order = line.Slice(cols[2].start, cols[2].length).ToInt();
		var bond = new Bond(Atoms[iA1 - 1], Atoms[iA2 - 1]);
		if (order == 4)
		{
			bond.IsAromatic = true;
			order = 1;
		}

		bond.Order = order;
		return bond;
	}

	protected override void ParseLine(ReadOnlySpan<char> line)
	{
		line = line.Trim();
		if (_pickingAtoms && _idx < _noAtoms)
		{
			Atoms.Add(ParseAtom(line));
			_idx++;
			if (_idx == _noAtoms)
			{
				_pickingAtoms = false;
				_pickingBonds = true;
				_idx = 0;
			}
		}
		else if (_pickingBonds && _idx < _noBonds)
		{
			Bonds.Add(ParseBond(line));
			_idx++;
		}

		//get counts from information line
		if (line.EndsWith(Version.AsSpan()))
			GetFileInfo(line);
	}

	private void GetFileInfo(ReadOnlySpan<char> line)
	{
		var cols = line.WhiteSpaceSplit();
		_noAtoms = line.Slice(cols[0].start, cols[0].length).ToInt();
		_noBonds = line.Slice(cols[1].start, cols[1].length).ToInt();
		_pickingAtoms = true;
	}
}
