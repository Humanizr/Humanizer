namespace Humanizer;

/// <summary>
/// Formats collections into localized, human-readable lists.
/// </summary>
/// <remarks>
/// Built-in implementations ignore rendered items that are <c>null</c>, empty, or whitespace after
/// formatting, and they may trim the surviving values before joining them. They also expect the
/// input collection and any formatter delegate to be non-<c>null</c>.
/// </remarks>
public interface ICollectionFormatter
{
    /// <summary>
    /// Formats the collection for display by calling <c>ToString()</c> on each item.
    /// </summary>
    /// <typeparam name="T">The item type in the collection.</typeparam>
    /// <param name="collection">The collection to format.</param>
    /// <returns>The formatted collection.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="collection"/> is <c>null</c>.</exception>
    string Humanize<T>(IEnumerable<T> collection);

    /// <summary>
    /// Formats the collection for display by calling <paramref name="objectFormatter"/> on each item.
    /// </summary>
    /// <typeparam name="T">The item type in the collection.</typeparam>
    /// <param name="collection">The collection to format.</param>
    /// <param name="objectFormatter">The formatter used to convert each item into text.</param>
    /// <returns>The formatted collection.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="collection"/> or <paramref name="objectFormatter"/> is <c>null</c>.
    /// </exception>
    string Humanize<T>(IEnumerable<T> collection, Func<T, string?> objectFormatter);

    /// <summary>
    /// Formats the collection for display by calling <paramref name="objectFormatter"/> on each item.
    /// </summary>
    /// <typeparam name="T">The item type in the collection.</typeparam>
    /// <param name="collection">The collection to format.</param>
    /// <param name="objectFormatter">The formatter used to convert each item into text.</param>
    /// <returns>The formatted collection.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="collection"/> or <paramref name="objectFormatter"/> is <c>null</c>.
    /// </exception>
    string Humanize<T>(IEnumerable<T> collection, Func<T, object?> objectFormatter);

    /// <summary>
    /// Formats the collection for display by calling <c>ToString()</c> on each item and using
    /// <paramref name="separator"/> to join the rendered items.
    /// </summary>
    /// <typeparam name="T">The item type in the collection.</typeparam>
    /// <param name="collection">The collection to format.</param>
    /// <param name="separator">The localized separator used to join the rendered items.</param>
    /// <remarks>
    /// Implementations may place the separator before the final item or between every item depending on the list style.
    /// </remarks>
    /// <returns>The formatted collection.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="collection"/> is <c>null</c>.</exception>
    string Humanize<T>(IEnumerable<T> collection, string separator);

    /// <summary>
    /// Formats the collection for display by calling <paramref name="objectFormatter"/> on each item and
    /// using <paramref name="separator"/> to join the rendered items.
    /// </summary>
    /// <typeparam name="T">The item type in the collection.</typeparam>
    /// <param name="collection">The collection to format.</param>
    /// <param name="objectFormatter">The formatter used to convert each item into text.</param>
    /// <param name="separator">The localized separator used to join the rendered items.</param>
    /// <remarks>
    /// Implementations may place the separator before the final item or between every item depending on the list style.
    /// </remarks>
    /// <returns>The formatted collection.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="collection"/> or <paramref name="objectFormatter"/> is <c>null</c>.
    /// </exception>
    string Humanize<T>(IEnumerable<T> collection, Func<T, string?> objectFormatter, string separator);

    /// <summary>
    /// Formats the collection for display by calling <paramref name="objectFormatter"/> on each item and
    /// using <paramref name="separator"/> to join the rendered items.
    /// </summary>
    /// <typeparam name="T">The item type in the collection.</typeparam>
    /// <param name="collection">The collection to format.</param>
    /// <param name="objectFormatter">The formatter used to convert each item into text.</param>
    /// <param name="separator">The localized separator used to join the rendered items.</param>
    /// <remarks>
    /// Implementations may place the separator before the final item or between every item depending on the list style.
    /// </remarks>
    /// <returns>The formatted collection.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="collection"/> or <paramref name="objectFormatter"/> is <c>null</c>.
    /// </exception>
    string Humanize<T>(IEnumerable<T> collection, Func<T, object?> objectFormatter, string separator);
}