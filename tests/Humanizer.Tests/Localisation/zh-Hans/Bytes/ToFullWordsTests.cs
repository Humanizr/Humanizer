namespace zhHans.Bytes;

[UseCulture("zh-Hans")]
public class ToFullWordsTests
{
    [Fact]
    public void ReturnsSingularByte() =>
        Assert.Equal("1 字节", ByteSize.FromBytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralBytes() =>
        Assert.Equal("10 字节", ByteSize.FromBytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularGigabyte() =>
        Assert.Equal("1 吉字节", ByteSize.FromGigabytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralGigabytes() =>
        Assert.Equal("10 吉字节", ByteSize.FromGigabytes(10).ToFullWords());
}
