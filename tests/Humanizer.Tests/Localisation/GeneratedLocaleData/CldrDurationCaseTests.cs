namespace GeneratedLocaleData;

[UseCulture("en-US")]
public class CldrDurationCaseTests
{
    [Theory]
    [InlineData("da-DK", GrammaticalCase.Genitive, TimeUnit.Day, 1, "1 dags")]
    [InlineData("sv-SE", GrammaticalCase.Genitive, TimeUnit.Day, 1, "1 dygns")]
    [InlineData("nn-NO", GrammaticalCase.Genitive, TimeUnit.Week, 1, "1 vekes")]
    [InlineData("nb-NO", GrammaticalCase.Genitive, TimeUnit.Week, 1, "1 ukes")]
    [InlineData("ro-RO", GrammaticalCase.Genitive, TimeUnit.Day, 1, "unei zile")]
    [InlineData("am-ET", GrammaticalCase.Accusative, TimeUnit.Day, 1, "አንድ ቀን")]
    [InlineData("hi-IN", GrammaticalCase.Oblique, TimeUnit.Day, 1, "1 दिन")]
    [InlineData("pa-IN", GrammaticalCase.Oblique, TimeUnit.Week, 1, "1 ਹਫ਼ਤੇ")]
    public void PinnedCldrDurationCasesAreCultureSpecific(
        string cultureName,
        GrammaticalCase grammaticalCase,
        TimeUnit unit,
        int count,
        string expected)
    {
        var duration = unit == TimeUnit.Week
            ? TimeSpan.FromDays(count * 7)
            : TimeSpan.FromDays(count);

        Assert.Equal(
            expected,
            duration.HumanizeWithCase(
                grammaticalCase,
                culture: new(cultureName),
                maxUnit: unit,
                minUnit: unit));
    }

    [Fact]
    public void ApplicableLocaleWithoutVerifiedFormsFailsClearly()
    {
        var exception = Assert.Throws<NotSupportedException>(
            () => TimeSpan.FromDays(1).HumanizeWithCase(
                GrammaticalCase.Dative,
                culture: new("az"),
                maxUnit: TimeUnit.Day,
                minUnit: TimeUnit.Day));

        Assert.Contains("verified duration forms are unavailable", exception.Message);
    }
}