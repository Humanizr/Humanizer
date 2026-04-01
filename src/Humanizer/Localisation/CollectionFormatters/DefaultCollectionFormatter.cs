namespace Humanizer;

/// <summary>
/// Formats collections by joining their items with a localized conjunction.
/// </summary>
class DefaultCollectionFormatter(string defaultSeparator) : ICollectionFormatter
{
    /// <summary>
    /// Gets the separator used when callers do not provide one explicitly.
    /// </summary>
    protected string DefaultSeparator { get; } = defaultSeparator;

    public virtual string Humanize<T>(IEnumerable<T> collection) =>
        Humanize(collection, o => o?.ToString(), DefaultSeparator);

    public virtual string Humanize<T>(IEnumerable<T> collection, Func<T, string?> objectFormatter) =>
        Humanize(collection, objectFormatter, DefaultSeparator);

    public string Humanize<T>(IEnumerable<T> collection, Func<T, object?> objectFormatter) =>
        Humanize(collection, objectFormatter, DefaultSeparator);

    public virtual string Humanize<T>(IEnumerable<T> collection, string separator) =>
        Humanize(collection, o => o?.ToString(), separator);

    public virtual string Humanize<T>(IEnumerable<T> collection, Func<T, string?> objectFormatter, string separator)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(objectFormatter);

        return HumanizeDisplayStrings(
            collection.Select(objectFormatter),
            separator);
    }

    public string Humanize<T>(IEnumerable<T> collection, Func<T, object?> objectFormatter, string separator)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(objectFormatter);

        return HumanizeDisplayStrings(
            collection
                .Select(objectFormatter)
                .Select(o => o?.ToString()),
            separator);
    }

    string HumanizeDisplayStrings(IEnumerable<string?> strings, string separator)
    {
        // Materialize once so the filtered display count and the final join can share the same sequence.
        // Re-enumerating here would be wrong for one-shot enumerables and would change the allocation profile.
        var itemsArray = strings
            .Select(item => item == null ? string.Empty : item.Trim())
            .Where(item => !string.IsNullOrWhiteSpace(item))
            .ToArray();

        var count = itemsArray.Length;

        if (count == 0)
        {
            return "";
        }

        if (count == 1)
        {
            return itemsArray[0];
        }

        var conjunctionFormat = GetConjunctionFormatString(count);
        if (conjunctionFormat == "{0} {1} {2}")
        {
            // Fast path: avoid string.Format for default format.
            return string.Concat(
                string.Join(", ", itemsArray, 0, itemsArray.Length - 1),
                " ",
                separator,
                " ",
                itemsArray[^1]);
        }

        return string.Format(conjunctionFormat,
            string.Join(", ", itemsArray, 0, itemsArray.Length - 1),
            separator,
            itemsArray[^1]);
    }

    /// <summary>
    /// Returns the format string used to combine the final two items.
    /// </summary>
    /// <param name="itemCount">The number of displayable items remaining after filtering.</param>
    /// <returns>The format string used for the final join operation.</returns>
    protected virtual string GetConjunctionFormatString(int itemCount) => "{0} {1} {2}";
}
