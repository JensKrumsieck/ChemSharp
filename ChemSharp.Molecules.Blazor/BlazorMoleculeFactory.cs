using ChemSharp.Files;
using Microsoft.AspNetCore.Components.Forms;

namespace ChemSharp.Molecules.Blazor;

public static class BlazorMoleculeFactory
{
    /// <summary>
    /// Create Molecule from IBrowserFile
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public async static Task<Molecule> CreateAsync(IBrowserFile file, long maxFileSize = 8192000L)
    {
        var extension = FileHandler.GetExtension(file.Name);
        var stream = file.OpenReadStream(maxFileSize);
        return await MoleculeFactory.CreateFromStreamAsync(stream, extension);
    }
}
