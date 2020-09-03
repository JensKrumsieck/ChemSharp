using System;
using System.Linq;

namespace ChemSharp.Extensions
{
    public static class StringUtil
    {
        /// <summary>
        /// shorthand for splitting strings into line array
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string[] LineSplit(this string input) => input.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.None);

        /// <summary>
        /// Remove brackets from number string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StripUncertainity(this string input) => input.Split('(').First();

        public static string[] WhiteSpaceSplit(this string input) => input
            .Split(new[] {" ", "\t"}, StringSplitOptions.None).Where(s => !string.IsNullOrEmpty(s)).ToArray();
    }
}
