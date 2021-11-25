using ChemSharp.Files;
using ChemSharp.Molecules.DataProviders;
using Microsoft.AspNetCore.Components.Forms;
using System.Runtime.Serialization;

namespace ChemSharp.Molecules.Blazor
{
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
            var provider = (AbstractAtomDataProvider)FormatterServices.GetSafeUninitializedObject(MoleculeFactory.DataProviderDictionary[extension]);
            var stream = file.OpenReadStream(maxFileSize);
            using var sr = new StreamReader(stream);
            provider.Content = (await sr.ReadToEndAsync()).Split("\n");
            provider.ReadData();
            return new Molecule(provider);
        }
    }
}
