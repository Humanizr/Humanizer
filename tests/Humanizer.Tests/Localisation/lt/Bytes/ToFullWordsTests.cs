namespace lt.Bytes;

[UseCulture("lt")]
public class ToFullWordsTests
{
    [Fact]
    public void ReturnsSingularByte() =>
        Assert.Equal("1 baitas", ByteSize.FromBytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralBytes() =>
        Assert.Equal("10 baitų", ByteSize.FromBytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularGigabyte() =>
        Assert.Equal("1 gigabaitas", ByteSize.FromGigabytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralGigabytes() =>
        Assert.Equal("10 gigabaitų", ByteSize.FromGigabytes(10).ToFullWords());
}
