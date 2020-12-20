using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ChemSharp.Files
{
    /// <summary>
    /// Parameter Files are files using a key value storage to store data like the .par or jcamp files
    /// </summary>
    public class ParameterFile : PlainFile<string>
    {
        public IDictionary<string, string> Storage { get; set; }

        public string Delimiter;
        /// <summary>
        /// <inheritdoc cref="PlainFile{T}(string)"/>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="delimiter"></param>
        public ParameterFile(string path, string delimiter) : base(path)
        {
            Delimiter = delimiter;
            PropertyChanged += OnPropertyChanged;
        }

        /// <summary>
        ///<inheritdoc cref="PlainFile{T}()"/>
        /// </summary>
        /// <param name="delimiter"></param>
        public ParameterFile(string delimiter)
        {
            Delimiter = delimiter;
            PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //content changed = data is ready!
            if (e.PropertyName != nameof(Content)) return;
            Storage = ReadStorage().ToDictionary(s => s.Key, s => s.Value);
        }


        private IEnumerable<KeyValuePair<string, string>> ReadStorage() =>
            from line in Content
            select line.Split(Delimiter, 2, StringSplitOptions.RemoveEmptyEntries)
            into raw
            where raw.Length == 2
            select new KeyValuePair<string, string>(raw[0].Trim(),
                raw[1].Trim());
    }
}
