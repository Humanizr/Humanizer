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

        var items = collection.Select(objectFormatter)
            .Select(item => item?.Trim())
            .Where(item => !string.IsNullOrWhiteSpace(item))
            .ToArray();

        return items.Length switch
        {
            0 => string.Empty,
            1 => items[0]!,
            2 => $"{items[0]} {separator}{items[1]}",
            _ => $"{string.Join(", ", items, 0, items.Length - 1)} {separator}{items[^1]}"
        };
    }

    public string Humanize<T>(IEnumerable<T> collection, Func<T, object?> objectFormatter, string separator) =>
        Humanize(collection, item => objectFormatter(item)?.ToString(), separator);
}
