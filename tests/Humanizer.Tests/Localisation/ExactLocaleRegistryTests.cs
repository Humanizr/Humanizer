#if NET6_0_OR_GREATER
namespace Humanizer.Tests.Localisation;

public class ExactLocaleRegistryTests
{
    [Fact]
    [UseCulture("pt-BR")]
    public void Collection_formatter_resolves_exact_locale() =>
        Assert.Equal("1 e 2", new[] { 1, 2 }.Humanize());

    [Fact]
    [UseCulture("zh-Hans")]
    public void Collection_formatter_resolves_script_specific_locale() =>
        Assert.Equal("1 和 2", new[] { 1, 2 }.Humanize());

    [Fact]
    [UseCulture("ja-JP")]
    public void Default_time_only_clock_notation_is_culture_formatted_for_non_english_locales() =>
        Assert.Equal(new TimeOnly(15, 45).ToString("t"), new TimeOnly(15, 45).ToClockNotation());

    [Fact]
    [UseCulture("ja-JP")]
    public void Default_date_to_ordinal_words_is_culture_formatted_for_non_english_locales() =>
        Assert.Equal(new DateTime(2015, 1, 1).ToString("d"), new DateTime(2015, 1, 1).ToOrdinalWords());

    [Fact]
    [UseCulture("ja-JP")]
    public void Default_dateonly_to_ordinal_words_is_culture_formatted_for_non_english_locales() =>
        Assert.Equal(new DateOnly(2015, 1, 1).ToString("d"), new DateOnly(2015, 1, 1).ToOrdinalWords());

    [Fact]
    [UseCulture("ar")]
    public void Default_date_fallback_strips_directional_mark_artifacts()
    {
        var result = new DateTime(2015, 1, 1).ToOrdinalWords();
        Assert.DoesNotContain('\u200e', result);
        Assert.DoesNotContain('\u200f', result);
        Assert.DoesNotContain('\u061c', result);
    }

    [Fact]
    [UseCulture("pt-BR")]
    public void Ordinalizer_resolves_exact_locale() =>
        Assert.Equal("1º", 1.Ordinalize(GrammaticalGender.Masculine));

    [Fact]
    [UseCulture("pt-BR")]
    public void Date_to_ordinal_words_resolves_exact_locale() =>
        Assert.Equal("1º de janeiro de 2015", new DateTime(2015, 1, 1).ToOrdinalWords());

    [Fact]
    [UseCulture("pt-BR")]
    public void DateOnly_to_ordinal_words_resolves_exact_locale() =>
        Assert.Equal("1º de janeiro de 2015", new DateOnly(2015, 1, 1).ToOrdinalWords());

    [Fact]
    [UseCulture("pt-BR")]
    public void TimeOnly_clock_notation_resolves_exact_locale() =>
        Assert.Equal("meia-noite", new TimeOnly(0, 0).ToClockNotation());

    [Fact]
    public void Words_to_number_round_trips_supported_exact_locale()
    {
        var culture = new CultureInfo("pt-BR");
        var words = 99.ToWords(culture);

        Assert.True(words.TryToNumber(out var parsedNumber, culture));
        Assert.Equal(99, parsedNumber);
    }

    [Fact]
    public void Words_to_number_does_not_fall_back_to_english_for_unsupported_exact_locale()
    {
        var culture = new CultureInfo("ga-IE");

        Assert.Throws<NotSupportedException>(() => "one".ToNumber(culture));
    }
}
#endif
