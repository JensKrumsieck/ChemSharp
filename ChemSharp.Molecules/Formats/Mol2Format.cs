using ChemSharp.Memory;
using ChemSharp.Molecules.DataProviders;

namespace ChemSharp.Molecules.Formats;

public partial class Mol2Format : FileFormat, IAtomFileFormat, IBondFileFormat
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
		var residueRaw = line.Slice(cols[7].start, cols[7].length);
		var idPos = residueRaw.FirstNumeric();
		var residue = idPos != -1 ? residueRaw[..idPos] : residueRaw;
		var resId = idPos != -1 ? residueRaw[idPos..].ToInt() : 0;
		type = type.PointSplit();
		//most of the time type contains the actual type, so casting to string is ok!
		var typeStr = type.ToString();
		var pos = id.AsSpan().FirstNumeric();
		var symbol = ElementDataProvider.ColorData.ContainsKey(typeStr)
			? typeStr
			: pos != -1
				? id[..pos]
				: id;
		return new Atom(symbol, x, y, z) {Title = id, Residue = residue.ToString(), ResidueId = resId};
	}

	public List<Bond> Bonds { get; } = new();


	public Bond ParseBond(ReadOnlySpan<char> line)
	{
		//subtract 1 as mol2 starts counting at 1
		var cols = line.WhiteSpaceSplit();
		var a1 = line.Slice(cols[1].start, cols[1].length).ToInt() - 1;
		var a2 = line.Slice(cols[2].start, cols[2].length).ToInt() - 1;
		var type = line.Slice(cols[3].start, cols[3].length);
		var aromatic = type.StartsWith("ar".AsSpan());
#if NETSTANDARD2_0
		var suc = int.TryParse(type.ToString(), out var order);
#else
		var suc = int.TryParse(type, out var order);
#endif
		return new Bond(Atoms[a1], Atoms[a2]) {IsAromatic = aromatic, Order = suc ? order : 1};
	}

	protected override void ParseLine(ReadOnlySpan<char> line)
	{
		//@<TRIPOS> marks block beginning
		if (line.StartsWith(Tripos.AsSpan()))
			SetPickingIndicator(line);
		else
		{
			//no block beginning, parse if allowed
			if (_pickingAtoms) Atoms.Add(ParseAtom(line));
			if (_pickingBonds) Bonds.Add(ParseBond(line));
		}
	}

	private void SetPickingIndicator(ReadOnlySpan<char> line)
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
}
