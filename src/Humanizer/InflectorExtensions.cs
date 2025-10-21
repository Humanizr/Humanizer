//The Inflector class was cloned from Inflector (https://github.com/srkirkland/Inflector)

//The MIT License (MIT)

//Copyright (c) 2013 Scott Kirkland

//Permission is hereby granted, free of charge, to any person obtaining a copy of
//this software and associated documentation files (the "Software"), to deal in
//the Software without restriction, including without limitation the rights to
//use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//the Software, and to permit persons to whom the Software is furnished to do so,
//subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Humanizer;

public static partial class InflectorExtensions
{
#if NET7_0_OR_GREATER
    [GeneratedRegex(@"(?:[ _-]+|^)([a-zA-Z])")]
    private static partial Regex PascalizeRegexGenerated();

    private static Regex PascalizeRegex() => PascalizeRegexGenerated();

    [GeneratedRegex(@"([\p{Lu}]+)([\p{Lu}][\p{Ll}])")]
    private static partial Regex UnderscoreRegex1Generated();

    private static Regex UnderscoreRegex1() => UnderscoreRegex1Generated();

    [GeneratedRegex(@"([\p{Ll}\d])([\p{Lu}])")]
    private static partial Regex UnderscoreRegex2Generated();

    private static Regex UnderscoreRegex2() => UnderscoreRegex2Generated();

    [GeneratedRegex(@"[-\s]")]
    private static partial Regex UnderscoreRegex3Generated();

    private static Regex UnderscoreRegex3() => UnderscoreRegex3Generated();
#else
    private static readonly Regex PascalizeRegexField = new(@"(?:[ _-]+|^)([a-zA-Z])", RegexOptions.Compiled);
    private static Regex PascalizeRegex() => PascalizeRegexField;

    private static readonly Regex UnderscoreRegex1Field = new(@"([\p{Lu}]+)([\p{Lu}][\p{Ll}])", RegexOptions.Compiled);
    private static Regex UnderscoreRegex1() => UnderscoreRegex1Field;

    private static readonly Regex UnderscoreRegex2Field = new(@"([\p{Ll}\d])([\p{Lu}])", RegexOptions.Compiled);
    private static Regex UnderscoreRegex2() => UnderscoreRegex2Field;

    private static readonly Regex UnderscoreRegex3Field = new(@"[-\s]", RegexOptions.Compiled);
    private static Regex UnderscoreRegex3() => UnderscoreRegex3Field;
#endif

    /// <summary>
    /// Pluralizes the provided input considering irregular words
    /// </summary>
    /// <param name="word">Word to be pluralized</param>
    /// <param name="inputIsKnownToBeSingular">Normally you call Pluralize on singular words; but if you're unsure call it with false</param>
    [return: NotNullIfNotNull(nameof(word))]
    public static string? Pluralize(this string? word, bool inputIsKnownToBeSingular = true) =>
        Vocabularies.Default.Pluralize(word, inputIsKnownToBeSingular);

    /// <summary>
    /// Singularizes the provided input considering irregular words
    /// </summary>
    /// <param name="word">Word to be singularized</param>
    /// <param name="inputIsKnownToBePlural">Normally you call Singularize on plural words; but if you're unsure call it with false</param>
    /// <param name="skipSimpleWords">Skip singularizing single words that have an 's' on the end</param>
    public static string Singularize(this string word, bool inputIsKnownToBePlural = true, bool skipSimpleWords = false) =>
        Vocabularies.Default.Singularize(word, inputIsKnownToBePlural, skipSimpleWords);

    extension(string input)
    {
        /// <summary>
        /// Humanizes the input with Title casing
        /// </summary>
        public string Titleize()
        {
            var humanized = input.Humanize();
            // If humanization returns empty string (no recognized letters), preserve original input
            return humanized.Length == 0 ? input : humanized.ApplyCase(LetterCasing.Title);
        }

        /// <summary>
        /// By default, pascalize converts strings to UpperCamelCase also removing underscores
        /// </summary>
        public string Pascalize() =>
            PascalizeRegex().Replace(input, match => match
                .Groups[1]
                .Value.ToUpper(CultureInfo.CurrentUICulture));

        /// <summary>
        /// Same as Pascalize except that the first character is lower case
        /// </summary>
        public string Camelize()
        {
            var word = input.Pascalize();
            return word.Length > 0
                ? StringHumanizeExtensions.Concat(
                    char.ToLower(word[0]),
                    word.AsSpan(1))
                : word;
        }

        /// <summary>
        /// Separates the input words with underscore
        /// </summary>
        public string Underscore() =>
            UnderscoreRegex3()
                .Replace(
                    UnderscoreRegex2().Replace(
                        UnderscoreRegex1().Replace(input, "$1_$2"), "$1_$2"), "_")
                .ToLower(CultureInfo.CurrentUICulture);

        /// <summary>
        /// Separates the input words with hyphens and all the words are converted to lowercase
        /// </summary>
        public string Kebaberize() =>
            Underscore(input)
                .Dasherize();
    }

    /// <summary>
    /// Replaces underscores with dashes in the string
    /// </summary>
    public static string Dasherize(this string underscoredWord) =>
        underscoredWord.Replace('_', '-');

    /// <summary>
    /// Replaces underscores with hyphens in the string
    /// </summary>
    public static string Hyphenate(this string underscoredWord) =>
        Dasherize(underscoredWord);
}