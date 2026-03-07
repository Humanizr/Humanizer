namespace pt_BR.Bytes;

[UseCulture("pt-BR")]
public class ByteSizeExtensionsTests
{
    [Theory]
    [InlineData(2, null, "2 TB")]
    [InlineData(2, "GB", "2048 GB")]
    [InlineData(2.123, "#.#", "2,1 TB")]
    public void HumanizesTerabytes(double input, string? format, string expectedValue) =>
        Assert.Equal(expectedValue, input.Terabytes().Humanize(format));

    [Theory]
    [InlineData(0, null, "0 bit")]
    [InlineData(0, "GB", "0 GB")]
    [InlineData(2, null, "2 GB")]
    [InlineData(2, "MB", "2048 MB")]
    [InlineData(2.123, "#.##", "2,12 GB")]
    public void HumanizesGigabytes(double input, string? format, string expectedValue) =>
        Assert.Equal(expectedValue, input.Gigabytes().Humanize(format));

    [Theory]
    [InlineData(0, null, "0 bit")]
    [InlineData(0, "MB", "0 MB")]
    [InlineData(2, null, "2 MB")]
    [InlineData(2, "KB", "2048 KB")]
    [InlineData(2.123, "#", "2 MB")]
    public void HumanizesMegabytes(double input, string? format, string expectedValue) =>
        Assert.Equal(expectedValue, input.Megabytes().Humanize(format));

    [Theory]
    [InlineData(0, null, "0 bit")]
    [InlineData(0, "KB", "0 KB")]
    [InlineData(2, null, "2 KB")]
    [InlineData(2, "B", "2048 B")]
    [InlineData(2.123, "#.####", "2,123 KB")]
    public void HumanizesKilobytes(double input, string? format, string expectedValue) =>
        Assert.Equal(expectedValue, input.Kilobytes().Humanize(format));

    [Theory]
    [InlineData(0, null, "0 bit")]
    [InlineData(0, "#.##", "0 bit")]
    [InlineData(0, "#.## B", "0 B")]
    [InlineData(0, "B", "0 B")]
    [InlineData(2, null, "2 B")]
    [InlineData(2000, "KB", "1,95 KB")]
    [InlineData(2123, "#.##", "2,07 KB")]
    [InlineData(10000000, "KB", "9765,63 KB")]
    [InlineData(10000000, "#,##0 KB", "9.766 KB")]
    [InlineData(10000000, "#,##0.# KB", "9.765,6 KB")]
    public void HumanizesBytes(double input, string? format, string expectedValue)
    {
        expectedValue = expectedValue.Replace(".", NumberFormatInfo.CurrentInfo.NumberGroupSeparator);
        Assert.Equal(
            expectedValue,
            input
                .Bytes()
                .Humanize(format));
    }

    [Theory]
    [InlineData(0, null, "0 bit")]
    [InlineData(0, "b", "0 bit")]
    [InlineData(2, null, "2 bit")]
    [InlineData(12, "B", "1,5 B")]
    [InlineData(10000, "#.# KB", "1,2 KB")]
    public void HumanizesBits(long input, string? format, string expectedValue) =>
        Assert.Equal(expectedValue, input.Bits().Humanize(format));
}
