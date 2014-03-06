using Humanizer.ByteSizez;
using Xunit;

namespace Humanizer.Tests.Bytes
{
    public class ToStringMethod
    {
        [Fact]
        public void ReturnsLargestMetricSuffix()
        {
            // Arrange
            var b = ByteSize.FromKiloBytes(10.5);

            // Act
            var result = b.ToString();

            // Assert
            Assert.Equal("10.5 KB", result);
        }

        [Fact]
        public void ReturnsDefaultNumberFormat()
        {
            // Arrange
            var b = ByteSize.FromKiloBytes(10.5);

            // Act
            var result = b.ToString("KB");

            // Assert
            Assert.Equal("10.5 KB", result);
        }

        [Fact]
        public void ReturnsProvidedNumberFormat()
        {
            // Arrange
            var b = ByteSize.FromKiloBytes(10.1234);

            // Act
            var result = b.ToString("#.#### KB");

            // Assert
            Assert.Equal("10.1234 KB", result);
        }

        [Fact]
        public void ReturnsBits()
        {
            // Arrange
            var b = ByteSize.FromBits(10);

            // Act
            var result = b.ToString("##.#### b");

            // Assert
            Assert.Equal("10 b", result);
        }

        [Fact]
        public void ReturnsBytes()
        {
            // Arrange
            var b = ByteSize.FromBytes(10);

            // Act
            var result = b.ToString("##.#### B");

            // Assert
            Assert.Equal("10 B", result);
        }

        [Fact]
        public void ReturnsKiloBytes()
        {
            // Arrange
            var b = ByteSize.FromKiloBytes(10);

            // Act
            var result = b.ToString("##.#### KB");

            // Assert
            Assert.Equal("10 KB", result);
        }

        [Fact]
        public void ReturnsMegaBytes()
        {
            // Arrange
            var b = ByteSize.FromMegaBytes(10);

            // Act
            var result = b.ToString("##.#### MB");

            // Assert
            Assert.Equal("10 MB", result);
        }

        [Fact]
        public void ReturnsGigaBytes()
        {
            // Arrange
            var b = ByteSize.FromGigaBytes(10);

            // Act
            var result = b.ToString("##.#### GB");

            // Assert
            Assert.Equal("10 GB", result);
        }

        [Fact]
        public void ReturnsTeraBytes()
        {
            // Arrange
            var b = ByteSize.FromTeraBytes(10);

            // Act
            var result = b.ToString("##.#### TB");

            // Assert
            Assert.Equal("10 TB", result);
        }

        [Fact]
        public void ReturnsSelectedFormat()
        {
            // Arrange
            var b = ByteSize.FromTeraBytes(10);

            // Act
            var result = b.ToString("0.0 TB");

            // Assert
            Assert.Equal("10.0 TB", result);
        }

        [Fact]
        public void ReturnsLargestMetricPrefixLargerThanZero()
        {
            // Arrange
            var b = ByteSize.FromMegaBytes(.5);

            // Act
            var result = b.ToString("#.#");

            // Assert
            Assert.Equal("512 KB", result);
        }

        [Fact]
        public void ReturnsLargestMetricPrefixLargerThanZeroForNegativeValues()
        {
            // Arrange
            var b = ByteSize.FromMegaBytes(-.5);

            // Act
            var result = b.ToString("#.#");

            // Assert
            Assert.Equal("-512 KB", result);
        }
    }
}
