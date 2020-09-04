using ChemSharp.Extensions;

namespace ChemSharp.Files.Spectroscopy
{
    /// <summary>
    /// Helping ACQUS to get processed information
    /// </summary>
    public class PROCS : JCampFile
    {
        public float Offset;
        public int FTSize;
        public float[] PhaseCorrection;

        public PROCS(string path) : base(path)
        {
            Init();
        }

        private void Init()
        {
            Offset = Parameters.TryAndGet("$OFFSET").ToFloat();
            FTSize = Parameters.TryAndGet("$FTSIZE").ToInt();
            PhaseCorrection = new[]
            {
                Parameters.TryAndGet("$PHC0").ToFloat(),
                Parameters.TryAndGet("$PCH1").ToFloat()
            };
        }
    }
}
