namespace ca;

[UseCulture("ca")]
public class DateHumanizeTests
{
    [Theory]
    [InlineData(1, "fa un segon")]
    [InlineData(2, "fa 2 segons")]
    public void SecondsAgo(int seconds, string expected) =>
        DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);

    [Theory]
    [InlineData(1, "d'aquí un segon")]
    [InlineData(2, "d'aquí 2 segons")]
    public void SecondsFromNow(int seconds, string expected) =>
        DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);

    [Theory]
    [InlineData(1, "fa un minut")]
    [InlineData(2, "fa 2 minuts")]
    [InlineData(60, "fa una hora")]
    public void MinutesAgo(int minutes, string expected) =>
        DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);

    [Theory]
    [InlineData(1, "d'aquí un minut")]
    [InlineData(2, "d'aquí 2 minuts")]
    public void MinutesFromNow(int minutes, string expected) =>
        DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);

    [Theory]
    [InlineData(1, "fa una hora")]
    [InlineData(2, "fa 2 hores")]
    public void HoursAgo(int hours, string expected) =>
        DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);

    [Theory]
    [InlineData(1, "d'aquí una hora")]
    [InlineData(2, "d'aquí 2 hores")]
    public void HoursFromNow(int hours, string expected) =>
        DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);

    [Theory]
    [InlineData(1, "ahir")]
    [InlineData(2, "fa 2 dies")]
    public void DaysAgo(int days, string expected) =>
        DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);

    [Theory]
    [InlineData(1, "demà")]
    [InlineData(2, "d'aquí 2 dies")]
    public void DaysFromNow(int days, string expected) =>
        DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);

    [Theory]
    [InlineData(1, "fa un mes")]
    [InlineData(2, "fa 2 mesos")]
    public void MonthsAgo(int months, string expected) =>
        DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);

    [Theory]
    [InlineData(1, "d'aquí un mes")]
    [InlineData(2, "d'aquí 2 mesos")]
    public void MonthsFromNow(int months, string expected) =>
        DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);

    [Theory]
    [InlineData(1, "fa un any")]
    [InlineData(2, "fa 2 anys")]
    public void YearsAgo(int years, string expected) =>
        DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);

    [Theory]
    [InlineData(1, "d'aquí un any")]
    [InlineData(2, "d'aquí 2 anys")]
    public void YearsFromNow(int years, string expected) =>
        DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
}