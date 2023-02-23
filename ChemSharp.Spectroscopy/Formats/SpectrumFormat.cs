#if NETSTANDARD2_0
//for Dictionary Deconstruct
using ChemSharp.Extensions;
#endif
using ChemSharp.Spectroscopy.Extension;

namespace ChemSharp.Spectroscopy.Formats;

public abstract class SpectrumFormat
{
	protected Dictionary<string, Action<string>> NeededFiles;
	protected Func<string, string[], bool> ValidationMethod;

	protected void Load(string filename)
	{
		if (!ValidationMethod(filename, NeededFiles.Keys.ToArray()))
			throw new ArgumentException($"The file '{filename}' is not supported by {GetType().Name}");
		var baseFilename = FileExtensions.GetBaseFilename(filename);
		foreach (var (ext, func) in NeededFiles)
		{
			var fileExtension = ext;
			//some devices store extension in uppercase
			if (!File.Exists(baseFilename + ext)) fileExtension = ext.ToUpper();
			func(baseFilename + fileExtension);
		}
	}
}
