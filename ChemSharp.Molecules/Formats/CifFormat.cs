using System;
using System.Collections.Generic;
using System.Numerics;
using ChemSharp.Mathematics;
using ChemSharp.Memory;
using ChemSharp.Molecules.Extensions;
using static System.IO.Path;


namespace ChemSharp.Molecules.Formats;

public class CifFormat : FileFormat, IAtomFileFormat, IBondFileFormat
{
	private const string Loop = "loop_";
	private const string AtomsLoop = "_atom_site_label";
	private const string BondsLoop = "_geom_bond";

	private const string LenghtLines = "_cell_length_";
	private const string AngleLines = "_cell_angle_";

	private const string HeaderLabel = "_atom_site_label";
	private const string HeaderSymbol = "_atom_site_type_symbol";
	private const string HeaderFractX = "_atom_site_fract_x";
	private const string HeaderFractY = "_atom_site_fract_y";
	private const string HeaderFractZ = "_atom_site_fract_z";
	private const string HeaderDisorder = "_atom_site_disorder_group";

	private readonly float[] _cellParams = new float[6];
	private int _colDisorder;
	private int _colFractX;
	private int _colFractY;
	private int _colFractZ;

	private int _colLabel;
	private int _colSymbol;
	private Matrix4x4? _conversionMatrix;

	private int _idh;

	private int _idx;

	private bool _pickingAtoms;
	private bool _pickingBonds;

	public CifFormat(string path) : base(path) { }

	public List<Atom> Atoms { get; } = new();

	public Atom? ParseAtom(ReadOnlySpan<char> line)
	{
		//filter disorder
		var cols = line.WhiteSpaceSplit();
		var disorder = 0;
		if (_colDisorder != 0 && _colDisorder < cols.Length)
#if NETSTANDARD2_0
			int.TryParse(line.Slice(cols[_colDisorder].start, cols[_colDisorder].length).ToString(), out disorder);
#else
			int.TryParse(line.Slice(cols[_colDisorder].start, cols[_colDisorder].length).ToString(), out disorder);
#endif
		if (disorder >= 2) return null;
		//parse atom
		var label = line.Slice(cols[_colLabel].start, cols[_colLabel].length);
		var symbol = line.Slice(cols[_colSymbol].start, cols[_colSymbol].length);
		var fractX = line.Slice(cols[_colFractX].start, cols[_colFractX].length).RemoveUncertainty().ToSingle();
		var fractY = line.Slice(cols[_colFractY].start, cols[_colFractY].length).RemoveUncertainty().ToSingle();
		var fractZ = line.Slice(cols[_colFractZ].start, cols[_colFractZ].length).RemoveUncertainty().ToSingle();

		var coords =
			FractionalCoordinates.FractionalToCartesian(new Vector3(fractX, fractY, fractZ), _conversionMatrix!.Value);
		return new Atom(symbol.ToString()) {Location = coords, Title = label.ToString()};
	}

	public List<Bond> Bonds { get; } = new();

	public Bond? ParseBond(ReadOnlySpan<char> line)
	{
		var cols = line.WhiteSpaceSplit();
		var strAtom1 = line.Slice(cols[0].start, cols[0].length).ToString();
		var strAtom2 = line.Slice(cols[1].start, cols[1].length).ToString();
		var (a1, a2) = Atoms.FindPairwise(strAtom1, strAtom2);
		if (a1 == null || a2 == null) return null!;
		return new Bond(a1, a2);
	}

	protected override void ParseLine(ReadOnlySpan<char> line)
	{
		line = line.Trim();
		if (line.Length == 0) return;
		//pick cell params
		if (!_conversionMatrix.HasValue &&
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

	private void SetPickingIndicator(ReadOnlySpan<char> line)
	{
		//_loop marks block beginning
		if (line.StartsWith(Loop.AsSpan()))
		{
			_pickingAtoms = false;
			_pickingBonds = false;
		}

		//determine whether to check for atoms or bonds
		if (line.StartsWith(AtomsLoop.AsSpan()))
		{
			_pickingAtoms = true;
			_pickingBonds = false;
		}
		else if (line.StartsWith(BondsLoop.AsSpan()))
		{
			_pickingBonds = true;
			_pickingAtoms = false;
		}
	}

	private void ExtractHeader(ReadOnlySpan<char> line)
	{
		if (line.StartsWith(HeaderLabel.AsSpan())) _colLabel = _idh;
		if (line.StartsWith(HeaderSymbol.AsSpan())) _colSymbol = _idh;
		if (line.StartsWith(HeaderFractX.AsSpan())) _colFractX = _idh;
		if (line.StartsWith(HeaderFractY.AsSpan())) _colFractY = _idh;
		if (line.StartsWith(HeaderFractZ.AsSpan())) _colFractZ = _idh;
		if (line.StartsWith(HeaderDisorder.AsSpan())) _colDisorder = _idh;
		_idh++;
	}

	private void ParseCellParams(ReadOnlySpan<char> line)
	{
		var cols = line.WhiteSpaceSplit();
		_cellParams[_idx] = line[cols[1].start..].RemoveUncertainty().ToSingle();
		_idx++;

		if (_idx == 6)
			_conversionMatrix = FractionalCoordinates.ConversionMatrix(
			                                                           _cellParams[0],
			                                                           _cellParams[1],
			                                                           _cellParams[2],
			                                                           _cellParams[3],
			                                                           _cellParams[4],
			                                                           _cellParams[5]);
	}

	public static Molecule Read(string path)
	{
		var format = new CifFormat(path);
		format.ReadInternal();
		return new Molecule(format.Atoms, format.Bonds) {Title = GetFileNameWithoutExtension(format.Path)};
	}
}
