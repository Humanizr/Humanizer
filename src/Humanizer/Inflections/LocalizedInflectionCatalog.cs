namespace Humanizer;

/// <summary>
/// Generated CLDR cardinal-rule and atomic inflection assignments.
/// </summary>
static partial class LocalizedInflectionCatalog
{
    public static bool TrySelectCategory(
        CultureInfo culture,
        decimal quantity,
        out CardinalPluralCategory category)
    {
        if (TryResolveRule(culture, out var rule))
        {
            category = CardinalPluralRules.Select(rule, quantity);
            return true;
        }

        category = CardinalPluralCategory.Other;
        return false;
    }

    internal static InflectionResult Inflect(
        string input,
        CultureInfo culture,
        InflectionDirection direction,
        bool allowProductive,
        CardinalPluralCategory? category = null)
    {
        return TryResolveBundle(culture, out var bundle, out var status)
            ? bundle.Inflect(input, direction, allowProductive, category)
            : new(status, input);
    }

    internal static InflectionResult Inflect(
        string input,
        CultureInfo culture,
        InflectionDirection direction,
        bool allowProductive,
        CardinalPluralCategory category,
        CardinalPluralOperands operands)
    {
        return TryResolveBundle(culture, out var bundle, out var status)
            ? bundle.Inflect(input, direction, allowProductive, category, operands)
            : new(status, input);
    }

    internal static InflectionResult Inflect(
        string input,
        CultureInfo culture,
        InflectionDirection direction,
        bool allowProductive,
        CardinalPluralCategory category,
        double quantity)
    {
        return TryResolveBundle(culture, out var bundle, out var status)
            ? bundle.Inflect(input, direction, allowProductive, category, quantity)
            : new(status, input);
    }

    static bool TryResolveBundle(
        CultureInfo culture,
        [NotNullWhen(true)] out InflectionBundle? bundle,
        out InflectionStatus status)
    {
        if (!GeneratedCultureResolver.TryResolve(culture.Name, out var resolution))
        {
            bundle = null;
            status = InflectionStatus.Unsupported;
            return false;
        }

        if (resolution.InflectionTerminal is { } terminal)
        {
            bundle = null;
            status = terminal;
            return false;
        }

        bundle = null;
        if (resolution.InflectionOwner is not { } owner ||
            !TryResolveBundleCore(owner, out bundle) ||
            bundle is null)
        {
            status = InflectionStatus.Unknown;
            return false;
        }

        status = default;
        return true;
    }

    static bool TryResolveRule(CultureInfo culture, out CardinalPluralRuleKind rule)
    {
        if (GeneratedCultureResolver.TryResolve(culture.Name, out var resolution))
        {
            if (TryResolveRuleCore(culture.Name, out rule))
                return true;

            return TryResolveRuleCore(resolution.LocaleProfileOwner, out rule);
        }

        rule = default;
        return false;
    }

    private static partial bool TryResolveRuleCore(
        string localeCode,
        out CardinalPluralRuleKind rule);

    private static partial bool TryResolveBundleCore(
        string owner,
        [NotNullWhen(true)] out InflectionBundle? bundle);
}