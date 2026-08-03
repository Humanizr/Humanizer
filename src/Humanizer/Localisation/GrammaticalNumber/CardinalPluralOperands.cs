using System.Numerics;

namespace Humanizer;

/// <summary>
/// Exact operands defined by Unicode CLDR cardinal plural rules for
/// <see cref="decimal"/>, <see cref="long"/>, and <see cref="double"/> quantities,
/// with digit components stored as <see cref="BigInteger"/> values.
/// </summary>
readonly struct CardinalPluralOperands
{
    internal const int InverseRyuPowerCount = 292;
    internal const int DirectRyuPowerCount = 326;
    static readonly BigInteger[] PowersOfTen = CreatePowersOfTen();

    CardinalPluralOperands(BigInteger absoluteDigits, int scale)
    {
        IsSupported = true;
        AbsoluteDigits = absoluteDigits;
        V = scale;

        if (scale == 0)
        {
            IntegerDigits = absoluteDigits;
            FractionDigits = BigInteger.Zero;
        }
        else if (scale > 28)
        {
            IntegerDigits = BigInteger.Zero;
            FractionDigits = absoluteDigits;
        }
        else
        {
            var divisor = PowerOfTen(scale);
            IntegerDigits = BigInteger.DivRem(absoluteDigits, divisor, out var fractionDigits);
            FractionDigits = fractionDigits;
        }

        var fractionDigitsWithoutTrailingZeros = FractionDigits;
        var visibleFractionDigitCountWithoutTrailingZeros = scale;
        while (visibleFractionDigitCountWithoutTrailingZeros > 0 &&
               fractionDigitsWithoutTrailingZeros % 10 == 0)
        {
            fractionDigitsWithoutTrailingZeros /= 10;
            visibleFractionDigitCountWithoutTrailingZeros--;
        }

        FractionDigitsWithoutTrailingZeros = fractionDigitsWithoutTrailingZeros;
        W = visibleFractionDigitCountWithoutTrailingZeros;
    }

    /// <summary>
    /// Gets whether the source value has an exact operand representation.
    /// Unsupported inputs fail closed as the default instance, where this value is
    /// <see langword="false"/>.
    /// </summary>
    public bool IsSupported { get; }
    public BigInteger AbsoluteDigits { get; }
    public BigInteger IntegerDigits { get; }
    public BigInteger FractionDigits { get; }
    public BigInteger FractionDigitsWithoutTrailingZeros { get; }
    public int V { get; }
    public int W { get; }

    public static CardinalPluralOperands Create(decimal value)
    {
        var bits = decimal.GetBits(value);
        var absoluteDigits =
            (BigInteger)(uint)bits[0] |
            (BigInteger)(uint)bits[1] << 32 |
            (BigInteger)(uint)bits[2] << 64;
        var scale = (bits[3] >> 16) & 0xFF;

        return new(absoluteDigits, scale);
    }

    public static CardinalPluralOperands Create(long value) =>
        new(BigInteger.Abs(new(value)), 0);

    public static CardinalPluralOperands Create(double value)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            return default;

        value = Math.Abs(value);

        if (value is 0)
            return new(BigInteger.Zero, 0);

        GetShortestRoundTrip(value, out var digits, out var decimalExponent);
        return Create(digits, decimalExponent);
    }

    static CardinalPluralOperands Create(ulong digits, int decimalExponent) =>
        decimalExponent < 0
            ? new(new BigInteger(digits), -decimalExponent)
            : new(new BigInteger(digits) * PowerOfTen(decimalExponent), 0);

    internal static void GetShortestRoundTrip(
        double value,
        out ulong digits,
        out int decimalExponent)
    {
        var bits = unchecked((ulong)BitConverter.DoubleToInt64Bits(value));
        ToShortestDecimal(
            bits & 0x000FFFFFFFFFFFFF,
            (uint)((bits >> 52) & 0x7FF),
            out digits,
            out decimalExponent);
    }

    internal static (ulong Low, ulong High) GetInverseRyuPower(int index) =>
        GetRyuPower(RyuPowerCache.Tables.Inverse, index);

    internal static (ulong Low, ulong High) GetDirectRyuPower(int index) =>
        GetRyuPower(RyuPowerCache.Tables.Direct, index);

    static (ulong Low, ulong High) GetRyuPower(ulong[] table, int index)
    {
        var offset = checked(index * 2);
        return (table[offset], table[offset + 1]);
    }

    // Portions of this file are adapted from Ryu.
    // Copyright 2018 Ulf Adams
    // Distributed under the Boost Software License, Version 1.0.
    // See accompanying THIRD-PARTY-NOTICES.txt or https://www.boost.org/LICENSE_1_0.txt.
    static void ToShortestDecimal(
        ulong ieeeMantissa,
        uint ieeeExponent,
        out ulong digits,
        out int decimalExponent)
    {
        int binaryExponent;
        ulong mantissa;
        if (ieeeExponent == 0)
        {
            binaryExponent = 1 - 1023 - 52 - 2;
            mantissa = ieeeMantissa;
        }
        else
        {
            binaryExponent = (int)ieeeExponent - 1023 - 52 - 2;
            mantissa = (1UL << 52) | ieeeMantissa;
        }

        var acceptBounds = (mantissa & 1) == 0;
        var scaledMantissa = 4 * mantissa;
        var lowerBoundaryShift = ieeeMantissa != 0 || ieeeExponent <= 1 ? 1U : 0U;
        ulong center;
        ulong upper;
        ulong lower;
        int baseTenExponent;
        var lowerIsTrailingZeros = false;
        var centerIsTrailingZeros = false;
        if (binaryExponent >= 0)
        {
            var power = Log10PowerOfTwo(binaryExponent);
            if (binaryExponent > 3)
                power--;
            baseTenExponent = (int)power;
            var bitCount = 125 + PowerOfFiveBits((int)power) - 1;
            var shift = -binaryExponent + (int)power + bitCount;
            center = MultiplyAndShiftAll(
                mantissa,
                RyuPowerCache.Tables.Inverse,
                (int)power,
                shift,
                lowerBoundaryShift,
                out upper,
                out lower);
            if (power <= 21)
            {
                if (scaledMantissa % 5 == 0)
                {
                    centerIsTrailingZeros = IsMultipleOfPowerOfFive(
                        scaledMantissa,
                        power);
                }
                else if (acceptBounds)
                {
                    lowerIsTrailingZeros = IsMultipleOfPowerOfFive(
                        scaledMantissa - 1 - lowerBoundaryShift,
                        power);
                }
                else if (IsMultipleOfPowerOfFive(scaledMantissa + 2, power))
                {
                    upper--;
                }
            }
        }
        else
        {
            var power = Log10PowerOfFive(-binaryExponent);
            if (-binaryExponent > 1)
                power--;
            baseTenExponent = (int)power + binaryExponent;
            var index = -binaryExponent - (int)power;
            var bitCount = PowerOfFiveBits(index) - 125;
            var shift = (int)power - bitCount;
            center = MultiplyAndShiftAll(
                mantissa,
                RyuPowerCache.Tables.Direct,
                index,
                shift,
                lowerBoundaryShift,
                out upper,
                out lower);
            if (power <= 1)
            {
                centerIsTrailingZeros = true;
                if (acceptBounds)
                    lowerIsTrailingZeros = lowerBoundaryShift == 1;
                else
                    upper--;
            }
            else if (power < 63)
            {
                centerIsTrailingZeros = IsMultipleOfPowerOfTwo(
                    scaledMantissa,
                    power);
            }
        }

        var removed = 0;
        byte lastRemovedDigit = 0;
        if (lowerIsTrailingZeros || centerIsTrailingZeros)
        {
            while (upper / 10 > lower / 10)
            {
                lowerIsTrailingZeros &= lower % 10 == 0;
                centerIsTrailingZeros &= lastRemovedDigit == 0;
                lastRemovedDigit = (byte)(center % 10);
                center /= 10;
                upper /= 10;
                lower /= 10;
                removed++;
            }

            if (lowerIsTrailingZeros)
            {
                while (lower % 10 == 0)
                {
                    centerIsTrailingZeros &= lastRemovedDigit == 0;
                    lastRemovedDigit = (byte)(center % 10);
                    center /= 10;
                    upper /= 10;
                    lower /= 10;
                    removed++;
                }
            }

            if (centerIsTrailingZeros &&
                lastRemovedDigit == 5 &&
                center % 2 == 0)
            {
                lastRemovedDigit = 4;
            }

            digits = center +
                     (center == lower && (!acceptBounds || !lowerIsTrailingZeros) ||
                      lastRemovedDigit >= 5
                         ? 1UL
                         : 0UL);
        }
        else
        {
            var roundUp = false;
            if (upper / 100 > lower / 100)
            {
                roundUp = center % 100 >= 50;
                center /= 100;
                upper /= 100;
                lower /= 100;
                removed += 2;
            }

            while (upper / 10 > lower / 10)
            {
                roundUp = center % 10 >= 5;
                center /= 10;
                upper /= 10;
                lower /= 10;
                removed++;
            }

            digits = center + (center == lower || roundUp ? 1UL : 0UL);
        }

        decimalExponent = baseTenExponent + removed;
    }

    static ulong MultiplyAndShiftAll(
        ulong mantissa,
        ulong[] multipliers,
        int multiplierIndex,
        int shift,
        uint lowerBoundaryShift,
        out ulong upper,
        out ulong lower)
    {
        upper = MultiplyAndShift(
            4 * mantissa + 2,
            multipliers,
            multiplierIndex,
            shift);
        lower = MultiplyAndShift(
            4 * mantissa - 1 - lowerBoundaryShift,
            multipliers,
            multiplierIndex,
            shift);
        return MultiplyAndShift(
            4 * mantissa,
            multipliers,
            multiplierIndex,
            shift);
    }

    static ulong MultiplyAndShift(
        ulong value,
        ulong[] multipliers,
        int multiplierIndex,
        int shift)
    {
        var offset = multiplierIndex * 2;
        Multiply64(
            value,
            multipliers[offset],
            out _,
            out var lowProductHigh);
        Multiply64(
            value,
            multipliers[offset + 1],
            out var highProductLow,
            out var highProductHigh);
        var sum = lowProductHigh + highProductLow;
        if (sum < lowProductHigh)
            highProductHigh++;
        // Ryu's fixed-point multiplier requires shift to be greater than 64.
        var distance = shift - 64;
        return sum >> distance | highProductHigh << (64 - distance);
    }

    static void Multiply64(
        ulong left,
        ulong right,
        out ulong low,
        out ulong high)
    {
        var leftLow = (uint)left;
        var leftHigh = (uint)(left >> 32);
        var rightLow = (uint)right;
        var rightHigh = (uint)(right >> 32);
        var lowLow = (ulong)leftLow * rightLow;
        var lowHigh = (ulong)leftLow * rightHigh;
        var highLow = (ulong)leftHigh * rightLow;
        var highHigh = (ulong)leftHigh * rightHigh;
        var middle = highLow + (lowLow >> 32);
        var middleLow = (uint)middle;
        var middleHigh = middle >> 32;
        var upperMiddle = lowHigh + middleLow;
        low = upperMiddle << 32 | (uint)lowLow;
        high = highHigh + middleHigh + (upperMiddle >> 32);
    }

    static int PowerOfFiveBits(int exponent) =>
        (int)(((uint)exponent * 1217359 >> 19) + 1);

    static uint Log10PowerOfTwo(int exponent) =>
        (uint)exponent * 78913 >> 18;

    static uint Log10PowerOfFive(int exponent) =>
        (uint)exponent * 732923 >> 20;

    static bool IsMultipleOfPowerOfFive(ulong value, uint exponent)
    {
        for (var index = 0U; index < exponent; index++)
        {
            if (value % 5 != 0)
                return false;
            value /= 5;
        }

        return true;
    }

    static bool IsMultipleOfPowerOfTwo(ulong value, uint exponent) =>
        (value & ((1UL << (int)exponent) - 1)) == 0;

    static BigInteger PowerOfTen(int exponent)
    {
        var value = BigInteger.One;
        for (var index = 0; exponent != 0; index++, exponent >>= 1)
        {
            if ((exponent & 1) != 0)
                value *= PowersOfTen[index];
        }

        return value;
    }

    static RyuPowerTables CreateRyuPowerTables()
    {
        ulong[] inverseSeeds =
        [
            1, 2305843009213693952,
            5955668970331000884, 1784059615882449851,
            8982663654677661702, 1380349269358112757,
            7286864317269821294, 2135987035920910082,
            7005857020398200553, 1652639921975621497,
            17965325103354776697, 1278668206209430417,
            8928596168509315048, 1978643211784836272,
            10075671573058298858, 1530901034580419511,
            597001226353042382, 1184477304306571148,
            1527430471115325346, 1832889850782397517,
            12533209867169019542, 1418129833677084982,
            5577825024675947042, 2194449627517475473,
            11006974540203867551, 1697873161311732311,
            10313493231639821582, 1313665730009899186,
            12701016819766672773, 2032799256770390445
        ];
        uint[] inverseOffsets =
        [
            0x54544554, 0x04055545, 0x10041000, 0x00400414,
            0x40010000, 0x41155555, 0x00000454, 0x00010044,
            0x40000000, 0x44000041, 0x50454450, 0x55550054,
            0x51655554, 0x40004000, 0x01000001, 0x00010500,
            0x51515411, 0x05555554, 0x00000000
        ];
        ulong[] directSeeds =
        [
            0, 1152921504606846976,
            0, 1490116119384765625,
            1032610780636961552, 1925929944387235853,
            7910200175544436838, 1244603055572228341,
            16941905809032713930, 1608611746708759036,
            13024893955298202172, 2079081953128979843,
            6607496772837067824, 1343575221513417750,
            17332926989895652603, 1736530273035216783,
            13037379183483547984, 2244412773384604712,
            1605989338741628675, 1450417759929778918,
            9630225068416591280, 1874621017369538693,
            665883850346957067, 1211445438634777304,
            14931890668723713708, 1565756531257009982
        ];
        uint[] directOffsets =
        [
            0x00000000, 0x00000000, 0x00000000, 0x00000000,
            0x40000000, 0x59695995, 0x55545555, 0x56555515,
            0x41150504, 0x40555410, 0x44555145, 0x44504540,
            0x45555550, 0x40004000, 0x96440440, 0x55565565,
            0x54454045, 0x40154151, 0x55559155, 0x51405555,
            0x00000105
        ];
        ulong[] smallPowers =
        [
            1, 5, 25, 125, 625, 3125, 15625, 78125,
            390625, 1953125, 9765625, 48828125, 244140625,
            1220703125, 6103515625, 30517578125, 152587890625,
            762939453125, 3814697265625, 19073486328125,
            95367431640625, 476837158203125, 2384185791015625,
            11920928955078125, 59604644775390625, 298023223876953125
        ];

        const int blockSize = 26;
        var inverse = new ulong[InverseRyuPowerCount * 2];
        for (var index = 0; index < inverse.Length / 2; index++)
        {
            var block = (index + blockSize - 1) / blockSize;
            var blockExponent = block * blockSize;
            var offset = blockExponent - index;
            var seedLow = inverseSeeds[block * 2];
            var seedHigh = inverseSeeds[block * 2 + 1];
            if (offset == 0)
            {
                inverse[index * 2] = seedLow;
                inverse[index * 2 + 1] = seedHigh;
            }
            else
            {
                var multiplier = smallPowers[offset];
                var delta =
                    PowerOfFiveBits(blockExponent) -
                    PowerOfFiveBits(index);
                var correction =
                    (inverseOffsets[index / 16] >> ((index % 16) * 2)) & 3;
                ComputeRyuSplit(
                    multiplier,
                    seedLow - 1,
                    seedHigh,
                    delta,
                    correction + 1,
                    out inverse[index * 2],
                    out inverse[index * 2 + 1]);
            }
        }

        var direct = new ulong[DirectRyuPowerCount * 2];
        for (var index = 0; index < direct.Length / 2; index++)
        {
            var block = index / blockSize;
            var blockExponent = block * blockSize;
            var offset = index - blockExponent;
            var seedLow = directSeeds[block * 2];
            var seedHigh = directSeeds[block * 2 + 1];
            if (offset == 0)
            {
                direct[index * 2] = seedLow;
                direct[index * 2 + 1] = seedHigh;
            }
            else
            {
                var multiplier = smallPowers[offset];
                var delta =
                    PowerOfFiveBits(index) -
                    PowerOfFiveBits(blockExponent);
                var correction =
                    (directOffsets[index / 16] >> ((index % 16) * 2)) & 3;
                ComputeRyuSplit(
                    multiplier,
                    seedLow,
                    seedHigh,
                    delta,
                    correction,
                    out direct[index * 2],
                    out direct[index * 2 + 1]);
            }
        }

        return new(inverse, direct);
    }

    static void ComputeRyuSplit(
        ulong multiplier,
        ulong lowFactor,
        ulong highFactor,
        int shift,
        uint correction,
        out ulong low,
        out ulong high)
    {
        Multiply64(multiplier, lowFactor, out var lowProductLow, out var lowProductHigh);
        Multiply64(multiplier, highFactor, out var highProductLow, out var highProductHigh);
        var inverseShift = 64 - shift;
        var firstLow = lowProductLow >> shift | lowProductHigh << inverseShift;
        var firstHigh = lowProductHigh >> shift;
        var secondLow = highProductLow << inverseShift;
        var secondHigh = highProductHigh << inverseShift | highProductLow >> shift;
        low = firstLow + secondLow;
        high = firstHigh + secondHigh + (low < firstLow ? 1UL : 0UL);
        var uncorrected = low;
        low += correction;
        if (low < uncorrected)
            high++;
    }

    static BigInteger[] CreatePowersOfTen()
    {
        var powers = new BigInteger[9];
        powers[0] = new BigInteger(10);
        for (var index = 1; index < powers.Length; index++)
            powers[index] = powers[index - 1] * powers[index - 1];

        return powers;
    }

    sealed class RyuPowerTables(ulong[] inverse, ulong[] direct)
    {
        public ulong[] Inverse { get; } = inverse;
        public ulong[] Direct { get; } = direct;
    }

    static class RyuPowerCache
    {
        public static RyuPowerTables Tables { get; } = CreateRyuPowerTables();
    }
}