using System;
using Humanizer.Bytes;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Bytes
{
    public class ByteRateTests : AmbientCulture
    {
        public ByteRateTests() : base("en") { }
        
        [Theory]
        [InlineData(400, 1, "400 B/s")]
        [InlineData(4 * 1024, 1, "4 KB/s")]
        [InlineData(4 * 1024 * 1024, 1, "4 MB/s")]
        [InlineData(4 * 2 * 1024 * 1024, 2, "4 MB/s")]
        [InlineData(4 * 1024, 0.1, "40 KB/s")]
        [InlineData(15 * 60 * 1024 * 1024, 60, "15 MB/s")]
        public void HumanizesRates(long inputBytes, double perSeconds, string expectedValue)
        {
            var size = new ByteSize(inputBytes);
            var interval = TimeSpan.FromSeconds(perSeconds);

            var rate = size.Per(interval).ToRatePerSecond();

            Assert.Equal(expectedValue, rate);
        }
    }
}
