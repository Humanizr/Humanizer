using System;
using System.Collections.Generic;
using Humanizer.Configuration;


namespace Humanizer
{
    /// <summary>
    /// Humanizes an IEnumerable into a human readable list
    /// </summary>
    public static class CollectionHumanizeExtensions
    {
        /// <summary>
        /// Formats the collection for display, calling ToString() on each object and
        /// using the default separator for the current culture.
        /// </summary>
        /// <returns></returns>
        public static string Humanize<T>(this IEnumerable<T> collection)
        {
            return Configurator.CollectionFormatter.Humanize(collection);
        }

        /// <summary>
        /// Formats the collection for display, calling `objectFormatter` on each object
        /// and using the default separator for the current culture.
        /// </summary>
        /// <returns></returns>
        public static string Humanize<T>(this IEnumerable<T> collection, Func<T, String> displayFormatter)
        {
            if (displayFormatter == null)
                throw new ArgumentNullException("displayFormatter");

            return Configurator.CollectionFormatter.Humanize(collection, displayFormatter);
        }

        /// <summary>
        /// Formats the collection for display, calling ToString() on each object
        /// and using the provided separator.
        /// </summary>
        /// <returns></returns>
        public static string Humanize<T>(this IEnumerable<T> collection, String separator)
        {

            return Configurator.CollectionFormatter.Humanize(collection, separator);
        }

        /// <summary>
        /// Formats the collection for display, calling `objectFormatter` on each object
        /// and using the provided separator.
        /// </summary>
        /// <returns></returns>
        public static string Humanize<T>(this IEnumerable<T> collection, Func<T, String> displayFormatter, String separator)
        {
            if (displayFormatter == null)
                throw new ArgumentNullException("displayFormatter");

            return Configurator.CollectionFormatter.Humanize(collection, displayFormatter, separator);
        }
    }
}
