namespace Humanizer;

/// <summary>
/// Immutable generated profile for one localized inflection culture.
/// </summary>
sealed class LocalizedInflectionProfile
{
    readonly FrozenDictionary<string, CardinalInflectionForms> lexemes;
    readonly FrozenDictionary<string, string?> reverseLexemes;

    public LocalizedInflectionProfile(
        CardinalPluralRuleKind cardinalRule,
        IReadOnlyDictionary<string, CardinalInflectionForms> lexemes)
    {
        CardinalRule = cardinalRule;
        this.lexemes = lexemes.ToFrozenDictionary(
            static entry => Normalize(entry.Key),
            static entry => entry.Value,
            StringComparer.Ordinal);
        reverseLexemes = BuildReverseLexemes(lexemes);
    }

    public CardinalPluralRuleKind CardinalRule { get; }

    public bool TryGetForms(string lemma, [NotNullWhen(true)] out CardinalInflectionForms? forms) =>
        lexemes.TryGetValue(Normalize(lemma), out forms);

    public bool TryGetLemma(string form, [NotNullWhen(true)] out string? lemma)
    {
        if (reverseLexemes.TryGetValue(Normalize(form), out lemma) && lemma is not null)
        {
            return true;
        }

        lemma = null;
        return false;
    }

    static FrozenDictionary<string, string?> BuildReverseLexemes(
        IReadOnlyDictionary<string, CardinalInflectionForms> lexemes)
    {
        var reverse = new Dictionary<string, string?>(StringComparer.Ordinal);
        foreach (var forms in lexemes.Values)
        {
            AddReverse(reverse, forms.Lemma, forms.Lemma);
            AddReverse(reverse, forms.Other, forms.Lemma);
            AddReverse(reverse, forms.Zero, forms.Lemma);
            AddReverse(reverse, forms.One, forms.Lemma);
            AddReverse(reverse, forms.Two, forms.Lemma);
            AddReverse(reverse, forms.Few, forms.Lemma);
            AddReverse(reverse, forms.Many, forms.Lemma);
        }

        return reverse.ToFrozenDictionary(StringComparer.Ordinal);
    }

    static void AddReverse(Dictionary<string, string?> reverse, string? form, string lemma)
    {
        if (form is null)
        {
            return;
        }

        var key = Normalize(form);
        if (reverse.TryGetValue(key, out var existing) &&
            !string.Equals(existing, lemma, StringComparison.Ordinal))
        {
            reverse[key] = null;
            return;
        }

        reverse[key] = lemma;
    }

    static string Normalize(string value) =>
        value.IsNormalized(NormalizationForm.FormC)
            ? value
            : value.Normalize(NormalizationForm.FormC);
}

/// <summary>
/// Generated exact localized noun lexicons and CLDR cardinal-rule assignments.
/// </summary>
static partial class LocalizedInflectionCatalog
{
    public static bool TrySelectCategory(
        CultureInfo culture,
        decimal quantity,
        out CardinalPluralCategory category)
    {
        if (TryResolve(culture, out var profile))
        {
            category = CardinalPluralRules.Select(profile.CardinalRule, quantity);
            return true;
        }

        category = CardinalPluralCategory.Other;
        return false;
    }

    public static bool TryInflect(
        CultureInfo culture,
        string lemma,
        decimal quantity,
        [NotNullWhen(true)] out string? result)
    {
        if (TryResolve(culture, out var profile) &&
            profile.TryGetForms(lemma, out var forms))
        {
            var category = CardinalPluralRules.Select(profile.CardinalRule, quantity);
            return forms.TryGetForm(category, out result);
        }

        result = null;
        return false;
    }

    public static bool TryLemmatize(
        CultureInfo culture,
        string form,
        [NotNullWhen(true)] out string? lemma)
    {
        if (TryResolve(culture, out var profile))
        {
            return profile.TryGetLemma(form, out lemma);
        }

        lemma = null;
        return false;
    }

    static bool TryResolve(CultureInfo culture, [NotNullWhen(true)] out LocalizedInflectionProfile? profile)
    {
        profile = null;
        for (var current = culture; !string.IsNullOrEmpty(current.Name); current = current.Parent)
        {
            if (TryResolveCore(current.Name, out var resolvedProfile))
            {
                profile = resolvedProfile;
                return true;
            }
        }

        return false;
    }

    private static partial bool TryResolveCore(
        string localeCode,
        [NotNullWhen(true)] out LocalizedInflectionProfile? profile);
}