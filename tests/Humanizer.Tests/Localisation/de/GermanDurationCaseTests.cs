namespace de;

[UseCulture("de-DE")]
public class GermanDurationCaseTests
{
    [Theory]
    [InlineData(GrammaticalCase.Nominative, "eine Woche")]
    [InlineData(GrammaticalCase.Genitive, "einer Woche")]
    [InlineData(GrammaticalCase.Dative, "einer Woche")]
    [InlineData(GrammaticalCase.Accusative, "eine Woche")]
    public void AuthoredSingularAppliesTheRequestedCase(GrammaticalCase grammaticalCase, string expected) =>
        Assert.Equal(
            expected,
            TimeSpan.FromDays(7).HumanizeWithCase(grammaticalCase));

    [Fact]
    public void MultipleCountsRemainNumeric() =>
        Assert.Equal(
            "2 Wochen",
            TimeSpan.FromDays(14).HumanizeWithCase(GrammaticalCase.Dative));
}