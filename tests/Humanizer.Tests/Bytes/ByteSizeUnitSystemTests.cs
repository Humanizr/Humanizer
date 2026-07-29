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
    }

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