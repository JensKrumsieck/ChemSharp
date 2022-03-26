using System;
using System.Collections.Generic;

namespace ChemSharp.GraphTheory;

public interface IGraph<TVertex, TEdge> where TEdge : IEdge<TVertex> where TVertex: IEquatable<TVertex>
{
	public HashSet<TVertex> Vertices { get; }
	public HashSet<TEdge> Edges { get; }

	public List<TVertex> Neighbors(TVertex needle);
}
