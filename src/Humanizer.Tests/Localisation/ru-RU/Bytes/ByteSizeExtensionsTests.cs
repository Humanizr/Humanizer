namespace ruRU.Bytes;

[UseCulture("ru-RU")]
public class ByteSizeExtensionsTests
{
    [Theory]
    [InlineData(2, null, "2 ТБ")]
    [InlineData(2, "GB", "2048 ГБ")]
    [InlineData(2.1, null, "2,1 ТБ")]
    [InlineData(2.123, "#.#", "2,1 ТБ")]
    public void HumanizesTerabytes(double input, string? format, string expectedValue) =>
        Assert.Equal(expectedValue, input.Terabytes().Humanize(format));

    [Theory]
    [InlineData(0, null, "0 бит")]
    [InlineData(0, "GB", "0 ГБ")]
    [InlineData(2, null, "2 ГБ")]
    [InlineData(2, "MB", "2048 МБ")]
    [InlineData(2.123, "#.##", "2,12 ГБ")]
    public void HumanizesGigabytes(double input, string? format, string expectedValue) =>
        Assert.Equal(expectedValue, input.Gigabytes().Humanize(format));

    [Theory]
    [InlineData(0, null, "0 бит")]
    [InlineData(0, "MB", "0 МБ")]
    [InlineData(2, null, "2 МБ")]
    [InlineData(2, "KB", "2048 КБ")]
    [InlineData(2.123, "#", "2 МБ")]
    public void HumanizesMegabytes(double input, string? format, string expectedValue) =>
        Assert.Equal(expectedValue, input.Megabytes().Humanize(format));

    [Theory]
    [InlineData(0, null, "0 бит")]
    [InlineData(0, "KB", "0 КБ")]
    [InlineData(2, null, "2 КБ")]
    [InlineData(2, "B", "2048 Б")]
    [InlineData(2.123, "#.####", "2,123 КБ")]
    public void HumanizesKilobytes(double input, string? format, string expectedValue) =>
        Assert.Equal(expectedValue, input.Kilobytes().Humanize(format));

    [Theory]
    [InlineData(0, null, "0 бит")]
    [InlineData(0, "#.##", "0 бит")]
    [InlineData(0, "#.## B", "0 Б")]
    [InlineData(0, "B", "0 Б")]
    [InlineData(2, null, "2 Б")]
    [InlineData(2000, "KB", "1,95 КБ")]
    [InlineData(2123, "#.##", "2,07 КБ")]
    [InlineData(10000000, "KB", "9765,63 КБ")]
    [InlineData(10000000, "#,##0 KB", "9 766 КБ")]
    [InlineData(10000000, "#,##0.# KB", "9 765,6 КБ")]
    public void HumanizesBytes(double input, string? format, string expectedValue) =>
        Assert.Equal(expectedValue, input.Bytes().Humanize(format));

    [Theory]
    [InlineData(0, null, "0 бит")]
    [InlineData(0, "b", "0 бит")]
    [InlineData(2, null, "2 бит")]
    [InlineData(12, "B", "1,5 Б")]
    [InlineData(10000, "#.# KB", "1,2 КБ")]
    public void HumanizesBits(long input, string? format, string expectedValue) =>
        Assert.Equal(expectedValue, input.Bits().Humanize(format));
}