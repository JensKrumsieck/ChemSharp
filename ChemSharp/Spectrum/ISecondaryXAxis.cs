namespace ChemSharp.Spectrum
{
    public interface ISecondaryXAxis
    {
        float[] SecondaryXAxis { get; set; }

        float PrimaryToSecondary(float primaryValue);

        float SecondaryToPrimary(float secondaryValue);
    }
}
