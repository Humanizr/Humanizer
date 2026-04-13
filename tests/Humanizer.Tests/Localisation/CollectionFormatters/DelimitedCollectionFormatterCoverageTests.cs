namespace Humanizer.Tests.Localisation.CollectionFormatters;

public class DelimitedCollectionFormatterCoverageTests
{
    readonly DelimitedCollectionFormatter formatter = new("、");

    // ── Overload 1: Humanize<T>(IEnumerable<T>) ──

    [Fact]
    public void Humanize_EmptyCollection_ReturnsEmpty()
    {
        var result = formatter.Humanize(Array.Empty<string>());
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Humanize_SingleItem_ReturnsItem()
    {
        var result = formatter.Humanize(new[] { "Alpha" });
        Assert.Equal("Alpha", result);
    }

    [Fact]
    public void Humanize_MultipleItems_JoinsWithDelimiter()
    {
        var result = formatter.Humanize(new[] { "A", "B", "C" });
        Assert.Equal("A、B、C", result);
    }

    [Fact]
    public void Humanize_NullCollection_ThrowsArgumentNullException()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => formatter.Humanize<string>(null!));
        Assert.Equal("collection", ex.ParamName);
    }

    // ── Overload 2: Humanize<T>(IEnumerable<T>, Func<T, string?>) ──

    [Fact]
    public void Humanize_StringFormatter_FormatsItems()
    {
        var result = formatter.Humanize(new[] { 1, 2, 3 }, n => $"#{n}");
        Assert.Equal("#1、#2、#3", result);
    }

    [Fact]
    public void Humanize_StringFormatter_NullCollection_ThrowsArgumentNullException()
    {
        var ex = Assert.Throws<ArgumentNullException>(
            () => formatter.Humanize<int>(null!, n => n.ToString()));
        Assert.Equal("collection", ex.ParamName);
    }

    [Fact]
    public void Humanize_StringFormatter_NullFormatter_ThrowsArgumentNullException()
    {
        var ex = Assert.Throws<ArgumentNullException>(
            () => formatter.Humanize(new[] { 1 }, (Func<int, string?>)null!));
        Assert.Equal("objectFormatter", ex.ParamName);
    }

    // ── Overload 3: Humanize<T>(IEnumerable<T>, Func<T, object?>) ──

    [Fact]
    public void Humanize_ObjectFormatter_FormatsItems()
    {
        var result = formatter.Humanize(new[] { "a", "b" }, (Func<string, object?>)(s => s.ToUpperInvariant()));
        Assert.Equal("A、B", result);
    }

    [Fact]
    public void Humanize_ObjectFormatter_NullCollection_ThrowsArgumentNullException()
    {
        var ex = Assert.Throws<ArgumentNullException>(
            () => formatter.Humanize<string>(null!, (Func<string, object?>)(s => s)));
        Assert.Equal("collection", ex.ParamName);
    }

    [Fact]
    public void Humanize_ObjectFormatter_NullFormatter_ThrowsArgumentNullException()
    {
        var ex = Assert.Throws<ArgumentNullException>(
            () => formatter.Humanize(new[] { "a" }, (Func<string, object?>)null!));
        Assert.Equal("objectFormatter", ex.ParamName);
    }

    // ── Overload 4: Humanize<T>(IEnumerable<T>, string) ──

    [Fact]
    public void Humanize_CustomSeparator_OverridesDefault()
    {
        var result = formatter.Humanize(new[] { "X", "Y", "Z" }, " | ");
        Assert.Equal("X | Y | Z", result);
    }

    [Fact]
    public void Humanize_CustomSeparator_SingleItem_ReturnsItem()
    {
        var result = formatter.Humanize(new[] { "solo" }, " | ");
        Assert.Equal("solo", result);
    }

    // ── Overload 5: Humanize<T>(IEnumerable<T>, Func<T, string?>, string) ──

    [Fact]
    public void Humanize_StringFormatterAndSeparator_FormatsAndJoins()
    {
        var result = formatter.Humanize(new[] { 10, 20 }, n => n.ToString(), " + ");
        Assert.Equal("10 + 20", result);
    }

    // ── Overload 6: Humanize<T>(IEnumerable<T>, Func<T, object?>, string) ──

    [Fact]
    public void Humanize_ObjectFormatterAndSeparator_FormatsAndJoins()
    {
        var result = formatter.Humanize(
            new[] { "x", "y" },
            (Func<string, object?>)(s => s.ToUpperInvariant()),
            " & ");
        Assert.Equal("X & Y", result);
    }

    [Fact]
    public void Humanize_ObjectFormatterAndSeparator_NullFormatter_ThrowsArgumentNullException()
    {
        var ex = Assert.Throws<ArgumentNullException>(
            () => formatter.Humanize(new[] { "a" }, (Func<string, object?>)null!, " | "));
        Assert.Equal("objectFormatter", ex.ParamName);
    }

    // ── Whitespace skipping ──

    [Fact]
    public void Humanize_SkipsWhitespaceItems()
    {
        var result = formatter.Humanize(new[] { "A", "  ", "B" });
        Assert.Equal("A、B", result);
    }

    [Fact]
    public void Humanize_SkipsNullFormattedItems()
    {
        var result = formatter.Humanize(new[] { "A", null, "B" }, (Func<string?, string?>)(s => s));
        Assert.Equal("A、B", result);
    }

    [Fact]
    public void Humanize_TrimsFormattedItems()
    {
        var result = formatter.Humanize(new[] { "  A  ", "  B  " });
        Assert.Equal("A、B", result);
    }

    [Fact]
    public void Humanize_AllWhitespaceItems_ReturnsEmpty()
    {
        var result = formatter.Humanize(new[] { " ", "\t", "" });
        Assert.Equal(string.Empty, result);
    }
}