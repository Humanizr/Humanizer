#if NET6_0_OR_GREATER

namespace ptBR;

[UseCulture("pt-BR")]
public class FallbackRecoveryTests
{
    [Fact]
    public void DateTimeUsesExactLocaleConverter() =>
        Assert.Equal("1º de janeiro de 2015", new DateTime(2015, 1, 1).ToOrdinalWords());

    [Fact]
    public void DateOnlyUsesExactLocaleConverter() =>
        Assert.Equal("1º de janeiro de 2015", new DateOnly(2015, 1, 1).ToOrdinalWords());

    [Fact]
    public void TimeOnlyUsesExactLocaleConverter() =>
        Assert.Equal("uma e vinte e três", new TimeOnly(13, 23).ToClockNotation());
}

#endif
