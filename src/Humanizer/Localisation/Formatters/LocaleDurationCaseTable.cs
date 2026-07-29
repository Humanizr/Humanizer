namespace Humanizer;

enum LocaleDurationCaseClassification
{
    Distinct,
    Invariant,
    NotApplicable,
    Unsupported
}

enum LocalizedDurationCaseUnitKind
{
    Phrase,
    SameAsNominative,
    NotApplicable,
    Unsupported
}

readonly record struct LocalizedDurationCaseUnit(
    LocalizedDurationCaseUnitKind Kind,
    LocalizedTimeSpanPhrase? Phrase = null);

readonly record struct LocalizedDurationCase(LocalizedDurationCaseUnit[] Units);

sealed class LocaleDurationCaseTable(
    LocaleDurationCaseClassification classification,
    GrammaticalCase citationCase,
    FormatterNumberDetectorKind numberDetector,
    LocalizedDurationCase?[] cases)
{
    readonly LocalizedDurationCase?[] cases = cases;

    public LocaleDurationCaseClassification Classification { get; } = classification;
    public GrammaticalCase CitationCase { get; } = citationCase;
    public FormatterNumberDetectorKind NumberDetector { get; } = numberDetector;

    public bool TryGetCase(GrammaticalCase grammaticalCase, out LocalizedDurationCase value)
    {
        if ((uint)grammaticalCase >= (uint)cases.Length)
        {
            value = default;
            return false;
        }

        var candidate = cases[(int)grammaticalCase];
        if (candidate is { } found)
        {
            value = found;
            return true;
        }

        value = default;
        return false;
    }
}

static partial class LocaleDurationCaseTableCatalog
{
    public static LocaleDurationCaseTable? Resolve(CultureInfo culture)
    {
        for (var current = culture; current != CultureInfo.InvariantCulture; current = current.Parent)
        {
            if (ResolveCore(current.Name) is { } table)
            {
                return table;
            }

            if (string.IsNullOrEmpty(current.Name))
            {
                break;
            }
        }

        return null;
    }

    internal static partial LocaleDurationCaseTable? ResolveCore(string localeCode);
}