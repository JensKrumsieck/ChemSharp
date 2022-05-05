namespace ChemSharp.Molecules.ElementalAnalysis;

public struct Impurity
{
	public string Formula;

	public double Lower;

	public double Step;

	public double Upper;

	public Impurity(string formula, double lower, double upper, double step)
	{
		Formula = formula;
		Lower = lower;
		Upper = upper;
		Step = step;
	}
}
