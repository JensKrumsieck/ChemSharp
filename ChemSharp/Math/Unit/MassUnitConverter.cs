namespace ChemSharp.Math.Unit
{
    public class MassUnitConverter : AbstractUnitConverter
    {
        public MassUnitConverter(string from, string to) : base(from, to)
        {
            AddConversion("kg", 1);
            AddConversion("g", 1e-3);
            AddConversion("mg", 1e-6);
            AddConversion("µg", 1e-9);
            AddConversion("t", 1000);
            AddConversion("lbs", 0.45359237);
            AddConversion("Solar Mass", 1.98847e30);
            AddConversion("Earth Mass", 5.97237e24);
            AddConversion("u", 1.660540e-27);
        }
    }
}
