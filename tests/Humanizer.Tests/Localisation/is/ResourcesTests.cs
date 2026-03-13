namespace @is;

public class ResourcesTests
{
    [Fact]
    [UseCulture("is")]
    public void GetCultureSpecificTranslationsWithImplicitCulture()
    {
        var format = Resources.GetResource("DateHumanize_MultipleYearsAgo");
        Assert.Equal("fyrir {0} árum", format);
    }

    [Fact]
    public void GetCultureSpecificTranslationsWithExplicitCulture()
    {
        var format = Resources.GetResource("DateHumanize_SingleYearAgo", new("is"));
        Assert.Equal("fyrir einu ári", format);
    }

    [Theory]
    [InlineData("DateHumanize_TwoDaysAgo", "fyrir 2 dögum")]
    [InlineData("DateHumanize_TwoDaysFromNow", "eftir 2 daga")]
    [InlineData("DateHumanize_MultipleDaysAgo_Dual", "fyrir {0} dögum")]
    [InlineData("DateHumanize_MultipleDaysAgo_Paucal", "fyrir {0} dögum")]
    [InlineData("DateHumanize_MultipleDaysFromNow_Dual", "eftir {0} daga")]
    [InlineData("DateHumanize_MultipleDaysFromNow_Paucal", "eftir {0} daga")]
    [InlineData("TimeSpanHumanize_MultipleDays_Dual", "{0} dagar")]
    [InlineData("TimeSpanHumanize_MultipleDays_Paucal", "{0} dagar")]
    [InlineData("TimeSpanHumanize_Age", "{0}")]
    public void ResidualParityResourcesExist(string resourceKey, string expected) =>
        Assert.Equal(expected, Resources.GetResource(resourceKey, new("is")));
}
