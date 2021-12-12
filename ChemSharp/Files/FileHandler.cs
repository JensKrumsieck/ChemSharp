using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace ChemSharp.Files
{
    public static class FileHandler
    {
        /// <summary>
        /// Stores file handle recipes
        /// </summary>
        public static readonly Dictionary<string, Func<string, IFile>> RecipeDictionary = new();

        /// <summary>
        /// Handles file by given path
        /// </summary>
        /// <param name="filename"></param>
        /// <exception cref="FileLoadException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <returns></returns>
        public static IFile Handle(string filename)
        {
            if (!File.Exists(filename)) throw new FileNotFoundException($"The specified file {filename} could not be found");
            var ext = Path.GetExtension(filename);
            //fallback for nmr files
            ext = string.IsNullOrEmpty(ext) ? Path.GetFileName(filename) : ext.Remove(0, 1);
            ext = ext.ToLower();
            if (!RecipeDictionary.ContainsKey(ext)) throw new NotSupportedException("This file type is not supported, did you add your recipe?");
            var file = RecipeDictionary[ext](filename);
            if (file is null) throw new FileLoadException("Could not load file");
            //read any data
            file.ReadFile();

            return file;
        }

        /// <summary>
        /// Get Extension as string
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

        /// <summary>
        /// Handles IDataProvider creation
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dataProviderDictionary"></param>
        /// <returns></returns>
        public static T CreateProvider<T>(string path, Dictionary<string, Type> dataProviderDictionary)
        {
            var ext = FileHandler.GetExtension(path);
            //check if ext is supported
            if (!dataProviderDictionary.ContainsKey(ext)) throw new FileLoadException($"File type {ext} is not supported");
            var type = dataProviderDictionary[ext];
            var param = Expression.Parameter(typeof(string), "path");
            var creator = Expression
                .Lambda<Func<string, T>>(
                    Expression.New(
                        type.GetConstructor(new[] { typeof(string) }) ??
                        throw new InvalidOperationException("null given, Extension Handling failed"), param), param)
                .Compile();
            return creator(path);
        }
    }
}
