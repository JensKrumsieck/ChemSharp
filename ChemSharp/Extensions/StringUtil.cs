using System;
using System.Globalization;

namespace ChemSharp.Extensions
{
    public static class StringUtil
    {
        /// <summary>
        /// shorthand for splitting strings into line array
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string[] DefaultSplit(this string input) => input.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

        /// <summary>
        /// Converts String to Int in Invariant Culture
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int ToInt(this string input) => Convert.ToInt32(input, CultureInfo.InvariantCulture);

        /// <summary>
        /// Converts String to float in Invariant Culture
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static float ToSingle(this string input) => Convert.ToSingle(input, CultureInfo.InvariantCulture);

        /// <summary>
        /// Converts String to double in Invariant Culture
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double ToDouble(this string input) => Convert.ToDouble(input, CultureInfo.InvariantCulture);

    }
}
