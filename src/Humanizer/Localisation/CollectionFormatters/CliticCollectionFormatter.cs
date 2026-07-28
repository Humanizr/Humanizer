namespace Humanizer;

/// <summary>
/// Formats collections by treating the final separator as a clitic conjunction.
/// </summary>
class CliticCollectionFormatter(string conjunction) : ICollectionFormatter
{
    readonly DefaultCollectionFormatter fallbackFormatter = new(conjunction);

    public string Humanize<T>(IEnumerable<T> collection) =>
        Humanize(collection, item => item?.ToString(), conjunction);

    public string Humanize<T>(IEnumerable<T> collection, Func<T, string?> objectFormatter) =>
        Humanize(collection, objectFormatter, conjunction);

    public string Humanize<T>(IEnumerable<T> collection, Func<T, object?> objectFormatter) =>
        Humanize(collection, objectFormatter, conjunction);

    public string Humanize<T>(IEnumerable<T> collection, string separator) =>
        fallbackFormatter.Humanize(collection, separator);

    public string Humanize<T>(IEnumerable<T> collection, Func<T, string?> objectFormatter, string separator)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(objectFormatter);

        StringBuilder? head = null;
        string? lastItem = null;
        var count = 0;

        foreach (var item in collection)
        {
            var formatted = objectFormatter(item)?.Trim();
            if (string.IsNullOrWhiteSpace(formatted))
            {
                continue;
            }

            if (count == 0)
            {
                // Hold the first visible item separately so the 0- and 1-item paths stay allocation-free.
                lastItem = formatted;
                count = 1;
                continue;
            }

            if (count == 1)
            {
                // Once we have two visible items, build the comma-separated head and keep the tail isolated.
                head = new StringBuilder(lastItem);
                lastItem = formatted;
                count = 2;
                continue;
            }

            // The penultimate item belongs in the comma-separated head; the last item stays separate so the
            // locale-specific conjunction can decide whether it needs to cliticize the previous word.
            head!.Append(", ");
            head.Append(lastItem);
            lastItem = formatted;
            count++;
        }

        return count switch
        {
            0 => string.Empty,
            1 => lastItem!,
            _ => string.Concat(head, " ", separator, lastItem)
        };
    }

    public string Humanize<T>(IEnumerable<T> collection, Func<T, object?> objectFormatter, string separator)
    {
        ArgumentNullException.ThrowIfNull(objectFormatter);

        return Humanize(collection, item => objectFormatter(item)?.ToString(), separator);
    }
}