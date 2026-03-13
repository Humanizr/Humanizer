namespace koKR.Bytes;

[UseCulture("ko-KR")]
public class ToFullWordsTests
{
    [Fact]
    public void ReturnsSingularBit() =>
        Assert.Equal("1 비트", ByteSize.FromBits(1).ToFullWords());

    [Fact]
    public void ReturnsPluralBits() =>
        Assert.Equal("2 비트", ByteSize.FromBits(2).ToFullWords());

    [Fact]
    public void ReturnsSingularByte() =>
        Assert.Equal("1 바이트", ByteSize.FromBytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralBytes() =>
        Assert.Equal("10 바이트", ByteSize.FromBytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularKiloByte() =>
        Assert.Equal("1 킬로바이트", ByteSize.FromKilobytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralKilobytes() =>
        Assert.Equal("10 킬로바이트", ByteSize.FromKilobytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularMegabyte() =>
        Assert.Equal("1 메가바이트", ByteSize.FromMegabytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralMegabytes() =>
        Assert.Equal("10 메가바이트", ByteSize.FromMegabytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularGigabyte() =>
        Assert.Equal("1 기가바이트", ByteSize.FromGigabytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralGigabytes() =>
        Assert.Equal("10 기가바이트", ByteSize.FromGigabytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularTerabyte() =>
        Assert.Equal("1 테라바이트", ByteSize.FromTerabytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralTerabytes() =>
        Assert.Equal("10 테라바이트", ByteSize.FromTerabytes(10).ToFullWords());

    [Theory]
    [InlineData(229376, "B", "229376 바이트")]
    [InlineData(229376, "# KB", "224 킬로바이트")]
    public void ToFullWordsFormatted(double input, string format, string expectedValue) =>
        Assert.Equal(expectedValue, ByteSize.FromBytes(input).ToFullWords(format));
}
