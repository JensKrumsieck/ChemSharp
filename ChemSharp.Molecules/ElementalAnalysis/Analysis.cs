using ChemSharp.Molecules.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChemSharp.Molecules.ElementalAnalysis
{
    public sealed class Analysis
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
                TheoreticalAnalysis = Formula.ElementalAnalysis();
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
                UpdateDeviation();
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
                UpdateDeviation();
            }
        }
        /// <summary>
        /// Contains the deviation between experimental and theoretical data
        /// </summary>
        public Dictionary<string, double> Deviation { get; set; }

        /// <summary>
        /// contains Impurities 
        /// </summary>
        public List<Impurity> Impurities { get; set; } = new();

        /// <summary>
        /// ctor
        /// </summary>
        public Analysis() { }

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
        public static Analysis FromMolecule(Molecule mol) => new() { Formula = mol.Atoms.SumFormula() };

        /// <summary>
        /// Updates Deviation
        /// </summary>
        private void UpdateDeviation()
        {
            if (TheoreticalAnalysis != null && ExperimentalAnalysis != null)
                Deviation = ElementalAnalysisUtil.Deviation(TheoreticalAnalysis, ExperimentalAnalysis);
        }
    }
}
