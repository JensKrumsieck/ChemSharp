using ChemSharp.Extensions;
using ChemSharp.Files.Spectroscopy;
using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Calls directly after initialization
        /// </summary>
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

            //set secondary X Axis to G Axis
            SecondaryXAxis = GAxis.ToArray();
        }

        /// <summary>
        /// Calculate GValue for one given point.
        /// </summary>
        /// <param name="xInput"></param>
        /// <param name="frequency"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static float GValue(float xInput, float frequency, string unit = "G")
        {
            //convert input to Tesla unit
            var value = unit switch
            {
                "G" => xInput / 10000,
                "mT" => xInput / 1000,
                _ => xInput
            };

            return (float) (Constants.Planck * frequency * 1e9 / (Constants.BohrM * value));
        }


        public IEnumerable<float> GAxis
        {
            get
            {
                for (var i = 0; i < Data.Length; i++) yield return GValue(Data[i].X, Frequency, !string.IsNullOrEmpty(Unit) ? Unit : "G");
            }
        }
    }
}
