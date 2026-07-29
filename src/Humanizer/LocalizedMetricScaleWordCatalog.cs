namespace Humanizer;

static partial class LocalizedMetricScaleWordCatalog
{
    public static bool TryResolve(CultureInfo culture, char symbol, double displayedNumber, out string value)
    {
        _ = LocalizedInflectionCatalog.TrySelectCategory(
            culture,
            (decimal)displayedNumber,
            out var category);
        var language = culture.TwoLetterISOLanguageName;
        for (var current = culture;
             !string.IsNullOrEmpty(current.Name) &&
             string.Equals(current.TwoLetterISOLanguageName, language, StringComparison.OrdinalIgnoreCase);
             current = current.Parent)
        {
            if (TryResolveCore(current.Name, symbol, category, out var resolved))
            {
                value = resolved;
                return true;
            }
        }

        value = string.Empty;
        return false;
    }

    private static partial bool TryResolveCore(string localeCode, char symbol, CardinalPluralCategory category, out string value);
}

static class MetricScaleWordForms
{
    public static string Select(
        CardinalPluralCategory category,
        string singular,
        string plural,
        string? two,
        string? few) =>
        category switch
        {
            CardinalPluralCategory.One => singular,
            CardinalPluralCategory.Two => two ?? few ?? plural,
            CardinalPluralCategory.Few => few ?? plural,
            _ => plural
        };
}