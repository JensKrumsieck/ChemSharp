namespace ChemSharp.Molecules;

public sealed class Bond
{
	public readonly Atom Atom1;
	public readonly Atom Atom2;

	private Atom[]? _atoms;

	/// <summary>
	///     Indicates whether bond is aromatic
	/// </summary>
	public bool IsAromatic = false;

	/// <summary>
	///     Bond order, not supported by all file types
	/// </summary>
	public int Order = 1;

	public Bond(Atom a1, Atom a2)
	{
		Atom1 = a1;
		Atom2 = a2;
	}

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
