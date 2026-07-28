using System.Collections.Frozen;

namespace Humanizer.Tests.Localisation;

public class ConjoinedGenderedScaleNumberToWordsConverterTests
{
    [Fact]
    public void UsesProfileDataForGenderedCardinalUnits()
    {
        var converter = CreateConverter();

        Assert.Equal("one-m", converter.Convert(1, GrammaticalGender.Masculine));
        Assert.Equal("one-f", converter.Convert(1, GrammaticalGender.Feminine));
        Assert.Equal("one-n", converter.Convert(1, GrammaticalGender.Neuter));
        Assert.Equal("two-m", converter.Convert(2, GrammaticalGender.Masculine));
        Assert.Equal("two", converter.Convert(2, GrammaticalGender.Feminine));
        Assert.Equal("two", converter.Convert(2, GrammaticalGender.Neuter));
    }

    [Theory]
    [InlineData(0, GrammaticalGender.Feminine, "zero-f")]
    [InlineData(1, GrammaticalGender.Masculine, "first-u-m")]
    [InlineData(20, GrammaticalGender.Neuter, "twenty-u-n")]
    [InlineData(100, GrammaticalGender.Feminine, "hundred-o-f")]
    [InlineData(1000, GrammaticalGender.Masculine, "one-f thousand-o-m")]
    public void UsesProfileDataForGenderedOrdinalForms(int number, GrammaticalGender gender, string expected)
    {
        var converter = CreateConverter();

        Assert.Equal(expected, converter.ConvertToOrdinal(number, gender));
    }

    static ConjoinedGenderedScaleNumberToWordsConverter CreateConverter() =>
        new(new(
            "minus",
            "and",
            CreateMap(20, index => index switch
            {
                0 => "zero",
                1 => "one",
                2 => "two",
                _ => index.ToString(CultureInfo.InvariantCulture)
            }),
            CreateMap(10, index => index switch
            {
                2 => "twenty",
                _ => index.ToString(CultureInfo.InvariantCulture)
            }),
            CreateMap(10, index => index switch
            {
                1 => "one hundred",
                _ => index.ToString(CultureInfo.InvariantCulture)
            }),
            CreateMap(10, index => index switch
            {
                1 => "hundred",
                _ => index.ToString(CultureInfo.InvariantCulture)
            }),
            CreateMap(20, index => index switch
            {
                1 => "first",
                _ => index.ToString(CultureInfo.InvariantCulture)
            }),
            new(
                new Dictionary<int, string> { [1] = "one-m", [2] = "two-m" }.ToFrozenDictionary(),
                new Dictionary<int, string> { [1] = "one-f" }.ToFrozenDictionary(),
                new Dictionary<int, string> { [1] = "one-n" }.ToFrozenDictionary()),
            new("zero-m", "zero-f", "zero-n"),
            new("-o-m", "-o-f", "-o-n"),
            new("-u-m", "-u-f", "-u-n"),
            [new(1000, GrammaticalGender.Feminine, "thousand", "thousands", "thousand")]
        ));

    static string[] CreateMap(int length, Func<int, string> valueFactory)
    {
        var values = new string[length];
        for (var index = 0; index < values.Length; index++)
        {
            values[index] = valueFactory(index);
        }

        return values;
    }
}