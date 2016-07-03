using System;
using System.Collections.Generic;
using System.Linq;

namespace Humanizer.Localisation.CollectionFormatters
{
    class DefaultCollectionFormatter : ICollectionFormatter
    {
        protected string DefaultSeparator = "";

        public DefaultCollectionFormatter(string defaultSeparator)
        {
            DefaultSeparator = defaultSeparator;
        }

        public virtual string Humanize<T>(IEnumerable<T> collection)
        {
            return Humanize(collection, o => o?.ToString(), DefaultSeparator);
        }

        public virtual string Humanize<T>(IEnumerable<T> collection, Func<T, string> objectFormatter)
        {
            return Humanize(collection, objectFormatter, DefaultSeparator);
        }

        public virtual string Humanize<T>(IEnumerable<T> collection, string separator)
        {
            return Humanize(collection, o => o?.ToString(), separator);
        }

        public virtual string Humanize<T>(IEnumerable<T> collection, Func<T, string> objectFormatter, string separator)
        {
            if (collection == null)
                throw new ArgumentException("collection");

            var items = collection
                .Select(objectFormatter)
                .Select(item => item == null ? string.Empty : item.Trim())
                .Where(item => !string.IsNullOrWhiteSpace(item));

            var itemsArray = items.ToArray();
            var count = itemsArray.Length;

            if (count == 0)
                return "";

            if (count == 1)
                return itemsArray[0];

            var itemsBeforeLast = itemsArray.Take(count - 1);
            var lastItem = itemsArray.Skip(count - 1).First();

            return string.Format(GetConjunctionFormatString(count),
                string.Join(", ", itemsBeforeLast),
                separator,
                lastItem);
        }

        protected virtual string GetConjunctionFormatString(int itemCount) => "{0} {1} {2}";
    }
}
