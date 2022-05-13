using System.Collections.Generic;
using ChemSharp.Molecules.ElementalAnalysis;
using ChemSharp.Molecules.Extensions;
using ChemSharp.Molecules.Formats;
using FluentAssertions;
using Xunit;

namespace ChemSharp.Molecules.Tests.Extensions;

public class ElementalAnalysisTests
{
	[Fact]
	public void ElementalAnalysis_ShouldBeValidFromFile()
	{
		const string file = "files/cif.cif";
		var mol = CifFormat.Read(file);
		var formula = mol.Atoms.SumFormula();
		var chn = formula.ElementalAnalysis();
		var exp = new Dictionary<string, double> {{"C", 61.55}, {"H", 3.745}, {"N", 7.178}};
		exp.Should().BeSubsetOf(chn);
	}

	[Fact]
	public void Deviation_ShouldBeValidFromFile()
	{
		const string file = "files/cif.cif";
		var mol = CifFormat.Read(file);
		var formula = mol.Atoms.SumFormula();
		var chn = formula.ElementalAnalysis();
		var exp = new Dictionary<string, double> {{"C", 60.55}, {"H", 3.145}, {"N", 1.178}};
		var dev = ElementalAnalysisUtil.Deviation(chn, exp);
		var expected = new Dictionary<string, double> {{"C", 1.00}, {"H", .60}, {"N", 6.00}};
		dev.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public void Deviation_ShouldBeValidWithImpurities()
	{
		const string file = "files/cif.cif";
		var mol = CifFormat.Read(file);
		var formula = mol.Atoms.SumFormula();
		var chn = formula.ElementalAnalysis();
		var exp = new Dictionary<string, double> {{"C", 60.55}, {"H", 2.7}, {"N", 7.7}};
		var dev = ElementalAnalysisUtil.Deviation(chn, exp);
		var expected = new Dictionary<string, double> {{"C", 1.00}, {"H", .60}, {"N", 6.00}};
		var dcm = new Impurity("CH2Cl2", 0, 1, 0.1);
		var hexane = new Impurity("C6H14", 0, 1, 0.1);
		var water = new Impurity("H2O", 0, 1, 0.1);
		var best = ElementalAnalysisUtil.Solve(formula, exp, new HashSet<Impurity> {dcm, hexane, water});
		//should be 0.2
		best[0].Should().Be(.2);
	}

	[Fact]
	public void Everything_ShouldWorkWithStringsOnly()
	{
		const string formula = "C52H40ClN5O4Zn"; //Zinc isoporphyrin
		var dcm = new Impurity("CH2Cl2", 0, 1, .1);
		var hexane = new Impurity("C6H14", 0, 1, .1);
		var experimental = new Dictionary<string, double> {{"C", 68.12}, {"H", 4.77}, {"N", 6.71}};
		var best = ElementalAnalysisUtil.Solve(formula, experimental, new[] {dcm, hexane});
		best[0].Should().BeApproximately(.5, 1e-7);
		best[1].Should().BeApproximately(.7, 1e-7);
	}

	[Fact]
	public void Error_ShouldBeLessFor0_5DCM()
	{
		const string formula = "C70H55ClF6N6Zn";
		var dcm = new Impurity("CH2Cl2", 0, 1, .1);
		var exp = new Dictionary<string, double> {{"C", 68.41}, {"H", 4.841}, {"N", 6.78}};
		var best = ElementalAnalysisUtil.Solve(formula, exp, new[] {dcm});
		best[0].Should().BeApproximately(.5, 1e-7);
	}
}
