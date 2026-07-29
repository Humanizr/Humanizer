using System.Reflection;
using System.Runtime.InteropServices;
using Humanizer.Tests.Localisation;

[UseCulture("en")]
public class ByteSizeUnitSystemTests
{
    [Fact]
    public void PreservesLegacyLayoutValuesAndDefaults()
    {
        Assert.Equal(48, Marshal.SizeOf<ByteSize>());
        Assert.Collection(
            typeof(ByteSize)
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .OrderBy(field => field.MetadataToken),
            field => verifyField(field, "<Bits>k__BackingField", typeof(long)),
            field => verifyField(field, "<Bytes>k__BackingField", typeof(double)),
            field => verifyField(field, "<Kilobytes>k__BackingField", typeof(double)),
            field => verifyField(field, "<Megabytes>k__BackingField", typeof(double)),
            field => verifyField(field, "<Gigabytes>k__BackingField", typeof(double)),
            field => verifyField(field, "<Terabytes>k__BackingField", typeof(double)));
        Assert.Equal(1024, ByteSize.BytesInKilobyte);
        Assert.Equal(0, (int)ByteSizeUnitSystem.Legacy);
#if NET5_0_OR_GREATER
        var dataUnits = Enum.GetValues<DataUnit>();
#else
        var dataUnits = (DataUnit[])Enum.GetValues(typeof(DataUnit));
#endif
        Assert.Equal(
            Enumerable.Range(0, 13),
            dataUnits.Take(13).Select(value => (int)value));
        Assert.Equal("1 KB", ByteSize.FromBytes(1024).ToString());
        Assert.Equal(ByteSize.FromKilobytes(1), ByteSize.Parse("1 KB", default(IFormatProvider)));

        static void verifyField(FieldInfo field, string name, Type type)
        {
            Assert.Equal(name, field.Name);
            Assert.Equal(type, field.FieldType);
        }
    }

    [Fact]
    public void LegacySystemEntryPointsDelegateToExistingApis()
    {
        var culture = CultureInfo.GetCultureInfo("en-US");
        var size = ByteSize.FromBytes(10_242);
        var rate = size.Per(TimeSpan.FromSeconds(2));
        const string format = "0.0";
        const string text = "1.5 KB";

        Assert.Equal(size.ToString(format, culture), size.Format(ByteSizeUnitSystem.Legacy, format, culture));
        Assert.Equal(size.ToFullWords(format, culture), size.FormatFullWords(ByteSizeUnitSystem.Legacy, format, culture));
        Assert.Equal(size.Humanize(format, culture), size.HumanizeWithUnitSystem(ByteSizeUnitSystem.Legacy, format, culture));
        Assert.Equal(
            size.HumanizeComposite(precision: 3, formatProvider: culture, separator: ", ", toWords: true),
            size.HumanizeCompositeWithUnitSystem(
                ByteSizeUnitSystem.Legacy,
                precision: 3,
                formatProvider: culture,
                separator: ", ",
                toWords: true));
        Assert.Equal(
            rate.Humanize(format, TimeUnit.Minute, culture),
            rate.HumanizeWithUnitSystem(ByteSizeUnitSystem.Legacy, format, TimeUnit.Minute, culture));
        var exactRate = ByteSize.FromBits(9_007_199_254_740_993).Per(TimeSpan.FromSeconds(2));
        Assert.Equal(
            exactRate.Humanize("0 b", TimeUnit.Minute, culture),
            exactRate.HumanizeWithUnitSystem(ByteSizeUnitSystem.Legacy, "0 b", TimeUnit.Minute, culture));
        Assert.Equal(ByteSize.Parse(text, culture), ByteSize.ParseWithUnitSystem(text, ByteSizeUnitSystem.Legacy, culture));

        Assert.True(ByteSize.TryParse(text, culture, out var parsed));
        Assert.True(ByteSize.TryParseWithUnitSystem(text, ByteSizeUnitSystem.Legacy, culture, out var parsedWithSystem));
        Assert.Equal(parsed, parsedWithSystem);

        Assert.True(ByteSize.TryParse(text.AsSpan(), culture, out var parsedSpan));
        Assert.True(ByteSize.TryParseSpanWithUnitSystem(
            text.AsSpan(),
            ByteSizeUnitSystem.Legacy,
            culture,
            out var parsedSpanWithSystem));
        Assert.Equal(parsedSpan, parsedSpanWithSystem);
    }

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi, "fr")]
    [InlineData(ByteSizeUnitSystem.DecimalSi, "fr-CA")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, "fr")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, "fr-CA")]
    public void ExplicitSystemsUseCanonicalBaseUnitSymbols(ByteSizeUnitSystem unitSystem, string cultureName)
    {
        var culture = CultureInfo.GetCultureInfo(cultureName);
        var byteSize = ByteSize.FromBytes(1);
        var bitSize = ByteSize.FromBits(1);
        var formattedBytes = byteSize.Format(unitSystem, "0.0 B", culture);
        var formattedBits = bitSize.Format(unitSystem, "0 b", culture);

        Assert.Equal($"{1d.ToString("0.0", culture)} B", formattedBytes);
        Assert.Equal("1 b", formattedBits);
        Assert.Equal($"{1d.ToString("0.0", culture)} b", bitSize.Format(unitSystem, "0.0 b", culture));
        Assert.Equal("1 B", byteSize.Format(unitSystem, formatProvider: culture));
        Assert.Equal("1 b", bitSize.Format(unitSystem, formatProvider: culture));
        Assert.Equal(byteSize, ByteSize.ParseWithUnitSystem(formattedBytes, unitSystem, culture));
        Assert.Equal(bitSize, ByteSize.ParseWithUnitSystem(formattedBits, unitSystem, culture));
        Assert.Equal(byteSize.ToFullWords("0 B", culture), byteSize.FormatFullWords(unitSystem, "0 B", culture));
        Assert.Equal(bitSize.ToFullWords("0 b", culture), bitSize.FormatFullWords(unitSystem, "0 b", culture));
    }

    [Fact]
    public void LegacyLocaleDataUnitBehaviorRemainsUnchanged()
    {
        var kurdish = CultureInfo.GetCultureInfo("ku");
        Assert.Equal("2 بایتs", ByteSize.FromBytes(2).ToFullWords("0 B", kurdish));
        Assert.Equal("2 کیلۆبایتs", ByteSize.FromKilobytes(2).ToFullWords("0 KB", kurdish));
        Assert.Equal("2 مێگابایتs", ByteSize.FromMegabytes(2).ToFullWords("0 MB", kurdish));

        var lithuanian = CultureInfo.GetCultureInfo("lt");
        Assert.Equal("2 gigabaitass", ByteSize.FromGigabytes(2).ToFullWords("0 GB", lithuanian));

        var ukrainian = CultureInfo.GetCultureInfo("uk");
        var zero = new ByteSize(0);
        Assert.Equal("0 біт", zero.ToFullWords("0 b", ukrainian));
        Assert.Equal("0 байт", zero.ToFullWords("0 B", ukrainian));
        Assert.Equal("0 кілобайт", zero.ToFullWords("0 KB", ukrainian));
        Assert.Equal("0 мегабайт", zero.ToFullWords("0 MB", ukrainian));
        Assert.Equal("0 гігабайт", zero.ToFullWords("0 GB", ukrainian));
        Assert.Equal("0 терабайт", zero.ToFullWords("0 TB", ukrainian));
    }

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi, "MB", "megabytes")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, "MiB", "mebibytes")]
    public void FullWordsUseDecoratedDisplayedCount(
        ByteSizeUnitSystem unitSystem,
        string unit,
        string plural)
    {
        var bytes = unitSystem == ByteSizeUnitSystem.DecimalSi
            ? ByteSize.BytesInDecimalMegabyte
            : ByteSize.BytesInMebibyte;
        var size = ByteSize.FromBytes(bytes);
        var culture = CultureInfo.GetCultureInfo("en-US");

        Assert.Equal($"100% {plural}", size.FormatFullWords(unitSystem, $"0% {unit}", culture));
        Assert.Equal($"-100% {plural}", (-size).FormatFullWords(unitSystem, $"0% {unit}", culture));
        Assert.Equal($"1000‰ {plural}", size.FormatFullWords(unitSystem, $"0‰ {unit}", culture));

        if (unitSystem == ByteSizeUnitSystem.DecimalSi)
        {
            var french = CultureInfo.GetCultureInfo("fr");
            Assert.Equal(
                $"{1d.ToString("0%", french)} mégaoctets",
                size.FormatFullWords(unitSystem, $"0% {unit}", french));
        }
    }

    [Fact]
    public void ExposesDecimalConstantsComputedPropertiesAndFactories()
    {
        var size = ByteSize.FromDecimalExabytes(0.001001001001001001);

        Assert.Equal(1000, ByteSize.BytesInDecimalKilobyte);
        Assert.Equal(1000000, ByteSize.BytesInDecimalMegabyte);
        Assert.Equal(1000000000, ByteSize.BytesInDecimalGigabyte);
        Assert.Equal(1000000000000, ByteSize.BytesInDecimalTerabyte);
        Assert.Equal(1000000000000000, ByteSize.BytesInDecimalPetabyte);
        Assert.Equal(1000000000000000000, ByteSize.BytesInDecimalExabyte);
        Assert.Equal(size.Bytes / 1000, size.DecimalKilobytes);
        Assert.Equal(size.Bytes / 1000000, size.DecimalMegabytes);
        Assert.Equal(size.Bytes / 1000000000, size.DecimalGigabytes);
        Assert.Equal(size.Bytes / 1000000000000, size.DecimalTerabytes);
        Assert.Equal(size.Bytes / 1000000000000000, size.DecimalPetabytes);
        Assert.Equal(size.Bytes / 1000000000000000000, size.DecimalExabytes);
        Assert.Throws<ArgumentOutOfRangeException>(() => ByteSize.FromDecimalExabytes(double.NaN));
        Assert.Throws<ArgumentOutOfRangeException>(() => ByteSize.FromDecimalExabytes(2));

        Func<double, ByteSize>[] factories =
        [
            ByteSize.FromDecimalKilobytes,
            ByteSize.FromDecimalMegabytes,
            ByteSize.FromDecimalGigabytes,
            ByteSize.FromDecimalTerabytes,
            ByteSize.FromDecimalPetabytes,
            ByteSize.FromDecimalExabytes
        ];
        double[] factors =
        [
            ByteSize.BytesInDecimalKilobyte,
            ByteSize.BytesInDecimalMegabyte,
            ByteSize.BytesInDecimalGigabyte,
            ByteSize.BytesInDecimalTerabyte,
            ByteSize.BytesInDecimalPetabyte,
            ByteSize.BytesInDecimalExabyte
        ];

        for (var index = 0; index < factories.Length; index++)
        {
            var boundary = ByteSize.BytesInPebibyte * 1024d / factors[index];

            Assert.Throws<ArgumentOutOfRangeException>(() => factories[index](double.NaN));
            Assert.Throws<ArgumentOutOfRangeException>(() => factories[index](double.PositiveInfinity));
            Assert.Throws<ArgumentOutOfRangeException>(() => factories[index](double.NegativeInfinity));
            Assert.Throws<ArgumentOutOfRangeException>(() => factories[index](boundary));
            Assert.Equal(long.MinValue, factories[index](-boundary).Bits);
        }
    }

    [Theory]
    [InlineData(999, "999 B", "999 B")]
    [InlineData(1000, "1 kB", "1000 B")]
    [InlineData(1023, "1.02 kB", "1023 B")]
    [InlineData(1024, "1.02 kB", "1 KiB")]
    [InlineData(1000000, "1 MB", "976.56 KiB")]
    [InlineData(1048576, "1.05 MB", "1 MiB")]
    [InlineData(-1000, "-1 kB", "-1000 B")]
    public void FormatsAutomaticSystemBoundaries(double bytes, string decimalExpected, string binaryExpected)
    {
        var size = ByteSize.FromBytes(bytes);

        Assert.Equal(decimalExpected, size.Format(ByteSizeUnitSystem.DecimalSi));
        Assert.Equal(binaryExpected, size.Format(ByteSizeUnitSystem.BinaryIec));
    }

    [Fact]
    public void FormatsExplicitUnitsWordsCompositeAndRates()
    {
        var size = ByteSize.FromBytes(1001001);

        Assert.Equal("1.001001 MB", size.Format(ByteSizeUnitSystem.DecimalSi, "0.000000 MB"));
        Assert.Equal("1 megabyte", ByteSize.FromDecimalMegabytes(1).FormatFullWords(ByteSizeUnitSystem.DecimalSi));
        Assert.Equal("1 MB 1 kB 1 B", size.HumanizeCompositeWithUnitSystem(ByteSizeUnitSystem.DecimalSi, precision: 3));
        Assert.Equal(
            "-1 MiB 1 KiB 1 B",
            ByteSize.FromBytes(-1049601).HumanizeCompositeWithUnitSystem(ByteSizeUnitSystem.BinaryIec, precision: 3));
        Assert.Equal(
            "1 MB/s",
            ByteSize.FromDecimalMegabytes(1)
                .Per(TimeSpan.FromSeconds(1))
                .HumanizeWithUnitSystem(ByteSizeUnitSystem.DecimalSi));
        Assert.Equal(
            "1 MiB/s",
            ByteSize.FromMebibytes(1)
                .Per(TimeSpan.FromSeconds(1))
                .HumanizeWithUnitSystem(ByteSizeUnitSystem.BinaryIec));
    }

    [Theory]
    [InlineData(0, ByteSizeUnitSystem.DecimalSi, "0 b")]
    [InlineData(0, ByteSizeUnitSystem.BinaryIec, "0 b")]
    [InlineData(long.MaxValue, ByteSizeUnitSystem.DecimalSi, "1.15 EB")]
    [InlineData(long.MinValue, ByteSizeUnitSystem.DecimalSi, "-1.15 EB")]
    [InlineData(long.MaxValue, ByteSizeUnitSystem.BinaryIec, "1024 PiB")]
    [InlineData(long.MinValue, ByteSizeUnitSystem.BinaryIec, "-1024 PiB")]
    public void FormatsZeroAndBitRangeLimits(long bits, ByteSizeUnitSystem unitSystem, string expected) =>
        Assert.Equal(expected, ByteSize.FromBits(bits).Format(unitSystem));

    [Theory]
    [InlineData("el", 2, ByteSizeUnitSystem.BinaryIec, "GiB", "2 γκιμπιμπάιτ")]
    [InlineData("he", 0, ByteSizeUnitSystem.DecimalSi, "EB", "0 אקסה-בייטים")]
    [InlineData("ig", 2, ByteSizeUnitSystem.DecimalSi, "GB", "2 jigabaịt")]
    [InlineData("ku", 0, ByteSizeUnitSystem.DecimalSi, "EB", "0 ئێگزابایت")]
    [InlineData("my", 2, ByteSizeUnitSystem.BinaryIec, "MiB", "2 မက်ဘီဘိုက်")]
    [InlineData("so", 2, ByteSizeUnitSystem.DecimalSi, "PB", "2 betabeyt")]
    [InlineData("zh-Hans", 2, ByteSizeUnitSystem.BinaryIec, "PiB", "2 二进制拍字节")]
    [InlineData("zh-Hant", 2, ByteSizeUnitSystem.BinaryIec, "PiB", "2 二進位拍位元組")]
    [InlineData("zu-ZA", 2, ByteSizeUnitSystem.BinaryIec, "PiB", "2 ama-pebibytes")]
    public void UsesReviewedLocaleOwnedUnitWords(
        string locale,
        int quantity,
        ByteSizeUnitSystem unitSystem,
        string symbol,
        string expected)
    {
        var size = ByteSize.ParseWithUnitSystem($"{quantity} {symbol}", unitSystem, CultureInfo.InvariantCulture);

        Assert.Equal(
            expected,
            size.FormatFullWords(unitSystem, $"0 {symbol}", CultureInfo.GetCultureInfo(locale)));
    }

    [Fact]
    public void PromotesAutomaticUnitsUsingTheRequestedPrecision()
    {
        Assert.Equal(
            "8 b",
            ByteSize.FromBytes(0.999).Format(ByteSizeUnitSystem.DecimalSi, "0"));
        Assert.Equal(
            "-8 b",
            ByteSize.FromBytes(-0.999).Format(ByteSizeUnitSystem.DecimalSi, "0"));
        Assert.Equal(
            "1 kB",
            ByteSize.FromBytes(999.6).Format(ByteSizeUnitSystem.DecimalSi, "0"));
        Assert.Equal(
            "1 KiB",
            ByteSize.FromBytes(1023.6).Format(ByteSizeUnitSystem.BinaryIec, "0"));
        Assert.Equal(
            "1 MB",
            ByteSize.FromBytes(999600).Format(ByteSizeUnitSystem.DecimalSi, "0"));
        Assert.Equal(
            "1 megabyte",
            ByteSize.FromBytes(999600).FormatFullWords(ByteSizeUnitSystem.DecimalSi, "0"));
        Assert.Equal(
            "1 MiB",
            ByteSize.FromBytes(1023.6 * 1024).Format(ByteSizeUnitSystem.BinaryIec, "0"));
        Assert.Equal(
            "999.600 kB",
            ByteSize.FromBytes(999600).Format(ByteSizeUnitSystem.DecimalSi, "0.000"));
        Assert.Equal(
            "1E+0 MB",
            ByteSize.FromBytes(999600).Format(ByteSizeUnitSystem.DecimalSi, "0E+0"));
        Assert.Equal(
            "999.999 B",
            ByteSize.FromBytes(999.9994).Format(
                ByteSizeUnitSystem.DecimalSi,
                "F3",
                CultureInfo.InvariantCulture));
        Assert.Equal(
            "1.000 kB",
            ByteSize.FromBytes(999.9996).Format(
                ByteSizeUnitSystem.DecimalSi,
                "F3",
                CultureInfo.InvariantCulture));
        Assert.Equal(
            "1023.999 B",
            ByteSize.FromBytes(1023.9994).Format(
                ByteSizeUnitSystem.BinaryIec,
                "F3",
                CultureInfo.InvariantCulture));
        Assert.Equal(
            "1.000 KiB",
            ByteSize.FromBytes(1023.9996).Format(
                ByteSizeUnitSystem.BinaryIec,
                "F3",
                CultureInfo.InvariantCulture));
        Assert.Equal(
            "(1) kB",
            ByteSize.FromBytes(-999.6).Format(
                ByteSizeUnitSystem.DecimalSi,
                "0;(0);0",
                CultureInfo.InvariantCulture));
        Assert.Equal(
            "(1) KiB",
            ByteSize.FromBytes(-1023.6).Format(
                ByteSizeUnitSystem.BinaryIec,
                "0;(0);0",
                CultureInfo.InvariantCulture));
        var french = CultureInfo.GetCultureInfo("fr");
        Assert.Equal(
            "[1,0] kB",
            ByteSize.FromBytes(-999.96).Format(ByteSizeUnitSystem.DecimalSi, "0.0;[0.0];0.0", french));
        Assert.Equal(
            "[1,0] KiB",
            ByteSize.FromBytes(-1023.96).Format(ByteSizeUnitSystem.BinaryIec, "0.0;[0.0];0.0", french));
    }

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi, "0 'gibberish'", "1 gibberish B")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, "0 'MB'", "1 MB B")]
    [InlineData(ByteSizeUnitSystem.DecimalSi, "0 \\M\\i\\B", "1 MiB B")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, "0 \\M\\B", "1 MB B")]
    public void IgnoresQuotedAndEscapedUnitLikeLiterals(
        ByteSizeUnitSystem unitSystem,
        string format,
        string expected) =>
        Assert.Equal(
            expected,
            ByteSize.FromBytes(1).Format(unitSystem, format, CultureInfo.InvariantCulture));

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi, -1000, "0.0 'MiB' kB;-0.0 'MiB' kB;0.0 'MiB' kB", "-1.0 MiB kB")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, -1024, "0.0 'MB' KiB;-0.0 'MB' KiB;0.0 'MB' KiB", "-1.0 MB KiB")]
    public void SelectsOnlyActiveUnitTokensAcrossCustomFormatSections(
        ByteSizeUnitSystem unitSystem,
        double bytes,
        string format,
        string expected) =>
        Assert.Equal(
            expected,
            ByteSize.FromBytes(bytes).Format(unitSystem, format, CultureInfo.InvariantCulture));

    [Theory]
    [InlineData("1 kB", ByteSizeUnitSystem.DecimalSi, 1000)]
    [InlineData("1 KB", ByteSizeUnitSystem.DecimalSi, 1000)]
    [InlineData("1.5 MB", ByteSizeUnitSystem.DecimalSi, 1500000)]
    [InlineData("1 KiB", ByteSizeUnitSystem.BinaryIec, 1024)]
    [InlineData("1.5 MiB", ByteSizeUnitSystem.BinaryIec, 1572864)]
    public void ParsesOnlySelectedSystemTokens(string text, ByteSizeUnitSystem unitSystem, double expectedBytes)
    {
        Assert.True(ByteSize.TryParseWithUnitSystem(text, unitSystem, out var parsed));
        Assert.Equal(expectedBytes, parsed.Bytes);
        Assert.Equal(parsed, ByteSize.ParseWithUnitSystem(text, unitSystem));
        Assert.True(ByteSize.TryParseSpanWithUnitSystem(text.AsSpan(), unitSystem, out parsed));
    }

    [Fact]
    public void RejectsAmbiguousOrUnknownExplicitSystems()
    {
        Assert.False(ByteSize.TryParseWithUnitSystem("1 MB", ByteSizeUnitSystem.BinaryIec, out _));
        Assert.False(ByteSize.TryParseWithUnitSystem("1 MiB", ByteSizeUnitSystem.DecimalSi, out _));
        Assert.False(ByteSize.TryParseWithUnitSystem("1 EiB", ByteSizeUnitSystem.BinaryIec, out _));
        Assert.Throws<FormatException>(() => ByteSize.FromBytes(1).Format(ByteSizeUnitSystem.BinaryIec, "0 EiB"));
        Assert.Throws<FormatException>(() => ByteSize.FromBytes(1).Format(ByteSizeUnitSystem.DecimalSi, "0 MB MiB"));
        Assert.Throws<FormatException>(() => ByteSize.FromBytes(1).Format(ByteSizeUnitSystem.BinaryIec, "0 KiB kB"));
        Assert.Throws<FormatException>(() => ByteSize.FromDecimalMegabytes(1).Format(ByteSizeUnitSystem.DecimalSi, "0 MB kB"));
        Assert.Throws<FormatException>(() => ByteSize.FromBytes(1).Format(ByteSizeUnitSystem.DecimalSi, "0 B b"));
        Assert.Throws<FormatException>(() => ByteSize.FromDecimalMegabytes(1).Format(ByteSizeUnitSystem.DecimalSi, "0 MB B"));
        Assert.EndsWith(" PiB", ByteSize.MaxValue.Format(ByteSizeUnitSystem.BinaryIec));
        Assert.Throws<ArgumentOutOfRangeException>(() => ByteSize.FromBytes(1).Format((ByteSizeUnitSystem)99));
        Assert.Throws<ArgumentOutOfRangeException>(() => ByteSize.TryParseWithUnitSystem("1 B", (ByteSizeUnitSystem)99, out _));
    }

    [Fact]
    public void ParsesExactBitRange()
    {
        Assert.Equal(
            ByteSize.FromBits(long.MaxValue),
            ByteSize.ParseWithUnitSystem($"{long.MaxValue} b", ByteSizeUnitSystem.DecimalSi));
        Assert.Equal(
            ByteSize.FromBits(long.MinValue),
            ByteSize.ParseWithUnitSystem($"{long.MinValue} b", ByteSizeUnitSystem.BinaryIec));
        Assert.False(ByteSize.TryParseWithUnitSystem("9223372036854775808 b", ByteSizeUnitSystem.DecimalSi, out _));
        Assert.False(ByteSize.TryParseWithUnitSystem("-9223372036854775809 b", ByteSizeUnitSystem.BinaryIec, out _));
    }

    [Theory]
    [InlineData(long.MaxValue, ByteSizeUnitSystem.DecimalSi, "9223372036854775807 b")]
    [InlineData(long.MinValue, ByteSizeUnitSystem.DecimalSi, "-9223372036854775808 b")]
    [InlineData(long.MaxValue, ByteSizeUnitSystem.BinaryIec, "9223372036854775807 b")]
    [InlineData(long.MinValue, ByteSizeUnitSystem.BinaryIec, "-9223372036854775808 b")]
    public void FormatsAndParsesExactBitRange(
        long bits,
        ByteSizeUnitSystem unitSystem,
        string expected)
    {
        var formatted = ByteSize.FromBits(bits).Format(unitSystem, "0 b", CultureInfo.InvariantCulture);

        Assert.Equal(expected, formatted);
        Assert.Equal(
            ByteSize.FromBits(bits),
            ByteSize.ParseWithUnitSystem(formatted, unitSystem, CultureInfo.InvariantCulture));
    }

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi)]
    [InlineData(ByteSizeUnitSystem.BinaryIec)]
    public void ExplicitBitFormatUsesStoredBitsForNegativeSubByte(ByteSizeUnitSystem unitSystem)
    {
        var size = ByteSize.FromBytes(-0.999);
        var formatted = size.Format(unitSystem, "0 b", CultureInfo.InvariantCulture);

        Assert.Equal(-7, size.Bits);
        Assert.Equal("-7 b", formatted);
        Assert.Equal("-8 b", size.Format(unitSystem, formatProvider: CultureInfo.InvariantCulture));
        Assert.Equal(
            size.Bits,
            ByteSize.ParseWithUnitSystem(formatted, unitSystem, CultureInfo.InvariantCulture).Bits);
    }

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi)]
    [InlineData(ByteSizeUnitSystem.BinaryIec)]
    public void ExplicitBitFullWordsUsesStoredBitsForInflection(ByteSizeUnitSystem unitSystem)
    {
        var size = ByteSize.FromBytes(-0.249);

        Assert.Equal(-1, size.Bits);
        Assert.Equal(
            "-1 bit",
            size.FormatFullWords(unitSystem, "0 b", CultureInfo.InvariantCulture));
        Assert.Equal(
            "-2 bits",
            ByteSize.FromBits(-2).FormatFullWords(
                unitSystem,
                "0 b",
                CultureInfo.GetCultureInfo("en")));
    }

    [Theory]
    [InlineData(9007199254741001, "9007199254741001 bit")]
    [InlineData(long.MaxValue, "9223372036854775807 bitov")]
    [InlineData(long.MinValue, "−9223372036854775808 bitov")]
    public void ExplicitBitFullWordsPreserveWideLocaleGrammar(long bits, string expected) =>
        Assert.Equal(
            expected,
            ByteSize.FromBits(bits).FormatFullWords(
                ByteSizeUnitSystem.DecimalSi,
                "0 b",
                CultureInfo.GetCultureInfo("sl")));

    [Fact]
    public void ExplicitBitFullWordsUseDisplayedStandardFormatForWideLocaleGrammar() =>
        Assert.Equal(
            "9E+015 bitov",
            ByteSize.FromBits(9_007_199_254_741_001).FormatFullWords(
                ByteSizeUnitSystem.DecimalSi,
                "E0 b",
                CultureInfo.GetCultureInfo("sl")));

    [Fact]
    public void ExplicitFullWordsPreserveWidePolishModuloGrammar() =>
        Assert.Equal(
            "2147483652 kilobajty",
            ByteSize.FromDecimalKilobytes(2_147_483_652).FormatFullWords(
                ByteSizeUnitSystem.DecimalSi,
                "0 kB",
                CultureInfo.GetCultureInfo("pl")));

    [Fact]
    public void ExplicitFullWordsPreserveHebrewIntegralAndFractionalForms()
    {
        var culture = CultureInfo.GetCultureInfo("he");

        Assert.Equal(
            "1 קילובייט",
            ByteSize.FromDecimalKilobytes(1).FormatFullWords(
                ByteSizeUnitSystem.DecimalSi,
                "0 kB",
                culture));
        Assert.Equal(
            "1.5 קילובייטים",
            ByteSize.FromDecimalKilobytes(1.5).FormatFullWords(
                ByteSizeUnitSystem.DecimalSi,
                "0.0 kB",
                culture));
    }

    [Fact]
    public void PreservesTypedNullAndDefaultLegacyBindings()
    {
        string? format = default;
        IFormatProvider? provider = default;

        Assert.Equal("1 KB", ByteSize.FromKilobytes(1).ToString(format, provider));
        Assert.Equal("KB", ByteSize.FromKilobytes(1).GetLargestWholeNumberSymbol(provider));
        Assert.Equal("kilobyte", ByteSize.FromKilobytes(1).GetLargestWholeNumberFullWord(provider));
        Assert.Equal("1 kilobyte", ByteSize.FromKilobytes(1).ToFullWords(format, provider));
        Assert.Equal("1 KB", ByteSize.FromKilobytes(1).Humanize(format, provider));
        Assert.Throws<ArgumentOutOfRangeException>(() => ByteSize.FromBytes(1).HumanizeComposite(default(int)));
        Assert.Equal("1 KB/s", ByteSize.FromKilobytes(1).Per(TimeSpan.FromSeconds(1)).Humanize(format));
    }

    [Fact]
    public void ProvidesGeneratedSystemUnitsForEveryShippedLocale()
    {
        (double Bytes, string Symbol, DataUnit DataUnit)[] decimalUnits =
        [
            (ByteSize.BytesInDecimalKilobyte, "kB", DataUnit.DecimalKilobyte),
            (ByteSize.BytesInDecimalMegabyte, "MB", DataUnit.DecimalMegabyte),
            (ByteSize.BytesInDecimalGigabyte, "GB", DataUnit.DecimalGigabyte),
            (ByteSize.BytesInDecimalTerabyte, "TB", DataUnit.DecimalTerabyte),
            (ByteSize.BytesInDecimalPetabyte, "PB", DataUnit.DecimalPetabyte),
            (ByteSize.BytesInDecimalExabyte, "EB", DataUnit.DecimalExabyte)
        ];
        (double Bytes, string Symbol, DataUnit DataUnit)[] binaryUnits =
        [
            (ByteSize.BytesInKibibyte, "KiB", DataUnit.BinaryKibibyte),
            (ByteSize.BytesInMebibyte, "MiB", DataUnit.BinaryMebibyte),
            (ByteSize.BytesInGibibyte, "GiB", DataUnit.BinaryGibibyte),
            (ByteSize.BytesInTebibyte, "TiB", DataUnit.BinaryTebibyte),
            (ByteSize.BytesInPebibyte, "PiB", DataUnit.BinaryPebibyte)
        ];

        foreach (var localeName in LocaleCoverageData.ShippedLocales)
        {
            var culture = CultureInfo.GetCultureInfo(localeName);
            var phraseTable = Assert.IsType<LocalePhraseTable>(LocalePhraseTableCatalog.Resolve(culture));
            var decimalSize = ByteSize.FromDecimalKilobytes(2);
            var binarySize = ByteSize.FromKibibytes(2);
            var decimalText = ByteSize.FromBytes(2500).Format(ByteSizeUnitSystem.DecimalSi, "0.0 kB", culture);
            var binaryText = ByteSize.FromBytes(2560).Format(ByteSizeUnitSystem.BinaryIec, "0.0 KiB", culture);

            Assert.EndsWith(" kB", decimalSize.Format(ByteSizeUnitSystem.DecimalSi, formatProvider: culture));
            Assert.EndsWith(" KiB", binarySize.Format(ByteSizeUnitSystem.BinaryIec, formatProvider: culture));
            Assert.NotEqual(
                decimalSize.Format(ByteSizeUnitSystem.DecimalSi, formatProvider: culture),
                decimalSize.FormatFullWords(ByteSizeUnitSystem.DecimalSi, formatProvider: culture));
            Assert.NotEqual(
                binarySize.Format(ByteSizeUnitSystem.BinaryIec, formatProvider: culture),
                binarySize.FormatFullWords(ByteSizeUnitSystem.BinaryIec, formatProvider: culture));
            Assert.Equal(ByteSize.FromBytes(2500), ByteSize.ParseWithUnitSystem(decimalText, ByteSizeUnitSystem.DecimalSi, culture));
            Assert.Equal(ByteSize.FromBytes(2560), ByteSize.ParseWithUnitSystem(binaryText, ByteSizeUnitSystem.BinaryIec, culture));
            Assert.EndsWith(" kB 1 B", ByteSize.FromBytes(2001).HumanizeCompositeWithUnitSystem(
                ByteSizeUnitSystem.DecimalSi,
                formatProvider: culture));
            Assert.Contains("kB/", ByteSize.FromDecimalKilobytes(2)
                .Per(TimeSpan.FromSeconds(1))
                .HumanizeWithUnitSystem(ByteSizeUnitSystem.DecimalSi, culture: culture));

            verifyUnits(decimalUnits, ByteSizeUnitSystem.DecimalSi);
            verifyUnits(binaryUnits, ByteSizeUnitSystem.BinaryIec);

            void verifyUnits(
                IEnumerable<(double Bytes, string Symbol, DataUnit DataUnit)> units,
                ByteSizeUnitSystem unitSystem)
            {
                foreach (var unit in units)
                {
                    Assert.True(phraseTable.TryGetDataUnitPhrase(unit.DataUnit, out var phrase));
                    Assert.NotNull(phrase.Forms);
                    Assert.Equal(unit.Symbol, phrase.Symbol);

                    var size = ByteSize.FromBytes(unit.Bytes);
                    var formatted = size.Format(unitSystem, $"0 {unit.Symbol}", culture);

                    Assert.Equal($"1 {unit.Symbol}", formatted);
                    Assert.Equal(size, ByteSize.ParseWithUnitSystem(formatted, unitSystem, culture));
                    Assert.NotEqual(formatted, size.FormatFullWords(unitSystem, $"0 {unit.Symbol}", culture));
                    Assert.Equal($"1 {unit.Symbol}", size.Format(unitSystem, formatProvider: culture));
                }
            }
        }
    }
}