using System;
using System.Collections.Generic;
using ChemSharp.Memory;
using ChemSharp.Molecules.DataProviders;

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
		var id = line.Slice(cols[1].start, cols[1].length).ToString();
		var x = line.Slice(cols[2].start, cols[2].length).ToSingle();
		var y = line.Slice(cols[3].start, cols[3].length).ToSingle();
		var z = line.Slice(cols[4].start, cols[4].length).ToSingle();
		var type = line.Slice(cols[5].start, cols[5].length);
		var residue = line.Slice(cols[7].start, cols[7].length);
		type = type.PointSplit();
		//string cast necessary?
		var typeStr = type.ToString();
		var symbol = ElementDataProvider.ColorData.ContainsKey(typeStr)
			? RegexUtil.AtomLabel.Match(typeStr).Value
			: RegexUtil.AtomLabel.Match(id).Value;
		return new Atom(symbol, x, y, z) {Title = id, Residue = residue.ToString()};
	}

	public List<Bond> Bonds { get; } = new();


	public Bond ParseBond(ReadOnlySpan<char> line)
	{
		//subtract 1 as mol2 starts counting at 1
		var cols = line.WhiteSpaceSplit();
		var a1 = line.Slice(cols[1].start, cols[1].length).ToInt() - 1;
		var a2 = line.Slice(cols[2].start, cols[2].length).ToInt() - 1;
		var type = line.Slice(cols[3].start, cols[3].length).ToString();
		var aromatic = type == "ar";
		var suc = int.TryParse(type, out var order);
		return new Bond(Atoms[a1], Atoms[a2]) {IsAromatic = aromatic, Order = suc ? order : 0};
	}

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
			if (_pickingAtoms) Atoms.Add(ParseAtom(line));
			if (_pickingBonds) Bonds.Add(ParseBond(line));
		}
	}

	public static Molecule Read(string path)
	{
		var format = new Mol2Format(path);
		format.ReadInternal();
		return new Molecule(format.Atoms, format.Bonds);
	}
}
