using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ChemSharp.Files.Molecule;

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
                bonds = ((IBondFile) input).Bonds;
            return new Molecule(input.Atoms, bonds);
        }

        /// <summary>
        /// Creates Molecule from Path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Molecule Create(string path)
        {
            var ext = Path.GetExtension(path);
            const string ns = "ChemSharp.Files.Molecule";
            var type = Assembly.GetExecutingAssembly().GetType(ns + ext.ToUpper(), true);
            var file = Activator.CreateInstance(type, path);
            return Create(file as IAtomFile);
        }
    }
}
