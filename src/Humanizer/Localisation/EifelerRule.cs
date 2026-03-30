namespace Humanizer;

static class EifelerRule
{
    const char Suffix = 'n';
#if NET8_0_OR_GREATER
    static readonly SearchValues<char> BlockingCharacters = SearchValues.Create("unitedzohay");
#else
    const string BlockingCharacters = "unitedzohay";
#endif

    public static string Apply(string word) => word.TrimEnd(Suffix);

    public static string ApplyIfNeeded(string word, string nextWord) =>
        DoesApply(nextWord.AsSpan())
            ? word.TrimEnd(Suffix)
            : word;

    public static bool DoesApply(ReadOnlySpan<char> nextWord)
    {
        var trimmed = nextWord.Trim();
        return !trimmed.IsEmpty &&
               !BlockingCharacters.Contains(trimmed[0]);
    }
}
