using Xunit;
// ReSharper disable IdentifierTypo 
// ReSharper disable StringLiteralTypo

namespace Humanizer.Tests
{
    public class TupleizeTests
    {
        [Theory]
        [InlineData(1, "single")]
        [InlineData(2, "double")]
        [InlineData(3, "triple")]
        [InlineData(4, "quadruple")]
        [InlineData(5, "quintuple")]
        [InlineData(6, "sextuple")]
        [InlineData(7, "septuple")]
        [InlineData(8, "octuple")]
        [InlineData(9, "nonuple")]
        [InlineData(10, "decuple")]
        [InlineData(100, "centuple")]
        [InlineData(1000, "milluple")]
        public void Given_int_with_named_tuple_gives_correct_result(int n, string expected)
        {
            Assert.Equal(expected, n.Tupleize());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void Given_other_number_returns_n_tuple(int n)
        {
            Assert.Equal($"{n}-tuple", n.Tupleize());
        }
    }
}
