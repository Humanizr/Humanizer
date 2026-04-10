using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Humanizer.Tests.Localisation;

public class LocaleTheoryMatrixCompletenessTests
{
    public static IEnumerable<object?[]> ShippedLocaleRows =>
        LocaleCoverageData.ShippedLocales.Select(static localeName => new object?[] { localeName });

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleFormatterExactTheoryData_DateDayPluralCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleFormatterExactTheoryData.DateDayPluralCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleFormatterExactTheoryData_MultiPartTimeSpanCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleFormatterExactTheoryData.MultiPartTimeSpanCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleFormatterExactTheoryData_TimeUnitSymbolCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleFormatterExactTheoryData.TimeUnitSymbolCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleFormatterExactTheoryData_ByteSizeSymbolCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleFormatterExactTheoryData.ByteSizeSymbolCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleFormatterExactTheoryData_ByteSizeFullWordCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleFormatterExactTheoryData.ByteSizeFullWordCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleFormatterExactTheoryData_CollectionHumanizeCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleFormatterExactTheoryData.CollectionHumanizeCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleFormatterExactTheoryData_HeadingCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleFormatterExactTheoryData.HeadingCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleFormatterExactTheoryData_HeadingAbbreviatedCardinalCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleFormatterExactTheoryData.HeadingAbbreviatedCardinalCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleNumberTheoryData_CardinalCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleNumberTheoryData.CardinalCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleNumberTheoryData_CardinalAddAndCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleNumberTheoryData.CardinalAddAndCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleNumberTheoryData_CardinalWordFormCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleNumberTheoryData.CardinalWordFormCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleNumberTheoryData_OrdinalCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleNumberTheoryData.OrdinalCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleNumberTheoryData_OrdinalWordFormCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleNumberTheoryData.OrdinalWordFormCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleNumberTheoryData_TupleCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleNumberTheoryData.TupleCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleNumberTheoryData_WordsToNumberCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleNumberTheoryData.WordsToNumberCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleNumberOverloadTheoryData_AddAndCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleNumberOverloadTheoryData.AddAndCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleNumberOverloadTheoryData_WordFormCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleNumberOverloadTheoryData.WordFormCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleNumberOverloadTheoryData_GenderCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleNumberOverloadTheoryData.GenderCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleNumberOverloadTheoryData_WordFormGenderCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleNumberOverloadTheoryData.WordFormGenderCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocalePhraseTheoryData_DateHumanizeCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocalePhraseTheoryData.DateHumanizeCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocalePhraseTheoryData_NullDateHumanizeCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocalePhraseTheoryData.NullDateHumanizeCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocalePhraseTheoryData_TimeSpanHumanizeCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocalePhraseTheoryData.TimeSpanHumanizeCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleNumberMagnitudeTheoryData_MagnitudeCardinalCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleNumberMagnitudeTheoryData.MagnitudeCardinalCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleNumberMagnitudeTheoryData_ExtendedMagnitudeCardinalCases_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleNumberMagnitudeTheoryData.ExtendedMagnitudeCardinalCases, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleCoverageData_FormatterExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleCoverageData.FormatterExpectationTheoryData, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleCoverageData_CollectionFormatterExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleCoverageData.CollectionFormatterExpectationTheoryData, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleCoverageData_NumberToWordsOrdinalExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleCoverageData.NumberToWordsOrdinalExpectationTheoryData, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleCoverageData_NumberToWordsCardinalExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleCoverageData.NumberToWordsCardinalExpectationTheoryData, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleCoverageData_OrdinalizerExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleCoverageData.OrdinalizerExpectationTheoryData, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleCoverageData_OrdinalizerDefaultExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleCoverageData.OrdinalizerDefaultExpectationTheoryData, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleCoverageData_OrdinalizerNegativeExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleCoverageData.OrdinalizerNegativeExpectationTheoryData, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleCoverageData_OrdinalizerWordFormExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleCoverageData.OrdinalizerWordFormExpectationTheoryData, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleCoverageData_DateToOrdinalWords2022January25ExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleCoverageData.DateToOrdinalWords2022January25ExpectationTheoryData, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleCoverageData_DateToOrdinalWords2015January1ExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleCoverageData.DateToOrdinalWords2015January1ExpectationTheoryData, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleCoverageData_DateToOrdinalWords2015February3ExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleCoverageData.DateToOrdinalWords2015February3ExpectationTheoryData, localeName);

#if NET6_0_OR_GREATER
    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleCoverageData_DateOnlyToOrdinalWords2022January25ExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleCoverageData.DateOnlyToOrdinalWords2022January25ExpectationTheoryData, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleCoverageData_DateOnlyToOrdinalWords2015January1ExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleCoverageData.DateOnlyToOrdinalWords2015January1ExpectationTheoryData, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleCoverageData_DateOnlyToOrdinalWords2015February3ExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleCoverageData.DateOnlyToOrdinalWords2015February3ExpectationTheoryData, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleCoverageData_TimeOnlyToClockNotation1323ExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleCoverageData.TimeOnlyToClockNotation1323ExpectationTheoryData, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleCoverageData_TimeOnlyToClockNotation1323RoundedExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleCoverageData.TimeOnlyToClockNotation1323RoundedExpectationTheoryData, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleCoverageData_TimeOnlyToClockNotation0105ExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleCoverageData.TimeOnlyToClockNotation0105ExpectationTheoryData, localeName);
#endif

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleCoverageData_WordsToNumberExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleCoverageData.WordsToNumberExpectationTheoryData, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleOrdinalizerMatrixData_OrdinalizerExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleOrdinalizerMatrixData.OrdinalizerExpectationTheoryData, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleOrdinalizerMatrixData_OrdinalizerDefaultExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleOrdinalizerMatrixData.OrdinalizerDefaultExpectationTheoryData, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleOrdinalizerMatrixData_OrdinalizerNegativeExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleOrdinalizerMatrixData.OrdinalizerNegativeExpectationTheoryData, localeName);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void LocaleOrdinalizerMatrixData_OrdinalizerWordFormExpectationTheoryData_IncludeLocale(string localeName) =>
        AssertLocaleCoverage(LocaleOrdinalizerMatrixData.OrdinalizerWordFormExpectationTheoryData, localeName);

    [Theory]
    [MemberData(nameof(AllLocaleGenderTheoryData))]
    public void LocaleNumberTheoryData_CardinalGenderCases_IncludeLocaleGender(string localeName, GrammaticalGender gender) =>
        AssertLocaleGenderCoverage(LocaleNumberTheoryData.CardinalGenderCases, localeName, 2, gender);

    [Theory]
    [MemberData(nameof(AllLocaleGenderTheoryData))]
    public void LocaleNumberTheoryData_CardinalWordFormGenderCases_IncludeLocaleGender(string localeName, GrammaticalGender gender) =>
        AssertLocaleGenderCoverage(LocaleNumberTheoryData.CardinalWordFormGenderCases, localeName, 3, gender);

    [Theory]
    [MemberData(nameof(AllLocaleGenderTheoryData))]
    public void LocaleNumberTheoryData_OrdinalGenderCases_IncludeLocaleGender(string localeName, GrammaticalGender gender) =>
        AssertLocaleGenderCoverage(LocaleNumberTheoryData.OrdinalGenderCases, localeName, 2, gender);

    [Theory]
    [MemberData(nameof(AllLocaleGenderTheoryData))]
    public void LocaleNumberTheoryData_OrdinalWordFormGenderCases_IncludeLocaleGender(string localeName, GrammaticalGender gender) =>
        AssertLocaleGenderCoverage(LocaleNumberTheoryData.OrdinalWordFormGenderCases, localeName, 2, gender);

    [Theory]
    [MemberData(nameof(AllLocaleGenderTheoryData))]
    public void LocaleCoverageData_OrdinalizerGenderExpectationTheoryData_IncludeLocaleGender(string localeName, GrammaticalGender gender) =>
        AssertLocaleGenderCoverage(LocaleCoverageData.OrdinalizerGenderExpectationTheoryData, localeName, 2, gender);

    [Theory]
    [MemberData(nameof(AllLocaleGenderTheoryData))]
    public void LocaleCoverageData_OrdinalizerWordFormGenderExpectationTheoryData_IncludeLocaleGender(string localeName, GrammaticalGender gender) =>
        AssertLocaleGenderCoverage(LocaleCoverageData.OrdinalizerWordFormGenderExpectationTheoryData, localeName, 2, gender);

    [Theory]
    [MemberData(nameof(AllLocaleGenderTheoryData))]
    public void LocaleCoverageData_OrdinalizerStringExactExpectationTheoryData_IncludeLocaleGender(string localeName, GrammaticalGender gender) =>
        AssertLocaleGenderCoverage(LocaleCoverageData.OrdinalizerStringExactExpectationTheoryData, localeName, 2, gender);

    [Theory]
    [MemberData(nameof(AllLocaleGenderTheoryData))]
    public void LocaleCoverageData_OrdinalizerNumberExactExpectationTheoryData_IncludeLocaleGender(string localeName, GrammaticalGender gender) =>
        AssertLocaleGenderCoverage(LocaleCoverageData.OrdinalizerNumberExactExpectationTheoryData, localeName, 2, gender);

    [Theory]
    [MemberData(nameof(AllLocaleGenderTheoryData))]
    public void LocaleOrdinalizerMatrixData_OrdinalizerGenderExpectationTheoryData_IncludeLocaleGender(string localeName, GrammaticalGender gender) =>
        AssertLocaleGenderCoverage(LocaleOrdinalizerMatrixData.OrdinalizerGenderExpectationTheoryData, localeName, 2, gender);

    [Theory]
    [MemberData(nameof(AllLocaleGenderTheoryData))]
    public void LocaleOrdinalizerMatrixData_OrdinalizerWordFormGenderExpectationTheoryData_IncludeLocaleGender(string localeName, GrammaticalGender gender) =>
        AssertLocaleGenderCoverage(LocaleOrdinalizerMatrixData.OrdinalizerWordFormGenderExpectationTheoryData, localeName, 2, gender);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void FormatterRegistryCoversYamlLocale(string localeName) =>
        Assert.Contains(localeName, GetRegisteredLocales<FormatterRegistry, IFormatter>(), StringComparer.OrdinalIgnoreCase);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void CollectionFormatterRegistryCoversYamlLocale(string localeName) =>
        Assert.Contains(localeName, GetRegisteredLocales<CollectionFormatterRegistry, ICollectionFormatter>(), StringComparer.OrdinalIgnoreCase);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void NumberToWordsConverterRegistryCoversYamlLocale(string localeName) =>
        Assert.Contains(localeName, GetRegisteredLocales<NumberToWordsConverterRegistry, INumberToWordsConverter>(), StringComparer.OrdinalIgnoreCase);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void OrdinalizerRegistryCoversYamlLocale(string localeName) =>
        Assert.Contains(localeName, GetRegisteredLocales<OrdinalizerRegistry, IOrdinalizer>(), StringComparer.OrdinalIgnoreCase);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void WordsToNumberConverterRegistryCoversYamlLocale(string localeName) =>
        Assert.Contains(localeName, GetRegisteredLocales<WordsToNumberConverterRegistry, IWordsToNumberConverter>(), StringComparer.OrdinalIgnoreCase);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void DateToOrdinalWordsConverterRegistryCoversYamlLocale(string localeName) =>
        Assert.Contains(localeName, GetRegisteredLocales<DateToOrdinalWordsConverterRegistry, IDateToOrdinalWordConverter>(), StringComparer.OrdinalIgnoreCase);

#if NET6_0_OR_GREATER
    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void DateOnlyToOrdinalWordsConverterRegistryCoversYamlLocale(string localeName) =>
        Assert.Contains(localeName, GetRegisteredLocales<DateOnlyToOrdinalWordsConverterRegistry, IDateOnlyToOrdinalWordConverter>(), StringComparer.OrdinalIgnoreCase);

    [Theory]
    [MemberData(nameof(ShippedLocaleRows))]
    public void TimeOnlyToClockNotationConvertersRegistryCoversYamlLocale(string localeName) =>
        Assert.Contains(localeName, GetRegisteredLocales<TimeOnlyToClockNotationConvertersRegistry, ITimeOnlyToClockNotationConverter>(), StringComparer.OrdinalIgnoreCase);
#endif

    public static TheoryData<string, GrammaticalGender> AllLocaleGenderTheoryData => CreateAllLocaleGenderTheoryData();

    static void AssertLocaleCoverage(IEnumerable dataSet, string localeName) =>
        Assert.Contains(localeName, EnumerateLocales(dataSet), StringComparer.OrdinalIgnoreCase);

    static void AssertLocaleGenderCoverage(IEnumerable dataSet, string localeName, int genderIndex, GrammaticalGender gender)
    {
        var keys = EnumerateLocaleGenderKeys(dataSet, genderIndex).ToArray();
        Assert.Contains($"{localeName}|{gender}", keys, StringComparer.OrdinalIgnoreCase);
    }

    static IEnumerable<string> EnumerateLocales(IEnumerable dataSet)
    {
        foreach (var row in EnumerateRows(dataSet))
        {
            yield return (string)row[0]!;
        }
    }

    static IEnumerable<string> EnumerateLocaleGenderKeys(IEnumerable dataSet, int genderIndex)
    {
        foreach (var row in EnumerateRows(dataSet))
        {
            yield return $"{(string)row[0]!}|{row[genderIndex]}";
        }
    }

    static bool ContainsLocale(IEnumerable dataSet, string localeName) =>
        EnumerateLocales(dataSet).Contains(localeName, StringComparer.OrdinalIgnoreCase);

    static IEnumerable<object?[]> EnumerateRows(IEnumerable dataSet)
    {
        foreach (var row in dataSet)
        {
            yield return ExtractRowData(row!);
        }
    }

    static TheoryData<string, GrammaticalGender> CreateAllLocaleGenderTheoryData()
    {
        var data = new TheoryData<string, GrammaticalGender>();
        foreach (var localeName in LocaleCoverageData.ShippedLocales)
        {
#if NET5_0_OR_GREATER
            foreach (var gender in Enum.GetValues<GrammaticalGender>())
#else
            foreach (var gender in (GrammaticalGender[])Enum.GetValues(typeof(GrammaticalGender)))
#endif
            {
                data.Add(localeName, (GrammaticalGender)gender);
            }
        }

        return data;
    }

    static string[] GetRegisteredLocales<TRegistry, TLocaliser>()
        where TRegistry : LocaliserRegistry<TLocaliser>, new()
        where TLocaliser : class
        => new TRegistry().GetRegisteredLocaleCodes();

    [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "xUnit theory rows expose GetData at runtime in the test assembly.")]
    static object?[] ExtractRowData(object row) =>
        row is object?[] values
            ? values
            : (object?[])row.GetType()
                .GetMethod("GetData", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, Type.DefaultBinder, Type.EmptyTypes, null)!
                .Invoke(row, null)!;
}