using System.Globalization;
using System.Security.Cryptography;
using System.Text;

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

const int scalarCount = 0x110000;
const uint allScripts = 0x03FFFFFF;

var check = args.Length == 2 && args[1] == "--check";
if (args.Length is < 1 or > 2 || args.Length == 2 && !check)
{
    Console.Error.WriteLine(
        "Usage: Humanizer.UnicodeDataGenerator <unicode-16-ucd-directory> [--check]");
    return 2;
}

var repositoryRoot = FindRepositoryRoot(Directory.GetCurrentDirectory());
var inputDirectory = Path.GetFullPath(args[0]);
var inputs = new Dictionary<string, string>(StringComparer.Ordinal)
{
    ["UnicodeData.txt"] = "ff58e5823bd095166564a006e47d111130813dcf8bf234ef79fa51a870edb48f", // DevSkim: ignore DS173237
    ["CaseFolding.txt"] = "6f1f9c588eb4a5c718d9e8f93b782685e5c7fec872cf05e8e6878053599e09bb", // DevSkim: ignore DS173237
    ["Scripts.txt"] = "9e88f0a677df47311106340be8ede2ecdacd9c1c931831218d2be6d5508e0039", // DevSkim: ignore DS173237
    ["ScriptExtensions.txt"] = "049117ce26b9769fe2749b06eef51a50a89faef4a97764dd2d81daa715980700", // DevSkim: ignore DS173237
    ["DerivedNormalizationProps.txt"] = "4d4c03892dea9146d674b686e495df2d55a28d071ac474041d73518f887abddc" // DevSkim: ignore DS173237
};

foreach (var input in inputs)
{
    var path = Path.Combine(inputDirectory, input.Key);
    if (!File.Exists(path))
    {
        throw new FileNotFoundException($"Missing Unicode 16 input '{input.Key}'.", path);
    }

    var actual = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();
    if (actual != input.Value)
    {
        throw new InvalidOperationException(
            $"Unicode 16 input '{input.Key}' has SHA-256 {actual}; expected {input.Value}.");
    }
}

var unicodeDataPath = Path.Combine(inputDirectory, "UnicodeData.txt");
var categories = new string?[scalarCount];
var uppercaseMappings = new Dictionary<int, int>();
var lowercaseMappings = new Dictionary<int, int>();
ReadUnicodeData(
    unicodeDataPath,
    categories,
    uppercaseMappings,
    lowercaseMappings);

var unicodeDataFile = Path.Combine(
    repositoryRoot,
    "src",
    "Humanizer",
    "Inflections",
    "InflectionUnicodeData.cs");

var unicodeSource = NormalizeLineEndings(File.ReadAllText(unicodeDataFile));
unicodeSource = ReplaceGeneratedBlock(
    unicodeSource,
    "generated-unicode-letter-mark",
    GenerateLetterMarkBlock(categories, inputs["UnicodeData.txt"]));
unicodeSource = ReplaceGeneratedBlock(
    unicodeSource,
    "generated-unicode-case-fold",
    GenerateMappingBlock(
        "CaseFolding.txt C+S",
        "SimpleCaseFold",
        ReadCaseFoldMappings(Path.Combine(inputDirectory, "CaseFolding.txt")),
        inputs["CaseFolding.txt"]));
unicodeSource = ReplaceGeneratedBlock(
    unicodeSource,
    "generated-unicode-lowercase",
    GenerateMappingBlock(
        "UnicodeData.txt simple lowercase",
        "SimpleLowercase",
        lowercaseMappings,
        inputs["UnicodeData.txt"]));
unicodeSource = ReplaceGeneratedBlock(
    unicodeSource,
    "generated-unicode-uppercase",
    GenerateMappingBlock(
        "UnicodeData.txt simple uppercase",
        "SimpleUppercase",
        uppercaseMappings,
        inputs["UnicodeData.txt"]));
unicodeSource = ReplaceGeneratedBlock(
    unicodeSource,
    "generated-unicode-case-categories",
    GenerateCaseCategoryBlock(categories, inputs["UnicodeData.txt"]));
unicodeSource = ReplaceGeneratedBlock(
    unicodeSource,
    "generated-unicode-scripts",
    GenerateScriptBlock(
        Path.Combine(inputDirectory, "Scripts.txt"),
        Path.Combine(inputDirectory, "ScriptExtensions.txt"),
        inputs["Scripts.txt"],
        inputs["ScriptExtensions.txt"]));
unicodeSource = ReplaceGeneratedBlock(
    unicodeSource,
    "generated-unicode-nfc-quick-check",
    GenerateNfcQuickCheckBlock(
        Path.Combine(inputDirectory, "DerivedNormalizationProps.txt"),
        inputs["DerivedNormalizationProps.txt"]));

var unicodeChanged = CheckOrWrite(unicodeDataFile, unicodeSource, check);
if (check)
{
    if (unicodeChanged)
    {
        throw new InvalidOperationException(
            "Generated Unicode 16 inflection data is stale. Run the generator without --check.");
    }

    Console.WriteLine("Unicode 16 inflection data is current and all five source hashes match.");
}
else
{
    Console.WriteLine("Regenerated Unicode 16 inflection data from all five hash-pinned inputs.");
}

return 0;

static string FindRepositoryRoot(string start)
{
    for (var current = new DirectoryInfo(start); current is not null; current = current.Parent)
    {
        if (File.Exists(Path.Combine(current.FullName, "Humanizer.slnx")))
        {
            return current.FullName;
        }
    }

    throw new InvalidOperationException(
        $"Could not find the Humanizer repository root from '{start}'.");
}

static string NormalizeLineEndings(string value) =>
    value.Replace("\r\n", "\n", StringComparison.Ordinal)
        .Replace('\r', '\n');

static bool CheckOrWrite(string path, string expected, bool check)
{
    var current = NormalizeLineEndings(File.ReadAllText(path));
    if (current == expected)
    {
        return false;
    }

    if (!check)
    {
        File.WriteAllText(
            path,
            expected,
            new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
    }

    return true;
}

static string ReplaceGeneratedBlock(string source, string name, string generated)
{
    var startMarker = $"    // <{name}>";
    var endMarker = $"    // </{name}>";
    var start = source.IndexOf(startMarker, StringComparison.Ordinal);
    var end = source.IndexOf(endMarker, StringComparison.Ordinal);
    if (start < 0 || end <= start)
    {
        throw new InvalidOperationException($"Missing or invalid generated block '{name}'.");
    }

    var contentStart = source.IndexOf('\n', start) + 1;
    return source[..contentStart] + generated.TrimEnd('\n') + "\n" + source[end..];
}

static void ReadUnicodeData(
    string path,
    string?[] categories,
    Dictionary<int, int> uppercaseMappings,
    Dictionary<int, int> lowercaseMappings)
{
    int? rangeStart = null;
    string? rangeCategory = null;
    foreach (var line in File.ReadLines(path))
    {
        var fields = line.Split(';');
        if (fields.Length != 15)
        {
            throw new InvalidOperationException($"Malformed UnicodeData row: {line}");
        }

        var scalar = ParseHex(fields[0]);
        var name = fields[1];
        var category = fields[2];
        if (name.EndsWith(", First>", StringComparison.Ordinal))
        {
            rangeStart = scalar;
            rangeCategory = category;
            continue;
        }

        if (name.EndsWith(", Last>", StringComparison.Ordinal))
        {
            if (rangeStart is null || rangeCategory != category)
            {
                throw new InvalidOperationException($"Unmatched UnicodeData range: {line}");
            }

            for (var value = rangeStart.Value; value <= scalar; value++)
            {
                categories[value] = category;
            }

            rangeStart = null;
            rangeCategory = null;
            continue;
        }

        categories[scalar] = category;
        if (fields[12].Length != 0)
        {
            uppercaseMappings.Add(scalar, ParseHex(fields[12]));
        }

        if (fields[13].Length != 0)
        {
            lowercaseMappings.Add(scalar, ParseHex(fields[13]));
        }
    }

    if (rangeStart is not null)
    {
        throw new InvalidOperationException("Unterminated UnicodeData range.");
    }
}

static Dictionary<int, int> ReadCaseFoldMappings(string path)
{
    var mappings = new Dictionary<int, int>();
    foreach (var rawLine in File.ReadLines(path))
    {
        var line = StripComment(rawLine);
        if (line.Length == 0)
        {
            continue;
        }

        var fields = line.Split(';', StringSplitOptions.TrimEntries);
        if (fields.Length < 3 || fields[1] is not ("C" or "S"))
        {
            continue;
        }

        var targets = fields[2].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (targets.Length != 1)
        {
            throw new InvalidOperationException($"Non-simple CaseFolding mapping: {rawLine}");
        }

        mappings.Add(ParseHex(fields[0]), ParseHex(targets[0]));
    }

    return mappings;
}

static string GenerateLetterMarkBlock(string?[] categories, string hash)
{
    var ranges = BuildRanges(
        categories.Select(static category =>
            category is not null && category.StartsWith('L')
                ? 1u
                : category is not null && category.StartsWith('M')
                    ? 2u
                    : 0u).ToArray());
    var bytes = new List<byte>();
    var previousFirst = 0;
    foreach (var range in ranges)
    {
        WriteVarUInt(bytes, range.First - previousFirst);
        WriteVarUInt(bytes, ((range.Last - range.First) * 2) + (range.Value == 2 ? 1 : 0));
        previousFirst = range.First;
    }

    var base64 = Convert.ToBase64String(bytes.ToArray());
    AssertGenerated(
        "letter/mark ranges",
        ranges.Count,
        998,
        base64,
        "dc113e9cd9168a99f4232c34ad07d1c7cf9cf704bccdaf719235a12fdd068e81"); // DevSkim: ignore DS173237
    var builder = new StringBuilder();
    builder.AppendLine($"    // Unicode 16.0.0 letter and mark categories, compressed into {ranges.Count:N0} ranges.");
    builder.AppendLine("    // UnicodeData.txt SHA-256:");
    builder.AppendLine($"    // {hash}.");
    builder.AppendLine($"    const int UnicodeLetterMarkRangeCount = {ranges.Count};");
    builder.AppendLine("    const string UnicodeLetterMarkData = \"\"\"");
    builder.AppendLine(WrapRawString(base64));
    builder.AppendLine("\"\"\";");
    builder.AppendLine();
    builder.Append("    static readonly int[] UnicodeLetterMarkRanges = DecodeUnicodeLetterMarkRanges();");
    return builder.ToString();
}

static string GenerateMappingBlock(
    string description,
    string identifier,
    Dictionary<int, int> mappings,
    string hash)
{
    var ranges = new List<MappingRange>();
    foreach (var mapping in mappings.OrderBy(static pair => pair.Key))
    {
        var delta = mapping.Value - mapping.Key;
        if (ranges.Count != 0 &&
            mapping.Key == ranges[^1].Last + 1 &&
            delta == ranges[^1].Delta)
        {
            ranges[^1] = ranges[^1] with { Last = mapping.Key };
        }
        else
        {
            ranges.Add(new(mapping.Key, mapping.Key, delta));
        }
    }

    var bytes = new List<byte>();
    var previousFirst = 0;
    foreach (var range in ranges)
    {
        WriteVarUInt(bytes, range.First - previousFirst);
        WriteVarUInt(bytes, range.Last - range.First);
        WriteVarUInt(bytes, range.Delta >= 0
            ? range.Delta * 2
            : (-range.Delta * 2) - 1);
        previousFirst = range.First;
    }

    var mappingCount = mappings.Count;
    var base64 = Convert.ToBase64String(bytes.ToArray());
    var expected = identifier switch
    {
        "SimpleCaseFold" => (697, 1_484, "72486c276e049596d85cabcc3403a594412070c77dbb7ae7bff4e881d3742013"), // DevSkim: ignore DS173237
        "SimpleLowercase" => (674, 1_460, "6745351649f4ae54d9e610efc7a31f54270e1a99b5b7382208ca01aeb3c01b3f"), // DevSkim: ignore DS173237
        "SimpleUppercase" => (690, 1_477, "c79db3da3d65ceaefddf07c370ccdc69f470769fb3933889be9d8841b836d085"), // DevSkim: ignore DS173237
        _ => throw new InvalidOperationException($"Unknown generated mapping '{identifier}'.")
    };
    AssertGenerated(
        identifier,
        ranges.Count,
        expected.Item1,
        base64,
        expected.Item3);
    if (mappingCount != expected.Item2)
    {
        throw new InvalidOperationException(
            $"Generated {identifier} contains {mappingCount:N0} mappings; expected {expected.Item2:N0}.");
    }

    var builder = new StringBuilder();
    if (identifier is "SimpleLowercase" or "SimpleUppercase")
    {
        builder.AppendLine($"    // Unicode 16.0.0 {description} mappings, compressed");
        builder.AppendLine($"    // into {ranges.Count:N0} source ranges from {mappingCount:N0} mappings. Source SHA-256:");
    }
    else
    {
        builder.AppendLine($"    // Unicode 16.0.0 {description} mappings, compressed into {ranges.Count:N0}");
        builder.AppendLine($"    // source ranges from {mappingCount:N0} mappings. Source SHA-256:");
    }
    builder.AppendLine($"    // {hash}.");
    builder.AppendLine($"    const int {identifier}RangeCount = {ranges.Count};");
    builder.AppendLine($"    const string {identifier}Data =");
    builder.Append(WrapConcatenatedString(base64));
    builder.AppendLine(";");
    builder.AppendLine();
    builder.AppendLine($"    static readonly int[] {identifier}Ranges = DecodeMappingRanges(");
    builder.AppendLine($"        {identifier}Data,");
    builder.Append($"        {identifier}RangeCount);");
    return builder.ToString();
}

static string GenerateCaseCategoryBlock(string?[] categories, string hash)
{
    var values = categories.Select(static category =>
        category switch
        {
            "Ll" => 1u,
            "Lu" => 2u,
            "Lt" => 3u,
            _ => 0u
        }).ToArray();
    var ranges = BuildRanges(values);
    var bytes = new List<byte>();
    var previousFirst = 0;
    foreach (var range in ranges)
    {
        WriteVarUInt(bytes, range.First - previousFirst);
        WriteVarUInt(bytes, range.Last - range.First);
        WriteVarUInt(bytes, (int)range.Value);
        previousFirst = range.First;
    }

    var base64 = Convert.ToBase64String(bytes.ToArray());
    AssertGenerated(
        "case-category ranges",
        ranges.Count,
        1_323,
        base64,
        "c24753dc47d8a13f90c8c0e7b87e0d1c17c81438248acfdfa58ae8b9f601cfb2"); // DevSkim: ignore DS173237
    var builder = new StringBuilder();
    builder.AppendLine("    // Unicode 16.0.0 lowercase, uppercase, and titlecase categories, compressed");
    builder.AppendLine($"    // into {ranges.Count:N0} ranges. UnicodeData.txt SHA-256:");
    builder.AppendLine($"    // {hash}.");
    builder.AppendLine($"    const int UnicodeCaseRangeCount = {ranges.Count};");
    builder.AppendLine("    const string UnicodeCaseData = \"\"\"");
    builder.AppendLine(WrapRawString(base64));
    builder.AppendLine("\"\"\";");
    builder.AppendLine();
    builder.Append("    static readonly int[] UnicodeCaseRanges = DecodeUnicodeCaseRanges();");
    return builder.ToString();
}

static string GenerateScriptBlock(
    string scriptsPath,
    string extensionsPath,
    string scriptsHash,
    string extensionsHash)
{
    var baseScripts = new string?[scalarCount];
    foreach (var row in ReadPropertyRows(scriptsPath))
    {
        for (var scalar = row.First; scalar <= row.Last; scalar++)
        {
            baseScripts[scalar] = row.Value;
        }
    }

    var extensionScripts = new Dictionary<int, string[]>();
    foreach (var row in ReadPropertyRows(extensionsPath))
    {
        var values = row.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        for (var scalar = row.First; scalar <= row.Last; scalar++)
        {
            extensionScripts.Add(scalar, values);
        }
    }

    var valuesByScalar = new uint[scalarCount];
    for (var scalar = 0; scalar < scalarCount; scalar++)
    {
        var baseName = baseScripts[scalar];
        if (extensionScripts.TryGetValue(scalar, out var extensions))
        {
            var extensionValue = extensions.Aggregate(
                0u,
                static (current, script) => current | GetScriptValue(script));
            valuesByScalar[scalar] = extensionValue;
            continue;
        }

        valuesByScalar[scalar] = baseName == "Inherited"
            ? allScripts
            : GetScriptValue(baseName);
    }

    if (valuesByScalar[0x1DFA] != 0)
    {
        throw new InvalidOperationException(
            "U+1DFA has only an unsupported Syriac ScriptExtensions value and must not fall back to All.");
    }

    var ranges = BuildRanges(valuesByScalar);
    var builder = new StringBuilder();
    builder.AppendLine("    // Checked generated table from Unicode 16.0.0 Scripts.txt");
    builder.AppendLine($"    // (SHA-256 {scriptsHash})");
    builder.AppendLine("    // and ScriptExtensions.txt");
    builder.AppendLine($"    // (SHA-256 {extensionsHash}).");
    builder.AppendLine("    // Common scalars without an exact supported Script_Extensions value are omitted;");
    builder.AppendLine("    // unlisted Inherited scalars inherit the surrounding declared script.");
    builder.AppendLine("    static readonly InflectionUnicodeScriptRange[] UnicodeScriptRanges =");
    builder.AppendLine("    [");
    var generatedRows = new StringBuilder();
    foreach (var range in ranges)
    {
        generatedRows.Append("        new(0x");
        generatedRows.Append(FormatScalar(range.First));
        generatedRows.Append(", 0x");
        generatedRows.Append(FormatScalar(range.Last));
        generatedRows.Append(", ");
        generatedRows.Append(FormatScripts(range.Value));
        generatedRows.AppendLine("),");
    }
    AssertGenerated(
        "script ranges",
        ranges.Count,
        534,
        RemoveWhitespace(generatedRows.ToString()),
        "096be4d227f52d120e203d57c3c77c39433b719f6b688c063455726cc0168857"); // DevSkim: ignore DS173237
    builder.Append(generatedRows);
    builder.Append("    ];");
    return builder.ToString();
}

static string GenerateNfcQuickCheckBlock(string path, string hash)
{
    var values = new uint[scalarCount];
    foreach (var row in ReadPropertyRows(path))
    {
        var parts = row.Value.Split(';', StringSplitOptions.TrimEntries);
        if (parts.Length == 2 && parts[0] == "NFC_QC" && parts[1] is "N" or "M")
        {
            for (var scalar = row.First; scalar <= row.Last; scalar++)
            {
                values[scalar] = 1;
            }
        }
    }

    var ranges = BuildRanges(values);
    var builder = new StringBuilder();
    builder.AppendLine("    // Unicode 16.0.0 NFC_QC=No/Maybe ranges from DerivedNormalizationProps.txt");
    builder.AppendLine($"    // (SHA-256 {hash}).");
    builder.AppendLine("    static readonly InflectionUnicodeRange[] UncertainNfcQuickCheckRanges =");
    builder.AppendLine("    [");
    var generatedRows = new StringBuilder();
    foreach (var range in ranges)
    {
        generatedRows.Append("        new(0x");
        generatedRows.Append(FormatScalar(range.First));
        generatedRows.Append(", 0x");
        generatedRows.Append(FormatScalar(range.Last));
        generatedRows.AppendLine("),");
    }
    AssertGenerated(
        "NFC quick-check ranges",
        ranges.Count,
        119,
        RemoveWhitespace(generatedRows.ToString()),
        "8678af01da9fcd0759eae1795136f8487a185b902fc2ac5f8a8c5d2033d7bccb"); // DevSkim: ignore DS173237
    builder.Append(generatedRows);
    builder.Append("    ];");
    return builder.ToString();
}

static IEnumerable<PropertyRow> ReadPropertyRows(string path)
{
    foreach (var rawLine in File.ReadLines(path))
    {
        var line = StripComment(rawLine);
        if (line.Length == 0)
        {
            continue;
        }

        var fields = line.Split(';', 2, StringSplitOptions.TrimEntries);
        if (fields.Length != 2)
        {
            throw new InvalidOperationException($"Malformed Unicode property row: {rawLine}");
        }

        var range = ParseRange(fields[0]);
        yield return new(range.First, range.Last, fields[1]);
    }
}

static string StripComment(string value)
{
    var comment = value.IndexOf('#');
    return (comment < 0 ? value : value[..comment]).Trim();
}

static (int First, int Last) ParseRange(string value)
{
    var parts = value.Split("..", StringSplitOptions.TrimEntries);
    return parts.Length switch
    {
        1 => (ParseHex(parts[0]), ParseHex(parts[0])),
        2 => (ParseHex(parts[0]), ParseHex(parts[1])),
        _ => throw new InvalidOperationException($"Invalid Unicode range '{value}'.")
    };
}

static int ParseHex(string value) =>
    int.Parse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);

static List<ValueRange> BuildRanges(uint[] values)
{
    var ranges = new List<ValueRange>();
    for (var first = 0; first < values.Length;)
    {
        var value = values[first];
        if (value == 0)
        {
            first++;
            continue;
        }

        var last = first;
        while (last + 1 < values.Length && values[last + 1] == value)
        {
            last++;
        }

        ranges.Add(new(first, last, value));
        first = last + 1;
    }

    return ranges;
}

static void WriteVarUInt(List<byte> bytes, int value)
{
    do
    {
        var current = (byte)(value & 0x7F);
        value >>= 7;
        if (value != 0)
        {
            current |= 0x80;
        }

        bytes.Add(current);
    }
    while (value != 0);
}

static string WrapRawString(string value) =>
    string.Join('\n', Chunk(value, 100));

static string WrapConcatenatedString(string value)
{
    var chunks = Chunk(value, 100).ToArray();
    var builder = new StringBuilder();
    for (var index = 0; index < chunks.Length; index++)
    {
        builder.Append("        \"");
        builder.Append(chunks[index]);
        builder.Append(index == chunks.Length - 1 ? '"' : "\" +");
        if (index != chunks.Length - 1)
        {
            builder.AppendLine();
        }
    }

    return builder.ToString();
}

static IEnumerable<string> Chunk(string value, int length)
{
    for (var index = 0; index < value.Length; index += length)
    {
        yield return value.Substring(index, Math.Min(length, value.Length - index));
    }
}

static string FormatScalar(int scalar) =>
    scalar.ToString(scalar <= 0xFFFF ? "X4" : "X5", CultureInfo.InvariantCulture);

static uint GetScriptValue(string? script) =>
    script switch
    {
        "Arab" or "Arabic" => 1u << 0,
        "Armn" or "Armenian" => 1u << 1,
        "Beng" or "Bengali" => 1u << 2,
        "Cyrl" or "Cyrillic" => 1u << 3,
        "Deva" or "Devanagari" => 1u << 4,
        "Geor" or "Georgian" => 1u << 5,
        "Grek" or "Greek" => 1u << 6,
        "Gujr" or "Gujarati" => 1u << 7,
        "Guru" or "Gurmukhi" => 1u << 8,
        "Hani" or "Han" => (1u << 9) | (1u << 11) | (1u << 14),
        "Hebr" or "Hebrew" => 1u << 10,
        "Hira" or "Hiragana" or "Kana" or "Katakana" => 1u << 11,
        "Khmr" or "Khmer" => 1u << 12,
        "Knda" or "Kannada" => 1u << 13,
        "Hang" or "Hangul" => 1u << 14,
        "Laoo" or "Lao" => 1u << 15,
        "Latn" or "Latin" => 1u << 16,
        "Mlym" or "Malayalam" => 1u << 17,
        "Mymr" or "Myanmar" => 1u << 18,
        "Orya" or "Oriya" => 1u << 19,
        "Taml" or "Tamil" => 1u << 20,
        "Telu" or "Telugu" => 1u << 21,
        "Thai" => 1u << 22,
        "Ethi" or "Ethiopic" => 1u << 23,
        "Mong" or "Mongolian" => 1u << 24,
        "Sinh" or "Sinhala" => 1u << 25,
        _ => 0
    };

static string FormatScripts(uint scripts)
{
    if (scripts == allScripts)
    {
        return "InflectionUnicodeScripts.All";
    }

    var names = new[]
    {
        "Arab", "Armn", "Beng", "Cyrl", "Deva", "Geor", "Grek", "Gujr", "Guru",
        "Hani", "Hebr", "Jpan", "Khmr", "Knda", "Kore", "Laoo", "Latn", "Mlym",
        "Mymr", "Orya", "Taml", "Telu", "Thai", "Ethi", "Mong", "Sinh"
    };
    var values = new List<string>();
    for (var index = 0; index < names.Length; index++)
    {
        if ((scripts & (1u << index)) != 0)
        {
            values.Add($"InflectionUnicodeScripts.{names[index]}");
        }
    }

    values.Sort(StringComparer.Ordinal);
    return string.Join(" | ", values);
}

static void AssertGenerated(
    string label,
    int actualCount,
    int expectedCount,
    string canonicalValue,
    string expectedHash)
{
    if (actualCount != expectedCount)
    {
        throw new InvalidOperationException(
            $"Generated {label} contains {actualCount:N0} ranges; expected {expectedCount:N0}.");
    }

    var actualHash = Convert.ToHexString(
        SHA256.HashData(Encoding.UTF8.GetBytes(canonicalValue)))
        .ToLowerInvariant();
    if (actualHash != expectedHash)
    {
        throw new InvalidOperationException(
            $"Generated {label} has semantic SHA-256 {actualHash}; expected {expectedHash}.");
    }
}

static string RemoveWhitespace(string value) =>
    new(value.Where(static character => !char.IsWhiteSpace(character)).ToArray());

readonly record struct ValueRange(int First, int Last, uint Value);
readonly record struct MappingRange(int First, int Last, int Delta);
readonly record struct PropertyRow(int First, int Last, string Value);