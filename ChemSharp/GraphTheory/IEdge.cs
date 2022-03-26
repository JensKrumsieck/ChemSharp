namespace ChemSharp.GraphTheory;

public interface IEdge<out TVertex>
{
	TVertex Source { get; }
	TVertex Target { get; }
}
