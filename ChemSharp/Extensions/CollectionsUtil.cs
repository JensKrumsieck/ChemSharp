using System.Collections.Generic;

namespace ChemSharp.Extensions
{
    public static class CollectionsUtil
    {
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
