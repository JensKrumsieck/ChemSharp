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
	///     Converts any Molecule (which is an UndirectedGraph&lt;Atom, Bond&gt;) to an int based UndirectedGraph&lt;int, Edge&lt;int&gt;&gt;.
	/// </summary>
	/// <param name="mol"></param>
	/// <returns></returns>
	public static UndirectedGraph<int, Edge<int>> ConvertToSimpleGraph(this Molecule mol) => new(
		 mol.Atoms.Select((_, i) => i).ToList(),
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

	/// <summary>
	///     Returns all mappings of search to target molecule as a dictionary containing the search atoms as keys with a list
	///     of the mapped atoms
	/// </summary>
	/// <param name="target"></param>
	/// <param name="search"></param>
	/// <returns></returns>
	public static Dictionary<Atom, List<Atom>> MapAll(this Molecule target, Molecule search)
	{
		var tmp = target;
		var mappings = new Dictionary<Atom, List<Atom>>();
		while (tmp.TryMap(search, out var mapping))
		{
			tmp = new Molecule(tmp.Atoms.Except(mapping!.Select(s => s.atomInTarget)));
			foreach (var (atomInTarget, atomInSearch) in mapping!)
				if (!mappings.ContainsKey(atomInSearch)) mappings[atomInSearch] = new List<Atom> {atomInTarget};
				else mappings[atomInSearch].Add(atomInTarget);
		}

		return mappings;
	}

	public static IEnumerable<Molecule> GetSubgraphs(this Molecule target, Molecule search)
	{
		var mappings = target.MapAll(search);
		if (!mappings.Any()) return new List<Molecule>();

		var tmp = new List<List<Atom>>();
		foreach (var list in mappings.Values)
		{
			for (var i = 0; i < list.Count; i++)
			{
				if (i >= tmp.Count) tmp.Add(new List<Atom>());
				tmp[i].Add(list[i]);
			}
		}

		return tmp.ToMolecules();
	}
}
