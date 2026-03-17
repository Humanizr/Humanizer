namespace uzCyrlUZ;

[UseCulture("uz-Cyrl-UZ")]
public class CollectionFormatterTests
{
    [Fact]
    public void TwoItems() =>
        Assert.Equal("1 ва 2", new[] { 1, 2 }.Humanize());

    [Fact]
    public void MoreThanTwoItems() =>
        Assert.Equal("1, 2 ва 3", new[] { 1, 2, 3 }.Humanize());
}
