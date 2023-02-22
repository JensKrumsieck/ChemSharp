namespace ChemSharp.Spectroscopy.Extension;

public static class FileExtensions
{
	public static bool ValidateWithExtensions(string filename, string[] extensions) =>
		extensions.Contains(Path.GetExtension(filename)) &&
		//check if all files are available
		extensions.All(ext => File.Exists(GetBaseFilename(filename) + ext));

	public static string GetBaseFilename(string filename) =>
		$"{Path.GetDirectoryName(filename)}\\{Path.GetFileNameWithoutExtension(filename)}";
}
