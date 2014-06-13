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
        /// <returns></returns>
        String Humanize<T>(IEnumerable<T> collection);

        /// <summary>
        /// Formats the collection for display, calling `objectFormatter` on each object.
        /// </summary>
        /// <returns></returns>
        String Humanize<T>(IEnumerable<T> collection, Func<T, String> objectFormatter);

        /// <summary>
        /// Formats the collection for display, calling ToString() on each object
        /// and using `separator` before the final item.
        /// </summary>
        /// <returns></returns>
        String Humanize<T>(IEnumerable<T> collection, String separator);

        /// <summary>
        /// Formats the collection for display, calling `objectFormatter` on each object
        /// and using `separator` before the final item.
        /// </summary>
        /// <returns></returns>
        String Humanize<T>(IEnumerable<T> collection, Func<T, String> objectFormatter, String separator);
    }
}
