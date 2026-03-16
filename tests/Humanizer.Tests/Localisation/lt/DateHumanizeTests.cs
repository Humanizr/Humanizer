namespace lt;

[UseCulture("lt")]
public class DateHumanizeTests
{
    [Fact]
    public void Now() =>
        DateHumanize.Verify("dabar", 0, TimeUnit.Millisecond, Tense.Past);

    [Fact]
    public void TwoDaysAgo() =>
        DateHumanize.Verify("prieš 2 dienas", 2, TimeUnit.Day, Tense.Past);

    [Fact]
    public void TwoDaysFromNow() =>
        DateHumanize.Verify("po 2 dienų", 2, TimeUnit.Day, Tense.Future);
}
