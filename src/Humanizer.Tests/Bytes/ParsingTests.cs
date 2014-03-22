using System;
using Humanizer.Bytes;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Bytes
{
    public class ParsingTests
    {
        [Fact]
        public void Parse()
        {
            Assert.Equal(ByteSize.FromKilobytes(1020), ByteSize.Parse("1020KB"));
        }

        [Fact]
        public void TryParse()
        {
            ByteSize resultByteSize;
            var resultBool = ByteSize.TryParse("1020KB", out resultByteSize);

            Assert.True(resultBool);
            Assert.Equal(ByteSize.FromKilobytes(1020), resultByteSize);
        }

        [Fact]
        public void ParseDecimalMegabytes()
        {
            Assert.Equal(ByteSize.FromMegabytes(100.5), ByteSize.Parse("100.5MB"));
        }

        [Theory]
        [InlineData("Unexpected Value")]
        [InlineData("1000")]
        [InlineData("KB")]
        public void TryParseReturnsFalseOnBadValue(string input)
        {
            ByteSize resultByteSize;
            var resultBool = ByteSize.TryParse(input, out resultByteSize);

            Assert.False(resultBool);
            Assert.Equal(new ByteSize(), resultByteSize);
        }

        [Fact]
        public void TryParseWorksWithLotsOfSpaces()
        {
            Assert.Equal(ByteSize.FromKilobytes(100), ByteSize.Parse(" 100 KB "));
        }

        [Fact]
        public void ParseThrowsOnPartialBits()
        {
            Assert.Throws<FormatException>(() => { ByteSize.Parse("10.5b"); });
        }

        [Fact]
        public void ParseThrowsOnInvalid()
        {
            Assert.Throws<FormatException>(() => { ByteSize.Parse("Unexpected Value"); });
        }

        [Fact]
        public void ParseThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => { ByteSize.Parse(null); });
        }

        [Fact]
        public void ParseBits()
        {
            Assert.Equal(ByteSize.FromBits(1), ByteSize.Parse("1b"));
        }

        [Fact]
        public void ParseBytes()
        {
            Assert.Equal(ByteSize.FromBytes(1), ByteSize.Parse("1B"));
        }

        [Fact]
        public void ParseKilobytes()
        {
            Assert.Equal(ByteSize.FromKilobytes(1020), ByteSize.Parse("1020KB"));
        }

        [Fact]
        public void ParseMegabytes()
        {
            Assert.Equal(ByteSize.FromMegabytes(1000), ByteSize.Parse("1000MB"));
        }

        [Fact]
        public void ParseGigabytes()
        {
            Assert.Equal(ByteSize.FromGigabytes(805), ByteSize.Parse("805GB"));
        }

        [Fact]
        public void ParseTerabytes()
        {
            Assert.Equal(ByteSize.FromTerabytes(100), ByteSize.Parse("100TB"));
        }
    }
}
