using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ChemSharp.Export;
using ChemSharp.Molecules.Extensions;
using ChemSharp.Molecules.Mathematics;
using Nodo;

namespace ChemSharp.Molecules;

public partial class Molecule : UndirectedGraph<Atom, Bond>, IExportable
{
	/// <summary>
	///     creates Molecule without Atoms to add later
	/// </summary>
	public Molecule() { }

	/// <summary>
	///     creates Molecule with IEnumerable of Atoms
	/// </summary>
	/// <param name="atoms"></param>
	/// <param name="bonds"></param>
	public Molecule(IEnumerable<Atom> atoms, IEnumerable<Bond>? bonds = null)
		: base(atoms, bonds ?? Enumerable.Empty<Bond>())
	{
		if (bonds == null) RecalculateBonds();
	}

	/// <summary>
	///     Title for Molecule
	/// </summary>
	public string Title { get; set; } = "";

	/// <summary>
	///     The Molecules Atoms
	/// </summary>
	public List<Atom> Atoms => Vertices;

	/// <summary>
	///     The Molecules Bonds
	/// </summary>
	public List<Bond> Bonds => Edges;

	public int Electrons => Atoms.Sum(s => s.Electrons) - ImplicitCharge;

	/// <summary>
	///     Charge is made up of atoms charges and the implicit charge entered for the molecule
	/// </summary>
	public int Charge => InferredCharge + ImplicitCharge;

	private int InferredCharge => Atoms.Sum(s => s.Charge);
	public int ImplicitCharge { get; set; } = 0;

	/// <summary>
	///     Spin Multiplicity following the rules 2s+1
	///     for s = 0 Multiplicity is 1 (default)
	/// </summary>
	public int Multiplicity { get; set; } = 1;

	public double Spin => (Multiplicity - 1d) / 2d;

	public bool IsParamagnetic => Electrons % 2 == 1;

	public string SumFormula => Atoms.SumFormula();
	public double MolecularWeight => Atoms.MolecularWeight();

	/// <summary>
	///     Recalculates Bonds based on Distances
	///     WARNING! Discards every existing Bond information
	/// </summary>
	public void RecalculateBonds()
	{
		Edges.Clear();
#if NET5_0_OR_GREATER
		RecalculateBondsSpan();
#else
		if (Atoms.Count < 600) RecalculateBondsNonParallel();
		else RecalculateBondsParallel();
#endif
	}

#if NET5_0_OR_GREATER
	private void RecalculateBondsSpan()
	{
		var span = CollectionsMarshal.AsSpan(Atoms);
		for (var i = 0; i < span.Length; i++)
		{
			var atomI = span[i];
			for (var j = i + 1; j < span.Length; ++j)
			{
				var atomJ = span[j];
				if (i == j || !atomI.BondToByCovalentRadii(atomJ))
					continue;

				Bonds.Add(new Bond(atomI, atomJ));
			}
		}
	}
#endif

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void RecalculateBondsParallel()
	{
		var matched = new ConcurrentStack<(int, int)>();
		Parallel.For(0, Atoms.Count, i =>
		{
			var atomI = Atoms[i];
			for (var j = i + 1; j < Atoms.Count; ++j)
			{
				var atomJ = Atoms[j];
				if (i == j || !atomI.BondToByCovalentRadii(atomJ))
					continue;
				matched.Push((i, j));
			}
		});
		foreach (var (i, j) in matched.ToArray()) Bonds.Add(new Bond(Atoms[i], Atoms[j]));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void RecalculateBondsNonParallel()
	{
		for (var i = 0; i < Atoms.Count; i++)
		{
			var atomI = Atoms[i];
			for (var j = i + 1; j < Atoms.Count; ++j)
			{
				var atomJ = Atoms[j];
				if (i == j || !atomI.BondToByCovalentRadii(atomJ))
					continue;

				Bonds.Add(new Bond(atomI, atomJ));
			}
		}
	}

	/// <summary>
	///     returns nonmetal neighbors of specific atom
	/// </summary>
	/// <param name="a"></param>
	/// <returns></returns>
	public IEnumerable<Atom> NonMetalNeighbors(Atom a) =>
		Neighbors(a) is { } _ && Neighbors(a).Any()
			? Neighbors(a).Where(s => s.IsNonMetal)
			: Enumerable.Empty<Atom>();
}
