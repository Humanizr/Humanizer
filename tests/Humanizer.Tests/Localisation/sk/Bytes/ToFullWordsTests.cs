namespace sk.Bytes;

[UseCulture("sk-SK")]
public class ToFullWordsTests
{
    [Fact]
    public void ReturnsSingularByte() =>
        Assert.Equal("1 bajt", ByteSize.FromBytes(1).ToFullWords());

    [Fact]
    public void ReturnsPaucalBytes() =>
        Assert.Equal("2 bajty", ByteSize.FromBytes(2).ToFullWords());

    [Fact]
    public void ReturnsPluralBytes() =>
        Assert.Equal("10 bajtov", ByteSize.FromBytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularGigabyte() =>
        Assert.Equal("1 gigabajt", ByteSize.FromGigabytes(1).ToFullWords());

    [Fact]
    public void ReturnsPaucalGigabytes() =>
        Assert.Equal("2 gigabajty", ByteSize.FromGigabytes(2).ToFullWords());

    [Fact]
    public void ReturnsPluralGigabytes() =>
        Assert.Equal("10 gigabajtov", ByteSize.FromGigabytes(10).ToFullWords());
}
