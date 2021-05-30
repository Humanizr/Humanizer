using Xunit;

namespace Humanizer.Tests.Localisation.fr.Bytes
{
    [UseCulture("fr-FR")]
    public class ByteSizeExtensionsTests
    {
        [Theory]
        [InlineData(2, null, "2 To")]
        [InlineData(2, "GB", "2048 Go")]
        [InlineData(2.123, "#.#", "2,1 To")]
        public void HumanizesTerabytes(double input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, input.Terabytes().Humanize(format));
        }

        [Theory]
        [InlineData(0, null, "0 b")]
        [InlineData(0, "GB", "0 Go")]
        [InlineData(2, null, "2 Go")]
        [InlineData(2, "MB", "2048 Mo")]
        [InlineData(2.123, "#.##", "2,12 Go")]
        public void HumanizesGigabytes(double input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, input.Gigabytes().Humanize(format));
        }

        [Theory]
        [InlineData(0, null, "0 b")]
        [InlineData(0, "MB", "0 Mo")]
        [InlineData(2, null, "2 Mo")]
        [InlineData(2, "KB", "2048 Ko")]
        [InlineData(2.123, "#", "2 Mo")]
        public void HumanizesMegabytes(double input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, input.Megabytes().Humanize(format));
        }

        [Theory]
        [InlineData(0, null, "0 b")]
        [InlineData(0, "KB", "0 Ko")]
        [InlineData(2, null, "2 Ko")]
        [InlineData(2, "B", "2048 o")]
        [InlineData(2.123, "#.####", "2,123 Ko")]
        public void HumanizesKilobytes(double input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, input.Kilobytes().Humanize(format));
        }

        [Theory]
        [InlineData(0, null, "0 b")]
        [InlineData(0, "#.##", "0 b")]
        [InlineData(0, "#.## B", "0 o")]
        [InlineData(0, "B", "0 o")]
        [InlineData(2, null, "2 o")]
        [InlineData(2000, "KB", "1,95 Ko")]
        [InlineData(2123, "#.##", "2,07 Ko")]
        [InlineData(10000000, "KB", "9765,63 Ko")]
        [InlineData(10000000, "#,##0 KB", "9 766 Ko")]
        [InlineData(10000000, "#,##0.# KB", "9 765,6 Ko")]
        public void HumanizesBytes(double input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, input.Bytes().Humanize(format));
        }

        [Theory]
        [InlineData(0, null, "0 b")]
        [InlineData(0, "b", "0 b")]
        [InlineData(2, null, "2 b")]
        [InlineData(12, "B", "1,5 o")]
        [InlineData(10000, "#.# KB", "1,2 Ko")]
        public void HumanizesBits(long input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, input.Bits().Humanize(format));
        }
    }
}
