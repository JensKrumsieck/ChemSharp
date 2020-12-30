using System;
using System.Collections.Generic;
using System.IO;

namespace ChemSharp.Files
{
    public static class FileHandler
    {
        /// <summary>
        /// Stores file handle recipes
        /// </summary>
        public static Dictionary<string, Func<string, IFile>> RecipeDictionary = new Dictionary<string, Func<string, IFile>>();

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
    }
}
