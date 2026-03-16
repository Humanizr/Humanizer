namespace hu.Bytes;

[UseCulture("hu-HU")]
public class ToFullWordsTests
{
    [Fact]
    public void ReturnsSingularBit() =>
        Assert.Equal("1 bit", ByteSize.FromBits(1).ToFullWords());

    [Fact]
    public void ReturnsPluralBits() =>
        Assert.Equal("2 bit", ByteSize.FromBits(2).ToFullWords());

    [Fact]
    public void ReturnsSingularByte() =>
        Assert.Equal("1 bájt", ByteSize.FromBytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralBytes() =>
        Assert.Equal("10 bájt", ByteSize.FromBytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularKiloByte() =>
        Assert.Equal("1 kilobájt", ByteSize.FromKilobytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralKilobytes() =>
        Assert.Equal("10 kilobájt", ByteSize.FromKilobytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularMegabyte() =>
        Assert.Equal("1 megabájt", ByteSize.FromMegabytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralMegabytes() =>
        Assert.Equal("10 megabájt", ByteSize.FromMegabytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularGigabyte() =>
        Assert.Equal("1 gigabájt", ByteSize.FromGigabytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralGigabytes() =>
        Assert.Equal("10 gigabájt", ByteSize.FromGigabytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularTerabyte() =>
        Assert.Equal("1 terabájt", ByteSize.FromTerabytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralTerabytes() =>
        Assert.Equal("10 terabájt", ByteSize.FromTerabytes(10).ToFullWords());

    [Theory]
    [InlineData(229376, "B", "229376 bájt")]
    [InlineData(229376, "# KB", "224 kilobájt")]
    public void ToFullWordsFormatted(double input, string format, string expectedValue) =>
        Assert.Equal(expectedValue, ByteSize.FromBytes(input).ToFullWords(format));
}
