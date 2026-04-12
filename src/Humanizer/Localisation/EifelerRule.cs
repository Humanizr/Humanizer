namespace Humanizer;

/// <summary>
/// Applies the Luxembourgish Eifeler rule to a word when the following word requires it.
/// </summary>
static class EifelerRule
{
    // The rule applies only when the next word starts with a non-blocking letter. Blocked initials
    // keep the trailing 'n', so the lookup stores the letters that prevent the trim.
    const char Suffix = 'n';
#if NET8_0_OR_GREATER
    static readonly SearchValues<char> BlockingCharacters = SearchValues.Create("unitedzohay");
#else
    const string BlockingCharacters = "unitedzohay";
#endif

    /// <summary>
    /// Removes the trailing <c>n</c> from <paramref name="word"/>.
    /// </summary>
    public static string Apply(string word) => word.TrimEnd(Suffix);

    /// <summary>
    /// Removes the trailing <c>n</c> from <paramref name="word"/> when the following word triggers the rule.
    /// </summary>
    public static string ApplyIfNeeded(string word, string nextWord) =>
        DoesApply(nextWord.AsSpan())
            ? word.TrimEnd(Suffix)
            : word;

    /// <summary>
    /// Determines whether the Eifeler rule applies to <paramref name="nextWord"/>.
    /// </summary>
    public static bool DoesApply(ReadOnlySpan<char> nextWord)
    {
        var trimmed = nextWord.Trim();
        return !trimmed.IsEmpty &&
               !BlockingCharacters.Contains(trimmed[0]);
    }
}