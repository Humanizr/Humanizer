namespace ukUA;

[UseCulture("uk-UA")]
public class ResourcesTests
{
    [Fact]
    public void DateHumanizeResidualResourcesExist()
    {
        Assert.Equal("{0} дні тому", Resources.GetResource("DateHumanize_MultipleDaysAgo_Dual", new("uk-UA")));
        Assert.Equal("{0} дні тому", Resources.GetResource("DateHumanize_MultipleDaysAgo_Paucal", new("uk-UA")));
        Assert.Equal("{0} днів тому", Resources.GetResource("DateHumanize_MultipleDaysAgo_Plural", new("uk-UA")));
        Assert.Equal("позавчора", Resources.GetResource("DateHumanize_TwoDaysAgo", new("uk-UA")));
        Assert.Equal("післязавтра", Resources.GetResource("DateHumanize_TwoDaysFromNow", new("uk-UA")));
        Assert.Equal("ніколи", Resources.GetResource("DateHumanize_Never", new("uk-UA")));
    }

    [Fact]
    public void TimeSpanResidualResourcesExist()
    {
        Assert.Equal("{0}", Resources.GetResource("TimeSpanHumanize_Age", new("uk-UA")));
        Assert.Equal("{0} дні", Resources.GetResource("TimeSpanHumanize_MultipleDays_Dual", new("uk-UA")));
        Assert.Equal("{0} днів", Resources.GetResource("TimeSpanHumanize_MultipleDays_Plural", new("uk-UA")));
        Assert.Equal("{0} роки", Resources.GetResource("TimeSpanHumanize_MultipleYears_Dual", new("uk-UA")));
        Assert.Equal("{0} років", Resources.GetResource("TimeSpanHumanize_MultipleYears_Plural", new("uk-UA")));
    }
}
