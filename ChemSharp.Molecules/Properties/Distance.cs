using ChemSharp.Mathematics;

namespace ChemSharp.Molecules.Properties;

public class Distance : KeyValueProperty
{
    public override double Value => MathV.Distance(Atom1.Location, Atom2.Location);
    public override string Key => $"{Atom1.Title} - {Atom2.Title}";
    public override string Unit => "Å";

    public Atom Atom1;
    public Atom Atom2;

    public Distance(Atom a1, Atom a2)
    {
        Atom1 = a1;
        Atom2 = a2;
    }
}
