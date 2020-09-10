using ChemSharp.Extensions;
using ChemSharp.Files.Molecule;
using System.Collections.Generic;
using System.Linq;

namespace ChemSharp.Molecule
{
    public static class MoleculeFactory
    {
        /// <summary>
        /// creates Molecule from File
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Molecule Create(IAtomFile input)
        {
            IEnumerable<Bond> bonds = null;
            if (input.GetType().GetInterfaces().Contains(typeof(IBondFile))) //has bond information
                bonds = ((IBondFile)input).Bonds;
            return new Molecule(input.Atoms, bonds);
        }

        /// <summary>
        /// Creates Molecule from Path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Molecule Create(string path) => Create(path.CreateObjectFromFile("Molecule") as IAtomFile);
    }
}
