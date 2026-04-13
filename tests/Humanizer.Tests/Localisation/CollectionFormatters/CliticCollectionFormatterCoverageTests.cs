namespace Humanizer.Tests.Localisation.CollectionFormatters;

public class CliticCollectionFormatterCoverageTests
{
    readonly CliticCollectionFormatter formatter = new("و");

    // ── Count switch arm: 0 (empty) ──

    [Fact]
    public void Humanize_EmptyCollection_ReturnsEmpty()
    {
        var result = formatter.Humanize(Array.Empty<string>());
        Assert.Equal(string.Empty, result);
    }

    // ── Count switch arm: 1 (single item) ──

    [Fact]
    public void Humanize_SingleItem_ReturnsItem()
    {
        var result = formatter.Humanize(new[] { "Alpha" });
        Assert.Equal("Alpha", result);
    }

    // ── Count switch arm: _ (two or more items) ──

    [Fact]
    public void Humanize_TwoItems_JoinsWithConjunction()
    {
        var result = formatter.Humanize(new[] { "A", "B" });
        // Clitic formatter: head + " " + separator + lastItem
        Assert.Equal("A وB", result);
    }

    [Fact]
    public void Humanize_ThreeItems_JoinsWithCommaAndConjunction()
    {
        var result = formatter.Humanize(new[] { "A", "B", "C" });
        // head = "A, B", separator = "و", lastItem = "C"
        Assert.Equal("A, B وC", result);
    }

    [Fact]
    public void Humanize_FourItems_JoinsCorrectly()
    {
        var result = formatter.Humanize(new[] { "A", "B", "C", "D" });
        Assert.Equal("A, B, C وD", result);
    }

    // ── Null guards ──

    [Fact]
    public void Humanize_NullCollection_ThrowsArgumentNullException()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => formatter.Humanize<string>(null!));
        Assert.Equal("collection", ex.ParamName);
    }

    [Fact]
    public void Humanize_NullObjectFormatter_ThrowsArgumentNullException()
    {
        var ex = Assert.Throws<ArgumentNullException>(
            () => formatter.Humanize(new[] { "A" }, (Func<string, string?>)null!));
        Assert.Equal("objectFormatter", ex.ParamName);
    }

    // ── Overload 2: Humanize<T>(IEnumerable<T>, Func<T, string?>) ──

    [Fact]
    public void Humanize_StringFormatter_FormatsItems()
    {
        var result = formatter.Humanize(new[] { 1, 2, 3 }, n => $"#{n}");
        Assert.Equal("#1, #2 و#3", result);
    }

    // ── Overload 3: Humanize<T>(IEnumerable<T>, Func<T, object?>) ──

    [Fact]
    public void Humanize_ObjectFormatter_FormatsItems()
    {
        var result = formatter.Humanize(
            new[] { "a", "b" },
            (Func<string, object?>)(s => s.ToUpperInvariant()));
        Assert.Equal("A وB", result);
    }

    [Fact]
    public void Humanize_ObjectFormatter_NullFormatter_ThrowsArgumentNullException()
    {
        var ex = Assert.Throws<ArgumentNullException>(
            () => formatter.Humanize(new[] { "a" }, (Func<string, object?>)null!));
        Assert.Equal("objectFormatter", ex.ParamName);
    }

    // ── Overload 4: Humanize<T>(IEnumerable<T>, string) — delegates to fallback ──

    [Fact]
    public void Humanize_CustomSeparator_DelegatesToFallbackFormatter()
    {
        var result = formatter.Humanize(new[] { "X", "Y", "Z" }, "or");
        // Delegates to DefaultCollectionFormatter which uses "{0} {1} {2}" format
        Assert.Equal("X, Y or Z", result);
    }

    // ── Overload 5: Humanize<T>(IEnumerable<T>, Func<T, string?>, string) ──

    [Fact]
    public void Humanize_StringFormatterAndSeparator_FormatsAndJoins()
    {
        var result = formatter.Humanize(new[] { 10, 20, 30 }, n => n.ToString(), "y");
        Assert.Equal("10, 20 y30", result);
    }

    [Fact]
    public void Humanize_StringFormatterAndSeparator_SingleItem()
    {
        var result = formatter.Humanize(new[] { 42 }, n => n.ToString(), "y");
        Assert.Equal("42", result);
    }

    [Fact]
    public void Humanize_StringFormatterAndSeparator_EmptyCollection()
    {
        var result = formatter.Humanize(Array.Empty<int>(), n => n.ToString(), "y");
        Assert.Equal(string.Empty, result);
    }

    // ── Overload 6: Humanize<T>(IEnumerable<T>, Func<T, object?>, string) ──

    [Fact]
    public void Humanize_ObjectFormatterAndSeparator_FormatsAndJoins()
    {
        var result = formatter.Humanize(
            new[] { "x", "y" },
            (Func<string, object?>)(s => s.ToUpperInvariant()),
            " & ");
        // Clitic join: string.Concat(head, " ", separator, lastItem) → "X" + " " + " & " + "Y"
        Assert.Equal("X  & Y", result);
    }

    [Fact]
    public void Humanize_ObjectFormatterAndSeparator_NullFormatter_ThrowsArgumentNullException()
    {
        var ex = Assert.Throws<ArgumentNullException>(
            () => formatter.Humanize(new[] { "a" }, (Func<string, object?>)null!, " & "));
        Assert.Equal("objectFormatter", ex.ParamName);
    }

    // ── Whitespace skipping ──

    [Fact]
    public void Humanize_SkipsWhitespaceItems()
    {
        var result = formatter.Humanize(new[] { "A", "  ", "B" });
        Assert.Equal("A وB", result);
    }

    [Fact]
    public void Humanize_TrimsFormattedItems()
    {
        var result = formatter.Humanize(new[] { "  A  ", "  B  " });
        Assert.Equal("A وB", result);
    }

    [Fact]
    public void Humanize_AllWhitespaceItems_ReturnsEmpty()
    {
        var result = formatter.Humanize(new[] { " ", "\t", "" });
        Assert.Equal(string.Empty, result);
    }
}