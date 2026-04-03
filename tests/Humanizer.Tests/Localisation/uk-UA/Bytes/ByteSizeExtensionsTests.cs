namespace ukUA.Bytes;

[UseCulture("uk-UA")]
public class ByteSizeExtensionsTests
{
    [Theory]
    [InlineData(0, null, "0 b")]
    [InlineData(0, "b", "0 b")]
    [InlineData(2, null, "2 b")]
    [InlineData(12, "B", "1,5 B")]
    [InlineData(10000, "#.# KB", "1,2 KB")]
    public void HumanizesBits(long input, string? format, string expectedValue) =>
        Assert.Equal(expectedValue, input.Bits().Humanize(format));
}
