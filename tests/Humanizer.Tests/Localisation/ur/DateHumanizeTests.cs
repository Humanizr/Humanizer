namespace ur;

[UseCulture("ur")]
public class DateHumanizeTests
{
    [Theory]
    [InlineData(-1, "کل")]
    [InlineData(-2, "2 دن پہلے")]
    [InlineData(-3, "3 دن پہلے")]
    [InlineData(-11, "11 دن پہلے")]
    public void DaysAgo(int days, string expected) =>
        DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);

    [Theory]
    [InlineData(1, "کل")]
    [InlineData(2, "2 دن بعد")]
    [InlineData(10, "10 دن بعد")]
    public void DaysFromNow(int days, string expected) =>
        DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);

    [Theory]
    [InlineData(-1, "ایک گھنٹہ پہلے")]
    [InlineData(-2, "2 گھنٹے پہلے")]
    [InlineData(-3, "3 گھنٹے پہلے")]
    [InlineData(-11, "11 گھنٹے پہلے")]
    public void HoursAgo(int hours, string expected) =>
        DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Past);

    [Theory]
    [InlineData(1, "ایک گھنٹے بعد")]
    [InlineData(2, "2 گھنٹے بعد")]
    [InlineData(10, "10 گھنٹے بعد")]
    public void HoursFromNow(int hours, string expected) =>
        DateHumanize.Verify(expected, hours, TimeUnit.Hour, Tense.Future);

    [Theory]
    [InlineData(-1, "ایک منٹ پہلے")]
    [InlineData(-2, "2 منٹ پہلے")]
    [InlineData(-3, "3 منٹ پہلے")]
    [InlineData(-11, "11 منٹ پہلے")]
    [InlineData(60, "ایک گھنٹہ پہلے")]
    public void MinutesAgo(int minutes, string expected) =>
        DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Past);

    [Theory]
    [InlineData(1, "ایک منٹ بعد")]
    [InlineData(2, "2 منٹ بعد")]
    [InlineData(10, "10 منٹ بعد")]
    public void MinutesFromNow(int minutes, string expected) =>
        DateHumanize.Verify(expected, minutes, TimeUnit.Minute, Tense.Future);

    [Theory]
    [InlineData(-1, "ایک ماہ پہلے")]
    [InlineData(-2, "2 ماہ پہلے")]
    [InlineData(-3, "3 ماہ پہلے")]
    [InlineData(-11, "11 ماہ پہلے")]
    public void MonthsAgo(int months, string expected) =>
        DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Past);

    [Theory]
    [InlineData(1, "ایک ماہ بعد")]
    [InlineData(2, "2 ماہ بعد")]
    [InlineData(10, "10 ماہ بعد")]
    public void MonthsFromNow(int months, string expected) =>
        DateHumanize.Verify(expected, months, TimeUnit.Month, Tense.Future);

    [Theory]
    [InlineData(-1, "ایک سیکنڈ پہلے")]
    [InlineData(-2, "2 سیکنڈ پہلے")]
    [InlineData(-3, "3 سیکنڈ پہلے")]
    [InlineData(-11, "11 سیکنڈ پہلے")]
    public void SecondsAgo(int seconds, string expected) =>
        DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);

    [Theory]
    [InlineData(0, "ابھی")]
    [InlineData(1, "ایک سیکنڈ بعد")]
    [InlineData(2, "2 سیکنڈ بعد")]
    [InlineData(10, "10 سیکنڈ بعد")]
    public void SecondsFromNow(int seconds, string expected) =>
        DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Future);

    [Theory]
    [InlineData(-1, "ایک سال پہلے")]
    [InlineData(-2, "2 سال پہلے")]
    [InlineData(-3, "3 سال پہلے")]
    [InlineData(-11, "11 سال پہلے")]
    public void YearsAgo(int years, string expected) =>
        DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Past);

    [Theory]
    [InlineData(1, "ایک سال بعد")]
    [InlineData(2, "2 سال بعد")]
    [InlineData(7, "7 سال بعد")]
    public void YearsFromNow(int years, string expected) =>
        DateHumanize.Verify(expected, years, TimeUnit.Year, Tense.Future);
}
