using System;

namespace ChemSharp.Molecules.Formats;

public partial class CifFormat
{
	private const string AtomsLoopComp = "_chem_comp_atom.comp_id";
	private const string BondsLoopComp = "_chem_comp_bond.comp_id";

	private const string HeaderLabelComp = "_chem_comp_atom.atom_id";
	private const string HeaderSymbolComp = "_chem_comp_atom.type_symbol";
	private const string HeaderCartXComp = "_chem_comp_atom.model_Cartn_x";
	private const string HeaderCartYComp = "_chem_comp_atom.model_Cartn_y";
	private const string HeaderCartZComp = "_chem_comp_atom.model_Cartn_z";
	private const string HeaderResidueComp = "_chem_comp_atom.comp_id";

	private void ExtractHeaderComp(ReadOnlySpan<char> line)
	{
		if (line.StartsWith(HeaderLabelComp.AsSpan())) _colLabel = _idh;
		if (line.StartsWith(HeaderSymbolComp.AsSpan())) _colSymbol = _idh;
		if (line.StartsWith(HeaderCartXComp.AsSpan())) _colX = _idh;
		if (line.StartsWith(HeaderCartYComp.AsSpan())) _colY = _idh;
		if (line.StartsWith(HeaderCartZComp.AsSpan())) _colZ = _idh;
		if (line.StartsWith(HeaderResidueComp.AsSpan())) _colRes = _idh;
		_idh++;
	}

	private void SetPickingIndicatorComp(ReadOnlySpan<char> line)
	{
		//determine whether to check for atoms or bonds
		if (line.StartsWith(AtomsLoopComp.AsSpan()))
		{
			_pickingAtoms = true;
			_pickingBonds = false;
		}
		else if (line.StartsWith(BondsLoopComp.AsSpan()))
		{
			_pickingBonds = true;
			_pickingAtoms = false;
		}
	}
}
