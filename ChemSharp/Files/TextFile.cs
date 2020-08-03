using System.IO;

namespace ChemSharp.Files
{
    public abstract class TextFile : AbstractFile
    {

        public string[] Data { get; private set; }

        protected TextFile(string path) : base (path){}
            
        public override void ReadFile()
        {
            if (File.Exists(Path)) Data = File.ReadAllLines(Path);
            else throw new FileNotFoundException($"The specified file {Path} could not be found");
        }
    }
}
