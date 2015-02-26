using System;
using System.Collections.Generic;
using System.Linq;

namespace Humanizer.Localisation.CollectionFormatters
{
    internal class BrazilianPortugueseCollectionFormatter : DefaultCollectionFormatter
    {
        public BrazilianPortugueseCollectionFormatter()
        {
            DefaultSeparator = "e";
        }

        public override string Humanize<T>(IEnumerable<T> collection, Func<T, String> objectFormatter, String separator)
        {
            if (collection == null)
                throw new ArgumentException("collection");

            var enumerable = collection as T[] ?? collection.ToArray();

            int count = enumerable.Count();

            if (count == 0)
                return "";

            if (count == 1)
                return objectFormatter(enumerable.First());

            return String.Format("{0} {1} {2}",
                                 String.Join(", ", enumerable.Take(count - 1).Select(objectFormatter)),
                                 separator,
                                 objectFormatter(enumerable.Skip(count - 1).First()));
        }
    }
}