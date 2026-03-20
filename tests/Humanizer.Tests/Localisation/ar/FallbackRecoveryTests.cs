#if NET6_0_OR_GREATER

namespace ar;

[UseCulture("ar")]
public class FallbackRecoveryTests
{
    [Fact]
    public void DateTimeFallbackStripsBidiMarks()
    {
        var date = new DateTime(2015, 1, 1);
        var raw = date.ToString("d", CultureInfo.CurrentCulture);

        Assert.Equal(Sanitize(raw), date.ToOrdinalWords());
    }

    [Fact]
    public void DateOnlyFallbackStripsBidiMarks()
    {
        var date = new DateOnly(2015, 1, 1);
        var raw = date.ToString("d", CultureInfo.CurrentCulture);

        Assert.Equal(Sanitize(raw), date.ToOrdinalWords());
    }

    static string Sanitize(string value) =>
        value.Replace("\u200e", string.Empty)
            .Replace("\u200f", string.Empty)
            .Replace("\u061c", string.Empty);
}

#endif
