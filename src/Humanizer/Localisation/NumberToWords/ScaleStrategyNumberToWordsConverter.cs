using System.Text;

namespace Humanizer;

/// <summary>
/// Shared Scandinavian-style scale renderer where the broad decomposition is shared but the exact
/// cardinal and ordinal policies differ by strategy.
///
/// The converter owns the structural walk:
/// - split the number into large scales, thousands, hundreds, tens, and units
/// - keep those segments in a stable order
/// - hand the generated profile the responsibility for exact joiners, plural suffixes, singular
///   one-forms, and ordinal suffix behavior
///
/// The end result should be a locale-correct Scandinavian cardinal or ordinal string without
/// hardcoding locale names into the algorithm itself. Generated strategy enums decide whether the
/// output follows the Norwegian Bokmal family or the Swedish family.
/// </summary>
class ScaleStrategyNumberToWordsConverter(ScaleStrategyNumberToWordsProfile profile)
    : GenderedNumberToWordsConverter(profile.DefaultGender)
{
    /// <summary>
    /// Immutable generated profile that owns the Scandinavian lexical tables and strategy values.
    /// </summary>
    readonly ScaleStrategyNumberToWordsProfile profile = profile;

    /// <inheritdoc/>
    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true) =>
        profile.CardinalStrategy switch
        {
            ScaleStrategyCardinalMode.NorwegianBokmal => ConvertNorwegian(number, gender),
            ScaleStrategyCardinalMode.Swedish => ConvertSwedish(number, profile.DefaultGender),
            _ => throw new InvalidOperationException("Unsupported Scandinavian cardinal strategy.")
        };

    /// <inheritdoc/>
    public override string ConvertToOrdinal(int number, GrammaticalGender gender) =>
        profile.OrdinalStrategy switch
        {
            ScaleStrategyOrdinalMode.NorwegianBokmal => ConvertNorwegianOrdinal(number, gender),
            ScaleStrategyOrdinalMode.Swedish => ConvertSwedishOrdinal(number, profile.DefaultGender),
            _ => throw new InvalidOperationException("Unsupported Scandinavian ordinal strategy.")
        };

    // Norwegian compounds are mostly concatenative, but large-scale spacing and exact ordinals still vary by generated suffix settings.
    string ConvertNorwegian(long number, GrammaticalGender gender)
    {
        if (number is > int.MaxValue or < int.MinValue)
        {
            throw new NotImplementedException();
        }

        return ConvertNorwegian((int)number, false, gender);
    }

    string ConvertNorwegianOrdinal(int number, GrammaticalGender gender) =>
        ConvertNorwegian(number, true, gender);

    // The Norwegian branch is mostly concatenative. The algorithm decides segment order, while the
    // generated profile decides where spaces survive, how exact ordinals terminate, and which
    // singular forms appear for one hundred or one thousand.
    string ConvertNorwegian(int number, bool isOrdinal, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return GetNorwegianUnitValue(0, isOrdinal);
        }

        if (number < 0)
        {
            return $"{profile.MinusWord} {ConvertNorwegian(-number, isOrdinal, gender)}";
        }

        if (!isOrdinal && number == 1)
        {
            return gender switch
            {
                GrammaticalGender.Feminine when !string.IsNullOrEmpty(profile.OneFeminine) => profile.OneFeminine,
                GrammaticalGender.Neuter when !string.IsNullOrEmpty(profile.OneNeuter) => profile.OneNeuter,
                _ => profile.OneDefault
            };
        }

        var parts = new List<string>();
        var hasLargeScale = false;

        // Scandinavian long-scale composition is the part that varies most by locale. The runtime
        // engine keeps the traversal stable while the generated scale records decide spacing,
        // plurality, prefixes, and exact-ordinal suffixes.
        foreach (var scale in profile.Scales)
        {
            if (scale.Value is not (>= 1_000_000 and <= int.MaxValue))
            {
                continue;
            }

            var count = number / (int)scale.Value;
            if (count == 0)
            {
                continue;
            }

            hasLargeScale = true;
            var exactOrdinal = isOrdinal && number % scale.Value == 0;
            parts.Add(FormatNorwegianLargeScale(scale, count, exactOrdinal));
            number %= (int)scale.Value;
        }

        var hasThousand = false;
        var thousands = number / 1000;
        if (thousands > 0)
        {
            hasThousand = true;
            parts.Add(FormatNorwegianThousand(thousands, number % 1000 < 100));
            number %= 1000;
        }

        var hasHundred = false;
        var hundreds = number / 100;
        if (hundreds > 0)
        {
            hasHundred = true;
            parts.Add(FormatNorwegianHundred(hundreds, hasLargeScale || hasThousand));
            number %= 100;
        }

        if (number > 0)
        {
            if (parts.Count != 0)
            {
                parts.Add(hasLargeScale && !hasHundred && !hasThousand
                    ? profile.LargeScaleRemainderJoiner
                    : profile.TensLinker);
            }

            parts.Add(number < 20
                ? GetNorwegianUnitValue(number, isOrdinal)
                : ConvertNorwegianTens(number, isOrdinal));
        }
        else if (isOrdinal)
        {
            parts[^1] += hasLargeScale ? profile.ExactLargeScaleOrdinalSuffix : profile.ExactDefaultOrdinalSuffix;
        }

        return string.Concat(parts).Trim();
    }

    // Large-scale rows already know how their exact ordinal should look, so the algorithm only has
    // to decide whether this scale terminates the full phrase.
    string FormatNorwegianLargeScale(ScaleStrategyScale scale, int count, bool exactOrdinal)
    {
        if (count == 1)
        {
            return exactOrdinal
                ? scale.Name
                : $"{profile.OneDefault} {scale.Name} ";
        }

        var scaleName = exactOrdinal ? scale.Name : scale.Plural;
        var suffix = exactOrdinal ? string.Empty : " ";
        return $"{ConvertNorwegian(count, false, scale.Gender)} {scaleName}{suffix}";
    }

    string FormatNorwegianThousand(int count, bool useStandaloneSingular) =>
        count == 1
            ? useStandaloneSingular
                ? profile.ThousandSingularWord
                : profile.ThousandCompositeSingularWord
            : $"{ConvertNorwegian(count, false, GrammaticalGender.Masculine)}{profile.ThousandWord}";

    string FormatNorwegianHundred(int count, bool useCompositeSingular) =>
        count == 1
            ? useCompositeSingular
                ? profile.HundredCompositeSingularWord
                : profile.HundredWord
            : $"{ConvertNorwegian(count, false, GrammaticalGender.Masculine)}{profile.HundredWord}";

    // Tens are the point where Scandinavian ordinal families diverge the most. The runtime keeps
    // the same shape for exact tens, but the generated profile decides whether the stem is trimmed
    // and which suffix gets appended to produce the final ordinal surface form.
    string ConvertNorwegianTens(int number, bool isOrdinal)
    {
        var lastPart = profile.TensMap[number / 10];
        var unit = number % 10;

        if (unit > 0)
        {
            return lastPart + GetNorwegianUnitValue(unit, isOrdinal);
        }

        return isOrdinal
            ? lastPart.TrimEnd(profile.TensOrdinalTrimEndCharacters.ToCharArray()) + profile.TensOrdinalSuffix
            : lastPart;
    }

    string GetNorwegianUnitValue(int number, bool isOrdinal)
    {
        if (!isOrdinal)
        {
            return profile.UnitsMap[number];
        }

        if (profile.OrdinalExceptions.TryGetValue(number, out var exactValue))
        {
            return exactValue;
        }

        if (number < profile.ShortOrdinalUpperBoundExclusive)
        {
            return profile.UnitsMap[number].TrimEnd(profile.ShortOrdinalTrimEndCharacters.ToCharArray()) + profile.ShortOrdinalTrimmedSuffix;
        }

        return profile.UnitsMap[number] + profile.ShortOrdinalSuffix;
    }

    // Swedish keeps more of its morphology in the generated scale metadata than Norwegian does.
    // This branch is therefore mostly a stitching algorithm: append the emitted scale records in
    // order, then stop at the exact terminal scale when an ordinal is required.
    string ConvertSwedish(long number, GrammaticalGender gender)
    {
        if (number is > int.MaxValue or < int.MinValue)
        {
            throw new NotImplementedException();
        }

        return ConvertSwedish((int)number, false, gender);
    }

    string ConvertSwedishOrdinal(int number, GrammaticalGender gender) =>
        ConvertSwedish(number, true, gender);

    // Exact scale ordinals, plural suffixes, and spacing all come from the generated scale array.
    // The method's job is to determine which scale row terminates the phrase and whether the final
    // under-hundred segment stays cardinal or becomes ordinal.
    string ConvertSwedish(int number, bool isOrdinal, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return isOrdinal
                ? profile.OrdinalExceptions[0]
                : profile.ZeroWord;
        }

        if (number < 0)
        {
            return $"{profile.MinusWord} {ConvertSwedish(-number, isOrdinal, gender)}";
        }

        var word = new StringBuilder();

        // Swedish expresses more of its shape in the generated scale metadata than Norwegian does,
        // so the converter mainly stitches together the emitted scale records and only switches on
        // exact-ordinal termination.
        foreach (var scale in profile.Scales)
        {
            var divided = number / (int)scale.Value;
            if (divided <= 0)
            {
                continue;
            }

            if (divided == 1 && !scale.DisplayOneUnit)
            {
                word.Append(scale.Name);
            }
            else
            {
                word.Append(ConvertSwedish(divided, false, scale.Gender));
                word.Append(scale.Prefix);
                word.Append(scale.Name);
            }

            if (divided > 1 && scale.PluralSuffix.Length != 0)
            {
                word.Append(scale.PluralSuffix);
            }

            number %= (int)scale.Value;

            if (number == 0 && isOrdinal)
            {
                word.Append(scale.OrdinalSuffix);
                return word.ToString();
            }

            if (number > 0)
            {
                word.Append(scale.Postfix);
            }
        }

        if (number == 0)
        {
            return word.ToString();
        }

        if (isOrdinal && profile.OrdinalExceptions.TryGetValue(number, out var exactOrdinal))
        {
            word.Append(exactOrdinal);
            return word.ToString();
        }

        if (number < 20)
        {
            word.Append(GetSwedishUnit(number, gender));
            return word.ToString();
        }

        var tens = profile.TensMap[number / 10];
        var unit = number % 10;
        if (unit == 0)
        {
            if (isOrdinal)
            {
                if (profile.OrdinalExceptions.TryGetValue(number, out var exactTensOrdinal))
                {
                    word.Append(exactTensOrdinal);
                }
                else
                {
                    word.Append(tens);
                    word.Append(profile.TensOrdinalSuffix);
                }

                return word.ToString();
            }

            word.Append(tens);
            return word.ToString();
        }

        word.Append(tens);
        word.Append(isOrdinal
            ? ConvertSwedish(unit, true, gender)
            : GetSwedishUnit(unit, gender));

        return word.ToString();
    }

    string GetSwedishUnit(int number, GrammaticalGender gender) =>
        number == 1 && gender == GrammaticalGender.Masculine
            ? profile.OneMasculine
            : profile.UnitsMap[number];
}

/// <summary>
/// Describes the cardinal composition family used by the Scandinavian shared engine.
/// </summary>
enum ScaleStrategyCardinalMode
{
    /// <summary>Uses the Norwegian Bokmal-style cardinal strategy.</summary>
    NorwegianBokmal,
    /// <summary>Uses the Swedish-style cardinal strategy.</summary>
    Swedish
}

/// <summary>
/// Describes the ordinal composition family used by the Scandinavian shared engine.
/// </summary>
enum ScaleStrategyOrdinalMode
{
    /// <summary>Uses the Norwegian Bokmal-style ordinal strategy.</summary>
    NorwegianBokmal,
    /// <summary>Uses the Swedish-style ordinal strategy.</summary>
    Swedish
}

/// <summary>
/// Immutable generated profile for <see cref="ScaleStrategyNumberToWordsConverter"/>.
/// </summary>
sealed class ScaleStrategyNumberToWordsProfile(
    ScaleStrategyCardinalMode cardinalStrategy,
    ScaleStrategyOrdinalMode ordinalStrategy,
    long maximumValue,
    GrammaticalGender defaultGender,
    string zeroWord,
    string minusWord,
    string oneDefault,
    string oneMasculine,
    string oneFeminine,
    string oneNeuter,
    string tensLinker,
    string largeScaleRemainderJoiner,
    string exactLargeScaleOrdinalSuffix,
    string exactDefaultOrdinalSuffix,
    string tensOrdinalTrimEndCharacters,
    string tensOrdinalSuffix,
    int shortOrdinalUpperBoundExclusive,
    string shortOrdinalTrimEndCharacters,
    string shortOrdinalTrimmedSuffix,
    string shortOrdinalSuffix,
    string hundredWord,
    string hundredCompositeSingularWord,
    string thousandWord,
    string thousandSingularWord,
    string thousandCompositeSingularWord,
    string[] unitsMap,
    string[] tensMap,
    string[] hundredUnitMap,
    ScaleStrategyScale[] scales,
    FrozenDictionary<int, string> ordinalExceptions)
{
    /// <summary>Gets the cardinal composition strategy.</summary>
    public ScaleStrategyCardinalMode CardinalStrategy { get; } = cardinalStrategy;
    /// <summary>Gets the ordinal composition strategy.</summary>
    public ScaleStrategyOrdinalMode OrdinalStrategy { get; } = ordinalStrategy;
    /// <summary>Gets the maximum supported absolute value.</summary>
    public long MaximumValue { get; } = maximumValue;
    /// <summary>Gets the default gender used when callers do not specify one.</summary>
    public GrammaticalGender DefaultGender { get; } = defaultGender;
    /// <summary>Gets the cardinal zero word.</summary>
    public string ZeroWord { get; } = zeroWord;
    /// <summary>Gets the word used to prefix negative values.</summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>Gets the default word for one.</summary>
    public string OneDefault { get; } = oneDefault;
    /// <summary>Gets the masculine word for one.</summary>
    public string OneMasculine { get; } = oneMasculine;
    /// <summary>Gets the feminine word for one.</summary>
    public string OneFeminine { get; } = oneFeminine;
    /// <summary>Gets the neuter word for one.</summary>
    public string OneNeuter { get; } = oneNeuter;
    /// <summary>Gets the joiner used between hundreds and the tens/units remainder.</summary>
    public string TensLinker { get; } = tensLinker;
    /// <summary>Gets the joiner used after a large scale when a small remainder follows.</summary>
    public string LargeScaleRemainderJoiner { get; } = largeScaleRemainderJoiner;
    /// <summary>Gets the suffix appended when an exact large-scale ordinal terminates the phrase.</summary>
    public string ExactLargeScaleOrdinalSuffix { get; } = exactLargeScaleOrdinalSuffix;
    /// <summary>Gets the suffix appended when an exact non-scale ordinal terminates the phrase.</summary>
    public string ExactDefaultOrdinalSuffix { get; } = exactDefaultOrdinalSuffix;
    /// <summary>Gets the characters trimmed from exact tens before appending the ordinal suffix.</summary>
    public string TensOrdinalTrimEndCharacters { get; } = tensOrdinalTrimEndCharacters;
    /// <summary>Gets the ordinal suffix appended to tens after trimming.</summary>
    public string TensOrdinalSuffix { get; } = tensOrdinalSuffix;
    /// <summary>Gets the exclusive upper bound for the short ordinal stem path.</summary>
    public int ShortOrdinalUpperBoundExclusive { get; } = shortOrdinalUpperBoundExclusive;
    /// <summary>Gets the characters trimmed for short unit ordinals.</summary>
    public string ShortOrdinalTrimEndCharacters { get; } = shortOrdinalTrimEndCharacters;
    /// <summary>Gets the suffix appended after short ordinal trimming.</summary>
    public string ShortOrdinalTrimmedSuffix { get; } = shortOrdinalTrimmedSuffix;
    /// <summary>Gets the suffix appended when short ordinals are not trimmed.</summary>
    public string ShortOrdinalSuffix { get; } = shortOrdinalSuffix;
    /// <summary>Gets the hundred word.</summary>
    public string HundredWord { get; } = hundredWord;
    /// <summary>Gets the composite singular hundred word.</summary>
    public string HundredCompositeSingularWord { get; } = hundredCompositeSingularWord;
    /// <summary>Gets the thousand word used in composite phrases.</summary>
    public string ThousandWord { get; } = thousandWord;
    /// <summary>Gets the standalone singular thousand word.</summary>
    public string ThousandSingularWord { get; } = thousandSingularWord;
    /// <summary>Gets the composite singular thousand word.</summary>
    public string ThousandCompositeSingularWord { get; } = thousandCompositeSingularWord;
    /// <summary>Gets the units lexicon.</summary>
    public string[] UnitsMap { get; } = unitsMap;
    /// <summary>Gets the tens lexicon.</summary>
    public string[] TensMap { get; } = tensMap;
    /// <summary>Gets the hundred-unit lexicon used by some cardinal paths.</summary>
    public string[] HundredUnitMap { get; } = hundredUnitMap;
    /// <summary>Gets the descending scale rows used during decomposition.</summary>
    public ScaleStrategyScale[] Scales { get; } = scales;
    /// <summary>Gets exact ordinal overrides that bypass the normal stem-plus-suffix rules.</summary>
    public FrozenDictionary<int, string> OrdinalExceptions { get; } = ordinalExceptions;
}

/// <summary>
/// One descending scale row for <see cref="ScaleStrategyNumberToWordsConverter"/>.
/// </summary>
/// <param name="Value">The divisor for the scale row.</param>
/// <param name="Name">The singular scale name.</param>
/// <param name="Plural">The plural scale name.</param>
/// <param name="Prefix">The prefix inserted before the scale name in concatenative forms.</param>
/// <param name="Postfix">The postfix inserted after the scale name when the locale requires one.</param>
/// <param name="PluralSuffix">The suffix appended when the count is plural.</param>
/// <param name="OrdinalSuffix">The suffix appended when the scale row terminates an ordinal phrase.</param>
/// <param name="DisplayOneUnit">Whether a count of one should still be rendered explicitly.</param>
/// <param name="Gender">The grammatical gender used for the scale row.</param>
readonly record struct ScaleStrategyScale(
    long Value,
    string Name,
    string Plural,
    string Prefix,
    string Postfix,
    string PluralSuffix,
    string OrdinalSuffix,
    bool DisplayOneUnit,
    GrammaticalGender Gender);
