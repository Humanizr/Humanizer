namespace vi.Bytes;

[UseCulture("vi")]
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
        Assert.Equal("1 byte", ByteSize.FromBytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralBytes() =>
        Assert.Equal("10 byte", ByteSize.FromBytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularGigabyte() =>
        Assert.Equal("1 gigabyte", ByteSize.FromGigabytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralGigabytes() =>
        Assert.Equal("10 gigabyte", ByteSize.FromGigabytes(10).ToFullWords());

    [Theory]
    [InlineData(229376, "B", "229376 byte")]
    [InlineData(229376, "# KB", "224 kilobyte")]
    public void ToFullWordsFormatted(double input, string format, string expectedValue) =>
        Assert.Equal(expectedValue, ByteSize.FromBytes(input).ToFullWords(format));
}
