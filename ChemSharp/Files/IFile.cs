namespace ChemSharp.Files;

public interface IFile
{
    /// <summary>
    /// Filepath of current file
    /// </summary>
    string Path { get; set; }

    /// <summary>
    /// Reads file content and saves it.
    /// </summary>
    void ReadFile();
}
