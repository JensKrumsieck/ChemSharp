using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ChemSharp.Molecules;
using ChemSharp.Molecules.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests.Molecules
{
    [TestClass]
    public class BondTests
    {
        [TestMethod]
        public void ABondHas2Atoms()
        {
            var atoms = new List<Atom>
            {
                new("He") {Location = new Vector3(1, 2, 3)},
                new("He") {Location = new Vector3(2, 2, 3)}
            };
            var bond = new Bond(atoms[0], atoms[1]);
            Assert.AreEqual(2, bond.Atoms.Length);
        }

        /// <summary>
        /// fixed an error where bonds vanish, when atoms are renamed...
        /// </summary>
        [TestMethod]
        public void BondDoNotVanishBug()
        {
            var atoms = new List<Atom>
            {
                new("He") {Location = new Vector3(1, 2, 3)},
                new("He") {Location = new Vector3(2, 2, 3)},
                new("He") {Location = new Vector3(2, 3, 3)},
                new("He") {Location = new Vector3(2, 2, 4)},
            };
            var bonds = new List<Bond>
            {
                new(atoms[0], atoms[1]),
                new(atoms[2], atoms[3]),
            };
            Assert.AreEqual(bonds.Count, 2);
            atoms[0].Title = "CHANGED!";
            Assert.AreEqual(bonds.Count, 2);
            atoms[1].Title = "CHANGED";
            Assert.AreEqual(bonds.Count, 2);
            foreach (var b in bonds) Assert.AreEqual(b.Atoms.Length, 2);
            foreach (var a in atoms) Assert.AreEqual(AtomUtil.Neighbors(a, bonds).Count(), 1);
        }
    }
}
