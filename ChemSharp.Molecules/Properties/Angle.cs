using System.Numerics;
using ChemSharp.Mathematics;

namespace ChemSharp.Molecules.Properties;

public class Angle : KeyValueProperty
{
    public override double Value => MathV.Angle(Atom1.Location, Atom2.Location, Atom3.Location);
    public override string Key => $"{Atom1.Title} - {Atom2.Title} - {Atom3.Title}";
    public override string Unit => "°";
    public Atom Atom1;
    public Atom Atom2;
    public Atom Atom3;

    public Angle(Atom a1, Atom a2, Atom a3)
    {
        Atom1 = a1;
        Atom2 = a2;
        Atom3 = a3;
    }

    /// <summary>
    /// Handles each set of atoms as plane and returns Angle
    /// </summary>
    /// <param name="angle2"></param>
    /// <returns></returns>
    public double PlaneAngle(Angle angle2)
    {
        var thisPlane = Plane.CreateFromVertices(Atom1.Location, Atom2.Location, Atom3.Location);
        var otherPlane = Plane.CreateFromVertices(angle2.Atom1.Location, angle2.Atom2.Location, angle2.Atom3.Location);
        return MathV.Angle(thisPlane, otherPlane);
    }
}
