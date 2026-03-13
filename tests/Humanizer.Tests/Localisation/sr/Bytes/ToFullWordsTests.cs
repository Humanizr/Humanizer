namespace sr.Bytes;

[UseCulture("sr")]
public class ToFullWordsTests
{
    [Fact]
    public void ReturnsSingularByte() =>
        Assert.Equal("1 бајт", ByteSize.FromBytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralBytes() =>
        Assert.Equal("10 бајтова", ByteSize.FromBytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularGigabyte() =>
        Assert.Equal("1 гигабајт", ByteSize.FromGigabytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralGigabytes() =>
        Assert.Equal("10 гигабајтова", ByteSize.FromGigabytes(10).ToFullWords());
}
