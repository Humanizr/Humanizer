namespace Humanizer.Tests.Localisation.enIN;

public class IndianScaleStyleTests
{
    static readonly CultureInfo EnglishIndia = new("en-IN");

    public static TheoryData<long, string, string> GoldenOutputs { get; } = new()
    {
        { 0, "", "" },
        { 10_000_000, "one crore", "one crore" },
        { 999_999_999, "ninety nine crore ninety nine lakh ninety nine thousand nine hundred and ninety nine", "ninety nine crore ninety nine lakh ninety nine thousand nine hundred and ninety nine" },
        { 1_000_000_000, "one arab", "one hundred crore" },
        { 999_999_999_999, "nine kharab ninety nine arab ninety nine crore ninety nine lakh ninety nine thousand nine hundred and ninety nine", "ninety nine thousand nine hundred and ninety nine crore ninety nine lakh ninety nine thousand nine hundred and ninety nine" },
        { 1_000_000_000_000, "ten kharab", "one lakh crore" },
        { 1_000_000_000_001, "ten kharab one", "one lakh crore one" },
        { 100_000_000_000_000, "ten neel", "one hundred lakh crore" },
        { 100_000_000_000_001, "ten neel one", "one hundred lakh crore one" },
        { 100_000_000_000_000_000, "one shankh", "one shankh" },
        { -1_000_000_000, "(Negative) one arab", "(Negative) one hundred crore" },
        { long.MaxValue, "ninety two shankh twenty three padma thirty seven neel twenty kharab thirty six arab eighty five crore forty seven lakh seventy five thousand eight hundred and seven", "ninety two shankh twenty three padma three hundred and seventy two lakh crore three thousand six hundred and eighty five crore forty seven lakh seventy five thousand eight hundred and seven" },
        { long.MinValue, "(Negative) ninety two shankh twenty three padma thirty seven neel twenty kharab thirty six arab eighty five crore forty seven lakh seventy five thousand eight hundred and eight", "(Negative) ninety two shankh twenty three padma three hundred and seventy two lakh crore three thousand six hundred and eighty five crore forty seven lakh seventy five thousand eight hundred and eight" },
    };

    [Theory]
    [MemberData(nameof(GoldenOutputs))]
    public void ToIndianWords_UsesSelectedScaleStyle(long number, string namedScales, string croreBased)
    {
        Assert.Equal(namedScales, number.ToIndianWords(IndianScaleStyle.NamedScales));
        Assert.Equal(croreBased, number.ToIndianWords(IndianScaleStyle.CroreBased));
    }

    [Theory]
    [MemberData(nameof(GoldenOutputs))]
    public void NamedScales_PreservesLegacyEnglishIndiaOutput(long number, string namedScales, string _)
    {
        if (number != long.MinValue)
        {
            Assert.Equal(number.ToWords(EnglishIndia), namedScales);
        }
    }

    [Fact]
    public void IntOverload_UsesSelectedScaleStyle() =>
        Assert.Equal("one hundred crore", 1_000_000_000.ToIndianWords(IndianScaleStyle.CroreBased));

    [Fact]
    public void UndefinedScaleStyle_Throws() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => 1.ToIndianWords((IndianScaleStyle)int.MaxValue));
}