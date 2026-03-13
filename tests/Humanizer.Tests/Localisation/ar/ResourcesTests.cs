namespace ar;

[UseCulture("ar")]
public class ResourcesTests
{
    [Fact]
    public void HasExplicitArabicNeverResource()
    {
        Assert.True(Resources.TryGetResource("DateHumanize_Never", new("ar"), out var actual));
        Assert.Equal("أبدًا", actual);
    }

    [Fact]
    public void HasExplicitArabicTwoDayResources()
    {
        Assert.True(Resources.TryGetResource("DateHumanize_TwoDaysAgo", new("ar"), out var twoDaysAgo));
        Assert.Equal("أول أمس", twoDaysAgo);

        Assert.True(Resources.TryGetResource("DateHumanize_TwoDaysFromNow", new("ar"), out var twoDaysFromNow));
        Assert.Equal("بعد غد", twoDaysFromNow);
    }

    [Fact]
    public void HasExplicitArabicAgeResource()
    {
        Assert.True(Resources.TryGetResource("TimeSpanHumanize_Age", new("ar"), out var actual));
        Assert.Equal("{0} من العمر", actual);
    }
}
