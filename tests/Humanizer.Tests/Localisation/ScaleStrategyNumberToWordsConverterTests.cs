using System.Collections.Frozen;

namespace Humanizer.Tests.Localisation;

public class ScaleStrategyNumberToWordsConverterTests
{
    [Fact]
    public void NorwegianZeroUsesExplicitZeroWordInsteadOfUnitsMapSlotZero()
    {
        var converter = new ScaleStrategyNumberToWordsConverter(new(
            ScaleStrategyCardinalMode.NorwegianBokmal,
            ScaleStrategyOrdinalMode.NorwegianBokmal,
            100,
            GrammaticalGender.Masculine,
            "null",
            "minus",
            "en",
            "en",
            "ei",
            "et",
            "og",
            "og ",
            "te",
            "de",
            "e",
            "ende",
            13,
            "e",
            "ende",
            "de",
            "hundre",
            "ethundre",
            "tusen",
            "tusen",
            "ettusen",
            ["", "en", "to", "tre", "fire", "fem", "seks", "sju", "atte", "ni", "ti", "elleve", "tolv", "tretten", "fjorten", "femten", "seksten", "sytten", "atten", "nitten"],
            ["", "ti", "tjue", "tretti", "forti", "femti", "seksti", "sytti", "atti", "nitti"],
            [],
            [],
            FrozenDictionary<int, string>.Empty));

        Assert.Equal("null", converter.Convert(0, GrammaticalGender.Masculine));
        Assert.Equal("null", converter.Convert(0, GrammaticalGender.Feminine));
        Assert.Equal("null", converter.Convert(0, GrammaticalGender.Neuter));
    }
}
