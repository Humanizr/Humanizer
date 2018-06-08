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
                throw new ArgumentOutOfRangeException(nameof(items));

            Regex regex = new Regex("^((The)|(the)|(a)|(A)|(An)|(an))\\s\\w+");
            string[] transformed = new string[items.Length];
            string article, removed, appended;

            for (int i = 0; i < items.Length; i++)
            {
                if (regex.IsMatch(items[i]))
                {
                    article = items[i].Substring(0, items[i].IndexOf(" "));
                    removed = items[i].Remove(0, items[i].IndexOf(" "));
                    appended = $"{removed} {article}";
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
            string[] inserted = new string[appended.Length];
            string suffix, original;

            for (int i = 0; i < appended.Length; i++)
            {
                if (appended[i].EndsWith(EnglishArticles.The.ToString()))
                {
                    suffix = appended[i].Substring(appended[i].IndexOf(" The"));
                    original = ToOriginalFormat(appended, suffix, i);
                    inserted[i] = original;
                }
                else if (appended[i].EndsWith(EnglishArticles.A.ToString()))
                {
                    suffix = appended[i].Substring(appended[i].IndexOf(" A"));
                    original = ToOriginalFormat(appended, suffix, i);
                    inserted[i] = original;
                }
                else if (appended[i].EndsWith(EnglishArticles.An.ToString()))
                {
                    suffix = appended[i].Substring(appended[i].IndexOf(" An"));
                    original = ToOriginalFormat(appended, suffix, i);
                    inserted[i] = original;
                }
                else if (appended[i].EndsWith(EnglishArticles.a.ToString()))
                {
                    suffix = appended[i].Substring(appended[i].IndexOf(" a"));
                    original = ToOriginalFormat(appended, suffix, i);
                    inserted[i] = original;
                }
                else if (appended[i].EndsWith(EnglishArticles.an.ToString()))
                {
                    suffix = appended[i].Substring(appended[i].IndexOf(" an"));
                    original = ToOriginalFormat(appended, suffix, i);
                    inserted[i] = original;
                }
                else if (appended[i].EndsWith(EnglishArticles.the.ToString()))
                {
                    suffix = appended[i].Substring(appended[i].IndexOf(" the"));
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
            string insertion = appended[i].Remove(appended[i].IndexOf(suffix));
            string original = $"{suffix} {insertion}";
            return original.Trim();
        }
    }
}
