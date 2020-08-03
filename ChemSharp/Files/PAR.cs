using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace ChemSharp.Files
{
    public class PAR : TextFile, IXSpectrumFile
    {
        public Dictionary<string, string> Parameters = new Dictionary<string, string>();

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
            //Generate X Axis Data
            var RES = int.Parse(Parameters["RES"], CultureInfo.InvariantCulture);
            var HCF = float.Parse(Parameters["HCF"], CultureInfo.InvariantCulture);
            var HSW = float.Parse(Parameters["HSW"], CultureInfo.InvariantCulture);
            var min = HCF - (HSW / 2);
            var d = HSW / (RES - 1);
            XData = new float[RES];
            for (int i = 0; i < RES; i++)
            {
                XData[i] = min + (d * i);
            }
        }

        public float[] XData { get; set; }
    }
}
