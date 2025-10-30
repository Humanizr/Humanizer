namespace Humanizer;

/// <summary>
/// Contains methods for removing, appending and prepending article prefixes for sorting strings ignoring the article.
/// </summary>
public static partial class EnglishArticle
{
#if NET7_0_OR_GREATER
    [GeneratedRegex(@"^((The)|(the)|(a)|(A)|(An)|(an))\s\w+")]
    private static partial Regex ArticleRegexGenerated();
    
    private static Regex ArticleRegex() => ArticleRegexGenerated();
#else
    private static readonly Regex ArticleRegexField = new(@"^((The)|(the)|(a)|(A)|(An)|(an))\s\w+", RegexOptions.Compiled);

    private static Regex ArticleRegex() => ArticleRegexField;
#endif

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
            if (ArticleRegex().IsMatch(item))
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

        for (var i = 0; i < appended.Length; i++)
        {
            var item = appended[i];
            var append = item.AsSpan();
            
            // Check for " the" (4 chars total including space)
            if (append.Length > 4 && append[^4] == ' ')
            {
                var lastThree = append[^3..];
                if (lastThree.Equals("the", StringComparison.OrdinalIgnoreCase))
                {
                    inserted[i] = string.Create(item.Length, item, (span, str) =>
                    {
                        var source = str.AsSpan();
                        var suffix = source[^3..];
                        var prefix = source[..^4];
                        suffix.CopyTo(span);
                        span[suffix.Length] = ' ';
                        prefix.CopyTo(span[(suffix.Length + 1)..]);
                    });
                    continue;
                }
            }
            
            // Check for " an" (3 chars total including space)
            if (append.Length > 3 && append[^3] == ' ')
            {
                var lastTwo = append[^2..];
                if (lastTwo.Equals("an", StringComparison.OrdinalIgnoreCase))
                {
                    inserted[i] = string.Create(item.Length, item, (span, str) =>
                    {
                        var source = str.AsSpan();
                        var suffix = source[^2..];
                        var prefix = source[..^3];
                        suffix.CopyTo(span);
                        span[suffix.Length] = ' ';
                        prefix.CopyTo(span[(suffix.Length + 1)..]);
                    });
                    continue;
                }
            }
            
            // Check for " a" (2 chars total including space)
            if (append.Length > 2 && append[^2] == ' ')
            {
                var lastOne = append[^1..];
                if (lastOne.Equals("a", StringComparison.OrdinalIgnoreCase))
                {
                    inserted[i] = string.Create(item.Length, item, (span, str) =>
                    {
                        var source = str.AsSpan();
                        var suffix = source[^1..];
                        var prefix = source[..^2];
                        suffix.CopyTo(span);
                        span[suffix.Length] = ' ';
                        prefix.CopyTo(span[(suffix.Length + 1)..]);
                    });
                    continue;
                }
            }
            
            inserted[i] = item;
        }

        return inserted;
    }
}