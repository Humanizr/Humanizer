namespace filPH;

[UseCulture("fil-PH")]
public class DateHumanizeTests
{
    [Theory]
    [InlineData(2, "2 araw ang nakalipas")]
    [InlineData(3, "3 araw ang nakalipas")]
    public void DaysAgo(int days, string expected) =>
        DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Past);

    [Theory]
    [InlineData(2, "2 araw mula ngayon")]
    [InlineData(3, "3 araw mula ngayon")]
    public void DaysFromNow(int days, string expected) =>
        DateHumanize.Verify(expected, days, TimeUnit.Day, Tense.Future);

    [Fact]
    public void Now() =>
        DateHumanize.Verify("ngayon", 0, TimeUnit.Second, Tense.Future);
}
