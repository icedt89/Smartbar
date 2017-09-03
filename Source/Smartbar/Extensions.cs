namespace JanHafner.Smartbar
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal static class Extensions
    {
        public static async Task<T> FirstAsync<T>(this IEnumerable<T> source, Func<T, Task<Boolean>> predicate)
        {
            foreach (var item in source)
            {
                if (await predicate(item))
                {
                    return item;
                }
            }

            throw new InvalidOperationException();
        }
    }
}
