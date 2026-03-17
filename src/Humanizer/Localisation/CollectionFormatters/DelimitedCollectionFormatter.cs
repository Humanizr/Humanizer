namespace Humanizer;

class DelimitedCollectionFormatter(string delimiter) : ICollectionFormatter
{
    public string Humanize<T>(IEnumerable<T> collection) =>
        Humanize(collection, item => item?.ToString(), delimiter);

    public string Humanize<T>(IEnumerable<T> collection, Func<T, string?> objectFormatter) =>
        Join(collection.Select(objectFormatter), delimiter);

    public string Humanize<T>(IEnumerable<T> collection, Func<T, object?> objectFormatter) =>
        Join(collection.Select(objectFormatter).Select(item => item?.ToString()), delimiter);

    public string Humanize<T>(IEnumerable<T> collection, string separator) =>
        Join(collection.Select(item => item?.ToString()), separator);

    public string Humanize<T>(IEnumerable<T> collection, Func<T, string?> objectFormatter, string separator) =>
        Join(collection.Select(objectFormatter), separator);

    public string Humanize<T>(IEnumerable<T> collection, Func<T, object?> objectFormatter, string separator) =>
        Join(collection.Select(objectFormatter).Select(item => item?.ToString()), separator);

    static string Join(IEnumerable<string?> values, string separator)
    {
        ArgumentNullException.ThrowIfNull(values);

        return string.Join(
            separator,
            values.Select(item => item?.Trim())
                .Where(item => !string.IsNullOrWhiteSpace(item)));
    }
}
