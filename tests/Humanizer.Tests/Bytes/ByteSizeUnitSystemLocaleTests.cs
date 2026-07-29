using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

[UseCulture("en")]
public class ByteSizeUnitSystemLocaleTests
{
    static readonly (ByteSizeUnitSystem System, string Symbol, DataUnit DataUnit, long Bytes)[] Units =
    [
        (ByteSizeUnitSystem.DecimalSi, "kB", DataUnit.DecimalKilobyte, ByteSize.BytesInDecimalKilobyte),
        (ByteSizeUnitSystem.DecimalSi, "MB", DataUnit.DecimalMegabyte, ByteSize.BytesInDecimalMegabyte),
        (ByteSizeUnitSystem.DecimalSi, "GB", DataUnit.DecimalGigabyte, ByteSize.BytesInDecimalGigabyte),
        (ByteSizeUnitSystem.DecimalSi, "TB", DataUnit.DecimalTerabyte, ByteSize.BytesInDecimalTerabyte),
        (ByteSizeUnitSystem.DecimalSi, "PB", DataUnit.DecimalPetabyte, ByteSize.BytesInDecimalPetabyte),
        (ByteSizeUnitSystem.DecimalSi, "EB", DataUnit.DecimalExabyte, ByteSize.BytesInDecimalExabyte),
        (ByteSizeUnitSystem.BinaryIec, "KiB", DataUnit.BinaryKibibyte, ByteSize.BytesInKibibyte),
        (ByteSizeUnitSystem.BinaryIec, "MiB", DataUnit.BinaryMebibyte, ByteSize.BytesInMebibyte),
        (ByteSizeUnitSystem.BinaryIec, "GiB", DataUnit.BinaryGibibyte, ByteSize.BytesInGibibyte),
        (ByteSizeUnitSystem.BinaryIec, "TiB", DataUnit.BinaryTebibyte, ByteSize.BytesInTebibyte),
        (ByteSizeUnitSystem.BinaryIec, "PiB", DataUnit.BinaryPebibyte, ByteSize.BytesInPebibyte)
    ];

    [Fact]
    public void GoldenMatrixCoversEveryAuthoredRootUnitAndRegionalVariant()
    {
        var rootRows = EnumerateRows(ByteSizeUnitSystemLocaleTheoryData.RootWords).ToArray();
        var rootGroups = rootRows.GroupBy(
                row => (string)row[0]!,
                StringComparer.Ordinal)
            .ToArray();
        var keys = rootRows
            .Select(row => $"{row[0]}|{row[1]}|{row[2]}")
            .ToArray();
        var variantRows = EnumerateRows(ByteSizeUnitSystemLocaleTheoryData.VariantParents).ToArray();
        var variants = variantRows.Select(row => (string)row[0]!).ToArray();
        var coveredLocales = rootGroups
            .Select(group => group.Key)
            .Concat(variants)
            .OrderBy(locale => locale, StringComparer.Ordinal)
            .ToArray();
        var shippedLocales = Humanizer.Tests.Localisation.LocaleCoverageData.ShippedLocales
            .OrderBy(locale => locale, StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(90, rootGroups.Length);
        Assert.All(rootGroups, group => Assert.Equal(Units.Length, group.Count()));
        Assert.Equal(90 * Units.Length, rootRows.Length);
        Assert.Equal(keys.Length, keys.Distinct(StringComparer.Ordinal).Count());
        Assert.All(
            rootRows,
            row => Assert.All(
                row.Skip(3),
                value => Assert.False(string.IsNullOrEmpty((string)value!))));
        Assert.Equal(12, variants.Length);
        Assert.Equal(variants.Length, variants.Distinct(StringComparer.Ordinal).Count());
        Assert.Equal(102, coveredLocales.Length);
        Assert.Equal(shippedLocales, coveredLocales);
    }

    [Theory]
    [MemberData(nameof(ByteSizeUnitSystemLocaleTheoryData.RootWords), MemberType = typeof(ByteSizeUnitSystemLocaleTheoryData))]
    public void FormatsReviewedWordsForEveryAuthoredLocaleRoot(
        string locale,
        ByteSizeUnitSystem unitSystem,
        string symbol,
        string zero,
        string singular,
        string plural,
        string pluralFive)
    {
        var culture = CultureInfo.GetCultureInfo(locale);

        verify(0, zero);
        verify(1, singular);
        verify(2, plural);
        verify(5, pluralFive);

        void verify(int count, string expected)
        {
            var unit = Assert.Single(Units, candidate => candidate.System == unitSystem && candidate.Symbol == symbol);
            var size = ByteSize.FromBytes(count * unit.Bytes);

            Assert.Equal(expected, size.FormatFullWords(unitSystem, $"0 {symbol}", culture));
        }
    }

    [Theory]
    [InlineData("sl", ByteSizeUnitSystem.DecimalSi, "kB", 3, "3 kilobajti")]
    [InlineData("sl", ByteSizeUnitSystem.BinaryIec, "KiB", 4, "4 kibibajti")]
    [InlineData("ar", ByteSizeUnitSystem.DecimalSi, "MB", 11, "11 ميجابايت")]
    public void FormatsRepresentativePaucalAndManyUnitWords(
        string locale,
        ByteSizeUnitSystem unitSystem,
        string symbol,
        int count,
        string expected)
    {
        var size = ByteSize.ParseWithUnitSystem($"{count} {symbol}", unitSystem, CultureInfo.InvariantCulture);

        Assert.Equal(
            expected,
            size.FormatFullWords(unitSystem, $"0 {symbol}", CultureInfo.GetCultureInfo(locale)));
    }

    [Theory]
    [InlineData("lt", ByteSizeUnitSystem.DecimalSi, "kB", "−")]
    [InlineData("lt", ByteSizeUnitSystem.BinaryIec, "KiB", "−")]
    [InlineData("kk", ByteSizeUnitSystem.DecimalSi, "kB", " ")]
    [InlineData("kk", ByteSizeUnitSystem.BinaryIec, "KiB", " ")]
    public void ExplicitFormattingOverridesRoundTrip(
        string locale,
        ByteSizeUnitSystem unitSystem,
        string symbol,
        string expectedOverride)
    {
        var culture = CultureInfo.GetCultureInfo(locale);
        var bytesPerUnit = unitSystem == ByteSizeUnitSystem.DecimalSi
            ? ByteSize.BytesInDecimalKilobyte
            : ByteSize.BytesInKibibyte;
        var size = ByteSize.FromBytes(-1234 * bytesPerUnit);
        var formatted = size.Format(unitSystem, $"#,##0 {symbol}", culture);

        Assert.Contains(expectedOverride, formatted);
        Assert.Equal(size, ByteSize.ParseWithUnitSystem(formatted, unitSystem, culture));
    }

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi, "kB", "kilobyți")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, "KiB", "kibibyți")]
    public void RomanianExplicitFullWordsUseDeAtNumericBoundaries(
        ByteSizeUnitSystem unitSystem,
        string symbol,
        string unitWord)
    {
        var culture = CultureInfo.GetCultureInfo("ro");

        foreach (var (count, format, expected) in new[]
                 {
                     ("0", "0", $"0 de {unitWord}"),
                     ("19", "0", $"19 {unitWord}"),
                     ("20", "0", $"20 de {unitWord}"),
                     ("21", "0", $"21 de {unitWord}"),
                     ("101", "0", $"101 {unitWord}"),
                     ("120", "0", $"120 de {unitWord}"),
                     ("20.5", "0.0", $"20,5 de {unitWord}")
                 })
        {
            var size = ByteSize.ParseWithUnitSystem($"{count} {symbol}", unitSystem, CultureInfo.InvariantCulture);

            Assert.Equal(
                expected,
                size.FormatFullWords(unitSystem, $"{format} {symbol}", culture));
        }
    }

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi)]
    [InlineData(ByteSizeUnitSystem.BinaryIec)]
    public void RomanianExplicitSharedUnitWordsUseDeAtNumericBoundaries(ByteSizeUnitSystem unitSystem)
    {
        var culture = CultureInfo.GetCultureInfo("ro");
        Assert.Equal(
            "0 de bit",
            ByteSize.FromBits(0).HumanizeCompositeWithUnitSystem(
                unitSystem,
                precision: 1,
                formatProvider: culture,
                toWords: true));

        foreach (var (count, usesDe) in new[]
                 {
                     (0, true),
                     (19, false),
                     (20, true),
                     (21, true),
                     (101, false),
                     (120, true)
                 })
        {
            verify(ByteSize.FromBits(count), "b", "bit", count, usesDe);
            verify(ByteSize.FromBytes(count), "B", count == 1 ? "byte" : "bytes", count, usesDe);
        }

        void verify(ByteSize size, string symbol, string unitWord, int count, bool usesDe)
        {
            var expected = usesDe
                ? $"{count} de {unitWord}"
                : $"{count} {unitWord}";

            Assert.Equal(expected, size.FormatFullWords(unitSystem, $"0 {symbol}", culture));
        }
    }

    [Theory]
    [InlineData(ByteSizeUnitSystem.DecimalSi, ByteSize.BytesInDecimalKilobyte, "kilobyți")]
    [InlineData(ByteSizeUnitSystem.BinaryIec, ByteSize.BytesInKibibyte, "kibibyți")]
    public void RomanianExplicitWordCompositesUseDeAtNumericBoundaries(
        ByteSizeUnitSystem unitSystem,
        long bytesPerUnit,
        string unitWord)
    {
        var culture = CultureInfo.GetCultureInfo("ro");

        foreach (var (count, usesDe) in new[]
                 {
                     (19, false),
                     (20, true),
                     (21, true),
                     (101, false),
                     (120, true)
                 })
        {
            var size = ByteSize.FromBytes(count * bytesPerUnit);
            var expected = usesDe
                ? $"{count} de {unitWord}"
                : $"{count} {unitWord}";

            Assert.Equal(
                expected,
                size.HumanizeCompositeWithUnitSystem(
                    unitSystem,
                    precision: 1,
                    formatProvider: culture,
                    toWords: true));
        }

        var negativeSign = NumberFormatInfo.GetInstance(culture).NegativeSign;
        foreach (var (byteCount, usesDe) in new[]
                 {
                     (19, false),
                     (20, true),
                     (21, true),
                     (101, false),
                     (120, true)
                 })
        {
            var multipart = ByteSize.FromBytes(20 * bytesPerUnit + byteCount);
            var bytePhrase = usesDe ? $"{byteCount} de bytes" : $"{byteCount} bytes";
            var expected = $"20 de {unitWord} {bytePhrase}";

            Assert.Equal(
                expected,
                multipart.HumanizeCompositeWithUnitSystem(
                    unitSystem,
                    precision: 2,
                    formatProvider: culture,
                    toWords: true));
            Assert.Equal(
                negativeSign + expected,
                (-multipart).HumanizeCompositeWithUnitSystem(
                    unitSystem,
                    precision: 2,
                    formatProvider: culture,
                    toWords: true));
        }
    }

    [Theory]
    [MemberData(nameof(ByteSizeUnitSystemLocaleTheoryData.VariantParents), MemberType = typeof(ByteSizeUnitSystemLocaleTheoryData))]
    public void RegionalVariantsInheritTheirParentUnitWords(string variant, string parent)
    {
        var variantCulture = CultureInfo.GetCultureInfo(variant);
        var parentCulture = CultureInfo.GetCultureInfo(parent);

        foreach (var unit in Units)
        {
            foreach (var count in new[] { 0, 1, 2, 5 })
            {
                Assert.Equal(
                    Configurator.Formatters.ResolveForCulture(parentCulture).DataUnitHumanize(unit.DataUnit, count, toSymbol: false),
                    Configurator.Formatters.ResolveForCulture(variantCulture).DataUnitHumanize(unit.DataUnit, count, toSymbol: false));
            }
        }
    }

    static IEnumerable<object?[]> EnumerateRows(IEnumerable dataSet)
    {
        foreach (var row in dataSet)
        {
            yield return ExtractRowData(row!);
        }
    }

    [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "xUnit theory rows expose GetData at runtime in the test assembly.")]
    static object?[] ExtractRowData(object row) =>
        row is object?[] values
            ? values
            : (object?[])row.GetType()
                .GetMethod("GetData", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, Type.DefaultBinder, Type.EmptyTypes, null)!
                .Invoke(row, null)!;
}