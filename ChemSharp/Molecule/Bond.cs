namespace ChemSharp.Molecule
{
    public class Bond
    {
        public Atom Atom1;
        public Atom Atom2;

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
    }
}
