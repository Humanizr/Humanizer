using Humanizer.Tests.Localisation;

#if NET6_0_OR_GREATER
namespace Humanizer.Tests.Localisation
{
    public class ChildCultureCollectionFormatterFallbackTests
    {
        [Theory]
        [InlineData("bn-BD", "1 ও 2")]
        [InlineData("fi-FI", "1 ja 2")]
        [InlineData("fil-PH", "1 at 2")]
        [InlineData("fr-BE", "1 et 2")]
        [InlineData("ko-KR", "1 및 2")]
        [InlineData("ms-MY", "1 dan 2")]
        [InlineData("nb-NO", "1 og 2")]
        [InlineData("ru-RU", "1 и 2")]
        [InlineData("uk-UA", "1 і 2")]
        [InlineData("zh-HK", "1、2")]
        public void ChildCulturesReuseParentCollectionFormatter(string cultureName, string expected)
        {
            var formatter = Configurator.CollectionFormatters.ResolveForCulture(new(cultureName));

            Assert.Equal(expected, formatter.Humanize(["1", "2"]));
        }
    }

    public class ExplicitCollectionFormatterCoverageTests
    {
        [Fact]
        public void NynorskCollectionFormatterUsesOg()
        {
            var formatter = Configurator.CollectionFormatters.ResolveForCulture(new("nn"));

            Assert.Equal("1 og 2", formatter.Humanize(["1", "2"]));
        }
    }

    public class ChildCultureFormatterFallbackTests
    {
        [Theory]
        [InlineData("de-CH", "gestern")]
        [InlineData("de-LI", "gestern")]
        [InlineData("fr-CH", "hier")]
        public void ChildCulturesReuseParentFormatter(string cultureName, string expected)
        {
            var formatter = Configurator.Formatters.ResolveForCulture(new(cultureName));

            Assert.Equal(expected, formatter.DateHumanize(TimeUnit.Day, Tense.Past, 1));
        }

        [Fact]
        public void HongKongChineseFormatterFallsBackToTraditionalChineseResources()
        {
            var formatter = Configurator.Formatters.ResolveForCulture(new("zh-HK"));

            Assert.Equal("昨天", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 1));
        }

        [Fact]
        public void TamilFormatterFallsBackToDefaultEnglishResources()
        {
            var formatter = Configurator.Formatters.ResolveForCulture(new("ta"));

            Assert.Equal("yesterday", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 1));
        }

        [Fact]
        public void IndianEnglishFormatterKeepsEnglishResources()
        {
            var formatter = Configurator.Formatters.ResolveForCulture(new("en-IN"));

            Assert.Equal("yesterday", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 1));
        }
    }
}

namespace deCH
{
    [UseCulture("de-CH")]
    public class LocaleFallbackSweepTests
    {
        [Fact]
        public void ChildCultureReusesGermanOrdinalizer() =>
            Assert.Equal("1.", "1".Ordinalize());

        [Fact]
        public void ChildCultureReusesGermanDateToOrdinalWordsConverter() =>
            Assert.Equal("1. Januar 2015", new DateTime(2015, 1, 1).ToOrdinalWords());

        [Fact]
        public void ChildCultureReusesGermanDateOnlyToOrdinalWordsConverter() =>
            Assert.Equal("1. Januar 2015", new DateOnly(2015, 1, 1).ToOrdinalWords());

        [Fact]
        public void ChildCultureReusesGermanClockNotationConverter() =>
            Assert.Equal("fünf vor halb zwei", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
}

namespace deLI
{
    [UseCulture("de-LI")]
    public class LocaleFallbackSweepTests
    {
        [Fact]
        public void ChildCultureReusesGermanOrdinalizer() =>
            Assert.Equal("1.", "1".Ordinalize());

        [Fact]
        public void ChildCultureReusesGermanDateToOrdinalWordsConverter() =>
            Assert.Equal("1. Januar 2015", new DateTime(2015, 1, 1).ToOrdinalWords());

        [Fact]
        public void ChildCultureReusesGermanDateOnlyToOrdinalWordsConverter() =>
            Assert.Equal("1. Januar 2015", new DateOnly(2015, 1, 1).ToOrdinalWords());

        [Fact]
        public void ChildCultureReusesGermanClockNotationConverter() =>
            Assert.Equal("fünf vor halb zwei", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
}

namespace frCH
{
    [UseCulture("fr-CH")]
    public class LocaleFallbackSweepTests
    {
        [Fact]
        public void ChildCultureReusesFrenchOrdinalizer() =>
            Assert.Equal("1er", "1".Ordinalize(GrammaticalGender.Masculine));

        [Fact]
        public void ChildCultureReusesFrenchDateToOrdinalWordsConverter() =>
            Assert.Equal("1er janvier 2015", new DateTime(2015, 1, 1).ToOrdinalWords());

        [Fact]
        public void ChildCultureReusesFrenchDateOnlyToOrdinalWordsConverter() =>
            Assert.Equal("1er janvier 2015", new DateOnly(2015, 1, 1).ToOrdinalWords());

        [Fact]
        public void ChildCultureReusesFrenchClockNotationConverter() =>
            Assert.Equal("treize heures vingt-cinq", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
}

namespace zhHK
{
    [UseCulture("zh-HK")]
    public class LocaleFallbackSweepTests
    {
        [Fact]
        public void ChildCultureReusesTraditionalChineseOrdinalizer() =>
            Assert.Equal("1", "1".Ordinalize());

        [Fact]
        public void ChildCultureUsesCurrentCultureShortDateForDateTime() =>
            Assert.Equal(new DateTime(2015, 1, 1).ToString("d", CultureInfo.CurrentCulture), new DateTime(2015, 1, 1).ToOrdinalWords());

        [Fact]
        public void ChildCultureUsesCurrentCultureShortDateForDateOnly() =>
            Assert.Equal(new DateOnly(2015, 1, 1).ToString("d", CultureInfo.CurrentCulture), new DateOnly(2015, 1, 1).ToOrdinalWords());

        [Fact]
        public void ChildCultureUsesCurrentCultureShortTimeForTimeOnly() =>
            Assert.Equal(new TimeOnly(13, 23).ToString("t", CultureInfo.CurrentCulture), new TimeOnly(13, 23).ToClockNotation());
    }
}

namespace enIN
{
    [UseCulture("en-IN")]
    public class LocaleFallbackSweepTests
    {
        [Fact]
        public void SupportedCultureKeepsEnglishDateOnlyOrdinalWords() =>
            Assert.Equal("1st January 2015", new DateOnly(2015, 1, 1).ToOrdinalWords());

        [Fact]
        public void SupportedCultureKeepsEnglishClockNotation() =>
            Assert.Equal("twenty-five past one", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
}

namespace ta
{
    [UseCulture("ta")]
    public class LocaleFallbackSweepTests
    {
        [Fact]
        public void SupportedCultureUsesCurrentCultureShortDateForDateTime() =>
            Assert.Equal(new DateTime(2015, 1, 1).ToString("d", CultureInfo.CurrentCulture), new DateTime(2015, 1, 1).ToOrdinalWords());

        [Fact]
        public void SupportedCultureUsesCurrentCultureShortDateForDateOnly() =>
            Assert.Equal(new DateOnly(2015, 1, 1).ToString("d", CultureInfo.CurrentCulture), new DateOnly(2015, 1, 1).ToOrdinalWords());

        [Fact]
        public void SupportedCultureUsesCurrentCultureShortTimeForTimeOnly() =>
            Assert.Equal(new TimeOnly(13, 23).ToString("t", CultureInfo.CurrentCulture), new TimeOnly(13, 23).ToClockNotation());
    }
}

namespace zuZA
{
    [UseCulture("zu-ZA")]
    public class LocaleFallbackSweepTests
    {
        static readonly CultureInfo UnknownCulture = new("zu-ZA");

        [Fact]
        public void UnknownCultureUsesDefaultCollectionFormatter() =>
            Assert.Equal("1 & 2", Configurator.CollectionFormatters.ResolveForCulture(UnknownCulture).Humanize(["1", "2"]));

        [Fact]
        public void UnknownCultureFallsBackToEnglishFormatter() =>
            Assert.Equal(
                Configurator.Formatters.ResolveForCulture(new("en")).DateHumanize(TimeUnit.Day, Tense.Past, 1),
                Configurator.Formatters.ResolveForCulture(UnknownCulture).DateHumanize(TimeUnit.Day, Tense.Past, 1));

        [Fact]
        public void UnknownCultureFallsBackToEnglishNumberToWords() =>
            Assert.Equal(42.ToWords(new("en")), 42.ToWords(UnknownCulture));

        [Fact]
        public void UnknownCultureUsesDefaultWordsToNumberConverter() =>
            Assert.Throws<NotSupportedException>(() => "one".ToNumber(UnknownCulture));

        [Fact]
        public void UnknownCultureUsesDefaultOrdinalizer() =>
            Assert.Equal("1", 1.Ordinalize(UnknownCulture));

        [Fact]
        public void UnknownCultureUsesDefaultDateToOrdinalWordsConverter() =>
            Assert.Equal(new DateTime(2015, 1, 1).ToString("d", CultureInfo.CurrentCulture), new DateTime(2015, 1, 1).ToOrdinalWords());

        [Fact]
        public void UnknownCultureUsesDefaultDateOnlyToOrdinalWordsConverter() =>
            Assert.Equal(new DateOnly(2015, 1, 1).ToString("d", CultureInfo.CurrentCulture), new DateOnly(2015, 1, 1).ToOrdinalWords());

        [Fact]
        public void UnknownCultureUsesDefaultClockNotationConverter() =>
            Assert.Equal(new TimeOnly(13, 23).ToString("t", CultureInfo.CurrentCulture), new TimeOnly(13, 23).ToClockNotation());

        [Fact]
        public void UnknownCultureFallsBackToEnglishHeadingTable()
        {
            Assert.Equal(0d.ToHeading(HeadingStyle.Full, new("en")), 0d.ToHeading(HeadingStyle.Full, UnknownCulture));
            Assert.Equal(0d.ToHeading(HeadingStyle.Abbreviated, new("en")), 0d.ToHeading(HeadingStyle.Abbreviated, UnknownCulture));
            Assert.Equal("NNW".FromAbbreviatedHeading(new("en")), "NNW".FromAbbreviatedHeading(UnknownCulture));
        }
    }
}
#endif
