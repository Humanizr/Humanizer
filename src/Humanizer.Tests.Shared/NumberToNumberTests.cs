using Xunit;

namespace Humanizer.Tests
{
    public class NumberToNumberTests
    {
        [Fact]
        public void IntToTens()
        {
            const int number = 1;
            Assert.Equal(10, number.Tens());
        }

        [Fact]
        public void UintToTens()
        {
            const uint number = 1;
            Assert.Equal(10U, number.Tens());
        }

        [Fact]
        public void LongToTens()
        {
            const long number = 1;
            Assert.Equal(10L, number.Tens());
        }

        [Fact]
        public void UlongToTens()
        {
            const ulong number = 1;
            Assert.Equal(10UL, number.Tens());
        }

        [Fact]
        public void DoubleToTens()
        {
            const double number = 1;
            Assert.Equal(10d, number.Tens());
        }

        [Fact]
        public void IntToHundreds()
        {
            const int number = 2;
            Assert.Equal(200, number.Hundreds());
        }

        [Fact]
        public void UintToHundreds()
        {
            const uint number = 2;
            Assert.Equal(200U, number.Hundreds());
        }

        [Fact]
        public void LongToHundreds()
        {
            const long number = 2;
            Assert.Equal(200L, number.Hundreds());
        }

        [Fact]
        public void UlongToHundreds()
        {
            const ulong number = 2;
            Assert.Equal(200UL, number.Hundreds());
        }

        [Fact]
        public void DoubleToHundreds()
        {
            const double number = 2;
            Assert.Equal(200d, number.Hundreds());
        }

        [Fact]
        public void IntToThousands()
        {
            const int number = 3;
            Assert.Equal(3000, number.Thousands());
        }

        [Fact]
        public void UintToThousands()
        {
            const uint number = 3;
            Assert.Equal(3000U, number.Thousands());
        }

        [Fact]
        public void LongToThousands()
        {
            const long number = 3;
            Assert.Equal(3000L, number.Thousands());
        }

        [Fact]
        public void UlongToThousands()
        {
            const ulong number = 3;
            Assert.Equal(3000UL, number.Thousands());
        }

        [Fact]
        public void DoubleToThousands()
        {
            const double number = 3;
            Assert.Equal(3000d, number.Thousands());
        }

        [Fact]
        public void IntToMillions()
        {
            const int number = 4;
            Assert.Equal(4000000, number.Millions());
        }

        [Fact]
        public void UintToMillions()
        {
            const uint number = 4;
            Assert.Equal(4000000U, number.Millions());
        }

        [Fact]
        public void LongToMillions()
        {
            const long number = 4;
            Assert.Equal(4000000L, number.Millions());
        }

        [Fact]
        public void UlongToMillions()
        {
            const ulong number = 4;
            Assert.Equal(4000000UL, number.Millions());
        }

        [Fact]
        public void DoubleToMillions()
        {
            const double number = 4;
            Assert.Equal(4000000d, number.Millions());
        }

        [Fact]
        public void IntToBillions()
        {
            const int number = 1;
            Assert.Equal(1000000000, number.Billions());
        }

        [Fact]
        public void UintToBillions()
        {
            const uint number = 1;
            Assert.Equal(1000000000U, number.Billions());
        }

        [Fact]
        public void LongToBillions()
        {
            const long number = 1;
            Assert.Equal(1000000000L, number.Billions());
        }

        [Fact]
        public void UlongToBillions()
        {
            const ulong number = 1;
            Assert.Equal(1000000000UL, number.Billions());
        }

        [Fact]
        public void DoubleToBillions()
        {
            const double number = 1;
            Assert.Equal(1000000000d, number.Billions());
        }
    }
}
