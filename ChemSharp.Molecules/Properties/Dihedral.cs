using ChemSharp.Mathematics;

namespace ChemSharp.Molecules.Properties;

public class Dihedral : KeyValueProperty
{
    public override double Value => MathV.Dihedral(Atom1.Location, Atom2.Location, Atom3.Location, Atom4.Location);
    public override string Key => $"{Atom1.Title} - {Atom2.Title} - {Atom3.Title} - {Atom4.Title}";
    public override string Unit => "°";
    public Atom Atom1;
    public Atom Atom2;
    public Atom Atom3;
    public Atom Atom4;

    public Dihedral(Atom a1, Atom a2, Atom a3, Atom a4)
    {

        Atom1 = a1;
        Atom2 = a2;
        Atom3 = a3;
        Atom4 = a4;
    }
}
