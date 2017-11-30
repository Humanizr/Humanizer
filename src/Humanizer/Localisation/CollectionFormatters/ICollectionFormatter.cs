using System;
using System.Collections.Generic;
using JetBrains.Annotations;

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
        [NotNull]
        [PublicAPI]
        string Humanize<T>([NotNull] IEnumerable<T> collection);

        /// <summary>
        /// Formats the collection for display, calling <paramref name="objectFormatter"/> on each element.
        /// </summary>
        [NotNull]
        [PublicAPI]
        string Humanize<T>([NotNull] IEnumerable<T> collection, [NotNull] Func<T, string> objectFormatter);

        /// <summary>
        /// Formats the collection for display, calling <paramref name="objectFormatter"/> on each element.
        /// </summary>
        string Humanize<T>(IEnumerable<T> collection, Func<T, object> objectFormatter);

        /// <summary>
        /// Formats the collection for display, calling ToString() on each object
        /// and using <paramref name="separator"/> before the final item.
        /// </summary>
        [NotNull]
        [PublicAPI]
        string Humanize<T>([NotNull] IEnumerable<T> collection, [NotNull] string separator);

        /// <summary>
        /// Formats the collection for display, calling <paramref name="objectFormatter"/> on each element.
        /// and using <paramref name="separator"/> before the final item.
        /// </summary>
        [NotNull]
        [PublicAPI]
        string Humanize<T>([NotNull] IEnumerable<T> collection, [NotNull] Func<T, string> objectFormatter, [NotNull] string separator);
    }
}
