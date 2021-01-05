using ChemSharp.Extensions;
using ChemSharp.Files;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChemSharp.Spectroscopy.DataProviders
{
    /// <summary>
    /// class for reading all XY pairs in single csv file
    /// </summary>
    public class MultiCSVProvider
    {
        static MultiCSVProvider()
        {
            if (!FileHandler.RecipeDictionary.ContainsKey("csv"))
                FileHandler.RecipeDictionary.Add("csv", s => new PlainFile<string>(s));
        }

        /// <summary>
        /// csv delimiter
        /// </summary>
        internal char Delimiter;
        /// <summary>
        /// The file's path
        /// </summary>
        public string Path;

        /// <summary>
        /// List of Headers for columns (Item1 X, Item2 Y)
        /// </summary>
        public List<(string, string)> XYHeaders { get; set; } = new List<(string, string)>();

        /// <summary>
        /// List of XY DataPoint Lists
        /// </summary>
        public List<DataPoint[]> MultiXYData { get; set; } = new List<DataPoint[]>();

        /// <summary>
        /// Creates new MultiCSVProvider.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="delimiter">The CSV column delimiter</param>
        /// <exception cref="NotSupportedException">When File Length less or equal 3</exception>
        public MultiCSVProvider(string path, char delimiter = ',')
        {
            Path = path;
            Delimiter = delimiter;
            var file = (PlainFile<string>)FileHandler.Handle(path);
            if (file.Content.Length <= 3) throw new NotSupportedException("File Length to small");
            var headerPos = CheckHeaderPos(file.Content);
            GetData(file.Content, headerPos);
        }

        /// <summary>
        /// Gets data from lines
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="headerPos"></param>
        private void GetData(string[] lines, int headerPos)
        {
            var data = lines.Skip(headerPos + 1).ToArray();
            var dataLength = data[0].Split(Delimiter).Length;
            if (dataLength % 2 != 0) dataLength -= 1;
            var headers = lines.ToArray()[headerPos];
            for (var i = 0; i < dataLength; i += 2)
            {
                MultiXYData.Add(new DataPoint[data.Length]);
                var line = headers.Split(Delimiter);
                XYHeaders.Add((line[i], line[i + 1]));
            }
            for (var i = 0; i < data.Length; i++) //loop through lines
            {
                var columns = data[i].Split(Delimiter);
                for (var j = 0; j < dataLength - 1; j += 2) //loop through columns
                {
                    var x = columns[j];
                    var y = columns[j + 1];
                    if (string.IsNullOrEmpty(x) || string.IsNullOrEmpty(y)) continue;
                    var dp = new DataPoint(x.ToDouble(), y.ToDouble());
                    MultiXYData[j / 2][i] = dp;
                }
            }
        }

        /// <summary>
        /// Try to get header position
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        private int CheckHeaderPos(IReadOnlyList<string> lines)
        {
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                var split = line.Split(Delimiter);
                if (double.TryParse(split[0], out _)) return i - 1;
            }
            return 0;
        }
    }
}
