namespace Humanizer;

/// <summary>
/// Contains extension methods for dehumanizing strings.
/// </summary>
public static class StringDehumanizeExtensions
{
    /// <summary>
    /// Converts a humanized string back to PascalCase format by splitting on spaces and capitalizing each word.
    /// </summary>
    /// <param name="input">The string to be dehumanized. Must not be null.</param>
    /// <returns>
    /// A PascalCase string formed by joining pascalized words without separators.
    /// If the input contains no non-whitespace characters, it is returned unchanged.
    /// </returns>
    /// <remarks>
    /// This method reverses the humanization process by:
    /// 1. Splitting the input on spaces (empty entries are ignored)
    /// 2. Humanizing each word (to handle any edge cases)
    /// 3. Pascalizing each word (capitalizing first letter)
    /// 4. Concatenating the words without separators
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
        var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (words.Length == 0)
        {
            return input;
        }

        if (words.Length == 1)
        {
            return words[0].Humanize().Pascalize();
        }

        return string.Concat(words.Select(word => word.Humanize().Pascalize()));
    }
}