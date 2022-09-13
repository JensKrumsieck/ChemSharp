using ChemSharp.Molecules.Formats;
using FluentAssertions;
using Xunit;

namespace ChemSharp.Molecules.Tests.Formats;

public class Mol2FormatTests
{
	[Theory, InlineData("files/benzene.mol2", 12, 12), InlineData("files/myo.mol2", 1437, 1312),
	 InlineData("files/tep.mol2", 46, 50), InlineData("files/ptcor.mol2", 129, 127)]
	public void Mol2Format_CanReadPlausibleData(string file, int atomsCount, int bondsCount)
	{
		var mol = Mol2Format.Read(file);
		mol.Atoms.Count.Should().Be(atomsCount);
		mol.Atoms.Count(s => s.Symbol == "DA").Should().Be(0);
		mol.Bonds.Count.Should().Be(bondsCount);
	}

	[Fact]
	public void PDB_Mol2_Have_Same_ChainIds()
	{
		var pdb = PDBFormat.Read("files/0001.pdb");
		var mol2 = Mol2Format.Read("files/0001.mol2");
		mol2.Atoms.Should().HaveCount(pdb.Atoms.Count);
		var pdbGrouped = pdb.Atoms.GroupBy(a => a.ChainId);
		var mol2Grouped = mol2.Atoms.GroupBy(a => a.ChainId);
		pdbGrouped.Should().HaveCount(mol2Grouped.Count());
	}

	[Fact]
	public void Cif_Mol2_Have_Same_ChainIds()
	{
		var cif = CifFormat.Read("files/4n4n.cif");
		var mol2 = Mol2Format.Read("files/0001.mol2");
		mol2.Atoms.Should().HaveCount(cif.Atoms.Count);
		var cifGrouped = cif.Atoms.GroupBy(a => a.ChainId);
		var mol2Grouped = mol2.Atoms.GroupBy(a => a.ChainId);
		cifGrouped.Should().HaveCount(mol2Grouped.Count());
	}
}
