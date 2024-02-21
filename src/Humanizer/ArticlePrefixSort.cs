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

            // ReSharper disable InconsistentNaming
            var The = "The".AsSpan();
            var _The = " The".AsSpan();
            var A = "A".AsSpan();
            var _A = " A".AsSpan();
            var An = "An".AsSpan();
            var _An = " An".AsSpan();
            var a = "a".AsSpan();
            var _a = " a".AsSpan();
            var an = "an".AsSpan();
            var _an = " an".AsSpan();
            var the = "the".AsSpan();
            var _the = " the".AsSpan();
            // ReSharper restore InconsistentNaming
            for (var i = 0; i < appended.Length; i++)
            {
                var append = appended[i].AsSpan();
                if (append.EndsWith(The))
                {
                    var suffix = append[append.IndexOf(_The, StringComparison.CurrentCulture)..];
                    inserted[i] = ToOriginalFormat(append, suffix);
                }
                else if (append.EndsWith(A))
                {
                    var suffix = append[append.IndexOf(_A, StringComparison.CurrentCulture)..];
                    inserted[i] = ToOriginalFormat(append, suffix);
                }
                else if (append.EndsWith(An))
                {
                    var suffix = append[append.IndexOf(_An, StringComparison.CurrentCulture)..];
                    inserted[i] = ToOriginalFormat(append, suffix);
                }
                else if (append.EndsWith(a))
                {
                    var suffix = append[append.IndexOf(_a, StringComparison.CurrentCulture)..];
                    inserted[i] = ToOriginalFormat(append, suffix);
                }
                else if (append.EndsWith(an))
                {
                    var suffix = append[append.IndexOf(_an, StringComparison.CurrentCulture)..];
                    inserted[i] = ToOriginalFormat(append, suffix);
                }
                else if (append.EndsWith(the))
                {
                    var suffix = append[append.IndexOf(_the, StringComparison.CurrentCulture)..];
                    inserted[i] = ToOriginalFormat(append, suffix);
                }
                else
                {
                    inserted[i] = append.ToString();
                }
            }
            return inserted;
        }

        static string ToOriginalFormat(ReadOnlySpan<char> append, ReadOnlySpan<char> suffix)
        {
            var suffixIndex = append.IndexOf(suffix, StringComparison.CurrentCulture);
            var insertion = append[..suffixIndex];
            suffix = suffix.TrimStart();
            insertion = insertion.TrimEnd();
            return $"{suffix} {insertion}";
        }
    }
}
