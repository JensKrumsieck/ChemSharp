using System;
using System.Linq;
using ChemSharp.Extensions;
using ChemSharp.Files.Spectroscopy;

namespace ChemSharp.Spectrum
{
    public class NMRSpectrum : AbstractSpectrum
    {
        /// <summary>
        /// Spectrum Nucleus
        /// </summary>
        public string Nucleus;
        /// <summary>
        /// Experiments Frequency
        /// </summary>
        public float Frequency;
        /// <summary>
        /// Date of measurement
        /// </summary>
        public DateTime Date;
        /// <summary>
        /// Holder number
        /// </summary>
        public int Holder;
        /// <summary>
        /// Instrument name
        /// </summary>
        public string Instrument;
        /// <summary>
        /// Solvent Name
        /// </summary>
        public string Solvent;

        public override void OnInit()
        {
            var file = (ACQUS)Files.FirstOrDefault(s => s.Path.Contains("acqus"));

            Frequency = file.Frequency;
            Nucleus = file.Type;

            var timestamp = file.Parameters.TryAndGet("$DATE");
            Date = new DateTime(1970,1,1,0,0,0);
            Date = Date.AddSeconds(timestamp.ToInt()).ToLocalTime();
            Holder = file.Parameters.TryAndGet("$HOLDER").ToInt();
            Instrument = file.Parameters.TryAndGet("$INSTRUM").BrukerRemove();
            Solvent = file.Parameters.TryAndGet("$SOLVENT").BrukerRemove();
        }
    }
}
