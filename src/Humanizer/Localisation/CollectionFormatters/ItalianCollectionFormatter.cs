using System;
using System.Collections.Generic;
using System.Linq;

namespace Humanizer.Localisation.CollectionFormatters
{
    internal class ItalianCollectionFormatter : EnglishCollectionFormatter
    {
        public ItalianCollectionFormatter()
            : base()
        {
            DefaultSeparator = "e";
        }

        public override string Humanize<T>(IEnumerable<T> collection, Func<T, String> objectFormatter, String separator)
        {
            if (collection == null)
                throw new ArgumentException("collection");

            T[] itemsArray = collection as T[] ?? collection.ToArray();

            int count = itemsArray.Length;

            if (count == 0)
                return "";

            if (count == 1)
                return objectFormatter(itemsArray[0]);

            IEnumerable<T> itemsBeforeLast = itemsArray.Take(count - 1);
            T lastItem = itemsArray.Skip(count - 1).First();

            return String.Format("{0} {1} {2}",
                String.Join(", ", itemsBeforeLast.Select(objectFormatter)),
                separator,
                objectFormatter(lastItem));
        }
    }
}
