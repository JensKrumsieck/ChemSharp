using System.Collections.Generic;

namespace ChemSharp.Extensions
{
    public static class CollectionsUtil
    {
        public static TValue TryAndGet<TKey, TValue>(this Dictionary<TKey, TValue> input, TKey key) =>
            input.ContainsKey(key) ? input[key] : default(TValue);
    }
}
