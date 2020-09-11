using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace ChemSharp.Files.Spectroscopy
{
    public class CSV : CSV<float>, IXYSpectrumFile
    {
        private const string Pattern = @"(\d+((.|,)?\d+)+)";
        public CSV(string path) : this(path, ',') { }

        public CSV(string path, char separator) : base(path, separator)
        {
            CsvTable = ReadData().ToList();
            XYData = CsvTable.Select(s => new Vector2(s[0], s[1])).ToArray();
        }

        protected override IEnumerable<float[]> ReadData()
        {
            return Data
                .Select(line => line.Split(Separator)
                    .Where(s => !string.IsNullOrWhiteSpace(s) && Regex.IsMatch(s, Pattern))
                    .Select(s =>
            {
                float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var ret);
                return ret;
            }).ToArray()).Where(s => s.Length != 0);
        }

        public Vector2[] XYData { get; set; }
    }
}
