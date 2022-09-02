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
}
