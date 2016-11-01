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
    public class CreatingTests
    {
        [Fact]
        public void Constructor()
        {
            var result = new ByteSize(1e12);
            
            Assert.Equal(8e12, result.Bits,3);
            Assert.Equal(1e12, result.Bytes,3);

            Assert.Equal(1e9, result.Kilobytes,3);
            Assert.Equal(1e6, result.Megabytes,3);
            Assert.Equal(1000, result.Gigabytes,3);
            Assert.Equal(1, result.Terabytes,3);

            Assert.Equal(976562500.0, result.Kibibytes, 3);
            Assert.Equal(953674.316, result.Mebibytes, 3);
            Assert.Equal(931.323, result.Gibibytes,3);
            Assert.Equal(0.909, result.Tebibytes,3);
        }

        [Fact]
        public void FromBits()
        {
            var result = ByteSize.FromBits(8);

            Assert.Equal(8, result.Bits);
            Assert.Equal(1, result.Bytes,3);
        }

        [Fact]
        public void FromBytes()
        {
            var result = ByteSize.FromBytes(1.5);

            Assert.Equal(12, result.Bits);
            Assert.Equal(1.5, result.Bytes, 3);
        }

        [Fact]
        public void FromKilobytes()
        {
            var result = ByteSize.FromKilobytes(1.5);

            Assert.Equal(1500, result.Bytes, 3);
            Assert.Equal(1.5, result.Kilobytes, 3);
        }
        [Fact]
        public void FromKibibytes()
        {
            var result = ByteSize.FromKibibytes(1.5);

            Assert.Equal(1536, result.Bytes, 3);
            Assert.Equal(1.5, result.Kibibytes, 3);
            Assert.Equal(1.536, result.Kilobytes, 3);
        }

        [Fact]
        public void FromMegabytes()
        {
            var result = ByteSize.FromMegabytes(1.5);

            Assert.Equal(1500000, result.Bytes, 3);
            Assert.Equal(1.5, result.Megabytes, 3);
        }
        [Fact]
        public void FromMebibytes()
        {
            var result = ByteSize.FromMebibytes(1.5);

            Assert.Equal(1572864, result.Bytes, 3);
            Assert.Equal(1.5, result.Mebibytes, 3);
            Assert.Equal(1.573, result.Megabytes, 3);
        }

        [Fact]
        public void FromGigabytes()
        {
            var result = ByteSize.FromGigabytes(1.5);

            Assert.Equal(1500000000, result.Bytes, 3);
            Assert.Equal(1.5, result.Gigabytes, 3);
        }

        [Fact]
        public void FromGibibytes()
        {
            var result = ByteSize.FromGibibytes(1.5);

            Assert.Equal(1610612736, result.Bytes, 3);
            Assert.Equal(1.5, result.Gibibytes, 3);
            Assert.Equal(1.611, result.Gigabytes, 3);
        }

        [Fact]
        public void FromTerabytes()
        {
            var result = ByteSize.FromTerabytes(1.5);

            Assert.Equal(1500000000000, result.Bytes, 3);
            Assert.Equal(1.5, result.Terabytes, 3);
        }
        [Fact]
        public void FromTebibytes()
        {
            var result = ByteSize.FromTebibytes(1.5);

            Assert.Equal(1649267441664, result.Bytes, 3);
            Assert.Equal(1.5, result.Tebibytes, 3);
            Assert.Equal(1.649, result.Terabytes, 3);
        }
    }
}
