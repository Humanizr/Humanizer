namespace Humanizer;

/// <summary>
/// Renders South Slavic cardinal numbers with generated scale-form detection and gendered unit
/// overrides.
/// </summary>
class SouthSlavicCardinalNumberToWordsConverter(SouthSlavicCardinalNumberToWordsProfile profile, CultureInfo culture) : GenderlessNumberToWordsConverter
{
    readonly SouthSlavicCardinalNumberToWordsProfile profile = profile;

    /// <inheritdoc/>
    public override string Convert(long input)
    {
        // The generated profile owns the supported magnitude ceiling, with an extra slot only when
        // the locale explicitly allows `long.MinValue` to survive the absolute-value conversion.
        if (GetAbsoluteValue(input) > profile.MaximumValue &&
            !(profile.AllowLongMin && input == long.MinValue))
        {
            throw new NotImplementedException();
        }

        if (input == 0)
        {
            return profile.ZeroWord;
        }

        var parts = new List<string>(8);
        var remaining = GetAbsoluteValue(input);

        if (input < 0)
        {
            parts.Add(profile.MinusWord);
        }

        AppendPositive(parts, remaining, GrammaticalGender.Masculine);

        return string.Join(" ", parts);
    }

    /// <inheritdoc/>
    public override string ConvertToOrdinal(int number) =>
        number.ToString(culture);

    /// <summary>
    /// Appends the positive magnitude using the configured scale rows.
    /// </summary>
    void AppendPositive(List<string> parts, ulong number, GrammaticalGender gender)
    {
        foreach (var scale in profile.Scales)
        {
            var count = number / scale.Value;
            if (count == 0)
            {
                continue;
            }

            // `count` itself may need the same grammatical-number walk as the top-level value, so
            // recurse rather than trying to splice digits directly.
            AppendScale(parts, count, scale);
            number %= scale.Value;
        }

        if (number != 0)
        {
            AppendUnderOneThousand(parts, (int)number, gender);
        }
    }

    /// <summary>
    /// Appends one scale row and selects the correct grammatical-number form.
    /// </summary>
    void AppendScale(List<string> parts, ulong count, SouthSlavicScale scale)
    {
        if (count == 1)
        {
            parts.Add(scale.OneForm);
            return;
        }

        // The detector chooses the scale form after the recursive count rendering is complete;
        // the obvious shortcut of picking the plural here would drop paucal and dual forms.
        AppendPositive(parts, count, scale.Gender);
        parts.Add(ChooseScaleForm((long)count, scale, profile.ScaleFormDetector));
    }

    /// <summary>
    /// Appends the fragment below one thousand.
    /// </summary>
    void AppendUnderOneThousand(List<string> parts, int number, GrammaticalGender gender)
    {
        if (number >= 100)
        {
            parts.Add(profile.HundredsMap[number / 100]);
            number %= 100;
        }

        if (number >= 20)
        {
            // The inverted-tens linker is a profile sentinel: only some locales want the unit and
            // tens stem welded together, and only for the compound branch.
            if (profile.NumberComposition == SouthSlavicNumberComposition.InvertedTensWithLinker)
            {
                var units = number % 10;
                if (units > 0)
                {
                    parts.Add(profile.UnitsMap[units] + profile.InvertedTensLinker + profile.TensMap[number / 10]);
                    return;
                }

                parts.Add(profile.TensMap[number / 10]);
                return;
            }

            parts.Add(profile.TensMap[number / 10]);
            number %= 10;
        }

        if (number <= 0)
        {
            return;
        }

        parts.Add((number, gender) switch
        {
            (1, GrammaticalGender.Feminine) => profile.FeminineOne,
            (2, GrammaticalGender.Feminine) => profile.FeminineTwo,
            _ => profile.UnitsMap[number]
        });
    }

    /// <summary>
    /// Chooses the correct scale form for the requested grammatical-number detector.
    /// </summary>
    static string ChooseScaleForm(long number, SouthSlavicScale scale, SouthSlavicScaleFormDetector detector) =>
        detector switch
        {
            // The grammar detector stays profile-owned so the converter does not branch on locale
            // names; the same traversal supports Russian- and Slovenian-style morphology.
            SouthSlavicScaleFormDetector.Russian => RussianGrammaticalNumberDetector.Detect(number) switch
            {
                RussianGrammaticalNumber.Singular => scale.Singular,
                RussianGrammaticalNumber.Paucal => scale.Paucal,
                _ => scale.Plural
            },
            SouthSlavicScaleFormDetector.Slovenian => number switch
            {
                2 => scale.Dual ?? scale.Paucal,
                3 or 4 => scale.TrialQuadral ?? scale.Paucal,
                _ => scale.Plural
            },
            _ => throw new InvalidOperationException("Unknown South Slavic scale form detector.")
        };

    /// <summary>
    /// Returns the absolute value while safely handling <see cref="long.MinValue"/>.
    /// </summary>
    static ulong GetAbsoluteValue(long value) =>
        value >= 0 ? (ulong)value : unchecked((ulong)(-(value + 1)) + 1);
}

/// <summary>
/// Immutable generated profile for <see cref="SouthSlavicCardinalNumberToWordsConverter"/>.
/// </summary>
/// <param name="maximumValue">The maximum supported absolute value.</param>
/// <param name="allowLongMin">Whether <see cref="long.MinValue"/> is allowed as an input.</param>
/// <param name="zeroWord">The word used for zero.</param>
/// <param name="minusWord">The word used to prefix negative values.</param>
/// <param name="scaleFormDetector">The grammatical-number detector used to choose scale forms.</param>
/// <param name="numberComposition">The tens composition strategy used by the locale.</param>
/// <param name="invertedTensLinker">The linker inserted when the locale uses inverted tens.</param>
/// <param name="unitsMap">The unit lexicon.</param>
/// <param name="tensMap">The tens lexicon.</param>
/// <param name="hundredsMap">The hundreds lexicon.</param>
/// <param name="feminineOne">The feminine form used for one.</param>
/// <param name="feminineTwo">The feminine form used for two.</param>
/// <param name="scales">The descending scale rows used during decomposition.</param>
internal sealed class SouthSlavicCardinalNumberToWordsProfile(
    ulong maximumValue,
    bool allowLongMin,
    string zeroWord,
    string minusWord,
    SouthSlavicScaleFormDetector scaleFormDetector,
    SouthSlavicNumberComposition numberComposition,
    string invertedTensLinker,
    string[] unitsMap,
    string[] tensMap,
    string[] hundredsMap,
    string feminineOne,
    string feminineTwo,
    SouthSlavicScale[] scales)
{
    /// <summary>Gets the maximum supported absolute value.</summary>
    public ulong MaximumValue { get; } = maximumValue;
    /// <summary>Gets a value indicating whether <see cref="long.MinValue"/> is allowed as an input.</summary>
    public bool AllowLongMin { get; } = allowLongMin;
    /// <summary>Gets the word used for zero.</summary>
    public string ZeroWord { get; } = zeroWord;
    /// <summary>Gets the word used to prefix negative values.</summary>
    public string MinusWord { get; } = minusWord;
    /// <summary>Gets the grammatical-number detector used to choose scale forms.</summary>
    public SouthSlavicScaleFormDetector ScaleFormDetector { get; } = scaleFormDetector;
    /// <summary>Gets the tens composition strategy used by the locale.</summary>
    public SouthSlavicNumberComposition NumberComposition { get; } = numberComposition;
    /// <summary>Gets the linker inserted when the locale uses inverted tens.</summary>
    public string InvertedTensLinker { get; } = invertedTensLinker;
    /// <summary>Gets the unit lexicon.</summary>
    public string[] UnitsMap { get; } = unitsMap;
    /// <summary>Gets the tens lexicon.</summary>
    public string[] TensMap { get; } = tensMap;
    /// <summary>Gets the hundreds lexicon.</summary>
    public string[] HundredsMap { get; } = hundredsMap;
    /// <summary>Gets the feminine form used for one.</summary>
    public string FeminineOne { get; } = feminineOne;
    /// <summary>Gets the feminine form used for two.</summary>
    public string FeminineTwo { get; } = feminineTwo;
    /// <summary>Gets the descending scale rows used during decomposition.</summary>
    public SouthSlavicScale[] Scales { get; } = scales;
}

/// <summary>
/// One descending scale row for <see cref="SouthSlavicCardinalNumberToWordsConverter"/>.
/// </summary>
/// <param name="Value">The divisor for the scale row.</param>
/// <param name="Gender">The grammatical gender used for the scale row.</param>
/// <param name="OneForm">The special form used when the count is exactly one.</param>
/// <param name="Singular">The singular form.</param>
/// <param name="Paucal">The paucal form.</param>
/// <param name="Plural">The plural form.</param>
/// <param name="Dual">The dual form, if the locale distinguishes it.</param>
/// <param name="TrialQuadral">The trial or quadral form, if the locale distinguishes it.</param>
internal readonly record struct SouthSlavicScale(
    ulong Value,
    GrammaticalGender Gender,
    string OneForm,
    string Singular,
    string Paucal,
    string Plural,
    string? Dual = null,
    string? TrialQuadral = null);

/// <summary>
/// Selects the grammatical-number detector used for scale forms.
/// </summary>
internal enum SouthSlavicScaleFormDetector
{
    /// <summary>Uses the Russian grammatical-number detector.</summary>
    Russian,
    /// <summary>Uses the Slovenian grammatical-number detector.</summary>
    Slovenian
}

/// <summary>
/// Selects whether tens use direct composition or the inverted-tens linker.
/// </summary>
internal enum SouthSlavicNumberComposition
{
    /// <summary>Uses direct tens composition.</summary>
    Direct,
    /// <summary>Uses the inverted-tens linker.</summary>
    InvertedTensWithLinker
}