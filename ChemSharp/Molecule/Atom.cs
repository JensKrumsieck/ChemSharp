using System.Numerics;

namespace ChemSharp.Molecule
{
    public class Atom : Element
    {
        public Vector3 Location { get; set; }

        public Atom(string symbol) : base(symbol)
        {

        }
    }
}
