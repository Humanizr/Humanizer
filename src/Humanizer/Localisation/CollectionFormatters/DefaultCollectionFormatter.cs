using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.CollectionFormatters
{
    class DefaultCollectionFormatter : ICollectionFormatter
    {
        protected String DefaultSeparator = "";

        public virtual string FormatForDisplay<T>(IEnumerable<T> collection)
        {
            return FormatForDisplay(collection, o => o.ToString(), DefaultSeparator);
        }

        public virtual string FormatForDisplay<T>(IEnumerable<T> collection, Func<T, String> objectFormatter)
        {
            return FormatForDisplay(collection, objectFormatter, DefaultSeparator);
        }

        public virtual string FormatForDisplay<T>(IEnumerable<T> collection, String separator)
        {
            return FormatForDisplay(collection, o => o.ToString(), separator);
        }

        public virtual string FormatForDisplay<T>(IEnumerable<T> collection, Func<T, String> objectFormatter, String separator)
        {
            throw new NotImplementedException("A collection formatter for the current culture has not been implemented yet.");
        }
    }
}
