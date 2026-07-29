using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

[UseCulture("en")]
public class ByteSizeUnitSystemLocaleTests
{
    static readonly (ByteSizeUnitSystem System, string Symbol, DataUnit DataUnit)[] Units =
    [
        (ByteSizeUnitSystem.DecimalSi, "kB", DataUnit.DecimalKilobyte),
        (ByteSizeUnitSystem.DecimalSi, "MB", DataUnit.DecimalMegabyte),
        (ByteSizeUnitSystem.DecimalSi, "GB", DataUnit.DecimalGigabyte),
        (ByteSizeUnitSystem.DecimalSi, "TB", DataUnit.DecimalTerabyte),
        (ByteSizeUnitSystem.DecimalSi, "PB", DataUnit.DecimalPetabyte),
        (ByteSizeUnitSystem.DecimalSi, "EB", DataUnit.DecimalExabyte),
        (ByteSizeUnitSystem.BinaryIec, "KiB", DataUnit.BinaryKibibyte),
        (ByteSizeUnitSystem.BinaryIec, "MiB", DataUnit.BinaryMebibyte),
        (ByteSizeUnitSystem.BinaryIec, "GiB", DataUnit.BinaryGibibyte),
        (ByteSizeUnitSystem.BinaryIec, "TiB", DataUnit.BinaryTebibyte),
        (ByteSizeUnitSystem.BinaryIec, "PiB", DataUnit.BinaryPebibyte)
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
            .Order(StringComparer.Ordinal)
            .ToArray();
        var shippedLocales = Humanizer.Tests.Localisation.LocaleCoverageData.ShippedLocales
            .Order(StringComparer.Ordinal)
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
            var dataUnit = Assert.Single(Units, unit => unit.System == unitSystem && unit.Symbol == symbol).DataUnit;
            var unitWord = Configurator.Formatters.ResolveForCulture(culture).DataUnitHumanize(dataUnit, count, toSymbol: false);
            Assert.Equal(expected, $"{count} {unitWord}");
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