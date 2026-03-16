namespace da;

[UseCulture("da")]
public class CollectionFormatterTests
{
    [Fact]
    public void OneItem()
    {
        var collection = new List<int>([1]);
        Assert.Equal("1", collection.Humanize());
    }

    [Fact]
    public void TwoItems()
    {
        var collection = new List<int>([1, 2]);
        Assert.Equal("1 og 2", collection.Humanize());
    }

    [Fact]
    public void MoreThanTwoItems()
    {
        var collection = new List<int>([1, 2, 3]);
        Assert.Equal("1, 2 og 3", collection.Humanize());
    }
}
