namespace Humanizer;

/// <summary>
/// Renders locales that build long-scale ordinals by mutating a scale stem rather than appending
/// a simple suffix to the final word.
/// </summary>
class LongScaleStemOrdinalNumberToWordsConverter(LongScaleStemOrdinalNumberToWordsConverter.Profile profile) : GenderedNumberToWordsConverter
{
    /// <summary>
    /// Immutable generated profile that owns the long-scale lexicon, inflection tables, and ordinal
    /// stem rules.
    /// </summary>
    public sealed record Profile
    {
        /// <summary>
        /// Initializes the generated profile used by <see cref="LongScaleStemOrdinalNumberToWordsConverter"/>.
        /// </summary>
        public Profile(
            string zeroWord,
            string negativeWord,
            string exactHundredWord,
            string thousandWord,
            string tensJoiner,
            string thousandOrdinalStem,
            string masculineOrdinalEnding,
            string feminineOrdinalEnding,
            string minLongValueWord,
            long highestScaleValue,
            int roundHigherScaleCompactValue,
            string highestScaleOrdinalSource,
            string highestScaleOrdinalTarget,
            string tupleFallbackSuffix,
            string[] hundredsMasculine,
            string[] hundredsFeminine,
            string[] hundredthsRoots,
            string[] ordinalUnitRoots,
            string[] tensMap,
            string[] tenthsRoots,
            string[] thousandthsRoots,
            string[] tupleMap,
            string[] unitsMasculine,
            string[] unitsMasculineAbbreviation,
            string[] unitsFeminine,
            LargeScale[] largeScales)
        {
            ZeroWord = zeroWord;
            NegativeWord = negativeWord;
            ExactHundredWord = exactHundredWord;
            ThousandWord = thousandWord;
            TensJoiner = tensJoiner;
            ThousandOrdinalStem = thousandOrdinalStem;
            MasculineOrdinalEnding = masculineOrdinalEnding;
            FeminineOrdinalEnding = feminineOrdinalEnding;
            MinLongValueWord = minLongValueWord;
            HighestScaleValue = highestScaleValue;
            RoundHigherScaleCompactValue = roundHigherScaleCompactValue;
            HighestScaleOrdinalSource = highestScaleOrdinalSource;
            HighestScaleOrdinalTarget = highestScaleOrdinalTarget;
            TupleFallbackSuffix = tupleFallbackSuffix;
            HundredsMasculine = hundredsMasculine;
            HundredsFeminine = hundredsFeminine;
            HundredthsRoots = hundredthsRoots;
            OrdinalUnitRoots = ordinalUnitRoots;
            TensMap = tensMap;
            TenthsRoots = tenthsRoots;
            ThousandthsRoots = thousandthsRoots;
            TupleMap = tupleMap;
            UnitsMasculine = unitsMasculine;
            UnitsMasculineAbbreviation = unitsMasculineAbbreviation;
            UnitsFeminine = unitsFeminine;
            LargeScales = largeScales;
        }

        /// <summary>Gets the word used for zero.</summary>
        public string ZeroWord { get; }

        /// <summary>Gets the word used to prefix negative values.</summary>
        public string NegativeWord { get; }

        /// <summary>Gets the exact word for one hundred.</summary>
        public string ExactHundredWord { get; }

        /// <summary>Gets the word used for one thousand.</summary>
        public string ThousandWord { get; }

        /// <summary>Gets the joiner used between tens and units.</summary>
        public string TensJoiner { get; }

        /// <summary>Gets the ordinal stem used for the thousand scale.</summary>
        public string ThousandOrdinalStem { get; }

        /// <summary>Gets the masculine ordinal ending.</summary>
        public string MasculineOrdinalEnding { get; }

        /// <summary>Gets the feminine ordinal ending.</summary>
        public string FeminineOrdinalEnding { get; }

        /// <summary>Gets the special word used for <see cref="long.MinValue"/>.</summary>
        public string MinLongValueWord { get; }

        /// <summary>Gets the largest scale value modeled by the stem rewrite rules.</summary>
        public long HighestScaleValue { get; }

        /// <summary>Gets the smallest value that should use the compact round-scale ordinal path.</summary>
        public int RoundHigherScaleCompactValue { get; }

        /// <summary>Gets the source stem rewritten for the highest-scale ordinal.</summary>
        public string HighestScaleOrdinalSource { get; }

        /// <summary>Gets the replacement stem used for the highest-scale ordinal.</summary>
        public string HighestScaleOrdinalTarget { get; }

        /// <summary>Gets the suffix appended when no named tuple exists.</summary>
        public string TupleFallbackSuffix { get; }

        /// <summary>Gets the masculine hundred lexicon.</summary>
        public string[] HundredsMasculine { get; }

        /// <summary>Gets the feminine hundred lexicon.</summary>
        public string[] HundredsFeminine { get; }

        /// <summary>Gets the roots used for ordinal hundreds.</summary>
        public string[] HundredthsRoots { get; }

        /// <summary>Gets the roots used for ordinal units.</summary>
        public string[] OrdinalUnitRoots { get; }

        /// <summary>Gets the tens lexicon.</summary>
        public string[] TensMap { get; }

        /// <summary>Gets the roots used for ordinal tens.</summary>
        public string[] TenthsRoots { get; }

        /// <summary>Gets the roots used for ordinal thousands.</summary>
        public string[] ThousandthsRoots { get; }

        /// <summary>Gets the named tuple lexicon.</summary>
        public string[] TupleMap { get; }

        /// <summary>Gets the masculine unit lexicon.</summary>
        public string[] UnitsMasculine { get; }

        /// <summary>Gets the abbreviated masculine unit lexicon.</summary>
        public string[] UnitsMasculineAbbreviation { get; }

        /// <summary>Gets the feminine unit lexicon.</summary>
        public string[] UnitsFeminine { get; }

        /// <summary>Gets the descending large-scale rows used above the modeled thousand range.</summary>
        public LargeScale[] LargeScales { get; }
    }

    /// <summary>
    /// One descending large-scale row for <see cref="LongScaleStemOrdinalNumberToWordsConverter"/>.
    /// </summary>
    /// <param name="Value">The divisor for the scale row.</param>
    /// <param name="SingularPrefix">The prefix used before the singular scale name.</param>
    /// <param name="Singular">The singular scale name.</param>
    /// <param name="Plural">The plural scale name.</param>
    public sealed record LargeScale(long Value, string SingularPrefix, string Singular, string Plural);

    readonly Profile profile = profile;

    /// <inheritdoc/>
    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true) =>
        Convert(number, WordForm.Normal, gender, addAnd);

    /// <inheritdoc/>
    public override string Convert(long number, WordForm wordForm, GrammaticalGender gender, bool addAnd = true)
    {
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        if (number == long.MinValue)
        {
            return profile.MinLongValueWord;
        }

        if (number < 0)
        {
            return profile.NegativeWord + " " + Convert(-number);
        }

        List<string> wordBuilder =
        [
            ConvertGreaterThanHighestScale(number, out var remainder),
            ConvertThousands(remainder, out remainder, gender),
            ConvertHundreds(remainder, out remainder, gender),
            ConvertUnits(remainder, gender, wordForm)
        ];

        return BuildWord(wordBuilder);
    }

    /// <inheritdoc/>
    public override string ConvertToOrdinal(int number, GrammaticalGender gender) =>
        ConvertToOrdinal(number, gender, WordForm.Normal);

    /// <inheritdoc/>
    public override string ConvertToOrdinal(int number, GrammaticalGender gender, WordForm wordForm)
    {
        if (number is 0 or int.MinValue)
        {
            return profile.ZeroWord;
        }

        if (number < 0)
        {
            return ConvertToOrdinal(Math.Abs(number), gender);
        }

        if (IsRoundHigherScale(number))
        {
            // Round higher-scale ordinals are formed by taking the cardinal multiple and appending
            // the dedicated highest-scale ordinal instead of trying to inflect the final units.
            return ConvertRoundHigherScaleOrdinal(number, gender);
        }

        if (IsRoundHighestScale(number))
        {
            // Round highest-scale values piggyback on the next-lower scale and then rewrite the
            // highest-scale source stem; this avoids duplicating the locale's irregular stem logic.
            return ConvertToOrdinal(number / 1000, gender)
                .Replace(profile.HighestScaleOrdinalSource, profile.HighestScaleOrdinalTarget);
        }

        List<string> wordBuilder =
        [
            ConvertHigherThousandsOrdinal(number, out var remainder, gender),
            ConvertMappedOrdinalNumber(remainder, 1000, profile.ThousandthsRoots, out remainder, gender),
            ConvertMappedOrdinalNumber(remainder, 100, profile.HundredthsRoots, out remainder, gender),
            ConvertMappedOrdinalNumber(remainder, 10, profile.TenthsRoots, out remainder, gender),
            ConvertOrdinalUnits(remainder, gender, wordForm)
        ];

        return BuildWord(wordBuilder);
    }

    /// <inheritdoc/>
    public override string ConvertToTuple(int number)
    {
        number = Math.Abs(number);
        return number < profile.TupleMap.Length
            ? profile.TupleMap[number]
            : Convert(number) + profile.TupleFallbackSuffix;
    }

    /// <summary>
    /// Renders scales above the highest explicitly modeled value.
    /// </summary>
    string ConvertGreaterThanHighestScale(long inputNumber, out long remainder)
    {
        List<string> wordBuilder = [];
        remainder = inputNumber;

        foreach (var scale in profile.LargeScales)
        {
            if (remainder / scale.Value <= 0)
            {
                continue;
            }

            if (remainder / scale.Value == 1)
            {
                wordBuilder.Add(scale.SingularPrefix + " " + scale.Singular);
            }
            else
            {
                var count = remainder / scale.Value;
                var countWords = count % 10 == 1
                    ? Convert(count, WordForm.Abbreviation, GrammaticalGender.Masculine)
                    : Convert(count);
                wordBuilder.Add(countWords + " " + scale.Plural);
            }

            remainder %= scale.Value;
        }

        return BuildWord(wordBuilder);
    }

    /// <summary>
    /// Renders the thousand segment and returns the remainder below one thousand.
    /// </summary>
    string ConvertThousands(long inputNumber, out long remainder, GrammaticalGender gender)
    {
        remainder = inputNumber;
        if (inputNumber / 1000 <= 0)
        {
            return string.Empty;
        }

        if (inputNumber / 1000 == 1)
        {
            remainder = inputNumber % 1000;
            return profile.ThousandWord;
        }

        var count = inputNumber / 1000;
        remainder = inputNumber % 1000;
        var countWords = gender == GrammaticalGender.Feminine
            ? Convert(count, GrammaticalGender.Feminine)
            : Convert(count, WordForm.Abbreviation, gender);
        return countWords + " " + profile.ThousandWord;
    }

    /// <summary>
    /// Renders the high-thousands ordinal stem and returns the remainder below one thousand.
    /// </summary>
    string ConvertHigherThousandsOrdinal(int number, out int remainder, GrammaticalGender gender)
    {
        remainder = number;
        if (number / 10000 <= 0)
        {
            return string.Empty;
        }

        var wordPart = Convert(number / 1000 * 1000, gender);

        if (number < 30000 || IsRoundNumber(number))
        {
            if (number == 21000)
            {
                // 21,000 has a locale-specific orthographic fixup. The string replacements look odd,
                // but this is the narrowest way to preserve the established surface form.
                wordPart = wordPart.Replace("a", string.Empty)
                    .Replace("ú", "u");
            }

            // When the number is exactly this higher scale, the previous word already names the
            // scale; leaving the embedded space would duplicate the scale word in the ordinal.
            wordPart = wordPart.Remove(wordPart.LastIndexOf(' '), 1);
        }

        remainder = number % 1000;
        return wordPart + profile.ThousandOrdinalStem + GetGenderedOrdinalEnding(gender);
    }

    /// <summary>
    /// Renders the hundreds segment and returns the remainder below one hundred.
    /// </summary>
    string ConvertHundreds(long inputNumber, out long remainder, GrammaticalGender gender)
    {
        remainder = inputNumber;
        if (inputNumber / 100 <= 0)
        {
            return string.Empty;
        }

        remainder = inputNumber % 100;
        return inputNumber == 100
            ? profile.ExactHundredWord
            : GetHundredsMap(gender)[inputNumber / 100];
    }

    /// <summary>
    /// Renders the final units and tens segment.
    /// </summary>
    string ConvertUnits(long inputNumber, GrammaticalGender gender, WordForm wordForm)
    {
        if (inputNumber <= 0)
        {
            return string.Empty;
        }

        var unitsMap = GetUnitsMap(gender, wordForm);
        if (inputNumber < 30)
        {
            return unitsMap[inputNumber];
        }

        var wordPart = profile.TensMap[inputNumber / 10];
        return inputNumber % 10 <= 0
            ? wordPart
            : wordPart + " " + profile.TensJoiner + " " + unitsMap[inputNumber % 10];
    }

    /// <summary>
    /// Renders an ordinal units segment.
    /// </summary>
    string ConvertOrdinalUnits(int number, GrammaticalGender gender, WordForm wordForm)
    {
        if (number is <= 0 or >= 10)
        {
            return string.Empty;
        }

        var root = profile.OrdinalUnitRoots[number];
        return gender switch
        {
            GrammaticalGender.Feminine => root + profile.FeminineOrdinalEnding,
            GrammaticalGender.Masculine or GrammaticalGender.Neuter when HasOrdinalAbbreviation(number, wordForm) => root,
            _ => root + profile.MasculineOrdinalEnding
        };
    }

    /// <summary>
    /// Renders a mapped ordinal segment for the requested divisor.
    /// </summary>
    string ConvertMappedOrdinalNumber(int number, int divisor, string[] map, out int remainder, GrammaticalGender gender)
    {
        remainder = number;
        if (number / divisor <= 0)
        {
            return string.Empty;
        }

        remainder = number % divisor;
        return map[number / divisor] + GetGenderedOrdinalEnding(gender);
    }

    /// <summary>
    /// Returns the hundreds lexicon for the requested gender.
    /// </summary>
    string[] GetHundredsMap(GrammaticalGender gender) =>
        gender == GrammaticalGender.Feminine ? profile.HundredsFeminine : profile.HundredsMasculine;

    /// <summary>
    /// Returns the units lexicon for the requested gender and word form.
    /// </summary>
    string[] GetUnitsMap(GrammaticalGender gender, WordForm wordForm) =>
        gender switch
        {
            GrammaticalGender.Feminine => profile.UnitsFeminine,
            GrammaticalGender.Masculine or GrammaticalGender.Neuter when wordForm == WordForm.Abbreviation => profile.UnitsMasculineAbbreviation,
            _ => profile.UnitsMasculine
        };

    /// <summary>
    /// Builds the special ordinal form used for round values above the highest scale.
    /// </summary>
    string ConvertRoundHigherScaleOrdinal(int number, GrammaticalGender gender)
    {
        var cardinalPart = Convert(number / profile.HighestScaleValue, WordForm.Abbreviation, gender);
        var separator = number == profile.RoundHigherScaleCompactValue ? string.Empty : " ";
        var ordinalPart = ConvertToOrdinal((int)profile.HighestScaleValue, gender);
        return cardinalPart + separator + ordinalPart;
    }

    /// <summary>
    /// Returns the gender-specific ordinal ending.
    /// </summary>
    string GetGenderedOrdinalEnding(GrammaticalGender gender) =>
        gender == GrammaticalGender.Feminine ? profile.FeminineOrdinalEnding : profile.MasculineOrdinalEnding;

    /// <summary>
    /// Determines whether a unit ordinal can use its abbreviated stem.
    /// </summary>
    static bool HasOrdinalAbbreviation(int number, WordForm wordForm) =>
        number is 1 or 3 && wordForm == WordForm.Abbreviation;

    /// <summary>
    /// Determines whether a value is an exact round higher-scale value.
    /// </summary>
    bool IsRoundHigherScale(int number) =>
        number >= profile.RoundHigherScaleCompactValue && number % profile.HighestScaleValue == 0;

    /// <summary>
    /// Determines whether a value is an exact round highest-scale value.
    /// </summary>
    bool IsRoundHighestScale(int number) =>
        number >= profile.HighestScaleValue && number % profile.HighestScaleValue == 0;

    /// <summary>
    /// Removes empty fragments before joining the final word string.
    /// </summary>
    static string BuildWord(List<string> wordParts)
    {
        wordParts.RemoveAll(string.IsNullOrEmpty);
        return string.Join(" ", wordParts);
    }

    /// <summary>
    /// Determines whether the value matches one of the round-number shapes that needs a stem rewrite.
    /// </summary>
    static bool IsRoundNumber(int number) =>
        (number % 10000 == 0 && number < 100000)
        || (number % 100000 == 0 && number < 1000000)
        || (number % 1000000 == 0 && number < 10000000)
        || (number % 10000000 == 0 && number < 100000000)
        || (number % 100000000 == 0 && number < 1000000000)
        || (number % 1000000000 == 0 && number < int.MaxValue);
}