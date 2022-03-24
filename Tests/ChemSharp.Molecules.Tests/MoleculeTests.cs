﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ChemSharp.Molecules.Extensions;
using Xunit;

namespace ChemSharp.Molecules.Tests;

public class MoleculeTests
{
    [Theory]
    [ClassData(typeof(MoleculeTestDataGenerator))]
    public void Molecule_ShouldBeCreatedViaFactory(string path, int atoms, int bonds, string? formula, double? weight)
    {
        var mol = MoleculeFactory.Create(path);
        Assert.Equal(atoms, mol.Atoms.Count);
        Assert.Equal(bonds, mol.Bonds.Count);
        if(!string.IsNullOrEmpty(formula)) 
            Assert.Equal(formula, mol.Atoms.SumFormula());
        if(weight.HasValue) 
            Assert.Equal(weight.Value, mol.Atoms.MolecularWeight(), 1);
    }
}

public class MoleculeTestDataGenerator : IEnumerable<object[]>
{
    //creates a list with objects containing path, atoms count, bonds count
    //sum formula and molecular weight#
    //chemspider 2D files (.mol) do not add implicit hydrogens
    private List<object[]> _data = new()
    {
        new object[] {"files/cif.cif", 79, 89, "C40Cl2H29MoN4O3", 780.51},
        new object[] {"files/cif_noTrim.cif", 79, 89, "C40Cl2H29MoN4O3", 780.51},
        new object[] {"files/benzene.mol2", 12, 12, "C6H6", 78.11184},
        new object[] {"files/myo.mol2", 1437, 1312, null!, null!},
        new object[] {"files/2spl.pdb", 1437, 1314, null!, null!},
        new object[] {"files/1hv4.pdb", 9288, 9562, null!, null!},
        new object[] {"files/oriluy.pdb", 130, 151, null!, null!},
        new object[] {"files/mescho.xyz", 23, 23, "C10H12O", 148.205},
        new object[] {"files/tep.mol", 46, 50, "C28H14N4", null!},
        new object[] {"files/benzene.mol", 6, 6, "C6", 72.1},
        new object[] {"files/benzene_arom.mol", 12, 12, "C6H6", 78.11184},
        new object[] {"files/corrole.mol", 37, 41, null!, null!}
    };

    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}