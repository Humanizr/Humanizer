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
    private const string PascalizePattern = @"(?:[ _-]+|^)([a-zA-Z])";
    private const string UnderscorePattern1 = @"([\p{Lu}]+)([\p{Lu}][\p{Ll}])";
    private const string UnderscorePattern2 = @"([\p{Ll}\d])([\p{Lu}])";
    private const string UnderscorePattern3 = @"[-\s]";

#if NET7_0_OR_GREATER
    [GeneratedRegex(PascalizePattern)]
    private static partial Regex PascalizeRegexGenerated();
    
    private static Regex PascalizeRegex() => PascalizeRegexGenerated();

    [GeneratedRegex(UnderscorePattern1)]
    private static partial Regex UnderscoreRegex1Generated();
    
    private static Regex UnderscoreRegex1() => UnderscoreRegex1Generated();

    [GeneratedRegex(UnderscorePattern2)]
    private static partial Regex UnderscoreRegex2Generated();
    
    private static Regex UnderscoreRegex2() => UnderscoreRegex2Generated();

    [GeneratedRegex(UnderscorePattern3)]
    private static partial Regex UnderscoreRegex3Generated();
    
    private static Regex UnderscoreRegex3() => UnderscoreRegex3Generated();
#else
    private static readonly Regex PascalizeRegexField = new(PascalizePattern, RegexOptions.Compiled);
    private static Regex PascalizeRegex() => PascalizeRegexField;

    private static readonly Regex UnderscoreRegex1Field = new(UnderscorePattern1, RegexOptions.Compiled);
    private static Regex UnderscoreRegex1() => UnderscoreRegex1Field;

    private static readonly Regex UnderscoreRegex2Field = new(UnderscorePattern2, RegexOptions.Compiled);
    private static Regex UnderscoreRegex2() => UnderscoreRegex2Field;

    private static readonly Regex UnderscoreRegex3Field = new(UnderscorePattern3, RegexOptions.Compiled);
    private static Regex UnderscoreRegex3() => UnderscoreRegex3Field;
#endif

    /// <summary>
    /// Converts a singular word to its plural form, handling both regular and irregular pluralizations.
    /// </summary>
    /// <param name="word">The word to be pluralized. Can be null.</param>
    /// <param name="inputIsKnownToBeSingular">
    /// Indicates whether the input is known to be in singular form. 
    /// Set to true (default) if you're certain the word is singular.
    /// Set to false if the word might already be plural, in which case the method will check and avoid double-pluralization.
    /// </param>
    /// <returns>
    /// The plural form of the word, or null if the input was null.
    /// Handles irregular plurals (e.g., "person" → "people", "child" → "children") and regular plurals (e.g., "cat" → "cats").
    /// </returns>
    /// <remarks>
    /// Uses the default vocabulary which includes English pluralization rules and common irregular forms.
    /// </remarks>
    /// <example>
    /// <code>
    /// "person".Pluralize() => "people"
    /// "cat".Pluralize() => "cats"
    /// "box".Pluralize() => "boxes"
    /// "man".Pluralize() => "men"
    /// "cats".Pluralize(inputIsKnownToBeSingular: false) => "cats" (avoids double pluralization)
    /// </code>
    /// </example>
    [return: NotNullIfNotNull(nameof(word))]
    public static string? Pluralize(this string? word, bool inputIsKnownToBeSingular = true) =>
        Vocabularies.Default.Pluralize(word, inputIsKnownToBeSingular);

    /// <summary>
    /// Converts a plural word to its singular form, handling both regular and irregular singularizations.
    /// </summary>
    /// <param name="word">The word to be singularized. Must not be null.</param>
    /// <param name="inputIsKnownToBePlural">
    /// Indicates whether the input is known to be in plural form.
    /// Set to true (default) if you're certain the word is plural.
    /// Set to false if the word might already be singular, in which case the method will check and avoid incorrect singularization.
    /// </param>
    /// <param name="skipSimpleWords">
    /// When true, skips singularization of simple words that just end in 's'.
    /// This helps avoid incorrectly singularizing words like "ross" to "ros".
    /// </param>
    /// <returns>
    /// The singular form of the word.
    /// Handles irregular singulars (e.g., "people" → "person", "children" → "child") and regular singulars (e.g., "cats" → "cat").
    /// </returns>
    /// <remarks>
    /// Uses the default vocabulary which includes English singularization rules and common irregular forms.
    /// </remarks>
    /// <example>
    /// <code>
    /// "people".Singularize() => "person"
    /// "cats".Singularize() => "cat"
    /// "boxes".Singularize() => "box"
    /// "men".Singularize() => "man"
    /// "person".Singularize(inputIsKnownToBePlural: false) => "person" (avoids incorrect singularization)
    /// </code>
    /// </example>
    public static string Singularize(this string word, bool inputIsKnownToBePlural = true, bool skipSimpleWords = false) =>
        Vocabularies.Default.Singularize(word, inputIsKnownToBePlural, skipSimpleWords);

    /// <summary>
    /// Converts a string to title case by humanizing it first and then applying title casing.
    /// Each word in the result will start with an uppercase letter.
    /// </summary>
    /// <param name="input">The string to be converted to title case. Must not be null.</param>
    /// <returns>
    /// A humanized string with each word capitalized (title case).
    /// If humanization produces an empty string, returns the original input unchanged.
    /// </returns>
    /// <remarks>
    /// This method first humanizes the input (breaking up PascalCase, underscores, etc.) and then applies title casing.
    /// </remarks>
    /// <example>
    /// <code>
    /// "some_title".Titleize() => "Some Title"
    /// "someTitle".Titleize() => "Some Title"
    /// "some-package_name".Titleize() => "Some Package Name"
    /// </code>
    /// </example>
    public static string Titleize(this string input)
    {
        var humanized = input.Humanize();
        // If humanization returns empty string (no recognized letters), preserve original input
        return humanized.Length == 0 ? input : humanized.ApplyCase(LetterCasing.Title);
    }

    /// <summary>
    /// Converts a string to PascalCase (UpperCamelCase) by capitalizing the first letter of each word
    /// and removing spaces, underscores, and dashes.
    /// </summary>
    /// <param name="input">The string to be pascalized. Must not be null.</param>
    /// <returns>
    /// A PascalCase version of the input where each word starts with an uppercase letter and 
    /// spaces, underscores, and dashes are removed.
    /// </returns>
    /// <remarks>
    /// PascalCase (also known as UpperCamelCase) is commonly used for class names and type names in .NET.
    /// </remarks>
    /// <example>
    /// <code>
    /// "some_property_name".Pascalize() => "SomePropertyName"
    /// "some property name".Pascalize() => "SomePropertyName"
    /// "some-property-name".Pascalize() => "SomePropertyName"
    /// </code>
    /// </example>
    public static string Pascalize(this string input) =>
        PascalizeRegex().Replace(input, match => match
            .Groups[1]
            .Value.ToUpper());

    /// <summary>
    /// Converts a string to camelCase (lowerCamelCase) by capitalizing the first letter of each word
    /// except the first word, and removing spaces, underscores, and dashes.
    /// </summary>
    /// <param name="input">The string to be camelized. Must not be null.</param>
    /// <returns>
    /// A camelCase version of the input where the first word starts with a lowercase letter,
    /// subsequent words start with uppercase letters, and spaces, underscores, and dashes are removed.
    /// </returns>
    /// <remarks>
    /// camelCase is the same as PascalCase except the first character is lowercase.
    /// It's commonly used for variable and method parameter names in .NET.
    /// </remarks>
    /// <example>
    /// <code>
    /// "some_property_name".Camelize() => "somePropertyName"
    /// "some property name".Camelize() => "somePropertyName"
    /// "SomePropertyName".Camelize() => "somePropertyName"
    /// </code>
    /// </example>
    public static string Camelize(this string input)
    {
        var word = input.Pascalize();
        return word.Length > 0
            ? StringHumanizeExtensions.Concat(
                char.ToLower(word[0]),
                word.AsSpan(1))
            : word;
    }

    /// <summary>
    /// Converts a string to lowercase and separates words with underscores, transforming 
    /// PascalCase, camelCase, and spaces into underscore_case.
    /// </summary>
    /// <param name="input">The string to be underscored. Must not be null.</param>
    /// <returns>
    /// A lowercase string with words separated by underscores instead of spaces, case changes, or dashes.
    /// </returns>
    /// <remarks>
    /// This transformation is commonly used for database column names, file names, and URL slugs in some conventions.
    /// </remarks>
    /// <example>
    /// <code>
    /// "SomePropertyName".Underscore() => "some_property_name"
    /// "somePropertyName".Underscore() => "some_property_name"
    /// "some-property-name".Underscore() => "some_property_name"
    /// "some property name".Underscore() => "some_property_name"
    /// </code>
    /// </example>
    public static string Underscore(this string input) =>
        UnderscoreRegex3()
            .Replace(
                UnderscoreRegex2().Replace(
                    UnderscoreRegex1().Replace(input, "$1_$2"), "$1_$2"), "_")
            .ToLower();

    /// <summary>
    /// Replaces all underscores in the string with dashes (hyphens).
    /// </summary>
    /// <param name="underscoredWord">The string containing underscores to be replaced with dashes. Must not be null.</param>
    /// <returns>
    /// A string with all underscores replaced by dashes.
    /// </returns>
    /// <remarks>
    /// This is typically used after calling <see cref="Underscore"/> to convert from underscore_case to dash-case (kebab-case).
    /// </remarks>
    /// <example>
    /// <code>
    /// "some_property_name".Dasherize() => "some-property-name"
    /// "some_longer_property_name".Dasherize() => "some-longer-property-name"
    /// </code>
    /// </example>
    public static string Dasherize(this string underscoredWord) =>
        underscoredWord.Replace('_', '-');

    /// <summary>
    /// Replaces all underscores in the string with hyphens. This is an alias for <see cref="Dasherize"/>.
    /// </summary>
    /// <param name="underscoredWord">The string containing underscores to be replaced with hyphens. Must not be null.</param>
    /// <returns>
    /// A string with all underscores replaced by hyphens.
    /// </returns>
    /// <remarks>
    /// This method is functionally identical to <see cref="Dasherize"/> and is provided for API clarity.
    /// </remarks>
    /// <example>
    /// <code>
    /// "some_property_name".Hyphenate() => "some-property-name"
    /// </code>
    /// </example>
    public static string Hyphenate(this string underscoredWord) =>
        Dasherize(underscoredWord);

    /// <summary>
    /// Converts a string to kebab-case (lowercase words separated by hyphens), transforming
    /// PascalCase, camelCase, spaces, and underscores into hyphenated lowercase text.
    /// </summary>
    /// <param name="input">The string to be converted to kebab-case. Must not be null.</param>
    /// <returns>
    /// A lowercase string with words separated by hyphens.
    /// </returns>
    /// <remarks>
    /// Kebab-case is commonly used for CSS class names, HTML attributes, and URL slugs.
    /// This is equivalent to calling <see cref="Underscore"/> followed by <see cref="Dasherize"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// "SomePropertyName".Kebaberize() => "some-property-name"
    /// "somePropertyName".Kebaberize() => "some-property-name"
    /// "some property name".Kebaberize() => "some-property-name"
    /// "some_property_name".Kebaberize() => "some-property-name"
    /// </code>
    /// </example>
    public static string Kebaberize(this string input) =>
        Underscore(input)
            .Dasherize();
}