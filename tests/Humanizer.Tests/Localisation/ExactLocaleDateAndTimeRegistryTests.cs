#if NET6_0_OR_GREATER
namespace Humanizer.Tests.Localisation;

public class ExactLocaleDateAndTimeRegistryTests
{
    [Fact]
    [UseCulture("ja-JP")]
    public void Ja_Jp_uses_owned_ordinal_date_pattern() =>
        Assert.Equal("2015年1月1日", new DateTime(2015, 1, 1).ToOrdinalWords());

    [Fact]
    [UseCulture("ja-JP")]
    public void Ja_Jp_uses_owned_dateonly_ordinal_date_pattern() =>
        Assert.Equal("2015年1月1日", new DateOnly(2015, 1, 1).ToOrdinalWords());

    [Fact]
    [UseCulture("ja-JP")]
    public void Ja_Jp_uses_owned_clock_notation() =>
        Assert.Equal("15時45分", new TimeOnly(15, 45).ToClockNotation());

    [Fact]
    [UseCulture("ja-JP")]
    public void Ja_Jp_uses_owned_rounded_clock_notation() =>
        Assert.Equal("13時25分", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));

    [Fact]
    [UseCulture("ar")]
    public void Ar_uses_owned_ordinal_date_pattern_without_bidi_marks()
    {
        var result = new DateTime(2015, 1, 1).ToOrdinalWords();

        Assert.Equal("1 يناير 2015", result);
        Assert.DoesNotContain('\u200e', result);
        Assert.DoesNotContain('\u200f', result);
        Assert.DoesNotContain('\u061c', result);
    }

    [Fact]
    [UseCulture("pt-BR")]
    public void Date_to_ordinal_words_resolves_exact_locale() =>
        Assert.Equal("1º de janeiro de 2015", new DateTime(2015, 1, 1).ToOrdinalWords());

    [Fact]
    [UseCulture("pt-BR")]
    public void Dateonly_to_ordinal_words_resolves_exact_locale() =>
        Assert.Equal("1º de janeiro de 2015", new DateOnly(2015, 1, 1).ToOrdinalWords());

    [Fact]
    [UseCulture("pt-BR")]
    public void Timeonly_clock_notation_resolves_exact_locale() =>
        Assert.Equal("meia-noite", new TimeOnly(0, 0).ToClockNotation());
}
#endif
