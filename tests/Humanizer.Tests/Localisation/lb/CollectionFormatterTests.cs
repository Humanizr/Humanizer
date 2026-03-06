namespace lb;

[UseCulture("lb-LU")]
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
        var humanized = "1 an 2";
        Assert.Equal(humanized, collection.Humanize());
    }

    [Fact]
    public void MoreThanTwoItems()
    {
        var collection = new List<int>([1, 2, 3]);
        var humanized = "1, 2 an 3";
        Assert.Equal(humanized, collection.Humanize());
    }
}
