namespace Humanizer.Tests.Localisation.@is.Bytes
{
    [UseCulture("is")]
    public class ByteSizeExtensionsTests
    {
        [Theory]
        [InlineData(2, null, "2 TB")]
        [InlineData(2, "GB", "2000 GB")]
        [InlineData(2.123, "#.#", "2,1 TB")]
        public void HumanizesTerabytes(double input, string format, string expectedValue) =>
            Assert.Equal(expectedValue, input.Terabytes().Humanize(format));

        [Theory]
        [InlineData(0, null, "0 b")]
        [InlineData(0, "GB", "0 GB")]
        [InlineData(2, null, "2 GB")]
        [InlineData(2, "MB", "2000 MB")]
        [InlineData(2.123, "#.##", "2,12 GB")]
        public void HumanizesGigabytes(double input, string format, string expectedValue) =>
            Assert.Equal(expectedValue, input.Gigabytes().Humanize(format));

        [Theory]
        [InlineData(0, null, "0 b")]
        [InlineData(0, "MB", "0 MB")]
        [InlineData(2, null, "2 MB")]
        [InlineData(2, "KB", "2000 kB")]
        [InlineData(2.123, "#", "2 MB")]
        public void HumanizesMegabytes(double input, string format, string expectedValue) =>
            Assert.Equal(expectedValue, input.Megabytes().Humanize(format));

        [Theory]
        [InlineData(0, null, "0 b")]
        [InlineData(0, "KB", "0 kB")]
        [InlineData(2, null, "2 kB")]
        [InlineData(2, "B", "2000 B")]
        [InlineData(2.123, "#.####", "2,123 kB")]
        public void HumanizesKilobytes(double input, string format, string expectedValue) =>
            Assert.Equal(expectedValue, input.Kilobytes().Humanize(format));

        [Theory]
        [InlineData(0, null, "0 b")]
        [InlineData(0, "#.##", "0 b")]
        [InlineData(0, "#.## B", "0 B")]
        [InlineData(0, "B", "0 B")]
        [InlineData(2, null, "2 B")]
        [InlineData(1950, "KB", "1,95 kB")]
        [InlineData(2123, "#.##", "2,12 kB")]
        [InlineData(9765630, "KB", "9765,63 kB")]
        [InlineData(9765630, "#,##0 KB", "9.766 kB")]
        [InlineData(9765630, "#,##0.# KB", "9.765,6 kB")]
        public void HumanizesBytes(double input, string format, string expectedValue) =>
            Assert.Equal(expectedValue, input.Bytes().Humanize(format));

        [Theory]
        [InlineData(0, null, "0 b")]
        [InlineData(0, "b", "0 b")]
        [InlineData(2, null, "2 b")]
        [InlineData(12, "B", "1,5 B")]
        [InlineData(10000, "#.# KB", "1,3 kB")]
        [InlineData(10000, "#.# KiB", "1,2 KiB")]
        public void HumanizesBits(long input, string format, string expectedValue) =>
            Assert.Equal(expectedValue, input.Bits().Humanize(format));
    }
}
