namespace ChemSharp.Molecules.Extensions;

public static class MoleculeLinq
{
	/// <summary>
	/// Returns a subset of a molecule matching the predicate foreach atom.
	/// </summary>
	/// <param name="mol"></param>
	/// <param name="predicate"></param>
	/// <returns></returns>
	public static Molecule Where(this Molecule mol, Func<Atom, bool> predicate)
	{
		//get subset of atoms
		var atoms = mol.Atoms.Where(predicate);
		//get all bonds where both atoms are matching the predicate
		var bonds = mol.Bonds.Where(b => predicate(b.Atom1) && predicate(b.Atom2));
		return new Molecule(atoms, bonds);
	}

	/// <summary>
	/// Use Constructor for Chaining!
	/// </summary>
	/// <param name="atoms"></param>
	/// <returns></returns>
	public static Molecule ToMolecule(this IEnumerable<Atom> atoms) => new(atoms);

	/// <summary>
	/// Create Molecule from Bonds
	/// </summary>
	/// <param name="bonds"></param>
	/// <returns></returns>
	public static Molecule ToMolecule(this IEnumerable<Bond> bonds)
	{
		var atoms = new List<Atom>();
		var enumerate = bonds.ToList();
		foreach (var bond in enumerate)
		{
			if (!atoms.Contains(bond.Atom1)) atoms.Add(bond.Atom1);
			if (!atoms.Contains(bond.Atom2)) atoms.Add(bond.Atom2);
		}

		return new Molecule(atoms, enumerate);
	}
}
