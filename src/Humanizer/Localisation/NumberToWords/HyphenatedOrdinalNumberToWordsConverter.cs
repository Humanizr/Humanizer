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
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        if (number < 0)
        {
            // Keep the sign separate so the positive branch can stay focused on the locale's
            // hyphenation rules and gendered lexicon.
            return profile.NegativeWord + " " + Convert(-number, gender);
        }

        if (number < 10)
        {
            return GetUnit((int)number, gender);
        }

        if (number < 20)
        {
            // Teens are stored as exact words because they often do not decompose cleanly into the
            // same unit and tens tables as higher numbers.
            return profile.Teens[number - 10];
        }

        if (number < 100)
        {
            return GetTens((int)number, gender);
        }

        if (number < 1000)
        {
            return GetHundreds((int)number, gender);
        }

        if (number < 1_000_000)
        {
            return GetThousands((int)number, gender);
        }

        if (number < 1_000_000_000)
        {
            return GetMillions((int)number, gender);
        }

        throw new NotImplementedException();
    }

    /// <summary>
    /// Converts the given value using the locale's hyphenated ordinal rules.
    /// </summary>
    /// <param name="number">The number to convert.</param>
    /// <param name="gender">The grammatical gender to use.</param>
    /// <returns>The localized ordinal words for <paramref name="number"/>.</returns>
    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        if (number < 0)
        {
            return profile.NegativeWord + " " + ConvertToOrdinal(-number, gender);
        }

        if (number == 0)
        {
            return profile.ZeroWord;
        }

        // Exact ordinal forms win before any structural suffixing is attempted.
        var exactOrdinals = gender == GrammaticalGender.Feminine
            ? profile.OrdinalFeminine
            : profile.OrdinalMasculine;

        if (number < exactOrdinals.Length && !string.IsNullOrEmpty(exactOrdinals[number]))
        {
            return exactOrdinals[number];
        }

        if (number < 100)
        {
            return GetOrdinalTens(number, gender);
        }

        if (number < 1000)
        {
            return GetOrdinalHundreds(number, gender);
        }

        if (number < 1_000_000)
        {
            return GetOrdinalThousands(number, gender);
        }

        if (number < 1_000_000_000)
        {
            return GetOrdinalMillions(number, gender);
        }

        throw new NotImplementedException();
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
        number = Math.Abs(number);
        return number < profile.TupleMap.Length
            ? profile.TupleMap[number]
            : Convert(number) + " " + profile.TupleFallbackWord;
    }

    /// <summary>
    /// Gets the gendered unit word for the supplied value.
    /// </summary>
    string GetUnit(int number, GrammaticalGender gender) =>
        gender == GrammaticalGender.Feminine ? profile.UnitsFeminine[number] : profile.UnitsMasculine[number];

    /// <summary>
    /// Gets the localized tens word for the supplied value.
    /// </summary>
    string GetTens(int number, GrammaticalGender gender)
    {
        var tens = number / 10;
        var units = number % 10;
        if (units == 0)
        {
            return profile.Tens[tens];
        }

        // Hyphenated cardinals keep the unit first and the tens second, joined by the locale's
        // configured separator.
        return profile.Tens[tens] + GetTensJoiner(tens) + GetCompoundUnit(units, gender);
    }

    /// <summary>
    /// Gets the localized hundreds word for the supplied value.
    /// </summary>
    string GetHundreds(int number, GrammaticalGender gender)
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
        return hundredPart + " " + (remainder < 10 ? GetCompoundUnit(remainder, gender) : Convert(remainder, gender));
    }

    /// <summary>
    /// Gets the localized thousands phrase for the supplied value.
    /// </summary>
    string GetThousands(int number, GrammaticalGender gender)
    {
        var thousands = number / 1000;
        var remainder = number % 1000;
        var thousandPart = thousands == 1
            ? profile.ThousandWord
            : Convert(thousands, gender) + " " + profile.ThousandWord;
        if (remainder == 0)
        {
            return thousandPart;
        }

        return thousandPart + " " + (remainder == 1 && gender == GrammaticalGender.Masculine ? profile.MasculineCompoundOne : Convert(remainder, gender));
    }

    /// <summary>
    /// Gets the localized millions phrase for the supplied value.
    /// </summary>
    string GetMillions(int number, GrammaticalGender gender)
    {
        var millions = number / 1_000_000;
        var remainder = number % 1_000_000;
        var millionPart = millions == 1
            ? profile.MillionSingularPrefix + " " + profile.MillionSingular
            : Convert(millions, GrammaticalGender.Masculine) + " " + profile.MillionPlural;
        if (remainder == 0)
        {
            return millionPart;
        }

        return millionPart + " " + (remainder == 1 && gender == GrammaticalGender.Masculine ? profile.MasculineCompoundOne : Convert(remainder, gender));
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
        if (remainder == 0 && hundreds == 1)
        {
            // One hundred is a special ordinal because the tens suffix attaches directly to the
            // hundred stem in the masculine/feminine tables.
            return hundredPart + (gender == GrammaticalGender.Feminine ? profile.FeminineTensOrdinalSuffix : profile.MasculineTensOrdinalSuffix);
        }

        if (remainder == 0)
        {
            return hundredPart;
        }

        string rest;
        if (hundreds == 1 && remainder % 10 != 0)
        {
            // Hundred-plus-remainder cases recurse into the ordinal renderer so the lower portion can
            // keep its own abbreviation and append rules.
            rest = ConvertToOrdinal(remainder, gender);
        }
        else
        {
            // Other hundreds keep the cardinal remainder, with a masculine ordinal appender only
            // when the locale expects one.
            rest = Convert(remainder, gender);
            if (gender == GrammaticalGender.Masculine && remainder != 1 && remainder % 10 == 1)
            {
                rest += profile.MasculineOrdinalAppender;
            }
        }

        return hundredPart + " " + rest;
    }

    /// <summary>
    /// Renders the ordinal thousands component for the supplied value.
    /// </summary>
    string GetOrdinalThousands(int number, GrammaticalGender gender)
    {
        var thousands = number / 1000;
        var remainder = number % 1000;
        var thousandPart = thousands == 1 ? profile.ThousandWord : Convert(thousands, gender) + " " + profile.ThousandWord;
        if (remainder == 0)
        {
            return thousandPart;
        }

        if (remainder == 100)
        {
            // Exact one hundred after a thousand keeps the dedicated hundred form.
            return thousandPart + " " + profile.HundredsMasculine[1];
        }

        var rest = Convert(remainder, gender);
        if (gender == GrammaticalGender.Masculine && remainder != 1 && remainder % 10 == 1)
        {
            // Masculine ordinals append a final marker only when the remainder ends in one.
            rest += profile.MasculineOrdinalAppender;
        }

        return thousandPart + " " + rest;
    }

    /// <summary>
    /// Renders the ordinal millions component for the supplied value.
    /// </summary>
    string GetOrdinalMillions(int number, GrammaticalGender gender)
    {
        var millions = number / 1_000_000;
        var remainder = number % 1_000_000;
        var millionPart = millions == 1
            ? profile.MillionSingularPrefix + " " + profile.MillionSingular
            : Convert(millions, GrammaticalGender.Masculine) + " " + profile.MillionPlural;
        if (remainder == 0)
        {
            return millionPart;
        }

        var rest = Convert(remainder, gender);
        if (gender == GrammaticalGender.Masculine && remainder != 1 && remainder % 10 == 1)
        {
            // Million ordinals follow the same masculine appender rule as thousands.
            rest += profile.MasculineOrdinalAppender;
        }

        return millionPart + " " + rest;
    }

    /// <summary>
    /// Gets the unit word used when a compound ends in one.
    /// </summary>
    string GetCompoundUnit(int number, GrammaticalGender gender) =>
        // Masculine compounds keep a dedicated "one" form only in the unit position.
        number == 1 && gender == GrammaticalGender.Masculine
            ? profile.MasculineCompoundOne
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
    /// <param name="MasculineOrdinalOne">The exact masculine ordinal form for one inside compounds.</param>
    /// <param name="FeminineOrdinalOne">The exact feminine ordinal form for one inside compounds.</param>
    /// <param name="DefaultTensJoiner">The default separator inserted between tens and trailing unit words.</param>
    /// <param name="SpecialTensJoiner">The special separator used for the configured tens value.</param>
    /// <param name="SpecialJoinerTensValue">The tens digit that uses <paramref name="SpecialTensJoiner"/> instead of the default separator.</param>
    /// <param name="MasculineTensOrdinalSuffix">The suffix appended to masculine tens stems when the tens are terminal.</param>
    /// <param name="FeminineTensOrdinalSuffix">The suffix appended to feminine tens stems when the tens are terminal.</param>
    /// <param name="MasculineOrdinalAppender">The extra masculine marker added when a compound remainder ends in one.</param>
    /// <param name="DefaultOrdinalAbbreviationMasculine">The fallback masculine ordinal abbreviation suffix.</param>
    /// <param name="DefaultOrdinalAbbreviationFeminine">The fallback feminine ordinal abbreviation suffix.</param>
    /// <param name="TupleFallbackWord">The noun appended when tuple rendering falls back to a numeric phrase.</param>
    /// <param name="UnitsMasculine">The masculine cardinal unit words keyed by digit value.</param>
    /// <param name="UnitsFeminine">The feminine cardinal unit words keyed by digit value.</param>
    /// <param name="Teens">The teen words keyed by teen offset.</param>
    /// <param name="Tens">The tens words keyed by decade value.</param>
    /// <param name="HundredsMasculine">The masculine hundreds words keyed by digit value.</param>
    /// <param name="HundredsFeminine">The feminine hundreds words keyed by digit value.</param>
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
        string MasculineOrdinalOne,
        string FeminineOrdinalOne,
        string DefaultTensJoiner,
        string SpecialTensJoiner,
        int SpecialJoinerTensValue,
        string MasculineTensOrdinalSuffix,
        string FeminineTensOrdinalSuffix,
        string MasculineOrdinalAppender,
        string DefaultOrdinalAbbreviationMasculine,
        string DefaultOrdinalAbbreviationFeminine,
        string TupleFallbackWord,
        string[] UnitsMasculine,
        string[] UnitsFeminine,
        string[] Teens,
        string[] Tens,
        string[] HundredsMasculine,
        string[] HundredsFeminine,
        string[] OrdinalMasculine,
        string[] OrdinalFeminine,
        string[] OrdinalTensStems,
        string[] OrdinalUnitComponents,
        string[] TupleMap,
        FrozenDictionary<string, string> OrdinalAbbreviations);
}
