namespace fi;

[UseCulture("fi-FI")]
public class FinnishDurationCaseTests
{
    [Theory]
    [InlineData(GrammaticalCase.Genitive, "1 päivän")]
    [InlineData(GrammaticalCase.Partitive, "1 päivää")]
    [InlineData(GrammaticalCase.Elative, "1 päivästä")]
    [InlineData(GrammaticalCase.Illative, "1 päivään")]
    public void DurationAppliesTheRequestedCase(GrammaticalCase grammaticalCase, string expected) =>
        Assert.Equal(
            expected,
            TimeSpan.FromDays(1).HumanizeWithCase(
                grammaticalCase,
                maxUnit: TimeUnit.Day,
                minUnit: TimeUnit.Day));
}