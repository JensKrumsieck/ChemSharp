using Nodo;
using Nodo.Extensions;

namespace ChemSharp.Molecules.Extensions;

public static class MoleculeGraphUtil
{
	public static bool
		IsSubgraphIsomorphicTo(this Molecule target, Molecule search, out Dictionary<int, int> mapping) =>
		ConvertToSimpleGraph(target).IsSubgraphIsomorphicTo(ConvertToSimpleGraph(search), out mapping);

	public static bool IsIsomorphicTo(this Molecule target, Molecule search, out Dictionary<int, int> mapping) =>
		ConvertToSimpleGraph(target).IsIsomorphicTo(ConvertToSimpleGraph(search), out mapping);

	public static bool IsSubgraphIsomorphicTo(this Molecule target, Molecule search) =>
		ConvertToSimpleGraph(target).IsSubgraphIsomorphicTo(ConvertToSimpleGraph(search));

	public static bool IsIsomorphicTo(this Molecule target, Molecule search) =>
		ConvertToSimpleGraph(target).IsIsomorphicTo(ConvertToSimpleGraph(search));

	/// <summary>
	///     Converts any Molecule (which is an UndirectedGraph<Atom, Bond>) to an int based UndirectedGraph<int, Edge<int>>.
	/// </summary>
	/// <param name="mol"></param>
	/// <returns></returns>
	public static UndirectedGraph<int, Edge<int>> ConvertToSimpleGraph(this Molecule mol) => new(
		 mol.Atoms.Select(a => mol.Atoms.IndexOf(a)).ToList(),
		 mol.Bonds.Select(b => new Edge<int>(
		                                     mol.Atoms.IndexOf(b.Atom1),
		                                     mol.Atoms.IndexOf(b.Atom2)))
		);

	/// <summary>
	///     Tries to map a search molecule onto a target molecule and returns boolean result.
	///     result contains mappings from target (Item1) to Search(Item2)
	/// </summary>
	/// <param name="target"></param>
	/// <param name="search"></param>
	/// <param name="result"></param>
	/// <returns></returns>
	public static bool TryMap(this Molecule target, Molecule search,
	                          out IList<(Atom atomInTarget, Atom atomInSearch)>? result)
	{
		result = null;
		if (!target.IsSubgraphIsomorphicTo(search, out var mapping)) return false;
		result = mapping.Select(kvp => (target.Atoms[kvp.Key], search.Atoms[kvp.Value])).ToList();
		return true;
	}
}
