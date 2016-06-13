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
            return Humanize(collection, o => o?.ToString(), DefaultSeparator, StringJoinOptions.Default);
        }

        public virtual string Humanize<T>(IEnumerable<T> collection, StringJoinOptions options)
        {
            return Humanize(collection, o => o?.ToString(), DefaultSeparator, options);
        }

        public virtual string Humanize<T>(IEnumerable<T> collection, Func<T, string> objectFormatter)
        {
            return Humanize(collection, objectFormatter, DefaultSeparator, StringJoinOptions.Default);
        }

        public virtual string Humanize<T>(IEnumerable<T> collection, Func<T, string> objectFormatter, StringJoinOptions options)
        {
            return Humanize(collection, objectFormatter, DefaultSeparator, options);
        }

        public virtual string Humanize<T>(IEnumerable<T> collection, string separator)
        {
            return Humanize(collection, o => o?.ToString(), separator, StringJoinOptions.Default);
        }

        public virtual string Humanize<T>(IEnumerable<T> collection, string separator, StringJoinOptions options)
        {
            return Humanize(collection, o => o?.ToString(), separator, options);
        }

        public virtual string Humanize<T>(IEnumerable<T> collection, Func<T, string> objectFormatter, string separator)
        {
            return Humanize(collection, objectFormatter, separator, StringJoinOptions.Default);
        }

        public virtual string Humanize<T>(IEnumerable<T> collection, Func<T, string> objectFormatter, string separator, StringJoinOptions options)
        {
            if (collection == null)
                throw new ArgumentException("collection");

            var items = collection.Select(objectFormatter);
            if ((options & StringJoinOptions.TrimEntries) != 0) items = items.Select(item => item == null ? string.Empty : item.Trim());
            if ((options & StringJoinOptions.RemoveBlankEntries) != 0) items = items.Where(item => !string.IsNullOrWhiteSpace(item));

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
