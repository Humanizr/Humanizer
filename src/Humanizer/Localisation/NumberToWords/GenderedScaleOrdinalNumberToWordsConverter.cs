namespace Humanizer;

/// <summary>
/// Shared renderer for locales that vary units, teens, and ordinal scale behavior by grammatical
/// gender.
///
/// The generated profile stores the parsed gendered lexicon so the runtime can focus on splitting
/// the number into triads and selecting the right scale word for each group.
/// </summary>
class GenderedScaleOrdinalNumberToWordsConverter(GenderedScaleOrdinalNumberToWordsConverter.Profile profile) : GenderedNumberToWordsConverter
{
    readonly GenderedWord[] units = profile.UnitsVariants.Select(static value => GenderedWord.Parse(value, hasNeuter: true)).ToArray();
    readonly GenderedWord[] teens = profile.TeensVariants.Select(static value => GenderedWord.Parse(value, hasNeuter: true)).ToArray();
    readonly GenderedOrdinalWord[] ordinalsUnderTen = profile.OrdinalUnderTenVariants.Select(static value => GenderedOrdinalWord.Parse(value)).ToArray();

    /// <summary>
    /// Converts the given value using the locale's gendered scale rules.
    /// </summary>
    /// <param name="input">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use.</param>
    /// <param name="addAnd">Reserved for compatibility with other converters; this implementation derives conjunction placement from the generated profile.</param>
    /// <returns>The localized cardinal words for <paramref name="input"/>.</returns>
    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        var magnitude = GetAbsoluteValue(input);
        if (magnitude == 0)
        {
            return profile.ZeroWord;
        }

        var parts = new List<string>();
        foreach (var scale in profile.Scales)
        {
            var count = (int)(magnitude / scale.Value);
            if (count == 0)
            {
                continue;
            }

            parts.Add(ConvertScalePart(scale, count));
            magnitude %= scale.Value;
        }

        if (magnitude > 0)
        {
            parts.Add(ConvertTriad((int)magnitude, gender));
        }

        var words = string.Join(" ", parts).Replace("  ", " ");
        return input < 0 ? profile.MinusWord + " " + words : words;
    }

    /// <summary>
    /// Converts the given value to a locale-specific ordinal phrase.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        var magnitude = number == int.MinValue ? (long)int.MaxValue + 1 : Math.Abs(number);
        if (magnitude == 0)
        {
            return profile.ZeroWord;
        }

        if (magnitude == 1)
        {
            var ordinal = ordinalsUnderTen[1].Get(gender);
            return number < 0 ? profile.MinusWord + " " + ordinal : ordinal;
        }

        if (magnitude <= 9)
        {
            // Low ordinals are formed by prefixing the shared ordinal stem to the unit word.
            var ordinal = GetOrdinalPrefix(gender) + " " + ordinalsUnderTen[(int)magnitude].Get(gender);
            return number < 0 ? profile.MinusWord + " " + ordinal : ordinal;
        }

        // Higher ordinals start from the cardinal shape and then apply a sequence of locale-specific
        // terminal edits.
        var words = RemoveTerminalScaleJoiner(Convert(magnitude, gender));

        if (gender == GrammaticalGender.Feminine && words.EndsWith("zeci", StringComparison.Ordinal))
        {
            // Feminine "zeci" forms normalize to "zece" when they stand as the terminal ordinal.
            words = StringHumanizeExtensions.Concat(words.AsSpan(0, words.Length - 4), "zece".AsSpan());
        }
        else if (gender == GrammaticalGender.Feminine && words.Contains("zeci", StringComparison.Ordinal) &&
                 (words.Contains("milioane", StringComparison.Ordinal) || words.Contains("miliarde", StringComparison.Ordinal)))
        {
            // Inside million/billion compounds, the same feminine ordinal ending is expressed as
            // "zecea" rather than the standalone form above.
            words = words.Replace("zeci", "zecea");
        }

        if (gender == GrammaticalGender.Feminine && words.StartsWith("un ", StringComparison.Ordinal))
        {
            // Feminine ordinals drop the leading "un" before the stem is rebuilt.
            words = words.AsSpan(2).TrimStart().ToString();
        }

        if (words.EndsWith("milioane", StringComparison.Ordinal) && gender == GrammaticalGender.Feminine)
        {
            // The million noun changes shape in feminine ordinals, so the terminal noun is rewritten
            // before the suffix is appended.
            words = StringHumanizeExtensions.Concat(words.AsSpan(0, words.Length - 8), "milioana".AsSpan());
        }

        var masculineSuffix = profile.MasculineOrdinalSuffix;
        if (words.EndsWith("milion", StringComparison.Ordinal))
        {
            if (gender == GrammaticalGender.Feminine)
            {
                // Feminine million ordinals keep the same stem rewrite as the plural form above.
                words = StringHumanizeExtensions.Concat(words.AsSpan(0, words.Length - 6), "milioana".AsSpan());
            }
            else
            {
                // Masculine million ordinals need an extra "u" before the generic ordinal suffix.
                masculineSuffix = "u" + masculineSuffix;
            }
        }
        else if (words.EndsWith("miliard", StringComparison.Ordinal) && gender != GrammaticalGender.Feminine)
        {
            // Billion ordinals follow the same masculine suffix extension as million ordinals.
            masculineSuffix = "u" + masculineSuffix;
        }
        else if (words.EndsWith("opt", StringComparison.Ordinal) && gender != GrammaticalGender.Feminine)
        {
            words += "u";
        }

        if (gender == GrammaticalGender.Feminine &&
            !words.EndsWith("zece", StringComparison.Ordinal) &&
            words.Length > 0 &&
            (words[^1] is 'a' or 'ă' or 'e' or 'i'))
        {
            // The terminal vowel is dropped only after all other feminine rewrites have run.
            words = words[..^1];
        }

        var result = GetOrdinalPrefix(gender) + " " + words + GetOrdinalSuffix(gender, masculineSuffix);
        return number < 0 ? profile.MinusWord + " " + result : result;
    }

    string RemoveTerminalScaleJoiner(string words)
    {
        foreach (var scale in profile.Scales)
        {
            var terminalScaleJoiner = " " + profile.JoinAboveTwenty + " " + scale.Plural;
            if (words.EndsWith(terminalScaleJoiner, StringComparison.Ordinal))
            {
                return StringHumanizeExtensions.Concat(
                    words.AsSpan(0, words.Length - terminalScaleJoiner.Length),
                    (" " + scale.Plural).AsSpan());
            }
        }

        return words;
    }

    /// <summary>
    /// Gets the locale-specific ordinal prefix for the requested gender.
    /// </summary>
    string GetOrdinalPrefix(GrammaticalGender gender) =>
        gender == GrammaticalGender.Feminine ? profile.FeminineOrdinalPrefix : profile.MasculineOrdinalPrefix;

    /// <summary>
    /// Gets the locale-specific ordinal suffix for the requested gender.
    /// </summary>
    string GetOrdinalSuffix(GrammaticalGender gender, string masculineSuffix) =>
        gender == GrammaticalGender.Feminine ? profile.FeminineOrdinalSuffix : masculineSuffix;

    /// <summary>
    /// Converts a triad into cardinal words using the supplied gender.
    /// </summary>
    string ConvertTriad(int number, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return string.Empty;
        }

        var tensAndUnits = number % 100;
        var hundreds = number / 100;
        var unitsDigit = tensAndUnits % 10;
        var tensDigit = tensAndUnits / 10;

        var words = HundredsToText(hundreds);
        words += (tensDigit >= 2 ? " " : string.Empty) + profile.TensMap[tensDigit];

        if (tensAndUnits <= 9)
        {
            // The units-only branch preserves gendered one/two variants without forcing a tens word.
            words += " " + units[tensAndUnits].Get(gender);
        }
        else if (tensAndUnits <= 19)
        {
            // Teens are a separate lexicon so they can keep the locale's irregular gendered forms.
            words += " " + teens[tensAndUnits - 10].Get(gender);
        }
        else if (unitsDigit != 0)
        {
            // Compound tens still need the conjunction before the trailing unit.
            words += " " + profile.JoinGroups + " " + units[unitsDigit].Get(gender);
        }

        return words.Trim();
    }

    /// <summary>
    /// Converts a hundreds digit into the locale's hundreds word.
    /// </summary>
    string HundredsToText(int hundreds)
    {
        if (hundreds == 0)
        {
            return string.Empty;
        }

        if (hundreds == 1)
        {
            return profile.FeminineSingular + " sută";
        }

        return units[hundreds].Get(GrammaticalGender.Feminine) + " sute";
    }

    /// <summary>
    /// Converts a higher scale group into cardinal words.
    /// </summary>
    string ConvertScalePart(Scale scale, int number)
    {
        if (number == 1)
        {
            // Singular scale nouns are stored directly because they are the most common ordinal
            // and cardinal spellings for the higher group.
            return scale.Singular;
        }

        var countWords = ConvertTriad(number, scale.CountGender);
        var joiner = number >= 20 ? " " + profile.JoinAboveTwenty : string.Empty;
        return countWords + joiner + " " + scale.Plural;
    }

    static ulong GetAbsoluteValue(long value) =>
        value >= 0 ? (ulong)value : unchecked((ulong)(-(value + 1)) + 1);

    /// <summary>
    /// Parsed gendered word variants for a single lexical item.
    /// </summary>
    readonly record struct GenderedWord(string Masculine, string Feminine, string Neuter)
    {
        /// <summary>
        /// Parses a pipe-delimited gendered value into a structured word.
        /// </summary>
        public static GenderedWord Parse(string value, bool hasNeuter)
        {
            var parts = value.Split('|');
            return parts.Length switch
            {
                >= 3 when hasNeuter => new(parts[0], parts[1], parts[2]),
                2 => new(parts[0], parts[1], parts[0]),
                _ => new(value, value, value)
            };
        }

        /// <summary>
        /// Gets the word for the requested grammatical gender.
        /// </summary>
        public string Get(GrammaticalGender gender) =>
            gender switch
            {
                GrammaticalGender.Feminine => Feminine,
                GrammaticalGender.Neuter => Neuter,
                _ => Masculine
            };
    }

    /// <summary>
    /// Parsed ordinal word variants for a single lexical item.
    /// </summary>
    readonly record struct GenderedOrdinalWord(string Masculine, string Feminine)
    {
        /// <summary>
        /// Parses a pipe-delimited ordinal value into a structured word.
        /// </summary>
        public static GenderedOrdinalWord Parse(string value)
        {
            var parts = value.Split('|');
            return parts.Length >= 2 ? new(parts[0], parts[1]) : new(value, value);
        }

        /// <summary>
        /// Gets the word for the requested grammatical gender.
        /// </summary>
        public string Get(GrammaticalGender gender) =>
            gender == GrammaticalGender.Feminine ? Feminine : Masculine;
    }

    /// <summary>
    /// Immutable generated profile for <see cref="GenderedScaleOrdinalNumberToWordsConverter"/>.
    /// </summary>
    /// <param name="ZeroWord">The cardinal zero word.</param>
    /// <param name="MinusWord">The word used to prefix negative values.</param>
    /// <param name="FeminineSingular">The feminine singular word used by the hundreds renderer.</param>
    /// <param name="MasculineOrdinalPrefix">The ordinal prefix used for masculine and neuter ordinals.</param>
    /// <param name="FeminineOrdinalPrefix">The ordinal prefix used for feminine ordinals.</param>
    /// <param name="MasculineOrdinalSuffix">The default masculine ordinal suffix appended after terminal rewrites.</param>
    /// <param name="FeminineOrdinalSuffix">The default feminine ordinal suffix appended after terminal rewrites.</param>
    /// <param name="JoinGroups">The conjunction inserted between tens and trailing unit words.</param>
    /// <param name="JoinAboveTwenty">The joiner inserted before plural scale nouns for counts of twenty or above.</param>
    /// <param name="UnitsVariants">The pipe-delimited unit variants keyed by digit value.</param>
    /// <param name="TeensVariants">The pipe-delimited teen variants keyed by teen offset.</param>
    /// <param name="TensMap">The tens lexicon keyed by decade value.</param>
    /// <param name="OrdinalUnderTenVariants">The pipe-delimited exact ordinal words for one through nine.</param>
    /// <param name="Scales">The descending scale rows used for higher-order groups.</param>
    public sealed record Profile(
        string ZeroWord,
        string MinusWord,
        string FeminineSingular,
        string MasculineOrdinalPrefix,
        string FeminineOrdinalPrefix,
        string MasculineOrdinalSuffix,
        string FeminineOrdinalSuffix,
        string JoinGroups,
        string JoinAboveTwenty,
        string[] UnitsVariants,
        string[] TeensVariants,
        string[] TensMap,
        string[] OrdinalUnderTenVariants,
        Scale[] Scales);

    /// <summary>
    /// One descending scale row for <see cref="GenderedScaleOrdinalNumberToWordsConverter"/>.
    /// </summary>
    /// <param name="Value">The numeric scale value.</param>
    /// <param name="Singular">The singular scale noun used when the group count is exactly one.</param>
    /// <param name="Plural">The plural scale noun used for counts above one.</param>
    /// <param name="CountGender">The grammatical gender to use when rendering the count that precedes this scale.</param>
    public sealed record Scale(
        ulong Value,
        string Singular,
        string Plural,
        GrammaticalGender CountGender);
}