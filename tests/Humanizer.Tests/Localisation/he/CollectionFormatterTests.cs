namespace he;

[UseCulture("he")]
public class CollectionFormatterTests
{
    [Fact]
    public void TwoItems() =>
        Assert.Equal("1 ו2", new[] { 1, 2 }.Humanize());

    [Fact]
    public void MoreThanTwoItems() =>
        Assert.Equal("1, 2 ו3", new[] { 1, 2, 3 }.Humanize());
}
