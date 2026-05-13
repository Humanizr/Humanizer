namespace Humanizer.Tests.Localisation.pa;

[UseCulture("pa")]
public class PunjabiNumberFormattingTests
{
    [Fact]
    public void ByteSizeDecimalOutput_UsesStablePunjabiSeparators()
    {
        Assert.Equal("1.95 KB", 2000.Bytes().Humanize());
    }

    [Fact]
    public void MetricOutput_UsesStablePunjabiNegativeAndDecimalSeparators()
    {
        Assert.Equal("-1.2k", (-1234L).ToMetric(decimals: 1));
    }
}