namespace Humanizer.Tests.Localisation.pa_Arab;

public class PunjabiArabicVariantTests
{
    static readonly CultureInfo PaArab = new("pa-Arab");

    public static TheoryData<string> PunjabiArabicCultures { get; } = new()
    {
        "pa-Arab"
    };

    [Theory]
    [MemberData(nameof(PunjabiArabicCultures))]
    public void NumberWordsAndParsing_UseShahmukhiPunjabi(string cultureName)
    {
        var culture = new CultureInfo(cultureName);
        const string words = "اک کروڑ تےئی لکھ پنتالی ہزار چھے سو اٹھتر";

        Assert.Equal(words, 12345678.ToWords(culture));
        Assert.Equal(12345678, words.ToNumber(culture));
        Assert.Equal("منفی پنج", (-5).ToWords(culture));
        Assert.Equal(-5, "مائنس پنج".ToNumber(culture));
        AssertNoGurmukhi(words);
    }

    [Theory]
    [MemberData(nameof(PunjabiArabicCultures))]
    public void Ordinals_UseShahmukhiGenderedForms(string cultureName)
    {
        var culture = new CultureInfo(cultureName);

        Assert.Equal("اکیواں", 21.ToOrdinalWords(GrammaticalGender.Masculine, culture));
        Assert.Equal("اکیویں", 21.ToOrdinalWords(GrammaticalGender.Feminine, culture));
        Assert.Equal("پنجویں", 5.Ordinalize(GrammaticalGender.Feminine, culture));
        Assert.Equal(21, "اکیویں".ToNumber(culture));
    }

    [Theory]
    [MemberData(nameof(PunjabiArabicCultures))]
    public void FormatterAndCollections_UseShahmukhiPunjabi(string cultureName)
    {
        var culture = new CultureInfo(cultureName);
        var formatter = Configurator.Formatters.ResolveForCulture(culture);
        var collectionFormatter = Configurator.CollectionFormatters.ResolveForCulture(culture);

        Assert.Equal("کل", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 1));
        Assert.Equal("2 دن", formatter.TimeSpanHumanize(TimeUnit.Day, 2));
        Assert.Equal("بائٹ", formatter.DataUnitHumanize(DataUnit.Byte, 2, toSymbol: false));
        Assert.Equal("گھنٹہ", formatter.TimeUnitHumanize(TimeUnit.Hour));
        Assert.Equal("1, 2 اتے 3", collectionFormatter.Humanize([1, 2, 3]));
    }

    [Theory]
    [MemberData(nameof(PunjabiArabicCultures))]
    [UseCulture("pa-Arab")]
    public void DateCompassAndNumberFormatting_UseVariantSpecificData(string cultureName)
    {
        var culture = new CultureInfo(cultureName);

        Assert.Equal("25 جنوری 2022", new DateTime(2022, 1, 25).ToOrdinalWords());
        Assert.Equal("5 مئی 2022", new DateTime(2022, 5, 5).ToOrdinalWords());
#if NET6_0_OR_GREATER
        Assert.Equal("سویرے اک وجے", new TimeOnly(1, 0).ToClockNotation());
#endif

        Assert.Equal("پورب", 90.0.ToHeading(HeadingStyle.Full, culture));
        Assert.Equal("1٫95 KB", 2000.Bytes().Humanize(culture));
    }

    [Fact]
    public void WordsToNumber_RejectsGurmukhiTokensUnderPunjabiArabicCulture()
    {
        Assert.False("ਪੰਜ".TryToNumber(out _, PaArab));
        Assert.Throws<ArgumentException>(() => "ਪੰਜ".ToNumber(PaArab));
    }

    [Fact]
    public void PunjabiArabic_IsTheOnlyAuthoredArabicScriptPunjabiVariant()
    {
        Assert.Equal("اک کروڑ تےئی لکھ پنتالی ہزار چھے سو اٹھتر", 12345678.ToWords(PaArab));
    }

    static void AssertNoGurmukhi(string value)
    {
        Assert.DoesNotContain(value, static c => c is >= '\u0A00' and <= '\u0A7F');
    }

}