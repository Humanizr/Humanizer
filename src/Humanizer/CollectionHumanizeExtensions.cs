using System;
using System.Collections.Generic;
using Humanizer.Configuration;
using JetBrains.Annotations;


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
        [Pure]
        [NotNull]
        [PublicAPI]
        public static string Humanize<T>([NotNull] this IEnumerable<T> collection)
        {
            return Configurator.CollectionFormatter.Humanize(collection);
        }

        /// <summary>
        /// Formats the collection for display, calling <paramref name="displayFormatter"/> on each element
        /// and using the default separator for the current culture.
        /// </summary>
        [Pure]
        [NotNull]
        [PublicAPI]
        [ContractAnnotation("displayFormatter:null => halt")]
        public static string Humanize<T>([NotNull] this IEnumerable<T> collection, [NotNull] Func<T, string> displayFormatter)
        {
            if (displayFormatter == null)
                throw new ArgumentNullException(nameof(displayFormatter));

            return Configurator.CollectionFormatter.Humanize(collection, displayFormatter);
        }

        /// <summary>
        /// Formats the collection for display, calling <paramref name="displayFormatter"/> on each element
        /// and using the default separator for the current culture.
        /// </summary>
        public static string Humanize<T>(this IEnumerable<T> collection, Func<T, object> displayFormatter)
        {
            if (displayFormatter == null)
                throw new ArgumentNullException(nameof(displayFormatter));

            return Configurator.CollectionFormatter.Humanize(collection, displayFormatter);
        }

        /// <summary>
        /// Formats the collection for display, calling ToString() on each object
        /// and using the provided separator.
        /// </summary>
        [Pure]
        [NotNull]
        [PublicAPI]
        public static string Humanize<T>([NotNull] this IEnumerable<T> collection, [NotNull] string separator)
        {

            return Configurator.CollectionFormatter.Humanize(collection, separator);
        }

        /// <summary>
        /// Formats the collection for display, calling <paramref name="displayFormatter"/> on each element
        /// and using the provided separator.
        /// </summary>
        [Pure]
        [NotNull]
        [PublicAPI]
        [ContractAnnotation("displayFormatter:null => halt")]
        public static string Humanize<T>([NotNull] this IEnumerable<T> collection, [NotNull] Func<T, string> displayFormatter, [NotNull] string separator)
        {
            if (displayFormatter == null)
                throw new ArgumentNullException(nameof(displayFormatter));

            return Configurator.CollectionFormatter.Humanize(collection, displayFormatter, separator);
        }

        /// <summary>
        /// Formats the collection for display, calling <paramref name="displayFormatter"/> on each element
        /// and using the provided separator.
        /// </summary>
        [Pure]
        [NotNull]
        [PublicAPI]
        [ContractAnnotation("displayFormatter:null => halt")]
        public static string Humanize<T>([NotNull] this IEnumerable<T> collection, [NotNull] Func<T, object> displayFormatter, [NotNull] string separator)
        {
            if (displayFormatter == null)
                throw new ArgumentNullException(nameof(displayFormatter));

            return Configurator.CollectionFormatter.Humanize(collection, displayFormatter, separator);
        }
    }
}
