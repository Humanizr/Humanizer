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
    public void MetricOutput_UsesStableHindiNegativeAndDecimalSeparators()
    {
        Assert.Equal("-1.2k", (-1234L).ToMetric(decimals: 1));
    }
}