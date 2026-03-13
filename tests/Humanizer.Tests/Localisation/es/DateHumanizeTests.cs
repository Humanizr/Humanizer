namespace es;

[UseCulture("es-ES")]
public class DateHumanizeTests
{
    [Theory]
    [InlineData(1, "hace un segundo")]
    [InlineData(2, "hace 2 segundos")]
    public void SecondsAgo(int seconds, string expected) =>
        DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);

    [Theory]
    [InlineData(1, "en un segundo")]
    [InlineData(2, "en 2 segundos")]
    public void SecondsFromNow(int seconds, string expected) =>
        DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);

    [Theory]
    [InlineData(1, "hace un minuto")]
    [InlineData(2, "hace 2 minutos")]
    [InlineData(60, "hace una hora")]
    public void MinutesAgo(int minutes, string expected) =>
        DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);

    [Theory]
    [InlineData(1, "en un minuto")]
    [InlineData(2, "en 2 minutos")]
    public void MinutesFromNow(int minutes, string expected) =>
        DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);

    [Theory]
    [InlineData(1, "hace una hora")]
    [InlineData(2, "hace 2 horas")]
    public void HoursAgo(int hours, string expected) =>
        DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);

    [Theory]
    [InlineData(1, "en una hora")]
    [InlineData(2, "en 2 horas")]
    public void HoursFromNow(int hours, string expected) =>
        DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);

    [Theory]
    [InlineData(1, "ayer")]
    [InlineData(2, "hace 2 días")]
    public void DaysAgo(int days, string expected) =>
        DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);

    [Fact]
    public void TwoDaysAgoResourceExists()
    {
        Assert.True(Resources.TryGetResource("DateHumanize_TwoDaysAgo", new("es-ES"), out var value));
        Assert.Equal("anteayer", value);
    }

    [Theory]
    [InlineData(1, "mañana")]
    [InlineData(2, "en 2 días")]
    public void DaysFromNow(int days, string expected) =>
        DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);

    [Fact]
    public void TwoDaysFromNowResourceExists()
    {
        Assert.True(Resources.TryGetResource("DateHumanize_TwoDaysFromNow", new("es-ES"), out var value));
        Assert.Equal("pasado mañana", value);
    }

    [Fact]
    public void DayPaucalResourcesExist()
    {
        Assert.True(Resources.TryGetResource("DateHumanize_MultipleDaysAgo_Paucal", new("es-ES"), out var past));
        Assert.True(Resources.TryGetResource("DateHumanize_MultipleDaysFromNow_Paucal", new("es-ES"), out var future));

        Assert.Equal("hace {0} días", past);
        Assert.Equal("en {0} días", future);
    }

    [Theory]
    [InlineData(1, "hace un mes")]
    [InlineData(2, "hace 2 meses")]
    public void MonthsAgo(int months, string expected) =>
        DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);

    [Theory]
    [InlineData(1, "en un mes")]
    [InlineData(2, "en 2 meses")]
    public void MonthsFromNow(int months, string expected) =>
        DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);

    [Theory]
    [InlineData(1, "hace un año")]
    [InlineData(2, "hace 2 años")]
    public void YearsAgo(int years, string expected) =>
        DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);

    [Theory]
    [InlineData(1, "en un año")]
    [InlineData(2, "en 2 años")]
    public void YearsFromNow(int years, string expected) =>
        DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
}
