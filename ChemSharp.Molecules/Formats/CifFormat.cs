using System;
using System.Collections.Generic;
using ChemSharp.Memory;
using ChemSharp.Molecules.Extensions;

namespace ChemSharp.Molecules.Formats;

internal enum CifType
{
	CCDC,
	mmCIF,
	Comp,
	NotSet
}

/// <summary>
///     Reads and Parses CIF Files from CCDC or PDB
/// </summary>
public partial class CifFormat : FileFormat, IAtomFileFormat, IBondFileFormat
{
	private const string Loop = "loop_";

	private int _a1;
	private int _a2 = 1;
	private int _colDisorder;
	private int _colLabel;
	private int _colRes;
	private int _colSymbol;

	private int _colX;
	private int _colY;
	private int _colZ;

	private int _idh;
	private int _idx;
	private bool _pickingAtoms;
	private bool _pickingBonds;
	private CifType _type = CifType.NotSet;

	public CifFormat(string path) : base(path) { }

	public List<Atom> Atoms { get; } = new();

	public Atom? ParseAtom(ReadOnlySpan<char> line) =>
		_type switch
		{
			CifType.CCDC => ParseAtomCCDC(line),
			CifType.mmCIF => ParseAtomInternal(line),
			CifType.Comp => ParseAtomInternal(line),
			_ => null
		};

	public List<Bond> Bonds { get; } = new();

	public Bond? ParseBond(ReadOnlySpan<char> line) =>
		_type switch
		{
			CifType.CCDC => ParseBondInternal(line),
			CifType.mmCIF => null, //does not have bonds!
			CifType.Comp => ParseBondInternal(line),
			_ => null
		};

	private Atom? ParseAtomInternal(ReadOnlySpan<char> line)
	{
		var cols = line.WhiteSpaceSplit();
		var label = line.Slice(cols[_colLabel].start, cols[_colLabel].length);
		var symbol = line.Slice(cols[_colSymbol].start, cols[_colSymbol].length);
		var x = line.Slice(cols[_colX].start, cols[_colX].length).RemoveUncertainty().ToSingle();
		var y = line.Slice(cols[_colY].start, cols[_colY].length).RemoveUncertainty().ToSingle();
		var z = line.Slice(cols[_colZ].start, cols[_colZ].length).RemoveUncertainty().ToSingle();
		var residue = line.Slice(cols[_colRes].start, cols[_colRes].length).ToString();
		return new Atom(symbol.ToString(), x, y, z) {Title = label.ToString(), Residue = residue};
	}

	private Bond? ParseBondInternal(ReadOnlySpan<char> line)
	{
		var cols = line.WhiteSpaceSplit();
		var strAtom1 = line.Slice(cols[_a1].start, cols[_a1].length).ToString();
		var strAtom2 = line.Slice(cols[_a2].start, cols[_a2].length).ToString();
		var (a1, a2) = Atoms.FindPairwise(strAtom1, strAtom2);
		if (a1 == null || a2 == null) return null!;
		return new Bond(a1, a2);
	}

	protected override void ParseLine(ReadOnlySpan<char> line)
	{
		line = line.Trim();
		if (line.Length == 0) return;
		if (_type == CifType.NotSet) DetermineType(line);

		//pick cell params when type is CCDC
		if (_type == CifType.CCDC &&
		    !_conversionMatrix.HasValue &&
		    (line.StartsWith(LenghtLines.AsSpan()) || line.StartsWith(AngleLines.AsSpan())))
			ParseCellParams(line);

		SetPickingIndicator(line);
		//no block beginning, parse if allowed
		if (_pickingAtoms && !line.StartsWith("_".AsSpan()))
		{
			var atom = ParseAtom(line);
			if (atom != null) Atoms.Add(atom);
		}
		else if (_pickingAtoms) ExtractHeader(line);
		else if (_pickingBonds && !line.StartsWith("_".AsSpan()))
		{
			var bond = ParseBond(line);
			if (bond != null) Bonds.Add(bond);
		}
	}

	/// <summary>
	///     Tries to determine the CCDC Subtype
	/// </summary>
	/// <param name="line"></param>
	private void DetermineType(ReadOnlySpan<char> line)
	{
		const string compNeedle = "_chem_comp.id";
		const string mmcifNeedle = "_pdbx";
		const string ccdcNeedle = "_atom_type_symbol";
		if (line.StartsWith(compNeedle.AsSpan()))
		{
			_type = CifType.Comp;
			_a1 = 1;
			_a2 = 2;
		}

		if (line.StartsWith(mmcifNeedle.AsSpan())) _type = CifType.mmCIF;
		if (line.StartsWith(ccdcNeedle.AsSpan())) _type = CifType.CCDC;
	}

	private void SetPickingIndicator(ReadOnlySpan<char> line)
	{
		//_loop marks block beginning
		if (line.StartsWith(Loop.AsSpan()) || line.StartsWith("#".AsSpan()))
		{
			_pickingAtoms = false;
			_pickingBonds = false;
		}

		if (_type == CifType.CCDC) SetPickingIndicatorCCDC(line);
		if (_type == CifType.mmCIF) SetPickingIndicatorMMCIF(line);
		if (_type == CifType.Comp) SetPickingIndicatorComp(line);
	}

	private void ExtractHeader(ReadOnlySpan<char> line)
	{
		if (_type == CifType.CCDC) ExtractHeaderCCDC(line);
		if (_type == CifType.mmCIF) ExtractHeaderMMCIF(line);
		if (_type == CifType.Comp) ExtractHeaderComp(line);
	}
}
