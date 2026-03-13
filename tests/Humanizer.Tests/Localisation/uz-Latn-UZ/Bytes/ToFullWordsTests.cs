namespace uzLatn.Bytes;

[UseCulture("uz-Latn-UZ")]
public class ToFullWordsTests
{
    [Fact]
    public void ReturnsSingularByte() =>
        Assert.Equal("1 bayt", ByteSize.FromBytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralBytes() =>
        Assert.Equal("10 bayt", ByteSize.FromBytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularGigabyte() =>
        Assert.Equal("1 gigabayt", ByteSize.FromGigabytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralGigabytes() =>
        Assert.Equal("10 gigabayt", ByteSize.FromGigabytes(10).ToFullWords());

    [Theory]
    [InlineData(229376, "B", "229376 bayt")]
    [InlineData(229376, "# KB", "224 kilobayt")]
    public void ToFullWordsFormatted(double input, string format, string expectedValue) =>
        Assert.Equal(expectedValue, ByteSize.FromBytes(input).ToFullWords(format));
}
