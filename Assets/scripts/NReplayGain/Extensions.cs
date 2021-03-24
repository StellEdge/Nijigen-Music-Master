using System;
using System.Collections.Generic;
using System.Linq;

namespace NReplayGain
{
    public static class Extensions
    {
        public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            int index = 0;
            foreach (T item in source)
            {
                if (predicate(item))
                {
                    return index;
                }
                ++index;
            }
            return -1;
        }

        public static int Sum(this IEnumerable<int> source)
        {
            int total = 0;
            foreach (int item in source)
            {
                total += item;
            }
            return total;
        }
    }
}
