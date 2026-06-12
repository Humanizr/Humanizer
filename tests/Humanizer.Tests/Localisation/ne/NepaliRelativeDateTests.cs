namespace Humanizer.Tests.Localisation.ne;

[UseCulture("ne")]
public class NepaliRelativeDateTests
{
    [Theory]
    [InlineData(1, TimeUnit.Second, Tense.Past, "एक सेकेन्ड अघि")]
    [InlineData(2, TimeUnit.Second, Tense.Future, "2 सेकेन्ड पछि")]
    [InlineData(1, TimeUnit.Minute, Tense.Past, "एक मिनेट अघि")]
    [InlineData(2, TimeUnit.Minute, Tense.Future, "2 मिनेट पछि")]
    [InlineData(1, TimeUnit.Hour, Tense.Past, "एक घण्टा अघि")]
    [InlineData(2, TimeUnit.Hour, Tense.Future, "2 घण्टा पछि")]
    [InlineData(1, TimeUnit.Day, Tense.Past, "हिजो")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "भोलि")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 दिन अघि")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "2 दिन पछि")]
    [InlineData(1, TimeUnit.Month, Tense.Past, "एक महिना अघि")]
    [InlineData(2, TimeUnit.Month, Tense.Future, "2 महिना पछि")]
    [InlineData(1, TimeUnit.Year, Tense.Past, "एक वर्ष अघि")]
    [InlineData(2, TimeUnit.Year, Tense.Future, "2 वर्ष पछि")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "अहिले")]
    public void DateHumanize_SingularAndPluralPerUnit(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("ne"));
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_NullDateUsesNepaliNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("कहिल्यै होइन", date.Humanize(culture: new CultureInfo("ne")));
    }
}