namespace Humanizer;

/// <summary>
/// Generated CLDR cardinal-rule assignments for every supported culture.
/// </summary>
static partial class LocalizedInflectionCatalog
{
    public static bool TrySelectCategory(
        CultureInfo culture,
        decimal quantity,
        out CardinalPluralCategory category)
    {
        if (TryResolve(culture, out var rule))
        {
            category = CardinalPluralRules.Select(rule, quantity);
            return true;
        }

        category = CardinalPluralCategory.Other;
        return false;
    }

    static bool TryResolve(CultureInfo culture, out CardinalPluralRuleKind rule)
    {
        var language = culture.TwoLetterISOLanguageName;
        for (var current = culture;
             !string.IsNullOrEmpty(current.Name) &&
             string.Equals(current.TwoLetterISOLanguageName, language, StringComparison.OrdinalIgnoreCase);
             current = current.Parent)
        {
            if (TryResolveCore(current.Name, out rule))
            {
                return true;
            }
        }

        rule = default;
        return false;
    }

    private static partial bool TryResolveCore(
        string localeCode,
        out CardinalPluralRuleKind rule);
}