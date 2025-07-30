namespace Humanizer;

class DefaultCollectionFormatter : ICollectionFormatter
{
    protected string DefaultSeparator = "";

    public DefaultCollectionFormatter(string defaultSeparator) =>
        DefaultSeparator = defaultSeparator;

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
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        if (objectFormatter == null)
        {
            throw new ArgumentNullException(nameof(objectFormatter));
        }

        return HumanizeDisplayStrings(
            collection.Select(objectFormatter),
            separator);
    }

    public string Humanize<T>(IEnumerable<T> collection, Func<T, object?> objectFormatter, string separator)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        if (objectFormatter == null)
        {
            throw new ArgumentNullException(nameof(objectFormatter));
        }

        return HumanizeDisplayStrings(
            collection
                .Select(objectFormatter)
                .Select(o => o?.ToString()),
            separator);
    }

    string HumanizeDisplayStrings(IEnumerable<string?> strings, string separator)
    {
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

        return string.Format(GetConjunctionFormatString(count),
            string.Join(", ", itemsArray, 0, itemsArray.Length - 1),
            separator,
            itemsArray[^1]);
    }

    protected virtual string GetConjunctionFormatString(int itemCount) => "{0} {1} {2}";
}