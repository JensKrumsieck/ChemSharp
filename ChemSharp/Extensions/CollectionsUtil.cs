﻿using System.Collections.Generic;
using System.Linq;

namespace ChemSharp.Extensions
{
    public static class CollectionsUtil
    {
        /// <summary>
        /// Equivalent to linspace in python
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<double> LinearRange(double from, double to, int count)
        {
            var step = (to - from) / (count - 1.0f);
            return Enumerable.Range(0, count).Select(s => (double)s * step + from);
        }

        /// <summary>
        /// shorthand method for try and get dictionary value
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue TryAndGet<TKey, TValue>(this Dictionary<TKey, TValue> input, TKey key) =>
            input.ContainsKey(key) ? input[key] : default;
    }
}
