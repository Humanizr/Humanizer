namespace pt;

[UseCulture("pt")]
public class DateHumanizeTests
{
    [Theory]
    [InlineData(-2, "há 2 segundos")]
    [InlineData(-1, "há um segundo")]
    public void SecondsAgo(int seconds, string expected) =>
        DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);

    [Theory]
    [InlineData(1, "daqui a um segundo")]
    [InlineData(2, "daqui a 2 segundos")]
    public void SecondsFromNow(int seconds, string expected) =>
        DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);

    [Theory]
    [InlineData(-2, "há 2 minutos")]
    [InlineData(-1, "há um minuto")]
    [InlineData(60, "há uma hora")]
    public void MinutesAgo(int minutes, string expected) =>
        DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);

    [Theory]
    [InlineData(1, "daqui a um minuto")]
    [InlineData(2, "daqui a 2 minutos")]
    public void MinutesFromNow(int minutes, string expected) =>
        DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);

    [Theory]
    [InlineData(-2, "há 2 horas")]
    [InlineData(-1, "há uma hora")]
    public void HoursAgo(int hours, string expected) =>
        DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);

    [Theory]
    [InlineData(1, "daqui a uma hora")]
    [InlineData(2, "daqui a 2 horas")]
    public void HoursFromNow(int hours, string expected) =>
        DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);

    [Theory]
    [InlineData(-2, "há 2 dias")]
    [InlineData(-1, "ontem")]
    public void DaysAgo(int days, string expected) =>
        DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);

    [Theory]
    [InlineData(1, "amanhã")]
    [InlineData(2, "daqui a 2 dias")]
    public void DaysFromNow(int days, string expected) =>
        DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);

    [Theory]
    [InlineData(-2, "há 2 meses")]
    [InlineData(-1, "há um mês")]
    public void MonthsAgo(int months, string expected) =>
        DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);

    [Theory]
    [InlineData(1, "daqui a um mês")]
    [InlineData(2, "daqui a 2 meses")]
    public void MonthsFromNow(int months, string expected) =>
        DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);

    [Theory]
    [InlineData(-2, "há 2 anos")]
    [InlineData(-1, "há um ano")]
    public void YearsAgo(int years, string expected) =>
        DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);

    [Theory]
    [InlineData(1, "daqui a um ano")]
    [InlineData(2, "daqui a 2 anos")]
    public void YearsFromNow(int years, string expected) =>
        DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);

    [Fact]
    public void Now() =>
        DateHumanize.Verify("agora", 0, TimeUnit.Day, Tense.Future);
}