namespace ChemSharp.Extensions;

public static class FileUtil
{
	/// <summary>
	///     Get Extension as string
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static string GetExtension(string path)
	{
		var ext = Path.GetExtension(path);
		//fallback for nmr files
		ext = string.IsNullOrEmpty(ext) ? Path.GetFileName(path) : ext.Remove(0, 1);
		return ext.ToLower();
	}
}
