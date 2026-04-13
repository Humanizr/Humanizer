namespace Humanizer.Tests.Localisation.Default;

/// <summary>
/// Tests for <see cref="DefaultNumberToWordsConverter"/>, the fallback when no locale-specific
/// number-to-words converter is registered.
/// </summary>
public class DefaultNumberToWordsConverterTests
{
    [Fact]
    [UseCulture("en-US")]
    public void Convert_ReturnsFormattedNumber()
    {
        var converter = new DefaultNumberToWordsConverter(CultureInfo.CurrentCulture);
        var result = converter.Convert(42L);

        Assert.Equal("42", result);
    }

    [Fact]
    [UseCulture("en-US")]
    public void Convert_Zero_ReturnsZeroString()
    {
        var converter = new DefaultNumberToWordsConverter(CultureInfo.CurrentCulture);
        var result = converter.Convert(0L);

        Assert.Equal("0", result);
    }

    [Fact]
    [UseCulture("en-US")]
    public void Convert_NegativeNumber_ReturnsNegativeString()
    {
        var converter = new DefaultNumberToWordsConverter(CultureInfo.CurrentCulture);
        var result = converter.Convert(-123L);

        Assert.Equal("-123", result);
    }

    [Fact]
    [UseCulture("en-US")]
    public void Convert_LargeNumber_ReturnsFormattedString()
    {
        var converter = new DefaultNumberToWordsConverter(CultureInfo.CurrentCulture);
        var result = converter.Convert(1000000L);

        Assert.Equal("1000000", result);
    }

    [Fact]
    [UseCulture("en-US")]
    public void ConvertToOrdinal_ReturnsFormattedNumber()
    {
        var converter = new DefaultNumberToWordsConverter(CultureInfo.CurrentCulture);
        var result = converter.ConvertToOrdinal(42);

        Assert.Equal("42", result);
    }

    [Fact]
    [UseCulture("en-US")]
    public void ConvertToOrdinal_Zero_ReturnsZeroString()
    {
        var converter = new DefaultNumberToWordsConverter(CultureInfo.CurrentCulture);
        var result = converter.ConvertToOrdinal(0);

        Assert.Equal("0", result);
    }

    [Fact]
    [UseCulture("en-US")]
    public void ConvertToOrdinal_NegativeNumber_ReturnsNegativeString()
    {
        var converter = new DefaultNumberToWordsConverter(CultureInfo.CurrentCulture);
        var result = converter.ConvertToOrdinal(-7);

        Assert.Equal("-7", result);
    }

    [Fact]
    public void Convert_WithNullCulture_ReturnsFormattedNumber()
    {
        var converter = new DefaultNumberToWordsConverter(null);
        var result = converter.Convert(42L);

        Assert.Equal("42", result);
    }

    [Fact]
    public void ConvertToOrdinal_WithNullCulture_ReturnsFormattedNumber()
    {
        var converter = new DefaultNumberToWordsConverter(null);
        var result = converter.ConvertToOrdinal(42);

        Assert.Equal("42", result);
    }
}