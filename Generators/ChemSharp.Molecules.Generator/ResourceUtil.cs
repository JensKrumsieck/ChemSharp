using System.Reflection;

namespace ChemSharp.Molecules.Generator;

public static class ResourceUtil
{
	/// <summary>
	///     Loads ResourceStream
	/// </summary>
	/// <param name="resourceName"></param>
	/// <returns></returns>
	public static Stream LoadResource(string resourceName) => Assembly.GetAssembly(typeof(ResourceUtil))
		?.GetManifestResourceStream(resourceName);

	/// <summary>
	///     Reads a String from Resource
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static string ReadResourceString(string path)
	{
		StreamReader sr;
		if (path.Contains("ChemSharp.Molecules.Generator.Resources")) //resource loading
		{
			var stream = LoadResource(path);
			sr = new StreamReader(stream);
		}
		else
			sr = new StreamReader(path);

		var data = sr.ReadToEnd();
		sr.Close();
		return data;
	}
}
