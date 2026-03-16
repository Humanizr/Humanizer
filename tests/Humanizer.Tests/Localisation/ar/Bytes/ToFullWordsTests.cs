namespace ar.Bytes;

[UseCulture("ar")]
public class ToFullWordsTests
{
    [Fact]
    public void ReturnsSingularByte() =>
        Assert.Equal("1 بايت", ByteSize.FromBytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralBytes() =>
        Assert.Equal("10 بايت", ByteSize.FromBytes(10).ToFullWords());

    [Fact]
    public void ReturnsSingularKiloByte() =>
        Assert.Equal("1 كيلوبايت", ByteSize.FromKilobytes(1).ToFullWords());

    [Fact]
    public void ReturnsPluralGigabytes() =>
        Assert.Equal("10 جيجابايت", ByteSize.FromGigabytes(10).ToFullWords());

    [Theory]
    [InlineData(229376, "B", "229376 بايت")]
    [InlineData(229376, "# KB", "224 كيلوبايت")]
    public void ToFullWordsFormatted(double input, string format, string expectedValue) =>
        Assert.Equal(expectedValue, ByteSize.FromBytes(input).ToFullWords(format));
}
