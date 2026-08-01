using System.Diagnostics;
using System.Numerics;

namespace Humanizer.Tests;

public class CardinalPluralOperandsTests
{
    [Fact]
    public void Create_MixedDoubleSet_MeetsPerformanceGate()
    {
        Assert.SkipUnless(
            Environment.GetEnvironmentVariable("HUMANIZER_RUN_PERFORMANCE_GATES") == "1",
            "performance gate disabled");

        var values = new double[1_024];
        values[0] = 1.25;
        values[1] = BitConverter.Int64BitsToDouble(0x2A1A165700694830);
        values[2] = double.Epsilon;
        values[3] = double.MaxValue;
        var bits = 0x9E3779B97F4A7C15UL;
        for (var index = 4; index < values.Length; index++)
        {
            bits = bits * 6364136223846793005 + 1442695040888963407;
            if ((bits & 0x7FF0000000000000) == 0x7FF0000000000000)
                bits ^= 0x0010000000000000;
            values[index] = BitConverter.Int64BitsToDouble(unchecked((long)bits));
        }

        const int operationCount = 100_000;
        var results = new List<string>();
        var worstNanoseconds = 0d;
        for (var warmup = 0; warmup < 10_000; warmup++)
            _ = CardinalPluralOperands.Create(values[warmup & (values.Length - 1)]);
        for (var run = 0; run < 3; run++)
        {
            var beforeAllocation = GC.GetAllocatedBytesForCurrentThread();
            var start = Stopwatch.GetTimestamp();
            var checksum = 0;
            for (var index = 0; index < operationCount; index++)
                checksum ^= CardinalPluralOperands.Create(
                    values[index & (values.Length - 1)]).V;
            var elapsed = Stopwatch.GetTimestamp() - start;
            var allocated = GC.GetAllocatedBytesForCurrentThread() - beforeAllocation;
            var nanoseconds =
                elapsed * 1_000_000_000d /
                Stopwatch.Frequency /
                operationCount;
            worstNanoseconds = Math.Max(worstNanoseconds, nanoseconds);
            results.Add(
                $"{run}:{nanoseconds:F1}ns:{allocated / operationCount}B:{checksum}");
        }

        Assert.True(
            worstNanoseconds <= 1_000,
            string.Join(Environment.NewLine, results));
    }

    [Fact]
    public void Create_LongMinValue_PreservesExactMagnitude()
    {
        var operands = CardinalPluralOperands.Create(long.MinValue);

        Assert.True(operands.IsSupported);
        Assert.Equal(BigInteger.Parse("9223372036854775808"), operands.AbsoluteDigits);
        Assert.Equal(BigInteger.Parse("9223372036854775808"), operands.IntegerDigits);
        Assert.Equal(0, operands.V);
    }

    [Fact]
    public void Create_DecimalMinValue_PreservesExactMagnitude()
    {
        var operands = CardinalPluralOperands.Create(decimal.MinValue);

        Assert.True(operands.IsSupported);
        Assert.Equal(
            BigInteger.Parse("79228162514264337593543950335"),
            operands.AbsoluteDigits);
        Assert.Equal(
            BigInteger.Parse("79228162514264337593543950335"),
            operands.IntegerDigits);
        Assert.Equal(0, operands.V);
    }

    [Fact]
    public void Create_DecimalScale_PreservesVisibleFractionCount()
    {
        var integer = CardinalPluralOperands.Create(1m);
        var visibleFraction = CardinalPluralOperands.Create(1.0m);

        Assert.Equal(0, integer.V);
        Assert.Equal(1, visibleFraction.V);
        Assert.Equal(BigInteger.Zero, visibleFraction.FractionDigits);
    }

    [Fact]
    public void Create_NegativeFraction_PreservesFractionOperands()
    {
        var operands = CardinalPluralOperands.Create(-1.2300m);

        Assert.True(operands.IsSupported);
        Assert.Equal(new BigInteger(12300), operands.AbsoluteDigits);
        Assert.Equal(BigInteger.One, operands.IntegerDigits);
        Assert.Equal(new BigInteger(2300), operands.FractionDigits);
        Assert.Equal(new BigInteger(23), operands.FractionDigitsWithoutTrailingZeros);
        Assert.Equal(4, operands.V);
        Assert.Equal(2, operands.W);
    }

    [Theory]
    [InlineData(1e-7, "1", 7, "0", "1")]
    [InlineData(1.25e20, "125000000000000000000", 0, "125000000000000000000", "0")]
    [InlineData(-1.25e-7, "125", 9, "0", "125")]
    public void Create_FiniteDouble_ExpandsRoundTripExponentExactly(
        double value,
        string expectedAbsoluteDigits,
        int expectedScale,
        string expectedIntegerDigits,
        string expectedFractionDigits)
    {
        var operands = CardinalPluralOperands.Create(value);

        Assert.True(operands.IsSupported);
        Assert.Equal(BigInteger.Parse(expectedAbsoluteDigits), operands.AbsoluteDigits);
        Assert.Equal(BigInteger.Parse(expectedIntegerDigits), operands.IntegerDigits);
        Assert.Equal(BigInteger.Parse(expectedFractionDigits), operands.FractionDigits);
        Assert.Equal(expectedScale, operands.V);
    }

    [Fact]
    public void Create_DoubleRoundTrip_IsStableAcrossRuntimes()
    {
        var value = BitConverter.Int64BitsToDouble(unchecked((long)0x2A1A165700694830));

        var operands = CardinalPluralOperands.Create(value);
        var success = CardinalPluralRules.TrySelect(
            CardinalPluralRuleKind.SouthSlavic,
            value,
            out var category);

        Assert.Equal(BigInteger.Parse("7109025719833441"), operands.AbsoluteDigits);
        Assert.Equal(121, operands.V);
        Assert.True(success);
        Assert.Equal(CardinalPluralCategory.One, category);
    }

    [Theory]
    [InlineData(
        unchecked((long)0x9DA9BF211C1E0B98),
        "8732372760174747",
        181)]
    [InlineData(
        unchecked((long)0x0ABD6631EE0D7528),
        "6118700771860197",
        272)]
    [InlineData(
        unchecked((long)0x43186B2097F478CD),
        "17182967933906432",
        1)]
    public void Create_DoubleShortestRoundTrip_IsStableAcrossRuntimes(
        long bits,
        string expectedAbsoluteDigits,
        int expectedScale)
    {
        var value = BitConverter.Int64BitsToDouble(bits);

        var operands = CardinalPluralOperands.Create(value);

        Assert.Equal(BigInteger.Parse(expectedAbsoluteDigits), operands.AbsoluteDigits);
        Assert.Equal(expectedScale, operands.V);
    }

    [Theory]
    [InlineData(1L, "5", -324)]
    [InlineData(3L, "15", -324)]
    [InlineData(0x0010000000000000L, "22250738585072014", -324)]
    [InlineData(0x3FF0000000000000L, "1", 0)]
    [InlineData(0x4000000000000000L, "2", 0)]
    [InlineData(0x2A1A165700694830L, "7109025719833441", -121)]
    [InlineData(0x7FEFFFFFFFFFFFFFL, "17976931348623157", 292)]
    public void LegacyShortestRoundTrip_MatchesReferenceAndModernRuntime(
        long bits,
        string expectedDigitsText,
        int expectedDecimalExponent)
    {
        var value = BitConverter.Int64BitsToDouble(bits);

        CardinalPluralOperands.GetLegacyShortestRoundTrip(
            value,
            out var actualDigits,
            out var actualDecimalExponent);

        var expectedDigits = ulong.Parse(
            expectedDigitsText,
            CultureInfo.InvariantCulture);
        Assert.Equal(expectedDigits, actualDigits);
        Assert.Equal(expectedDecimalExponent, actualDecimalExponent);

#if !NETFRAMEWORK
        ParseRuntimeRoundTrip(
            value.ToString("R", CultureInfo.InvariantCulture),
            out var runtimeDigits,
            out var runtimeDecimalExponent);
        Assert.Equal(runtimeDigits, actualDigits);
        Assert.Equal(runtimeDecimalExponent, actualDecimalExponent);
#endif
    }

    [Fact]
    public void Create_DoubleExtremes_PreserveExactShortestOperands()
    {
        var maximum = CardinalPluralOperands.Create(double.MaxValue);
        var predecessor = CardinalPluralOperands.Create(
            BitConverter.Int64BitsToDouble(0x7FEFFFFFFFFFFFFE));
        var epsilon = CardinalPluralOperands.Create(double.Epsilon);

        Assert.Equal(
            BigInteger.Parse("17976931348623157") * BigInteger.Pow(10, 292),
            maximum.AbsoluteDigits);
        Assert.Equal(0, maximum.V);
        Assert.Equal(
            BigInteger.Parse("17976931348623155") * BigInteger.Pow(10, 292),
            predecessor.AbsoluteDigits);
        Assert.Equal(0, predecessor.V);
        Assert.Equal(new BigInteger(5), epsilon.AbsoluteDigits);
        Assert.Equal(324, epsilon.V);
        Assert.Equal(BigInteger.Zero, epsilon.IntegerDigits);
        Assert.Equal(new BigInteger(5), epsilon.FractionDigits);
    }

    [Fact]
    public void Create_LowSubnormalRange_CompletesWithExactOperands()
    {
        for (var bits = 1L; bits <= 100; bits++)
        {
            var operands = CardinalPluralOperands.Create(
                BitConverter.Int64BitsToDouble(bits));

            Assert.True(operands.IsSupported);
            Assert.True(operands.AbsoluteDigits > 0);
        }

        var thirdSubnormal = CardinalPluralOperands.Create(
            BitConverter.Int64BitsToDouble(3));
        Assert.Equal(new BigInteger(15), thirdSubnormal.AbsoluteDigits);
        Assert.Equal(324, thirdSubnormal.V);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void Create_NonFiniteDouble_IsUnsupported(double value)
    {
        var operands = CardinalPluralOperands.Create(value);

        Assert.False(operands.IsSupported);
    }

    [Theory]
    [InlineData((int)CardinalPluralRuleKind.Spanish, 1e300, (int)CardinalPluralCategory.Many)]
    [InlineData((int)CardinalPluralRuleKind.Polish, 1e100, (int)CardinalPluralCategory.Many)]
    [InlineData((int)CardinalPluralRuleKind.Romanian, 1e-300, (int)CardinalPluralCategory.Few)]
    public void TrySelect_FiniteExponentDouble_UsesExactOperands(
        int kind,
        double value,
        int expected)
    {
        var success = CardinalPluralRules.TrySelect(
            (CardinalPluralRuleKind)kind,
            value,
            out var category);

        Assert.True(success);
        Assert.Equal((CardinalPluralCategory)expected, category);
    }

    [Fact]
    public void Select_LongInteger_UsesExactModuloRules()
    {
        var category = CardinalPluralRules.Select(
            CardinalPluralRuleKind.Polish,
            9_223_372_036_854_775_802L);

        Assert.Equal(CardinalPluralCategory.Few, category);
    }

    [Theory]
    [InlineData(101, (int)CardinalPluralCategory.Few)]
    [InlineData(119, (int)CardinalPluralCategory.Few)]
    [InlineData(120, (int)CardinalPluralCategory.Other)]
    public void RomanianCLDR48ModuloBoundariesSelectExpectedCategory(
        long quantity,
        int expected)
    {
        var category = CardinalPluralRules.Select(
            CardinalPluralRuleKind.Romanian,
            quantity);

        Assert.Equal((CardinalPluralCategory)expected, category);
    }

    [Fact]
    public void UnknownCardinalRuleMetadataFailsClosed()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => CardinalPluralRuleMetadata.GetReachableCategories("Unknown"));
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void TrySelect_NonFiniteDouble_IsUnsupported(double value)
    {
        var success = CardinalPluralRules.TrySelect(
            CardinalPluralRuleKind.EnglishLike,
            value,
            out var category);

        Assert.False(success);
        Assert.Equal(CardinalPluralCategory.Other, category);
    }

#if !NETFRAMEWORK
    static void ParseRuntimeRoundTrip(
        string roundTrip,
        out ulong digits,
        out int decimalExponent)
    {
        var exponentMarker = roundTrip.IndexOf('E');
        var significandEnd = exponentMarker < 0 ? roundTrip.Length : exponentMarker;
        var exponent = exponentMarker < 0
            ? 0
            : int.Parse(
                roundTrip[(exponentMarker + 1)..],
                CultureInfo.InvariantCulture);
        var decimalPoint = roundTrip.IndexOf('.');
        var scale = decimalPoint < 0 ? 0 : significandEnd - decimalPoint - 1;
        digits = 0;
        for (var index = 0; index < significandEnd; index++)
        {
            var character = roundTrip[index];
            if (character != '.')
                digits = digits * 10 + (uint)(character - '0');
        }

        decimalExponent = exponent - scale;
    }
#endif
}