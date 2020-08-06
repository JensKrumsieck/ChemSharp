using System.Numerics;

namespace ChemSharp.Molecule
{
    /// <summary>
    /// Represents an Atom : Element in the 3D Space
    /// </summary>
    public class Atom : Element
    {
        /// <summary>
        /// Location in 3D Space
        /// </summary>
        public Vector3 Location { get; set; }

        /// <inheritdoc cref="Element"/>
        public Atom(string symbol) : base(symbol)
        {

        }
    }
}
