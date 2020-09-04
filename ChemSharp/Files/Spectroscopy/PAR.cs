using ChemSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ChemSharp.Files.Spectroscopy
{
    /// <summary>
    /// PARameter file from Bruker EMX EPR Spectrometer
    /// </summary>
    public class PAR : TextFile, IXSpectrumFile
    {
        /// <summary>
        /// Parameter Dictionary
        /// </summary>
        public Dictionary<string, string> Parameters = new Dictionary<string, string>();
        public float[] XData { get; set; }

        public PAR(string path) : base(path)
        {
            foreach (var line in Data)
            {
                try
                {
                    Parameters.Add(line.Substring(0, 3).Trim(), line.Substring(3).Trim());
                }
                catch (Exception e)
                {
#if DEBUG
                    Debug.WriteLine(e);
#endif 
                }

            }
            //Generate X Axis Data from Parameters
            var RES = Parameters["RES"].ToInt();
            var HCF = Parameters["HCF"].ToFloat();
            var HSW = Parameters["HSW"].ToFloat();
            var min = HCF - (HSW / 2);
            var d = HSW / (RES - 1);
            XData = new float[RES];
            for (int i = 0; i < RES; i++)
            {
                XData[i] = min + (d * i);
            }
        }

    }
}
