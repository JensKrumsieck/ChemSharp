using System;
using System.Collections.Generic;
using System.Numerics;
using ChemSharp.Memory;

namespace ChemSharp.Molecules.Formats;

public class Mol2Format : FileFormat, IAtomFileFormat, IBondFileFormat
{
	private const string Tripos = "@<TRIPOS>";
	private const string AtomsBlock = $"{Tripos}ATOM";
	private const string BondsBlock = $"{Tripos}BOND";
	private bool _pickingAtoms;
	private bool _pickingBonds;

	public Mol2Format(string path) : base(path) { }

	public List<Atom> Atoms { get; } = new();

	public Atom ParseAtom(ReadOnlySpan<char> line)
	{
		var cols = line.WhiteSpaceSplit();
		var id = line.Slice(cols[1].Item1, cols[1].Item2);
		var x = line.Slice(cols[2].Item1, cols[2].Item2).ToSingle();
		var y = line.Slice(cols[3].Item1, cols[3].Item2).ToSingle();
		var z = line.Slice(cols[4].Item1, cols[4].Item2).ToSingle();
		var loc = new Vector3(x, y, z);
		var type = line.Slice(cols[5].Item1, cols[5].Item2);
		var pointLoc = type.IndexOf('.');
		type = pointLoc != -1 ? type[..pointLoc] : type;
		return null!;
	}


	public List<Bond> Bonds { get; } = new();
	public Bond ParseBond(ReadOnlySpan<char> line) => throw new NotImplementedException();

	protected override void ParseLine(ReadOnlySpan<char> line)
	{
		//@<TRIPOS> marks block beginning
		if (line.StartsWith(Tripos.AsSpan()))
		{
			//determine whether to check for atoms or bonds
			if (line.StartsWith(AtomsBlock.AsSpan()))
			{
				_pickingAtoms = true;
				_pickingBonds = false;
			}
			else if (line.StartsWith(BondsBlock.AsSpan()))
			{
				_pickingBonds = true;
				_pickingAtoms = false;
			}
			else
			{
				_pickingAtoms = false;
				_pickingBonds = false;
			}
		}
		else
		{
			//no block beginning, parse if allowed
			if (_pickingAtoms) ParseAtom(line);
			if (_pickingBonds) ParseBond(line);
		}
	}

	public static Molecule Read(string path)
	{
		var format = new Mol2Format(path);
		format.ReadInternal();
		return new Molecule(format.Atoms);
	}
}
