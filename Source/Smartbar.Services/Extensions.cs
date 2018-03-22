﻿namespace JanHafner.Smartbar.Services
{
    using System;
    using System.Collections.Generic;

    internal static class Extensions
    {
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFactory)
        {
			if (!dictionary.TryGetValue(key, out TValue result))
			{
				result = valueFactory();
				dictionary.Add(key, result);
			}

			return result;
        }
    }
}
