using System.Globalization;

namespace Humanizer.Tests.Localisation;

public class LocaleRegistrySweepTests
{
    const char LeftToRightMark = (char)0x200E;
    const char RightToLeftMark = (char)0x200F;
    const char ArabicLetterMark = (char)0x061C;

    public static IEnumerable<object?[]> ShippedLocaleRows =>
        LocaleCoverageData.ShippedLocales.Select(static localeName => new object?[] { localeName });

    [Theory]
    [MemberData(nameof(LocaleCoverageData.FormatterExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void Formatter_NativeLocales_UseExpectedRelativeDateAndDurationStrings(string localeName, string expectedYesterday, string expectedTwoDays)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo(localeName));

        Assert.Equal(expectedYesterday, formatter.DateHumanize(TimeUnit.Day, Tense.Past, 1));
        Assert.Equal(expectedTwoDays, formatter.TimeSpanHumanize(TimeUnit.Day, 2));
    }

    [Fact]
    [UseCulture("iv")]
    public void CollectionFormatter_UnsupportedCulture_UsesDefaultFormatter()
    {
        var dates = new[] { DateTime.UtcNow, DateTime.UtcNow.AddDays(10) };
        Assert.Equal(dates[0] + " & " + dates[1], dates.Humanize());
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.CollectionFormatterExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void CollectionFormatter_NativeLocales_UseExpectedConjunction(string localeName, string expectedTwo, string expectedThree)
    {
        var formatter = Configurator.CollectionFormatters.ResolveForCulture(new CultureInfo(localeName));

        Assert.Equal(expectedTwo, formatter.Humanize([1, 2]));
        Assert.Equal(expectedThree, formatter.Humanize([1, 2, 3]));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.NumberToWordsCardinalExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void NumberToWords_NativeLocales_UseExpectedCardinalForms(string localeName, long number, string expected)
    {
        var culture = new CultureInfo(localeName);

        Assert.Equal(expected, number.ToWords(culture));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.NumberToWordsOrdinalExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void NumberToWords_NativeLocales_UseExpectedOrdinalForms(string localeName, int number, string expected)
    {
        var culture = new CultureInfo(localeName);

        Assert.Equal(expected, number.ToOrdinalWords(GrammaticalGender.Feminine, culture));
    }

    [Fact]
    public void NumberToWords_BrazilianPortuguese_UsesGenderedForms()
    {
        var culture = new CultureInfo("pt-BR");

        Assert.Equal("uma", 1.ToWords(GrammaticalGender.Feminine, culture));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.OrdinalizerExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void Ordinalizer_NativeLocales_UseExpectedForms(string localeName, int number, string expected)
    {
        var culture = new CultureInfo(localeName);

        Assert.Equal(expected, number.Ordinalize(culture));
        Assert.Equal(expected, number.ToString(culture).Ordinalize(culture));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.OrdinalizerGenderExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void Ordinalizer_NativeLocales_UseExpectedGenderedForms(string localeName, int number, GrammaticalGender gender, string expected)
    {
        var culture = new CultureInfo(localeName);

        Assert.Equal(expected, number.Ordinalize(gender, culture));
        Assert.Equal(expected, number.ToString(culture).Ordinalize(gender, culture));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.OrdinalizerDefaultExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void Ordinalizer_ExactLocales_UseExpectedDefaultForms(string localeName, string number, string expected)
    {
        var culture = new CultureInfo(localeName);

        Assert.Equal(expected, int.Parse(number, NumberStyles.Integer, CultureInfo.InvariantCulture).Ordinalize(culture));
        Assert.Equal(expected, number.Ordinalize(culture));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.OrdinalizerNegativeExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void Ordinalizer_ExactLocales_UseExpectedNegativeFallbackForms(string localeName, int number, string expected)
    {
        var culture = new CultureInfo(localeName);

        Assert.Equal(expected, number.Ordinalize(culture));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.OrdinalizerWordFormExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void Ordinalizer_ExactLocales_UseExpectedWordFormOutputs(string localeName, int number, WordForm wordForm, string expected)
    {
        var culture = new CultureInfo(localeName);

        Assert.Equal(expected, number.Ordinalize(culture, wordForm));
        Assert.Equal(expected, number.ToString(culture).Ordinalize(culture, wordForm));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.OrdinalizerWordFormGenderExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void Ordinalizer_ExactLocales_UseExpectedGenderedWordFormOutputs(string localeName, int number, GrammaticalGender gender, WordForm wordForm, string expected)
    {
        var culture = new CultureInfo(localeName);

        Assert.Equal(expected, number.Ordinalize(gender, culture, wordForm));
        Assert.Equal(expected, number.ToString(culture).Ordinalize(gender, culture, wordForm));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.OrdinalizerStringExactExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void Ordinalizer_ExactLocales_UseExpectedStringOutputs(string localeName, string number, GrammaticalGender gender, string expected)
    {
        var culture = new CultureInfo(localeName);

        Assert.Equal(expected, number.Ordinalize(gender, culture));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.OrdinalizerNumberExactExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void Ordinalizer_ExactLocales_UseExpectedNumericOutputs(string localeName, int number, GrammaticalGender gender, string expected)
    {
        var culture = new CultureInfo(localeName);

        Assert.Equal(expected, number.Ordinalize(gender, culture));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateShortDate2022January25ReferenceTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateFormatting_ShortDate_2022January25_UsesExpectedCultureString(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateTime(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, ToVisibleText(date.ToString("d", CultureInfo.CurrentCulture)));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateLongDate2022January25ReferenceTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateFormatting_LongDate_2022January25_UsesExpectedCultureString(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateTime(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, ToVisibleText(date.ToString("D", CultureInfo.CurrentCulture)));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateShortDate2015January1ReferenceTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateFormatting_ShortDate_2015January1_UsesExpectedCultureString(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateTime(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, ToVisibleText(date.ToString("d", CultureInfo.CurrentCulture)));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateLongDate2015January1ReferenceTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateFormatting_LongDate_2015January1_UsesExpectedCultureString(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateTime(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, ToVisibleText(date.ToString("D", CultureInfo.CurrentCulture)));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateShortDate2015February3ReferenceTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateFormatting_ShortDate_2015February3_UsesExpectedCultureString(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateTime(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, ToVisibleText(date.ToString("d", CultureInfo.CurrentCulture)));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateLongDate2015February3ReferenceTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateFormatting_LongDate_2015February3_UsesExpectedCultureString(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateTime(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, ToVisibleText(date.ToString("D", CultureInfo.CurrentCulture)));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateToOrdinalWords2022January25ExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateToOrdinalWords_2022January25_UsesExpectedForms(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateTime(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, date.ToOrdinalWords());
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateToOrdinalWords2015January1ExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateToOrdinalWords_2015January1_UsesExpectedForms(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateTime(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, date.ToOrdinalWords());
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateToOrdinalWords2015February3ExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateToOrdinalWords_2015February3_UsesExpectedForms(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateTime(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, date.ToOrdinalWords());
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateToOrdinalWords2020February29ExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateToOrdinalWords_2020February29_UsesExpectedForms(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateTime(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, date.ToOrdinalWords());
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateToOrdinalWords2015September4ExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateToOrdinalWords_2015September4_UsesExpectedForms(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateTime(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, date.ToOrdinalWords());
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateToOrdinalWords1979November7ExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateToOrdinalWords_1979November7_UsesExpectedForms(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateTime(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, date.ToOrdinalWords());
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateToOrdinalWords2020March2ExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateToOrdinalWords_2020March2_UsesExpectedForms(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateTime(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, date.ToOrdinalWords());
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateToOrdinalWords2021October31ExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateToOrdinalWords_2021October31_UsesExpectedForms(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateTime(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, date.ToOrdinalWords());
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateToOrdinalWords2024December31ExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateToOrdinalWords_2024December31_UsesExpectedForms(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateTime(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, date.ToOrdinalWords());
    }

    [Theory]
    [InlineData(2015, 1, 1, "1 يناير 2015")]
    [InlineData(2024, 12, 31, "31 ديسمبر 2024")]
    public void DateToOrdinalWords_ArabicOutput_DoesNotIncludeDirectionalityControls(int year, int month, int day, string expected)
    {
        using var _ = new CultureSwap(new("ar"));
        var result = new DateTime(year, month, day).ToOrdinalWords();

        Assert.Equal(expected, result);
        Assert.DoesNotContain(LeftToRightMark, result);
        Assert.DoesNotContain(RightToLeftMark, result);
        Assert.DoesNotContain(ArabicLetterMark, result);
    }
#if NET6_0_OR_GREATER

    [Theory]
    [MemberData(nameof(LocaleCoverageData.TimeOnlyShortTime1323ReferenceTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void TimeFormatting_ShortTime_1323_UsesExpectedCultureString(string localeName, ClockExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var time = new TimeOnly(expected.Hours, expected.Minutes);
        Assert.Equal(expected.Expected, ToVisibleText(time.ToString("t", CultureInfo.CurrentCulture)));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.TimeOnlyLongTime1323ReferenceTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void TimeFormatting_LongTime_1323_UsesExpectedCultureString(string localeName, ClockExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var time = new TimeOnly(expected.Hours, expected.Minutes);
        Assert.Equal(expected.Expected, ToVisibleText(time.ToString("T", CultureInfo.CurrentCulture)));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.TimeOnlyShortTime1325ReferenceTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void TimeFormatting_ShortTime_1325_UsesExpectedCultureString(string localeName, ClockExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var time = new TimeOnly(expected.Hours, expected.Minutes);
        Assert.Equal(expected.Expected, ToVisibleText(time.ToString("t", CultureInfo.CurrentCulture)));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.TimeOnlyLongTime1325ReferenceTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void TimeFormatting_LongTime_1325_UsesExpectedCultureString(string localeName, ClockExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var time = new TimeOnly(expected.Hours, expected.Minutes);
        Assert.Equal(expected.Expected, ToVisibleText(time.ToString("T", CultureInfo.CurrentCulture)));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.TimeOnlyShortTime0105ReferenceTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void TimeFormatting_ShortTime_0105_UsesExpectedCultureString(string localeName, ClockExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var time = new TimeOnly(expected.Hours, expected.Minutes);
        Assert.Equal(expected.Expected, ToVisibleText(time.ToString("t", CultureInfo.CurrentCulture)));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.TimeOnlyLongTime0105ReferenceTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void TimeFormatting_LongTime_0105_UsesExpectedCultureString(string localeName, ClockExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var time = new TimeOnly(expected.Hours, expected.Minutes);
        Assert.Equal(expected.Expected, ToVisibleText(time.ToString("T", CultureInfo.CurrentCulture)));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateOnlyToOrdinalWords2022January25ExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateOnlyToOrdinalWords_2022January25_UsesExpectedForms(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateOnly(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, date.ToOrdinalWords());
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateOnlyToOrdinalWords2015January1ExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateOnlyToOrdinalWords_2015January1_UsesExpectedForms(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateOnly(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, date.ToOrdinalWords());
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateOnlyToOrdinalWords2015February3ExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateOnlyToOrdinalWords_2015February3_UsesExpectedForms(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateOnly(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, date.ToOrdinalWords());
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateOnlyToOrdinalWords2020February29ExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateOnlyToOrdinalWords_2020February29_UsesExpectedForms(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateOnly(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, date.ToOrdinalWords());
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateOnlyToOrdinalWords2015September4ExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateOnlyToOrdinalWords_2015September4_UsesExpectedForms(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateOnly(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, date.ToOrdinalWords());
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateOnlyToOrdinalWords1979November7ExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateOnlyToOrdinalWords_1979November7_UsesExpectedForms(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateOnly(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, date.ToOrdinalWords());
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateOnlyToOrdinalWords2020March2ExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateOnlyToOrdinalWords_2020March2_UsesExpectedForms(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateOnly(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, date.ToOrdinalWords());
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateOnlyToOrdinalWords2021October31ExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateOnlyToOrdinalWords_2021October31_UsesExpectedForms(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateOnly(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, date.ToOrdinalWords());
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.DateOnlyToOrdinalWords2024December31ExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void DateOnlyToOrdinalWords_2024December31_UsesExpectedForms(string localeName, DateExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var date = new DateOnly(expected.Year, expected.Month, expected.Day);
        Assert.Equal(expected.Expected, date.ToOrdinalWords());
    }

    [Theory]
    [InlineData(2015, 1, 1, "1 يناير 2015")]
    [InlineData(2024, 12, 31, "31 ديسمبر 2024")]
    public void DateOnlyToOrdinalWords_ArabicOutput_DoesNotIncludeDirectionalityControls(int year, int month, int day, string expected)
    {
        using var _ = new CultureSwap(new("ar"));
        var result = new DateOnly(year, month, day).ToOrdinalWords();

        Assert.Equal(expected, result);
        Assert.DoesNotContain(LeftToRightMark, result);
        Assert.DoesNotContain(RightToLeftMark, result);
        Assert.DoesNotContain(ArabicLetterMark, result);
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.TimeOnlyToClockNotation1323ExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void TimeOnlyToClockNotation_1323_UsesExpectedForms(string localeName, ClockExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var time = new TimeOnly(expected.Hours, expected.Minutes);
        Assert.Equal(expected.Expected, time.ToClockNotation());
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.TimeOnlyToClockNotation1323RoundedExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void TimeOnlyToClockNotation_1323RoundedToNearestFiveMinutes_UsesExpectedForms(string localeName, ClockExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var time = new TimeOnly(expected.Hours, expected.Minutes);
        Assert.Equal(expected.Expected, time.ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.TimeOnlyToClockNotation0105ExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void TimeOnlyToClockNotation_0105_UsesExpectedForms(string localeName, ClockExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var time = new TimeOnly(expected.Hours, expected.Minutes);
        Assert.Equal(expected.Expected, time.ToClockNotation());
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.TimeOnlyToClockNotationAdditionalExactExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void TimeOnlyToClockNotation_AdditionalExactLocales_UseExpectedForms(string localeName, ClockExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var time = new TimeOnly(expected.Hours, expected.Minutes);
        Assert.Equal(expected.Expected, time.ToClockNotation());
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.TimeOnlyToClockNotationAdditionalRoundedExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void TimeOnlyToClockNotation_AdditionalRoundedLocales_UseExpectedForms(string localeName, ClockExpectationRow expected)
    {
        using var _ = new CultureSwap(new(localeName));
        var time = new TimeOnly(expected.Hours, expected.Minutes);
        Assert.Equal(expected.Expected, time.ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
#endif

    [Theory]
    [MemberData(nameof(LocaleCoverageData.WordsToNumberExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void WordsToNumber_NativeLocales_ParseNativeWords(string localeName, long expected, string words)
    {
        var culture = new CultureInfo(localeName);

        Assert.Equal(expected, words.ToNumber(culture));
        Assert.True(words.TryToNumber(out var parsedNumber, culture, out var unrecognizedWord));
        Assert.Equal(expected, parsedNumber);
        Assert.Null(unrecognizedWord);
    }

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void WordsToNumber_RegisteredLocales_RoundTripNativeWords(string localeName)
    {
        var culture = new CultureInfo(localeName);

        foreach (var number in new[] { 1L, 21L, 105L, 1_001L })
        {
            var words = number.ToWords(culture);

            Assert.Equal(number, words.ToNumber(culture));
            Assert.True(words.TryToNumber(out var parsedNumber, culture, out var unrecognizedWord));
            Assert.Equal(number, parsedNumber);
            Assert.Null(unrecognizedWord);
        }
    }

    [Theory]
    [InlineData("eo", "one", 1L)]
    public void WordsToNumber_UnknownCultures_UseDefaultLexicon(string localeName, string words, long expected)
    {
        var culture = new CultureInfo(localeName);

        Assert.Equal(expected, words.ToNumber(culture));
        Assert.True(words.TryToNumber(out var parsedNumber, culture, out var unrecognizedWord));
        Assert.Equal(expected, parsedNumber);
        Assert.Null(unrecognizedWord);
    }

    static string ToVisibleText(string value) =>
        new(value.Where(static ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.Format).ToArray());
}
