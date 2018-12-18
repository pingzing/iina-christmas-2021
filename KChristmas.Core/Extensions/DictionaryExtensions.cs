using System;
using System.Collections.Generic;
using System.Linq;

namespace KChristmas.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static KeyValuePair<TKey, TValue> RandomEntry<TKey, TValue>(this IDictionary<TKey, TValue> dict)
        {
            Random rand = new Random();
            List<KeyValuePair<TKey, TValue>> list = dict.ToList();
            int size = list.Count;
            return list[rand.Next(size)];
        }
    }
}
