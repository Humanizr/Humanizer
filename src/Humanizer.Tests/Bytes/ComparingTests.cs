using Humanizer.Bytes;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Bytes
{
    public class ComparingTests
    {
        [Theory]
        [InlineData(13, 23, -1)]
        [InlineData(23, 23, 0)]
        [InlineData(45, 23, 1)]
        public void CompareStrongTyped(double value, double valueToCompareWith, int expectedResult)
        {
            var valueSize = new ByteSize(value);
            var otherSize = new ByteSize(valueToCompareWith);
            var result = valueSize.CompareTo(otherSize);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(13, 23, -1)]
        [InlineData(23, 23, 0)]
        [InlineData(45, 23, 1)]
        public void CompareUntyped(double value, double valueToCompareWith, int expectedResult)
        {
            var valueSize = new ByteSize(value);
            object otherSize = new ByteSize(valueToCompareWith);
            var result = valueSize.CompareTo(otherSize);

            Assert.Equal(expectedResult, result);
        }
    }
}