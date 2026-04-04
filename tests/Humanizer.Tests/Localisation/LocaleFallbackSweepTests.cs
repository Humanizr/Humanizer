using System.Globalization;

namespace Humanizer.Tests.Localisation;

public class LocaleFallbackSweepTests
{
    [Theory]
    [MemberData(nameof(LocaleCoverageData.FormatterLocaleTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void RegisteredFormatterLocalesExecuteRepresentativePaths(string cultureName)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo(cultureName));

        Assert.False(string.IsNullOrWhiteSpace(formatter.DateHumanize(TimeUnit.Day, Tense.Past, 1)));
        Assert.False(string.IsNullOrWhiteSpace(formatter.DateHumanize(TimeUnit.Day, Tense.Future, 2)));
        Assert.False(string.IsNullOrWhiteSpace(formatter.TimeSpanHumanize(TimeUnit.Day, 2)));
        Assert.False(string.IsNullOrWhiteSpace(formatter.DataUnitHumanize(DataUnit.Byte, 2, toSymbol: false)));
        Assert.False(string.IsNullOrWhiteSpace(formatter.TimeUnitHumanize(TimeUnit.Day)));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.FormatterParentInheritanceExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void FormatterParentLocalesUseExpectedInheritedOutputs(string cultureName, string expectedPast, string expectedTimeSpan)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo(cultureName));

        Assert.Equal(expectedPast, formatter.DateHumanize(TimeUnit.Day, Tense.Past, 1));
        Assert.Equal(expectedTimeSpan, formatter.TimeSpanHumanize(TimeUnit.Day, 2));
    }

    [Theory]
    [MemberData(nameof(LocaleCoverageData.CollectionFormatterExpectationTheoryData), MemberType = typeof(LocaleCoverageData))]
    public void RegisteredCollectionFormatterLocalesUseExpectedConjunction(string cultureName, string expectedTwo, string expectedThree)
    {
        var formatter = Configurator.CollectionFormatters.ResolveForCulture(new CultureInfo(cultureName));

        Assert.Equal(expectedTwo, formatter.Humanize([1, 2]));
        Assert.Equal(expectedThree, formatter.Humanize([1, 2, 3]));
    }

    [Fact]
    public void DanishCultureUsesLocalizedCollectionFormatter()
    {
        var formatter = Configurator.CollectionFormatters.ResolveForCulture(new CultureInfo("da"));

        Assert.Equal("1 og 2", formatter.Humanize([1, 2]));
    }

    [Theory]
    [InlineData("en")]
    [InlineData("de")]
    [InlineData("fr")]
    public void HeadingShortFormsRoundTripThroughLocalizedResourceKeys(string cultureName)
    {
        var culture = new CultureInfo(cultureName);
        var heading = 45d;

        var abbreviated = heading.ToHeading(HeadingStyle.Abbreviated, culture);
        var full = heading.ToHeading(HeadingStyle.Full, culture);

        Assert.Equal(heading, abbreviated.FromAbbreviatedHeading(culture));
        Assert.False(string.IsNullOrWhiteSpace(full));
    }
}
