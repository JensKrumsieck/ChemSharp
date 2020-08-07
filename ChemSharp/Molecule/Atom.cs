using System.Numerics;

namespace ChemSharp.Molecule
{
    /// <summary>
    /// Represents an Atom : Element in the 3D Space
    /// </summary>
    public class Atom : Element
    {
        /// <summary>
        /// See BondTo Method
        /// </summary>
        static readonly float Delta = 0.25f;

        /// <summary>
        /// Location in 3D Space
        /// </summary>
        public Vector3 Location { get; set; }

        /// <inheritdoc cref="Element"/>
        public Atom(string symbol) : base(symbol)
        {

        }

        /// <summary>
        /// Computes Distance To Other Atom
        /// Wrapper fro Vector3.Distance
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public float DistanceTo(Atom test) => Vector3.Distance(this.Location, test.Location);

        public bool BondTo(Atom test) => DistanceTo(test) < (this.VdWRadius);
    }
}
