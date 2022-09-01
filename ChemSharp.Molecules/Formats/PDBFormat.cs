using ChemSharp.Extensions;
using ChemSharp.Memory;

namespace ChemSharp.Molecules.Formats;

public partial class PDBFormat : FileFormat, IAtomFileFormat
{
	private const string Atom = "ATOM";
	private const string HetAtom = "HETATM";
	private PDBFormat(string path) : base(path) { }

	public List<Atom> Atoms { get; } = new();

	public Atom ParseAtom(ReadOnlySpan<char> line)
	{
		/*
		 * http://www.wwpdb.org/documentation/file-format-content/format33/sect9.html#ATOM
		COLUMNS        DATA  TYPE    FIELD        DEFINITION
		-------------------------------------------------------------------------------------
		 1 -  6        Record name   "ATOM  "
		 7 - 11        Integer       serial       Atom  serial number.
		13 - 16        Atom          name         Atom name.
		17             Character     altLoc       Alternate location indicator.
		18 - 20        Residue name  resName      Residue name.
		22             Character     chainID      Chain identifier.
		23 - 26        Integer       resSeq       Residue sequence number.
		27             AChar         iCode        Code for insertion of residues.
		31 - 38        Real(8.3)     x            Orthogonal coordinates for X in Angstroms.
		39 - 46        Real(8.3)     y            Orthogonal coordinates for Y in Angstroms.
		47 - 54        Real(8.3)     z            Orthogonal coordinates for Z in Angstroms.
		55 - 60        Real(6.2)     occupancy    Occupancy.
		61 - 66        Real(6.2)     tempFactor   Temperature  factor.
		77 - 78        LString(2)    element      Element symbol, right-justified.
		79 - 80        LString(2)    charge       Charge  on the atom.
	 */
		var title = line[12..16].Trim();
		var x = line[30..38].Trim();
		var y = line[38..46].Trim();
		var z = line[46..54].Trim();
		var symbol = line[76..78].Trim();
		var resName = line[17..20].Trim();
		//var chainId = line[21..22].Trim();
//#if NETCOREAPP3_0_OR_GREATER
//		var residue = string.Concat(resName,"-", chainId);
//#else
//		var residue = string.Concat(resName.ToString(), chainId.ToString());
//#endif
		return new Atom(symbol.ToString().UcFirst(), x.ToSingle(), y.ToSingle(), z.ToSingle())
		{
			Title = title.ToString(), Residue = resName.ToString()
		};
	}

	protected override void ParseLine(ReadOnlySpan<char> line)
	{
		line = line.Trim();
		if (line.StartsWith(Atom.AsSpan()) || line.StartsWith(HetAtom.AsSpan()))
			Atoms.Add(ParseAtom(line));
	}
}
