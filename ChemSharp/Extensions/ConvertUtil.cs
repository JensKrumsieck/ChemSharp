﻿using System;
using System.Globalization;
using System.Numerics;

namespace ChemSharp.Extensions
{
    public static class ConvertUtil
    {
        /// <summary>
        /// small parse extension as i like the ToXYZ() Syntax more ;)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static float ToFloat(this string? input) => input == null ? 0f : Convert.ToSingle(input, CultureInfo.InvariantCulture);

        /// <summary>
        /// small parse extension as i like the ToXYZ() Syntax more ;)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int ToInt(this string? input) => input == null ? 0 : Convert.ToInt32(input, CultureInfo.InvariantCulture);

        /// <summary>
        /// converts float array [3] to vector3
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Vector3 ToVector3(this float[] input) => new Vector3(input[0], input[1], input[2]);

        /// <summary>
        /// Converts three letter month to int
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int ToMonth(this string input)
        {
            return input switch
            {
                "Jan" => 1,
                "Feb" => 2,
                "Mar" => 3,
                "Apr" => 4,
                "May" => 5,
                "Jun" => 6,
                "Jul" => 7,
                "Aug" => 8,
                "Sep" => 9,
                "Oct" => 10,
                "Nov" => 11,
                "Dez" => 12,
                _ => 1
            };
        }
    }
}
