namespace ChemSharp.Spectroscopy.Extension;

public static class FileExtensions
{
	public static bool ValidateWithExtensions(string filename, string[] extensions)
	{
		if (!extensions.Contains(Path.GetExtension(filename).ToLower())) return false;
		foreach (var ext in extensions)
		{
			var baseName = GetBaseFilename(filename);
			if (!File.Exists(baseName + ext.ToLower()) && !File.Exists(baseName + ext.ToUpper())) return false;
		}

		return true;
	}

	public static string GetBaseFilename(string filename) =>
		$"{Path.GetDirectoryName(filename)}\\{Path.GetFileNameWithoutExtension(filename)}";
}
