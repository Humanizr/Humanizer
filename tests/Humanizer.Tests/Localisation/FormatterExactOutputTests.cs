namespace Humanizer.Tests.Localisation;

public class FormatterExactOutputTests
{
    static readonly DateTime LocalBase = new(2013, 6, 20, 11, 58, 22, DateTimeKind.Local);
    static readonly DateTime UtcBase = new(2013, 6, 20, 9, 58, 22, DateTimeKind.Utc);

    [Theory]
    [MemberData(nameof(LocaleFormatterExactTheoryData.DateDayPluralCases), MemberType = typeof(LocaleFormatterExactTheoryData))]
    public void UsesExpectedDateHumanizeDayPluralOutputs(string localeName, LocaleFormatterExactTheoryData.DateDayPluralExpectationRow expected)
    {
        var culture = GetCulture(localeName);

        for (var index = 0; index < LocaleFormatterExactTheoryData.DatePluralDayCounts.Length; index++)
        {
            var dayCount = LocaleFormatterExactTheoryData.DatePluralDayCounts[index];

            Assert.Equal(expected.PastFor(dayCount), ToVisibleText(LocalBase.AddDays(-dayCount).Humanize(false, LocalBase, culture)));
            Assert.Equal(expected.PastFor(dayCount), ToVisibleText(UtcBase.AddDays(-dayCount).Humanize(true, UtcBase, culture)));
            Assert.Equal(expected.FutureFor(dayCount), ToVisibleText(LocalBase.AddDays(dayCount).Humanize(false, LocalBase, culture)));
            Assert.Equal(expected.FutureFor(dayCount), ToVisibleText(UtcBase.AddDays(dayCount).Humanize(true, UtcBase, culture)));
        }
    }

    [Theory]
    [MemberData(nameof(LocaleFormatterExactTheoryData.MultiPartTimeSpanCases), MemberType = typeof(LocaleFormatterExactTheoryData))]
    public void UsesExpectedMultiPartTimeSpanHumanizeOutputs(string localeName, LocaleFormatterExactTheoryData.MultiPartTimeSpanExpectationRow expected)
    {
        var culture = GetCulture(localeName);

        Assert.Equal(expected.LargestSpan, ToVisibleText(TimeSpan.FromMilliseconds(1299630020).Humanize(3, culture: culture, collectionSeparator: null)));
        Assert.Equal(expected.DenseSpan, ToVisibleText(new TimeSpan(1, 0, 3, 4).Humanize(3, countEmptyUnits: false, culture: culture, maxUnit: TimeUnit.Day, minUnit: TimeUnit.Second, collectionSeparator: null)));
        Assert.Equal(expected.DenseSpanWithEmptyUnits, ToVisibleText(new TimeSpan(1, 0, 3, 4).Humanize(3, countEmptyUnits: true, culture: culture, maxUnit: TimeUnit.Day, minUnit: TimeUnit.Second, collectionSeparator: null)));
    }

    [Theory]
    [MemberData(nameof(LocaleFormatterExactTheoryData.TimeUnitSymbolCases), MemberType = typeof(LocaleFormatterExactTheoryData))]
    public void UsesExpectedTimeUnitSymbols(string localeName, LocaleFormatterExactTheoryData.TimeUnitSymbolExpectationRow expected)
    {
        var culture = GetCulture(localeName);

        Assert.Equal(expected.Millisecond, ToVisibleText(TimeUnit.Millisecond.ToSymbol(culture)));
        Assert.Equal(expected.Second, ToVisibleText(TimeUnit.Second.ToSymbol(culture)));
        Assert.Equal(expected.Minute, ToVisibleText(TimeUnit.Minute.ToSymbol(culture)));
        Assert.Equal(expected.Hour, ToVisibleText(TimeUnit.Hour.ToSymbol(culture)));
        Assert.Equal(expected.Day, ToVisibleText(TimeUnit.Day.ToSymbol(culture)));
        Assert.Equal(expected.Week, ToVisibleText(TimeUnit.Week.ToSymbol(culture)));
        Assert.Equal(expected.Month, ToVisibleText(TimeUnit.Month.ToSymbol(culture)));
        Assert.Equal(expected.Year, ToVisibleText(TimeUnit.Year.ToSymbol(culture)));
    }

    [Theory]
    [MemberData(nameof(LocaleFormatterExactTheoryData.ByteSizeSymbolCases), MemberType = typeof(LocaleFormatterExactTheoryData))]
    public void UsesExpectedByteSizeHumanizeSymbols(string localeName, LocaleFormatterExactTheoryData.ByteSizeSymbolExpectationRow expected)
    {
        var culture = GetCulture(localeName);

        Assert.Equal(expected.OneBit, ToVisibleText(ByteSize.FromBits(1).Humanize(culture)));
        Assert.Equal(expected.TwoBytes, ToVisibleText(ByteSize.FromBytes(2).Humanize(culture)));
        Assert.Equal(expected.TwoThousandBytesAsKilobytes, ToVisibleText(ByteSize.FromBytes(2000).Humanize("KB", culture)));
        Assert.Equal(expected.TwoMegabytesAsKilobytes, ToVisibleText(ByteSize.FromMegabytes(2).Humanize("KB", culture)));
    }

    [Theory]
    [MemberData(nameof(LocaleFormatterExactTheoryData.ByteSizeFullWordCases), MemberType = typeof(LocaleFormatterExactTheoryData))]
    public void UsesExpectedByteSizeFullWords(string localeName, LocaleFormatterExactTheoryData.ByteSizeFullWordExpectationRow expected)
    {
        var culture = GetCulture(localeName);

        Assert.Equal(expected.OneBit, ToVisibleText(ByteSize.FromBits(1).ToFullWords(provider: culture)));
        Assert.Equal(expected.OneByte, ToVisibleText(ByteSize.FromBytes(1).ToFullWords(provider: culture)));
        Assert.Equal(expected.TwoBytes, ToVisibleText(ByteSize.FromBytes(2).ToFullWords(provider: culture)));
        Assert.Equal(expected.TwoKilobytes, ToVisibleText(ByteSize.FromKilobytes(2).ToFullWords(provider: culture)));
        Assert.Equal(expected.TwoMegabytes, ToVisibleText(ByteSize.FromMegabytes(2).ToFullWords(provider: culture)));
    }

    [Theory]
    [MemberData(nameof(LocaleFormatterExactTheoryData.CollectionHumanizeCases), MemberType = typeof(LocaleFormatterExactTheoryData))]
    public void UsesExpectedCollectionHumanizeOutputs(string localeName, LocaleFormatterExactTheoryData.CollectionHumanizeExpectationRow expected)
    {
        var culture = GetCulture(localeName);

        WithCulture(
            culture,
            () =>
            {
                Assert.Equal(expected.Pair, ToVisibleText(new[] { 1, 2 }.Humanize()));
                Assert.Equal(expected.Triple, ToVisibleText(new[] { 1, 2, 3 }.Humanize()));
                Assert.Equal(expected.Quadruple, ToVisibleText(new[] { 1, 2, 3, 4 }.Humanize()));
            });
    }

    [Theory]
    [MemberData(nameof(LocaleFormatterExactTheoryData.HeadingCases), MemberType = typeof(LocaleFormatterExactTheoryData))]
    public void UsesExpectedLocalizedHeadingOutputs(string localeName, LocaleFormatterExactTheoryData.HeadingExpectationRow expected)
    {
        var culture = GetCulture(localeName);

        for (var index = 0; index < LocaleFormatterExactTheoryData.HeadingAngles.Length; index++)
        {
            var heading = LocaleFormatterExactTheoryData.HeadingAngles[index];
            var abbreviated = heading.ToHeading(HeadingStyle.Abbreviated, culture);
            var full = heading.ToHeading(HeadingStyle.Full, culture);

            Assert.Equal(expected.AbbreviatedForIndex(index), ToVisibleText(abbreviated));
            Assert.Equal(expected.FullForIndex(index), ToVisibleText(full));
        }
    }

    [Theory]
    [MemberData(nameof(LocaleFormatterExactTheoryData.HeadingCases), MemberType = typeof(LocaleFormatterExactTheoryData))]
    public void ParsesExpectedLocalizedHeadingAbbreviations(string localeName, LocaleFormatterExactTheoryData.HeadingExpectationRow expected)
    {
        var culture = GetCulture(localeName);
        foreach (var index in new[] { 0, 4, 8, 12 })
        {
            var expectedHeading = LocaleFormatterExactTheoryData.HeadingAngles[index];

            Assert.Equal(expectedHeading, expected.AbbreviatedForIndex(index).FromAbbreviatedHeading(culture));
        }
    }

    [Theory]
    [MemberData(nameof(LocaleFormatterExactTheoryData.HeadingAbbreviatedCardinalCases), MemberType = typeof(LocaleFormatterExactTheoryData))]
    public void UsesExpectedLocalizedCardinalHeadingAbbreviations(string localeName, LocaleFormatterExactTheoryData.CardinalHeadingExpectationRow expected)
    {
        var culture = GetCulture(localeName);

        for (var index = 0; index < LocaleFormatterExactTheoryData.CardinalHeadingAngles.Length; index++)
        {
            Assert.Equal(expected.AbbreviatedForIndex(index), ToVisibleText(LocaleFormatterExactTheoryData.CardinalHeadingAngles[index].ToHeading(HeadingStyle.Abbreviated, culture)));
        }
    }

    static CultureInfo GetCulture(string localeName) => CultureInfo.GetCultureInfo(localeName);

    static void WithCulture(CultureInfo culture, Action action) =>
        WithCulture<object?>(
            culture,
            () =>
            {
                action();
                return null;
            });

    static T WithCulture<T>(CultureInfo culture, Func<T> action)
    {
        var originalCulture = CultureInfo.CurrentCulture;
        var originalUICulture = CultureInfo.CurrentUICulture;

        try
        {
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            return action();
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUICulture;
        }
    }

    static string ToVisibleText(string value) =>
        new(value.Where(static ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.Format).ToArray());
}