namespace Humanizer.Tests.Localisation.ig;

[UseCulture("ig")]
public class IgboNumberFormattingTests
{
    [Fact]
    public void Ordinalize_UsesAuthoredNegativeSign()
    {
        Assert.Equal("nke -1", (-1).Ordinalize());
    }

    [Fact]
    public void ByteSizeDecimalOutput_UsesAuthoredDecimalSeparator()
    {
        Assert.Equal("1.95 KB", 2000.Bytes().Humanize());
    }

    [Fact]
    public void ByteSizeParsing_UsesAuthoredGroupDecimalAndNegativeSeparators()
    {
        var culture = new CultureInfo("ig");

        Assert.Equal(ByteSize.FromKilobytes(-2000.01), ByteSize.Parse("-2,000.01KB", culture));
    }

    [Fact]
    public void MetricOutput_UsesAuthoredNegativeAndDecimalSeparators()
    {
        Assert.Equal("-1.2k", (-1234L).ToMetric(decimals: 1));
    }
}