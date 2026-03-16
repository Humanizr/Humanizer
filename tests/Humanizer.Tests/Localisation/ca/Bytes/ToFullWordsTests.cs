namespace ca.Bytes;

[UseCulture("ca")]
public class ToFullWordsTests
{
    [Fact]
    public void ReturnsSingularBit() =>
        Assert.Equal("1 bit", ByteSize.FromBits(1).ToFullWords());

    [Fact]
    public void ReturnsPluralBits() =>
        Assert.Equal("2 bits", ByteSize.FromBits(2).ToFullWords());

    [Fact]
    public void ReturnsSingularByte() =>
        Assert.Equal("1 byte", ByteSize.FromBytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralBytes() =>
        Assert.Equal("10 bytes", ByteSize.FromBytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularKiloByte() =>
        Assert.Equal("1 kilobyte", ByteSize.FromKilobytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralKilobytes() =>
        Assert.Equal("10 kilobytes", ByteSize.FromKilobytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularMegabyte() =>
        Assert.Equal("1 megabyte", ByteSize.FromMegabytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralMegabytes() =>
        Assert.Equal("10 megabytes", ByteSize.FromMegabytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularGigabyte() =>
        Assert.Equal("1 gigabyte", ByteSize.FromGigabytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralGigabytes() =>
        Assert.Equal("10 gigabytes", ByteSize.FromGigabytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularTerabyte() =>
        Assert.Equal("1 terabyte", ByteSize.FromTerabytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralTerabytes() =>
        Assert.Equal("10 terabytes", ByteSize.FromTerabytes(10).ToFullWords());

    [Theory]
    [InlineData(229376, "B", "229376 bytes")]
    [InlineData(229376, "# KB", "224 kilobytes")]
    public void ToFullWordsFormatted(double input, string format, string expectedValue) =>
        Assert.Equal(expectedValue, ByteSize.FromBytes(input).ToFullWords(format));
}
