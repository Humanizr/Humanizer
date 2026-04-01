namespace koKR;

[UseCulture("ko-KR")]
public class DateHumanizeTests
{
    [Fact]
    public void TwoDaysAgo() =>
        DateHumanize.Verify("2일 전", 2, TimeUnit.Day, Tense.Past);

    [Fact]
    public void TwoDaysFromNow() =>
        DateHumanize.Verify("2일 후", 2, TimeUnit.Day, Tense.Future);

    [Fact]
    public void MultipleDaysUseNoSpaceCountedForm()
    {
        DateHumanize.Verify("3일 전", 3, TimeUnit.Day, Tense.Past);
        DateHumanize.Verify("3일 후", 3, TimeUnit.Day, Tense.Future);
    }
}
