//The MIT License (MIT)

//Copyright (c) 2013-2014 Omar Khudeira (http://omar.io)

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.

using System;

using Humanizer.Bytes;
using Xunit;

namespace Humanizer.Tests.Bytes
{
    [UseCulture("en")]
    public class ParsingTests
    {

        [Fact]
        public void Parse()
        {
            Assert.Equal(ByteSize.FromKilobytes(1020), ByteSize.Parse("1020kB"));
            Assert.Equal(ByteSize.FromKibibytes(1020), ByteSize.Parse("1020KiB"));
        }
        [Theory]
        [InlineData("1020kB")]
        [InlineData("1020 kB")]
        [InlineData("1020 KB")]
        [InlineData("1020 kB ")]
        [InlineData(" 1020 kB")]
        public void TryParseKilobytes(string rawValue)
        {
            var parseSuccess = ByteSize.TryParse(rawValue, out ByteSize resultByteSize);

            Assert.True(parseSuccess);
            Assert.Equal(ByteSize.FromKilobytes(1020), resultByteSize);
        }

        [Theory]
        [InlineData("1020kiB")]
        [InlineData("1020 kiB")]
        [InlineData("1020 KiB")]
        [InlineData("1020 kiB ")]
        [InlineData(" 1020 kiB")]
        public void TryParseKibibytes(string rawValue)
        {
            var parseSuccess = ByteSize.TryParse(rawValue, out ByteSize resultByteSize);

            Assert.True(parseSuccess);
            Assert.Equal(ByteSize.FromKibibytes(1020), resultByteSize);
        }

        [Fact]
        public void ParseDecimalMegabytes()
        {
            Assert.Equal(ByteSize.FromMegabytes(100.5), ByteSize.Parse("100.5MB"));
        }

        [Theory]
        [InlineData("Unexpected Value")]
        [InlineData("1000")]
        [InlineData("kb")]
        [InlineData("10-0kb")]
        [InlineData("10-kb")]
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
            Assert.Equal(ByteSize.FromKilobytes(100), ByteSize.Parse(" 100 kB "));
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
            Assert.Throws<FormatException>(() => { ByteSize.Parse(null); });
        }

        [Fact]
        public void ParseThrowsOnEmptyString()
        {
            Assert.Throws<FormatException>(() => { ByteSize.Parse(""); });
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
            Assert.Equal(ByteSize.FromKilobytes(1020), ByteSize.Parse("1020kB"));
        }

        [Fact]
        public void ParseKibibytes()
        {
            Assert.Equal(ByteSize.FromKibibytes(1020), ByteSize.Parse("1020kiB"));
        }

        [Fact]
        public void ParseMegabytes()
        {
            Assert.Equal(ByteSize.FromMegabytes(1000), ByteSize.Parse("1000MB"));
        }
        
        [Fact]
        public void ParseMebibytes()
        {
            Assert.Equal(ByteSize.FromMebibytes(1000), ByteSize.Parse("1000MiB"));
        }

        [Fact]
        public void ParseGigabytes()
        {
            Assert.Equal(ByteSize.FromGigabytes(805), ByteSize.Parse("805GB"));
        }

        [Fact]
        public void ParseGibibytes()
        {
            Assert.Equal(ByteSize.FromGibibytes(805), ByteSize.Parse("805GiB"));
        }

        [Fact]
        public void ParseTerabytes()
        {
            Assert.Equal(ByteSize.FromTerabytes(100), ByteSize.Parse("100TB"));
        }

        [Fact]
        public void ParseTebibytes()
        {
            Assert.Equal(ByteSize.FromTebibytes(100), ByteSize.Parse("100TiB"));
        }


        [Theory]
        [InlineData(-1024, "-1KB")]
        [InlineData(-1000, "-1KiB")]
        public void ParseNegativeBytes(int expectedBytes, string bytesToParse)
        {
            Assert.Equal(ByteSize.FromBytes(expectedBytes), ByteSize.Parse(bytesToParse));
        }
    }
}
