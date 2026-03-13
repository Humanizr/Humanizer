namespace zhHK.Bytes;

[UseCulture("zh-HK")]
public class ToFullWordsTests
{
    [Fact]
    public void ReturnsSingularByte() =>
        Assert.Equal("1 位元組", ByteSize.FromBytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralBytes() =>
        Assert.Equal("10 位元組", ByteSize.FromBytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularGigabyte() =>
        Assert.Equal("1 十億位元組", ByteSize.FromGigabytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralGigabytes() =>
        Assert.Equal("10 十億位元組", ByteSize.FromGigabytes(10).ToFullWords());
}
