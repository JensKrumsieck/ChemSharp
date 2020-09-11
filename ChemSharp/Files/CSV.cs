using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ChemSharp.Files
{
    /// <summary>
    /// Generic CSV File Class, use ChemSharp.Files.Spectroscopy.CSV if T is float for
    /// being significantly faster
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CSV<T> : TextFile where T : IConvertible
    {
        public List<T[]> CsvTable;

        /// <summary>
        /// Separator char, default is comma
        /// </summary>
        public char Separator { get; }

        /// <summary>
        /// Creates new instance of CSV file with ";" as separator
        /// </summary>
        /// <param name="path"></param>
        public CSV(string path) : this(path, ',')
        { }

        /// <summary>
        /// Creates new instance of CSV file with given separator
        /// </summary>
        /// <param name="path"></param>
        /// <param name="separator"></param>
        public CSV(string path, char separator) : base(path)
        {
            Separator = separator;
            CsvTable = ReadData().ToList();
        }

        /// <summary>
        /// Reads in CSV as string array
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<T[]> ReadData()
        {
            if (typeof(T).IsPrimitive || typeof(T) == typeof(string))
            {
                foreach (var line in Data.Where(s => !string.IsNullOrWhiteSpace(s)))
                {
                    var data = line.Split(Separator)
                        .Select(element =>
                        {
                            try
                            {
                                return (T)Convert.ChangeType(element, typeof(T), CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                return default;
                            }
                        })
                        .ToArray();
                    //check if line length is not 0 and is not full of default values
                    if (data.Length != 0 && data.Count(s => s.Equals(default(T))) != data.Length) yield return data;
                }
            }
            else
                throw new InvalidDataException("Type not supported");
        }
    }
}
