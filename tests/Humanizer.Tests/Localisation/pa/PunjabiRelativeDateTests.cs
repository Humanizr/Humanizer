namespace Humanizer.Tests.Localisation.pa;

[UseCulture("pa")]
public class PunjabiRelativeDateTests
{
    [Theory]
    [InlineData(1, TimeUnit.Second, Tense.Past, "ਇੱਕ ਸਕਿੰਟ ਪਹਿਲਾਂ")]
    [InlineData(2, TimeUnit.Second, Tense.Future, "2 ਸਕਿੰਟ ਵਿੱਚ")]
    [InlineData(1, TimeUnit.Minute, Tense.Past, "ਇੱਕ ਮਿੰਟ ਪਹਿਲਾਂ")]
    [InlineData(2, TimeUnit.Minute, Tense.Future, "2 ਮਿੰਟ ਵਿੱਚ")]
    [InlineData(1, TimeUnit.Hour, Tense.Past, "ਇੱਕ ਘੰਟਾ ਪਹਿਲਾਂ")]
    [InlineData(2, TimeUnit.Hour, Tense.Future, "2 ਘੰਟੇ ਵਿੱਚ")]
    [InlineData(1, TimeUnit.Day, Tense.Past, "ਕੱਲ੍ਹ")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "ਕੱਲ੍ਹ")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 ਦਿਨ ਪਹਿਲਾਂ")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "2 ਦਿਨ ਵਿੱਚ")]
    [InlineData(1, TimeUnit.Month, Tense.Past, "ਇੱਕ ਮਹੀਨਾ ਪਹਿਲਾਂ")]
    [InlineData(2, TimeUnit.Month, Tense.Future, "2 ਮਹੀਨਿਆਂ ਵਿੱਚ")]
    [InlineData(1, TimeUnit.Year, Tense.Past, "ਇੱਕ ਸਾਲ ਪਹਿਲਾਂ")]
    [InlineData(2, TimeUnit.Year, Tense.Future, "2 ਸਾਲਾਂ ਵਿੱਚ")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "ਹੁਣੇ")]
    public void DateHumanize_SingularAndPluralPerUnit(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(new CultureInfo("pa"));
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_NullDateUsesPunjabiNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("ਕਦੇ ਨਹੀਂ", date.Humanize(culture: new CultureInfo("pa")));
    }
}