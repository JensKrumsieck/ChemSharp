using ChemSharp.Extensions;
using Microsoft.AspNetCore.Components.Forms;

namespace ChemSharp.Molecules.Blazor;

public static class BlazorMoleculeFactory
{
	/// <summary>
	///     Create Molecule from IBrowserFile
	/// </summary>
	/// <param name="file"></param>
	/// <param name="maxFileSize"></param>
	/// <returns></returns>
	public static Molecule Create(IBrowserFile file, long maxFileSize = 8192000L)
	{
		var extension = FileUtil.GetExtension(file.Name);
		var stream = file.OpenReadStream(maxFileSize);
		return Molecule.FromStream(stream, extension);
	}
}
