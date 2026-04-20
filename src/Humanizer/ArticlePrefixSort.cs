namespace Humanizer;

/// <summary>
/// Contains methods for removing, appending and prepending article prefixes for sorting strings ignoring the article.
/// </summary>
public static class EnglishArticle
{
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
            if (TryGetArticlePrefixLength(item, out var articleLength))
            {
                var removed = item[articleLength..]
                    .TrimStart();
                var article = item[..articleLength];
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

    static bool TryGetArticlePrefixLength(ReadOnlySpan<char> item, out int articleLength)
    {
        articleLength = 0;
        if (IsArticle(item, "The"))
        {
            articleLength = 3;
            return true;
        }

        if (IsArticle(item, "the"))
        {
            articleLength = 3;
            return true;
        }

        if (IsArticle(item, "a"))
        {
            articleLength = 1;
            return true;
        }

        if (IsArticle(item, "A"))
        {
            articleLength = 1;
            return true;
        }

        if (IsArticle(item, "An"))
        {
            articleLength = 2;
            return true;
        }

        if (IsArticle(item, "an"))
        {
            articleLength = 2;
            return true;
        }

        return false;
    }

    static bool IsArticle(ReadOnlySpan<char> item, string article)
    {
        var articleLength = article.Length;
        return item.Length > articleLength + 1 &&
            item[..articleLength].SequenceEqual(article) &&
            item[articleLength] == ' ' &&
            IsRegexWordCharacter(item[articleLength + 1]);
    }

    static bool IsRegexWordCharacter(char c) =>
        CharUnicodeInfo.GetUnicodeCategory(c) is UnicodeCategory.UppercaseLetter
            or UnicodeCategory.LowercaseLetter
            or UnicodeCategory.TitlecaseLetter
            or UnicodeCategory.ModifierLetter
            or UnicodeCategory.OtherLetter
            or UnicodeCategory.DecimalDigitNumber
            or UnicodeCategory.ConnectorPunctuation
            or UnicodeCategory.NonSpacingMark;

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
                    inserted[i] = RearrangeArticle(item, suffixLength: 3, totalLength: 4);
                    continue;
                }
            }

            // Check for " an" (3 chars total including space)
            if (append.Length > 3 && append[^3] == ' ')
            {
                var lastTwo = append[^2..];
                if (lastTwo.Equals("an", StringComparison.OrdinalIgnoreCase))
                {
                    inserted[i] = RearrangeArticle(item, suffixLength: 2, totalLength: 3);
                    continue;
                }
            }

            // Check for " a" (2 chars total including space)
            if (append.Length > 2 && append[^2] == ' ')
            {
                var lastOne = append[^1..];
                if (lastOne.Equals("a", StringComparison.OrdinalIgnoreCase))
                {
                    inserted[i] = RearrangeArticle(item, suffixLength: 1, totalLength: 2);
                    continue;
                }
            }

            inserted[i] = item;
        }

        return inserted;
    }

    static string RearrangeArticle(string item, int suffixLength, int totalLength)
    {
#if NET6_0_OR_GREATER
        return string.Create(item.Length, (item, suffixLength, totalLength), (span, state) =>
        {
            var source = state.item.AsSpan();
            var suffix = source[^state.suffixLength..];
            var prefix = source[..^state.totalLength];
            suffix.CopyTo(span);
            span[suffix.Length] = ' ';
            prefix.CopyTo(span[(suffix.Length + 1)..]);
        });
#else
        var source = item.AsSpan();
        var suffix = source[^suffixLength..];
        var prefix = source[..^totalLength];
        return $"{suffix} {prefix}";
#endif
    }
}