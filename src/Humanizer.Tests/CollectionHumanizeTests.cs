public class SomeClass
{
    public string? SomeString;
    public int SomeInt;
    public override string ToString() =>
        "ToString";
}

[UseCulture("en")]
public class CollectionHumanizeTests
{
    [Fact]
    public void HumanizeReturnsOnlyNameWhenCollectionContainsOneItem()
    {
        var collection = new List<string> { "A String" };

        Assert.Equal("A String", collection.Humanize());
    }

    [Fact]
    public void HumanizeUsesSeparatorWhenMoreThanOneItemIsInCollection()
    {
        var collection = new List<string>
        {
            "A String",
            "Another String",
        };

        Assert.Equal("A String or Another String", collection.Humanize("or"));
    }

    [Fact]
    public void HumanizeDefaultsSeparatorToAnd()
    {
        var collection = new List<string>
        {
            "A String",
            "Another String",
        };

        Assert.Equal("A String and Another String", collection.Humanize());
    }

    [Fact]
    public void HumanizeUsesOxfordComma()
    {
        var collection = new List<string>
        {
            "A String",
            "Another String",
            "A Third String",
        };

        Assert.Equal("A String, Another String, or A Third String", collection.Humanize("or"));
    }

    readonly List<SomeClass> testCollection =
    [
        new() { SomeInt = 1, SomeString = "One" },
        new() { SomeInt = 2, SomeString = "Two" },
        new() { SomeInt = 3, SomeString = "Three" }
    ];

    [Fact]
    public void HumanizeDefaultsToToString() =>
        Assert.Equal("ToString, ToString, or ToString", testCollection.Humanize("or"));

    [Fact]
    public void HumanizeUsesStringDisplayFormatter()
    {
        var humanized = testCollection.Humanize(sc => $"SomeObject #{sc.SomeInt} - {sc.SomeString}");
        Assert.Equal("SomeObject #1 - One, SomeObject #2 - Two, and SomeObject #3 - Three", humanized);
    }

    [Fact]
    public void HumanizeUsesObjectDisplayFormatter()
    {
        var humanized = testCollection.Humanize(sc => sc.SomeInt);
        Assert.Equal("1, 2, and 3", humanized);
    }

    [Fact]
    public void HumanizeUsesStringDisplayFormatterWhenSeparatorIsProvided()
    {
        var humanized = testCollection.Humanize(sc => $"SomeObject #{sc.SomeInt} - {sc.SomeString}", "or");
        Assert.Equal("SomeObject #1 - One, SomeObject #2 - Two, or SomeObject #3 - Three", humanized);
    }

    [Fact]
    public void HumanizeUsesObjectDisplayFormatterWhenSeparatorIsProvided()
    {
        var humanized = testCollection.Humanize(sc => sc.SomeInt, "or");
        Assert.Equal("1, 2, or 3", humanized);
    }

    [Fact]
    public void HumanizeHandlesNullItemsWithoutAnException() =>
        Assert.Null(Record.Exception(() => new object?[] { null, null }.Humanize()));

    [Fact]
    public void HumanizeHandlesNullStringDisplayFormatterReturnsWithoutAnException() =>
        Assert.Null(Record.Exception(() => new[] { "A", "B", "C" }.Humanize(_ => null!)));

    [Fact]
    public void HumanizeHandlesNullObjectDisplayFormatterReturnsWithoutAnException() =>
        Assert.Null(Record.Exception(() => new[] { "A", "B", "C" }.Humanize(_ => (object)null!)));

    [Fact]
    public void HumanizeRunsStringDisplayFormatterOnNulls() =>
        Assert.Equal("1, (null), and 3", new int?[] { 1, null, 3 }.Humanize(_ => _?.ToString() ?? "(null)"));

    [Fact]
    public void HumanizeRunsObjectDisplayFormatterOnNulls() =>
        Assert.Equal("1, 2, and 3", new int?[] { 1, null, 3 }.Humanize(_ => _ ?? 2));

    [Fact]
    public void HumanizeRemovesEmptyItemsByDefault() =>
        Assert.Equal("A and C", new[] { "A", " ", "C" }.Humanize(DummyFormatter));

    [Fact]
    public void HumanizeTrimsItemsByDefault() =>
        Assert.Equal("A, B, and C", new[] { "A", "  B  ", "C" }.Humanize(DummyFormatter));

    /// <summary>
    /// Use the dummy formatter to ensure tests are testing formatter output rather than input
    /// </summary>
    static readonly Func<string, string> DummyFormatter = input => input;
}