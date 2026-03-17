namespace fa;

[UseCulture("fa")]
public class CollectionFormatterTests
{
    [Fact]
    public void TwoItems() =>
        Assert.Equal("1 و2", new[] { 1, 2 }.Humanize());

    [Fact]
    public void MoreThanTwoItems() =>
        Assert.Equal("1, 2 و3", new[] { 1, 2, 3 }.Humanize());
}
