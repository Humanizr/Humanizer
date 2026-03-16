namespace msMY.Bytes;

[UseCulture("ms-MY")]
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
        Assert.Equal("1 bait", ByteSize.FromBytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralBytes() =>
        Assert.Equal("10 bait", ByteSize.FromBytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularKiloByte() =>
        Assert.Equal("1 kilobait", ByteSize.FromKilobytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralKilobytes() =>
        Assert.Equal("10 kilobait", ByteSize.FromKilobytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularMegabyte() =>
        Assert.Equal("1 megabait", ByteSize.FromMegabytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralMegabytes() =>
        Assert.Equal("10 megabait", ByteSize.FromMegabytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularGigabyte() =>
        Assert.Equal("1 gigabait", ByteSize.FromGigabytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralGigabytes() =>
        Assert.Equal("10 gigabait", ByteSize.FromGigabytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularTerabyte() =>
        Assert.Equal("1 terabait", ByteSize.FromTerabytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralTerabytes() =>
        Assert.Equal("10 terabait", ByteSize.FromTerabytes(10).ToFullWords());
}
