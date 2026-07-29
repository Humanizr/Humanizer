namespace Humanizer.Tests;

[UseCulture("en-US")]
public class FractionalTimeSpanHumanizeTests
{
    static readonly CultureInfo English = new("en-US");

    [Theory]
    [InlineData(0.5, 7, "0.5 seconds")]
    [InlineData(1, 7, "1 second")]
    [InlineData(1.5, 7, "1.5 seconds")]
    [InlineData(2.1, 7, "2.1 seconds")]
    public void HumanizesFractionalSeconds(double seconds, int digits, string expected)
    {
        var actual = TimeSpan.FromSeconds(seconds).HumanizeWithFractionalSeconds(
            1,
            digits,
            MidpointRounding.ToEven,
            English,
            TimeUnit.Week);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void PreservesEveryTickAtSevenDigits()
    {
        var actual = TimeSpan.FromTicks(1).HumanizeWithFractionalSeconds(
            1,
            7,
            MidpointRounding.ToEven,
            English,
            TimeUnit.Second);

        Assert.Equal("0.0000001 seconds", actual);
    }

    [Theory]
    [InlineData(MidpointRounding.ToEven, "1.234 seconds")]
    [InlineData(MidpointRounding.AwayFromZero, "1.235 seconds")]
    public void SupportsBothMagnitudeRoundingModes(MidpointRounding roundingMode, string expected)
    {
        var actual = TimeSpan.FromTicks(12345000).HumanizeWithFractionalSeconds(
            1,
            3,
            roundingMode,
            English,
            TimeUnit.Second);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(599996000, "1 minute")]
    [InlineData(35999996000, "1 hour")]
    [InlineData(863999996000, "1 day")]
    [InlineData(6047999996000, "1 week")]
    public void RoundsEntireMagnitudeBeforeDecomposition(long ticks, string expected)
    {
        var actual = TimeSpan.FromTicks(ticks).HumanizeWithFractionalSeconds(
            2,
            3,
            MidpointRounding.ToEven,
            English,
            TimeUnit.Week);

        Assert.Equal(expected, actual);
        Assert.Equal(
            expected,
            TimeSpan.FromTicks(-ticks).HumanizeWithFractionalSeconds(
                2,
                3,
                MidpointRounding.ToEven,
                English,
                TimeUnit.Week));
    }

    [Fact]
    public void PrecisionCanReachASeparatedFractionalTerminal()
    {
        var timeSpan = TimeSpan.FromHours(1) + TimeSpan.FromSeconds(0.5);

        Assert.Equal(
            "1 hour, 0.5 seconds",
            timeSpan.HumanizeWithFractionalSeconds(
                2,
                7,
                MidpointRounding.ToEven,
                English,
                TimeUnit.Hour));
        Assert.Equal(
            "1 hour",
            timeSpan.HumanizeWithFractionalSeconds(
                2,
                true,
                7,
                MidpointRounding.ToEven,
                English,
                TimeUnit.Hour));
    }

    [Fact]
    public void ZeroAfterRoundingIsNumericSeconds()
    {
        var actual = TimeSpan.FromTicks(1).HumanizeWithFractionalSeconds(
            1,
            0,
            MidpointRounding.ToEven,
            English,
            TimeUnit.Week);

        Assert.Equal("0 seconds", actual);
    }

    [Fact]
    public void ZeroAfterRoundingUsesLocalizedNumericGrammar()
    {
        var actual = TimeSpan.FromTicks(1).HumanizeWithFractionalSeconds(
            1,
            0,
            MidpointRounding.ToEven,
            new("fr-FR"),
            TimeUnit.Week);

        Assert.Equal("0 seconde", actual);
    }

    [Fact]
    public void HandlesBothTimeSpanExtremesWithoutMagnitudeOverflow()
    {
        Assert.Equal(
            "922337203685.4775807 seconds",
            TimeSpan.MaxValue.HumanizeWithFractionalSeconds(
                1,
                7,
                MidpointRounding.ToEven,
                English,
                TimeUnit.Second));
        Assert.Equal(
            "922337203685.4775808 seconds",
            TimeSpan.MinValue.HumanizeWithFractionalSeconds(
                1,
                7,
                MidpointRounding.ToEven,
                English,
                TimeUnit.Second));
    }

    [Fact]
    public void RoundedExtremeCanExceedSignedTickRangeDuringFormatting()
    {
        Assert.Equal(
            "922337203685.5 seconds",
            TimeSpan.MinValue.HumanizeWithFractionalSeconds(
                1,
                1,
                MidpointRounding.ToEven,
                English,
                TimeUnit.Second));
        Assert.Equal(
            "922337203685.5 seconds",
            TimeSpan.MaxValue.HumanizeWithFractionalSeconds(
                1,
                1,
                MidpointRounding.ToEven,
                English,
                TimeUnit.Second));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(8)]
    public void RejectsInvalidFractionalDigitCounts(int digits) =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => TimeSpan.Zero.HumanizeWithFractionalSeconds(
                1,
                digits,
                MidpointRounding.ToEven,
                English,
                TimeUnit.Second));

    [Theory]
    [InlineData(MidpointRounding.ToZero)]
    [InlineData(MidpointRounding.ToNegativeInfinity)]
    [InlineData(MidpointRounding.ToPositiveInfinity)]
    public void RejectsDirectionalRounding(MidpointRounding roundingMode) =>
        Assert.Throws<ArgumentOutOfRangeException>(
            () => TimeSpan.Zero.HumanizeWithFractionalSeconds(
                1,
                3,
                roundingMode,
                English,
                TimeUnit.Second));

    [Fact]
    public void SymbolsUseLocalizedNumberFormattingAndExistingSecondSymbol()
    {
        var actual = TimeSpan.FromSeconds(1.5).HumanizeToSymbolsWithFractionalSeconds(
            1,
            7,
            MidpointRounding.ToEven,
            new("fr-FR"),
            TimeUnit.Second);

        Assert.Equal("1,5s", actual);
    }

    [Theory]
    [InlineData("fr-FR", 0.5, "0,5 seconde")]
    [InlineData("ru-RU", 1.5, "1,5 секунд")]
    [InlineData("sl", 1.5, "1,5 sekunde")]
    [InlineData("cs", 1.5, "1,5 sekundy")]
    public void UsesVisibleDecimalOperandsForLocaleGrammar(string cultureName, double seconds, string expected)
    {
        var actual = TimeSpan.FromSeconds(seconds).HumanizeWithFractionalSeconds(
            1,
            7,
            MidpointRounding.ToEven,
            new(cultureName),
            TimeUnit.Second);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("ar", 3, "3 ثوان")]
    [InlineData("ar", 11, "11 ثانية")]
    [InlineData("he", 0.5, "0.5 שניות")]
    [InlineData("pl", 5, "5 sekund")]
    [InlineData("lt", 2, "2 sekundės")]
    [InlineData("lt", 0.5, "0,5 sekundžių")]
    [InlineData("sl", 2, "2 sekundi")]
    [InlineData("ru", 2, "2 секунды")]
    [InlineData("ga", 7, "7 soicind")]
    [InlineData("ga", 11, "11 soicind")]
    [InlineData("ig", 1.5, "sekọnd 1.5")]
    [InlineData("hr", 1.1, "1,1 sekunde")]
    [InlineData("hr", 1.2, "1,2 sekunde")]
    [InlineData("hr", 1.5, "1,5 sekundi")]
    [InlineData("lt", 11, "11 sekundžių")]
    [InlineData("sl", 5, "5 sekund")]
    public void MapsCanonicalCategoriesThroughLocaleFormatterProfiles(
        string cultureName,
        double seconds,
        string expected)
    {
        var actual = TimeSpan.FromSeconds(seconds).HumanizeWithFractionalSeconds(
            1,
            7,
            MidpointRounding.ToEven,
            new(cultureName),
            TimeUnit.Second);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void UnsupportedCultureDoesNotFallBackToEnglish()
    {
        var formatter = new DefaultFormatter(new CultureInfo("eo"));

        var exception = Assert.Throws<InvalidOperationException>(
            () => formatter.TimeSpanHumanizeWithFractionalSeconds(0.5m, false));

        Assert.Contains("fractional-second grammar", exception.Message);
    }

    [Fact]
    public void LegacyFormatterHandlesRoundedIntegralButRejectsFractionalTerminal()
    {
        var culture = new CultureInfo("en-150");

        Assert.Equal(
            "2 seconds",
            TimeSpan.FromSeconds(1.6).HumanizeWithFractionalSeconds(
                1,
                0,
                MidpointRounding.ToEven,
                culture,
                TimeUnit.Second));
        Assert.Equal(
            "2147483647 seconds",
            TimeSpan.MaxValue.HumanizeWithFractionalSeconds(
                1,
                0,
                MidpointRounding.ToEven,
                culture,
                TimeUnit.Second));

        var exception = Assert.Throws<InvalidOperationException>(
            () => TimeSpan.FromSeconds(1.5).HumanizeWithFractionalSeconds(
                1,
                1,
                MidpointRounding.ToEven,
                culture,
                TimeUnit.Second));

        Assert.Contains(nameof(IFractionalTimeSpanFormatter), exception.Message);
    }

    [Fact]
    public void RegisteredFractionalFormatterReceivesSecondsAndSymbolMode()
    {
        var formatter = ModuleInitializer.FractionalFormatter;
        formatter.Calls.Clear();
        var culture = new CultureInfo("en-001");
        var timeSpan = TimeSpan.FromTicks(12345678);

        Assert.Equal(
            "captured",
            timeSpan.HumanizeWithFractionalSeconds(
                1,
                7,
                MidpointRounding.ToEven,
                culture,
                TimeUnit.Second));
        Assert.Equal(
            "captured",
            timeSpan.HumanizeToSymbolsWithFractionalSeconds(
                1,
                7,
                MidpointRounding.ToEven,
                culture,
                TimeUnit.Second));

        Assert.Equal(
            [(1.2345678m, false), (1.2345678m, true)],
            formatter.Calls);
    }

    [Fact]
    public void LegacyStrategyHandlesIntegralRequestButRejectsFractionalTerminal()
    {
        var originalStrategy = Configurator.TimeSpanHumanizeStrategy;
        var strategy = new LegacyStrategy();

        try
        {
            Configurator.TimeSpanHumanizeStrategy = strategy;

            Assert.Equal(
                "legacy",
                TimeSpan.FromSeconds(1.6).HumanizeWithFractionalSeconds(
                    1,
                    0,
                    MidpointRounding.ToEven,
                    English,
                    TimeUnit.Second));
            Assert.Equal(TimeSpan.FromSeconds(2), strategy.LastTimeSpan);

            var exception = Assert.Throws<InvalidOperationException>(
                () => TimeSpan.FromSeconds(1.5).HumanizeWithFractionalSeconds(
                    1,
                    1,
                    MidpointRounding.ToEven,
                    English,
                    TimeUnit.Second));
            Assert.Contains(nameof(IFractionalTimeSpanHumanizeStrategy), exception.Message);
        }
        finally
        {
            Configurator.TimeSpanHumanizeStrategy = originalStrategy;
        }
    }

    [Fact]
    public void LegacyStrategyReceivesSaturatedRoundedExtremesWhenSecondsAreHidden()
    {
        var originalStrategy = Configurator.TimeSpanHumanizeStrategy;
        var strategy = new LegacyStrategy();

        try
        {
            Configurator.TimeSpanHumanizeStrategy = strategy;

            TimeSpan.MaxValue.HumanizeWithFractionalSeconds(
                1,
                1,
                MidpointRounding.ToEven,
                English,
                TimeUnit.Week);
            Assert.Equal(TimeSpan.MaxValue, strategy.LastTimeSpan);

            TimeSpan.MinValue.HumanizeWithFractionalSeconds(
                1,
                1,
                MidpointRounding.ToEven,
                English,
                TimeUnit.Week);
            Assert.Equal(TimeSpan.MinValue, strategy.LastTimeSpan);
        }
        finally
        {
            Configurator.TimeSpanHumanizeStrategy = originalStrategy;
        }
    }

    [Theory]
    [InlineData(-1, MidpointRounding.ToEven, TimeUnit.Second, "maxFractionalDigits")]
    [InlineData(8, MidpointRounding.ToEven, TimeUnit.Second, "maxFractionalDigits")]
    [InlineData(3, MidpointRounding.ToZero, TimeUnit.Second, "roundingMode")]
    [InlineData(3, MidpointRounding.ToEven, TimeUnit.Millisecond, "maxUnit")]
    public void DirectDefaultStrategyCallsValidateFractionalArguments(
        int maxFractionalDigits,
        MidpointRounding roundingMode,
        TimeUnit maxUnit,
        string parameterName)
    {
        var strategy = new DefaultTimeSpanHumanizeStrategy();

        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => strategy.HumanizeWithFractionalSeconds(
                TimeSpan.Zero,
                1,
                false,
                English,
                maxUnit,
                ", ",
                maxFractionalDigits,
                roundingMode,
                false));

        Assert.Equal(parameterName, exception.ParamName);
    }

    [Fact]
    public void FractionalStrategyReceivesTheCompleteRequest()
    {
        var originalStrategy = Configurator.TimeSpanHumanizeStrategy;
        var strategy = new FractionalStrategy();
        var culture = new CultureInfo("fr-FR");
        var timeSpan = TimeSpan.FromTicks(123456789);

        try
        {
            Configurator.TimeSpanHumanizeStrategy = strategy;

            var actual = timeSpan.HumanizeToSymbolsWithFractionalSeconds(
                4,
                true,
                6,
                MidpointRounding.AwayFromZero,
                culture,
                TimeUnit.Month,
                null);

            Assert.Equal("fractional", actual);
            Assert.Equal(
                new(
                    timeSpan,
                    4,
                    true,
                    culture,
                    TimeUnit.Month,
                    null,
                    6,
                    MidpointRounding.AwayFromZero,
                    true),
                strategy.LastCall);
        }
        finally
        {
            Configurator.TimeSpanHumanizeStrategy = originalStrategy;
        }
    }

    [Fact]
    public void LegacyCallsRemainUnambiguousAndUnchanged()
    {
        Assert.Equal(string.Empty, default(TimeSpan).Humanize(default));
        Assert.Equal("1 second", TimeSpan.FromSeconds(1).Humanize(1, null));
        Assert.Equal("1s", TimeSpan.FromSeconds(1).HumanizeToSymbols(1, null));
    }

    sealed class LegacyStrategy : ITimeSpanHumanizeStrategy
    {
        public TimeSpan LastTimeSpan { get; private set; }

        public string Humanize(
            TimeSpan timeSpan,
            int precision,
            bool countEmptyUnits,
            CultureInfo? culture,
            TimeUnit maxUnit,
            TimeUnit minUnit,
            string? collectionSeparator,
            bool toWords,
            bool toSymbols)
        {
            LastTimeSpan = timeSpan;
            return "legacy";
        }
    }

    sealed class FractionalStrategy : ITimeSpanHumanizeStrategy, IFractionalTimeSpanHumanizeStrategy
    {
        public FractionalCall? LastCall { get; private set; }

        public string Humanize(
            TimeSpan timeSpan,
            int precision,
            bool countEmptyUnits,
            CultureInfo? culture,
            TimeUnit maxUnit,
            TimeUnit minUnit,
            string? collectionSeparator,
            bool toWords,
            bool toSymbols) =>
            throw new NotSupportedException();

        public string HumanizeWithFractionalSeconds(
            TimeSpan timeSpan,
            int precision,
            bool countEmptyUnits,
            CultureInfo? culture,
            TimeUnit maxUnit,
            string? collectionSeparator,
            int maxFractionalDigits,
            MidpointRounding roundingMode,
            bool toSymbols)
        {
            LastCall = new(
                timeSpan,
                precision,
                countEmptyUnits,
                culture,
                maxUnit,
                collectionSeparator,
                maxFractionalDigits,
                roundingMode,
                toSymbols);
            return "fractional";
        }
    }

    sealed record FractionalCall(
        TimeSpan TimeSpan,
        int Precision,
        bool CountEmptyUnits,
        CultureInfo? Culture,
        TimeUnit MaxUnit,
        string? CollectionSeparator,
        int MaxFractionalDigits,
        MidpointRounding RoundingMode,
        bool ToSymbols);
}