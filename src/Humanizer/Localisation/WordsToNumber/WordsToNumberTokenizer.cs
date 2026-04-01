namespace Humanizer;

/// <summary>
/// Splits normalized number phrases on spaces without allocating intermediate token arrays.
/// </summary>
static class WordsToNumberTokenizer
{
    /// <summary>
    /// Returns a span-based tokenizer for the supplied normalized phrase.
    /// </summary>
    /// <param name="words">A whitespace-delimited, already-normalized number phrase.</param>
    /// <returns>A lightweight enumerable over the phrase tokens.</returns>
    public static TokenEnumerable Enumerate(string words) => new(words);

    /// <summary>
    /// Returns the last token in <paramref name="words"/> or the original string when no token
    /// separator is present.
    /// </summary>
    /// <param name="words">The normalized phrase to inspect.</param>
    /// <returns>The last token, or the original string when no token separator exists.</returns>
    public static string GetLastTokenOrSelf(string words)
    {
        ReadOnlySpan<char> lastToken = default;

        foreach (var token in Enumerate(words))
        {
            lastToken = token;
        }

        return lastToken.IsEmpty ? words : lastToken.ToString();
    }

    /// <summary>
    /// Reads the next token from the tokenizer, or returns a pending token when one has already
    /// been buffered by the caller.
    /// </summary>
    /// <param name="enumerator">The tokenizer enumerator.</param>
    /// <param name="pendingToken">An already-read token that should be returned first.</param>
    /// <param name="token">When this method returns, the next token if one exists.</param>
    /// <returns><c>true</c> if a token was produced; otherwise, <c>false</c>.</returns>
    public static bool TryReadNext(ref Enumerator enumerator, ref string? pendingToken, [NotNullWhen(true)] out string? token)
    {
        if (pendingToken != null)
        {
            token = pendingToken;
            pendingToken = null;
            return true;
        }

        if (enumerator.MoveNext())
        {
            token = enumerator.Current.ToString();
            return true;
        }

        token = null;
        return false;
    }

    /// <summary>
    /// Enumerates tokens in a normalized number phrase.
    /// </summary>
    public ref struct TokenEnumerable(string words)
    {
        readonly ReadOnlySpan<char> words = words.AsSpan();

        /// <summary>
        /// Creates an enumerator over the underlying phrase.
        /// </summary>
        public Enumerator GetEnumerator() => new(words);
    }

    /// <summary>
    /// Tokenizes the phrase one space-delimited token at a time.
    /// </summary>
    public ref struct Enumerator(ReadOnlySpan<char> remaining)
    {
        ReadOnlySpan<char> remaining = remaining;

        /// <summary>
        /// Gets the current token span.
        /// </summary>
        public ReadOnlySpan<char> Current { get; private set; }

        /// <summary>
        /// Advances to the next token.
        /// </summary>
        /// <returns><c>true</c> if a token was produced; otherwise, <c>false</c>.</returns>
        public bool MoveNext()
        {
            // The normalizers already collapse punctuation and hyphenation to spaces, so the
            // tokenizer only needs to skip repeated whitespace and slice contiguous spans.
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
