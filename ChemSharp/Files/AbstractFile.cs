namespace ChemSharp.Files
{
    public abstract class AbstractFile : IFile
    {
        public string Path { get; set; }
        public abstract void ReadFile();

        protected AbstractFile(string path)
        {
            Path = path;
            ReadFile();
        }
    }
}
