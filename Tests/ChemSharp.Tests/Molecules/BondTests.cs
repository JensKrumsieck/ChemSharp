using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ChemSharp.Molecules;
using ChemSharp.Molecules.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChemSharp.Tests.Molecules;

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

    [TestMethod]
    public void AtomWithNoNeighborsReturnsEmpty()
    {
        var atom = new Atom("Nb");
        var bonds = new List<Bond>();
        var n = AtomUtil.Neighbors(atom, bonds);
        Assert.IsNotNull(n);
        Assert.AreEqual(n.Count(), 0);
    }

    [TestMethod]
    public void AtomWithJustMetalNeighborsReturnsEmpty()
    {
        var atoms = new List<Atom>
        {
            new("He") {Location = new Vector3(1, 2, 3)},
            new("Ni") {Location = new Vector3(2, 2, 3)},
            new("Co") {Location = new Vector3(2, 3, 3)},
            new("Fe") {Location = new Vector3(2, 2, 4)},
        };
        var bonds = new List<Bond>
        {
            new(atoms[0], atoms[1]),
            new(atoms[2], atoms[3]),
        };
        var mol = new Molecule(atoms, bonds);
        var n = mol.NonMetalNeighbors(atoms[0]);
        Assert.IsNotNull(n);
        Assert.AreEqual(n.Count(), 0);
    }

    [TestMethod]
    public void AtomWithNoNeighborsDoesNotFail()
    {
        var atom = new Atom("Nb");
        var bonds = new List<Bond>();
        var mol = new Molecule(new[] { atom }, bonds);
        var n = mol.NonMetalNeighbors(atom);
        Assert.IsNotNull(n);
        Assert.AreEqual(n.Count(), 0);
    }

    [TestMethod]
    public void PtCorDoesNotFail()
    {
        //Abraham B. Alemayehu, Hugo Vazquez-Lima, Christine M. Beavers, Kevin J. Gagnon, Jesper Bendix, Abhik Ghosh,
        //Chemical Communications, 2014, 50, 11093,
        //DOI: 10.1039/C4CC02548B
        const string file = "files/ptcor.mol2";

        var mol = MoleculeFactory.Create(file);
        //remember caching is on!
        foreach (var a in mol.Atoms)
        {
            var noMNeighbors = mol.NonMetalNeighbors(a);
            Assert.IsNotNull(noMNeighbors);
            var neighbors = mol.Neighbors(a);
            Assert.IsNotNull(neighbors);
            Assert.AreEqual(neighbors.Count, AtomUtil.Neighbors(a, mol.Bonds).Count());
        }
    }
}
