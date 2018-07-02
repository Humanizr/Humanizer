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
        public static string Humanize<T>(this IEnumerable<T> collection)
        {
            return Configurator.CollectionFormatter.Humanize(collection);
        }

        /// <summary>
        /// Formats the collection for display, calling <paramref name="displayFormatter"/> on each element
        /// and using the default separator for the current culture.
        /// </summary>
        public static string Humanize<T>(this IEnumerable<T> collection, Func<T, string> displayFormatter)
        {
            if (displayFormatter == null)
            {
                throw new ArgumentNullException(nameof(displayFormatter));
            }

            return Configurator.CollectionFormatter.Humanize(collection, displayFormatter);
        }

        /// <summary>
        /// Formats the collection for display, calling <paramref name="displayFormatter"/> on each element
        /// and using the default separator for the current culture.
        /// </summary>
        public static string Humanize<T>(this IEnumerable<T> collection, Func<T, object> displayFormatter)
        {
            if (displayFormatter == null)
            {
                throw new ArgumentNullException(nameof(displayFormatter));
            }

            return Configurator.CollectionFormatter.Humanize(collection, displayFormatter);
        }

        /// <summary>
        /// Formats the collection for display, calling ToString() on each object
        /// and using the provided separator.
        /// </summary>
        public static string Humanize<T>(this IEnumerable<T> collection, string separator)
        {

            return Configurator.CollectionFormatter.Humanize(collection, separator);
        }

        /// <summary>
        /// Formats the collection for display, calling <paramref name="displayFormatter"/> on each element
        /// and using the provided separator.
        /// </summary>
        public static string Humanize<T>(this IEnumerable<T> collection, Func<T, string> displayFormatter, string separator)
        {
            if (displayFormatter == null)
            {
                throw new ArgumentNullException(nameof(displayFormatter));
            }

            return Configurator.CollectionFormatter.Humanize(collection, displayFormatter, separator);
        }

        /// <summary>
        /// Formats the collection for display, calling <paramref name="displayFormatter"/> on each element
        /// and using the provided separator.
        /// </summary>
        public static string Humanize<T>(this IEnumerable<T> collection, Func<T, object> displayFormatter, string separator)
        {
            if (displayFormatter == null)
            {
                throw new ArgumentNullException(nameof(displayFormatter));
            }

            return Configurator.CollectionFormatter.Humanize(collection, displayFormatter, separator);
        }
    }
}
