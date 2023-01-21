using System;
using System.Collections.Generic;
using System.Linq;

namespace Sevens.Extension
{
    public static class EnumerableExtensions
    {
        private static readonly Random DefaultRandom = new Random();

        public static TValue RandomOne<TValue>(this IEnumerable<TValue> self)
        {
            return RandomOne(self, DefaultRandom);
        }

        public static TValue RandomOne<TValue>(this IEnumerable<TValue> self, Random random)
        {
            // ReSharper disable PossibleMultipleEnumeration
            return self.ElementAt(random.Next(self.Count()));
        }
    }
}