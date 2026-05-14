namespace Humanizer.Tests.Localisation.ne;

[UseCulture("ne")]
public class NepaliNumberFormattingTests
{
    [Fact]
    public void ByteSizeDecimalOutput_UsesStableNepaliSeparators()
    {
        Assert.Equal("1.95 KB", 2000.Bytes().Humanize());
    }

    [Fact]
    public void MetricOutput_UsesStableNepaliNegativeAndDecimalSeparators()
    {
        Assert.Equal("-1.2k", (-1234L).ToMetric(decimals: 1));
    }
}