#if NET6_0_OR_GREATER

namespace en;

public class FallbackRecoveryTests
{
    [UseCulture("en-GB")]
    [Fact]
    public void DateOnlyFallbackKeepsEnglishOrdinalWords() =>
        Assert.Equal("1st January 2015", new DateOnly(2015, 1, 1).ToOrdinalWords());

    [UseCulture("en-GB")]
    [Fact]
    public void TimeOnlyFallbackKeepsEnglishClockNotation() =>
        Assert.Equal("twenty-five past one", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
}

#endif
