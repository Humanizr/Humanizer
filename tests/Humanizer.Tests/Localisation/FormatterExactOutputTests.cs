namespace Humanizer.Tests.Localisation;

public class FormatterExactOutputTests
{
    static readonly DateTime localBase = new(2013, 6, 20, 11, 58, 22, DateTimeKind.Local);
    static readonly DateTime utcBase = new(2013, 6, 20, 9, 58, 22, DateTimeKind.Utc);

    [Theory]
    [MemberData(nameof(LocaleFormatterExactTheoryData.DateDayPluralCases), MemberType = typeof(LocaleFormatterExactTheoryData))]
    public void UsesExpectedDateHumanizeDayPluralOutputs(string localeName, string[] expectedPast, string[] expectedFuture)
    {
        Assert.Equal(LocaleFormatterExactTheoryData.DatePluralDayCounts.Length, expectedPast.Length);
        Assert.Equal(LocaleFormatterExactTheoryData.DatePluralDayCounts.Length, expectedFuture.Length);

        var culture = GetCulture(localeName);

        for (var index = 0; index < LocaleFormatterExactTheoryData.DatePluralDayCounts.Length; index++)
        {
            var dayCount = LocaleFormatterExactTheoryData.DatePluralDayCounts[index];

            Assert.Equal(expectedPast[index], localBase.AddDays(-dayCount).Humanize(false, localBase, culture));
            Assert.Equal(expectedPast[index], utcBase.AddDays(-dayCount).Humanize(true, utcBase, culture));
            Assert.Equal(expectedFuture[index], localBase.AddDays(dayCount).Humanize(false, localBase, culture));
            Assert.Equal(expectedFuture[index], utcBase.AddDays(dayCount).Humanize(true, utcBase, culture));
        }
    }

    [Theory]
    [MemberData(nameof(LocaleFormatterExactTheoryData.MultiPartTimeSpanCases), MemberType = typeof(LocaleFormatterExactTheoryData))]
    public void UsesExpectedMultiPartTimeSpanHumanizeOutputs(string localeName, string[] expected)
    {
        var culture = GetCulture(localeName);

        string[] actual =
        [
            TimeSpan.FromMilliseconds(1299630020).Humanize(3, culture: culture, collectionSeparator: null),
            new TimeSpan(1, 0, 3, 4).Humanize(3, countEmptyUnits: false, culture: culture, maxUnit: TimeUnit.Day, minUnit: TimeUnit.Second, collectionSeparator: null),
            new TimeSpan(1, 0, 3, 4).Humanize(3, countEmptyUnits: true, culture: culture, maxUnit: TimeUnit.Day, minUnit: TimeUnit.Second, collectionSeparator: null)
        ];

        Assert.Equal(expected.Length, actual.Length);
        Assert.Collection(actual, expected.Select<string, Action<string>>(expectedValue => actualValue => Assert.Equal(expectedValue, actualValue)).ToArray());
    }

    [Theory]
    [MemberData(nameof(LocaleFormatterExactTheoryData.TimeUnitSymbolCases), MemberType = typeof(LocaleFormatterExactTheoryData))]
    public void UsesExpectedTimeUnitSymbols(string localeName, string[] expected)
    {
        var culture = GetCulture(localeName);

        string[] actual =
        [
            TimeUnit.Millisecond.ToSymbol(culture),
            TimeUnit.Second.ToSymbol(culture),
            TimeUnit.Minute.ToSymbol(culture),
            TimeUnit.Hour.ToSymbol(culture),
            TimeUnit.Day.ToSymbol(culture),
            TimeUnit.Week.ToSymbol(culture),
            TimeUnit.Month.ToSymbol(culture),
            TimeUnit.Year.ToSymbol(culture)
        ];

        Assert.Equal(expected.Length, actual.Length);
        Assert.Collection(actual, expected.Select<string, Action<string>>(expectedValue => actualValue => Assert.Equal(expectedValue, actualValue)).ToArray());
    }

    [Theory]
    [MemberData(nameof(LocaleFormatterExactTheoryData.ByteSizeSymbolCases), MemberType = typeof(LocaleFormatterExactTheoryData))]
    public void UsesExpectedByteSizeHumanizeSymbols(string localeName, string[] expected)
    {
        var culture = GetCulture(localeName);

        string[] actual =
        [
            ByteSize.FromBits(1).Humanize(culture),
            ByteSize.FromBytes(2).Humanize(culture),
            ByteSize.FromBytes(2000).Humanize("KB", culture),
            ByteSize.FromMegabytes(2).Humanize("KB", culture)
        ];

        Assert.Equal(expected.Length, actual.Length);
        Assert.Collection(actual, expected.Select<string, Action<string>>(expectedValue => actualValue => Assert.Equal(expectedValue, actualValue)).ToArray());
    }

    [Theory]
    [MemberData(nameof(LocaleFormatterExactTheoryData.ByteSizeFullWordCases), MemberType = typeof(LocaleFormatterExactTheoryData))]
    public void UsesExpectedByteSizeFullWords(string localeName, string[] expected)
    {
        var culture = GetCulture(localeName);

        string[] actual =
        [
            ByteSize.FromBits(1).ToFullWords(provider: culture),
            ByteSize.FromBytes(1).ToFullWords(provider: culture),
            ByteSize.FromBytes(2).ToFullWords(provider: culture),
            ByteSize.FromKilobytes(2).ToFullWords(provider: culture),
            ByteSize.FromMegabytes(2).ToFullWords(provider: culture)
        ];

        Assert.Equal(expected.Length, actual.Length);
        Assert.Collection(actual, expected.Select<string, Action<string>>(expectedValue => actualValue => Assert.Equal(expectedValue, actualValue)).ToArray());
    }

    [Theory]
    [MemberData(nameof(LocaleFormatterExactTheoryData.CollectionHumanizeCases), MemberType = typeof(LocaleFormatterExactTheoryData))]
    public void UsesExpectedCollectionHumanizeOutputs(string localeName, string[] expected)
    {
        var culture = GetCulture(localeName);
        var actual = WithCulture<string[]>(
            culture,
            static () =>
            [
                new[] { 1, 2 }.Humanize(),
                new[] { 1, 2, 3 }.Humanize(),
                new[] { 1, 2, 3, 4 }.Humanize()
            ]);

        Assert.Equal(expected.Length, actual.Length);
        Assert.Collection(actual, expected.Select<string, Action<string>>(expectedValue => actualValue => Assert.Equal(expectedValue, actualValue)).ToArray());
    }

    [Theory]
    [MemberData(nameof(LocaleFormatterExactTheoryData.HeadingCases), MemberType = typeof(LocaleFormatterExactTheoryData))]
    public void UsesExpectedLocalizedHeadingOutputs(string localeName, string[] expectedAbbreviated, string[] expectedFull)
    {
        Assert.Equal(LocaleFormatterExactTheoryData.HeadingAngles.Length, expectedAbbreviated.Length);
        Assert.Equal(LocaleFormatterExactTheoryData.HeadingAngles.Length, expectedFull.Length);

        var culture = GetCulture(localeName);

        for (var index = 0; index < LocaleFormatterExactTheoryData.HeadingAngles.Length; index++)
        {
            var heading = LocaleFormatterExactTheoryData.HeadingAngles[index];
            var abbreviated = heading.ToHeading(HeadingStyle.Abbreviated, culture);
            var full = heading.ToHeading(HeadingStyle.Full, culture);

            Assert.Equal(expectedAbbreviated[index], abbreviated);
            Assert.Equal(expectedFull[index], full);
        }
    }

    static CultureInfo GetCulture(string localeName) => CultureInfo.GetCultureInfo(localeName);

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
}
