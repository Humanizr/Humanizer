using System;
using System.Text.RegularExpressions;

namespace Humanizer
{
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

            var regex = new Regex("^((The)|(the)|(a)|(A)|(An)|(an))\\s\\w+");
            var transformed = new string[items.Length];

            for (var i = 0; i < items.Length; i++)
            {
                if (regex.IsMatch(items[i]))
                {
                    var article = items[i].Substring(0, items[i].IndexOf(" ", StringComparison.CurrentCulture));
                    var removed = items[i].Remove(0, items[i].IndexOf(" ", StringComparison.CurrentCulture));
                    var appended = $"{removed} {article}";
                    transformed[i] = appended.Trim();
                }
                else
                {
                    transformed[i] = items[i].Trim();
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
                if (appended[i].EndsWith(EnglishArticles.The.ToString()))
                {
                    suffix = appended[i].Substring(appended[i].IndexOf(" The", StringComparison.CurrentCulture));
                    original = ToOriginalFormat(appended, suffix, i);
                    inserted[i] = original;
                }
                else if (appended[i].EndsWith(EnglishArticles.A.ToString()))
                {
                    suffix = appended[i].Substring(appended[i].IndexOf(" A", StringComparison.CurrentCulture));
                    original = ToOriginalFormat(appended, suffix, i);
                    inserted[i] = original;
                }
                else if (appended[i].EndsWith(EnglishArticles.An.ToString()))
                {
                    suffix = appended[i].Substring(appended[i].IndexOf(" An", StringComparison.CurrentCulture));
                    original = ToOriginalFormat(appended, suffix, i);
                    inserted[i] = original;
                }
                else if (appended[i].EndsWith(EnglishArticles.A.ToString().ToLowerInvariant()))
                {
                    suffix = appended[i].Substring(appended[i].IndexOf(" a", StringComparison.CurrentCulture));
                    original = ToOriginalFormat(appended, suffix, i);
                    inserted[i] = original;
                }
                else if (appended[i].EndsWith(EnglishArticles.An.ToString().ToLowerInvariant()))
                {
                    suffix = appended[i].Substring(appended[i].IndexOf(" an", StringComparison.CurrentCulture));
                    original = ToOriginalFormat(appended, suffix, i);
                    inserted[i] = original;
                }
                else if (appended[i].EndsWith(EnglishArticles.The.ToString().ToLowerInvariant()))
                {
                    suffix = appended[i].Substring(appended[i].IndexOf(" the", StringComparison.CurrentCulture));
                    original = ToOriginalFormat(appended, suffix, i);
                    inserted[i] = original;
                }
                else
                {
                    inserted[i] = appended[i];
                }
            }
            return inserted;
        }

        private static string ToOriginalFormat(string[] appended, string suffix, int i)
        {
            var insertion = appended[i].Remove(appended[i].IndexOf(suffix, StringComparison.CurrentCulture));
            var original = $"{suffix} {insertion}";
            return original.Trim();
        }
    }
}
