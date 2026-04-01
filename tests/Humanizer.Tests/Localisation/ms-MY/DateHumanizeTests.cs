namespace msMY;

[UseCulture("ms-MY")]
public class DateHumanizeTests
{
    [Fact]
    public void MultipleDaysUseMalayCountedForm()
    {
        DateHumanize.Verify("3 hari yang lalu", 3, TimeUnit.Day, Tense.Past);
        DateHumanize.Verify("3 hari dari sekarang", 3, TimeUnit.Day, Tense.Future);
    }
}
