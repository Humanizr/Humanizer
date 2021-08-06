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
    public class ToStringTests
    {
        [Fact]
        public void ReturnsLargestMetricSuffix()
        {
            Assert.Equal("10.5 KB", ByteSize.FromKilobytes(10.5).ToString());
        }

        [Fact]
        public void ReturnsDefaultNumberFormat()
        {
            Assert.Equal("10.5 KB", ByteSize.FromKilobytes(10.501).ToString());
            Assert.Equal("10.5 KB", ByteSize.FromKilobytes(10.5).ToString("KB"));
        }

        [Fact]
        public void ReturnsProvidedNumberFormat()
        {
            Assert.Equal("10.1234 KB", ByteSize.FromKilobytes(10.1234).ToString("#.#### KB"));
        }

        [Fact]
        public void ReturnsBits()
        {
            Assert.Equal("10 b", ByteSize.FromBits(10).ToString("##.#### b"));
        }

        [Fact]
        public void ReturnsBytes()
        {
            Assert.Equal("10 B", ByteSize.FromBytes(10).ToString("##.#### B"));
        }

        [Fact]
        public void ReturnsKilobytes()
        {
            Assert.Equal("10 KB", ByteSize.FromKilobytes(10).ToString("##.#### KB"));
        }

        [Fact]
        public void ReturnsMegabytes()
        {
            Assert.Equal("10 MB", ByteSize.FromMegabytes(10).ToString("##.#### MB"));
        }

        [Fact]
        public void ReturnsGigabytes()
        {
            Assert.Equal("10 GB", ByteSize.FromGigabytes(10).ToString("##.#### GB"));
        }

        [Fact]
        public void ReturnsTerabytes()
        {
            Assert.Equal("10 TB", ByteSize.FromTerabytes(10).ToString("##.#### TB"));
        }

        [Fact]
        public void ReturnsSelectedFormat()
        {
            Assert.Equal("10.0 TB", ByteSize.FromTerabytes(10).ToString("0.0 TB"));
        }

        [Fact]
        public void ReturnsLargestMetricPrefixLargerThanZero()
        {
            Assert.Equal("512 KB", ByteSize.FromMegabytes(.5).ToString("#.#"));
        }

        [Fact]
        public void ReturnsLargestMetricPrefixLargerThanZeroForNegativeValues()
        {
            Assert.Equal("-512 KB", ByteSize.FromMegabytes(-.5).ToString("#.#"));
        }

        [Fact]
        public void ReturnsBytesViaGeneralFormat()
        {
            Assert.Equal("10 B", $"{ByteSize.FromBytes(10)}");
        }
    }
}
