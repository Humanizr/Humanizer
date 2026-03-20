#if NET6_0_OR_GREATER

namespace ja;

[UseCulture("ja-JP")]
public class FallbackRecoveryTests
{
    [Fact]
    public void DateTimeFallbackUsesCultureShortDate() =>
        Assert.Equal(new DateTime(2015, 1, 1).ToString("d", CultureInfo.CurrentCulture), new DateTime(2015, 1, 1).ToOrdinalWords());

    [Fact]
    public void DateOnlyFallbackUsesCultureShortDate() =>
        Assert.Equal(new DateOnly(2015, 1, 1).ToString("d", CultureInfo.CurrentCulture), new DateOnly(2015, 1, 1).ToOrdinalWords());

    [Fact]
    public void TimeOnlyFallbackUsesCultureShortTime() =>
        Assert.Equal(new TimeOnly(13, 23).ToString("t", CultureInfo.CurrentCulture), new TimeOnly(13, 23).ToClockNotation());
}

#endif
