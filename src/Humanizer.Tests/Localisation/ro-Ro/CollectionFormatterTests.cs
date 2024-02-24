﻿namespace roRO;

[UseCulture("ro-RO")]
public class CollectionFormatterTests
{
    [Fact]
    public void OneItem()
    {
        var collection = new List<int>(new[] { 1 });
        var humanized = "1";
        Assert.Equal(humanized, collection.Humanize());
    }

    [Fact]
    public void TwoItems()
    {
        var collection = new List<int>(new[] { 1, 2 });
        var humanized = "1 și 2";
        Assert.Equal(humanized, collection.Humanize());
    }

    [Fact]
    public void MoreThanTwoItems()
    {
        var collection = new List<int>(new[] { 1, 2, 3 });
        var humanized = "1, 2 și 3";
        Assert.Equal(humanized, collection.Humanize());
    }
}