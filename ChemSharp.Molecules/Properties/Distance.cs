using System.Numerics;

namespace ChemSharp.Molecules.Properties;

public class Distance : KeyValueProperty
{
	public readonly Atom Atom1;
	public readonly Atom Atom2;

	public Distance(Atom a1, Atom a2)
	{
		Atom1 = a1;
		Atom2 = a2;
	}

	public override double Value => Vector3.Distance(Atom1.Location, Atom2.Location);
	public override string Key => $"{Atom1.Title} - {Atom2.Title}";
	public override string Unit => "Å";
}
