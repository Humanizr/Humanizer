namespace de;

[UseCulture("de")]
public class CollectionFormatterTests
{
    [Fact]
    public void OneItem()
    {
        var collection = new List<int>([1]);
        var humanized = "1";
        Assert.Equal(humanized, collection.Humanize());
    }

    [Fact]
    public void TwoItems()
    {
        var collection = new List<int>([1, 2]);
        var humanized = "1 und 2";
        Assert.Equal(humanized, collection.Humanize());
    }

    [Fact]
    public void MoreThanTwoItems()
    {
        var collection = new List<int>([1, 2, 3]);
        var humanized = "1, 2 und 3";
        Assert.Equal(humanized, collection.Humanize());
    }
}