namespace ChemSharp.Spectroscopy.Extensions;

public static class FileExtensions
{
	internal static string[] BrukerNMRFiles = {"fid", "1r", "1i", "acqus", "procs"};
	internal static string[] BrukerEPRFiles = {"par", "spc"};
	internal static string[] VarianUVVisFiles = {"dsw"};
	internal static string[] CSVFiles = {"csv"};

	public static bool ValidateWithExtensions(string filename, IEnumerable<string> extensions)
	{
		if (!extensions.Contains(Path.GetExtension(filename).ToLower())) return false;
		var baseName = GetBaseFilename(filename);
		return extensions.All(ext => File.Exists(baseName + ext.ToLower()) || File.Exists(baseName + ext.ToUpper()));
	}

	public static bool ValidateBrukerNMR(string filename, IEnumerable<string> filenames)
	{
		var baseName = GetBrukerStyleBaseName(filename);
		return filenames.Where(file => !file.Contains("pdata")).All(file => File.Exists(baseName + file));
	}

	public static string GetBaseFilename(string filename) => Path.GetExtension(filename) != ""
		? $"{Path.GetDirectoryName(filename)}\\{Path.GetFileNameWithoutExtension(filename)}"
		: GetBrukerStyleBaseName(filename);

	private static string GetBrukerStyleBaseName(string filename)
	{
		var baseName = Path.GetDirectoryName(filename);
		if (baseName.Contains("pdata")) baseName = Path.GetFullPath(Path.Combine(baseName, @"..\..\"));
		return baseName + @"\";
	}

	public static Dictionary<string, string> ReadStorageFile(string file, char delimiter)
	{
		var content = File.ReadAllLines(file);
		var storage = (from line in content
		               select line.Split(new[] {delimiter}, 2, StringSplitOptions.RemoveEmptyEntries)
		               into raw
		               where raw.Length == 2
		               select new KeyValuePair<string, string>(raw[0].Trim(),
		                                                       raw[1].Trim()))
			.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		return storage;
	}
}
