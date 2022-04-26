using System;
using System.Numerics;
using ChemSharp.Mathematics;
using ChemSharp.Memory;

// ReSharper disable InconsistentNaming

namespace ChemSharp.Molecules.Formats;

public partial class CifFormat
{
	private const string AtomsLoopCCDC = "_atom_site_label";
	private const string BondsLoopCCDC = "_geom_bond";

	private const string HeaderLabelCCDC = "_atom_site_label";
	private const string HeaderSymbolCCDC = "_atom_site_type_symbol";
	private const string HeaderDisorderCCDC = "_atom_site_disorder_group";
	private const string HeaderFractX = "_atom_site_fract_x";
	private const string HeaderFractY = "_atom_site_fract_y";
	private const string HeaderFractZ = "_atom_site_fract_z";

	private const string LenghtLines = "_cell_length_";
	private const string AngleLines = "_cell_angle_";
	private readonly float[] _cellParams = new float[6];

	private Matrix4x4? _conversionMatrix;

	private Atom? ParseAtomCCDC(ReadOnlySpan<char> line)
	{
		var cols = line.WhiteSpaceSplit();
		//filter disorder
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
		var fractX = line.Slice(cols[_colX].start, cols[_colX].length).RemoveUncertainty().ToSingle();
		var fractY = line.Slice(cols[_colY].start, cols[_colY].length).RemoveUncertainty().ToSingle();
		var fractZ = line.Slice(cols[_colZ].start, cols[_colZ].length).RemoveUncertainty().ToSingle();

		var coords =
			FractionalCoordinates.FractionalToCartesian(new Vector3(fractX, fractY, fractZ), _conversionMatrix!.Value);
		return new Atom(symbol.ToString()) {Location = coords, Title = label.ToString()};
	}

	private void ExtractHeaderCCDC(ReadOnlySpan<char> line)
	{
		if (line.StartsWith(HeaderLabelCCDC.AsSpan())) _colLabel = _idh;
		if (line.StartsWith(HeaderSymbolCCDC.AsSpan())) _colSymbol = _idh;
		if (line.StartsWith(HeaderFractX.AsSpan())) _colX = _idh;
		if (line.StartsWith(HeaderFractY.AsSpan())) _colY = _idh;
		if (line.StartsWith(HeaderFractZ.AsSpan())) _colZ = _idh;
		if (line.StartsWith(HeaderDisorderCCDC.AsSpan())) _colDisorder = _idh;
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

	private void SetPickingIndicatorCCDC(ReadOnlySpan<char> line)
	{
		//determine whether to check for atoms or bonds
		if (line.StartsWith(AtomsLoopCCDC.AsSpan()))
		{
			_pickingAtoms = true;
			_pickingBonds = false;
		}
		else if (line.StartsWith(BondsLoopCCDC.AsSpan()))
		{
			_pickingBonds = true;
			_pickingAtoms = false;
		}
	}
}
