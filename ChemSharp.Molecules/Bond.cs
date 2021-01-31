using System.Collections.Generic;

namespace ChemSharp.Molecules
{
    public class Bond
    {
        public Atom Atom1;
        public Atom Atom2;

        /// <summary>
        /// Bond order, not supported by all file types
        /// </summary>
        public int Order = 1;

        /// <summary>
        /// Indicates whether bond is aromatic
        /// </summary>
        public bool IsAromatic = false;

        /// <summary>
        /// Gets Bond Length
        /// </summary>
        public float Length => Atom1.DistanceTo(Atom2);

        public Bond(Atom a1, Atom a2)
        {
            Atom1 = a1;
            Atom2 = a2;
        }

        public override string ToString() => $"{Atom1.Title} - {Atom2.Title} : {Length}";

        /// <summary>
        /// Returns Atoms as List
        /// to do LINQ
        /// </summary>
        public List<Atom> Atoms => new List<Atom> { Atom1, Atom2 };
    }
}
