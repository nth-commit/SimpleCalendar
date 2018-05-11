using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq
{
    static class EnumerableExtensions
    {
        public static (IEnumerable<T> matches, IEnumerable<T> nonMatches) Fork<T>(
            this IEnumerable<T> source,
            Func<T, bool> pred)
        {
            var groupedByMatching = source.ToLookup(pred);
            return (groupedByMatching[true], groupedByMatching[false]);
        }
    }
}
