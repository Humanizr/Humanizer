namespace lv;

[UseCulture("lv")]
public class DateHumanizeTests
{
    [Fact]
    public void TwoDaysAgo() =>
        DateHumanize.Verify("pirms 2 dienām", 2, TimeUnit.Day, Tense.Past);

    [Fact]
    public void TwoDaysFromNow() =>
        DateHumanize.Verify("pēc 2 dienām", 2, TimeUnit.Day, Tense.Future);

    [Fact]
    public void MultipleDaysUseLatvianCountedForm()
    {
        DateHumanize.Verify("pirms 3 dienām", 3, TimeUnit.Day, Tense.Past);
        DateHumanize.Verify("pēc 3 dienām", 3, TimeUnit.Day, Tense.Future);
    }
}
