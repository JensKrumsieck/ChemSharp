using System.IO;
using ChemSharp.Files;
using FluentAssertions;
using Xunit;

namespace ChemSharp.Molecules.Tests.Formats;

public class MoleculeFileFormatTests
{
	[Theory,
	 InlineData("files/cif.cif", 79, 89),
	 InlineData("files/cif_noTrim.cif", 79, 89),
	 InlineData("files/mmcif.cif", 1291, 1251),
	 InlineData("files/ligand.cif", 44, 46),
	 InlineData("files/147288.cif", 206, 230),
	 InlineData("files/4r21.cif", 6752, 6892),
	 InlineData("files/benzene.mol2", 12, 12), InlineData("files/myo.mol2", 1437, 1312),
	 InlineData("files/tep.mol2", 46, 50), InlineData("files/ptcor.mol2", 129, 127),
	 InlineData("files/benzene_3d.mol", 12, 12),
	 InlineData("files/benzene_arom.mol", 12, 12),
	 InlineData("files/benzene.mol", 6, 6), InlineData("files/oriluy.pdb", 130, 151),
	 InlineData("files/2spl.pdb", 1437, 1314), InlineData("files/1hv4.pdb", 9288, 9562)
	]
	[InlineData("files/cif.xyz", 102, 155)]
	[InlineData("files/mescho.xyz", 23, 23),
	 InlineData("files/1484829.cif", 466, 528)]
	public void Molecule_FromFile_ShouldBeValid(string filename, int atoms, int bonds)
	{
		var mol = Molecule.FromFile(filename);
		mol.Atoms.Count.Should().Be(atoms);
		mol.Bonds.Count.Should().Be(bonds);
	}

	[Theory,
	 InlineData("files/cif.cif", 79, 89),
	 InlineData("files/cif_noTrim.cif", 79, 89),
	 InlineData("files/mmcif.cif", 1291, 1251),
	 InlineData("files/ligand.cif", 44, 46),
	 InlineData("files/147288.cif", 206, 230),
	 InlineData("files/4r21.cif", 6752, 6892),
	 InlineData("files/benzene.mol2", 12, 12), InlineData("files/myo.mol2", 1437, 1312),
	 InlineData("files/tep.mol2", 46, 50), InlineData("files/ptcor.mol2", 129, 127),
	 InlineData("files/benzene_3d.mol", 12, 12),
	 InlineData("files/benzene_arom.mol", 12, 12),
	 InlineData("files/benzene.mol", 6, 6), InlineData("files/oriluy.pdb", 130, 151),
	 InlineData("files/2spl.pdb", 1437, 1314), InlineData("files/1hv4.pdb", 9288, 9562)
	]
	[InlineData("files/cif.xyz", 102, 155)]
	[InlineData("files/mescho.xyz", 23, 23),
	 InlineData("files/1484829.cif", 466, 528)]
	public void Molecule_FromStream_ShouldBeValid(string filename, int atoms, int bonds)
	{
		var stream = File.Open(filename, FileMode.Open);
		var ext = FileHandler.GetExtension(filename);
		var mol = Molecule.FromStream(stream, ext);
		mol.Atoms.Count.Should().Be(atoms);
		mol.Bonds.Count.Should().Be(bonds);
	}
}
