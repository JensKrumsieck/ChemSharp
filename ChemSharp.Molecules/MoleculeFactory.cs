using ChemSharp.Files;
using ChemSharp.Molecules.DataProviders;
using System;
using System.Collections.Generic;

namespace ChemSharp.Molecules
{
    public static class MoleculeFactory
    {
        /// <summary>
        /// Contains a file extension - type relationship
        /// </summary>
        public static Dictionary<string, Type> DataProviderDictionary = new();

        /// <summary>
        /// Import supported types for auto creation
        /// </summary>
        static MoleculeFactory()
        {
            DataProviderDictionary.Add("cif", typeof(CIFDataProvider));
            DataProviderDictionary.Add("xyz", typeof(XYZDataProvider));
            DataProviderDictionary.Add("mol2", typeof(Mol2DataProvider));
            DataProviderDictionary.Add("cdxml", typeof(CDXMLDataProvider));
        }

        /// <summary>
        /// Creates a Spectrum object with auto type detection
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Molecule Create(string path) => new((IAtomDataProvider)CreateProvider(path));

        /// <summary>
        /// Handles IDataProvider creation
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IAtomDataProvider CreateProvider(string path) =>
            FileHandler.CreateProvider<IAtomDataProvider>(path, DataProviderDictionary);
    }
}
