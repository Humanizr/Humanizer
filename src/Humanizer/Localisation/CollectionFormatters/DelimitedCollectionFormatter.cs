namespace Humanizer;

/// <summary>
/// Formats collections by joining the display values with a delimiter.
/// </summary>
class DelimitedCollectionFormatter(string delimiter) : ICollectionFormatter
{
    public string Humanize<T>(IEnumerable<T> collection) =>
        Humanize(collection, item => item?.ToString(), delimiter);

    public string Humanize<T>(IEnumerable<T> collection, Func<T, string?> objectFormatter) =>
        Join(collection, objectFormatter, delimiter);

    public string Humanize<T>(IEnumerable<T> collection, Func<T, object?> objectFormatter) =>
        Humanize(collection, objectFormatter, delimiter);

    public string Humanize<T>(IEnumerable<T> collection, string separator) =>
        Join(collection, item => item?.ToString(), separator);

    public string Humanize<T>(IEnumerable<T> collection, Func<T, string?> objectFormatter, string separator) =>
        Join(collection, objectFormatter, separator);

    public string Humanize<T>(IEnumerable<T> collection, Func<T, object?> objectFormatter, string separator)
    {
        ArgumentNullException.ThrowIfNull(objectFormatter);

        return Join(collection, item => objectFormatter(item)?.ToString(), separator);
    }

    /// <summary>
    /// Joins the formatted values with <paramref name="separator"/> while skipping blank items.
    /// </summary>
    static string Join<T>(IEnumerable<T> collection, Func<T, string?> objectFormatter, string separator)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(objectFormatter);

        StringBuilder? builder = null;
        string? firstValue = null;

        foreach (var item in collection)
        {
            var formatted = objectFormatter(item)?.Trim();
            if (string.IsNullOrWhiteSpace(formatted))
            {
                continue;
            }

            if (firstValue == null)
            {
                // Keep the first visible item outside the builder so the common one-item path avoids allocation.
                firstValue = formatted;
                continue;
            }

            // Delimited lists use the same separator between every visible item, so we can append uniformly
            // once the first value has been established.
            builder ??= new StringBuilder(firstValue);
            builder.Append(separator);
            builder.Append(formatted);
        }

        return builder?.ToString() ?? firstValue ?? string.Empty;
    }
}