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

        return HumanizeDisplayStrings(collection, objectFormatter, separator);
    }

    public string Humanize<T>(IEnumerable<T> collection, Func<T, object?> objectFormatter, string separator)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(objectFormatter);

        return HumanizeDisplayObjects(collection, objectFormatter, separator);
    }

    string HumanizeDisplayStrings<T>(IEnumerable<T> collection, Func<T, string?> objectFormatter, string separator)
    {
        var items = CreateDisplayItems(collection);
        foreach (var item in collection)
        {
            AddDisplayString(items, objectFormatter(item));
        }

        return HumanizeDisplayItems(items, separator);
    }

    string HumanizeDisplayObjects<T>(IEnumerable<T> collection, Func<T, object?> objectFormatter, string separator)
    {
        var items = CreateDisplayItems(collection);
        foreach (var item in collection)
        {
            AddDisplayString(items, objectFormatter(item)?.ToString());
        }

        return HumanizeDisplayItems(items, separator);
    }

    static List<string> CreateDisplayItems<T>(IEnumerable<T> collection) =>
        collection switch
        {
            ICollection<T> items => new(items.Count),
            IReadOnlyCollection<T> items => new(items.Count),
            _ => []
        };

    static void AddDisplayString(List<string> items, string? item)
    {
        if (string.IsNullOrWhiteSpace(item))
        {
            return;
        }

        items.Add(item!.Trim());
    }

    string HumanizeDisplayItems(List<string> items, string separator)
    {
        var count = items.Count;

        if (count == 0)
        {
            return "";
        }

        if (count == 1)
        {
            return items[0];
        }

        var conjunctionFormat = GetConjunctionFormatString(count);
        if (conjunctionFormat == "{0} {1} {2}")
        {
            return count == 2
                ? string.Concat(items[0], " ", separator, " ", items[1])
                : string.Concat(JoinLeadingItems(items), " ", separator, " ", items[^1]);
        }

        return string.Format(conjunctionFormat,
            JoinLeadingItems(items),
            separator,
            items[^1]);
    }

    static string JoinLeadingItems(List<string> items)
    {
        var count = items.Count - 1;
        if (count == 1)
        {
            return items[0];
        }

        var builder = new StringBuilder();
        builder.Append(items[0]);
        for (var i = 1; i < count; i++)
        {
            builder.Append(", ");
            builder.Append(items[i]);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Returns the format string used to combine the final two items.
    /// </summary>
    /// <param name="itemCount">The number of displayable items remaining after filtering.</param>
    /// <returns>The format string used for the final join operation.</returns>
    protected virtual string GetConjunctionFormatString(int itemCount) => "{0} {1} {2}";
}