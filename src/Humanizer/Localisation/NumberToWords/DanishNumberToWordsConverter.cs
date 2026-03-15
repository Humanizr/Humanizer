namespace Humanizer;

class DanishNumberToWordsConverter : GenderlessNumberToWordsConverter
{
    static readonly string[] UnitsMap = ["nul", "en", "to", "tre", "fire", "fem", "seks", "syv", "otte", "ni", "ti", "elleve", "tolv", "tretten", "fjorten", "femten", "seksten", "sytten", "atten", "nitten"];
    static readonly string[] TensMap = ["", "", "tyve", "tredive", "fyrre", "halvtreds", "tres", "halvfjerds", "firs", "halvfems"];
    static readonly Dictionary<int, string> OrdinalMap = new()
    {
        [0] = "nulte",
        [1] = "første",
        [2] = "anden",
        [3] = "tredje",
        [4] = "fjerde",
        [5] = "femte",
        [6] = "sjette",
        [7] = "syvende",
        [8] = "ottende",
        [9] = "niende",
        [10] = "tiende",
        [11] = "ellevte",
        [12] = "tolvte",
        [13] = "trettende",
        [14] = "fjortende",
        [15] = "femtende",
        [16] = "sekstende",
        [17] = "syttende",
        [18] = "attende",
        [19] = "nittende",
        [20] = "tyvende"
    };

    public override string Convert(long number)
    {
        if (number == 0)
        {
            return UnitsMap[0];
        }

        if (number < 0)
        {
            return $"minus {Convert(-number)}";
        }

        if (number >= 1_000_000_000)
        {
            var billions = number / 1_000_000_000;
            var words = billions == 1 ? "en milliard" : $"{Convert(billions)} milliarder";
            return AppendRemainder(words, number % 1_000_000_000);
        }

        if (number >= 1_000_000)
        {
            var millions = number / 1_000_000;
            var words = millions == 1 ? "en million" : $"{Convert(millions)} millioner";
            return AppendRemainder(words, number % 1_000_000);
        }

        if (number >= 1_000)
        {
            var thousands = number / 1_000;
            var words = thousands == 1 ? "et tusind" : $"{Convert(thousands)} tusind";
            return AppendRemainder(words, number % 1_000);
        }

        if (number >= 100)
        {
            var hundreds = number / 100;
            var words = hundreds == 1 ? "et hundrede" : $"{Convert(hundreds)} hundrede";
            return AppendRemainder(words, number % 100);
        }

        if (number >= 20)
        {
            var tens = TensMap[number / 10];
            var unit = number % 10;
            return unit == 0 ? tens : $"{UnitsMap[unit]}og{tens}";
        }

        return UnitsMap[number];
    }

    public override string ConvertToOrdinal(int number)
    {
        if (number < 0)
        {
            return $"minus {ConvertToOrdinal(-number)}";
        }

        if (OrdinalMap.TryGetValue(number, out var ordinal))
        {
            return ordinal;
        }

        if (number is > 20 and < 30)
        {
            return $"{UnitsMap[number % 10]}ogtyvende";
        }

        return $"{Convert(number)}ende";
    }

    string AppendRemainder(string prefix, long remainder) =>
        remainder == 0 ? prefix : $"{prefix}{(remainder < 100 ? " og " : " ")}{Convert(remainder)}";
}
