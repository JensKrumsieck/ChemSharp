using ChemSharp.Molecules.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ChemSharp.Molecules.ElementalAnalysis
{
    public class Analysis : INotifyPropertyChanged
    {
        private string _formula;
        /// <summary>
        /// Represents the Sum formula
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

        private Dictionary<string, double> _theoreticalAnalysis;
        /// <summary>
        /// Contains the calculated theoretical elemental analysis
        /// </summary>

        public Dictionary<string, double> TheoreticalAnalysis
        {
            get => _theoreticalAnalysis;
            set
            {
                _theoreticalAnalysis = value;
                OnPropertyChanged();
            }
        }

        private Dictionary<string, double> _experimentalAnalysis;
        /// <summary>
        /// Contains the Experimental Data
        /// </summary>
        public Dictionary<string, double> ExperimentalAnalysis
        {
            get => _experimentalAnalysis;
            set
            {
                _experimentalAnalysis = value;
                OnPropertyChanged();
            }
        }

        private Dictionary<string, double> _deviation;
        /// <summary>
        /// Contains the deviation between experimental and theoretical data
        /// </summary>
        public Dictionary<string, double> Deviation
        {
            get => _deviation;
            set
            {
                _deviation = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// contains Impurities
        /// </summary>
        public ObservableCollection<Impurity> Impurities { get; set; } = new ObservableCollection<Impurity>();

        /// <summary>
        /// ctor
        /// </summary>
        public Analysis()
        {
            PropertyChanged += OnPropertyChanged;
        }

        /// <summary>
        /// Pass-through method to <see cref="ElementalAnalysisUtil.Solve"/>
        /// </summary>
        /// <returns></returns>
        public async Task<double[]> Solve() =>
            await Task.Run(() => ElementalAnalysisUtil.Solve(Formula, ExperimentalAnalysis, Impurities));

        /// <summary>
        /// returns new Instance from Molecule
        /// </summary>
        /// <param name="mol"></param>
        /// <returns></returns>
        public static Analysis FromMolecule(Molecule mol) => new Analysis() { Formula = mol.Atoms.SumFormula() };

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Formula):
                    TheoreticalAnalysis = Formula.ElementalAnalysis();
                    break;
                case nameof(ExperimentalAnalysis):
                case nameof(TheoreticalAnalysis):
                    if(TheoreticalAnalysis != null && ExperimentalAnalysis != null) 
                        Deviation = ElementalAnalysisUtil.Deviation(TheoreticalAnalysis, ExperimentalAnalysis);
                    break;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
