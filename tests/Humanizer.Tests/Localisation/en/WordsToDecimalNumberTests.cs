namespace Humanizer.Tests.Localisation.en;

[UseCulture("en-US")]
public class WordsToDecimalNumberTests
{
    public static IEnumerable<object[]> SupportedCultureRows =>
        global::Humanizer.Tests.Localisation.LocaleCoverageData.ShippedLocales
            .Select(static localeName => new object[] { localeName });

    [Theory]
    [InlineData("one point two", "1.2")]
    [InlineData("three point one four", "3.14")]
    [InlineData("one point two\u00A0three", "1.23")]
    [InlineData("zero point zero five", "0.05")]
    [InlineData("one point five zero", "1.50")]
    [InlineData("minus two point five", "-2.5")]
    [InlineData("minus zero point five", "-0.5")]
    [InlineData("negative point zero five", "-0.05")]
    [InlineData("one hundred and five point zero", "105.0")]
    [InlineData("ten quintillion point one", "10000000000000000000.1")]
    [InlineData("ten quintillion one point two", "10000000000000000001.2")]
    [InlineData("negative ten quintillion point one", "-10000000000000000000.1")]
    [InlineData("point five", "0.5")]
    public void ParsesEnglishDecimalPhrasesAndPreservesScale(string words, string expected)
    {
        var parsed = words.ToDecimalNumber(CultureInfo.CurrentCulture);

        Assert.Equal(expected, parsed.ToString(CultureInfo.InvariantCulture));
    }

    [Fact]
    public void ParsesLocaleSpecificScaleBeyondLongRange()
    {
        var parsed = "ninety-three shankh point one".ToDecimalNumber(new("en-IN"));

        Assert.Equal("9300000000000000000.1", parsed.ToString(CultureInfo.InvariantCulture));
    }

    [Theory]
    [InlineData("one", "one")]
    [InlineData("one point", "point")]
    [InlineData("point", "point")]
    [InlineData("one point two point three", "point")]
    [InlineData("one point two and three", "and")]
    [InlineData("one point ten", "ten")]
    [InlineData("one point 2", "2")]
    [InlineData("one dot two", "one dot two")]
    [InlineData("onepointtwo", "onepointtwo")]
    [InlineData("one point twothree", "twothree")]
    [InlineData("minusone point two", "minusone")]
    public void RejectsMalformedDecimalPhrases(string words, string expectedUnrecognizedWord)
    {
        var success = words.TryToDecimalNumber(
            out var parsed,
            CultureInfo.CurrentCulture,
            out var unrecognizedWord);

        Assert.False(success);
        Assert.Equal(0m, parsed);
        Assert.Equal(expectedUnrecognizedWord, unrecognizedWord);
        Assert.Throws<FormatException>(() => words.ToDecimalNumber(CultureInfo.CurrentCulture));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void TryToDecimalNumberReturnsFalseForEmptyInput(string words)
    {
        Assert.False(words.TryToDecimalNumber(out var parsed, CultureInfo.CurrentCulture));
        Assert.Equal(0m, parsed);
    }

    [Fact]
    public void NullInputUsesTryAndThrowConventions()
    {
        string words = null!;

        Assert.False(words.TryToDecimalNumber(out var parsed, CultureInfo.CurrentCulture));
        Assert.Equal(0m, parsed);
        Assert.Throws<ArgumentNullException>(() => words.ToDecimalNumber(CultureInfo.CurrentCulture));
    }

    [Fact]
    public void DirectConvertersHonorNullInputContract()
    {
        var english = new LocalizedWordsToDecimalNumberConverter(
            CultureInfo.CurrentCulture,
            "point",
            ["minus ", "negative "],
            []);
        var unsupported = UnsupportedWordsToDecimalNumberConverter.Instance;

        Assert.False(english.TryConvert(null!, out _, out var englishUnrecognizedNumber));
        Assert.Equal(string.Empty, englishUnrecognizedNumber);
        Assert.True(english.TryConvert("one point two", out _, out var recognizedNumber));
        Assert.Null(recognizedNumber);
        Assert.False(unsupported.TryConvert(null!, out _, out var unsupportedUnrecognizedNumber));
        Assert.Equal(string.Empty, unsupportedUnrecognizedNumber);
        Assert.Throws<ArgumentNullException>(() => unsupported.Convert(null!));
    }

    [Fact]
    public void DirectConverterRejectsEmptyDecimalMarker() =>
        Assert.Throws<ArgumentException>(() => new LocalizedWordsToDecimalNumberConverter(
            CultureInfo.CurrentCulture,
            " \t ",
            [],
            []));

    [Fact]
    public void RejectsFractionBeyondDecimalScale()
    {
        var words = $"zero point {string.Join(" ", Enumerable.Repeat("one", 29))}";

        Assert.False(words.TryToDecimalNumber(
            out var parsed,
            CultureInfo.CurrentCulture,
            out var unrecognizedWord));
        Assert.Equal(0m, parsed);
        Assert.Equal("one", unrecognizedWord);
    }

    [Fact]
    public void RejectsValueThatCannotPreserveItsAuthoredScale()
    {
        var words = $"{long.MaxValue.ToWords(CultureInfo.CurrentCulture)} point one two three four five six seven eight nine zero";

        Assert.False(words.TryToDecimalNumber(out var parsed, CultureInfo.CurrentCulture));
        Assert.Equal(0m, parsed);
        Assert.Throws<FormatException>(() => words.ToDecimalNumber(CultureInfo.CurrentCulture));
    }

    [Fact]
    public void PreservesNegativeZeroSignAndScale()
    {
        var parsed = "minus zero point zero".ToDecimalNumber(CultureInfo.CurrentCulture);
        var bits = decimal.GetBits(parsed);

        Assert.Equal(1, bits[3] >> 16 & 0xFF);
        Assert.NotEqual(0, bits[3] & int.MinValue);
    }

    [Fact]
    public void FrenchCultureParsesLocalizedDecimalWords()
    {
        var culture = new CultureInfo("fr-FR");

        Assert.True("un virgule deux".TryToDecimalNumber(
            out var parsed,
            culture,
            out var unrecognizedWord));
        Assert.Equal(1.2m, parsed);
        Assert.Null(unrecognizedWord);
        Assert.Equal(1.2m, "un virgule deux".ToDecimalNumber(culture));
    }

    [Theory]
    [InlineData("fr-FR", "un virgule deux")]
    [InlineData("hi-IN", "एक दशमलव दो")]
    [InlineData("hy", "մեկ ստորակետ երկու")]
    [InlineData("ur-PK", "ایک اعشاریہ دو")]
    [InlineData("zh-CN", "一点二")]
    [InlineData("zu-ZA", "kanye idesimalikhoma kubili")]
    public void ParsesRepresentativeNativeDecimalPhrases(string cultureName, string words) =>
        Assert.Equal(1.2m, words.ToDecimalNumber(new(cultureName)));

    [Fact]
    public void MalteseParsesNegativeSuffixAfterCompleteDecimalPhrase() =>
        Assert.Equal(
            -1.2m,
            "wieħed punt tnejn inqas minn żero".ToDecimalNumber(new("mt")));

    [Theory]
    [MemberData(nameof(SupportedCultureRows))]
    public void AllSupportedCulturesParseAuthoredDecimalMarkerWithGeneratedDigits(string cultureName)
    {
        var culture = new CultureInfo(cultureName);
        var converter = Assert.IsType<LocalizedWordsToDecimalNumberConverter>(
            Configurator.GetWordsToDecimalNumberConverter(culture));
        var words = $"{1.ToWords(culture)} {converter.DecimalMarker} {2.ToWords(culture)}";

        Assert.True(words.TryToDecimalNumber(out var parsed, culture, out var unrecognizedWord));
        Assert.Equal(1.2m, parsed);
        Assert.Null(unrecognizedWord);
        Assert.Equal(1.2m, words.ToDecimalNumber(culture));
    }

    [Fact]
    public void SameLanguageDescendantUsesLocalizedParentProfile()
    {
        var culture = new CultureInfo("fr-CA");

        Assert.Equal(1.2m, "un virgule deux".ToDecimalNumber(culture));
    }

    [Fact]
    public void NonEnglishCultureDoesNotUseEnglishOverflowComposition()
    {
        var culture = new CultureInfo("fr-FR");

        Assert.False("dix trillions virgule un".TryToDecimalNumber(
            out var parsed,
            culture,
            out _));
        Assert.Equal(0m, parsed);
    }

    [Fact]
    public void UnsupportedCultureUsesTryAndThrowConventions()
    {
        var culture = new CultureInfo("gd");

        Assert.False("aon point dhà".TryToDecimalNumber(
            out var parsed,
            culture,
            out var unrecognizedWord));
        Assert.Equal(0m, parsed);
        Assert.Equal("aon point dhà", unrecognizedWord);
        Assert.Throws<NotSupportedException>(() => "aon point dhà".ToDecimalNumber(culture));
    }

    [Fact]
    public void IntegerToNumberSemanticsRemainUnchanged()
    {
        Assert.False("one point two".TryToNumber(
            out var parsed,
            CultureInfo.CurrentCulture,
            out var unrecognizedWord));
        Assert.Equal(0L, parsed);
        Assert.Equal("point", unrecognizedWord);
    }
}