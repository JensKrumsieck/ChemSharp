using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChemSharp.Molecules.ElementalAnalysis;

/// <summary>
///     class for impurity handling in Elemental Analysis
/// </summary>
public class Impurity : INotifyPropertyChanged
{
	private string _formula;

	private double _lower;

	private double _step;

	private double _upper;

	/// <summary>
	///     Ctor
	/// </summary>
	/// <param name="formula"></param>
	/// <param name="lower"></param>
	/// <param name="upper"></param>
	/// <param name="step"></param>
	public Impurity(string formula, double lower, double upper, double step)
	{
		Formula = formula;
		Lower = lower;
		Upper = upper;
		Step = step;
	}

	/// <summary>
	///     Contains Sumformula as string
	/// </summary>
	public string Formula
	{
		get => _formula;
		set
		{
			_formula = value;
			OnPropertyChanged();
		}
	}

	/// <summary>
	///     Contains lower calculation bounds
	/// </summary>
	public double Lower
	{
		get => _lower;
		set
		{
			_lower = value;
			OnPropertyChanged();
		}
	}

	/// <summary>
	///     Contains upper calculation bounds
	/// </summary>
	public double Upper
	{
		get => _upper;
		set
		{
			_upper = value;
			OnPropertyChanged();
		}
	}

	/// <summary>
	///     Contains step for calculation
	/// </summary>
	public double Step
	{
		get => _step;
		set
		{
			_step = value;
			OnPropertyChanged();
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
