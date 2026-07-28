namespace Humanizer.Tests.Localisation;

static class RtlBidiControlSweep
{
    static readonly char[] BidiControls =
    [
        '\u061C',
        '\u200E',
        '\u200F',
        '\u202A',
        '\u202B',
        '\u202C',
        '\u202D',
        '\u202E',
        '\u2066',
        '\u2067',
        '\u2068',
        '\u2069'
    ];

    public static void AssertRawLocalizedOutputsDoNotContainBidiControls()
    {
        var culture = CultureInfo.CurrentCulture;
        var formatter = Configurator.Formatters.ResolveForCulture(culture);
        var outputs = new List<(string Surface, string Value)>
        {
            ("cardinal", 1234567.ToWords(culture)),
            ("ordinal words", 21.ToOrdinalWords(GrammaticalGender.Masculine, culture)),
            ("numeric ordinal", 21.Ordinalize(culture)),
            ("date", new DateTime(2024, 12, 31).ToOrdinalWords()),
            ("relative date", formatter.DateHumanize(TimeUnit.Day, Tense.Past, 2)),
            ("duration", TimeSpan.FromDays(2).Humanize(culture: culture)),
            ("quantity", "request".ToQuantity(2, format: null, formatProvider: culture)),
            ("data quantity", ByteSize.FromBytes(2).ToFullWords(provider: culture)),
            ("time unit", formatter.TimeUnitHumanize(TimeUnit.Hour))
        };

#if NET6_0_OR_GREATER
        outputs.Add(("clock", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.None, culture)));
#endif

        foreach (var (surface, value) in outputs)
            AssertNoBidiControls(surface, value);
    }

    public static void AssertNoBidiControls(string surface, string value)
    {
        Assert.NotEmpty(value);
        foreach (var bidiControl in BidiControls)
        {
            Assert.False(
                value.Contains(bidiControl),
                $"{CultureInfo.CurrentCulture.Name} {surface} contains U+{(int)bidiControl:X4}: {value}");
        }
    }
}