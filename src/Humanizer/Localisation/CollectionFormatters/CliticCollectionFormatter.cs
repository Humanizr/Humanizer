namespace Humanizer;

class CliticCollectionFormatter(string conjunction) : ICollectionFormatter
{
    readonly DefaultCollectionFormatter fallbackFormatter = new(conjunction);

    public string Humanize<T>(IEnumerable<T> collection) =>
        Humanize(collection, item => item?.ToString(), conjunction);

    public string Humanize<T>(IEnumerable<T> collection, Func<T, string?> objectFormatter) =>
        Humanize(collection, objectFormatter, conjunction);

    public string Humanize<T>(IEnumerable<T> collection, Func<T, object?> objectFormatter) =>
        Humanize(collection, item => objectFormatter(item)?.ToString(), conjunction);

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
                lastItem = formatted;
                count = 1;
                continue;
            }

            if (count == 1)
            {
                head = new StringBuilder(lastItem);
                lastItem = formatted;
                count = 2;
                continue;
            }

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

    public string Humanize<T>(IEnumerable<T> collection, Func<T, object?> objectFormatter, string separator) =>
        Humanize(collection, item => objectFormatter(item)?.ToString(), separator);
}
