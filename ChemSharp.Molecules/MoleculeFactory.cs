﻿using ChemSharp.Files;
using ChemSharp.Molecules.DataProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ChemSharp.Molecules
{
    public static class MoleculeFactory
    {
        /// <summary>
        /// Contains a file extension - type relationship
        /// </summary>
        public static readonly Dictionary<string, Type> DataProviderDictionary = new();

        /// <summary>
        /// Import supported types for auto creation
        /// </summary>
        static MoleculeFactory()
        {
            DataProviderDictionary.Add("cif", typeof(CIFDataProvider));
            DataProviderDictionary.Add("xyz", typeof(XYZDataProvider));
            DataProviderDictionary.Add("mol2", typeof(Mol2DataProvider));
            DataProviderDictionary.Add("pdb", typeof(PDBDataProvider));
            DataProviderDictionary.Add("cdxml", typeof(CDXMLDataProvider));
        }

        /// <summary>
        /// Creates a Spectrum object with auto type detection
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Molecule Create(string path) => new(CreateProvider(path));

        /// <summary>
        /// Handles IDataProvider creation
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IAtomDataProvider CreateProvider(string path) =>
            FileHandler.CreateProvider<IAtomDataProvider>(path, DataProviderDictionary);

        /// <summary>
        /// Creates Molecule from Stream Asynchronous.
        /// Format needs to be given
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ext">File Format extension without a dot</param>
        /// <returns></returns>
        public static async Task<Molecule> CreateFromStreamAsync(Stream data, string ext)
        {
            using var sr = new StreamReader(data);
            var provider = (AbstractAtomDataProvider)FormatterServices.GetSafeUninitializedObject(DataProviderDictionary[ext]);
            provider.Content = (await sr.ReadToEndAsync()).Split(Environment.NewLine.ToCharArray());
            provider.ReadData();
            return new Molecule(provider);
        }
    }
}
