using System;
using System.IO;
using System.Reflection;

namespace ChemSharp.Extensions
{
    public static class Reflection
    {
        /// <summary>
        /// Creates Object from File if the specified type exists
        /// </summary>
        /// <param name="path"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static object CreateObjectFromFile(this string path, string folder)
        {
            var ext = Path.GetExtension(path);
            if (string.IsNullOrEmpty(ext)) ext = "." + Path.GetFileName(path);
            const string ns = "ChemSharp.Files";
            var type = Assembly.GetExecutingAssembly().GetType($"{ns}.{folder}{ext.ToUpper()}", false);
            return type == null ? null : Activator.CreateInstance(type, path);
        }
    }
}
