namespace Humanizer.Tests.Localisation.Default;

[UseCulture("eo")]
public class RegistryFallbackTests
{
    [Fact]
    public void DateTimeToOrdinalWords_UnregisteredCulture_UsesDefaultConverter()
    {
        var date = new DateTime(2024, 3, 15);
        var result = date.ToOrdinalWords();

        var expected = date.ToString("d", CultureInfo.GetCultureInfo("eo"));
        Assert.Equal(expected, result);
    }

    [Fact]
    public void DateTimeToOrdinalWords_GrammaticalCase_UnregisteredCulture_UsesDefaultConverter()
    {
        var date = new DateTime(2024, 3, 15);
        var result = date.ToOrdinalWords(GrammaticalCase.Genitive);

        var expected = date.ToString("d", CultureInfo.GetCultureInfo("eo"));
        Assert.Equal(expected, result);
    }

#if NET6_0_OR_GREATER
    [Fact]
    public void DateOnlyToOrdinalWords_UnregisteredCulture_UsesDefaultConverter()
    {
        var date = new DateOnly(2024, 3, 15);
        var result = date.ToOrdinalWords();

        var expected = date.ToString("d", CultureInfo.GetCultureInfo("eo"));
        Assert.Equal(expected, result);
    }

    [Fact]
    public void DateOnlyToOrdinalWords_GrammaticalCase_UnregisteredCulture_UsesDefaultConverter()
    {
        var date = new DateOnly(2024, 3, 15);
        var result = date.ToOrdinalWords(GrammaticalCase.Genitive);

        var expected = date.ToString("d", CultureInfo.GetCultureInfo("eo"));
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TimeOnlyToClockNotation_UnregisteredCulture_UsesEnglishProfile()
    {
        var time = new TimeOnly(13, 23);
        var result = time.ToClockNotation();

        Assert.Equal("one twenty-three", result);
    }
#endif
}
