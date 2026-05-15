using System.Collections.Frozen;

namespace Humanizer.Tests.Localisation;

public class LinkedVigesimalNumberToWordsConverterTests
{
    [Fact]
    public void UsesBaseTerminalRemainderJoinerWhenAlternateIsNotConfigured()
    {
        var converter = new LinkedVigesimalNumberToWordsConverter(CreateNumberProfile());

        Assert.Equal("twenty-r a apple", converter.Convert(21));
    }

    [Theory]
    [InlineData(21, "twenty-r ac apple")]
    [InlineData(22, "twenty-r a bee")]
    public void ChoosesAlternateTerminalRemainderJoinerByRenderedRemainderInitial(long number, string expected)
    {
        var converter = new LinkedVigesimalNumberToWordsConverter(CreateNumberProfile(
            terminalRemainderAlternateJoiner: "ac",
            terminalRemainderAlternateJoinerInitials: "ae"));

        Assert.Equal(expected, converter.Convert(number));
    }

    [Theory]
    [InlineData("twenty-r a apple", 21)]
    [InlineData("twenty-r ac apple", 21)]
    [InlineData("twenty-r a bee", 22)]
    [InlineData("twenty-r ac bee", 22)]
    public void WordsToNumberAcceptsBaseAndAlternateTerminalRemainderJoiners(string words, long expected)
    {
        var converter = new LinkedVigesimalWordsToNumberConverter(new(
            CreateWords(),
            CreateScales(),
            "a",
            terminalRemainderAlternateJoiner: "ac"));

        Assert.Equal(expected, converter.Convert(words));
    }

    [Fact]
    public void WordsToNumberMatchesLongerTerminalRemainderJoinerBeforePrefix()
    {
        var converter = new LinkedVigesimalWordsToNumberConverter(new(
            CreateWords(),
            CreateScales(),
            "a",
            terminalRemainderAlternateJoiner: "a plus"));

        Assert.Equal(21, converter.Convert("twenty-r a plus apple"));
    }

    static LinkedVigesimalNumberToWordsProfile CreateNumberProfile(
        string terminalRemainderAlternateJoiner = "",
        string terminalRemainderAlternateJoinerInitials = "") =>
        new(
            "zero",
            "minus",
            " ",
            " ",
            "a",
            100,
            "",
            CreateWords(),
            CreateScales(),
            terminalRemainderAlternateJoiner: terminalRemainderAlternateJoiner,
            terminalRemainderAlternateJoinerInitials: terminalRemainderAlternateJoinerInitials);

    static string[] CreateWords() =>
    [
        "zero",
        "apple",
        "bee",
        "three",
        "four",
        "five",
        "six",
        "seven",
        "eight",
        "nine",
        "ten",
        "eleven",
        "twelve",
        "thirteen",
        "fourteen",
        "fifteen",
        "sixteen",
        "seventeen",
        "eighteen",
        "nineteen"
    ];

    static LinkedVigesimalScale[] CreateScales() =>
    [
        new(
            20,
            "twenty",
            "twenty-r",
            "score",
            "score-r",
            " ",
            FrozenDictionary<int, string>.Empty,
            FrozenDictionary<int, string>.Empty)
    ];
}