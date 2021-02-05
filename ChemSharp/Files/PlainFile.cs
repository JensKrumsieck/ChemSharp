using ChemSharp.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ChemSharp.Files
{
    /// <summary>
    /// Note: T can be either numeric or string!
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PlainFile<T> : IFile where T : IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        private string _path;
        /// <summary>
        /// <inheritdoc cref="IFile.Path"/>
        /// </summary>
        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                ReadBytes();
            }
        }

        private T[] _content;
        /// <summary>
        /// The File's RAW content
        /// </summary>
        public T[] Content
        {
            get => _content;
            set
            {
                _content = value;
                ContentChanged();
            }
        }

        /// <summary>
        /// You can specify a EntryByte for binary files
        /// </summary>
        public int ByteOffset = 0x0;

        /// <summary>
        /// Length of Byte Data ca be specified
        /// </summary>
        public int CutOffLength = 0;

        /// <summary>
        /// File content as Bytearray
        /// </summary>
        public byte[] Bytes { get; private set; }

        /// <summary>
        /// Reads the file's content
        /// </summary>
        /// <exception cref="FileLoadException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public void ReadFile()
        {
            if (Bytes is null) ReadBytes();
            if (Bytes.Length == 0) throw new FileLoadException($@"Could not load File {Path}");

            if (typeof(T).IsNumeric())
            {
                if (ByteOffset != 0)
                    Bytes = new ArraySegment<byte>(Bytes, ByteOffset,
                        CutOffLength != 0 ? CutOffLength : Bytes.Length - ByteOffset).ToArray();

                Content = new T[Bytes.Length / Marshal.SizeOf<T>()];
                Buffer.BlockCopy(Bytes, 0, Content, 0, Bytes.Length);
            }
            //we checked Type before, so it's ok to cast like this!
            else if (typeof(T) == typeof(string)) Content = Encoding.UTF8.GetString(Bytes).DefaultSplit() as T[];
            else throw new NotSupportedException($"The given Type {typeof(T).FullName} is not supported");
        }

        /// <summary>
        /// ctor with path
        /// </summary>
        /// <param name="path"></param>
        public PlainFile(string path) : this() => Path = path;

        /// <summary>
        /// ctor without arguments
        /// </summary>
        public PlainFile() { }

        /// <summary>
        /// Reads file as bytes, used to read in file at path change
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        internal void ReadBytes()
        {
            if (!File.Exists(Path)) throw new FileNotFoundException($"The specified File {Path} could not be found!");
            Bytes = File.ReadAllBytes(Path);
        }

        /// <summary>
        /// Fires when content changes
        /// </summary>
        protected virtual void ContentChanged() { }
    }
}
