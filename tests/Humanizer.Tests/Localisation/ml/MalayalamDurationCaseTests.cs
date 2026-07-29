namespace ml;

[UseCulture("ml-IN")]
public class MalayalamDurationCaseTests
{
    [Theory]
    [InlineData(GrammaticalCase.Genitive, "1 ദിവസത്തിന്റെ")]
    [InlineData(GrammaticalCase.Dative, "1 ദിവസത്തിന്")]
    [InlineData(GrammaticalCase.Accusative, "1 ദിവസത്തെ")]
    [InlineData(GrammaticalCase.Instrumental, "1 ദിവസം കൊണ്ട്")]
    [InlineData(GrammaticalCase.Locative, "1 ദിവസത്തിൽ")]
    [InlineData(GrammaticalCase.Sociative, "1 ദിവസത്തോട്")]
    public void DurationAppliesTheRequestedCase(GrammaticalCase grammaticalCase, string expected) =>
        Assert.Equal(
            expected,
            TimeSpan.FromDays(1).HumanizeWithCase(
                grammaticalCase,
                maxUnit: TimeUnit.Day,
                minUnit: TimeUnit.Day));

    [Fact]
    public void SociativeMillisecondIsExplicitlyUnsupported() =>
        Assert.Throws<NotSupportedException>(
            () => TimeSpan.FromMilliseconds(1).HumanizeWithCase(
                GrammaticalCase.Sociative,
                maxUnit: TimeUnit.Millisecond,
                minUnit: TimeUnit.Millisecond));
}