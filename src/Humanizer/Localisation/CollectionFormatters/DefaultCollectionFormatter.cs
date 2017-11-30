using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Humanizer.Localisation.CollectionFormatters
{
    class DefaultCollectionFormatter : ICollectionFormatter
    {
        protected string DefaultSeparator = "";

        public DefaultCollectionFormatter(string defaultSeparator)
        {
            DefaultSeparator = defaultSeparator;
        }

        [ContractAnnotation("collection:null => halt")]
        public virtual string Humanize<T>(IEnumerable<T> collection)
        {
            return Humanize(collection, o => o?.ToString(), DefaultSeparator);
        }

        [ContractAnnotation("collection:null => halt")]
        public virtual string Humanize<T>(IEnumerable<T> collection, Func<T, string> objectFormatter)
        {
            return Humanize(collection, objectFormatter, DefaultSeparator);
        }

        [ContractAnnotation("collection:null => halt")]
        public string Humanize<T>(IEnumerable<T> collection, Func<T, object> objectFormatter)
        {
            return Humanize(collection, objectFormatter, DefaultSeparator);
        }

        [ContractAnnotation("collection:null => halt")]
        public virtual string Humanize<T>(IEnumerable<T> collection, string separator)
        {
            return Humanize(collection, o => o?.ToString(), separator);
        }

        [ContractAnnotation("collection:null => halt")]
        public virtual string Humanize<T>(IEnumerable<T> collection, Func<T, string> objectFormatter, string separator)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (objectFormatter == null) throw new ArgumentNullException(nameof(objectFormatter));

            return HumanizeDisplayStrings(
                collection.Select(objectFormatter),
                separator);
        }

        public string Humanize<T>(IEnumerable<T> collection, Func<T, object> objectFormatter, string separator)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (objectFormatter == null) throw new ArgumentNullException(nameof(objectFormatter));

            return HumanizeDisplayStrings(
                collection.Select(objectFormatter).Select(o => o?.ToString()),
                separator);
        }

        private string HumanizeDisplayStrings(IEnumerable<string> strings, string separator)
        {
            var itemsArray = strings
                .Select(item => item == null ? string.Empty : item.Trim())
                .Where(item => !string.IsNullOrWhiteSpace(item))
                .ToArray();

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

        [NotNull]
        protected virtual string GetConjunctionFormatString(int itemCount) => "{0} {1} {2}";
    }
}
