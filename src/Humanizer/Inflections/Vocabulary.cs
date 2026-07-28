namespace Humanizer;

/// <summary>
/// A container for exceptions to simple pluralization/singularization rules.
/// Vocabularies.Default contains an extensive list of rules for US English.
/// At this time, multiple vocabularies and removing existing rules are not supported.
/// </summary>
public partial class Vocabulary
{
    internal Vocabulary()
    {
    }

    readonly List<Rule> plurals = [];
    readonly List<Rule> singulars = [];
    readonly HashSet<string> uncountables = new(StringComparer.CurrentCultureIgnoreCase);

    private const string LetterSPattern = "^([sS])[sS]*$";

#if NET7_0_OR_GREATER
    [GeneratedRegex(LetterSPattern)]
    private static partial Regex LetterSRegexGenerated();

    private static Regex LetterSRegex() => LetterSRegexGenerated();
#else
    private static readonly Regex LetterSRegexField = new(LetterSPattern, RegexOptions.Compiled);

    private static Regex LetterSRegex() => LetterSRegexField;
#endif

    /// <summary>
    /// Adds a word to the vocabulary which cannot easily be pluralized/singularized by RegEx, e.g. "person" and "people".
    /// </summary>
    /// <param name="singular">The singular form of the irregular word, e.g. "person".</param>
    /// <param name="plural">The plural form of the irregular word, e.g. "people".</param>
    /// <param name="matchEnding">True to match these words on their own as well as at the end of longer words. False, otherwise.</param>
    public void AddIrregular(string singular, string plural, bool matchEnding = true)
    {
        if (matchEnding)
        {
            var singularSubstring = singular[1..];
            var pluralSubString = plural[1..];
            AddPlural($"({singular[0]}){singularSubstring}$", $"$1{pluralSubString}");
            AddSingular($"({plural[0]}){pluralSubString}$", $"$1{singularSubstring}");
        }
        else
        {
            AddPlural($"^{singular}$", plural);
            AddSingular($"^{plural}$", singular);
        }
    }

    /// <summary>
    /// Adds an uncountable word to the vocabulary, e.g. "fish".  Will be ignored when plurality is changed.
    /// </summary>
    /// <param name="word">Word to be added to the list of uncountables.</param>
    public void AddUncountable(string word) =>
        uncountables.Add(word);

    /// <summary>
    /// Adds a rule to the vocabulary that does not follow trivial rules for pluralization, e.g. "bus" -> "buses"
    /// </summary>
    /// <param name="rule">RegEx to be matched, case insensitive, e.g. "(bus)es$"</param>
    /// <param name="replacement">RegEx replacement  e.g. "$1"</param>
    public void AddPlural(string rule, string replacement) =>
        plurals.Add(new(rule, replacement));

    /// <summary>
    /// Adds a rule to the vocabulary that does not follow trivial rules for singularization, e.g. "vertices/indices -> "vertex/index"
    /// </summary>
    /// <param name="rule">RegEx to be matched, case insensitive, e.g. ""(vert|ind)ices$""</param>
    /// <param name="replacement">RegEx replacement  e.g. "$1ex"</param>
    public void AddSingular(string rule, string replacement) =>
        singulars.Add(new(rule, replacement));

    internal void MarkRulesAsBuiltIn()
    {
        foreach (var rule in plurals)
        {
            rule.IsBuiltIn = true;
        }

        foreach (var rule in singulars)
        {
            rule.IsBuiltIn = true;
        }
    }

    /// <summary>
    /// Pluralizes the provided input considering irregular words
    /// </summary>
    /// <param name="word">Word to be pluralized</param>
    /// <param name="inputIsKnownToBeSingular">Normally you call Pluralize on singular words; but if you're unsure call it with false</param>
    [return: NotNullIfNotNull(nameof(word))]
    public string? Pluralize(string? word, bool inputIsKnownToBeSingular = true)
    {
        if (word == null)
        {
            return null;
        }

        var s = LetterS(word);
        if (s != null)
        {
            return s + "s";
        }

        var result = ApplyRules(plurals, word, false, out var wholeWordMatch);

        var compoundHeadLength = CompoundHeadLength(word);
        if (compoundHeadLength > 0 && !wholeWordMatch)
        {
            return Pluralize(word[..compoundHeadLength], inputIsKnownToBeSingular) + word[compoundHeadLength..];
        }

        if (inputIsKnownToBeSingular)
        {
            return result ?? word;
        }

        var asSingular = ApplyRules(singulars, word, false);
        var asSingularAsPlural = ApplyRules(plurals, asSingular, false);
        if (asSingular != null &&
            asSingular != word &&
            !string.Equals(asSingular + "s", word, StringComparison.OrdinalIgnoreCase) &&
            asSingularAsPlural == word &&
            result != word)
        {
            return word;
        }

        return result!;
    }

    /// <summary>
    /// Singularizes the provided input considering irregular words
    /// </summary>
    /// <param name="word">Word to be singularized</param>
    /// <param name="inputIsKnownToBePlural">Normally you call Singularize on plural words; but if you're unsure call it with false</param>
    /// <param name="skipSimpleWords">Skip singularizing single words that have an 's' on the end</param>
    [return: NotNullIfNotNull(nameof(word))]
    public string? Singularize(string? word, bool inputIsKnownToBePlural = true, bool skipSimpleWords = false)
    {
        if (word == null)
        {
            return null;
        }
        var s = LetterS(word);
        if (s != null)
        {
            return s;
        }

        var result = ApplyRules(singulars, word, skipSimpleWords, out var wholeWordMatch);

        var compoundHeadLength = CompoundHeadLength(word);
        if (compoundHeadLength > 0 && !wholeWordMatch)
        {
            return Singularize(word[..compoundHeadLength], inputIsKnownToBePlural, skipSimpleWords) + word[compoundHeadLength..];
        }

        if (inputIsKnownToBePlural)
        {
            return result ?? word;
        }

        // the Plurality is unknown so we should check all possibilities
        var asPlural = ApplyRules(plurals, word, false);
        if (asPlural == word ||
            string.Equals(word + "s", asPlural, StringComparison.OrdinalIgnoreCase))
        {
            return result ?? word;
        }

        var asPluralAsSingular = ApplyRules(singulars, asPlural, false);
        if (asPluralAsSingular != word ||
            result == word)
        {
            return result ?? word;
        }

        return word;
    }

    string? ApplyRules(IList<Rule> rules, string? word, bool skipFirstRule)
    {
        return ApplyRules(rules, word, skipFirstRule, out _);
    }

    string? ApplyRules(IList<Rule> rules, string? word, bool skipFirstRule, out bool wholeWordMatch)
    {
        wholeWordMatch = false;

        if (word == null)
        {
            return null;
        }

        if (word.Length < 1)
        {
            return word;
        }

        if (IsUncountable(word))
        {
            wholeWordMatch = true;
            return word;
        }

        var result = word;
        var end = skipFirstRule ? 1 : 0;
        for (var i = rules.Count - 1; i >= end; i--)
        {
            if ((result = rules[i].Apply(word, out wholeWordMatch)) != null)
            {
                break;
            }
        }

        if (result == null)
        {
            return null;
        }

        return MatchUpperCase(word, result);
    }

    bool IsUncountable(string word) =>
        uncountables.Contains(word);

    static int CompoundHeadLength(string word)
    {
        for (var i = 1; i < word.Length - 3; i++)
        {
            if (!IsHorizontalWhitespace(word[i - 1]) ||
                word[i] is not ('p' or 'P') ||
                word[i + 1] is not ('e' or 'E') ||
                word[i + 2] is not ('r' or 'R') ||
                !IsHorizontalWhitespace(word[i + 3]))
            {
                continue;
            }

            var headLength = i - 1;
            while (headLength > 0 && IsHorizontalWhitespace(word[headLength - 1]))
            {
                headLength--;
            }

            var complementStart = i + 4;
            while (complementStart < word.Length && IsHorizontalWhitespace(word[complementStart]))
            {
                complementStart++;
            }

            if (headLength > 0 && complementStart < word.Length && !EndsWithAs(word, headLength))
            {
                return headLength;
            }
        }

        return 0;
    }

    static bool EndsWithAs(string word, int length) =>
        length >= 2 &&
        word[length - 2] is 'a' or 'A' &&
        word[length - 1] is 's' or 'S' &&
        (length == 2 || !char.IsLetterOrDigit(word[length - 3]));

    static bool IsHorizontalWhitespace(char character) =>
        character == '\t' || char.GetUnicodeCategory(character) == UnicodeCategory.SpaceSeparator;

    static string MatchUpperCase(string word, string replacement) =>
        word.Length > 1 && word.Any(char.IsUpper) && !word.Any(char.IsLower)
            ? replacement.ToUpperInvariant()
            : char.IsUpper(word[0]) && char.IsLower(replacement[0])
                ? StringHumanizeExtensions.Concat(char.ToUpper(replacement[0]), replacement.AsSpan(1))
                : replacement;

    /// <summary>
    /// If the word is the letter s, singular or plural, return the letter s singular
    /// </summary>
    static string? LetterS(string word)
    {
        var s = LetterSRegex().Match(word);
        return s.Groups.Count > 1 ? s.Groups[1].Value : null;
    }

    class Rule(string pattern, string replacement)
    {
        readonly Regex regex = new(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public bool IsBuiltIn { get; set; }

        public string? Apply(string word, out bool wholeWordMatch)
        {
            var match = regex.Match(word);
            if (!match.Success)
            {
                wholeWordMatch = false;
                return null;
            }

            wholeWordMatch = !IsBuiltIn && match.Index == 0 && match.Length == word.Length;
            return regex.Replace(word, replacement);
        }
    }
}