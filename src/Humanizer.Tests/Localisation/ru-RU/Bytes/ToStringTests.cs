namespace ruRU.Bytes;

[UseCulture("ru-RU")]
public class ToStringTests
{
    [Fact]
    public void ReturnsLargestMetricSuffix() =>
        Assert.Equal("10,5 КБ", ByteSize.FromKilobytes(10.5).ToString());

    [Fact]
    public void ReturnsDefaultNumberFormat() =>
        Assert.Equal("10,5 КБ", ByteSize.FromKilobytes(10.5).ToString("KB"));

    [Fact]
    public void ReturnsProvidedNumberFormat() =>
        Assert.Equal("10,1234 КБ", ByteSize.FromKilobytes(10.1234).ToString("#.#### KB"));

    [Fact]
    public void ReturnsBits() =>
        Assert.Equal("10 бит", ByteSize.FromBits(10).ToString("##.#### b"));

    [Fact]
    public void ReturnsBytes() =>
        Assert.Equal("10 Б", ByteSize.FromBytes(10).ToString("##.#### B"));

    [Fact]
    public void ReturnsKilobytes() =>
        Assert.Equal("10 КБ", ByteSize.FromKilobytes(10).ToString("##.#### KB"));

    [Fact]
    public void ReturnsMegabytes() =>
        Assert.Equal("10 МБ", ByteSize.FromMegabytes(10).ToString("##.#### MB"));

    [Fact]
    public void ReturnsGigabytes() =>
        Assert.Equal("10 ГБ", ByteSize.FromGigabytes(10).ToString("##.#### GB"));

    [Fact]
    public void ReturnsTerabytes() =>
        Assert.Equal("10 ТБ", ByteSize.FromTerabytes(10).ToString("##.#### TB"));

    [Fact]
    public void ReturnsSelectedFormat() =>
        Assert.Equal("10,0 ТБ", ByteSize.FromTerabytes(10).ToString("0.0 TB"));

    [Fact]
    public void ReturnsLargestMetricPrefixLargerThanZero() =>
        Assert.Equal("512 КБ", ByteSize.FromMegabytes(.5).ToString("#.#"));

    [Fact]
    public void ReturnsLargestMetricPrefixLargerThanZeroForNegativeValues() =>
        Assert.Equal("-512 КБ", ByteSize.FromMegabytes(-.5).ToString("#.#"));

    [Fact]
    public void ReturnsBytesViaGeneralFormat() =>
        Assert.Equal("10 Б", $"{ByteSize.FromBytes(10)}");
}