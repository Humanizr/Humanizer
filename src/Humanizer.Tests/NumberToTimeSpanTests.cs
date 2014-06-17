using System;
using Xunit;

namespace Humanizer.Tests
{
    public class NumberToTimeSpanTests
    {
        [Fact]
        public void ByteToMilliseconds()
        {
            const byte number = 1;
            Assert.Equal(new TimeSpan(0, 0, 0, 0, 1), number.Milliseconds());
        }

        [Fact]
        public void SbyteToMilliseconds()
        {
            const sbyte number = 1;
            Assert.Equal(new TimeSpan(0, 0, 0, 0, 1), number.Milliseconds());
        }

        [Fact]
        public void ShortToMilliseconds()
        {
            const short number = 1;
            Assert.Equal(new TimeSpan(0, 0, 0, 0, 1), number.Milliseconds());
        }

        [Fact]
        public void UshortToMilliseconds()
        {
            const ushort number = 1;
            Assert.Equal(new TimeSpan(0, 0, 0, 0, 1), number.Milliseconds());
        }

        [Fact]
        public void IntToMilliseconds()
        {
            const int number = 1;
            Assert.Equal(new TimeSpan(0, 0, 0, 0, 1), number.Milliseconds());
        }

        [Fact]
        public void UintToMilliseconds()
        {
            const uint number = 1;
            Assert.Equal(new TimeSpan(0, 0, 0, 0, 1), number.Milliseconds());
        }

        [Fact]
        public void LongToMilliseconds()
        {
            const long number = 1;
            Assert.Equal(new TimeSpan(0, 0, 0, 0, 1), number.Milliseconds());
        }

        [Fact]
        public void UlongToMilliseconds()
        {
            const ulong number = 1;
            Assert.Equal(new TimeSpan(0, 0, 0, 0, 1), number.Milliseconds());
        }

        [Fact]
        public void ByteToMinutes()
        {
            const byte number = 2;
            Assert.Equal(new TimeSpan(0, 0, 2, 0), number.Minutes());
        }

        [Fact]
        public void SbyteToMinutes()
        {
            const sbyte number = 2;
            Assert.Equal(new TimeSpan(0, 0, 2, 0), number.Minutes());
        }

        [Fact]
        public void ShortToMinutes()
        {
            const short number = 2;
            Assert.Equal(new TimeSpan(0, 0, 2, 0), number.Minutes());
        }

        [Fact]
        public void UshortToMinutes()
        {
            const ushort number = 2;
            Assert.Equal(new TimeSpan(0, 0, 2, 0), number.Minutes());
        }

        [Fact]
        public void IntToMinutes()
        {
            const int number = 2;
            Assert.Equal(new TimeSpan(0, 0, 2, 0), number.Minutes());
        }

        [Fact]
        public void UintToMinutes()
        {
            const uint number = 2;
            Assert.Equal(new TimeSpan(0, 0, 2, 0), number.Minutes());
        }

        [Fact]
        public void LongToMinutes()
        {
            const long number = 2;
            Assert.Equal(new TimeSpan(0, 0, 2, 0), number.Minutes());
        }

        [Fact]
        public void UlongToMinutes()
        {
            const ulong number = 2;
            Assert.Equal(new TimeSpan(0, 0, 2, 0), number.Minutes());
        }

        [Fact]
        public void ByteToSeconds()
        {
            const byte number = 3;
            Assert.Equal(new TimeSpan(0, 0, 0, 3), number.Seconds());
        }

        [Fact]
        public void SbyteToSeconds()
        {
            const sbyte number = 3;
            Assert.Equal(new TimeSpan(0, 0, 0, 3), number.Seconds());
        }

        [Fact]
        public void ShortToSeconds()
        {
            const short number = 3;
            Assert.Equal(new TimeSpan(0, 0, 0, 3), number.Seconds());
        }

        [Fact]
        public void UshortToSeconds()
        {
            const ushort number = 3;
            Assert.Equal(new TimeSpan(0, 0, 0, 3), number.Seconds());
        }

        [Fact]
        public void IntToSeconds()
        {
            const int number = 3;
            Assert.Equal(new TimeSpan(0, 0, 0, 3), number.Seconds());
        }

        [Fact]
        public void UintToSeconds()
        {
            const uint number = 3;
            Assert.Equal(new TimeSpan(0, 0, 0, 3), number.Seconds());
        }

        [Fact]
        public void LongToSeconds()
        {
            const long number = 3;
            Assert.Equal(new TimeSpan(0, 0, 0, 3), number.Seconds());
        }

        [Fact]
        public void UlongToSeconds()
        {
            const ulong number = 3;
            Assert.Equal(new TimeSpan(0, 0, 0, 3), number.Seconds());
        }

        [Fact]
        public void ByteToHours()
        {
            const byte number = 4;
            Assert.Equal(new TimeSpan(0, 4, 0, 0), number.Hours());
        }

        [Fact]
        public void SbyteToHours()
        {
            const sbyte number = 4;
            Assert.Equal(new TimeSpan(0, 4, 0, 0), number.Hours());
        }

        [Fact]
        public void ShortToHours()
        {
            const short number = 4;
            Assert.Equal(new TimeSpan(0, 4, 0, 0), number.Hours());
        }

        [Fact]
        public void UshortToHours()
        {
            const ushort number = 4;
            Assert.Equal(new TimeSpan(0, 4, 0, 0), number.Hours());
        }

        [Fact]
        public void IntToHours()
        {
            const int number = 4;
            Assert.Equal(new TimeSpan(0, 4, 0, 0), number.Hours());
        }

        [Fact]
        public void UintToHours()
        {
            const uint number = 4;
            Assert.Equal(new TimeSpan(0, 4, 0, 0), number.Hours());
        }

        [Fact]
        public void LongToHours()
        {
            const long number = 4;
            Assert.Equal(new TimeSpan(0, 4, 0, 0), number.Hours());
        }

        [Fact]
        public void UlongToHours()
        {
            const ulong number = 4;
            Assert.Equal(new TimeSpan(0, 4, 0, 0), number.Hours());
        }

        [Fact]
        public void ByteToDays()
        {
            const byte number = 5;
            Assert.Equal(new TimeSpan(5, 0, 0, 0), number.Days());
        }

        [Fact]
        public void SbyteToDays()
        {
            const sbyte number = 5;
            Assert.Equal(new TimeSpan(5, 0, 0, 0), number.Days());
        }

        [Fact]
        public void ShortToDays()
        {
            const short number = 5;
            Assert.Equal(new TimeSpan(5, 0, 0, 0), number.Days());
        }

        [Fact]
        public void UshortToDays()
        {
            const ushort number = 5;
            Assert.Equal(new TimeSpan(5, 0, 0, 0), number.Days());
        }

        [Fact]
        public void IntToDays()
        {
            const int number = 5;
            Assert.Equal(new TimeSpan(5, 0, 0, 0), number.Days());
        }

        [Fact]
        public void UintToDays()
        {
            const uint number = 5;
            Assert.Equal(new TimeSpan(5, 0, 0, 0), number.Days());
        }

        [Fact]
        public void LongToDays()
        {
            const long number = 5;
            Assert.Equal(new TimeSpan(5, 0, 0, 0), number.Days());
        }

        [Fact]
        public void UlongToDays()
        {
            const ulong number = 5;
            Assert.Equal(new TimeSpan(5, 0, 0, 0), number.Days());
        }

        [Fact]
        public void ByteToWeeks()
        {
            const byte number = 6;
            var now = DateTime.Now;
            Assert.Equal(now.AddDays(42), now.Add(number.Weeks()));
        }

        [Fact]
        public void SbyteToWeeks()
        {
            const sbyte number = 6;
            var now = DateTime.Now;
            Assert.Equal(now.AddDays(42), now.Add(number.Weeks()));
        }

        [Fact]
        public void ShortToWeeks()
        {
            const short number = 6;
            var now = DateTime.Now;
            Assert.Equal(now.AddDays(42), now.Add(number.Weeks()));
        }

        [Fact]
        public void UshortToWeeks()
        {
            const ushort number = 6;
            var now = DateTime.Now;
            Assert.Equal(now.AddDays(42), now.Add(number.Weeks()));
        }

        [Fact]
        public void IntToWeeks()
        {
            const int number = 6;
            var now = DateTime.Now;
            Assert.Equal(now.AddDays(42), now.Add(number.Weeks()));
        }

        [Fact]
        public void UintToWeeks()
        {
            const uint number = 6;
            var now = DateTime.Now;
            Assert.Equal(now.AddDays(42), now.Add(number.Weeks()));
        }

        [Fact]
        public void LongToWeeks()
        {
            const long number = 6;
            var now = DateTime.Now;
            Assert.Equal(now.AddDays(42), now.Add(number.Weeks()));
        }

        [Fact]
        public void UlongToWeeks()
        {
            const ulong number = 6;
            var now = DateTime.Now;
            Assert.Equal(now.AddDays(42), now.Add(number.Weeks()));
        }
    }
}
