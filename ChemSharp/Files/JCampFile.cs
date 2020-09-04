using System.Collections.Generic;
using System.Linq;

namespace ChemSharp.Files
{
    public class JCampFile : TextFile
    {
        public Dictionary<string, string> Parameters;
        public JCampFile(string path) : base(path)
        {
            Parameters = ReadData().ToDictionary(s => s.Key, s => s.Value);
        }

        /// <summary>
        /// Reads in Data from JCamp Format
        /// JCAMP Data format saves keys with two leading #-signs, raw value follows after =-sign.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<KeyValuePair<string, string>> ReadData() => 
            from line in Data 
            select line.Split("=")
            into raw 
            where raw.Length == 2 
            let key = raw[0].Remove(0, 2) 
            let value = raw[1].Trim() 
            select new KeyValuePair<string, string>(key, value);
    }
}
