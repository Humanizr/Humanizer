using System.Collections.Frozen;

namespace Humanizer.Tests;

public class CoverageGapTests
{
    [Fact]
    public void NoMatchFoundExceptionConstructorsPopulateMessageAndInnerException()
    {
        var defaultException = new NoMatchFoundException();
        Assert.Null(defaultException.InnerException);

        var messageException = new NoMatchFoundException("No match");
        Assert.Equal("No match", messageException.Message);
        Assert.Null(messageException.InnerException);

        var inner = new InvalidOperationException("Inner");
        var wrapped = new NoMatchFoundException("Wrapped", inner);
        Assert.Equal("Wrapped", wrapped.Message);
        Assert.Same(inner, wrapped.InnerException);
    }

    [Fact]
    public void WordsToNumberTryOverloadWithoutUnrecognizedWordReportsSuccessAndFailure()
    {
        Assert.True("twenty one".TryToNumber(out var parsedNumber, CultureInfo.InvariantCulture));
        Assert.Equal(21, parsedNumber);

        Assert.False("twenty mystery".TryToNumber(out parsedNumber, CultureInfo.InvariantCulture));
        Assert.Equal(0, parsedNumber);
    }

    [Fact]
    public void TokenMapOrdinalBuilderStringOverloadBuildsDefaultOrdinals()
    {
        var ordinals = TokenMapWordsToNumberOrdinalMapBuilder.Build(
            "en",
            TokenMapNormalizationProfile.LowercaseRemovePeriods,
            TokenMapOrdinalGenderVariant.None);

        Assert.Equal(1, ordinals["first"]);
        Assert.Equal(21, ordinals["twenty first"]);
    }

    [Theory]
    [InlineData((int)TokenMapOrdinalGenderVariant.None, "default 7")]
    [InlineData((int)TokenMapOrdinalGenderVariant.MasculineAndFeminine, "masculine 7")]
    [InlineData((int)TokenMapOrdinalGenderVariant.MasculineAndFeminine, "feminine 7")]
    [InlineData((int)TokenMapOrdinalGenderVariant.All, "default 7")]
    [InlineData((int)TokenMapOrdinalGenderVariant.All, "feminine 7")]
    [InlineData((int)TokenMapOrdinalGenderVariant.All, "neuter 7")]
    public void TokenMapOrdinalBuilderConverterOverloadCoversEveryGenderVariant(int variantValue, string expectedKey)
    {
        var variant = (TokenMapOrdinalGenderVariant)variantValue;

        var ordinals = TokenMapWordsToNumberOrdinalMapBuilder.Build(
            new GenderEchoOrdinalConverter(),
            TokenMapNormalizationProfile.LowercaseRemovePeriods,
            variant);

        Assert.Equal(7, ordinals[expectedKey]);
    }

    [Theory]
    [InlineData("42", 42)]
    [InlineData("minus two", -2)]
    [InlineData("negative THREE", -3)]
    [InlineData("one thousand two", 1002)]
    [InlineData("thousandone", 1001)]
    [InlineData("twothousandsone", 2001)]
    [InlineData("hundredfive", 105)]
    [InlineData("twohundredssix", 206)]
    [InlineData("twotyone", 21)]
    [InlineData("fiveteen", 15)]
    [InlineData("two-million, three", 2000003)]
    public void SuffixScaleConverterParsesSuffixAndNormalizedForms(string words, long expected)
    {
        var converter = new SuffixScaleWordsToNumberConverter(SuffixScaleProfile);

        Assert.True(converter.TryConvert(words, out var parsed, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
        Assert.Equal(expected, converter.Convert(words));
    }

    [Fact]
    public void SuffixScaleConverterReportsEmptyAndUnrecognizedInputs()
    {
        var converter = new SuffixScaleWordsToNumberConverter(SuffixScaleProfile);

        var empty = Assert.Throws<ArgumentException>(() => converter.Convert("  "));
        Assert.Equal("Input words cannot be empty.", empty.Message);

        Assert.False(converter.TryConvert("twohundredsstwenty", out var parsed, out var unrecognizedWord));
        Assert.Equal(0, parsed);
        Assert.Equal("twohundredsstwenty", unrecognizedWord);

        var invalid = Assert.Throws<ArgumentException>(() => converter.Convert("twohundredsstwenty"));
        Assert.Equal("Unrecognized number word: twohundredsstwenty", invalid.Message);
    }

    [UseCulture("en-US")]
    [Theory]
    [InlineData((int)OrdinalDateDayMode.Numeric, 1, "1 January 2024")]
    [InlineData((int)OrdinalDateDayMode.Ordinal, 2, "2nd January 2024")]
    [InlineData((int)OrdinalDateDayMode.OrdinalWhenDayIsOne, 1, "1st January 2024")]
    [InlineData((int)OrdinalDateDayMode.OrdinalWhenDayIsOne, 2, "2 January 2024")]
    [InlineData((int)OrdinalDateDayMode.MasculineOrdinalWhenDayIsOne, 1, "1st January 2024")]
    [InlineData((int)OrdinalDateDayMode.MasculineOrdinalWhenDayIsOne, 2, "2 January 2024")]
    [InlineData((int)OrdinalDateDayMode.DotSuffix, 2, "2. January 2024")]
    public void OrdinalDatePatternFormatsEveryDayMode(int dayModeValue, int day, string expected)
    {
        var dayMode = (OrdinalDateDayMode)dayModeValue;
        var pattern = new OrdinalDatePattern("{day} MMMM yyyy", dayMode);

        Assert.Equal(expected, pattern.Format(new DateTime(2024, 1, day)));
    }

    [UseCulture("en-US")]
    [Fact]
    public void OrdinalDatePatternUsesMonthOverridesAndGenitiveContext()
    {
        var pattern = new OrdinalDatePattern(
            "{day} MMMM yyyy",
            OrdinalDateDayMode.Numeric,
            months: MonthNames("Month"),
            monthsGenitive: MonthNames("Genitive"));

        Assert.Equal("3 Genitive1 2024", pattern.Format(new DateTime(2024, 1, 3)));
    }

    [UseCulture("en-US")]
    [Fact]
    public void OrdinalDatePatternUsesMonthOverrideWithoutGenitiveWhenDayIsNotAdjacent()
    {
        var pattern = new OrdinalDatePattern(
            "MMMM yyyy",
            OrdinalDateDayMode.Numeric,
            months: MonthNames("Month"),
            monthsGenitive: MonthNames("Genitive"));

        Assert.Equal("Month1 2024", pattern.Format(new DateTime(2024, 1, 3)));
    }

    [UseCulture("en-US")]
    [Fact]
    public void OrdinalDatePatternFallsBackWhenTemplateHasNoDayMarkerAndStripsDirectionalityControls()
    {
        var pattern = new OrdinalDatePattern("\u200EMMMM\u200F yyyy \u061C{day}", OrdinalDateDayMode.Numeric);

        Assert.Equal("January 2024 3", pattern.Format(new DateTime(2024, 1, 3)));
    }

    [UseCulture("en-US")]
    [Fact]
    public void OrdinalDatePatternSupportsNativeCalendarModeAndRejectsUnknownDayMode()
    {
        var native = new OrdinalDatePattern("{day} MMMM yyyy", OrdinalDateDayMode.Numeric, OrdinalDateCalendarMode.Native);
        Assert.Equal("3 January 2024", native.Format(new DateTime(2024, 1, 3)));

        var invalid = new OrdinalDatePattern("{day} MMMM yyyy", (OrdinalDateDayMode)999);
        var exception = Assert.Throws<InvalidOperationException>(() => invalid.Format(new DateTime(2024, 1, 3)));
        Assert.Equal("Unsupported ordinal date day mode.", exception.Message);
    }

#if NET6_0_OR_GREATER
    [UseCulture("en-US")]
    [Fact]
    public void OrdinalDatePatternFormatsDateOnlyValues()
    {
        var pattern = new OrdinalDatePattern("{day} MMMM yyyy", OrdinalDateDayMode.Ordinal);

        Assert.Equal("22nd February 2024", pattern.Format(new DateOnly(2024, 2, 22)));
    }
#endif

    [Theory]
    [InlineData((int)TokenMapNormalizationProfile.CollapseWhitespace, " one \t two  ", "one two")]
    [InlineData((int)TokenMapNormalizationProfile.LowercaseRemovePeriods, " ONE-two,. ", "one two")]
    [InlineData((int)TokenMapNormalizationProfile.LowercaseReplacePeriodsWithSpaces, " ONE.two-three, four ", "one two three four")]
    [InlineData((int)TokenMapNormalizationProfile.LowercaseRemovePeriodsAndDiacritics, " Één-twó. ", "een two")]
    [InlineData((int)TokenMapNormalizationProfile.PunctuationToSpacesRemoveDiacritics, " Één/twó;three ", "Een two three")]
    [InlineData((int)TokenMapNormalizationProfile.Persian, "ي‌ك،.", "ی ک")]
    public void TokenMapNormalizerCoversEveryProfile(int profileValue, string words, string expected)
    {
        var profile = (TokenMapNormalizationProfile)profileValue;

        Assert.Equal(expected, TokenMapWordsToNumberNormalizer.Normalize(words, profile));
    }

    [Fact]
    public void TokenMapNormalizerRejectsUnknownProfile()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => TokenMapWordsToNumberNormalizer.Normalize("one", (TokenMapNormalizationProfile)999));

        Assert.Equal("profile", exception.ParamName);
    }

    [Theory]
    [InlineData("123", 123)]
    [InlineData("minus two", -2)]
    [InlineData("two negative", -2)]
    [InlineData("ordinal third", 3)]
    [InlineData("21st", 21)]
    [InlineData("two million billion", 2_000_000_000_000_000)]
    [InlineData("two hundred three", 203)]
    [InlineData("two teen", 12)]
    [InlineData("two hundred", 200)]
    [InlineData("kathreex", 3)]
    [InlineData("two ten", 20)]
    [InlineData("hundred two ten", 120)]
    [InlineData("special phrase", 77)]
    [InlineData("two thousand third", 2003)]
    [InlineData("two thousandth", 2000)]
    [InlineData("twomillionth", 2_000_000)]
    public void TokenMapConverterParsesConfiguredGrammarBranches(string words, long expected)
    {
        var converter = new TokenMapWordsToNumberConverter(TokenMapRules);

        Assert.True(converter.TryConvert(words, out var parsed, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
        Assert.Equal(expected, converter.Convert(words));
    }

    [Fact]
    public void TokenMapConverterReportsInvalidEmptyAndOverflowInputs()
    {
        var converter = new TokenMapWordsToNumberConverter(TokenMapRules);

        var empty = Assert.Throws<ArgumentException>(() => converter.Convert(" "));
        Assert.Equal("Input words cannot be empty.", empty.Message);

        Assert.False(converter.TryConvert("mystery", out var parsed, out var unrecognizedWord));
        Assert.Equal(0, parsed);
        Assert.Equal("mystery", unrecognizedWord);

        Assert.False(converter.TryConvert("huge hundred", out parsed, out unrecognizedWord));
        Assert.Equal(0, parsed);
        Assert.Equal("huge hundred", unrecognizedWord);

        var invalid = Assert.Throws<ArgumentException>(() => converter.Convert("mystery"));
        Assert.Equal("Unrecognized number word: mystery", invalid.Message);
    }

    [Fact]
    public void TokenMapConverterCoversTerminalOrdinalRejectionAndOverflowBranches()
    {
        var converter = new TokenMapWordsToNumberConverter(TokenMapRules);

        Assert.False(converter.TryConvert("first one", out _, out var unrecognizedWord));
        Assert.Equal("first", unrecognizedWord);

        var exactOrdinalConverter = new TokenMapWordsToNumberConverter(CreateTokenMapRulesWithoutOrdinalScales());
        Assert.False(exactOrdinalConverter.TryConvert("first one", out _, out unrecognizedWord));
        Assert.Equal("first", unrecognizedWord);

        var exactOrdinalOverflow = new TokenMapWordsToNumberConverter(CreateTokenMapRulesWithExactOrdinalOverflow());
        Assert.False(exactOrdinalOverflow.TryConvert("one max", out _, out unrecognizedWord));
        Assert.Equal("max", unrecognizedWord);

        var ordinalScaleOverflow = new TokenMapWordsToNumberConverter(CreateTokenMapRulesWithOrdinalScaleOverflow());
        Assert.False(ordinalScaleOverflow.TryConvert("two hugeord", out _, out unrecognizedWord));
        Assert.Equal("hugeord", unrecognizedWord);

        var gluedOrdinalOverflow = new TokenMapWordsToNumberConverter(CreateTokenMapRulesWithGluedOrdinalOverflow());
        Assert.False(gluedOrdinalOverflow.TryConvert("twoillionth", out _, out unrecognizedWord));
        Assert.Equal("twoillionth", unrecognizedWord);
    }

    [Theory]
    [InlineData("42", 42)]
    [InlineData("minus two", -2)]
    [InlineData("first", 1)]
    [InlineData("twoth", 2)]
    [InlineData("two hundred and three", 203)]
    [InlineData("twomillionandthree", 2000003)]
    [InlineData("fooenzig", 22)]
    [InlineData("two thousand three", 2003)]
    [InlineData("hundred", 100)]
    public void InvertedTensConverterParsesConfiguredBranches(string words, long expected)
    {
        var converter = new InvertedTensWordsToNumberConverter(InvertedTensProfile);

        Assert.True(converter.TryConvert(words, out var parsed, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
        Assert.Equal(expected, converter.Convert(words));
    }

    [Fact]
    public void InvertedTensConverterReportsEmptyAndInvalidInputs()
    {
        var converter = new InvertedTensWordsToNumberConverter(InvertedTensProfile);

        var empty = Assert.Throws<ArgumentException>(() => converter.Convert(""));
        Assert.Equal("Input words cannot be empty.", empty.Message);

        Assert.False(converter.TryConvert("two mystery", out var parsed, out var unrecognizedWord));
        Assert.Equal(0, parsed);
        Assert.Equal("mystery", unrecognizedWord);

        var invalid = Assert.Throws<ArgumentException>(() => converter.Convert("mystery"));
        Assert.Equal("Unrecognized number word: mystery", invalid.Message);
    }

    [Theory]
    [InlineData("42", 42)]
    [InlineData("minus-one", -1)]
    [InlineData("one thousand two", 1002)]
    [InlineData("twentythree", 23)]
    [InlineData("twenfour", 24)]
    [InlineData("thousand", 1000)]
    [InlineData("three-thousand-five", 3005)]
    public void PrefixedTensConverterParsesConfiguredBranches(string words, long expected)
    {
        var converter = new PrefixedTensScaleWordsToNumberConverter(PrefixedTensProfile);

        Assert.True(converter.TryConvert(words, out var parsed, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
        Assert.Equal(expected, converter.Convert(words));
    }

    [Fact]
    public void PrefixedTensConverterReportsEmptyAndInvalidInputs()
    {
        var converter = new PrefixedTensScaleWordsToNumberConverter(PrefixedTensProfile);

        var empty = Assert.Throws<ArgumentException>(() => converter.Convert(" "));
        Assert.Equal("Input words cannot be empty.", empty.Message);

        Assert.False(converter.TryConvert("twenhundred", out var parsed, out var unrecognizedWord));
        Assert.Equal(0, parsed);
        Assert.Equal("twenhundred", unrecognizedWord);

        var invalid = Assert.Throws<ArgumentException>(() => converter.Convert("mystery"));
        Assert.Equal("Unrecognized number word: mystery", invalid.Message);
    }

    [Theory]
    [InlineData("十", 10)]
    [InlineData("万", 10000)]
    [InlineData("負二", -2)]
    [InlineData("第二目", 2)]
    [InlineData("三百二十万五", 3200005)]
    public void EastAsianSingleCharacterConverterParsesConfiguredBranches(string words, long expected)
    {
        var converter = new EastAsianPositionalWordsToNumberConverter(EastAsianSingleCharacterProfile);

        Assert.True(converter.TryConvert(words, out var parsed, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
        Assert.Equal(expected, converter.Convert(words));
    }

    [Theory]
    [InlineData("ten", 10)]
    [InlineData("thousand", 1000)]
    [InlineData("twotenthousandone", 20001)]
    public void EastAsianMultiCharacterConverterParsesConfiguredBranches(string words, long expected)
    {
        var converter = new EastAsianPositionalWordsToNumberConverter(EastAsianMultiCharacterProfile);

        Assert.True(converter.TryConvert(words, out var parsed, out var unrecognizedWord));
        Assert.Equal(expected, parsed);
        Assert.Null(unrecognizedWord);
        Assert.Equal(expected, converter.Convert(words));
    }

    [Fact]
    public void EastAsianConverterReportsEmptyAndInvalidInputs()
    {
        var converter = new EastAsianPositionalWordsToNumberConverter(EastAsianSingleCharacterProfile);

        var empty = Assert.Throws<ArgumentException>(() => converter.Convert(" "));
        Assert.Equal("Input words cannot be empty.", empty.Message);

        Assert.False(converter.TryConvert("一x", out var parsed, out var unrecognizedWord));
        Assert.Equal(0, parsed);
        Assert.Equal("一", unrecognizedWord);

        var multi = new EastAsianPositionalWordsToNumberConverter(EastAsianMultiCharacterProfile);
        Assert.False(multi.TryConvert("onemystery", out parsed, out unrecognizedWord));
        Assert.Equal(0, parsed);
        Assert.Equal("one", unrecognizedWord);
    }

    [UseCulture("en")]
    [Fact]
    public void ByteSizeCoversComparisonOperatorsAndFallbackFormatting()
    {
        var one = ByteSize.FromBytes(1);
        var two = ByteSize.FromBytes(2);

        Assert.False(one.Equals(null));
        Assert.False(one.Equals("1 B"));
        Assert.True(one.Equals(ByteSize.FromBits(8)));
        Assert.Equal(1, one.CompareTo(null));
        var exception = Assert.Throws<ArgumentException>(() => one.CompareTo("1 B"));
        Assert.Equal("Object is not a ByteSize", exception.Message);

        Assert.True(one == ByteSize.FromBits(8));
        Assert.True(one != two);
        Assert.True(one < two);
        Assert.True(one <= two);
        Assert.True(two > one);
        Assert.True(two >= one);

        var incremented = one;
        incremented++;
        Assert.Equal(two, incremented);

        var decremented = two;
        decremented--;
        Assert.Equal(one, decremented);

        Assert.Equal(ByteSize.FromBytes(-1), -one);
        Assert.Equal("1 byte", one.ToFullWords());
        Assert.Equal("2 bytes", two.ToFullWords());
        Assert.Equal("0 b", ByteSize.FromBits(0).ToString());
        Assert.Equal("0 bit", ByteSize.FromBits(0).ToFullWords());
    }

    [UseCulture("en")]
    [Fact]
    public void ByteSizeParsingCoversBitValidationAndSpanProviderBranches()
    {
        Assert.True(ByteSize.TryParse("8b".AsSpan(), CultureInfo.InvariantCulture, out var bits));
        Assert.Equal(ByteSize.FromBits(8), bits);

        Assert.True(ByteSize.TryParse("1B".AsSpan(), CultureInfo.InvariantCulture, out var bytes));
        Assert.Equal(ByteSize.FromBytes(1), bytes);

        Assert.False(ByteSize.TryParse("1.5b", out _));
        Assert.False(ByteSize.TryParse("NaN b", out _));
        Assert.False(ByteSize.TryParse(new string('9', 40) + "b", out _));
        Assert.False(ByteSize.TryParse("1XB", out _));
    }

    [UseCulture("en")]
    [Fact]
    public void DefaultFormatterCoversPhraseTableBranches()
    {
        var formatter = new DefaultFormatter("en");

        Assert.Equal("now", formatter.DateHumanize(TimeUnit.Day, Tense.Future, 0));
        Assert.Equal("yesterday", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 1));
        Assert.Equal("2 days ago", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 2));
        Assert.Equal("2 days from now", formatter.DateHumanize(TimeUnit.Day, Tense.Future, 2));

        Assert.Equal("no time", formatter.TimeSpanHumanize(TimeUnit.Second, 0, toWords: true));
        Assert.Equal("one second", formatter.TimeSpanHumanize(TimeUnit.Second, 1, toWords: true));
        Assert.Equal("2 seconds", formatter.TimeSpanHumanize(TimeUnit.Second, 2));

        Assert.Equal("B", formatter.DataUnitHumanize(DataUnit.Byte, 1));
        Assert.Equal("byte", formatter.DataUnitHumanize(DataUnit.Byte, 1, toSymbol: false));
        Assert.Equal("bytes", formatter.DataUnitHumanize(DataUnit.Byte, 2, toSymbol: false));
        Assert.Equal("s", formatter.TimeUnitHumanize(TimeUnit.Second));
        Assert.Equal("{value} old", formatter.TimeSpanHumanize_Age());
    }

    [Fact]
    public void SmallUtilityBranchesCoverInvalidAndFallbackPaths()
    {
        Assert.Equal("hello", "HELLO".Transform(CultureInfo.InvariantCulture, To.LowerCase));
        Assert.Throws<ArgumentOutOfRangeException>(() => "hello".ApplyCase((LetterCasing)42));
        Assert.Equal("gudde", EifelerRule.Apply("gudden"));

        var headingTable = new HeadingTable(["North"], ["N"]);
        Assert.False(headingTable.TryParseAbbreviated("missing", CultureInfo.InvariantCulture, out var heading));
        Assert.Equal(-1, heading);

        var invariantHeadingTable = (HeadingTable)typeof(HeadingTableCatalog)
            .GetProperty("Invariant", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)!
            .GetValue(null)!;
        Assert.Equal("N", invariantHeadingTable.GetHeading(HeadingStyle.Full, 0));

        Assert.Equal("dual", new LocalizedPhraseForms("default", Dual: "dual").Resolve(FormatterNumberForm.Dual));
        Assert.Equal("paucal", new LocalizedPhraseForms("default", Paucal: "paucal").Resolve(FormatterNumberForm.Paucal));
        Assert.Equal("plural", new LocalizedPhraseForms("default", Plural: "plural").Resolve(FormatterNumberForm.Plural));
        Assert.Equal("default", new LocalizedPhraseForms("default").Resolve((FormatterNumberForm)42));

        Assert.Equal("1st", "1".Ordinalize());
        Assert.Equal("1st", "1".Ordinalize(WordForm.Normal));
        Assert.Equal("1st", "1".Ordinalize(GrammaticalGender.Masculine, WordForm.Normal));
        Assert.Equal("1st", 1.Ordinalize(WordForm.Normal));
        Assert.Equal("1st", 1.Ordinalize(GrammaticalGender.Masculine, WordForm.Normal));

        var formatter = new DefaultFormatter("en");
        var registry = new LocaliserRegistry<IFormatter>(new DefaultFormatter("fr"));
        registry.Register("en-US", formatter);
        Assert.Same(formatter, registry.ResolveForCulture(new CultureInfo("en-US")));
    }

    [Fact]
    public void DefaultRegistriesAndBaseConvertersCoverFallbackFactories()
    {
        var unsupportedCulture = new CultureInfo("eo");

        Assert.IsType<DefaultDateToOrdinalWordConverter>(
            Configurator.DateToOrdinalWordsConverters.ResolveForCulture(unsupportedCulture));

#if NET6_0_OR_GREATER
        Assert.IsType<DefaultDateOnlyToOrdinalWordConverter>(
            Configurator.DateOnlyToOrdinalWordsConverters.ResolveForCulture(unsupportedCulture));
        Assert.NotNull(Configurator.TimeOnlyToClockNotationConverters.ResolveForCulture(unsupportedCulture));
#endif

        var converter = new ScaleStrategyNumberToWordsConverter(CreateScaleStrategyProfile(
            ScaleStrategyCardinalMode.NorwegianBokmal,
            ScaleStrategyOrdinalMode.NorwegianBokmal));

        Assert.Equal("two", converter.Convert(2, WordForm.Normal));
    }

    [Fact]
    public void DefaultFormatterCoversMissingPhraseBranches()
    {
        var empty = new FormatterHarness(CreateLocalePhraseTable());

        Assert.True(empty.HasPhraseTable);
        Assert.Throws<InvalidOperationException>(() => empty.DateHumanize(TimeUnit.Day, Tense.Future, 1));
        Assert.Throws<InvalidOperationException>(() => empty.TimeSpanHumanize(TimeUnit.Hour, 2));
        Assert.Throws<InvalidOperationException>(() => empty.DataUnitHumanize(DataUnit.Byte, 1));
        Assert.Throws<InvalidOperationException>(() => empty.TimeUnitHumanize(TimeUnit.Hour));

        var dataWithoutSymbol = new FormatterHarness(CreateLocalePhraseTable(dataUnit: new(Forms: new("byte"))));
        Assert.Throws<InvalidOperationException>(() => dataWithoutSymbol.DataUnitHumanize(DataUnit.Byte, 1));

        var dataWithoutForms = new FormatterHarness(CreateLocalePhraseTable(dataUnit: new(Symbol: "B")));
        Assert.Throws<InvalidOperationException>(() => dataWithoutForms.DataUnitHumanize(DataUnit.Byte, 1, toSymbol: false));

        var dateWithoutMultiple = new FormatterHarness(CreateLocalePhraseTable(dateFuture: new()));
        Assert.Throws<InvalidOperationException>(() => dateWithoutMultiple.DateHumanize(TimeUnit.Day, Tense.Future, 2));

        var timeSpanWithoutMultiple = new FormatterHarness(CreateLocalePhraseTable(timeSpan: new()));
        Assert.Throws<InvalidOperationException>(() => timeSpanWithoutMultiple.TimeSpanHumanize(TimeUnit.Hour, 2));
    }

    [Fact]
    public void DefaultFormatterCoversOverrideAndCountPlacementBranches()
    {
        var datePhrase = new LocalizedDatePhrase(
            Multiple: new(new("days"), PhraseCountPlacement.BeforeForm));
        var dateBlocked = new FormatterHarness(CreateLocalePhraseTable(dateFuture: datePhrase))
        {
            UseDatePhraseTable = false
        };
        Assert.Throws<InvalidOperationException>(() => dateBlocked.DateHumanize(TimeUnit.Day, Tense.Future, 2));

        var timeSpanPhrase = new LocalizedTimeSpanPhrase(
            Multiple: new(new("hours"), PhraseCountPlacement.BeforeForm));
        var timeSpanBlocked = new FormatterHarness(CreateLocalePhraseTable(timeSpan: timeSpanPhrase))
        {
            UseTimeSpanPhraseTable = false
        };
        Assert.Throws<InvalidOperationException>(() => timeSpanBlocked.TimeSpanHumanize(TimeUnit.Hour, 2));

        var afterCount = new FormatterHarness(CreateLocalePhraseTable(timeSpan: new(
            Multiple: new(new("hours"), PhraseCountPlacement.AfterForm, BeforeCountText: "about", AfterCountText: "long"))));
        Assert.Equal("about hours 2 long", afterCount.TimeSpanHumanize(TimeUnit.Hour, 2));

        var fallbackPlacement = new FormatterHarness(CreateLocalePhraseTable(timeSpan: new(
            Multiple: new(new("{count} units"), (PhraseCountPlacement)42))));
        Assert.Equal("2 units", fallbackPlacement.TimeSpanHumanize(TimeUnit.Hour, 2));

        Assert.False(fallbackPlacement.CallShouldUseDatePhraseTemplate(TimeUnit.Day, Tense.Future, 2, datePhrase));
    }

    [Fact]
    public void DefaultFormatterCoversFallbackScalarAndVariantBranches()
    {
        var scalarFallbacks = new FormatterHarness(CreateLocalePhraseTable(
            timeSpan: new(
                Single: "1 hour",
                Multiple: new(new("hours"), PhraseCountPlacement.BeforeForm)),
            dataUnit: new(
                Forms: new("byte"),
                Template: new(null, "{count} {unit} total")),
            timeUnit: new(Forms: new("hour"))));

        Assert.Equal("now", scalarFallbacks.DateHumanize_Now());
        Assert.Equal("never", scalarFallbacks.DateHumanize_Never());
        Assert.Equal("no time", scalarFallbacks.TimeSpanHumanize_Zero());
        Assert.Equal("{0}", scalarFallbacks.TimeSpanHumanize_Age());
        Assert.Equal("1 hour", scalarFallbacks.TimeSpanHumanize(TimeUnit.Hour, 1, toWords: true));
        Assert.Equal("words-2 hours", scalarFallbacks.TimeSpanHumanize(TimeUnit.Hour, 2, toWords: true));
        Assert.Equal("2 hours", scalarFallbacks.TimeSpanHumanize(TimeUnit.Hour, 2));
        Assert.Equal("2 byte total", scalarFallbacks.DataUnitHumanize(DataUnit.Byte, 2, toSymbol: false));
        Assert.Throws<InvalidOperationException>(() => scalarFallbacks.TimeUnitHumanize(TimeUnit.Hour));

        var singleWithoutValue = new FormatterHarness(CreateLocalePhraseTable(timeSpan: new(
            Multiple: new(new("hours"), PhraseCountPlacement.None))));
        Assert.Equal("hours", singleWithoutValue.TimeSpanHumanize(TimeUnit.Hour, 1));
    }

    [Fact]
    public void DynamicCharacterPreservingTruncatorCoversNullEmptyAndDelimiterEdges()
    {
        var truncator = new DynamicNumberOfCharactersAndPreserveWordsTruncator();

        Assert.Null(truncator.Truncate(null, 3, "..."));
        Assert.Equal(string.Empty, truncator.Truncate(string.Empty, 3, "..."));
        Assert.Equal("abc", truncator.Truncate("abc", 3, "......"));
        Assert.Equal("...", truncator.Truncate("abcdef", 3, "..."));
        Assert.Equal(string.Empty, truncator.Truncate("abcdef", 2, "..."));
        Assert.Equal("...", truncator.Truncate("abcdef", 3, "...", TruncateFrom.Left));
        Assert.Equal(string.Empty, truncator.Truncate("abcdef", 2, "...", TruncateFrom.Left));
        Assert.Equal("   abc", truncator.Truncate("   abc", 3, "..."));
        Assert.Equal("abc   ", truncator.Truncate("abc   ", 3, "...", TruncateFrom.Left));
    }

    [Fact]
    public void DynamicCharacterPreservingTruncatorCoversOversizedDelimiterBranches()
    {
        var truncator = new DynamicNumberOfCharactersAndPreserveWordsTruncator();

        Assert.Equal(string.Empty, truncator.Truncate("alpha beta", 4, ".........."));
        Assert.Equal("beta", truncator.Truncate("alpha beta", 4, "..........", TruncateFrom.Left));
        Assert.Equal(string.Empty, truncator.Truncate("alphabet beta", 4, ".........."));
        Assert.Equal(string.Empty, truncator.Truncate("alpha betabet", 4, "..........", TruncateFrom.Left));
    }

    [Fact]
    public void DynamicCharacterPreservingTruncatorCoversDelimiterBoundaryBranches()
    {
        var truncator = new DynamicNumberOfCharactersAndPreserveWordsTruncator();

        Assert.Equal(string.Empty, truncator.Truncate("abc", -1, string.Empty));
        Assert.Equal("..", truncator.Truncate("   abc", 2, ".."));
        Assert.Equal("a", truncator.Truncate("a beta", 2, "...."));
        Assert.Equal("..", truncator.Truncate("a beta", 2, ".."));

        Assert.Equal(string.Empty, truncator.Truncate("abc", -1, string.Empty, TruncateFrom.Left));
        Assert.Equal("..", truncator.Truncate("abc   ", 2, "..", TruncateFrom.Left));
        Assert.Equal("a", truncator.Truncate("beta a", 2, "....", TruncateFrom.Left));
        Assert.Equal("..", truncator.Truncate("beta a", 2, "..", TruncateFrom.Left));
    }

    [Fact]
    public void FixedTruncatorsCoverNullAndTerminalFallbackBranches()
    {
        var fixedLength = new FixedLengthTruncator();
        Assert.Null(fixedLength.Truncate(null, 3, "..."));
        Assert.Equal("def", fixedLength.Truncate("abcdef", 3, "..........", TruncateFrom.Left));

        var fixedCharacters = new FixedNumberOfCharactersTruncator();
        Assert.Null(fixedCharacters.Truncate(null, 3, "..."));
        Assert.Equal("def", fixedCharacters.Truncate("abcdef", 3, "..........", TruncateFrom.Left));
        Assert.Equal("abcde", fixedCharacters.Truncate("abcde", 2, ".."));

        var fixedWords = new FixedNumberOfWordsTruncator();
        Assert.Null(fixedWords.Truncate(null, 1, "..."));
        Assert.Equal("one...", fixedWords.Truncate("one two", 1, "..."));
        Assert.Equal("...two", fixedWords.Truncate("one two", 1, "...", TruncateFrom.Left));
        Assert.Equal("onetwo...", fixedWords.Truncate("onetwo", 0, "..."));
        Assert.Equal("...onetwo", fixedWords.Truncate("onetwo", 0, "...", TruncateFrom.Left));

        var dynamicWords = new DynamicLengthAndPreserveWordsTruncator();
        Assert.Null(dynamicWords.Truncate(null, 1, "..."));
        Assert.Equal("...", dynamicWords.Truncate("supercalifragilistic", 8, "..."));
        Assert.Equal("...", dynamicWords.Truncate("   super", 5, "..."));
        Assert.Equal("...", dynamicWords.Truncate("supercalifragilistic", 8, "...", TruncateFrom.Left));
    }

    [Fact]
    public void SuffixOrdinalizerCoversConvenienceConstructorAndZeroOption()
    {
        Assert.Equal("1st", new SuffixOrdinalizer("st").Convert(1, "1"));

        var ordinalizer = new SuffixOrdinalizer("m", "f", "n", zeroAsPlainNumber: true);
        Assert.Equal("0", ordinalizer.Convert(0, "zero", GrammaticalGender.Feminine));
        Assert.Equal("2n", ordinalizer.Convert(2, "2", GrammaticalGender.Neuter));
    }

    [Fact]
    public void WordFormTemplateOrdinalizerCoversExactAndNegativeModes()
    {
        var ordinalizer = new WordFormTemplateOrdinalizer(
            new("fr-FR"),
            CreateWordFormTemplateOrdinalizerOptions());

        Assert.Equal("two-m", ordinalizer.Convert(2, "2"));
        Assert.Equal("m-3-exact", ordinalizer.Convert(3, "3"));
        Assert.Equal("f-14-last-f", ordinalizer.Convert(14, "14", GrammaticalGender.Feminine));
        Assert.Equal("n-9-n", ordinalizer.Convert(9, "9", GrammaticalGender.Neuter));
        Assert.Equal("am-5-am", ordinalizer.Convert(5, "5", GrammaticalGender.Masculine, WordForm.Abbreviation));

        var negativeNone = new WordFormTemplateOrdinalizer(
            CultureInfo.InvariantCulture,
            CreateWordFormTemplateOrdinalizerOptions(WordFormTemplateOrdinalizer.NegativeNumberMode.None));
        Assert.Equal("m--4-last-m", negativeNone.Convert(-4, "-4"));

        var negativeInvariant = new WordFormTemplateOrdinalizer(
            CultureInfo.InvariantCulture,
            CreateWordFormTemplateOrdinalizerOptions(WordFormTemplateOrdinalizer.NegativeNumberMode.AbsoluteInvariant));
        Assert.Equal("m-4-last-m", negativeInvariant.Convert(-4, "-4"));

        var negativeCulture = new WordFormTemplateOrdinalizer(
            new("fr-FR"),
            CreateWordFormTemplateOrdinalizerOptions(WordFormTemplateOrdinalizer.NegativeNumberMode.AbsoluteCulture));
        Assert.Equal("m-4-last-m", negativeCulture.Convert(-4, "-4"));

        var minValueOrdinalizer = new WordFormTemplateOrdinalizer(
            CultureInfo.InvariantCulture,
            CreateWordFormTemplateOrdinalizerOptions(minValueAsPlainNumber: true));
        Assert.Equal("0", minValueOrdinalizer.Convert(int.MinValue, int.MinValue.ToString(CultureInfo.InvariantCulture)));

        var invalidNegativeMode = new WordFormTemplateOrdinalizer(
            CultureInfo.InvariantCulture,
            CreateWordFormTemplateOrdinalizerOptions((WordFormTemplateOrdinalizer.NegativeNumberMode)42));
        Assert.Throws<InvalidOperationException>(() => invalidNegativeMode.Convert(-1, "-1"));
    }

    [Fact]
    public void TokenMapWordsToNumberNormalizerCoversFastAndBuilderEdges()
    {
        Assert.Equal(string.Empty, TokenMapWordsToNumberNormalizer.Normalize("   ", TokenMapNormalizationProfile.CollapseWhitespace));
        Assert.Equal("one two", TokenMapWordsToNumberNormalizer.Normalize(" one\t \n two ", TokenMapNormalizationProfile.CollapseWhitespace));

        Assert.Equal(string.Empty, TokenMapWordsToNumberNormalizer.Normalize(" ", TokenMapNormalizationProfile.LowercaseRemovePeriods));
        Assert.Equal("one two", TokenMapWordsToNumberNormalizer.Normalize("One,\tTwo.", TokenMapNormalizationProfile.LowercaseRemovePeriods));
        Assert.Equal("onetwo", TokenMapWordsToNumberNormalizer.Normalize("one,two", TokenMapNormalizationProfile.LowercaseRemovePeriods));
        Assert.Equal("one two", TokenMapWordsToNumberNormalizer.Normalize("one  two", TokenMapNormalizationProfile.LowercaseRemovePeriods));
        Assert.Equal("one two", TokenMapWordsToNumberNormalizer.Normalize("one\ttwo", TokenMapNormalizationProfile.LowercaseRemovePeriods));

        Assert.Equal(string.Empty, TokenMapWordsToNumberNormalizer.Normalize("", TokenMapNormalizationProfile.LowercaseReplacePeriodsWithSpaces));
        Assert.Equal("one two", TokenMapWordsToNumberNormalizer.Normalize("One,.\tTwo-", TokenMapNormalizationProfile.LowercaseReplacePeriodsWithSpaces));
        Assert.Equal("onetwo", TokenMapWordsToNumberNormalizer.Normalize("one,two", TokenMapNormalizationProfile.LowercaseReplacePeriodsWithSpaces));
        Assert.Equal("one two", TokenMapWordsToNumberNormalizer.Normalize("one  two", TokenMapNormalizationProfile.LowercaseReplacePeriodsWithSpaces));
        Assert.Equal("one two", TokenMapWordsToNumberNormalizer.Normalize("one\ttwo", TokenMapNormalizationProfile.LowercaseReplacePeriodsWithSpaces));

        Assert.Equal(string.Empty, TokenMapWordsToNumberNormalizer.Normalize(" ", TokenMapNormalizationProfile.PunctuationToSpacesRemoveDiacritics));
        Assert.Equal("A B", TokenMapWordsToNumberNormalizer.Normalize(" Á;B/ ", TokenMapNormalizationProfile.PunctuationToSpacesRemoveDiacritics));
        Assert.Equal("کی", TokenMapWordsToNumberNormalizer.Normalize("ك،ي\u200c", TokenMapNormalizationProfile.Persian));
        Assert.Equal("ک ی", TokenMapWordsToNumberNormalizer.Normalize("ك\u200cي", TokenMapNormalizationProfile.Persian));
    }

    [Theory]
    [InlineData("minus 42", -42)]
    [InlineData("one hundred of three", 103)]
    public void InvertedTensConverterCoversNegativeIntegerAndIgnoredRemainderBranches(string words, long expected)
    {
        var converter = new InvertedTensWordsToNumberConverter(InvertedTensProfile);

        Assert.True(converter.TryConvert(words, out var parsed));
        Assert.Equal(expected, parsed);
    }

    [Fact]
    public void InvertedTensConverterCoversCollapsedOptionalAndIgnoredRecoveryBranches()
    {
        var converter = new InvertedTensWordsToNumberConverter(InvertedTensProfile);

        var collapsed = InvokeInvertedTensTryParseCompact(converter, "foo en zig");
        Assert.True(collapsed.Success);
        Assert.Equal(22, collapsed.Value);
        Assert.Null(collapsed.UnrecognizedWord);

        var emptyScaleTail = InvokeInvertedTensTryParseCompact(converter, "twothousand");
        Assert.True(emptyScaleTail.Success);
        Assert.Equal(2000, emptyScaleTail.Value);
        Assert.Null(emptyScaleTail.UnrecognizedWord);

        var suffixOnly = InvokeInvertedTensTryParseCompact(converter, "th");
        Assert.False(suffixOnly.Success);
        Assert.Equal("th", suffixOnly.UnrecognizedWord);

        Assert.Equal(string.Empty, InvokePrivate<string>(typeof(InvertedTensWordsToNumberConverter), converter, "StripLeadingIgnoredTokens", string.Empty));
        Assert.Equal("one", InvokePrivate<string>(typeof(InvertedTensWordsToNumberConverter), converter, "StripLeadingIgnoredTokens", "and of one"));
        Assert.Equal("one", InvokePrivate<string>(typeof(InvertedTensWordsToNumberConverter), converter, "StripLeadingIgnoredTokens", "andone"));
    }

    [Theory]
    [InlineData("minus one", -1)]
    [InlineData("first", 1)]
    [InlineData("one hundred five", 105)]
    [InlineData("twothousandandfive", 2005)]
    [InlineData("two thousand five", 2005)]
    [InlineData("two thousand", 2000)]
    [InlineData("twenty", 20)]
    [InlineData("twentyfive", 25)]
    public void CompoundScaleConverterCoversScaleSequenceAndOptionalBranches(string words, long expected)
    {
        var converter = new CompoundScaleWordsToNumberConverter(CompoundScaleProfile);

        Assert.True(converter.TryConvert(words, out var parsed));
        Assert.Equal(expected, parsed);
        Assert.Equal(expected, converter.Convert(words));
    }

    [Fact]
    public void CompoundScaleConverterReportsEmptyAndInvalidInputs()
    {
        var converter = new CompoundScaleWordsToNumberConverter(CompoundScaleProfile);

        Assert.Equal("Input words cannot be empty.", Assert.Throws<ArgumentException>(() => converter.Convert(" ")).Message);
        Assert.False(converter.TryConvert("one mystery", out var parsed, out var unrecognizedWord));
        Assert.Equal(0, parsed);
        Assert.Equal("mystery", unrecognizedWord);
        Assert.Equal("Unrecognized number word: mystery", Assert.Throws<ArgumentException>(() => converter.Convert("mystery")).Message);
    }

    [Theory]
    [InlineData("minus one", -1)]
    [InlineData("first", 1)]
    [InlineData("score one", 20)]
    [InlineData("score five", 25)]
    [InlineData("twenty teen three", 33)]
    [InlineData("two hundred three", 203)]
    [InlineData("two thousand three", 2003)]
    public void VigesimalCompoundConverterCoversLookaheadAndScaleBranches(string words, long expected)
    {
        var converter = new VigesimalCompoundWordsToNumberConverter(VigesimalProfile);

        Assert.True(converter.TryConvert(words, out var parsed));
        Assert.Equal(expected, parsed);
        Assert.Equal(expected, converter.Convert(words));
    }

    [Fact]
    public void VigesimalCompoundConverterReportsEmptyAndInvalidInputs()
    {
        var converter = new VigesimalCompoundWordsToNumberConverter(VigesimalProfile);

        Assert.Equal("Input words cannot be empty.", Assert.Throws<ArgumentException>(() => converter.Convert("")).Message);
        Assert.False(converter.TryConvert("one mystery", out var parsed, out var unrecognizedWord));
        Assert.Equal(0, parsed);
        Assert.Equal("mystery", unrecognizedWord);
        Assert.Equal("Unrecognized number word: mystery", Assert.Throws<ArgumentException>(() => converter.Convert("mystery")).Message);
    }

    [Theory]
    [InlineData("minus one", -1)]
    [InlineData("first", 1)]
    [InlineData("21st", 21)]
    [InlineData("one hundred two", 102)]
    [InlineData("twothousandtwo", 2002)]
    [InlineData("and one", 1)]
    [InlineData("úno", 1)]
    public void GreedyCompoundConverterCoversNormalizationGreedyAndOrdinalBranches(string words, long expected)
    {
        var converter = new GreedyCompoundWordsToNumberConverter(GreedyProfile);

        Assert.True(converter.TryConvert(words, out var parsed));
        Assert.Equal(expected, parsed);
        Assert.Equal(expected, converter.Convert(words));
    }

    [Fact]
    public void GreedyCompoundConverterReportsEmptyAndInvalidInputs()
    {
        var converter = new GreedyCompoundWordsToNumberConverter(GreedyProfile);

        Assert.Equal("Input words cannot be empty.", Assert.Throws<ArgumentException>(() => converter.Convert(" ")).Message);
        Assert.False(converter.TryConvert("one mystery", out var parsed, out var unrecognizedWord));
        Assert.Equal(0, parsed);
        Assert.Equal("mystery", unrecognizedWord);
        Assert.Equal("Unrecognized number word: mystery", Assert.Throws<ArgumentException>(() => converter.Convert("mystery")).Message);
    }

    [Fact]
    public void GreedyCompoundConverterCoversEmptyNormalizedAndNoAbbreviationBranches()
    {
        var converter = new GreedyCompoundWordsToNumberConverter(GreedyProfile);

        Assert.False(converter.TryConvert(",", out var parsed, out var unrecognizedWord));
        Assert.Equal(0, parsed);
        Assert.Equal(string.Empty, unrecognizedWord);

        var noAbbreviationConverter = new GreedyCompoundWordsToNumberConverter(new GreedyCompoundWordsToNumberProfile(
            GreedyProfile.CardinalMap,
            GreedyProfile.OrdinalMap,
            GreedyProfile.NegativePrefixes,
            GreedyProfile.IgnoredTokens,
            [],
            GreedyProfile.CharactersToRemove,
            GreedyProfile.CharactersToReplaceWithSpace,
            GreedyProfile.TextReplacements,
            GreedyProfile.Lowercase,
            GreedyProfile.RemoveDiacritics));
        Assert.False(noAbbreviationConverter.TryConvert("21st", out parsed, out unrecognizedWord));
        Assert.Equal("21st", unrecognizedWord);
        Assert.Equal("one", InvokePrivate<string>(
            typeof(GreedyCompoundWordsToNumberConverter),
            null,
            "Normalize",
            [typeof(string), typeof(string), typeof(string), typeof(StringReplacement[]), typeof(bool), typeof(bool)],
            " ONE- ",
            string.Empty,
            "-",
            Array.Empty<StringReplacement>(),
            true,
            false));
    }

    [Fact]
    public void NumberToWordsLocaleSmokeCoversSharedConverterFamilies()
    {
        string[] locales =
        [
            "ar", "az", "bg", "ca", "cs", "da", "de", "el", "es", "fa", "fi-FI",
            "fr", "he", "hr", "hu", "is", "it", "ja", "ko", "lb", "lt", "lv",
            "mt", "nb", "nl", "pl", "pt", "ro", "ru", "sk", "sl", "sr",
            "sr-Latn", "ta", "tr", "uk", "ur", "vi", "zh-CN"
        ];
        int[] numbers = [0, 1, 2, 5, 11, 21, 99, 100, 101, 115, 999, 1000, 1001, 2000, 5000, 1_000_000, -1001];
        int[] ordinals = [1, 2, 3, 10, 21, 100, 1000];

        foreach (var locale in locales)
        {
            var culture = new CultureInfo(locale);
            foreach (var number in numbers)
            {
                Assert.False(string.IsNullOrWhiteSpace(number.ToWords(culture)));
            }

            foreach (var ordinal in ordinals)
            {
                Assert.False(string.IsNullOrWhiteSpace(ordinal.ToOrdinalWords(culture)));
            }
        }

        var catalan = new CultureInfo("ca");
        Assert.Throws<NotImplementedException>(() => 1_000_000_000L.ToWords(catalan));
        Assert.Throws<NotImplementedException>(() => 1_000_000_000.ToOrdinalWords(catalan));
        Assert.False(string.IsNullOrWhiteSpace(21.ToOrdinalWords(GrammaticalGender.Masculine, WordForm.Normal, catalan)));
        Assert.False(string.IsNullOrWhiteSpace(2.ToOrdinalWords(GrammaticalGender.Masculine, WordForm.Abbreviation, catalan)));

        Assert.Throws<NotImplementedException>(() => 1_000_000_000_000L.ToWords(new CultureInfo("pt")));
        Assert.Throws<NotImplementedException>(() => long.MinValue.ToWords(new CultureInfo("cs")));
        Assert.Throws<ArgumentOutOfRangeException>(() => 0.ToOrdinalWords((GrammaticalGender)999, new CultureInfo("bg")));
        Assert.Throws<ArgumentOutOfRangeException>(() => 100.ToOrdinalWords((GrammaticalGender)999, new CultureInfo("bg")));
        Assert.Throws<ArgumentOutOfRangeException>(() => 1.ToOrdinalWords((GrammaticalGender)999, new CultureInfo("bg")));
        Assert.Throws<ArgumentOutOfRangeException>(() => 2.ToOrdinalWords((GrammaticalGender)999, new CultureInfo("is")));
        Assert.Throws<ArgumentOutOfRangeException>(() => 1.ToWords((GrammaticalGender)999, new CultureInfo("is")));
        Assert.Throws<ArgumentOutOfRangeException>(() => 1.ToWords((GrammaticalGender)999, new CultureInfo("cs")));
        Assert.Throws<NotImplementedException>(() => ((long)int.MaxValue + 1).ToWords(new CultureInfo("it")));
        Assert.Throws<NotImplementedException>(() => ((long)int.MaxValue + 1).ToWords(new CultureInfo("ro")));
        Assert.Throws<ArgumentOutOfRangeException>(() => 1.ToOrdinalWords((GrammaticalGender)999, new CultureInfo("de")));
        Assert.False(string.IsNullOrWhiteSpace(0.ToOrdinalWords(new CultureInfo("lv"))));
        Assert.False(string.IsNullOrWhiteSpace(2.ToWords(GrammaticalGender.Feminine, new CultureInfo("lv"))));
    }

    [Theory]
    [InlineData((int)GrammaticalGender.Masculine, 1, "primer")]
    [InlineData((int)GrammaticalGender.Neuter, 1, "primer")]
    [InlineData((int)GrammaticalGender.Masculine, 2, "segundo")]
    [InlineData((int)GrammaticalGender.Neuter, 2, "segundo")]
    [InlineData((int)GrammaticalGender.Masculine, 3, "tercer")]
    [InlineData((int)GrammaticalGender.Neuter, 3, "tercer")]
    public void SpanishLongScaleStemOrdinalCoversAbbreviatedOrdinalUnits(int genderValue, int number, string expected)
    {
        var spanish = new CultureInfo("es");
        var gender = (GrammaticalGender)genderValue;

        Assert.Equal(expected, number.ToOrdinalWords(gender, WordForm.Abbreviation, spanish));
    }

    [Fact]
    public void SpanishLongScaleStemOrdinalCoversFeminineOrdinalUnits()
    {
        var spanish = new CultureInfo("es");

        Assert.Equal("primera", 1.ToOrdinalWords(GrammaticalGender.Feminine, spanish));
    }

    [Fact]
    public void SpanishLongScaleStemOrdinalCoversGenderedCardinalUnits()
    {
        var spanish = new CultureInfo("es");

        Assert.Equal("un", 1.ToWords(WordForm.Abbreviation, GrammaticalGender.Masculine, spanish));
        Assert.Equal("un", 1.ToWords(WordForm.Abbreviation, GrammaticalGender.Neuter, spanish));
        Assert.Equal("uno", 1.ToWords(WordForm.Normal, GrammaticalGender.Masculine, spanish));
        Assert.Equal("uno", 1.ToWords(WordForm.Normal, GrammaticalGender.Neuter, spanish));
        Assert.Equal("una", 1.ToWords(GrammaticalGender.Feminine, spanish));
    }

    [Theory]
    [InlineData(31_000, false)]
    [InlineData(40_000, true)]
    [InlineData(100_000, true)]
    [InlineData(1_000_000, true)]
    [InlineData(10_000_000, true)]
    [InlineData(100_000_000, true)]
    [InlineData(1_000_000_000, true)]
    [InlineData(2_000_000_000, true)]
    [InlineData(int.MaxValue, false)]
    public void SpanishLongScaleStemOrdinalCoversRoundNumberBoundaries(int number, bool expected)
    {
        Assert.Equal(expected, InvokePrivate<bool>(
            typeof(LongScaleStemOrdinalNumberToWordsConverter),
            null,
            "IsRoundNumber",
            number));
    }

    [Fact]
    public void PluralizedScaleConverterCoversCardinalScaleFormsAndUnitStrategies()
    {
        var polish = new PluralizedScaleNumberToWordsConverter(
            CreatePluralizedScaleProfile(PluralizedScaleFormDetector.Polish, PluralizedScaleUnitVariantStrategy.Polish),
            CultureInfo.InvariantCulture);

        Assert.Equal("zero", polish.Convert(0));
        Assert.Equal("minus jeden thousand-one dwie", polish.Convert(-1002, GrammaticalGender.Feminine));
        Assert.Equal("dwa thousand-few", polish.Convert(2000));
        Assert.Equal("five thousand-many", polish.Convert(5000));
        Assert.Equal("hundred", polish.Convert(100));
        Assert.Equal("twenty jeden", polish.Convert(21));

        var lithuanian = new PluralizedScaleNumberToWordsConverter(
            CreatePluralizedScaleProfile(PluralizedScaleFormDetector.Lithuanian, PluralizedScaleUnitVariantStrategy.Lithuanian),
            CultureInfo.InvariantCulture);

        Assert.Equal("dvi", lithuanian.Convert(2, GrammaticalGender.Feminine));
        Assert.Equal("viena", lithuanian.Convert(1, GrammaticalGender.Feminine));
        Assert.Equal("septynios", lithuanian.Convert(7, GrammaticalGender.Feminine));

        var invariant = new PluralizedScaleNumberToWordsConverter(
            CreatePluralizedScaleProfile(PluralizedScaleFormDetector.RussianPaucal, PluralizedScaleUnitVariantStrategy.None),
            CultureInfo.InvariantCulture);
        Assert.Equal("vienas thousand-one", invariant.Convert(1000));
    }

    [Fact]
    public void PluralizedScaleConverterCoversLithuanianOrdinalsAndInvalidModes()
    {
        var converter = new PluralizedScaleNumberToWordsConverter(
            CreatePluralizedScaleProfile(PluralizedScaleFormDetector.Lithuanian, PluralizedScaleUnitVariantStrategy.Lithuanian),
            CultureInfo.InvariantCulture);

        Assert.Equal("zerothis", converter.ConvertToOrdinal(0));
        Assert.Equal("zerothė", converter.ConvertToOrdinal(0, GrammaticalGender.Feminine));
        Assert.Equal("thousandthmasc", converter.ConvertToOrdinal(1000));
        Assert.Equal("du thousandthfem", converter.ConvertToOrdinal(2000, GrammaticalGender.Feminine));
        Assert.Equal("hundredthmasc", converter.ConvertToOrdinal(100));
        Assert.Equal("twentiethfem", converter.ConvertToOrdinal(20, GrammaticalGender.Feminine));
        Assert.Equal("twenty firstmasc", converter.ConvertToOrdinal(21));

        var numeric = new PluralizedScaleNumberToWordsConverter(
            CreatePluralizedScaleProfile(
                PluralizedScaleFormDetector.Polish,
                PluralizedScaleUnitVariantStrategy.Polish,
                PluralizedScaleOrdinalMode.NumericCulture),
            CultureInfo.InvariantCulture);
        Assert.Equal("12", numeric.ConvertToOrdinal(12));

        var invalidOrdinal = new PluralizedScaleNumberToWordsConverter(
            CreatePluralizedScaleProfile(
                PluralizedScaleFormDetector.Polish,
                PluralizedScaleUnitVariantStrategy.Polish,
                (PluralizedScaleOrdinalMode)42),
            CultureInfo.InvariantCulture);
        Assert.Throws<InvalidOperationException>(() => invalidOrdinal.ConvertToOrdinal(1));

        var invalidUnitStrategy = new PluralizedScaleNumberToWordsConverter(
            CreatePluralizedScaleProfile(PluralizedScaleFormDetector.Polish, (PluralizedScaleUnitVariantStrategy)42),
            CultureInfo.InvariantCulture);
        Assert.Throws<InvalidOperationException>(() => invalidUnitStrategy.Convert(1));

        var invalidDetector = new PluralizedScaleNumberToWordsConverter(
            CreatePluralizedScaleProfile((PluralizedScaleFormDetector)42, PluralizedScaleUnitVariantStrategy.None),
            CultureInfo.InvariantCulture);
        Assert.Throws<InvalidOperationException>(() => invalidDetector.Convert(1000));
    }

    [Fact]
    public void PluralizedScaleConverterCoversRemainingInternalDetectorAndGuardBranches()
    {
        var russian = new PluralizedScaleNumberToWordsConverter(
            CreatePluralizedScaleProfile(PluralizedScaleFormDetector.RussianPaucal, PluralizedScaleUnitVariantStrategy.None),
            CultureInfo.InvariantCulture);
        Assert.Equal("du thousand-few", russian.Convert(2000));
        Assert.Equal("five thousand-many", russian.Convert(5000));

        var lithuanian = new PluralizedScaleNumberToWordsConverter(
            CreatePluralizedScaleProfile(PluralizedScaleFormDetector.Lithuanian, PluralizedScaleUnitVariantStrategy.Lithuanian),
            CultureInfo.InvariantCulture);
        Assert.Equal("twenty", InvokePrivate<string>(
            typeof(PluralizedScaleNumberToWordsConverter),
            lithuanian,
            "GetCardinalUnit",
            [typeof(int), typeof(GrammaticalGender), typeof(bool)],
            20,
            GrammaticalGender.Masculine,
            false));
        Assert.Throws<System.Reflection.TargetInvocationException>(() => InvokePrivate<string>(
            typeof(PluralizedScaleNumberToWordsConverter),
            lithuanian,
            "GetLithuanianGenderedUnit",
            [typeof(string), typeof(GrammaticalGender)],
            "du",
            (GrammaticalGender)999));
        Assert.Throws<System.Reflection.TargetInvocationException>(() => InvokePrivate<string>(
            typeof(PluralizedScaleNumberToWordsConverter),
            lithuanian,
            "GetLithuanianOrdinalSuffix",
            [typeof(GrammaticalGender)],
            (GrammaticalGender)999));
    }

    [Fact]
    public void TerminalOrdinalScaleConverterCoversScaleAndGenderBranches()
    {
        var converter = new TerminalOrdinalScaleNumberToWordsConverter(CreateTerminalOrdinalScaleProfile());

        Assert.Equal("minus ones", converter.Convert(-1));
        Assert.Equal("one-thousand-with one-hundred-after", converter.Convert(1100));
        Assert.Equal("twoi thousands", converter.Convert(2000));
        Assert.Equal("minus first-m", converter.ConvertToOrdinal(-1));
        Assert.Equal("thousandth-f", converter.ConvertToOrdinal(1000, GrammaticalGender.Feminine));
        Assert.Equal("twoi thousandth-m", converter.ConvertToOrdinal(2000));
        Assert.Equal("twenty-m", converter.ConvertToOrdinal(20));
        Assert.Equal("hundredth-m", converter.ConvertToOrdinal(100));
        Assert.Throws<System.Reflection.TargetInvocationException>(() => InvokePrivate<string>(
            typeof(TerminalOrdinalScaleNumberToWordsConverter),
            null,
            "GetCardinalUnitEnding",
            [typeof(GrammaticalGender), typeof(int)],
            (GrammaticalGender)999,
            1));
        Assert.Throws<System.Reflection.TargetInvocationException>(() => InvokePrivate<string>(
            typeof(TerminalOrdinalScaleNumberToWordsConverter),
            converter,
            "GetOrdinalSuffix",
            [typeof(GrammaticalGender)],
            (GrammaticalGender)999));
    }

    [Fact]
    public void ConjunctionalScaleConverterCoversRecursiveScaleLeadingOneAndTupleBranches()
    {
        var converter = new ConjunctionalScaleNumberToWordsConverter(CreateConjunctionalScaleProfile());

        Assert.Equal("minus one", converter.Convert(-1));
        Assert.Equal("one hundred", converter.Convert(100, addAnd: false));
        Assert.Equal("one hundred and one", converter.Convert(101));
        Assert.Equal("one thousand and one", converter.Convert(1001));
        Assert.Equal("one thousand thousand", converter.Convert(1_000_000));
        Assert.Equal("hundredth", converter.ConvertToOrdinal(100));
        Assert.Equal("thousandth", converter.ConvertToOrdinal(1000));
        Assert.Equal("twenty-one thousandth", converter.ConvertToOrdinal(21_000));
        Assert.Equal("pair", converter.ConvertToTuple(2));
        Assert.Equal("3-tuple", converter.ConvertToTuple(3));

        var afterScaleOnly = new ConjunctionalScaleNumberToWordsConverter(CreateConjunctionalScaleProfile(
            ConjunctionalScaleAndStrategy.AfterScaleSubHundredRemainderOnly));
        Assert.False(string.IsNullOrWhiteSpace(afterScaleOnly.Convert(1001)));

        var terminalScaleOnly = new ConjunctionalScaleNumberToWordsConverter(CreateConjunctionalScaleProfile(
            ConjunctionalScaleAndStrategy.WithinGroupAndTerminalScaleSubHundredRemainder));
        Assert.False(string.IsNullOrWhiteSpace(terminalScaleOnly.Convert(1001)));

        var invalid = new ConjunctionalScaleNumberToWordsConverter(CreateConjunctionalScaleProfile(andStrategy: (ConjunctionalScaleAndStrategy)42));
        Assert.Throws<InvalidOperationException>(() => invalid.Convert(101));
    }

    [Fact]
    public void ScaleStrategyConverterCoversNorwegianOrdinalAndErrorBranches()
    {
        var converter = new ScaleStrategyNumberToWordsConverter(CreateScaleStrategyProfile(
            ScaleStrategyCardinalMode.NorwegianBokmal,
            ScaleStrategyOrdinalMode.NorwegianBokmal));

        Assert.Equal("one-f", converter.Convert(1, GrammaticalGender.Feminine));
        Assert.Equal("one-n", converter.Convert(1, GrammaticalGender.Neuter));
        Assert.Equal("zeroth", converter.ConvertToOrdinal(0));
        Assert.Equal("hundredth-default", converter.ConvertToOrdinal(100));
        Assert.Equal("millionth-large", converter.ConvertToOrdinal(1_000_000));
        Assert.Equal("twentieth", converter.ConvertToOrdinal(20));
        Assert.Equal("sixth", converter.ConvertToOrdinal(6));
        Assert.Throws<NotImplementedException>(() => converter.Convert(long.MinValue));

        var invalidCardinal = new ScaleStrategyNumberToWordsConverter(CreateScaleStrategyProfile(
            (ScaleStrategyCardinalMode)42,
            ScaleStrategyOrdinalMode.NorwegianBokmal));
        Assert.Throws<InvalidOperationException>(() => invalidCardinal.Convert(1));
    }

    [Fact]
    public void ScaleStrategyConverterCoversSwedishOrdinalAndErrorBranches()
    {
        var converter = new ScaleStrategyNumberToWordsConverter(CreateScaleStrategyProfile(
            ScaleStrategyCardinalMode.Swedish,
            ScaleStrategyOrdinalMode.Swedish));

        Assert.Equal("minus one-m", converter.Convert(-1));
        Assert.Equal("one-m thousandth-scale", converter.ConvertToOrdinal(1_000));
        Assert.Equal("twentieth", converter.ConvertToOrdinal(20));
        Assert.Equal("twentyfirst", converter.ConvertToOrdinal(21));
        Assert.Throws<NotImplementedException>(() => converter.Convert(long.MinValue));

        var suffixOrdinal = new ScaleStrategyNumberToWordsConverter(CreateScaleStrategyProfile(
            ScaleStrategyCardinalMode.Swedish,
            ScaleStrategyOrdinalMode.Swedish,
            new Dictionary<int, string> { [0] = "zeroth", [1] = "first" }.ToFrozenDictionary()));
        Assert.Equal("twentyieth", suffixOrdinal.ConvertToOrdinal(20));

        var invalidOrdinal = new ScaleStrategyNumberToWordsConverter(CreateScaleStrategyProfile(
            ScaleStrategyCardinalMode.Swedish,
            (ScaleStrategyOrdinalMode)42));
        Assert.Throws<InvalidOperationException>(() => invalidOrdinal.ConvertToOrdinal(1));
    }

    [UseCulture("en-US")]
    [Fact]
    public void OrdinalDatePatternCoversMonthSubstitutionNonAdjacentAndNoMonthCases()
    {
        var months = Enumerable.Repeat("unused", 12).ToArray();
        months[0] = "Nom";
        var genitiveMonths = Enumerable.Repeat("unused", 12).ToArray();
        genitiveMonths[0] = "Gen";

        var noMonth = new OrdinalDatePattern("{day} yyyy", OrdinalDateDayMode.Numeric, months: months);
        Assert.Equal("2 2024", noMonth.Format(new DateTime(2024, 1, 2)));

        var nonAdjacentMonth = new OrdinalDatePattern("MMMM yyyy {day}", OrdinalDateDayMode.Numeric, months: months, monthsGenitive: genitiveMonths);
        Assert.Equal("Nom 2024 2", nonAdjacentMonth.Format(new DateTime(2024, 1, 2)));

        var dayOfWeekAdjacent = new OrdinalDatePattern("MMMM dddd {day}", OrdinalDateDayMode.Numeric, months: months, monthsGenitive: genitiveMonths);
        Assert.Equal("Nom Tuesday 2", dayOfWeekAdjacent.Format(new DateTime(2024, 1, 2)));
    }

    [UseCulture("en-US")]
    [Fact]
    public void OrdinalDatePatternCoversEscapedMonthShortMonthAndMarkerFallbacks()
    {
        var months = Enumerable.Repeat("unused", 12).ToArray();
        months[0] = "O'Clock";
        var genitiveMonths = Enumerable.Repeat("unused", 12).ToArray();
        genitiveMonths[0] = "Genitive";

        var escapedThenRealMonth = new OrdinalDatePattern("'MMMM' MMMM {day}", OrdinalDateDayMode.Numeric, months: months);
        Assert.Equal("MMMM OClock 2", escapedThenRealMonth.Format(new DateTime(2024, 1, 2)));

        var shortMonthPattern = new OrdinalDatePattern("MMM {day}", OrdinalDateDayMode.Numeric, months: months);
        Assert.Equal("Jan 2", shortMonthPattern.Format(new DateTime(2024, 1, 2)));

        var quotedLiteralBetweenDayAndMonth = new OrdinalDatePattern(
            "{day} 'literal' MMMM",
            OrdinalDateDayMode.Numeric,
            months: months,
            monthsGenitive: genitiveMonths);
        Assert.Equal("2 literal Genitive", quotedLiteralBetweenDayAndMonth.Format(new DateTime(2024, 1, 2)));

        var pattern = new OrdinalDatePattern("{day}", OrdinalDateDayMode.Numeric);
        Assert.Equal("prefix second", InvokePrivate<string>(typeof(OrdinalDatePattern), null, "ReplaceDayMarker", "prefix <<DAY>>", "second", 2));
        Assert.False(InvokePrivate<bool>(
            typeof(OrdinalDatePattern),
            null,
            "FindAdjacentDayOfMonth",
            [typeof(string), typeof(int), typeof(bool)],
            "'literal'",
            0,
            false));
    }

    [UseCulture("ar-SA")]
    [Fact]
    public void OrdinalDatePatternCoversGregorianCalendarFallbackCulture()
    {
        var pattern = new OrdinalDatePattern("{day} MMMM yyyy", OrdinalDateDayMode.Numeric);

        Assert.False(string.IsNullOrWhiteSpace(pattern.Format(new DateTime(2024, 1, 2))));
    }

#if NET6_0_OR_GREATER
    [Fact]
    public void PhraseClockNotationConverterCoversFixedBucketRangeDefaultAndFallbackPaths()
    {
        var profile = CreateClockProfile(
            midnight: "midnight",
            midday: "midday",
            min5: "{article} {hour} {minutes} {minuteSuffix} {dayPeriod}",
            defaultTemplate: "{hour}:{minutes} {dayPeriod}",
            pastHourTemplate: "{minutes} past {hour} {minuteSuffix}",
            beforeHalfTemplate: "{minutesFromHalf} before half {nextHour} {minuteSuffix}",
            afterHalfTemplate: "{minutesFromHalf} after half {hour} {minuteSuffix}",
            beforeNextTemplate: "{minutesReverse} before {nextArticle} {nextHour} {minuteSuffix}",
            minuteSuffixSingular: "minute",
            minuteSuffixPaucal: "minutes-paucal",
            minuteSuffixPlural: "minutes",
            singularArticle: "the",
            pluralArticle: "les",
            earlyMorning: "early",
            morning: "morning",
            afternoon: "afternoon",
            night: "night");
        var converter = new PhraseClockNotationConverter(profile);

        Assert.Equal("midnight", converter.Convert(new TimeOnly(0, 0), ClockNotationRounding.None));
        Assert.Equal("midday", converter.Convert(new TimeOnly(12, 0), ClockNotationRounding.None));
        Assert.Equal("the 1 5 minutes early", converter.Convert(new TimeOnly(1, 5), ClockNotationRounding.None));
        Assert.Equal("7 past 7 minutes morning", converter.Convert(new TimeOnly(7, 7), ClockNotationRounding.None));
        Assert.Equal("2 before half 8 minutes-paucal morning", converter.Convert(new TimeOnly(7, 28), ClockNotationRounding.None));
        Assert.Equal("2 after half 7 minutes-paucal morning", converter.Convert(new TimeOnly(7, 32), ClockNotationRounding.None));
        Assert.Equal("2 before les 8 minutes-paucal morning", converter.Convert(new TimeOnly(7, 58), ClockNotationRounding.None));
        Assert.Equal("23 past 13 minutes-paucal afternoon", converter.Convert(new TimeOnly(13, 23), ClockNotationRounding.None));
        Assert.Equal("2:0 early", converter.Convert(new TimeOnly(1, 58), ClockNotationRounding.NearestFiveMinutes));

        var fallback = new PhraseClockNotationConverter(CreateClockProfile());
        Assert.Equal("2 7", fallback.Convert(new TimeOnly(2, 7), ClockNotationRounding.None));
    }

    [Fact]
    public void PhraseClockNotationConverterCoversCompactAndEifelerTemplatePaths()
    {
        var minuteWords = Enumerable.Repeat(string.Empty, 60).ToArray();
        minuteWords[23] = "twenty three";
        var profile = CreateClockProfile(
            hourMode: PhraseClockHourMode.H12,
            hourOneWord: "een",
            hourTwelveWord: "twelve-word",
            hourSuffixSingular: "hour",
            hourSuffixPaucal: "hours-paucal",
            hourSuffixPlural: "hours",
            min0: "{hour} {minuteSuffix}",
            defaultTemplate: "{hour} {minutes} {minuteSuffix}",
            minuteSuffixSingular: "minute",
            minuteSuffixPlural: "minutes",
            minuteWordsMap: minuteWords,
            compactMinuteWords: true,
            applyEifelerRule: true);
        var converter = new PhraseClockNotationConverter(profile);

        Assert.Equal("ee minutes", converter.Convert(new TimeOnly(1, 0), ClockNotationRounding.None));
        Assert.Equal("twelve-word minutes", converter.Convert(new TimeOnly(12, 0), ClockNotationRounding.None));
        Assert.Equal("two hours-paucal minutes", converter.Convert(new TimeOnly(2, 0), ClockNotationRounding.None));
        Assert.Equal("two hours-paucal twenty three minutes", converter.Convert(new TimeOnly(2, 23), ClockNotationRounding.None));
    }

    [Fact]
    public void PhraseClockNotationConverterCoversMalformedTemplatesAndFallbackBranches()
    {
        var invalidMode = new PhraseClockNotationConverter(CreateClockProfile(
            hourMode: (PhraseClockHourMode)42,
            defaultTemplate: "{hour}"));
        Assert.Equal("one", invalidMode.Convert(new TimeOnly(13, 1), ClockNotationRounding.None));

        var sparseTemplate = new PhraseClockNotationConverter(CreateClockProfile(
            pastHourTemplate: "{minutes}  past {unknown} {hour}"));
        Assert.Equal("7 past 1", sparseTemplate.Convert(new TimeOnly(1, 7), ClockNotationRounding.None));

        var malformedBucket = new PhraseClockNotationConverter(CreateClockProfile(
            min5: "{hour} {"));
        Assert.Equal("1 {", malformedBucket.Convert(new TimeOnly(1, 5), ClockNotationRounding.None));

        var malformedLookahead = new PhraseClockNotationConverter(CreateClockProfile(
            defaultTemplate: "{hour} {",
            applyEifelerRule: true));
        Assert.Equal("1 {", malformedLookahead.Convert(new TimeOnly(1, 1), ClockNotationRounding.None));

        var hourWords = Enumerable.Repeat(string.Empty, 13).ToArray();
        hourWords[1] = "een een";
        var eifelerLastWord = new PhraseClockNotationConverter(CreateClockProfile(
            hourMode: PhraseClockHourMode.H12,
            defaultTemplate: "{hour} a",
            hourWordsMap: hourWords,
            applyEifelerRule: true));
        Assert.Equal("een een a", eifelerLastWord.Convert(new TimeOnly(1, 1), ClockNotationRounding.None));

        var trailingEifeler = new PhraseClockNotationConverter(CreateClockProfile(
            defaultTemplate: "{hour}",
            applyEifelerRule: true));
        Assert.Equal("1", trailingEifeler.Convert(new TimeOnly(1, 1), ClockNotationRounding.None));

        var placeholderLookahead = new PhraseClockNotationConverter(CreateClockProfile(
            defaultTemplate: "{hour} {dayPeriod}",
            night: "night time",
            applyEifelerRule: true));
        Assert.Equal("23 night time", placeholderLookahead.Convert(new TimeOnly(23, 1), ClockNotationRounding.None));

        var eifelerMultiWord = new PhraseClockNotationConverter(CreateClockProfile(
            hourMode: PhraseClockHourMode.H12,
            defaultTemplate: "{hour} b",
            hourWordsMap: hourWords,
            applyEifelerRule: true));
        Assert.Equal("een ee b", eifelerMultiWord.Convert(new TimeOnly(1, 1), ClockNotationRounding.None));

        var articleLookahead = new PhraseClockNotationConverter(CreateClockProfile(
            hourMode: PhraseClockHourMode.H12,
            defaultTemplate: "{hour} {article}",
            hourWordsMap: hourWords,
            singularArticle: "bad",
            applyEifelerRule: true));
        Assert.Equal("een ee bad", articleLookahead.Convert(new TimeOnly(1, 1), ClockNotationRounding.None));

        var nextArticleLookahead = new PhraseClockNotationConverter(CreateClockProfile(
            hourMode: PhraseClockHourMode.H12,
            defaultTemplate: "{hour} {nextArticle}",
            hourWordsMap: hourWords,
            singularArticle: "bad",
            pluralArticle: "bad",
            applyEifelerRule: true));
        Assert.Equal("een ee bad", nextArticleLookahead.Convert(new TimeOnly(1, 1), ClockNotationRounding.None));

        var suffixResolver = new PhraseClockNotationConverter(CreateClockProfile(
            minuteSuffixSingular: "minute",
            minuteSuffixPlural: "minutes"));
        Assert.Equal("minutes", InvokePrivate<string>(typeof(PhraseClockNotationConverter), suffixResolver, "ResolveMinuteSuffixForRange", 0));
        Assert.Equal(string.Empty, InvokePrivate<string>(
            typeof(PhraseClockNotationConverter),
            null,
            "ExtractNextWordResolvingPlaceholders",
            [typeof(string), typeof(int), typeof(string), typeof(string), typeof(string), typeof(string)],
            "   ",
            0,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty));
    }
#endif

    [UseCulture("en")]
    [Fact]
    public void ByteSizeAndMetricNumeralCoverRemainingPublicFormattingBranches()
    {
        var terabyte = ByteSize.FromTerabytes(1);
        Assert.Equal("TB", terabyte.LargestWholeNumberSymbol);
        Assert.Equal("terabyte", terabyte.LargestWholeNumberFullWord);
        Assert.True(ByteSize.FromBytes(1).Equals((object)ByteSize.FromBits(8)));
        Assert.NotEqual(0, ByteSize.FromBytes(1).GetHashCode());
        Assert.Equal("1.00 byte", ByteSize.FromBytes(1).ToFullWords("0.00 B"));
        Assert.Equal("1 TB", terabyte.ToString("TB"));

        Assert.False(string.IsNullOrWhiteSpace(999_999_999_999_999_999L.ToMetric()));
        Assert.False(string.IsNullOrWhiteSpace(999_999_999_999_999_999L.ToMetric(MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord, 3)));
        Assert.False(string.IsNullOrWhiteSpace(999_999_999_999_999_999d.ToMetric(MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseLongScaleWord, 4)));
        Assert.False(string.IsNullOrWhiteSpace(long.MaxValue.ToMetric()));
        Assert.False(string.IsNullOrWhiteSpace(InvokePrivate<string>(
            typeof(MetricNumeralExtensions),
            null,
            "BuildMetricRepresentation",
            [typeof(long), typeof(int), typeof(MetricNumeralFormats?), typeof(int?)],
            1_000_000L,
            1,
            null,
            null)));
    }

    [Fact]
    public void WordsToNumberAdditionalConverterFamiliesCoverGuardAndCompositionBranches()
    {
        var prefixed = new PrefixedTensScaleWordsToNumberConverter(PrefixedTensProfile);
        Assert.True(prefixed.TryConvert("twenty", out var parsed));
        Assert.Equal(20, parsed);
        Assert.False(prefixed.TryConvert("-", out parsed, out var unrecognizedWord));
        Assert.Equal(0, parsed);
        Assert.Equal(string.Empty, unrecognizedWord);

        var compound = new CompoundScaleWordsToNumberConverter(CompoundScaleProfile);
        object?[] emptyOptionalArguments = [string.Empty, 1L];
        Assert.True(InvokePrivate<bool>(
            typeof(CompoundScaleWordsToNumberConverter),
            compound,
            "TryParseOptional",
            [typeof(string), typeof(long).MakeByRefType()],
            emptyOptionalArguments));
        Assert.Equal(0L, emptyOptionalArguments[1]);

        object?[] optionalArguments = ["and five", 0L];
        Assert.True(InvokePrivate<bool>(
            typeof(CompoundScaleWordsToNumberConverter),
            compound,
            "TryParseOptional",
            [typeof(string), typeof(long).MakeByRefType()],
            optionalArguments));
        Assert.Equal(5L, optionalArguments[1]);
        object?[] gluedIgnoredArguments = ["andfive", 0L];
        Assert.True(InvokePrivate<bool>(
            typeof(CompoundScaleWordsToNumberConverter),
            compound,
            "TryParseOptional",
            [typeof(string), typeof(long).MakeByRefType()],
            gluedIgnoredArguments));
        Assert.Equal(5L, gluedIgnoredArguments[1]);

        var linking = new LinkingAffixWordsToNumberConverter(LinkingAffixProfile);
        Assert.True(linking.TryConvert("minus two", out parsed));
        Assert.Equal(-2, parsed);
        Assert.Equal(15, linking.Convert("teenfive"));
        Assert.Equal(2005, linking.Convert("two thousand and five"));
        Assert.Equal(3, linking.Convert("threeka"));
        Assert.Equal("Input words cannot be empty.", Assert.Throws<ArgumentException>(() => linking.Convert(" ")).Message);
        Assert.False(linking.TryConvert("two mystery", out parsed, out unrecognizedWord));
        Assert.Equal(0, parsed);
        Assert.Equal("mystery", unrecognizedWord);
        Assert.Equal("Unrecognized number word: mystery", Assert.Throws<ArgumentException>(() => linking.Convert("mystery")).Message);

        var contracted = new ContractedScaleWordsToNumberConverter(ContractedScaleProfile);
        Assert.True(contracted.TryConvert("minus satu", out parsed));
        Assert.Equal(-1, parsed);
        Assert.Equal(15, contracted.Convert("puluh lima"));
        Assert.Equal(11, contracted.Convert("satu belas"));
        Assert.Equal(15, contracted.Convert("lima belas"));
        Assert.Equal(25, contracted.Convert("dua puluh lima"));
        Assert.Equal(2005, contracted.Convert("dua ribu dan lima"));
        Assert.Equal("Input words cannot be empty.", Assert.Throws<ArgumentException>(() => contracted.Convert(" ")).Message);
        Assert.False(contracted.TryConvert("dua mystery", out parsed, out unrecognizedWord));
        Assert.Equal(0, parsed);
        Assert.Equal("mystery", unrecognizedWord);
        Assert.Equal("Unrecognized number word: mystery", Assert.Throws<ArgumentException>(() => contracted.Convert("mystery")).Message);

        var suffix = new SuffixScaleWordsToNumberConverter(SuffixScaleProfile);
        Assert.True(suffix.TryConvert("42", out parsed));
        Assert.Equal(42, parsed);
        Assert.Equal(7, suffix.Convert("two five"));
        object?[] suffixOptionalArguments = [string.Empty, 1L];
        Assert.True(InvokePrivate<bool>(
            typeof(SuffixScaleWordsToNumberConverter),
            suffix,
            "TryParseOptional",
            [typeof(string), typeof(long).MakeByRefType()],
            suffixOptionalArguments));
        Assert.Equal(0L, suffixOptionalArguments[1]);

        var eastAsian = new EastAsianPositionalWordsToNumberConverter(EastAsianSingleCharacterProfile);
        Assert.True(eastAsian.TryConvert("十", out parsed));
        Assert.Equal(10, parsed);
        Assert.Equal("Unrecognized number word: 一", Assert.Throws<ArgumentException>(() => eastAsian.Convert("一x")).Message);
    }

    [Fact]
    public void ProfiledFormatterCoversRemainingStaticDetectorAndFallbackBranches()
    {
        Assert.Equal(FormatterNumberForm.Plural, InvokePrivate<FormatterNumberForm>(
            typeof(ProfiledFormatter),
            null,
            "DetectDataUnitForm",
            [typeof(double), typeof(FormatterNumberDetectorKind), typeof(FormatterNumberForm)],
            1.5d,
            FormatterNumberDetectorKind.SingularPlural,
            FormatterNumberForm.Plural));
        Assert.Equal("dual", InvokePrivate<string>(
            typeof(ProfiledFormatter),
            null,
            "ResolveProfiledPhraseForms",
            [typeof(LocalizedPhraseForms), typeof(FormatterNumberForm), typeof(FormatterNumberDetectorKind)],
            new LocalizedPhraseForms("default", Singular: "singular", Dual: "dual"),
            FormatterNumberForm.Dual,
            FormatterNumberDetectorKind.Between2And4Paucal));
        Assert.Equal("bytes", InvokePrivate<string>(
            typeof(ProfiledFormatter),
            null,
            "ApplyFallbackTransform",
            [typeof(string), typeof(double), typeof(FormatterDataUnitFallbackTransform)],
            "bytes",
            2d,
            FormatterDataUnitFallbackTransform.None));
        Assert.Equal(FormatterTimeUnitMask.None, InvokePrivate<FormatterTimeUnitMask>(
            typeof(FormatterDateFormRule),
            null,
            "GetTimeUnitMask",
            [typeof(TimeUnit)],
            (TimeUnit)999));
        Assert.Throws<System.Reflection.TargetInvocationException>(() => InvokePrivate<FormatterNumberForm>(
            typeof(ProfiledFormatter),
            null,
            "DetectNumberForm",
            [typeof(int), typeof(FormatterNumberDetectorKind)],
            1,
            (FormatterNumberDetectorKind)999));
        Assert.Throws<System.Reflection.TargetInvocationException>(() => InvokePrivate<string>(
            typeof(ProfiledFormatter),
            null,
            "ApplyFallbackTransform",
            [typeof(string), typeof(double), typeof(FormatterDataUnitFallbackTransform)],
            "bytes",
            2d,
            (FormatterDataUnitFallbackTransform)999));

        var invalidPlaceholder = new ProfiledFormatter(CultureInfo.InvariantCulture, new FormatterProfile(
            FormatterNumberDetectorKind.None,
            [],
            [],
            FormatterNumberDetectorKind.None,
            FormatterNumberForm.Default,
            FormatterDataUnitFallbackTransform.None,
            FormatterPrepositionMode.RomanianDe,
            FormatterSecondaryPlaceholderMode.LuxembourgishEifelerN));
        Assert.Throws<System.Reflection.TargetInvocationException>(() => InvokePrivate<string>(
            typeof(ProfiledFormatter),
            invalidPlaceholder,
            "GetSecondaryPlaceholder",
            [typeof(TimeUnit), typeof(int)],
            TimeUnit.Day,
            2));
    }

    [Fact]
    public void ProfiledFormatterCoversRemainingPhraseResolutionBranches()
    {
        var forms = new LocalizedPhraseForms("default", Singular: "singular", Dual: "dual", Paucal: "paucal", Plural: "plural");

        Assert.Equal("paucal", InvokePrivate<string>(
            typeof(ProfiledFormatter),
            null,
            "ResolveProfiledPhraseForms",
            [typeof(LocalizedPhraseForms), typeof(FormatterNumberForm), typeof(FormatterNumberDetectorKind)],
            forms,
            FormatterNumberForm.Paucal,
            FormatterNumberDetectorKind.Between2And4Paucal));
        Assert.Equal("dual", InvokePrivate<string>(
            typeof(ProfiledFormatter),
            null,
            "ResolveProfiledPhraseForms",
            [typeof(LocalizedPhraseForms), typeof(FormatterNumberForm), typeof(FormatterNumberDetectorKind)],
            forms,
            FormatterNumberForm.Dual,
            FormatterNumberDetectorKind.Slovenian));
        Assert.Equal("singular", InvokePrivate<string>(
            typeof(ProfiledFormatter),
            null,
            "ResolveProfiledPhraseForms",
            [typeof(LocalizedPhraseForms), typeof(FormatterNumberForm), typeof(FormatterNumberDetectorKind)],
            new LocalizedPhraseForms("default", Singular: "singular"),
            FormatterNumberForm.Dual,
            FormatterNumberDetectorKind.Between2And4Paucal));
        Assert.Equal("paucal", InvokePrivate<string>(
            typeof(ProfiledFormatter),
            null,
            "ResolveProfiledPhraseForms",
            [typeof(LocalizedPhraseForms), typeof(FormatterNumberForm), typeof(FormatterNumberDetectorKind)],
            new LocalizedPhraseForms("default", Paucal: "paucal"),
            FormatterNumberForm.Dual,
            FormatterNumberDetectorKind.Between2And4Paucal));
        Assert.Equal("default", InvokePrivate<string>(
            typeof(ProfiledFormatter),
            null,
            "ResolveProfiledPhraseForms",
            [typeof(LocalizedPhraseForms), typeof(FormatterNumberForm), typeof(FormatterNumberDetectorKind)],
            new LocalizedPhraseForms("default"),
            FormatterNumberForm.Dual,
            FormatterNumberDetectorKind.Between2And4Paucal));
        Assert.Equal("paucal", InvokePrivate<string>(
            typeof(ProfiledFormatter),
            null,
            "ResolveProfiledPhraseForms",
            [typeof(LocalizedPhraseForms), typeof(FormatterNumberForm), typeof(FormatterNumberDetectorKind)],
            forms,
            FormatterNumberForm.Paucal,
            FormatterNumberDetectorKind.Slovenian));
        Assert.Equal("paucal", InvokePrivate<string>(
            typeof(ProfiledFormatter),
            null,
            "ResolveProfiledPhraseForms",
            [typeof(LocalizedPhraseForms), typeof(FormatterNumberForm), typeof(FormatterNumberDetectorKind)],
            forms,
            FormatterNumberForm.Paucal,
            FormatterNumberDetectorKind.Russian));
        Assert.Equal("default", InvokePrivate<string>(
            typeof(ProfiledFormatter),
            null,
            "ResolveProfiledPhraseForms",
            [typeof(LocalizedPhraseForms), typeof(FormatterNumberForm), typeof(FormatterNumberDetectorKind)],
            new LocalizedPhraseForms("default"),
            FormatterNumberForm.Paucal,
            FormatterNumberDetectorKind.Russian));

        Assert.Equal(" de", InvokePrivate<string>(
            typeof(ProfiledFormatter),
            CreateProfiledFormatter(FormatterPrepositionMode.RomanianDe, FormatterSecondaryPlaceholderMode.None),
            "GetSecondaryPlaceholder",
            [typeof(TimeUnit), typeof(int)],
            TimeUnit.Day,
            20));
        Assert.Equal(string.Empty, InvokePrivate<string>(
            typeof(ProfiledFormatter),
            CreateProfiledFormatter(FormatterPrepositionMode.RomanianDe, FormatterSecondaryPlaceholderMode.None),
            "GetSecondaryPlaceholder",
            [typeof(TimeUnit), typeof(int)],
            TimeUnit.Day,
            19));
        Assert.Equal("n", InvokePrivate<string>(
            typeof(ProfiledFormatter),
            CreateProfiledFormatter(FormatterPrepositionMode.None, FormatterSecondaryPlaceholderMode.LuxembourgishEifelerN),
            "GetSecondaryPlaceholder",
            [typeof(TimeUnit), typeof(int)],
            TimeUnit.Day,
            3));
        Assert.Equal(string.Empty, InvokePrivate<string>(
            typeof(ProfiledFormatter),
            CreateProfiledFormatter(FormatterPrepositionMode.None, FormatterSecondaryPlaceholderMode.LuxembourgishEifelerN),
            "GetSecondaryPlaceholder",
            [typeof(TimeUnit), typeof(int)],
            TimeUnit.Day,
            4));

        var exactTwoFormatter = CreateProfiledFormatter(
            FormatterPrepositionMode.None,
            FormatterSecondaryPlaceholderMode.None,
            exactDateForms: [new(2, FormatterTimeUnitMask.Day, FormatterTenseMask.Future, FormatterNumberForm.Dual)]);
        var twoTemplatePhrase = new LocalizedDatePhrase(Template: new("two", "{0} dual days"));
        Assert.True(InvokePrivate<bool>(
            typeof(ProfiledFormatter),
            exactTwoFormatter,
            "ShouldUseDatePhraseTemplate",
            [typeof(TimeUnit), typeof(Tense), typeof(int), typeof(LocalizedDatePhrase)],
            TimeUnit.Day,
            Tense.Future,
            2,
            twoTemplatePhrase));
    }

    [Fact]
    public void PublicUtilityOverloadsCoverRemainingGuardBranches()
    {
        Assert.Null(Vocabularies.Default.Pluralize(null));
        Assert.Null(Vocabularies.Default.Singularize(null));

        Assert.Throws<ArgumentException>(() => RomanNumeralExtensions.FromRoman(ReadOnlySpan<char>.Empty));
        Assert.Throws<ArgumentOutOfRangeException>(() => 0.ToRoman());

        Assert.Equal("2 requests", "request".ToQuantity(2L));
        Assert.Equal("1.00 request", "request".ToQuantity(1L, "N2", CultureInfo.InvariantCulture));
        Assert.False(ByteSize.TryParse($"{new string('9', 400)} b", out _));

        Configurator.ResetUseEnumDescriptionPropertyLocator();
        try
        {
            Assert.NotNull(Configurator.EnumDescriptionPropertyLocator);
            var locatorException = Assert.Throws<Exception>(() => Configurator.UseEnumDescriptionPropertyLocator(static _ => true));
            Assert.Contains("UseEnumDescriptionPropertyLocator", locatorException.Message, StringComparison.Ordinal);
        }
        finally
        {
            Configurator.ResetUseEnumDescriptionPropertyLocator();
        }
    }

    [UseCulture("en-US")]
    [Fact]
    public void TimeSpanHumanizeCoversRemainingBoundaryAndFallbackBranches()
    {
        Assert.Equal("0 days", TimeSpan.Zero.Humanize(maxUnit: TimeUnit.Day, minUnit: TimeUnit.Day));
        Assert.Contains("month", TimeSpan.FromDays(60).Humanize(precision: 3, maxUnit: TimeUnit.Month, minUnit: TimeUnit.Day));
        Assert.Contains("week", TimeSpan.FromDays(21).Humanize(precision: 2, maxUnit: TimeUnit.Week, minUnit: TimeUnit.Day));
        Assert.Equal("2147483647 seconds", TimeSpan.FromSeconds(int.MaxValue).Humanize(maxUnit: TimeUnit.Second, minUnit: TimeUnit.Second));
        Assert.Equal("2 days", TimeSpanHumanizeExtensions.FormatAge("2 days", "{0}"));
        Assert.Equal(0, InvokePrivate<int>(
            typeof(TimeSpanHumanizeExtensions),
            null,
            "GetTimeUnitNumericalValue",
            [typeof(TimeUnit), typeof(TimeSpan), typeof(TimeUnit)],
            (TimeUnit)999,
            TimeSpan.FromSeconds(1),
            TimeUnit.Second));
    }

    [UseCulture("en-US")]
    [Fact]
    public void DateTimeHumanizeCoversMonthBoundaryAndApproximateYearBranches()
    {
        var january31 = new DateTime(2024, 1, 31);

        Assert.Equal("one month from now", DateTimeHumanizeAlgorithms.DefaultHumanize(new DateTime(2024, 2, 29), january31, CultureInfo.CurrentCulture));
        Assert.Equal("28 days from now", DateTimeHumanizeAlgorithms.DefaultHumanize(new DateTime(2024, 1, 29), new DateTime(2024, 1, 1), CultureInfo.CurrentCulture));
        Assert.Equal("one year from now", DateTimeHumanizeAlgorithms.DefaultHumanize(new DateTime(2024, 12, 16), new DateTime(2024, 1, 1), CultureInfo.CurrentCulture));
    }

    [Fact]
    public void SegmentedAndHarmonyConvertersCoverRemainingGuardAndSuffixBranches()
    {
        var segmented = new SegmentedScaleNumberToWordsConverter(CreateSegmentedScaleProfile());

        Assert.Equal(string.Empty, segmented.Convert(20_000));
        Assert.Equal("thousand onep", segmented.Convert(1001));
        Assert.Equal("twop thousands", segmented.Convert(2000));
        Assert.Equal(string.Empty, segmented.ConvertToOrdinal(34));
        Assert.Equal(string.Empty, segmented.ConvertToOrdinal(14));

        var harmony = new HarmonyOrdinalNumberToWordsConverter(CreateHarmonyOrdinalProfile());
        Assert.Throws<NotImplementedException>(() => harmony.Convert(2001));

        var invalidStrategy = new HarmonyOrdinalNumberToWordsConverter(CreateHarmonyOrdinalProfile(ordinalSuffixStrategy: (HarmonyOrdinalSuffixStrategy)42));
        Assert.Throws<InvalidOperationException>(() => invalidStrategy.ConvertToOrdinal(1));

        var missingSuffixes = new HarmonyOrdinalNumberToWordsConverter(CreateHarmonyOrdinalProfile(includeOrdinalSuffixes: false));
        Assert.Throws<InvalidOperationException>(() => missingSuffixes.ConvertToOrdinal(1));

        var incompleteMembership = new HarmonyOrdinalNumberToWordsConverter(CreateHarmonyOrdinalProfile(
            ordinalSuffixStrategy: HarmonyOrdinalSuffixStrategy.FinalCharacterMembership,
            secondOrdinalSuffixCharacters: string.Empty,
            ordinalSuffixPair: ["a"]));
        Assert.Throws<InvalidOperationException>(() => incompleteMembership.ConvertToOrdinal(1));
    }

    [UseCulture("en-US")]
    [Fact]
    public void DefaultFallbackConvertersUseCurrentCultureAndOrdinalOverloads()
    {
        var numberConverter = new DefaultNumberToWordsConverter(CultureInfo.InvariantCulture);
        Assert.Equal("12345", numberConverter.Convert(12345));
        Assert.Equal("42", numberConverter.ConvertToOrdinal(42));

        var date = new DateTime(2024, 2, 22);
        var dateConverter = new DefaultDateToOrdinalWordConverter();
        Assert.Equal("22nd February 2024", dateConverter.Convert(date));
        Assert.Equal("22nd February 2024", dateConverter.Convert(date, GrammaticalCase.Genitive));

#if NET6_0_OR_GREATER
        var dateOnly = new DateOnly(2024, 2, 22);
        var dateOnlyConverter = new DefaultDateOnlyToOrdinalWordConverter();
        Assert.Equal("22nd February 2024", dateOnlyConverter.Convert(dateOnly));
        Assert.Equal("22nd February 2024", dateOnlyConverter.Convert(dateOnly, GrammaticalCase.Genitive));
#endif
    }

    [UseCulture("fr-FR")]
    [Fact]
    public void DefaultDateFallbackConvertersUseNonEnglishShortDate()
    {
        var date = new DateTime(2024, 2, 22);
        var dateConverter = new DefaultDateToOrdinalWordConverter();
        Assert.Equal(date.ToString("d", CultureInfo.CurrentCulture), dateConverter.Convert(date));

#if NET6_0_OR_GREATER
        var dateOnly = new DateOnly(2024, 2, 22);
        var dateOnlyConverter = new DefaultDateOnlyToOrdinalWordConverter();
        Assert.Equal(dateOnly.ToString("d", CultureInfo.CurrentCulture), dateOnlyConverter.Convert(dateOnly));
#endif
    }

    [UseCulture("en-US")]
    [Fact]
    public void DateToOrdinalWordsCaseOverloadsDelegateToConfiguredConverters()
    {
        var date = new DateTime(2024, 2, 22);
        Assert.Equal("February 22nd, 2024", date.ToOrdinalWords(GrammaticalCase.Genitive));

#if NET6_0_OR_GREATER
        var dateOnly = new DateOnly(2024, 2, 22);
        Assert.Equal("February 22nd, 2024", dateOnly.ToOrdinalWords(GrammaticalCase.Genitive));
#endif
    }

    [Fact]
    public void LocalePhraseTableReportsFoundAndMissingPhrases()
    {
        var table = CreatePhraseTable();

        Assert.True(table.TryGetDatePhrase(TimeUnit.Day, Tense.Past, out var datePhrase));
        Assert.Equal("yesterday", datePhrase.Single);
        Assert.False(table.TryGetDatePhrase(TimeUnit.Hour, Tense.Past, out datePhrase));
        Assert.Equal(default, datePhrase);

        Assert.True(table.TryGetTimeSpanPhrase(TimeUnit.Minute, out var timeSpanPhrase));
        Assert.Equal("minute", timeSpanPhrase.Single);
        Assert.False(table.TryGetTimeSpanPhrase(TimeUnit.Hour, out timeSpanPhrase));
        Assert.Equal(default, timeSpanPhrase);

        Assert.True(table.TryGetDataUnitPhrase(DataUnit.Byte, out var dataUnitPhrase));
        Assert.Equal("B", dataUnitPhrase.Symbol);
        Assert.False(table.TryGetDataUnitPhrase(DataUnit.Kilobyte, out dataUnitPhrase));
        Assert.Equal(default, dataUnitPhrase);

        Assert.True(table.TryGetTimeUnitPhrase(TimeUnit.Second, out var timeUnitPhrase));
        Assert.Equal("s", timeUnitPhrase.Symbol);
        Assert.False(table.TryGetTimeUnitPhrase(TimeUnit.Millisecond, out timeUnitPhrase));
        Assert.Equal(default, timeUnitPhrase);

        Assert.Equal("now", table.DateHumanizeNow);
        Assert.Equal("never", table.DateHumanizeNever);
        Assert.Equal("zero", table.TimeSpanZero);
        Assert.Equal("age", table.TimeSpanAge);
        Assert.Equal("yesterday", table.GetDateHumanize(TimeUnit.Day, Tense.Past)?.Single);
        Assert.Equal("minute", table.GetTimeSpan(TimeUnit.Minute)?.Single);
        Assert.Equal("B", table.GetDataUnit(DataUnit.Byte)?.Symbol);
        Assert.Equal("s", table.GetTimeUnit(TimeUnit.Second)?.Symbol);
        Assert.Null(table.GetDateHumanize(TimeUnit.Hour, Tense.Past));
        Assert.Null(table.GetTimeSpan(TimeUnit.Hour));
        Assert.Null(table.GetDataUnit(DataUnit.Kilobyte));
        Assert.Equal("s", table.GetTimeUnitSymbol(TimeUnit.Second));
        Assert.Null(table.GetTimeUnitSymbol(TimeUnit.Millisecond));
    }

    [Fact]
    public void LocalePhraseTableCatalogFallsBackToEnglishWhenNoCultureMatchExists()
    {
        var table = LocalePhraseTableCatalog.Resolve(new CultureInfo("eo"));

        Assert.NotNull(table);
        Assert.Equal("now", table.DateNow);
    }

    [Fact]
    public void DelimitedCollectionFormatterCoversAllOverloadsAndNullGuards()
    {
        var formatter = new DelimitedCollectionFormatter("; ");
        var values = new string?[] { null, " alpha ", " ", "beta" };

        Assert.Equal("alpha; beta", formatter.Humanize(values));
        Assert.Equal("ALPHA; BETA", formatter.Humanize(values, static value => value?.Trim().ToUpperInvariant()));
        Assert.Equal("5; 6", formatter.Humanize([5, 6], static value => (object?)value));
        Assert.Equal("alpha | beta", formatter.Humanize(values, " | "));
        Assert.Equal("ALPHA | BETA", formatter.Humanize(values, static value => value?.Trim().ToUpperInvariant(), " | "));
        Assert.Equal("5 | 6", formatter.Humanize([5, 6, -1], static value => value < 0 ? null : (object?)value, " | "));
        Assert.Equal(string.Empty, formatter.Humanize(Array.Empty<string>()));
        Assert.Equal("solo", formatter.Humanize(["solo"]));

        var collectionException = Assert.Throws<ArgumentNullException>(() => formatter.Humanize<string>(null!));
        Assert.Equal("collection", collectionException.ParamName);

        var formatterException = Assert.Throws<ArgumentNullException>(() => formatter.Humanize(["value"], (Func<string, string?>)null!));
        Assert.Equal("objectFormatter", formatterException.ParamName);

        var defaultObjectFormatterException = Assert.Throws<ArgumentNullException>(() => formatter.Humanize(["value"], (Func<string, object?>)null!));
        Assert.Equal("objectFormatter", defaultObjectFormatterException.ParamName);

        var objectFormatterException = Assert.Throws<ArgumentNullException>(() => formatter.Humanize(["value"], (Func<string, object?>)null!, " | "));
        Assert.Equal("objectFormatter", objectFormatterException.ParamName);
    }

    [Fact]
    public void CliticCollectionFormatterCoversSwitchArmsAndOverloads()
    {
        var formatter = new CliticCollectionFormatter("and ");
        var values = new string?[] { null, " alpha ", "", "beta", "gamma" };

        Assert.Equal(string.Empty, formatter.Humanize(Array.Empty<string>()));
        Assert.Equal("alpha", formatter.Humanize([" alpha "]));
        Assert.Equal("alpha and beta", formatter.Humanize(["alpha", "beta"]));
        Assert.Equal("alpha, beta and gamma", formatter.Humanize(values));
        Assert.Equal("ALPHA, BETA and GAMMA", formatter.Humanize(values, static value => value?.Trim().ToUpperInvariant()));
        Assert.Equal("5 and 6", formatter.Humanize([5, 6], static value => (object?)value));
        Assert.Equal("alpha or beta", formatter.Humanize(["alpha", "beta"], "or"));
        Assert.Equal("ALPHA or BETA", formatter.Humanize(["alpha", "beta"], static value => value.ToUpperInvariant(), "or "));
        Assert.Equal("5 or 6", formatter.Humanize([5, 6, -1], static value => value < 0 ? null : (object?)value, "or "));

        var collectionException = Assert.Throws<ArgumentNullException>(() => formatter.Humanize<string>(null!, static value => value));
        Assert.Equal("collection", collectionException.ParamName);

        var formatterException = Assert.Throws<ArgumentNullException>(() => formatter.Humanize(["value"], (Func<string, string?>)null!, "and "));
        Assert.Equal("objectFormatter", formatterException.ParamName);

        var defaultObjectFormatterException = Assert.Throws<ArgumentNullException>(() => formatter.Humanize(["value"], (Func<string, object?>)null!));
        Assert.Equal("objectFormatter", defaultObjectFormatterException.ParamName);

        var objectFormatterException = Assert.Throws<ArgumentNullException>(() => formatter.Humanize(["value"], (Func<string, object?>)null!, "and "));
        Assert.Equal("objectFormatter", objectFormatterException.ParamName);
    }

    [UseCulture("en")]
    [Fact]
    public void OrdinalizeOverloadsCoverNullCultureAndFormatCharacterBranches()
    {
        Assert.Equal("21st", "21".Ordinalize((CultureInfo)null!));
        Assert.Equal("21st", "21".Ordinalize((CultureInfo)null!, WordForm.Normal));
        Assert.Equal("21st", "21".Ordinalize(GrammaticalGender.Masculine, (CultureInfo)null!));
        Assert.Equal("21st", "21".Ordinalize(GrammaticalGender.Masculine, (CultureInfo)null!, WordForm.Normal));

        Assert.Equal("21st", 21.Ordinalize((CultureInfo)null!));
        Assert.Equal("21st", 21.Ordinalize((CultureInfo)null!, WordForm.Normal));
        Assert.Equal("21st", 21.Ordinalize(GrammaticalGender.Masculine, (CultureInfo)null!));
        Assert.Equal("21st", 21.Ordinalize(GrammaticalGender.Masculine, (CultureInfo)null!, WordForm.Normal));

        Assert.Equal("21", InvokePrivate<string>(
            typeof(OrdinalizeExtensions),
            null,
            "NormalizeOrdinalNumberString",
            [typeof(string)],
            "2\u200e1"));
    }

    static LocalePhraseTable CreatePhraseTable()
    {
        var datePast = NewPhraseArray<LocalizedDatePhrase, TimeUnit>();
        var dateFuture = NewPhraseArray<LocalizedDatePhrase, TimeUnit>();
        var timeSpanUnits = NewPhraseArray<LocalizedTimeSpanPhrase, TimeUnit>();
        var dataUnits = NewPhraseArray<LocalizedUnitPhrase, DataUnit>();
        var timeUnits = NewPhraseArray<LocalizedUnitPhrase, TimeUnit>();

        datePast[(int)TimeUnit.Day] = new(Single: "yesterday");
        dateFuture[(int)TimeUnit.Day] = new(Single: "tomorrow");
        timeSpanUnits[(int)TimeUnit.Minute] = new(Single: "minute");
        dataUnits[(int)DataUnit.Byte] = new(Symbol: "B");
        timeUnits[(int)TimeUnit.Second] = new(Symbol: "s");

        return new("now", "never", "zero", "age", datePast, dateFuture, timeSpanUnits, dataUnits, timeUnits);
    }

    static T?[] NewPhraseArray<T, TEnum>()
        where T : struct
        where TEnum : struct, Enum =>
        new T?[EnumValues<TEnum>().Max(static value => Convert.ToInt32(value, CultureInfo.InvariantCulture)) + 1];

    static TEnum[] EnumValues<TEnum>()
        where TEnum : struct, Enum =>
#if NET5_0_OR_GREATER
        Enum.GetValues<TEnum>();
#else
        [.. Enum.GetValues(typeof(TEnum)).Cast<TEnum>()];
#endif

    static string[] MonthNames(string prefix) =>
    [
        $"{prefix}1",
        $"{prefix}2",
        $"{prefix}3",
        $"{prefix}4",
        $"{prefix}5",
        $"{prefix}6",
        $"{prefix}7",
        $"{prefix}8",
        $"{prefix}9",
        $"{prefix}10",
        $"{prefix}11",
        $"{prefix}12"
    ];

    static TokenMapWordsToNumberRules CreateTokenMapRulesWithoutOrdinalScales() =>
        new()
        {
            CardinalMap = new Dictionary<string, long>(StringComparer.Ordinal)
            {
                ["one"] = 1
            }.ToFrozenDictionary(StringComparer.Ordinal),
            ExactOrdinalMap = new Dictionary<string, long>(StringComparer.Ordinal)
            {
                ["first"] = 1
            }.ToFrozenDictionary(StringComparer.Ordinal),
            NormalizationProfile = TokenMapNormalizationProfile.LowercaseRemovePeriods,
            AllowTerminalOrdinalToken = true
        };

    static TokenMapWordsToNumberRules CreateTokenMapRulesWithExactOrdinalOverflow() =>
        new()
        {
            CardinalMap = new Dictionary<string, long>(StringComparer.Ordinal)
            {
                ["one"] = 1
            }.ToFrozenDictionary(StringComparer.Ordinal),
            ExactOrdinalMap = new Dictionary<string, long>(StringComparer.Ordinal)
            {
                ["max"] = long.MaxValue
            }.ToFrozenDictionary(StringComparer.Ordinal),
            NormalizationProfile = TokenMapNormalizationProfile.LowercaseRemovePeriods,
            AllowTerminalOrdinalToken = true
        };

    static TokenMapWordsToNumberRules CreateTokenMapRulesWithOrdinalScaleOverflow() =>
        new()
        {
            CardinalMap = new Dictionary<string, long>(StringComparer.Ordinal)
            {
                ["two"] = 2
            }.ToFrozenDictionary(StringComparer.Ordinal),
            OrdinalScaleMap = new Dictionary<string, long>(StringComparer.Ordinal)
            {
                ["hugeord"] = long.MaxValue
            }.ToFrozenDictionary(StringComparer.Ordinal),
            NormalizationProfile = TokenMapNormalizationProfile.LowercaseRemovePeriods,
            AllowTerminalOrdinalToken = true
        };

    static TokenMapWordsToNumberRules CreateTokenMapRulesWithGluedOrdinalOverflow() =>
        new()
        {
            CardinalMap = new Dictionary<string, long>(StringComparer.Ordinal)
            {
                ["two"] = 2
            }.ToFrozenDictionary(StringComparer.Ordinal),
            GluedOrdinalScaleSuffixes = new Dictionary<string, long>(StringComparer.Ordinal)
            {
                ["illionth"] = long.MaxValue
            }.ToFrozenDictionary(StringComparer.Ordinal),
            NormalizationProfile = TokenMapNormalizationProfile.LowercaseRemovePeriods
        };

    static ScaleStrategyNumberToWordsProfile CreateScaleStrategyProfile(
        ScaleStrategyCardinalMode cardinalMode,
        ScaleStrategyOrdinalMode ordinalMode,
        FrozenDictionary<int, string>? ordinalExceptions = null) =>
        new(
            cardinalMode,
            ordinalMode,
            long.MaxValue,
            GrammaticalGender.Masculine,
            "zero",
            "minus",
            "one",
            "one-m",
            "one-f",
            "one-n",
            "and",
            " plus ",
            "th-large",
            "th-default",
            "y",
            "ieth",
            5,
            "e",
            "th-trimmed",
            "th",
            "hundred",
            "onehundred",
            "thousand",
            "one thousand",
            "onethousand",
            CreateScaleStrategyUnits(),
            CreateScaleStrategyTens(),
            [],
            CreateScaleStrategyScales(),
            ordinalExceptions ?? new Dictionary<int, string>
            {
                [0] = "zeroth",
                [1] = "first",
                [2] = "second",
                [20] = "twentieth"
            }.ToFrozenDictionary());

    static string[] CreateScaleStrategyUnits()
    {
        var units = Enumerable.Repeat(string.Empty, 20).ToArray();
        units[1] = "one";
        units[2] = "two";
        units[3] = "three";
        units[4] = "four";
        units[5] = "five";
        units[6] = "six";
        return units;
    }

    static string[] CreateScaleStrategyTens()
    {
        var tens = Enumerable.Repeat(string.Empty, 10).ToArray();
        tens[2] = "twenty";
        return tens;
    }

    static ScaleStrategyScale[] CreateScaleStrategyScales() =>
    [
        new(1_000_000, "million", "millions", " ", " ", "s", "th-large", false, GrammaticalGender.Masculine),
        new(1_000, "thousand", "thousands", " ", " ", "s", "th-scale", true, GrammaticalGender.Masculine),
        new(100, "hundred", "hundreds", string.Empty, string.Empty, "s", "th-hundred", false, GrammaticalGender.Masculine)
    ];

    static TerminalOrdinalScaleNumberToWordsProfile CreateTerminalOrdinalScaleProfile()
    {
        var units = Enumerable.Repeat(string.Empty, 20).ToArray();
        units[1] = "one";
        units[2] = "two";
        var ordinalUnits = Enumerable.Repeat(string.Empty, 20).ToArray();
        ordinalUnits[1] = "first";
        ordinalUnits[2] = "second";
        var tens = Enumerable.Repeat(string.Empty, 10).ToArray();
        tens[2] = "twenty";
        var hundreds = Enumerable.Repeat(string.Empty, 10).ToArray();
        hundreds[1] = "hundredth";

        return new(
            "zero",
            "minus",
            units,
            ordinalUnits,
            tens,
            hundreds,
            "one-hundred",
            "one-hundred-after",
            "one-hundred-with",
            "hundreds",
            "-m",
            "-f",
            [new(1000, "one-thousand", "one-thousand-with", "thousands", "thousandth")]);
    }

    static ConjunctionalScaleNumberToWordsProfile CreateConjunctionalScaleProfile(
        ConjunctionalScaleAndStrategy andStrategy = ConjunctionalScaleAndStrategy.WithinGroupAndAfterScaleSubHundredRemainder)
    {
        var units = Enumerable.Repeat(string.Empty, 20).ToArray();
        units[0] = "zero";
        units[1] = "one";
        units[2] = "two";
        units[3] = "three";
        var ordinalUnits = Enumerable.Repeat(string.Empty, 20).ToArray();
        ordinalUnits[0] = "zeroth";
        ordinalUnits[1] = "first";
        ordinalUnits[2] = "second";
        var tens = Enumerable.Repeat(string.Empty, 10).ToArray();
        tens[2] = "one twenty";
        var ordinalTens = Enumerable.Repeat(string.Empty, 10).ToArray();
        ordinalTens[2] = "twentieth";

        return new(
            "minus",
            "and",
            "hundred",
            "hundredth",
            "-",
            true,
            ConjunctionalScaleAddAndMode.UseCallerFlag,
            andStrategy,
            "-tuple",
            ConjunctionalScaleOrdinalLeadingOneStrategy.OmitLeadingOne,
            ConjunctionalScaleOrdinalMode.English,
            units,
            ordinalUnits,
            tens,
            ordinalTens,
            [new(1000, "thousand", "thousandth")],
            new Dictionary<int, string> { [2] = "pair" }.ToFrozenDictionary());
    }

    static SegmentedScaleNumberToWordsProfile CreateSegmentedScaleProfile()
    {
        var units = Enumerable.Repeat(string.Empty, 13).ToArray();
        var pluralUnits = Enumerable.Repeat(string.Empty, 13).ToArray();
        for (var i = 0; i < units.Length; i++)
        {
            units[i] = i.ToString(CultureInfo.InvariantCulture);
            pluralUnits[i] = i + "p";
        }

        units[0] = "zero";
        units[1] = "one";
        units[2] = "two";
        pluralUnits[1] = "onep";
        pluralUnits[2] = "twop";
        var tens = Enumerable.Repeat(string.Empty, 10).ToArray();
        tens[1] = "teen";
        tens[2] = "twenty";
        var hundreds = Enumerable.Repeat(string.Empty, 10).ToArray();
        hundreds[1] = "hundred";
        var pluralHundreds = Enumerable.Repeat(string.Empty, 10).ToArray();
        pluralHundreds[1] = "hundredp";

        return new(
            10_000,
            "zero",
            "minus",
            "teen",
            "one hundred",
            units,
            pluralUnits,
            tens,
            hundreds,
            pluralHundreds,
            [new(1000, "thousand", "thousands", SegmentedScaleVariant.Pluralized, SegmentedScaleVariant.Pluralized, SegmentedScaleVariant.Default)],
            100,
            new Dictionary<int, string>
            {
                [10] = "tenth",
                [100] = "hundredth"
            }.ToFrozenDictionary());
    }

    static HarmonyOrdinalNumberToWordsProfile CreateHarmonyOrdinalProfile(
        HarmonyOrdinalSuffixStrategy ordinalSuffixStrategy = HarmonyOrdinalSuffixStrategy.LastVowelMap,
        FrozenDictionary<char, string>? ordinalSuffixes = null,
        bool includeOrdinalSuffixes = true,
        string? secondOrdinalSuffixCharacters = "e",
        string[]? ordinalSuffixPair = null)
    {
        var units = Enumerable.Repeat(string.Empty, 10).ToArray();
        units[0] = "zero";
        units[1] = "one";
        units[2] = "two";
        var tens = Enumerable.Repeat(string.Empty, 10).ToArray();
        tens[2] = "twenty";

        return new(
            -1000,
            2000,
            "minus",
            "hundred",
            HarmonyOrdinalHundredStrategy.AllowExplicitOneInComposite,
            units,
            tens,
            [new(1000, "thousand", OmitOneWhenSingular: true)],
            ordinalSuffixStrategy,
            softenTerminalTBeforeSuffix: true,
            dropTerminalVowelBeforeHarmonySuffix: true,
            includeOrdinalSuffixes ? ordinalSuffixes ?? new Dictionary<char, string> { ['e'] = "th" }.ToFrozenDictionary() : null,
            secondOrdinalSuffixCharacters,
            ordinalSuffixPair ?? ["a", "b"],
            new Dictionary<char, string> { ['e'] = "tuple" }.ToFrozenDictionary());
    }

    static readonly SuffixScaleWordsToNumberProfile SuffixScaleProfile = new(
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["zero"] = 0,
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3,
            ["four"] = 4,
            ["five"] = 5,
            ["six"] = 6,
            ["seven"] = 7,
            ["eight"] = 8,
            ["nine"] = 9
        }.ToFrozenDictionary(StringComparer.Ordinal),
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["thousand"] = 1_000,
            ["million"] = 1_000_000
        }.ToFrozenDictionary(StringComparer.Ordinal),
        [
            new("million", "millions", 1_000_000),
            new("thousand", "thousands", 1_000)
        ],
        "hundred",
        "hundreds",
        "ty",
        "teen",
        ["minus ", "negative "]);

    static readonly TokenMapWordsToNumberRules TokenMapRules = new()
    {
        CardinalMap = new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["zero"] = 0,
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3,
            ["four"] = 4,
            ["five"] = 5,
            ["nine"] = 9,
            ["ten"] = 10,
            ["hundred"] = 100,
            ["thousand"] = 1_000,
            ["million"] = 1_000_000,
            ["billion"] = 1_000_000_000,
            ["special phrase"] = 77,
            ["huge"] = long.MaxValue
        }.ToFrozenDictionary(StringComparer.Ordinal),
        ExactOrdinalMap = new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["first"] = 1,
            ["third"] = 3
        }.ToFrozenDictionary(StringComparer.Ordinal),
        OrdinalScaleMap = new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["thousandth"] = 1_000
        }.ToFrozenDictionary(StringComparer.Ordinal),
        GluedOrdinalScaleSuffixes = new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["millionth"] = 1_000_000
        }.ToFrozenDictionary(StringComparer.Ordinal),
        CompositeScaleMap = new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["million billion"] = 1_000_000_000_000_000
        }.ToFrozenDictionary(StringComparer.Ordinal),
        NormalizationProfile = TokenMapNormalizationProfile.LowercaseRemovePeriods,
        NegativePrefixes = ["minus "],
        NegativeSuffixes = [" negative"],
        OrdinalPrefixes = ["ordinal "],
        IgnoredTokens = ["and"],
        LeadingTokenPrefixesToTrim = ["ka"],
        MultiplierTokens = ["ten"],
        TokenSuffixesToStrip = ["x"],
        OrdinalAbbreviationSuffixes = ["st", "nd", "rd", "th"],
        TeenSuffixTokens = ["teen"],
        HundredSuffixTokens = ["hundred"],
        AllowTerminalOrdinalToken = true,
        UseHundredMultiplier = true,
        AllowInvariantIntegerInput = true,
        UnitTokenMinValue = 1,
        UnitTokenMaxValue = 9,
        HundredSuffixMinValue = 1,
        HundredSuffixMaxValue = 9,
        ScaleThreshold = 1000
    };

    static readonly InvertedTensWordsToNumberProfile InvertedTensProfile = new(
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3,
            ["hundred"] = 100,
            ["thousand"] = 1_000,
            ["million"] = 1_000_000
        }.ToFrozenDictionary(StringComparer.Ordinal),
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3
        }.ToFrozenDictionary(StringComparer.Ordinal),
        [new("zig", 20), new("tig", 30)],
        "en",
        ["thousand", "million"],
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["first"] = 1
        }.ToFrozenDictionary(StringComparer.Ordinal),
        ["minus "],
        ["and", "of"],
        ["th"],
        [new("foo", "two")],
        allowInvariantIntegerInput: true);

    static readonly CompoundScaleWordsToNumberProfile CompoundScaleProfile = new(
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["one"] = 1,
            ["two"] = 2,
            ["five"] = 5,
            ["twenty"] = 20,
            ["hundred"] = 100,
            ["thousand"] = 1_000
        }.ToFrozenDictionary(StringComparer.Ordinal),
        ["twenty"],
        ["thousand"],
        "and",
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["first"] = 1
        }.ToFrozenDictionary(StringComparer.Ordinal),
        ["minus "],
        sequenceMultiplierThreshold: 100);

    static readonly VigesimalCompoundWordsToNumberProfile VigesimalProfile = new(
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3,
            ["five"] = 5,
            ["score"] = 20,
            ["twenty"] = 20,
            ["hundred"] = 100,
            ["thousand"] = 1_000
        }.ToFrozenDictionary(StringComparer.Ordinal),
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["first"] = 1
        }.ToFrozenDictionary(StringComparer.Ordinal),
        ["minus "],
        ["and"],
        "score",
        ["one", "two"],
        20,
        "teen",
        new[] { 20L }.ToFrozenSet());

    static readonly GreedyCompoundWordsToNumberProfile GreedyProfile = new(
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["one"] = 1,
            ["two"] = 2,
            ["twenty"] = 20,
            ["hundred"] = 100,
            ["thousand"] = 1_000,
            ["and"] = 0
        }.ToFrozenDictionary(StringComparer.Ordinal),
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["first"] = 1
        }.ToFrozenDictionary(StringComparer.Ordinal),
        ["minus "],
        ["and"],
        ["st", "nd", "rd", "th"],
        ",",
        "_-",
        [new("uno", "one")],
        lowercase: true,
        removeDiacritics: true);

    static readonly PrefixedTensScaleWordsToNumberProfile PrefixedTensProfile = new(
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3,
            ["four"] = 4,
            ["five"] = 5
        }.ToFrozenDictionary(StringComparer.Ordinal),
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["twenty"] = 20,
            ["thirty"] = 30
        }.ToFrozenDictionary(StringComparer.Ordinal),
        [new("thousand", 1_000)],
        [new("twen", 20)],
        ["minus"]);

    static readonly LinkingAffixWordsToNumberProfile LinkingAffixProfile = new(
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3,
            ["five"] = 5,
            ["hundred"] = 100,
            ["thousand"] = 1_000
        }.ToFrozenDictionary(StringComparer.Ordinal),
        "teen",
        10,
        ["ka"],
        ["and"],
        ["minus "]);

    static readonly ContractedScaleWordsToNumberProfile ContractedScaleProfile = new(
        "minus",
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["satu"] = 1,
            ["dua"] = 2,
            ["lima"] = 5,
            ["belas"] = 10,
            ["puluh"] = 10,
            ["ratus"] = 100,
            ["ribu"] = 1_000
        }.ToFrozenDictionary(StringComparer.Ordinal));

    static readonly EastAsianPositionalWordsToNumberProfile EastAsianSingleCharacterProfile = new(
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["一"] = 1,
            ["二"] = 2,
            ["三"] = 3,
            ["五"] = 5
        }.ToFrozenDictionary(StringComparer.Ordinal),
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["十"] = 10,
            ["百"] = 100
        }.ToFrozenDictionary(StringComparer.Ordinal),
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["万"] = 10_000
        }.ToFrozenDictionary(StringComparer.Ordinal),
        ["負"],
        "第",
        "目",
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["初"] = 1
        }.ToFrozenDictionary(StringComparer.Ordinal));

    static readonly EastAsianPositionalWordsToNumberProfile EastAsianMultiCharacterProfile = new(
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["one"] = 1,
            ["two"] = 2
        }.ToFrozenDictionary(StringComparer.Ordinal),
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["ten"] = 10,
            ["hundred"] = 100
        }.ToFrozenDictionary(StringComparer.Ordinal),
        new Dictionary<string, long>(StringComparer.Ordinal)
        {
            ["thousand"] = 1_000
        }.ToFrozenDictionary(StringComparer.Ordinal),
        [],
        string.Empty,
        string.Empty);

    static WordFormTemplateOrdinalizer.Options CreateWordFormTemplateOrdinalizerOptions(
        WordFormTemplateOrdinalizer.NegativeNumberMode negativeMode = WordFormTemplateOrdinalizer.NegativeNumberMode.None,
        bool minValueAsPlainNumber = false)
    {
        var masculine = new WordFormTemplateOrdinalizer.PatternSet(
            CreateOrdinalizerPattern(
                "m-",
                "-m",
                exactReplacements: new() { [2] = "two-m" },
                exactSuffixes: new() { [3] = "-exact" },
                lastDigitSuffixes: new() { [4] = "-last-m" }),
            CreateOrdinalizerPattern("am-", "-am"));
        var feminine = new WordFormTemplateOrdinalizer.PatternSet(
            CreateOrdinalizerPattern("f-", "-f", lastDigitSuffixes: new() { [4] = "-last-f" }),
            CreateOrdinalizerPattern("af-", "-af"));
        var neuter = new WordFormTemplateOrdinalizer.PatternSet(
            CreateOrdinalizerPattern("n-", "-n"),
            CreateOrdinalizerPattern("an-", "-an"));

        return new(masculine, feminine, neuter, MinValueAsPlainNumber: minValueAsPlainNumber, NegativeMode: negativeMode);
    }

    static WordFormTemplateOrdinalizer.Pattern CreateOrdinalizerPattern(
        string prefix,
        string defaultSuffix,
        Dictionary<int, string>? exactReplacements = null,
        Dictionary<int, string>? exactSuffixes = null,
        Dictionary<int, string>? lastDigitSuffixes = null) =>
        new(
            prefix,
            defaultSuffix,
            (exactReplacements ?? []).ToFrozenDictionary(),
            (exactSuffixes ?? []).ToFrozenDictionary(),
            (lastDigitSuffixes ?? []).ToFrozenDictionary());

    static PluralizedScaleNumberToWordsProfile CreatePluralizedScaleProfile(
        PluralizedScaleFormDetector formDetector,
        PluralizedScaleUnitVariantStrategy unitVariantStrategy,
        PluralizedScaleOrdinalMode ordinalMode = PluralizedScaleOrdinalMode.Lithuanian) =>
        new(
            "zero",
            "minus",
            "zeroth",
            CreatePluralizedUnits(),
            CreatePluralizedTens(),
            CreatePluralizedHundreds(),
            [new(1000, GrammaticalGender.Masculine, "thousand-one", "thousand-few", "thousand-many", "thousandth", OmitLeadingOne: false)],
            formDetector,
            unitVariantStrategy,
            ordinalMode,
            supportsNeuter: true,
            masculineOrdinalSuffix: "masc",
            feminineOrdinalSuffix: "fem",
            CreatePluralizedOrdinalUnits(),
            CreatePluralizedOrdinalTens(),
            CreatePluralizedOrdinalHundreds());

    static string[] CreatePluralizedUnits()
    {
        var units = Enumerable.Repeat(string.Empty, 21).ToArray();
        units[1] = "vienas";
        units[2] = "du";
        units[3] = "three";
        units[4] = "four";
        units[5] = "five";
        units[6] = "six";
        units[7] = "septyni";
        units[20] = "twenty";
        return units;
    }

    static string[] CreatePluralizedTens()
    {
        var tens = Enumerable.Repeat(string.Empty, 10).ToArray();
        tens[2] = "twenty";
        return tens;
    }

    static string[] CreatePluralizedHundreds()
    {
        var hundreds = Enumerable.Repeat(string.Empty, 10).ToArray();
        hundreds[1] = "hundred";
        return hundreds;
    }

    static string[] CreatePluralizedOrdinalUnits()
    {
        var units = Enumerable.Repeat(string.Empty, 20).ToArray();
        units[1] = "first";
        units[2] = "second";
        return units;
    }

    static string[] CreatePluralizedOrdinalTens()
    {
        var tens = Enumerable.Repeat(string.Empty, 10).ToArray();
        tens[2] = "twentieth";
        return tens;
    }

    static string[] CreatePluralizedOrdinalHundreds()
    {
        var hundreds = Enumerable.Repeat(string.Empty, 10).ToArray();
        hundreds[1] = "hundredth";
        return hundreds;
    }

    static LocalePhraseTable CreateLocalePhraseTable(
        LocalizedDatePhrase? dateFuture = null,
        LocalizedTimeSpanPhrase? timeSpan = null,
        LocalizedUnitPhrase? dataUnit = null,
        LocalizedUnitPhrase? timeUnit = null)
    {
        var timeUnitCount = EnumValues<TimeUnit>().Length;

        var datePast = new LocalizedDatePhrase?[timeUnitCount];
        var dateFuturePhrases = new LocalizedDatePhrase?[timeUnitCount];
        var timeSpanUnits = new LocalizedTimeSpanPhrase?[timeUnitCount];
        var dataUnits = new LocalizedUnitPhrase?[EnumValues<DataUnit>().Length];
        var timeUnits = new LocalizedUnitPhrase?[timeUnitCount];

        dateFuturePhrases[(int)TimeUnit.Day] = dateFuture;
        timeSpanUnits[(int)TimeUnit.Hour] = timeSpan;
        dataUnits[(int)DataUnit.Byte] = dataUnit;
        timeUnits[(int)TimeUnit.Hour] = timeUnit;

        return new(null, null, null, null, datePast, dateFuturePhrases, timeSpanUnits, dataUnits, timeUnits);
    }

    static ProfiledFormatter CreateProfiledFormatter(
        FormatterPrepositionMode prepositionMode,
        FormatterSecondaryPlaceholderMode secondaryPlaceholderMode,
        FormatterDateFormRule[]? exactDateForms = null,
        FormatterTimeSpanFormRule[]? exactTimeSpanForms = null) =>
        new(CultureInfo.InvariantCulture, new FormatterProfile(
            FormatterNumberDetectorKind.None,
            exactDateForms ?? [],
            exactTimeSpanForms ?? [],
            FormatterNumberDetectorKind.None,
            FormatterNumberForm.Default,
            FormatterDataUnitFallbackTransform.None,
            prepositionMode,
            secondaryPlaceholderMode));

    sealed class FormatterHarness : DefaultFormatter
    {
        public FormatterHarness(LocalePhraseTable table)
            : base(CultureInfo.InvariantCulture)
            => SetPhraseTable(this, table);

        public bool UseDatePhraseTable { get; init; } = true;

        public bool UseTimeSpanPhraseTable { get; init; } = true;

        public bool HasPhraseTable => PhraseTable is not null;

        public bool CallShouldUseDatePhraseTemplate(TimeUnit unit, Tense tense, int count, LocalizedDatePhrase phrase) =>
            ShouldUseDatePhraseTemplate(unit, tense, count, phrase);

        internal override bool ShouldUseDatePhraseTable(TimeUnit unit, Tense tense, int count, LocalizedDatePhrase phrase) =>
            UseDatePhraseTable;

        internal override bool ShouldUseTimeSpanPhraseTable(TimeUnit unit, int count, bool toWords, LocalizedTimeSpanPhrase phrase) =>
            UseTimeSpanPhraseTable;

        protected override string NumberToWords(TimeUnit unit, int number, CultureInfo culture) =>
            $"words-{number}";

        static void SetPhraseTable(DefaultFormatter formatter, LocalePhraseTable table) =>
            typeof(DefaultFormatter)
                .GetField("phraseTable", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                .SetValue(formatter, table);
    }

#if NET6_0_OR_GREATER
    static PhraseClockNotationProfile CreateClockProfile(
        PhraseClockHourMode hourMode = PhraseClockHourMode.Numeric,
        GrammaticalGender hourGender = GrammaticalGender.Masculine,
        GrammaticalGender minuteGender = GrammaticalGender.Masculine,
        string midnight = "",
        string midday = "",
        string min0 = "",
        string min5 = "",
        string min10 = "",
        string min15 = "",
        string min20 = "",
        string min25 = "",
        string min30 = "",
        string min35 = "",
        string min40 = "",
        string min45 = "",
        string min50 = "",
        string min55 = "",
        string defaultTemplate = "",
        string zeroFiller = "",
        string earlyMorning = "",
        string morning = "",
        string afternoon = "",
        string night = "",
        PhraseClockDayPeriodPosition dayPeriodPosition = PhraseClockDayPeriodPosition.Suffix,
        string hourZeroWord = "",
        string hourOneWord = "",
        string hourTwelveWord = "",
        string hourSuffixSingular = "",
        string hourSuffixPlural = "",
        string singularArticle = "",
        string pluralArticle = "",
        bool applyEifelerRule = false,
        string pastHourTemplate = "",
        string beforeHalfTemplate = "",
        string afterHalfTemplate = "",
        string beforeNextTemplate = "",
        string minuteSuffixSingular = "",
        string minuteSuffixPlural = "",
        string hourSuffixPaucal = "",
        string minuteSuffixPaucal = "",
        string[]? hourWordsMap = null,
        string[]? minuteWordsMap = null,
        bool compactMinuteWords = false,
        bool paucalLowOnly = false,
        string compactConjunction = "") =>
        new(
            hourMode,
            hourGender,
            minuteGender,
            midnight,
            midday,
            min0,
            min5,
            min10,
            min15,
            min20,
            min25,
            min30,
            min35,
            min40,
            min45,
            min50,
            min55,
            defaultTemplate,
            zeroFiller,
            earlyMorning,
            morning,
            afternoon,
            night,
            dayPeriodPosition,
            hourZeroWord,
            hourOneWord,
            hourTwelveWord,
            hourSuffixSingular,
            hourSuffixPlural,
            singularArticle,
            pluralArticle,
            applyEifelerRule,
            pastHourTemplate,
            beforeHalfTemplate,
            afterHalfTemplate,
            beforeNextTemplate,
            minuteSuffixSingular,
            minuteSuffixPlural,
            hourSuffixPaucal,
            minuteSuffixPaucal,
            hourWordsMap ?? [],
            minuteWordsMap ?? [],
            compactMinuteWords,
            paucalLowOnly,
            compactConjunction);
#endif

    static (bool Success, long Value, string? UnrecognizedWord) InvokeInvertedTensTryParseCompact(
        InvertedTensWordsToNumberConverter converter,
        string words)
    {
        object?[] arguments = [words, 0L, null];
        var success = InvokePrivate<bool>(typeof(InvertedTensWordsToNumberConverter), converter, "TryParseCompact", arguments);

        return (success, (long)arguments[1]!, (string?)arguments[2]);
    }

    static T InvokePrivate<T>(
        [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicMethods)] Type targetType,
        object? target,
        string methodName,
        params object?[] arguments)
    {
        var flags = System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.Static |
            System.Reflection.BindingFlags.NonPublic;
        var method = targetType.GetMethod(methodName, flags);
        Assert.NotNull(method);

        return (T)method.Invoke(method.IsStatic ? null : target, arguments)!;
    }

    static T InvokePrivate<T>(
        [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.NonPublicMethods)] Type targetType,
        object? target,
        string methodName,
        Type[] parameterTypes,
        params object?[] arguments)
    {
        var flags = System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.Static |
            System.Reflection.BindingFlags.NonPublic;
        var method = targetType.GetMethod(methodName, flags, binder: null, types: parameterTypes, modifiers: null);
        Assert.NotNull(method);

        return (T)method.Invoke(method.IsStatic ? null : target, arguments)!;
    }

    sealed class GenderEchoOrdinalConverter : INumberToWordsConverter
    {
        public string Convert(long number) => number.ToString(CultureInfo.InvariantCulture);

        public string Convert(long number, WordForm wordForm) => Convert(number);

        public string Convert(long number, bool addAnd) => Convert(number);

        public string Convert(long number, bool addAnd, WordForm wordForm) => Convert(number);

        public string Convert(long number, GrammaticalGender gender, bool addAnd = true) => Convert(number);

        public string Convert(long number, WordForm wordForm, GrammaticalGender gender, bool addAnd = true) => Convert(number);

        public string ConvertToOrdinal(int number) => $"default {number}.";

        public string ConvertToOrdinal(int number, WordForm wordForm) => ConvertToOrdinal(number);

        public string ConvertToOrdinal(int number, GrammaticalGender gender) =>
            $"{gender.ToString().ToLowerInvariant()} {number}.";

        public string ConvertToOrdinal(int number, GrammaticalGender gender, WordForm wordForm) =>
            ConvertToOrdinal(number, gender);

        public string ConvertToTuple(int number) => Convert(number);
    }
}