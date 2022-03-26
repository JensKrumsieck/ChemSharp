namespace ChemSharp.GraphTheory;

public class Edge<TVertex> : IEdge<TVertex>
{
	public TVertex Source { get; }
	public TVertex Target { get; }

	public Edge(TVertex source, TVertex target)
	{
		Source = source;
		Target = target;
	}

	public override string ToString() => $"{Source} <-> {Target}";
}
