namespace Humanizer;

internal class GreekWordsToNumberConverter : GenderlessWordsToNumberConverter
{
    static readonly FrozenDictionary<string, int> CardinalMap = new Dictionary<string, int>(StringComparer.Ordinal)
    {
        ["ένα"] = 1,
        ["ενα"] = 1,
        ["ένας"] = 1,
        ["ενας"] = 1,
        ["μία"] = 1,
        ["μια"] = 1,
        ["δύο"] = 2,
        ["δυο"] = 2,
        ["τρία"] = 3,
        ["τρια"] = 3,
        ["τρείς"] = 3,
        ["τρεις"] = 3,
        ["τέσσερα"] = 4,
        ["τεσσερα"] = 4,
        ["πέντε"] = 5,
        ["πεντε"] = 5,
        ["έξι"] = 6,
        ["εξι"] = 6,
        ["επτά"] = 7,
        ["επτα"] = 7,
        ["οκτώ"] = 8,
        ["οκτω"] = 8,
        ["εννέα"] = 9,
        ["εννεα"] = 9,
        ["δέκα"] = 10,
        ["δεκα"] = 10,
        ["έντεκα"] = 11,
        ["εντεκα"] = 11,
        ["δώδεκα"] = 12,
        ["δωδεκα"] = 12,
        ["δεκατρείς"] = 13,
        ["δεκατρεις"] = 13,
        ["δεκατέσσερα"] = 14,
        ["δεκατεσσερα"] = 14,
        ["είκοσι"] = 20,
        ["εικοσι"] = 20,
        ["τριάντα"] = 30,
        ["τριαντα"] = 30,
        ["σαράντα"] = 40,
        ["σαραντα"] = 40,
        ["πενήντα"] = 50,
        ["πενηντα"] = 50,
        ["εξήντα"] = 60,
        ["εξηντα"] = 60,
        ["εβδομήντα"] = 70,
        ["εβδομηντα"] = 70,
        ["ογδόντα"] = 80,
        ["ογδοντα"] = 80,
        ["ενενήντα"] = 90,
        ["ενενηντα"] = 90,
        ["εκατό"] = 100,
        ["εκατο"] = 100,
        ["εκατόν"] = 100,
        ["εκατον"] = 100,
        ["διακόσια"] = 200,
        ["διακοσια"] = 200,
        ["τριακόσια"] = 300,
        ["τριακοσια"] = 300,
        ["τετρακόσια"] = 400,
        ["τετρακοσια"] = 400,
        ["πεντακόσια"] = 500,
        ["πεντακοσια"] = 500,
        ["εξακόσια"] = 600,
        ["εξακοσια"] = 600,
        ["επτακόσια"] = 700,
        ["επτακοσια"] = 700,
        ["οκτακόσια"] = 800,
        ["οκτακοσια"] = 800,
        ["εννιακόσια"] = 900,
        ["εννιακοσια"] = 900,
        ["χίλια"] = 1_000,
        ["χιλια"] = 1_000,
        ["χιλιάδες"] = 1_000,
        ["χιλιαδες"] = 1_000,
        ["εκατομμύριο"] = 1_000_000,
        ["εκατομμυριο"] = 1_000_000,
        ["εκατομμύρια"] = 1_000_000,
        ["εκατομμυρια"] = 1_000_000,
        ["δισεκατομμύριο"] = 1_000_000_000,
        ["δισεκατομμυριο"] = 1_000_000_000,
        ["δισεκατομμύρια"] = 1_000_000_000,
        ["δισεκατομμυρια"] = 1_000_000_000
    }.ToFrozenDictionary(StringComparer.Ordinal);

    public override int Convert(string words)
    {
        if (!TryConvert(words, out var parsedValue, out var unrecognizedWord))
        {
            throw new ArgumentException($"Unrecognized number word: {unrecognizedWord}");
        }

        return parsedValue;
    }

    public override bool TryConvert(string words, out int parsedValue) =>
        TryConvert(words, out parsedValue, out _);

    public override bool TryConvert(string words, out int parsedValue, out string? unrecognizedWord)
    {
        if (string.IsNullOrWhiteSpace(words))
        {
            throw new ArgumentException("Input words cannot be empty.");
        }

        var normalized = Normalize(words);
        if (TryParseCardinal(normalized, out parsedValue, out unrecognizedWord))
        {
            return true;
        }

        parsedValue = default;
        return false;
    }

    static string Normalize(string words) =>
        Regex.Replace(words.Replace(",", string.Empty)
                .Replace(".", string.Empty)
                .Replace("-", " ")
                .ToLowerInvariant()
                .Trim(),
            @"\s+",
            " ");

    static bool TryParseCardinal(string words, out int value, out string? unrecognizedWord)
    {
        var total = 0;
        var current = 0;
        unrecognizedWord = null;

        foreach (var token in words.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            if (!CardinalMap.TryGetValue(token, out var numeric))
            {
                value = default;
                unrecognizedWord = token;
                return false;
            }

            if (numeric >= 1000)
            {
                total += (current == 0 ? 1 : current) * numeric;
                current = 0;
                continue;
            }

            current += numeric;
        }

        value = total + current;
        return true;
    }
}
