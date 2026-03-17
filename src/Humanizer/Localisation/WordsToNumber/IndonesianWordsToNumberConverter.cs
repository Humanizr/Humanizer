namespace Humanizer;

internal class IndonesianWordsToNumberConverter : MalayWordsToNumberConverterBase
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["nol"] = 0,
        ["satu"] = 1,
        ["dua"] = 2,
        ["tiga"] = 3,
        ["empat"] = 4,
        ["lima"] = 5,
        ["enam"] = 6,
        ["tujuh"] = 7,
        ["delapan"] = 8,
        ["sembilan"] = 9,
        ["sepuluh"] = 10,
        ["sebelas"] = 11,
        ["belas"] = 10,
        ["puluh"] = 10,
        ["seratus"] = 100,
        ["ratus"] = 100,
        ["seribu"] = 1000,
        ["ribu"] = 1000,
        ["juta"] = 1_000_000,
        ["miliar"] = 1_000_000_000
    }.ToFrozenDictionary(StringComparer.Ordinal);

    protected override string MinusWord => "minus";
    protected override string ZeroWord => "nol";
    protected override FrozenDictionary<string, int> Cardinals => CardinalMap;
}
