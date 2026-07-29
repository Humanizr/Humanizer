namespace Humanizer;

/// <summary>
/// Shared renderer for locales that form compound numbers with hyphenated or joined subparts and
/// expose dedicated ordinal tables for common forms.
///
/// The generated profile supplies the exact lexical tables while the runtime keeps the recursive
/// decomposition and abbreviation logic stable.
/// </summary>
class HyphenatedOrdinalNumberToWordsConverter(HyphenatedOrdinalNumberToWordsConverter.Profile profile) : GenderedNumberToWordsConverter
{
    /// <summary>
    /// Converts the given value using the locale's hyphenated cardinal rules.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use when the locale distinguishes it.</param>
    /// <param name="addAnd">Reserved for compatibility with other converters; this implementation derives conjunction placement from the generated profile.</param>
    /// <returns>The localized cardinal words for <paramref name="number"/>.</returns>
    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true)
    {
        var magnitude = GetAbsoluteValue(number);
        if (magnitude == 0)
        {
            return profile.ZeroWord;
        }

        var words = ConvertPositive(magnitude, gender, false, false);
        return number < 0 ? profile.NegativeWord + " " + words : words;
    }

    string ConvertPositive(ulong number, GrammaticalGender gender, bool isScaleCount, bool isScaleRemainder)
    {
        if (number < 10)
        {
            if (number == 1 && gender == GrammaticalGender.Masculine)
            {
                if (isScaleCount)
                {
                    return profile.MasculineScaleCountOne;
                }

                if (isScaleRemainder)
                {
                    return profile.MasculineCompoundOne;
                }
            }

            return GetUnit((int)number, gender);
        }

        if (number < 20)
        {
            // Teens are stored as exact words because they often do not decompose cleanly into the
            // same unit and tens tables as higher numbers.
            return profile.Teens[(int)(number - 10)];
        }

        if (number < 100)
        {
            return GetTens((int)number, gender, isScaleCount);
        }

        if (number < 1000)
        {
            return GetHundreds((int)number, gender, isScaleCount);
        }

        return GetScaleWords(number, gender, isScaleCount);
    }

    /// <summary>
    /// Converts the given value using the locale's hyphenated ordinal rules.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        var magnitude = number == int.MinValue ? (ulong)int.MaxValue + 1 : (ulong)Math.Abs(number);
        var words = ConvertToOrdinalPositive(magnitude, gender);
        return number < 0 ? profile.NegativeWord + " " + words : words;
    }

    string ConvertToOrdinalPositive(ulong number, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        // Exact ordinal forms win before any structural suffixing is attempted.
        var exactOrdinals = gender == GrammaticalGender.Feminine
            ? profile.OrdinalFeminine
            : profile.OrdinalMasculine;

        if (number < (ulong)exactOrdinals.Length && !string.IsNullOrEmpty(exactOrdinals[(int)number]))
        {
            return exactOrdinals[(int)number];
        }

        if (number < 100)
        {
            return GetOrdinalTens((int)number, gender);
        }

        if (number < 1000)
        {
            return GetOrdinalHundreds((int)number, gender);
        }

        foreach (var scale in profile.Scales)
        {
            if (number < scale.Value)
            {
                continue;
            }

            var count = number / scale.Value;
            var remainder = number % scale.Value;
            var scaleOrdinal = gender == GrammaticalGender.Feminine
                ? scale.OrdinalFeminine
                : scale.OrdinalMasculine;
            if (remainder == 0)
            {
                return count == 1
                    ? scaleOrdinal
                    : ConvertPositive(
                        count,
                        scale.CountUsesRequestedGender ? gender : GrammaticalGender.Masculine,
                        true,
                        false) + " " + scaleOrdinal;
            }

            return ConvertPositive(number - remainder, gender, false, false) + " " +
                   ConvertToOrdinalPositive(remainder, gender);
        }

        throw new InvalidOperationException("The scale profile must include the thousand row.");
    }

    /// <summary>
    /// Converts the given value to an ordinal abbreviation when requested.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use.</param>
    /// <param name="wordForm">The requested word form.</param>
    /// <returns>The localized ordinal abbreviation or full ordinal words for <paramref name="number"/>.</returns>
    public override string ConvertToOrdinal(int number, GrammaticalGender gender, WordForm wordForm)
    {
        if (wordForm != WordForm.Abbreviation)
        {
            return ConvertToOrdinal(number, gender);
        }

        // Abbreviation mode is its own contract: the caller wants the abbreviated ordinal, not the
        // full hyphenated spelling.
        if (profile.OrdinalAbbreviations.TryGetValue(GetOrdinalAbbreviationKey(number, gender), out var abbreviation))
        {
            return abbreviation;
        }

        // Fall back to the numeric abbreviation pattern when there is no exact dictionary hit.
        return number + (gender == GrammaticalGender.Feminine
            ? profile.DefaultOrdinalAbbreviationFeminine
            : GetMasculineOrdinalAbbreviationSuffix(number));
    }

    /// <summary>
    /// Converts the given value to the locale's tuple form.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <returns>The localized tuple words for <paramref name="number"/>.</returns>
    public override string ConvertToTuple(int number)
    {
        var magnitude = number == int.MinValue ? (long)int.MaxValue + 1 : Math.Abs(number);
        return magnitude < profile.TupleMap.Length
            ? profile.TupleMap[(int)magnitude]
            : Convert(magnitude) + " " + profile.TupleFallbackWord;
    }

    string GetScaleWords(ulong number, GrammaticalGender gender, bool isScaleCount)
    {
        foreach (var scale in profile.Scales)
        {
            if (number < scale.Value)
            {
                continue;
            }

            var count = number / scale.Value;
            var remainder = number % scale.Value;
            if (scale.GroupCountByThousands && count >= 1000)
            {
                var thousands = count / 1000;
                var units = count % 1000;
                if (units > 0 && (thousands < 10 || units >= 100))
                {
                    var combined = ConvertPositive(
                        count,
                        scale.CountUsesRequestedGender ? gender : GrammaticalGender.Masculine,
                        true,
                        false) + " " + scale.Plural;
                    return remainder == 0
                        ? combined
                        : combined + " " + ConvertPositive(remainder, gender, isScaleCount, !isScaleCount);
                }

                var grouped = thousands == 1
                    ? profile.ThousandWord
                    : ConvertPositive(thousands, GrammaticalGender.Masculine, true, false) + " " + profile.ThousandWord;
                grouped += " " + scale.Plural;
                if (units > 0)
                {
                    grouped += " " + ConvertPositive(units, GrammaticalGender.Masculine, true, false) + " " + scale.Plural;
                }

                return remainder == 0
                    ? grouped
                    : grouped + " " + ConvertPositive(remainder, gender, isScaleCount, !isScaleCount);
            }

            var scalePart = count == 1
                ? scale.OmitOne
                    ? scale.Singular
                    : scale.SingularPrefix + " " + scale.Singular
                : ConvertPositive(
                    count,
                    scale.CountUsesRequestedGender ? gender : GrammaticalGender.Masculine,
                    true,
                    false) + " " + scale.Plural;
            if (remainder == 0)
            {
                return scalePart;
            }

            return scalePart + " " + ConvertPositive(remainder, gender, isScaleCount, !isScaleCount);
        }

        throw new InvalidOperationException("The scale profile must include the thousand row.");
    }

    static ulong GetAbsoluteValue(long value) =>
        value >= 0 ? (ulong)value : unchecked((ulong)(-(value + 1)) + 1);

    /// <summary>
    /// Gets the gendered unit word for the supplied value.
    /// </summary>
    string GetUnit(int number, GrammaticalGender gender) =>
        gender == GrammaticalGender.Feminine ? profile.UnitsFeminine[number] : profile.UnitsMasculine[number];

    /// <summary>
    /// Gets the localized tens word for the supplied value.
    /// </summary>
    string GetTens(int number, GrammaticalGender gender, bool isScaleCount)
    {
        var tens = number / 10;
        var units = number % 10;
        if (units == 0)
        {
            return profile.Tens[tens];
        }

        // Hyphenated cardinals keep the unit first and the tens second, joined by the locale's
        // configured separator.
        return profile.Tens[tens] + GetTensJoiner(tens) + GetCompoundUnit(units, gender, isScaleCount);
    }

    /// <summary>
    /// Gets the localized hundreds word for the supplied value.
    /// </summary>
    string GetHundreds(int number, GrammaticalGender gender, bool isScaleCount)
    {
        var hundreds = number / 100;
        var remainder = number % 100;
        var hundredPart = gender == GrammaticalGender.Feminine ? profile.HundredsFeminine[hundreds] : profile.HundredsMasculine[hundreds];
        if (remainder == 0)
        {
            return hundredPart;
        }

        // Recurse through the main cardinal path so teen values keep their exact lexical forms,
        // but preserve the locale's dedicated unit-one compound forms for values like 101.
        return hundredPart + " " + (remainder < 10
            ? GetCompoundUnit(remainder, gender, isScaleCount)
            : ConvertPositive((ulong)remainder, gender, isScaleCount, false));
    }

    /// <summary>
    /// Renders the ordinal tens component for the supplied value.
    /// </summary>
    string GetOrdinalTens(int number, GrammaticalGender gender)
    {
        var tens = number / 10;
        var remainder = number % 10;
        var suffix = gender == GrammaticalGender.Feminine ? profile.FeminineTensOrdinalSuffix : profile.MasculineTensOrdinalSuffix;
        if (remainder == 0)
        {
            var tensStem = profile.OrdinalTensStems[tens];
            return tensStem + suffix;
        }

        var unitOrdinal = GetOrdinalUnitComponent(remainder, gender);
        return profile.Tens[tens] + GetTensJoiner(tens) + unitOrdinal;
    }

    /// <summary>
    /// Renders the ordinal hundreds component for the supplied value.
    /// </summary>
    string GetOrdinalHundreds(int number, GrammaticalGender gender)
    {
        var hundreds = number / 100;
        var remainder = number % 100;
        var hundredPart = gender == GrammaticalGender.Feminine ? profile.HundredsFeminine[hundreds] : profile.HundredsMasculine[hundreds];
        if (remainder == 0)
        {
            return gender == GrammaticalGender.Feminine
                ? profile.OrdinalHundredsFeminine[hundreds]
                : profile.OrdinalHundredsMasculine[hundreds];
        }

        return hundredPart + " " + ConvertToOrdinalPositive((ulong)remainder, gender);
    }

    /// <summary>
    /// Gets the unit word used when a compound ends in one.
    /// </summary>
    string GetCompoundUnit(int number, GrammaticalGender gender, bool isScaleCount) =>
        // Masculine compounds keep a dedicated "one" form only in the unit position.
        number == 1 && gender == GrammaticalGender.Masculine
            ? isScaleCount ? profile.MasculineScaleCountOne : profile.MasculineCompoundOne
            : GetUnit(number, gender);

    /// <summary>
    /// Gets the ordinal unit component used inside compound ordinals.
    /// </summary>
    string GetOrdinalUnitComponent(int number, GrammaticalGender gender) =>
        // Unit ordinals are either exact one-forms or a unit stem plus the tens suffix.
        number == 1
            ? (gender == GrammaticalGender.Feminine ? profile.FeminineOrdinalOne : profile.MasculineOrdinalOne)
            : profile.OrdinalUnitComponents[number] + (gender == GrammaticalGender.Feminine
                ? profile.FeminineTensOrdinalSuffix
                : profile.MasculineTensOrdinalSuffix);

    string GetTensJoiner(int tens) =>
        // Only one tens value uses the special joiner; the rest stay with the default separator.
        tens == profile.SpecialJoinerTensValue ? profile.SpecialTensJoiner : profile.DefaultTensJoiner;

    static string GetOrdinalAbbreviationKey(int number, GrammaticalGender gender) =>
        number.ToString(CultureInfo.InvariantCulture) + "|" + gender;

    string GetMasculineOrdinalAbbreviationSuffix(int number) =>
        (number % 10) switch
        {
            1 or 3 => "r",
            2 or 7 => "n",
            _ => profile.DefaultOrdinalAbbreviationMasculine
        };

    /// <summary>
    /// Immutable generated profile for <see cref="HyphenatedOrdinalNumberToWordsConverter"/>.
    /// </summary>
    /// <param name="ZeroWord">The cardinal zero word.</param>
    /// <param name="NegativeWord">The word used to prefix negative values.</param>
    /// <param name="ThousandWord">The base thousand noun.</param>
    /// <param name="MillionSingularPrefix">The singular prefix used before the million noun.</param>
    /// <param name="MillionSingular">The singular million noun.</param>
    /// <param name="MillionPlural">The plural million noun.</param>
    /// <param name="MasculineCompoundOne">The masculine unit-one form used inside compounds.</param>
    /// <param name="FeminineCompoundOne">The feminine unit-one form used inside compounds.</param>
    /// <param name="MasculineScaleCountOne">The masculine cardinal one form used before scale nouns.</param>
    /// <param name="MasculineOrdinalOne">The exact masculine ordinal form for one inside compounds.</param>
    /// <param name="FeminineOrdinalOne">The exact feminine ordinal form for one inside compounds.</param>
    /// <param name="DefaultTensJoiner">The default separator inserted between tens and trailing unit words.</param>
    /// <param name="SpecialTensJoiner">The special separator used for the configured tens value.</param>
    /// <param name="SpecialJoinerTensValue">The tens digit that uses <paramref name="SpecialTensJoiner"/> instead of the default separator.</param>
    /// <param name="MasculineTensOrdinalSuffix">The suffix appended to masculine tens stems when the tens are terminal.</param>
    /// <param name="FeminineTensOrdinalSuffix">The suffix appended to feminine tens stems when the tens are terminal.</param>
    /// <param name="DefaultOrdinalAbbreviationMasculine">The fallback masculine ordinal abbreviation suffix.</param>
    /// <param name="DefaultOrdinalAbbreviationFeminine">The fallback feminine ordinal abbreviation suffix.</param>
    /// <param name="TupleFallbackWord">The noun appended when tuple rendering falls back to a numeric phrase.</param>
    /// <param name="UnitsMasculine">The masculine cardinal unit words keyed by digit value.</param>
    /// <param name="UnitsFeminine">The feminine cardinal unit words keyed by digit value.</param>
    /// <param name="Teens">The teen words keyed by teen offset.</param>
    /// <param name="Tens">The tens words keyed by decade value.</param>
    /// <param name="HundredsMasculine">The masculine hundreds words keyed by digit value.</param>
    /// <param name="HundredsFeminine">The feminine hundreds words keyed by digit value.</param>
    /// <param name="OrdinalHundredsMasculine">The masculine exact hundred ordinals keyed by digit value.</param>
    /// <param name="OrdinalHundredsFeminine">The feminine exact hundred ordinals keyed by digit value.</param>
    /// <param name="OrdinalMasculine">The exact masculine ordinal lookup table keyed by absolute value.</param>
    /// <param name="OrdinalFeminine">The exact feminine ordinal lookup table keyed by absolute value.</param>
    /// <param name="OrdinalTensStems">The stems used when an exact tens value becomes ordinal.</param>
    /// <param name="OrdinalUnitComponents">The unit stems used inside compound ordinals.</param>
    /// <param name="TupleMap">The exact tuple names keyed by number.</param>
    /// <param name="OrdinalAbbreviations">The exact ordinal abbreviations keyed by number and gender.</param>
    public sealed record Profile(
        string ZeroWord,
        string NegativeWord,
        string ThousandWord,
        string MillionSingularPrefix,
        string MillionSingular,
        string MillionPlural,
        string MasculineCompoundOne,
        string FeminineCompoundOne,
        string MasculineScaleCountOne,
        string MasculineOrdinalOne,
        string FeminineOrdinalOne,
        string DefaultTensJoiner,
        string SpecialTensJoiner,
        int SpecialJoinerTensValue,
        string MasculineTensOrdinalSuffix,
        string FeminineTensOrdinalSuffix,
        string DefaultOrdinalAbbreviationMasculine,
        string DefaultOrdinalAbbreviationFeminine,
        string TupleFallbackWord,
        string[] UnitsMasculine,
        string[] UnitsFeminine,
        string[] Teens,
        string[] Tens,
        string[] HundredsMasculine,
        string[] HundredsFeminine,
        string[] OrdinalHundredsMasculine,
        string[] OrdinalHundredsFeminine,
        string[] OrdinalMasculine,
        string[] OrdinalFeminine,
        string[] OrdinalTensStems,
        string[] OrdinalUnitComponents,
        string[] TupleMap,
        FrozenDictionary<string, string> OrdinalAbbreviations,
        Scale[] Scales);

    /// <summary>One descending cardinal scale row.</summary>
    public sealed record Scale(
        ulong Value,
        string SingularPrefix,
        string Singular,
        string Plural,
        string OrdinalMasculine,
        string OrdinalFeminine,
        bool OmitOne = false,
        bool CountUsesRequestedGender = false,
        bool GroupCountByThousands = false);
}