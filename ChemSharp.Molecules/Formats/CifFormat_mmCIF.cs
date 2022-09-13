namespace ChemSharp.Molecules.Formats;

// ReSharper disable InconsistentNaming
public partial class CifFormat
{
	private const string AtomsLoopMMCif = "_atom_site.group_PDB";

	private const string HeaderLabelMMCif = "_atom_site.label_atom_id";
	private const string HeaderSymbolMMCif = "_atom_site.type_symbol";
	private const string HeaderCartXMMCif = "_atom_site.Cartn_x";
	private const string HeaderCartYMMCif = "_atom_site.Cartn_y";
	private const string HeaderCartZMMCif = "_atom_site.Cartn_z";
	private const string HeaderResidueMMCif = "_atom_site.auth_comp_id";
	private const string HeaderChainIdMMCif = "_atom_site.auth_asym_id";
	private const string HeaderSeqIdMMCif = "_atom_site.auth_seq_id";

	private void ExtractHeaderMMCIF(ReadOnlySpan<char> line)
	{
		if (line.StartsWith(HeaderLabelMMCif.AsSpan())) _colLabel = _idh;
		if (line.StartsWith(HeaderSymbolMMCif.AsSpan())) _colSymbol = _idh;
		if (line.StartsWith(HeaderCartXMMCif.AsSpan())) _colX = _idh;
		if (line.StartsWith(HeaderCartYMMCif.AsSpan())) _colY = _idh;
		if (line.StartsWith(HeaderCartZMMCif.AsSpan())) _colZ = _idh;
		if (line.StartsWith(HeaderResidueMMCif.AsSpan())) _colRes = _idh;
		if (line.StartsWith(HeaderSeqIdMMCif.AsSpan())) _colSeqId = _idh;
		if (line.StartsWith(HeaderChainIdMMCif.AsSpan())) _colChainId = _idh;
		_idh++;
	}

	private void SetPickingIndicatorMMCIF(ReadOnlySpan<char> line)
	{
		//determine whether to check for atoms or bonds
		if (!line.StartsWith(AtomsLoopMMCif.AsSpan())) return;
		_pickingAtoms = true;
		_pickingBonds = false;
	}
}
