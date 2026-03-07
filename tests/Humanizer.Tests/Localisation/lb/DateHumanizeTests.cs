namespace lb;

[UseCulture("lb-LU")]
public class DateHumanizeTests
{
    [Theory]
    [InlineData(1, "virun enger Sekonn")]
    [InlineData(2, "virun 2 Sekonnen")]
    [InlineData(4, "viru 4 Sekonnen")]
    [InlineData(10, "virun 10 Sekonnen")]
    public void SecondsAgo(int seconds, string expected) =>
        DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);

    [Theory]
    [InlineData(1, "an enger Sekonn")]
    [InlineData(2, "an 2 Sekonnen")]
    [InlineData(4, "a 4 Sekonnen")]
    [InlineData(10, "an 10 Sekonnen")]
    public void SecondsFromNow(int seconds, string expected) =>
        DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);

    [Theory]
    [InlineData(1, "virun enger Minutt")]
    [InlineData(2, "virun 2 Minutten")]
    [InlineData(4, "viru 4 Minutten")]
    [InlineData(10, "virun 10 Minutten")]
    [InlineData(60, "virun enger Stonn")]
    [InlineData(240, "viru 4 Stonnen")]
    public void MinutesAgo(int minutes, string expected) =>
        DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);

    [Theory]
    [InlineData(1, "an enger Minutt")]
    [InlineData(2, "an 2 Minutten")]
    [InlineData(4, "a 4 Minutten")]
    [InlineData(10, "an 10 Minutten")]
    public void MinutesFromNow(int minutes, string expected) =>
        DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);

    [Theory]
    [InlineData(1, "virun enger Stonn")]
    [InlineData(2, "virun 2 Stonnen")]
    [InlineData(4, "viru 4 Stonnen")]
    [InlineData(10, "virun 10 Stonnen")]
    public void HoursAgo(int hours, string expected) =>
        DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);

    [Theory]
    [InlineData(1, "an enger Stonn")]
    [InlineData(2, "an 2 Stonnen")]
    [InlineData(4, "a 4 Stonnen")]
    [InlineData(10, "an 10 Stonnen")]
    public void HoursFromNow(int hours, string expected) =>
        DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);

    [Theory]
    [InlineData(1, "gëschter")]
    [InlineData(2, "virgëschter")]
    [InlineData(4, "viru 4 Deeg")]
    [InlineData(10, "virun 10 Deeg")]
    public void DaysAgo(int days, string expected) =>
        DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);

    [Theory]
    [InlineData(1, "muer")]
    [InlineData(2, "iwwermuer")]
    [InlineData(4, "a 4 Deeg")]
    [InlineData(10, "an 10 Deeg")]
    public void DaysFromNow(int days, string expected) =>
        DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);

    [Theory]
    [InlineData(1, "virun engem Mount")]
    [InlineData(2, "virun 2 Méint")]
    [InlineData(4, "viru 4 Méint")]
    [InlineData(10, "virun 10 Méint")]
    public void MonthsAgo(int months, string expected) =>
        DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);

    [Theory]
    [InlineData(1, "an engem Mount")]
    [InlineData(2, "an 2 Méint")]
    [InlineData(4, "a 4 Méint")]
    [InlineData(10, "an 10 Méint")]
    public void MonthsFromNow(int months, string expected) =>
        DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);

    [Theory]
    [InlineData(1, "virun engem Joer")]
    [InlineData(2, "virun 2 Joer")]
    [InlineData(4, "viru 4 Joer")]
    public void YearsAgo(int years, string expected) =>
        DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);

    [Theory]
    [InlineData(1, "an engem Joer")]
    [InlineData(2, "an 2 Joer")]
    [InlineData(4, "a 4 Joer")]
    public void YearsFromNow(int years, string expected) =>
        DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
}
