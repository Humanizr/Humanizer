namespace Humanizer;

class DelimitedCollectionFormatter(string delimiter) : ICollectionFormatter
{
    public string Humanize<T>(IEnumerable<T> collection) =>
        Humanize(collection, item => item?.ToString(), delimiter);

    public string Humanize<T>(IEnumerable<T> collection, Func<T, string?> objectFormatter) =>
        Join(collection, objectFormatter, delimiter);

    public string Humanize<T>(IEnumerable<T> collection, Func<T, object?> objectFormatter) =>
        Join(collection, item => objectFormatter(item)?.ToString(), delimiter);

    public string Humanize<T>(IEnumerable<T> collection, string separator) =>
        Join(collection, item => item?.ToString(), separator);

    public string Humanize<T>(IEnumerable<T> collection, Func<T, string?> objectFormatter, string separator) =>
        Join(collection, objectFormatter, separator);

    public string Humanize<T>(IEnumerable<T> collection, Func<T, object?> objectFormatter, string separator) =>
        Join(collection, item => objectFormatter(item)?.ToString(), separator);

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
                firstValue = formatted;
                continue;
            }

            builder ??= new StringBuilder(firstValue);
            builder.Append(separator);
            builder.Append(formatted);
        }

        return builder?.ToString() ?? firstValue ?? string.Empty;
    }
}
