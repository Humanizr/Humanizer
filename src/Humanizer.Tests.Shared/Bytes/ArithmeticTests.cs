using Humanizer.Bytes;
using Xunit;

namespace Humanizer.Tests.Bytes
{
    public class ArithmeticTests
    {
        [Fact]
        public void Add()
        {
            var size1 = ByteSize.FromBytes(1);
            var result = size1.Add(size1);

            Assert.Equal(2, result.Bytes);
            Assert.Equal(16, result.Bits);
        }

        [Fact]
        public void AddBits()
        {
            var size = ByteSize.FromBytes(1).AddBits(8);

            Assert.Equal(2, size.Bytes);
            Assert.Equal(16, size.Bits);
        }

        [Fact]
        public void AddBytes()
        {
            var size = ByteSize.FromBytes(1).AddBytes(1);

            Assert.Equal(2, size.Bytes);
            Assert.Equal(16, size.Bits);
        }

        [Fact]
        public void AddKilobytes()
        {
            var size = ByteSize.FromKilobytes(2).AddKilobytes(2);

            Assert.Equal(4 * 1000 * 8, size.Bits);
            Assert.Equal(4 * 1000, size.Bytes);
            Assert.Equal(4, size.Kilobytes);
        }
        [Fact]
        public void AddKibibytes()
        {
            var size = ByteSize.FromKibibytes(2).AddKibibytes(2);

            Assert.Equal(4 * 1024 * 8, size.Bits);
            Assert.Equal(4 * 1024, size.Bytes);
            Assert.Equal(4, size.Kibibytes);
        }

        [Fact]
        public void AddMegabytes()
        {
            var size = ByteSize.FromMegabytes(2).AddMegabytes(2);

            Assert.Equal(4 * 1000 * 1000 * 8, size.Bits);
            Assert.Equal(4 * 1000 * 1000, size.Bytes);
            Assert.Equal(4 * 1000, size.Kilobytes);
            Assert.Equal(4, size.Megabytes);
        }

        [Fact]
        public void AddMebibytes()
        {
            var size = ByteSize.FromMebibytes(2).AddMebibytes(2);

            Assert.Equal(4 * 1024 * 1024 * 8, size.Bits);
            Assert.Equal(4 * 1024 * 1024, size.Bytes);
            Assert.Equal(4 * 1024, size.Kibibytes);
            Assert.Equal(4, size.Mebibytes);
        }

        [Fact]
        public void AddGigabytes()
        {
            var size = ByteSize.FromGigabytes(2).AddGigabytes(2);

            Assert.Equal(4d * 1000 * 1000 * 1000 * 8, size.Bits);
            Assert.Equal(4d * 1000 * 1000 * 1000, size.Bytes);
            Assert.Equal(4d * 1000 * 1000, size.Kilobytes);
            Assert.Equal(4d * 1000, size.Megabytes);
            Assert.Equal(4d, size.Gigabytes);
        }

        [Fact]
        public void AddGibibytes()
        {
            var size = ByteSize.FromGibibytes(2).AddGibibytes(2);

            Assert.Equal(4d * 1024 * 1024 * 1024 * 8, size.Bits);
            Assert.Equal(4d * 1024 * 1024 * 1024, size.Bytes);
            Assert.Equal(4d * 1024 * 1024, size.Kibibytes);
            Assert.Equal(4d * 1024, size.Mebibytes);
            Assert.Equal(4d, size.Gibibytes);
        }

        [Fact]
        public void AddTerabytes()
        {
            var size = ByteSize.FromTerabytes(2).AddTerabytes(2);

            Assert.Equal(4d * 1000 * 1000 * 1000 * 1000 * 8, size.Bits);
            Assert.Equal(4d * 1000 * 1000 * 1000 * 1000, size.Bytes);
            Assert.Equal(4d * 1000 * 1000 * 1000, size.Kilobytes);
            Assert.Equal(4d * 1000 * 1000, size.Megabytes);
            Assert.Equal(4d * 1000, size.Gigabytes);
            Assert.Equal(4d, size.Terabytes);
        }

        [Fact]
        public void AddTebibytes()
        {
            var size = ByteSize.FromTebibytes(2).AddTebibytes(2);

            Assert.Equal(4d * 1024 * 1024 * 1024 * 1024 * 8, size.Bits);
            Assert.Equal(4d * 1024 * 1024 * 1024 * 1024, size.Bytes);
            Assert.Equal(4d * 1024 * 1024 * 1024, size.Kibibytes);
            Assert.Equal(4d * 1024 * 1024, size.Mebibytes);
            Assert.Equal(4d * 1024, size.Gibibytes);
            Assert.Equal(4d, size.Tebibytes);
        }

        [Fact]
        public void Subtract()
        {
            var size = ByteSize.FromBytes(4).Subtract(ByteSize.FromBytes(2));

            Assert.Equal(16, size.Bits);
            Assert.Equal(2, size.Bytes);
        }

        [Fact]
        public void IncrementOperator()
        {
            var size = ByteSize.FromBytes(2);
            size++;

            Assert.Equal(24, size.Bits);
            Assert.Equal(3, size.Bytes);
        }

        [Fact]
        public void MinusOperator()
        {
            var size = ByteSize.FromBytes(2);

            size = -size;

            Assert.Equal(-16, size.Bits);
            Assert.Equal(-2, size.Bytes);
        }

        [Fact]
        public void DecrementOperator()
        {
            var size = ByteSize.FromBytes(2);
            size--;

            Assert.Equal(8, size.Bits);
            Assert.Equal(1, size.Bytes);
        }

        [Fact]
        public void PlusOperator()
        {
            var size1 = ByteSize.FromBytes(1);
            var size2 = ByteSize.FromBytes(1);

            var result = size1 + size2;

            Assert.Equal(2, result.Bytes);
        }
    }
}
