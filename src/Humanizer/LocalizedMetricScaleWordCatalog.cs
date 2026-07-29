namespace Humanizer;

static partial class LocalizedMetricScaleWordCatalog
{
    public static bool TryResolve(CultureInfo culture, char symbol, bool singular, out string value)
    {
        var language = culture.TwoLetterISOLanguageName;
        for (var current = culture;
             !string.IsNullOrEmpty(current.Name) &&
             string.Equals(current.TwoLetterISOLanguageName, language, StringComparison.OrdinalIgnoreCase);
             current = current.Parent)
        {
            if (TryResolveCore(current.Name, symbol, singular, out value))
            {
                return true;
            }
        }

        value = string.Empty;
        return false;
    }

    private static partial bool TryResolveCore(string localeCode, char symbol, bool singular, out string value);
}