using ChemSharp.Memory;
using ChemSharp.Molecules.DataProviders;

namespace ChemSharp.Molecules.Formats;

public partial class Mol2Format : FileFormat, IAtomFileFormat, IBondFileFormat
{
	private const string Tripos = "@<TRIPOS>";
	private const string AtomsBlock = $"{Tripos}ATOM";
	private const string BondsBlock = $"{Tripos}BOND";
	private const string SubstructureBlock = $"{Tripos}SUBSTRUCTURE";

	private readonly Dictionary<int, HashSet<int>> _chainIdToAtomId = new();
	private int _i;
	private bool _pickingAtoms;
	private bool _pickingBonds;
	private bool _pickingSubstructure;

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
		var chainId = (char)line.Slice(cols[6].start, cols[6].length).ToInt();
		if (!_chainIdToAtomId.ContainsKey(chainId)) _chainIdToAtomId[chainId] = new HashSet<int>();
		_chainIdToAtomId[chainId].Add(_i++);
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
			if (_pickingSubstructure) UpdateChainId(line);
		}
	}

	private void UpdateChainId(ReadOnlySpan<char> line)
	{
		var cols = line.WhiteSpaceSplit();
		if (cols.Length <= 5) return;
		if (cols[5].length == 0) return;
		var id = line.Slice(cols[0].start, cols[0].length).ToInt(); //index of chain
		var chainIdRaw = line.Slice(cols[5].start, cols[5].length).Trim();
		var chainId = chainIdRaw.Length > 0 ? chainIdRaw[0] : 'ä';
		var atoms = Atoms.Where((_, j) => _chainIdToAtomId[id].Contains(j));
		foreach (var atom in atoms) atom.ChainId = chainId;
	}

	private void SetPickingIndicator(ReadOnlySpan<char> line)
	{
		//determine whether to check for atoms or bonds
		if (line.StartsWith(AtomsBlock.AsSpan()))
		{
			_pickingAtoms = true;
			_pickingBonds = false;
			_pickingSubstructure = false;
		}
		else if (line.StartsWith(BondsBlock.AsSpan()))
		{
			_pickingBonds = true;
			_pickingAtoms = false;
			_pickingSubstructure = false;
		}
		else if (line.StartsWith(SubstructureBlock.AsSpan()))
		{
			_pickingAtoms = false;
			_pickingBonds = false;
			_pickingSubstructure = true;
		}
		else
		{
			_pickingAtoms = false;
			_pickingBonds = false;
			_pickingSubstructure = false;
		}
	}
}
