namespace Humanizer;

static class WordsToNumberTokenizer
{
    public static TokenEnumerable Enumerate(string words) => new(words);

    public static string GetLastTokenOrSelf(string words)
    {
        ReadOnlySpan<char> lastToken = default;

        foreach (var token in Enumerate(words))
        {
            lastToken = token;
        }

        return lastToken.IsEmpty ? words : lastToken.ToString();
    }

    public ref struct TokenEnumerable(string words)
    {
        readonly ReadOnlySpan<char> words = words.AsSpan();

        public Enumerator GetEnumerator() => new(words);
    }

    public ref struct Enumerator(ReadOnlySpan<char> remaining)
    {
        ReadOnlySpan<char> remaining = remaining;

        public ReadOnlySpan<char> Current { get; private set; }

        public bool MoveNext()
        {
            while (!remaining.IsEmpty && remaining[0] == ' ')
            {
                remaining = remaining[1..];
            }

            if (remaining.IsEmpty)
            {
                Current = default;
                return false;
            }

            var separatorIndex = remaining.IndexOf(' ');
            if (separatorIndex < 0)
            {
                Current = remaining;
                remaining = [];
                return true;
            }

            Current = remaining[..separatorIndex];
            remaining = remaining[(separatorIndex + 1)..];
            return true;
        }
    }
}
