using System.Collections.Frozen;

namespace Humanizer.Tests.Localisation;

public class JoinedScaleNumberToWordsConverterTests
{
    [Theory]
    [InlineData(100, "hundred")]
    [InlineData(101, "hundred-r one")]
    [InlineData(1000, "thousand")]
    [InlineData(1001, "thousand-r one")]
    [InlineData(2001, "two thousand-r one")]
    public void SupportsExactAndRemainderScaleAndHundredForms(long number, string expected)
    {
        var converter = new JoinedScaleNumberToWordsConverter(CreateProfile(
            [new(1000, "thousand", "thousand-r", OmitOneWhenSingular: true)],
            ["", "hundred"],
            ["", "hundred-r"]));

        Assert.Equal(expected, converter.Convert(number));
    }

    [Theory]
    [InlineData(1_000_000, "one million")]
    [InlineData(2_000_000, "two millions")]
    [InlineData(1_000_001, "one million-r one")]
    [InlineData(2_000_001, "two millions-r one")]
    public void SupportsSingularPluralExactAndRemainderScaleForms(long number, string expected)
    {
        var converter = new JoinedScaleNumberToWordsConverter(CreateProfile(
            [new(1_000_000, "million", "million-r", "millions", "millions-r"), new(1000, "thousand")],
            ["", "hundred"],
            []));

        Assert.Equal(expected, converter.Convert(number));
    }

    [Fact]
    public void RequireOrdinalExceptionDisablesCompoundOrdinalFallback()
    {
        var converter = new JoinedScaleNumberToWordsConverter(CreateProfile(
            [],
            ["", "hundred"],
            [],
            requireOrdinalException: true,
            compoundOrdinalRemainder: 1,
            compoundOrdinalWord: "first"));

        Assert.Throws<NotImplementedException>(() => converter.ConvertToOrdinal(21));
    }

    [Fact]
    public void RequireOrdinalExceptionDisablesGenderedOrdinalFallbackButKeepsExactReplacements()
    {
        var converter = new JoinedScaleNumberToWordsConverter(CreateProfile(
            [],
            ["", "hundred"],
            [],
            requireOrdinalException: true,
            ordinal: new JoinedScaleOrdinalProfile(
                new("", "th", new Dictionary<int, string> { [1] = "first" }.ToFrozenDictionary()),
                null,
                null,
                GrammaticalGender.Masculine)));

        Assert.Equal("first", converter.ConvertToOrdinal(1));
        Assert.Throws<NotImplementedException>(() => converter.ConvertToOrdinal(2));
    }

    static JoinedScaleNumberToWordsProfile CreateProfile(
        JoinedScale[] scales,
        string[] hundredsMap,
        string[] hundredsMapWithRemainder,
        bool requireOrdinalException = false,
        JoinedScaleOrdinalProfile? ordinal = null,
        int? compoundOrdinalRemainder = null,
        string? compoundOrdinalWord = null) =>
        new(
            2_000_001,
            "zero",
            "minus",
            " ",
            " ",
            " ",
            "",
            false,
            "",
            null,
            null,
            [],
            [],
            hundredsMap,
            hundredsMapWithRemainder,
            CreateSubHundredMap(),
            FrozenDictionary<int, string>.Empty,
            FrozenDictionary<int, string>.Empty,
            scales,
            requireOrdinalException: requireOrdinalException,
            ordinal: ordinal,
            compoundOrdinalRemainder: compoundOrdinalRemainder,
            compoundOrdinalWord: compoundOrdinalWord);

    static string[] CreateSubHundredMap()
    {
        var values = new string[100];
        values[0] = "zero";
        values[1] = "one";
        values[2] = "two";
        for (var index = 3; index < values.Length; index++)
        {
            values[index] = index.ToString(CultureInfo.InvariantCulture);
        }

        return values;
    }
}