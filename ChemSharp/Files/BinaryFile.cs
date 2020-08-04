using System.IO;

namespace ChemSharp.Files
{
    public abstract class BinaryFile : AbstractFile
    {
        public byte[] Data { get; private set; }

        protected BinaryFile(string path) : base(path) { }

        public override void ReadFile()
        {
            if (File.Exists(Path)) Data = File.ReadAllBytes(Path);
            else throw new FileNotFoundException($"The specified file {Path} could not be found");
        }
    }
}
