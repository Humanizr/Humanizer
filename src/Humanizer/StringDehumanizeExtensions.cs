namespace Humanizer;

/// <summary>
/// Contains extension methods for dehumanizing strings.
/// </summary>
public static class StringDehumanizeExtensions
{
    /// <summary>
    /// Converts a humanized string back to PascalCase format by removing spaces and capitalizing each word.
    /// </summary>
    /// <param name="input">The string to be dehumanized. Must not be null.</param>
    /// <returns>
    /// A PascalCase string with all spaces removed and each word capitalized.
    /// If the input is already in PascalCase (contains no spaces), it is returned unchanged.
    /// </returns>
    /// <remarks>
    /// This method reverses the humanization process by:
    /// 1. Splitting the input on spaces
    /// 2. Humanizing each word (to handle any edge cases)
    /// 3. Pascalizing each word (capitalizing first letter)
    /// 4. Removing all spaces
    /// This is the inverse operation of <see cref="StringHumanizeExtensions.Humanize(string)"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// "some string".Dehumanize() => "SomeString"
    /// "Some String".Dehumanize() => "SomeString"
    /// "Some string".Dehumanize() => "SomeString"
    /// "SomeStringAndAnotherString".Dehumanize() => "SomeStringAndAnotherString" // Already dehumanized, returned unchanged
    /// </code>
    /// </example>
    public static string Dehumanize(this string input)
    {
        var pascalizedWords = input
            .Split(' ')
            .Select(word => word
                .Humanize()
                .Pascalize());
        return string
            .Concat(pascalizedWords)
            .Replace(" ", "");
    }
}