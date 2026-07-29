namespace hu;

[UseCulture("hu-HU")]
public class HungarianDurationCaseTests
{
    [Theory]
    [InlineData(GrammaticalCase.Accusative, "1 napot")]
    [InlineData(GrammaticalCase.Instrumental, "1 nappal")]
    [InlineData(GrammaticalCase.Terminative, "1 napig")]
    [InlineData(GrammaticalCase.Translative, "1 nappá")]
    public void DurationAppliesTheRequestedCase(GrammaticalCase grammaticalCase, string expected) =>
        Assert.Equal(
            expected,
            TimeSpan.FromDays(1).HumanizeWithCase(
                grammaticalCase,
                maxUnit: TimeUnit.Day,
                minUnit: TimeUnit.Day));
}