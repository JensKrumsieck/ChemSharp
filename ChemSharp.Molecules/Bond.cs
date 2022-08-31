using Nodo;

namespace ChemSharp.Molecules;

public sealed class Bond : Edge<Atom>
{
	private Atom[]? _atoms;

	/// <summary>
	///     Indicates whether bond is aromatic
	/// </summary>
	public bool IsAromatic = false;

	/// <summary>
	///     Bond order, not supported by all file types
	/// </summary>
	public int Order = 1;

	public Bond(Atom source, Atom target) : base(source, target) { }
	public Atom Atom1 => Source;
	public Atom Atom2 => Target;

	/// <summary>
	///     Gets Bond Length
	/// </summary>
	public float Length => Atom1.DistanceTo(Atom2);

	/// <summary>
	///     Returns Atoms as List to do LINQ
	/// </summary>
	public Atom[] Atoms => _atoms ??= new[] {Atom1, Atom2};

	public override string ToString() => $"{Atom1.Title} - {Atom2.Title} : {Length}";
}
