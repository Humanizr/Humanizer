﻿namespace Humanizer
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
                string suffix;
                string original;
                var append = appended[i];
                if (append.EndsWith(EnglishArticles.The.ToString()))
                {
                    suffix = append.Substring(append.IndexOf(" The", StringComparison.CurrentCulture));
                    original = ToOriginalFormat(appended, suffix, i);
                    inserted[i] = original;
                }
                else if (append.EndsWith(EnglishArticles.A.ToString()))
                {
                    suffix = append.Substring(append.IndexOf(" A", StringComparison.CurrentCulture));
                    original = ToOriginalFormat(appended, suffix, i);
                    inserted[i] = original;
                }
                else if (append.EndsWith(EnglishArticles.An.ToString()))
                {
                    suffix = append.Substring(append.IndexOf(" An", StringComparison.CurrentCulture));
                    original = ToOriginalFormat(appended, suffix, i);
                    inserted[i] = original;
                }
                else if (append.EndsWith(EnglishArticles.A.ToString().ToLowerInvariant()))
                {
                    suffix = append.Substring(append.IndexOf(" a", StringComparison.CurrentCulture));
                    original = ToOriginalFormat(appended, suffix, i);
                    inserted[i] = original;
                }
                else if (append.EndsWith(EnglishArticles.An.ToString().ToLowerInvariant()))
                {
                    suffix = append.Substring(append.IndexOf(" an", StringComparison.CurrentCulture));
                    original = ToOriginalFormat(appended, suffix, i);
                    inserted[i] = original;
                }
                else if (append.EndsWith(EnglishArticles.The.ToString().ToLowerInvariant()))
                {
                    suffix = append.Substring(append.IndexOf(" the", StringComparison.CurrentCulture));
                    original = ToOriginalFormat(appended, suffix, i);
                    inserted[i] = original;
                }
                else
                {
                    inserted[i] = append;
                }
            }
            return inserted;
        }

        static string ToOriginalFormat(string[] appended, string suffix, int i)
        {
            var insertion = appended[i].Remove(appended[i].IndexOf(suffix, StringComparison.CurrentCulture));
            var original = $"{suffix} {insertion}";
            return original.Trim();
        }
    }
}
