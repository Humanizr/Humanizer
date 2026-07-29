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
        Assert.Equal(
            "9007199254740990 b",
            ByteSize.FromBits(9_007_199_254_740_993)
                .Format(ByteSizeUnitSystem.Legacy, "0 b", CultureInfo.InvariantCulture));
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

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi, "MB", ByteSize.BytesInDecimalMegabyte, "en-150", "megabyte #.## 0")]
    [InlineData(ByteSizeUnitSystem.DecimalSi, "MB", ByteSize.BytesInDecimalMegabyte, "en-MY", "megabyte")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, "MiB", ByteSize.BytesInMebibyte, "en-150", "mebibyte #.## 0")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, "MiB", ByteSize.BytesInMebibyte, "en-MY", "mebibyte")]
    public void CustomFormattersReceiveDisplayedCount(
        ByteSizeUnitSystem unitSystem,
        string unit,
        double bytes,
        string locale,
        string expectedUnitText) =>
        Assert.Equal(
            $"1 {expectedUnitText}",
            ByteSize.FromBytes(1.2 * bytes).FormatFullWords(
                unitSystem,
                $"0 {unit}",
                CultureInfo.GetCultureInfo(locale)));

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi, "MB", ByteSize.BytesInDecimalMegabyte, "megabyte")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, "MiB", ByteSize.BytesInMebibyte, "mebibyte")]
    public void UnitOnlyFullWordsUseDisplayedCount(
        ByteSizeUnitSystem unitSystem,
        string unit,
        double bytes,
        string singular)
    {
        var size = ByteSize.FromBytes(1.004 * bytes);

        Assert.Equal(
            $"1 {singular}",
            size.FormatFullWords(unitSystem, unit, CultureInfo.InvariantCulture));
        Assert.Equal(
            $"1 {singular} #.## 0",
            size.FormatFullWords(unitSystem, unit, CultureInfo.GetCultureInfo("en-150")));
    }

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi, "MB", ByteSize.BytesInDecimalMegabyte, "megabyte", "megabytes")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, "MiB", ByteSize.BytesInMebibyte, "mebibyte", "mebibytes")]
    public void RegisteredFormatterUnitTextIsOpaqueToNumericFormatting(
        ByteSizeUnitSystem unitSystem,
        string unit,
        double bytes,
        string singular,
        string plural)
    {
        var culture = CultureInfo.GetCultureInfo("en-150");
        var size = ByteSize.FromBytes(1.2 * bytes);
        var expected = $"1.2 {plural} #.## 0";

        Assert.Equal(expected, size.FormatFullWords(unitSystem, $"0.0 {unit}", culture));
        Assert.Equal(expected, size.FormatFullWords(unitSystem, unit, culture));

        var customCulture = (CultureInfo)culture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = "\u0001";
        Assert.Equal(
            $"1\u00012 {plural} #.## 0",
            size.FormatFullWords(unitSystem, $"0.0 {unit}", customCulture));

        Assert.Equal(
            $"1\u0001{singular} #.## 0",
            size.FormatFullWords(unitSystem, $"0'\u0001'{unit}", culture));

        customCulture.NumberFormat.CurrencySymbol = "\u0001";
        customCulture.NumberFormat.CurrencyPositivePattern = 1;
        Assert.Equal(
            $"1\u0001{singular} #.## 0",
            size.FormatFullWords(unitSystem, $"C0{unit}", customCulture));
    }

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi, "MB", "megabyte")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, "MiB", "mebibyte")]
    public void StandardPercentFormatsUsePercentSeparators(
        ByteSizeUnitSystem unitSystem,
        string unit,
        string singular)
    {
        var numberFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        numberFormat.PercentDecimalSeparator = ",";
        numberFormat.PercentGroupSeparator = ".";
        numberFormat.PercentPositivePattern = 1;
        var bytes = unitSystem == ByteSizeUnitSystem.DecimalSi
            ? ByteSize.BytesInDecimalMegabyte
            : ByteSize.BytesInMebibyte;

        Assert.Equal(
            $"{0.01.ToString("P1", numberFormat)} {singular}",
            ByteSize.FromBytes(0.01 * bytes).FormatFullWords(unitSystem, $"P1 {unit}", numberFormat));
        Assert.Equal(
            $"1.0 {singular}",
            ByteSize.FromBytes(bytes).FormatFullWords(unitSystem, $"0.0 {unit}", numberFormat));
        Assert.Equal(
            $"1.0{numberFormat.PerMilleSymbol} {singular}",
            ByteSize.FromBytes(0.001 * bytes).FormatFullWords(unitSystem, $"0.0‰ {unit}", numberFormat));
    }

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi, "MB", "megabytes", 9.996)]
    [InlineData(ByteSizeUnitSystem.BinaryIec, "MiB", "mebibytes", 10.236)]
    public void StandardPercentFormatsUseTrailingNegativePatterns(
        ByteSizeUnitSystem unitSystem,
        string unit,
        string plural,
        double carryBytes)
    {
        var numberFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        var bytes = unitSystem == ByteSizeUnitSystem.DecimalSi
            ? ByteSize.BytesInDecimalMegabyte
            : ByteSize.BytesInMebibyte;
        var promotedUnit = unitSystem == ByteSizeUnitSystem.DecimalSi ? "kB" : "KiB";

        foreach (var pattern in new[] { 5, 7 })
        {
            numberFormat.PercentNegativePattern = pattern;
            Assert.Equal(
                $"{(-0.016).ToString("P0", numberFormat)} {plural}",
                ByteSize.FromBytes(-0.016 * bytes).FormatFullWords(unitSystem, $"P0 {unit}", numberFormat));
            Assert.Equal(
                $"{(-0.009996).ToString("P0", numberFormat)} {promotedUnit}",
                ByteSize.FromBytes(-carryBytes).Format(unitSystem, "P0", numberFormat));
        }
    }

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi, "MB", "megabyte", "megabytes")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, "MiB", "mebibyte", "mebibytes")]
    public void StandardCurrencyFormatsUseDisplayedCount(
        ByteSizeUnitSystem unitSystem,
        string unit,
        string singular,
        string plural)
    {
        var bytes = unitSystem == ByteSizeUnitSystem.DecimalSi
            ? ByteSize.BytesInDecimalMegabyte
            : ByteSize.BytesInMebibyte;
        var culture = CultureInfo.GetCultureInfo("en-US");

        Assert.Equal(
            $"{1.2.ToString("C0", culture)} {singular}",
            ByteSize.FromBytes(1.2 * bytes).FormatFullWords(unitSystem, $"C0 {unit}", culture));
        Assert.Equal(
            $"{1.6.ToString("C0", culture)} {plural}",
            ByteSize.FromBytes(1.6 * bytes).FormatFullWords(unitSystem, $"C0 {unit}", culture));

        var boundary = unitSystem == ByteSizeUnitSystem.DecimalSi ? 999.6 : 1023.6;
        var promotedUnit = unitSystem == ByteSizeUnitSystem.DecimalSi ? "kB" : "KiB";
        Assert.Equal(
            $"{0.9996.ToString("C0", culture)} {promotedUnit}",
            ByteSize.FromBytes(boundary).Format(unitSystem, "C0", culture));
    }

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi, "MB", "mégaoctet")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, "MiB", "mébioctet")]
    public void StandardCurrencyFormatsUseCultureCurrencySeparators(
        ByteSizeUnitSystem unitSystem,
        string unit,
        string singular)
    {
        var culture = CultureInfo.GetCultureInfo("fr-FR");
        var bytes = unitSystem == ByteSizeUnitSystem.DecimalSi
            ? ByteSize.BytesInDecimalMegabyte
            : ByteSize.BytesInMebibyte;

        Assert.Equal(
            $"{1.2.ToString("C0", culture)} {singular}",
            ByteSize.FromBytes(1.2 * bytes).FormatFullWords(unitSystem, $"C0 {unit}", culture));
    }

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi, "B", "byte")]
    [InlineData(ByteSizeUnitSystem.DecimalSi, "b", "bit")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, "B", "byte")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, "b", "bit")]
    public void StandardCurrencyFormatsDoNotReplaceCurrencySymbolAsUnit(
        ByteSizeUnitSystem unitSystem,
        string symbol,
        string word)
    {
        var numberFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        numberFormat.CurrencySymbol = symbol;
        var size = symbol == ByteSize.ByteSymbol
            ? ByteSize.FromBytes(1)
            : ByteSize.ParseWithUnitSystem("1 b", unitSystem, CultureInfo.InvariantCulture);

        Assert.Equal(
            $"{symbol}1 {word}",
            size.FormatFullWords(unitSystem, $"C0 {symbol}", numberFormat));
    }

    [Fact]
    public void ExplicitFullWordsHandleTinyDisplayedDecimals() =>
        Assert.Equal(
            "0.0000000000000000000000000001 kilobyte",
            ByteSize.FromDecimalKilobytes(1e-28).FormatFullWords(
                ByteSizeUnitSystem.DecimalSi,
                "0.0000000000000000000000000000 kB",
                CultureInfo.InvariantCulture));

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi, ByteSize.BytesInDecimalKilobyte, "kB", "kilobytes")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, ByteSize.BytesInKibibyte, "KiB", "kibibytes")]
    public void EnglishExactFullWordsPluralizeZeroAndFractionalOther(
        ByteSizeUnitSystem unitSystem,
        double bytes,
        string symbol,
        string plural)
    {
        var culture = CultureInfo.GetCultureInfo("en");

        Assert.Equal(
            $"0 {plural}",
            ByteSize.FromBytes(0).FormatFullWords(unitSystem, $"0 {symbol}", culture));
        Assert.Equal(
            $"0.5 {plural}",
            ByteSize.FromBytes(0.5 * bytes).FormatFullWords(unitSystem, $"0.0 {symbol}", culture));
    }

    [Theory]
    [InlineData("fr", 0, "0 kilooctet", "0 kibioctet")]
    [InlineData("pt", 0, "0 kilobyte", "0 kibibyte")]
    [InlineData("da", 0.5, "0,5 kilobyte", "0,5 kibibyte")]
    [InlineData("fil", 4, "4 kilobytes", "4 kibibytes")]
    [InlineData("mk", 21, "21 килобајт", "21 кибибајт")]
    [InlineData("mk", 1.1, "1,1 килобајт", "1,1 кибибајт")]
    public void ExactFullWordsUseCanonicalOneCategory(
        string cultureName,
        double count,
        string expectedDecimal,
        string expectedBinary)
    {
        var culture = CultureInfo.GetCultureInfo(cultureName);

        Assert.Equal(
            expectedDecimal,
            ByteSize.FromBytes(count * ByteSize.BytesInDecimalKilobyte)
                .FormatFullWords(ByteSizeUnitSystem.DecimalSi, "0.## kB", culture));
        Assert.Equal(
            expectedBinary,
            ByteSize.FromBytes(count * ByteSize.BytesInKibibyte)
                .FormatFullWords(ByteSizeUnitSystem.BinaryIec, "0.## KiB", culture));
    }

    [Fact]
    public void StandardFormatsDoNotReplaceTextInsideUnitToken()
    {
        var culture = CultureInfo.InvariantCulture;

        Assert.Equal(
            $"{1d.ToString("G", culture)} GB",
            ByteSize.FromDecimalGigabytes(1).Format(ByteSizeUnitSystem.DecimalSi, "G GB", culture));
        Assert.Equal(
            $"{1d.ToString("P", culture)} PB",
            ByteSize.FromDecimalPetabytes(1).Format(ByteSizeUnitSystem.DecimalSi, "P PB", culture));
        Assert.Equal(
            $"{1d.ToString("E", culture)} EB",
            ByteSize.FromDecimalExabytes(1).Format(ByteSizeUnitSystem.DecimalSi, "E EB", culture));
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
            "-7 b",
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
    [InlineData(ByteSizeUnitSystem.DecimalSi, 999999.6, "1‰ GB", "1‰ gigabyte")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, 1048575.6, "1000‰ MiB", "1000‰ mebibytes")]
    public void RepeatsAutomaticCarryUntilDisplayedCountFits(
        ByteSizeUnitSystem unitSystem,
        double bytes,
        string expectedSymbol,
        string expectedWords)
    {
        var size = ByteSize.FromBytes(bytes);

        Assert.Equal(expectedSymbol, size.Format(unitSystem, "0‰", CultureInfo.InvariantCulture));
        Assert.Equal(expectedWords, size.FormatFullWords(unitSystem, "0‰", CultureInfo.InvariantCulture));
    }

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi, 1, "125‰ B", "125‰ bytes")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, 2, "250‰ B", "250‰ bytes")]
    public void CarriesAutomaticBitsToBytes(
        ByteSizeUnitSystem unitSystem,
        long bits,
        string expectedSymbol,
        string expectedWords)
    {
        var size = ByteSize.FromBits(bits);

        Assert.Equal(expectedSymbol, size.Format(unitSystem, "0‰", CultureInfo.InvariantCulture));
        Assert.Equal(expectedWords, size.FormatFullWords(unitSystem, "0‰", CultureInfo.InvariantCulture));
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
            long.MaxValue,
            ByteSize.ParseWithUnitSystem($"{long.MaxValue} b", ByteSizeUnitSystem.DecimalSi).Bits);
        Assert.Equal(
            long.MinValue,
            ByteSize.ParseWithUnitSystem($"{long.MinValue} b", ByteSizeUnitSystem.BinaryIec).Bits);
        Assert.False(ByteSize.TryParseWithUnitSystem("9223372036854775808 b", ByteSizeUnitSystem.DecimalSi, out _));
        Assert.False(ByteSize.TryParseWithUnitSystem("-9223372036854775809 b", ByteSizeUnitSystem.BinaryIec, out _));
    }

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi)]
    [InlineData(ByteSizeUnitSystem.BinaryIec)]
    public void ParsesIntegralFixedPointBitsExactly(ByteSizeUnitSystem unitSystem)
    {
        var parsed = ByteSize.ParseWithUnitSystem("9007199254740993.0 b", unitSystem, CultureInfo.InvariantCulture);

        Assert.Equal(9_007_199_254_740_993, parsed.Bits);
        Assert.Equal(
            "9007199254740993 b",
            parsed.Format(unitSystem, "0 b", CultureInfo.InvariantCulture));
        Assert.False(ByteSize.TryParseWithUnitSystem("1.1 b", unitSystem, CultureInfo.InvariantCulture, out _));
        Assert.False(ByteSize.TryParseWithUnitSystem("9223372036854775808.0 b", unitSystem, CultureInfo.InvariantCulture, out _));
        Assert.False(ByteSize.TryParseWithUnitSystem("-9223372036854775809.0 b", unitSystem, CultureInfo.InvariantCulture, out _));
    }

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi, "MB", ByteSize.BytesInDecimalMegabyte)]
    [InlineData(ByteSizeUnitSystem.BinaryIec, "MiB", ByteSize.BytesInMebibyte)]
    public void ParsesScientificExplicitOutput(
        ByteSizeUnitSystem unitSystem,
        string symbol,
        double bytes)
    {
        var size = ByteSize.FromBytes(bytes);
        var formatted = size.Format(unitSystem, $"E0 {symbol}", CultureInfo.InvariantCulture);

        Assert.Equal(size, ByteSize.ParseWithUnitSystem(formatted, unitSystem, CultureInfo.InvariantCulture));
        Assert.Equal(
            15,
            ByteSize.ParseWithUnitSystem("1.5E+001 b", unitSystem, CultureInfo.InvariantCulture).Bits);
        Assert.False(ByteSize.TryParseWithUnitSystem("1.5E-001 b", unitSystem, CultureInfo.InvariantCulture, out _));
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
        var size = ByteSize.ParseWithUnitSystem($"{bits} b", unitSystem, CultureInfo.InvariantCulture);
        var formatted = size.Format(unitSystem, "0 b", CultureInfo.InvariantCulture);

        Assert.Equal(expected, formatted);
        Assert.Equal(size, ByteSize.ParseWithUnitSystem(formatted, unitSystem, CultureInfo.InvariantCulture));
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
        Assert.Equal("-7 b", size.Format(unitSystem, formatProvider: CultureInfo.InvariantCulture));
        Assert.Equal(
            "0 b",
            ByteSize.FromBytes(-0.1).Format(unitSystem, formatProvider: CultureInfo.InvariantCulture));
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
            ByteSize.ParseWithUnitSystem($"{bits} b", ByteSizeUnitSystem.DecimalSi, CultureInfo.InvariantCulture).FormatFullWords(
                ByteSizeUnitSystem.DecimalSi,
                "0 b",
                CultureInfo.GetCultureInfo("sl")));

    [Fact]
    public void ExplicitBitFullWordsUseDisplayedStandardFormatForWideLocaleGrammar() =>
        Assert.Equal(
            "9E+015 bitov",
            ByteSize.ParseWithUnitSystem(
                    "9007199254741001 b",
                    ByteSizeUnitSystem.DecimalSi,
                    CultureInfo.InvariantCulture)
                .FormatFullWords(
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
            Assert.EndsWith(" KiB 1 B", ByteSize.FromBytes(2049).HumanizeCompositeWithUnitSystem(
                ByteSizeUnitSystem.BinaryIec,
                formatProvider: culture));
            Assert.Contains("kB/", ByteSize.FromDecimalKilobytes(2)
                .Per(TimeSpan.FromSeconds(1))
                .HumanizeWithUnitSystem(ByteSizeUnitSystem.DecimalSi, culture: culture));
            Assert.Contains("KiB/", ByteSize.FromKibibytes(2)
                .Per(TimeSpan.FromSeconds(1))
                .HumanizeWithUnitSystem(ByteSizeUnitSystem.BinaryIec, culture: culture));

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

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi, "MB", ByteSize.BytesInDecimalMegabyte)]
    [InlineData(ByteSizeUnitSystem.BinaryIec, "MiB", ByteSize.BytesInMebibyte)]
    public void ExplicitParsingUsesCurrentMutableNumberFormat(
        ByteSizeUnitSystem unitSystem,
        string symbol,
        double bytesPerUnit)
    {
        var numberFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();

        Assert.True(ByteSize.TryParseWithUnitSystem($"1.5 {symbol}", unitSystem, numberFormat, out _));

        numberFormat.NegativeSign = "~";
        numberFormat.NumberDecimalSeparator = ",";
        numberFormat.NumberGroupSeparator = "_";

        Assert.True(ByteSize.TryParseWithUnitSystem($"~1_500,5 {symbol}", unitSystem, numberFormat, out var parsed));
        Assert.Equal(ByteSize.FromBytes(-1500.5 * bytesPerUnit), parsed);
    }

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi)]
    [InlineData(ByteSizeUnitSystem.BinaryIec)]
    public void AutomaticFormattingPreservesNumericLookingLiterals(ByteSizeUnitSystem unitSystem)
    {
        foreach (var format in new[] { "0 '#.##'", @"0 \#\.\#\#" })
        {
            var size = ByteSize.FromBytes(1);

            Assert.Equal("1 #.## B", size.Format(unitSystem, format, CultureInfo.InvariantCulture));
            Assert.Equal("1 #.## byte", size.FormatFullWords(unitSystem, format, CultureInfo.InvariantCulture));
        }
    }

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi, ByteSize.BytesInDecimalKilobyte, "kB", "kilobyte")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, ByteSize.BytesInKibibyte, "KiB", "kibibyte")]
    public void ExplicitUnitFormattingPreservesNumericLookingLiterals(
        ByteSizeUnitSystem unitSystem,
        double bytes,
        string symbol,
        string unitWord)
    {
        foreach (var format in new[] { $"0 '#.##' {symbol}", $@"0 \#\.\#\# {symbol}" })
        {
            var size = ByteSize.FromBytes(bytes);

            Assert.Equal($"1 #.## {symbol}", size.Format(unitSystem, format, CultureInfo.InvariantCulture));
            Assert.Equal($"1 #.## {unitWord}", size.FormatFullWords(unitSystem, format, CultureInfo.InvariantCulture));
        }
    }
}

[UseCulture("fi")]
public class ByteSizeUnitSystemCurrentCultureTests
{
    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi, ByteSize.BytesInDecimalKilobyte, "kB")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, ByteSize.BytesInKibibyte, "KiB")]
    public void ExplicitDefaultProviderRoundTripsFormattingOverrides(
        ByteSizeUnitSystem unitSystem,
        double bytes,
        string symbol)
    {
        var size = ByteSize.FromBytes(-1.5 * bytes);
        var formatted = size.Format(unitSystem, $"0.0 {symbol}");

        Assert.StartsWith("−", formatted);
        Assert.Equal(size, ByteSize.ParseWithUnitSystem(formatted, unitSystem));
        Assert.True(ByteSize.TryParseWithUnitSystem(formatted, unitSystem, out var parsed));
        Assert.Equal(size, parsed);
        Assert.True(ByteSize.TryParseSpanWithUnitSystem(formatted.AsSpan(), unitSystem, out parsed));
        Assert.Equal(size, parsed);
    }
}