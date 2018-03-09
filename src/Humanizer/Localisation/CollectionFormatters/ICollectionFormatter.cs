using System;
using System.Collections.Generic;

namespace Humanizer.Localisation.CollectionFormatters
{
    /// <summary>
    /// An interface you should implement to localize Humanize for collections
    /// </summary>
    public interface ICollectionFormatter
    {
        /// <summary>
        /// Formats the collection for display, calling ToString() on each object.
        /// </summary>
        string Humanize<T>(IEnumerable<T> collection);

        /// <summary>
        /// Formats the collection for display, calling <paramref name="objectFormatter"/> on each element.
        /// </summary>
        string Humanize<T>(IEnumerable<T> collection, Func<T, string> objectFormatter);

        /// <summary>
        /// Formats the collection for display, calling <paramref name="objectFormatter"/> on each element.
        /// </summary>
        string Humanize<T>(IEnumerable<T> collection, Func<T, object> objectFormatter);

        /// <summary>
        /// Formats the collection for display, calling ToString() on each object
        /// and using <paramref name="separator"/> before the final item.
        /// </summary>
        string Humanize<T>(IEnumerable<T> collection, string separator);

        /// <summary>
        /// Formats the collection for display, calling <paramref name="objectFormatter"/> on each element.
        /// and using <paramref name="separator"/> before the final item.
        /// </summary>
        string Humanize<T>(IEnumerable<T> collection, Func<T, string> objectFormatter, string separator);

        /// <summary>
        /// Formats the collection for display, calling <paramref name="objectFormatter"/> on each element.
        /// and using <paramref name="separator"/> before the final item.
        /// </summary>
        string Humanize<T>(IEnumerable<T> collection, Func<T, object> objectFormatter, string separator);
    }
}
