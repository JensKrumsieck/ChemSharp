using Nodo;
using Nodo.Extensions;

namespace ChemSharp.Molecules.Extensions;

public static class MoleculeUtil
{
	public static bool IsSubgraphIsomorphicTo(this Molecule target, Molecule search) =>
		ConvertToSimpleGraph(target).IsSubgraphIsomorphicTo(ConvertToSimpleGraph(search));

	public static bool IsIsomorphicTo(this Molecule target, Molecule search) =>
		ConvertToSimpleGraph(target).IsIsomorphicTo(ConvertToSimpleGraph(search));

	public static UndirectedGraph<int, Edge<int>> ConvertToSimpleGraph(this Molecule mol) => new(
		 mol.Atoms.Select(a => mol.Atoms.IndexOf(a)).ToList(),
		 mol.Bonds.Select(b => new Edge<int>(
		                                     mol.Atoms.IndexOf(b.Atom1),
		                                     mol.Atoms.IndexOf(b.Atom2)))
		);
}
