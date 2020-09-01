using System;
using System.Linq;
using ChemSharp.Extensions;
using ChemSharp.Files.Spectroscopy;

namespace ChemSharp.Spectrum
{
    public class EPRSpectrum : AbstractSpectrum
    {
        /// <summary>
        /// EPR Frequency
        /// </summary>
        public float Frequency;

        /// <summary>
        /// Modulation amplitude
        /// </summary>
        public float ModAmp;

        /// <summary>
        /// ConversionTime
        /// </summary>
        public float ConversionTime;

        /// <summary>
        /// Time Conversion
        /// </summary>
        public float TimeConversion;

        /// <summary>
        /// Microwave Power
        /// </summary>
        public float Power;

        /// <summary>
        /// Receiver gain
        /// </summary>
        public float ReceiverGain;

        /// <summary>
        /// Number of scans
        /// </summary>
        public int Scans;

        /// <summary>
        /// Time Spectrum has been recorded
        /// </summary>
        public DateTime Time;

        /// <summary>
        /// Given Operator Name
        /// </summary>
        public string Operator;

        /// <summary>
        /// Given Comment
        /// </summary>
        public string Comment;

        /// <summary>
        /// The frequency unit
        /// </summary>
        public string Unit;

        public override void OnInit()
        {
            base.OnInit();

            var file = (PAR)Files.FirstOrDefault(s => s.Path.Contains(".par"));

            if (file == null) return;
            //there's no automated way...
            Frequency = file.Parameters.TryAndGet("MF").ToFloat();
            ModAmp = file.Parameters.TryAndGet("RMA").ToFloat();
            ConversionTime = file.Parameters.TryAndGet("RCT").ToFloat();
            TimeConversion = file.Parameters.TryAndGet("RTC").ToFloat();
            Power = file.Parameters.TryAndGet("MP").ToFloat();
            ReceiverGain = file.Parameters.TryAndGet("RRG").ToFloat();
            Scans = file.Parameters.TryAndGet("JNS").ToInt();
            var datestamp = file.Parameters.TryAndGet("JDA");
            var parts = datestamp.Split(".");
            var timestamp = file.Parameters.TryAndGet("JTM");
            var time = timestamp.Split(":");
            Time = new DateTime(parts[2].ToInt(), parts[1].ToMonth(), parts[0].ToInt(), time[0].ToInt(),
                time[1].ToInt(), 0);
            Operator = file.Parameters.TryAndGet("JON");
            Comment = file.Parameters.TryAndGet("JCO");
            Unit = file.Parameters.TryAndGet("JUN");
        }
    }
}
