namespace Humanizer.Tests.Localisation.hi;

[UseCulture("hi")]
public class HindiNumberFormattingTests
{
    [Fact]
    public void ByteSizeDecimalOutput_UsesStableHindiSeparators()
    {
        Assert.Equal("1.95 KB", 2000.Bytes().Humanize());
    }

    [Fact]
    public void NegativeNumberFormat_UsesAsciiMinusSign()
    {
        Assert.Equal("-1,234.5", (-1234.5).ToString("N1", new CultureInfo("hi")));
    }
}