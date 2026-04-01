namespace thTH;

[UseCulture("th-TH")]
public class DateHumanizeTests
{
    [Theory]
    [InlineData(1, "หนึ่งวินาทีที่แล้ว")]
    [InlineData(10, "10 วินาทีที่แล้ว")]
    [InlineData(59, "59 วินาทีที่แล้ว")]
    [InlineData(60, "หนึ่งนาทีที่แล้ว")]
    public void SecondsAgo(int seconds, string expected) =>
        DateHumanize.Verify(expected, seconds, TimeUnit.Second, Tense.Past);

    [Fact]
    public void MultipleDaysUseThaiCountedForms()
    {
        DateHumanize.Verify("3 วันที่แล้ว", 3, TimeUnit.Day, Tense.Past);
        DateHumanize.Verify("อีก 3 วัน", 3, TimeUnit.Day, Tense.Future);
    }
}
