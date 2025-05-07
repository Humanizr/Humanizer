namespace lt;

[UseCulture("lt")]
public class DateHumanizeTests
{
    [Fact]
    public void Now() =>
        DateHumanize.Verify("dabar", 0, TimeUnit.Millisecond, Tense.Past);
}