namespace ChemSharp.Molecules.Formats;

public interface IBondFileFormat
{
	public List<Bond> Bonds { get; }
	public Bond? ParseBond(ReadOnlySpan<char> line);
}
