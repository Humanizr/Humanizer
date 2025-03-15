namespace lt;

[UseCulture("lt")]
public class DateHÍumanizeTests
{
    [Fact]
    public void Now() =>
        DateHumanize.Verify("dabar", 0, TimeUnit.Millisecond, Tense.Past);
}