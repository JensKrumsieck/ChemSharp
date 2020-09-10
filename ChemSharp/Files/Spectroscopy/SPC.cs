namespace ChemSharp.Files.Spectroscopy
{
    /// <summary>
    /// Bruker EMX EPR Spectrometer File Type
    /// </summary>
    public class SPC : DataBinaryFile<float>, IYSpectrumFile
    {
        public float[] YData { get; set; }

        public SPC(string path) : base(path) => YData = ConvertedData;

    }
}
