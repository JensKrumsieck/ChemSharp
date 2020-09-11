namespace ChemSharp.Math.Unit
{
    public class EnergyUnitConverter : AbstractUnitConverter
    {
        public EnergyUnitConverter(string from, string to) : base(from, to)
        {
            AddConversion("J", 1);
            AddConversion("eV", Constants.ElectronCharge);
            AddConversion("kJ/mol", 1000 / Constants.Avogadro);
            AddConversion("cal", 4.184);
            AddConversion("kcal/mol", 4184 / Constants.Avogadro);
            AddConversion("hartree", Constants.Hartree);
            AddConversion("cm^-1", Constants.Planck * Constants.SpeedOfLight * 100);
            AddConversion("Hz", Constants.Planck);
            AddConversion("Kelvin", Constants.Boltzmann);
            AddConversion("nm", new MappedFunction()
            {
                Function = i => Constants.Planck * Constants.SpeedOfLight / (i * 1e-9),
                Reciprocal = true
            });
        }
    }
}
