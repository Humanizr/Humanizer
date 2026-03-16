namespace Humanizer;

class CompactCollectionFormatter(string conjunction, string listSeparator = "、") : ICollectionFormatter
{
    public string Humanize<T>(IEnumerable<T> collection) =>
        Humanize(collection, value => value?.ToString());

    public string Humanize<T>(IEnumerable<T> collection, Func<T, string?> objectFormatter)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(objectFormatter);

        var items = collection
            .Select(objectFormatter)
            .Where(static value => !string.IsNullOrWhiteSpace(value))
            .Select(static value => value!.Trim())
            .ToArray();

        return items.Length switch
        {
            0 => string.Empty,
            1 => items[0],
            2 => $"{items[0]}{conjunction}{items[1]}",
            _ => $"{string.Join(listSeparator, items, 0, items.Length - 1)}{conjunction}{items[^1]}"
        };
    }

    public string Humanize<T>(IEnumerable<T> collection, Func<T, object?> objectFormatter) =>
        Humanize(collection, value => objectFormatter(value)?.ToString());

    public string Humanize<T>(IEnumerable<T> collection, string separator) =>
        Humanize(collection, value => value?.ToString());

    public string Humanize<T>(IEnumerable<T> collection, Func<T, string?> objectFormatter, string separator) =>
        Humanize(collection, objectFormatter);

    public string Humanize<T>(IEnumerable<T> collection, Func<T, object?> objectFormatter, string separator) =>
        Humanize(collection, objectFormatter);
}
