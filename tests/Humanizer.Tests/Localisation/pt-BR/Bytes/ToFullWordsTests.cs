namespace ptBR.Bytes;

[UseCulture("pt-BR")]
public class ToFullWordsTests
{
    [Fact]
    public void ReturnsPluralBytes() =>
        Assert.Equal("10 bytes", ByteSize.FromBytes(10).ToFullWords());

    [Fact]
    public void ReturnsPluralGigabytes() =>
        Assert.Equal("10 gigabytes", ByteSize.FromGigabytes(10).ToFullWords());

    [Theory]
    [InlineData(229376, "B", "229376 bytes")]
    [InlineData(229376, "# KB", "224 kilobytes")]
    public void ToFullWordsFormatted(double input, string format, string expectedValue) =>
        Assert.Equal(expectedValue, ByteSize.FromBytes(input).ToFullWords(format));
}
