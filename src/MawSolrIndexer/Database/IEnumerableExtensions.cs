using System;
using System.Collections.Generic;
using System.Linq;

namespace MawSolrIndexer.Database
{
    public static class IEnumerableExtensions
    {
        public static R[] UniqueArray<T,R>(this IEnumerable<T> list, Func<T,R> selector)
        {
            return list
                .Select(selector)
                .Distinct()
                .ToArray();
        }
    }
}