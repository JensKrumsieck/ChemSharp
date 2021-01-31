namespace ChemSharp
{
    public static class Constants
    {
        /// <summary>
        /// Planck h in Js
        /// </summary>
        public const double Planck = 6.62607015e-34; //in Js

        /// <summary> 
        /// reduced planck number in Js
        /// </summary>
        public const double ReducedPlanck = Planck / (2 * System.Math.PI);

        /// <summary>
        /// Speed of Light in m/s
        /// </summary>
        public const double SpeedOfLight = 299792458; //in m/s

        /// <summary>
        /// Elemental Charge in Coulomb
        /// </summary>
        public const double ElectronCharge = 1.602176634e-19;

        /// <summary>
        /// Mass of electron in kg
        /// </summary>
        public const double ElectronMass = 9.1093837015e-31;

        /// <summary>
        /// Boltzmann constant in J/K
        /// </summary>
        public const double Boltzmann = 1.38064e-23;

        /// <summary>
        /// Bohr magneton
        /// </summary>
        public const double BohrM = (ElectronCharge * Planck / (2 * System.Math.PI)) / (2 * ElectronMass);

        /// <summary>
        /// Epsilon_0 in As/Vm
        /// </summary>
        public const double Permittivity = 8.8541878128e-12; //in As/Vm

        /// <summary>
        /// Bohr radius
        /// </summary>
        public const double BohrRadius =
            (4 *
             System.Math.PI *
             Permittivity *
             (ReducedPlanck * ReducedPlanck) /
             (ElectronMass * (ElectronCharge * ElectronCharge)));

        /// <summary>
        /// Avogadro constant
        /// </summary>
        public const double Avogadro = 6.02214076e23;

        /// <summary>
        /// Universal Gas Constant in J/Kmol
        /// </summary>
        public const double GasConstant = Avogadro * Boltzmann;

        /// <summary>
        /// Faraday constant in C/mol
        /// </summary>
        public const double Faraday = Avogadro * ElectronCharge;

        /// <summary>
        /// Hartree constant
        /// </summary>
        public const double Hartree = (ReducedPlanck * ReducedPlanck) / (ElectronMass * (BohrRadius * BohrRadius));

        /// <summary>
        /// Free g factor for electron
        /// </summary>
        public const double GFree = 2.00231930436256;

        /// <summary>
        /// Angstroms to pixels factor
        /// </summary>
        public const float AngstromToPixels = 20;
    }
}
