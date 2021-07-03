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

using Humanizer.Bytes;
using Xunit;

namespace Humanizer.Tests.Bytes
{
    [UseCulture("en")]
    public class ToFullWordsTests
    {
        [Fact]
        public void ReturnsSingularBit()
        {
            Assert.Equal("1 bit", ByteSize.FromBits(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralBits()
        {
            Assert.Equal("2 bits", ByteSize.FromBits(2).ToFullWords());
        }

        [Fact]
        public void ReturnsSingularByte()
        {
            Assert.Equal("1 byte", ByteSize.FromBytes(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralBytes()
        {
            Assert.Equal("10 bytes", ByteSize.FromBytes(10).ToFullWords());
        }

        [Fact]
        public void ReturnsSingularKiloByte()
        {
            Assert.Equal("1 kilobyte", ByteSize.FromKilobytes(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralKilobytes()
        {
            Assert.Equal("10 kilobytes", ByteSize.FromKilobytes(10).ToFullWords());
        }

        [Fact]
        public void ReturnsSingularMegabyte()
        {
            Assert.Equal("1 megabyte", ByteSize.FromMegabytes(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralMegabytes()
        {
            Assert.Equal("10 megabytes", ByteSize.FromMegabytes(10).ToFullWords());
        }

        [Fact]
        public void ReturnsSingularGigabyte()
        {
            Assert.Equal("1 gigabyte", ByteSize.FromGigabytes(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralGigabytes()
        {
            Assert.Equal("10 gigabytes", ByteSize.FromGigabytes(10).ToFullWords());
        }

        [Fact]
        public void ReturnsSingularTerabyte()
        {
            Assert.Equal("1 terabyte", ByteSize.FromTerabytes(1).ToFullWords());
        }

        [Fact]
        public void ReturnsPluralTerabytes()
        {
            Assert.Equal("10 terabytes", ByteSize.FromTerabytes(10).ToFullWords());
        }

        [Theory]
        [InlineData(229376, "B", "229376 bytes")]
        [InlineData(229376, "# KB", "224 kilobytes")]
        public void ToFullWordsFormatted(double input, string format, string expectedValue)
        {
            Assert.Equal(expectedValue, ByteSize.FromBytes(input).ToFullWords(format));
        }
    }
}
