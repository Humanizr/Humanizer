namespace Humanizer
{
    /// <summary>
    /// Contains methods for removing, appending and prepending article prefixes for sorting strings ignoring the article.
    /// </summary>
    public static class EnglishArticle
    {
        static Regex _regex = new("^((The)|(the)|(a)|(A)|(An)|(an))\\s\\w+");

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
                var item = items[i].AsSpan();
                if (_regex.IsMatch(item))
                {
                    var indexOf = item.IndexOf(' ');
                    var removed = item[indexOf..].TrimStart();
                    var article = item[..indexOf].TrimEnd();
                    transformed[i] = $"{removed} {article}";
                }
                else
                {
                    transformed[i] = item.Trim().ToString();
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
                var append = appended[i];
                if (append.EndsWith(EnglishArticles.The.ToString()))
                {
                    var suffix = append[append.IndexOf(" The", StringComparison.CurrentCulture)..];
                    inserted[i] = ToOriginalFormat(append, suffix);
                }
                else if (append.EndsWith(EnglishArticles.A.ToString()))
                {
                    var suffix = append[append.IndexOf(" A", StringComparison.CurrentCulture)..];
                    inserted[i] = ToOriginalFormat(append, suffix);
                }
                else if (append.EndsWith(EnglishArticles.An.ToString()))
                {
                    var suffix = append[append.IndexOf(" An", StringComparison.CurrentCulture)..];
                    inserted[i] = ToOriginalFormat(append, suffix);
                }
                else if (append.EndsWith(EnglishArticles.A.ToString().ToLowerInvariant()))
                {
                    var suffix = append[append.IndexOf(" a", StringComparison.CurrentCulture)..];
                    inserted[i] = ToOriginalFormat(append, suffix);
                }
                else if (append.EndsWith(EnglishArticles.An.ToString().ToLowerInvariant()))
                {
                    var suffix = append[append.IndexOf(" an", StringComparison.CurrentCulture)..];
                    inserted[i] = ToOriginalFormat(append, suffix);
                }
                else if (append.EndsWith(EnglishArticles.The.ToString().ToLowerInvariant()))
                {
                    var suffix = append[append.IndexOf(" the", StringComparison.CurrentCulture)..];
                    inserted[i] = ToOriginalFormat(append, suffix);
                }
                else
                {
                    inserted[i] = append;
                }
            }
            return inserted;
        }

        static string ToOriginalFormat(string append, string suffix)
        {
            var suffixIndex = append.AsSpan().IndexOf(suffix.AsSpan(), StringComparison.CurrentCulture);
            var insertion = append.Remove(suffixIndex);
            suffix = suffix.TrimStart();
            insertion = insertion.TrimEnd();
            return $"{suffix} {insertion}";
        }
    }
}
