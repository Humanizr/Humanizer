namespace Humanizer;

/// <summary>
/// Contains methods for removing, appending and prepending article prefixes for sorting strings ignoring the article.
/// </summary>
public static class EnglishArticle
{
    static readonly Regex ArticleRegex = new("^((The)|(the)|(a)|(A)|(An)|(an))\\s\\w+", RegexOptions.Compiled);

    /// <summary>
    /// Removes the prefixed article and appends it to the same string.
    /// </summary>
    /// <param name="items">The input array of strings</param>
    /// <returns>Sorted string array</returns>
    public static string[] AppendArticlePrefix(string[] items)
    {
        if (items.Length == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(items));
        }

        var transformed = new string[items.Length];

        for (var i = 0; i < items.Length; i++)
        {
            var item = items[i]
                .AsSpan();
            if (ArticleRegex.IsMatch(item))
            {
                var indexOf = item.IndexOf(' ');
                var removed = item[indexOf..]
                    .TrimStart();
                var article = item[..indexOf]
                    .TrimEnd();
                transformed[i] = $"{removed} {article}";
            }
            else
            {
                transformed[i] = item
                    .Trim()
                    .ToString();
            }
        }

        Array.Sort(transformed);
        return transformed;
    }

    /// <summary>
    /// Removes the previously appended article and prepends it to the same string.
    /// </summary>
    /// <param name="appended">Sorted string array</param>
    /// <returns>String array</returns>
    public static string[] PrependArticleSuffix(string[] appended)
    {
        var inserted = new string[appended.Length];
        var the = " the".AsSpan();
        var an = " an".AsSpan();
        var a = " a".AsSpan();

        for (var i = 0; i < appended.Length; i++)
        {
            var append = appended[i]
                .AsSpan();
            if (append.EndsWith(the, StringComparison.OrdinalIgnoreCase))
            {
                inserted[i] = ToOriginalFormat(append, 3);
            }
            else if (append.EndsWith(an, StringComparison.OrdinalIgnoreCase))
            {
                inserted[i] = ToOriginalFormat(append, 2);
            }
            else if (append.EndsWith(a, StringComparison.OrdinalIgnoreCase))
            {
                inserted[i] = ToOriginalFormat(append, 1);
            }
            else
            {
                inserted[i] = appended[i];
            }
        }

        return inserted;
    }

    static string ToOriginalFormat(CharSpan value, int suffixLength) =>
        $"{value[^suffixLength..]} {value[..^(suffixLength + 1)]}";
}