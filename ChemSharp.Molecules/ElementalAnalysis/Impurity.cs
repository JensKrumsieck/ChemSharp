using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChemSharp.Molecules.ElementalAnalysis;

/// <summary>
/// class for impurity handling in Elemental Analysis
/// </summary>
public class Impurity : INotifyPropertyChanged
{
    private string _formula;
    /// <summary>
    /// Contains Sumformula as string
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

    private double _lower;
    /// <summary>
    /// Contains lower calculation bounds
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

    private double _upper;
    /// <summary>
    /// Contains upper calculation bounds
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

    private double _step;
    /// <summary>
    /// Contains step for calculation
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

    /// <summary>
    /// Ctor
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

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
