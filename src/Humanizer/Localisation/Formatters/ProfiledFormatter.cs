using System.Diagnostics;

namespace Humanizer;

/// <summary>
/// Describes how a profile detects grammatical-number forms.
/// </summary>
enum FormatterNumberDetectorKind
{
    /// <summary>
    /// Use the default resource key without adding a number-based suffix.
    /// </summary>
    None,

    /// <summary>
    /// Use singular for one item and plural for every other item.
    /// </summary>
    SingularPlural,

    /// <summary>
    /// Use singular, dual, plural, and default forms in the style of Arabic plural rules.
    /// </summary>
    ArabicLike,

    /// <summary>
    /// Use a paucal form for values between two and four.
    /// </summary>
    Between2And4Paucal,

    /// <summary>
    /// Use the South Slavic singular, paucal, and default rules.
    /// </summary>
    SouthSlavic,

    /// <summary>
    /// Use the Slovenian singular, dual, paucal, and default rules.
    /// </summary>
    Slovenian,

    /// <summary>
    /// Use the Russian grammatical-number detector.
    /// </summary>
    Russian,

    /// <summary>
    /// Use the Lithuanian grammatical-number detector.
    /// </summary>
    Lithuanian
}

/// <summary>
/// Represents the grammatical form selected by a profile detector.
/// </summary>
enum FormatterNumberForm
{
    /// <summary>
    /// Use the resource key without a grammatical suffix.
    /// </summary>
    Default,

    /// <summary>
    /// Use the singular form.
    /// </summary>
    Singular,

    /// <summary>
    /// Use the dual form.
    /// </summary>
    Dual,

    /// <summary>
    /// Use the paucal form.
    /// </summary>
    Paucal,

    /// <summary>
    /// Use the plural form.
    /// </summary>
    Plural
}

/// <summary>
/// Describes how a profile can transform data-unit fallbacks when no exact resource exists.
/// </summary>
enum FormatterDataUnitFallbackTransform
{
    /// <summary>
    /// Leave the fallback string unchanged.
    /// </summary>
    None,

    /// <summary>
    /// Remove a trailing <c>s</c> from the fallback string.
    /// </summary>
    TrimTrailingS,

    /// <summary>
    /// Apply the Latvian singular/plural fallback adjustment.
    /// </summary>
    Latvian
}

/// <summary>
/// Describes whether a profile inserts a locale-specific preposition placeholder.
/// </summary>
enum FormatterPrepositionMode
{
    /// <summary>
    /// Do not inject a preposition placeholder.
    /// </summary>
    None,

    /// <summary>
    /// Insert the Romanian <c>de</c> preposition when required.
    /// </summary>
    RomanianDe
}

/// <summary>
/// Describes whether a profile uses a locale-specific secondary placeholder.
/// </summary>
enum FormatterSecondaryPlaceholderMode
{
    /// <summary>
    /// Do not use a secondary placeholder.
    /// </summary>
    None,

    /// <summary>
    /// Use the Luxembourgish Eifeler-rule placeholder.
    /// </summary>
    LuxembourgishEifelerN
}

/// <summary>
/// DefaultFormatter variant that applies declarative locale profile data.
/// </summary>
// Shared formatter kernel used whenever a locale can be expressed as declarative resource-key,
// suffix, preposition, and gender-selection rules. The generator builds FormatterProfile
// instances from locale-owned YAML so runtime formatting stays branch-light and parse-free.
sealed class ProfiledFormatter(CultureInfo culture, FormatterProfile profile) : DefaultFormatter(culture)
{
    const char LuxembourgishEifelerSuffix = 'n';
    readonly FormatterProfile profile = profile;

    /// <summary>
    /// Formats a data unit using the profile's exact-key, suffix, and fallback rules.
    /// </summary>
    public override string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true)
    {
        if (toSymbol)
        {
            // Symbol forms are shared across profiles, so they bypass the grammar-specific lookup path entirely.
            return base.DataUnitHumanize(dataUnit, count, toSymbol);
        }

        // When a profile has no dedicated data-unit grammar, keep the base spelling and only apply the
        // locale-specific fallback transform. That preserves simple locales without forcing them through
        // the suffix machinery below.
        if (profile.DataUnitDetector == FormatterNumberDetectorKind.None && profile.DataUnitSuffixes.IsEmpty)
        {
            return ApplyFallbackTransform(base.DataUnitHumanize(dataUnit, count, toSymbol), count, profile.DataUnitFallbackTransform);
        }

        var resourceKey = DataUnitResourceKeys.GetResourceKey(dataUnit, false);
        var numberForm = DetectDataUnitForm(count, profile.DataUnitDetector, profile.DataUnitNonIntegralForm);
        var suffix = profile.DataUnitSuffixes.GetSuffix(numberForm);

        // Try the suffixed resource first because some locales only define special counted forms and leave the
        // unsuffixed resource as the generic fallback.
        if (suffix.Length != 0 &&
            Resources.TryGetResourceWithFallback(resourceKey + suffix, Culture, out var exactForm))
        {
            return exactForm;
        }

        // If the exact form is missing, the unsuffixed resource is the next best match before we fall back
        // to the base spellings and transforms.
        if (Resources.TryGetResourceWithFallback(resourceKey, Culture, out var resource))
        {
            return resource;
        }

        return ApplyFallbackTransform(base.DataUnitHumanize(dataUnit, count, toSymbol), count, profile.DataUnitFallbackTransform);
    }

    /// <summary>
    /// Converts numbers using a gender-aware rule when the profile provides one.
    /// </summary>
    protected override string NumberToWords(TimeUnit unit, int number, CultureInfo culture) =>
        profile.UnitGenders.TryGetValue(unit, out var gender)
            ? number.ToWords(gender, culture)
            : base.NumberToWords(unit, number, culture);

    /// <summary>
    /// Formats a unit that may need a locale-specific preposition or secondary placeholder.
    /// </summary>
    protected override string Format(TimeUnit unit, string resourceKey, int number, bool toWords = false)
    {
        var resolvedKey = GetResourceKey(resourceKey, number);
        var resourceString = Resources.GetResource(resolvedKey, Culture);
        // Compute the word form once because the same formatted value may feed different placeholder shapes.
        var numberAsWord = NumberToWords(unit, number, Culture);
        object value = toWords ? numberAsWord : number;

        // The generator emits exactly one supported placeholder mode per profile. If a locale ever combines
        // them unexpectedly, fail fast so the bad schema cannot silently produce the wrong grammar.
        return (profile.PrepositionMode, profile.SecondaryPlaceholderMode) switch
        {
            (FormatterPrepositionMode.None, FormatterSecondaryPlaceholderMode.None) =>
                string.Format(resourceString, value),
            (FormatterPrepositionMode.RomanianDe, FormatterSecondaryPlaceholderMode.None) =>
                string.Format(resourceString, value, ShouldUseRomanianPreposition(number) ? " de" : string.Empty),
            (FormatterPrepositionMode.None, FormatterSecondaryPlaceholderMode.LuxembourgishEifelerN) =>
                string.Format(resourceString, value, EifelerRule.DoesApply(numberAsWord.AsSpan()) ? string.Empty : LuxembourgishEifelerSuffix),
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    /// Applies the profile's key suffixes and exact overrides to the canonical resource key.
    /// </summary>
    protected override string GetResourceKey(string resourceKey, int number)
    {
        // Exact overrides win before generic suffix detection so exceptional keys do not inherit the wrong
        // plural rule just because they share a common resource prefix.
        foreach (var rule in profile.ResourceKeyOverrides)
        {
            if (rule.IsMatch(resourceKey, number))
            {
                return resourceKey + rule.Suffix;
            }
        }

        var suffix = profile.ResourceSuffixes.GetSuffix(DetectNumberForm(number, profile.ResourceKeyDetector));
        return suffix.Length == 0
            ? resourceKey
            : resourceKey + suffix;
    }

    /// <summary>
    /// Detects the grammatical form for data-unit counts.
    /// </summary>
    static FormatterNumberForm DetectDataUnitForm(double count, FormatterNumberDetectorKind detector, FormatterNumberForm nonIntegralForm)
    {
        var absoluteCount = Math.Abs(count);
        // Fractional counts often need a dedicated form, so we check the non-integral branch before folding
        // into the integer detector logic.
        if (absoluteCount % 1 != 0)
        {
            return nonIntegralForm;
        }

        return DetectNumberForm((int)absoluteCount, detector);
    }

    /// <summary>
    /// Detects the grammatical form for integer counts.
    /// </summary>
    static FormatterNumberForm DetectNumberForm(int number, FormatterNumberDetectorKind detector)
    {
        var absoluteNumber = Math.Abs(number);

        return detector switch
        {
            FormatterNumberDetectorKind.None => FormatterNumberForm.Default,
            FormatterNumberDetectorKind.SingularPlural => absoluteNumber == 1 ? FormatterNumberForm.Singular : FormatterNumberForm.Plural,
            FormatterNumberDetectorKind.ArabicLike => absoluteNumber switch
            {
                1 => FormatterNumberForm.Singular,
                2 => FormatterNumberForm.Dual,
                >= 3 and <= 10 => FormatterNumberForm.Plural,
                _ => FormatterNumberForm.Default
            },
            FormatterNumberDetectorKind.Between2And4Paucal => absoluteNumber switch
            {
                1 => FormatterNumberForm.Singular,
                > 1 and < 5 => FormatterNumberForm.Paucal,
                _ => FormatterNumberForm.Default
            },
            FormatterNumberDetectorKind.SouthSlavic => DetectSouthSlavicForm(absoluteNumber),
            FormatterNumberDetectorKind.Slovenian => absoluteNumber switch
            {
                1 => FormatterNumberForm.Singular,
                2 => FormatterNumberForm.Dual,
                3 or 4 => FormatterNumberForm.Paucal,
                _ => FormatterNumberForm.Default
            },
            FormatterNumberDetectorKind.Russian => RussianGrammaticalNumberDetector.Detect(absoluteNumber) switch
            {
                RussianGrammaticalNumber.Singular => FormatterNumberForm.Singular,
                RussianGrammaticalNumber.Paucal => FormatterNumberForm.Paucal,
                _ => FormatterNumberForm.Default
            },
            FormatterNumberDetectorKind.Lithuanian => LithuanianNumberFormDetector.Detect(absoluteNumber) switch
            {
                LithuanianNumberForm.Singular => FormatterNumberForm.Singular,
                LithuanianNumberForm.GenitivePlural => FormatterNumberForm.Plural,
                _ => FormatterNumberForm.Default
            },
            _ => throw new UnreachableException()
        };
    }

    /// <summary>
    /// Detects the South Slavic singular and paucal forms.
    /// </summary>
    static FormatterNumberForm DetectSouthSlavicForm(int absoluteNumber)
    {
        var mod10 = absoluteNumber % 10;
        var mod100 = absoluteNumber % 100;

        // Values ending in 11-14 use the default form even when the last digit looks singular or paucal.
        if (mod10 == 1 && mod100 != 11)
        {
            return FormatterNumberForm.Singular;
        }

        return mod10 is > 1 and < 5 && mod100 is not 12 and not 13 and not 14
            ? FormatterNumberForm.Paucal
            : FormatterNumberForm.Default;
    }

    /// <summary>
    /// Applies a lightweight fallback transformation when the locale does not provide an exact resource.
    /// </summary>
    static string ApplyFallbackTransform(string resourceValue, double count, FormatterDataUnitFallbackTransform transform) =>
        transform switch
        {
            FormatterDataUnitFallbackTransform.None => resourceValue,
            FormatterDataUnitFallbackTransform.TrimTrailingS => resourceValue.TrimEnd('s'),
            FormatterDataUnitFallbackTransform.Latvian => count == 1
                ? resourceValue.TrimEnd('i') + 's'
                : resourceValue.TrimEnd('s'),
            _ => throw new UnreachableException()
        };

    /// <summary>
    /// Determines whether Romanian needs the <c>de</c> preposition for the current value.
    /// </summary>
    static bool ShouldUseRomanianPreposition(int number)
    {
        var numeral = Math.Abs(number % 100);
        return numeral is < 1 or > 19;
    }
}

/// <summary>
/// Describes the declarative rules used by <see cref="ProfiledFormatter"/> for one locale.
/// </summary>
// Compact generated rule object for ProfiledFormatter. Each field corresponds to one structural
// formatting concern rather than to a locale name so new locales usually stay data-only.
sealed record FormatterProfile
{
    /// <summary>
    /// Initializes a profile with the generated rule tables for one locale.
    /// </summary>
    /// <param name="resourceKeyDetector">The detector used for resource-key suffix selection.</param>
    /// <param name="resourceSuffixes">The suffix map used when a resource key needs a number-based suffix.</param>
    /// <param name="resourceKeyOverrides">Exact-number overrides that take precedence over suffix detection.</param>
    /// <param name="dataUnitDetector">The detector used for data-unit word forms.</param>
    /// <param name="dataUnitSuffixes">The suffix map used for data-unit word forms.</param>
    /// <param name="dataUnitNonIntegralForm">The form to use for non-integral data-unit counts.</param>
    /// <param name="dataUnitFallbackTransform">The fallback transform applied when an exact data-unit resource is missing.</param>
    /// <param name="prepositionMode">The mode that controls locale-specific preposition placeholders.</param>
    /// <param name="secondaryPlaceholderMode">The mode that controls locale-specific secondary placeholders.</param>
    /// <param name="unitGenders">Optional grammatical genders used when converting units to words.</param>
    public FormatterProfile(
        FormatterNumberDetectorKind resourceKeyDetector,
        FormatterSuffixMap resourceSuffixes,
        FormatterResourceKeyOverride[] resourceKeyOverrides,
        FormatterNumberDetectorKind dataUnitDetector,
        FormatterSuffixMap dataUnitSuffixes,
        FormatterNumberForm dataUnitNonIntegralForm,
        FormatterDataUnitFallbackTransform dataUnitFallbackTransform,
        FormatterPrepositionMode prepositionMode,
        FormatterSecondaryPlaceholderMode secondaryPlaceholderMode,
        FrozenDictionary<TimeUnit, GrammaticalGender>? unitGenders = null)
    {
        ResourceKeyDetector = resourceKeyDetector;
        ResourceSuffixes = resourceSuffixes;
        ResourceKeyOverrides = resourceKeyOverrides;
        DataUnitDetector = dataUnitDetector;
        DataUnitSuffixes = dataUnitSuffixes;
        DataUnitNonIntegralForm = dataUnitNonIntegralForm;
        DataUnitFallbackTransform = dataUnitFallbackTransform;
        PrepositionMode = prepositionMode;
        SecondaryPlaceholderMode = secondaryPlaceholderMode;
        UnitGenders = unitGenders ?? FrozenDictionary<TimeUnit, GrammaticalGender>.Empty;
    }

    /// <summary>
    /// Gets the detector used for resource-key suffix selection.
    /// </summary>
    public FormatterNumberDetectorKind ResourceKeyDetector { get; }

    /// <summary>
    /// Gets the suffix map used when a resource key needs a number-based suffix.
    /// </summary>
    public FormatterSuffixMap ResourceSuffixes { get; }

    /// <summary>
    /// Gets the exact-number overrides that take precedence over generic suffix detection.
    /// </summary>
    public FormatterResourceKeyOverride[] ResourceKeyOverrides { get; }

    /// <summary>
    /// Gets the detector used for data-unit word forms.
    /// </summary>
    public FormatterNumberDetectorKind DataUnitDetector { get; }

    /// <summary>
    /// Gets the suffix map used for data-unit word forms.
    /// </summary>
    public FormatterSuffixMap DataUnitSuffixes { get; }

    /// <summary>
    /// Gets the form to use for non-integral data-unit counts.
    /// </summary>
    public FormatterNumberForm DataUnitNonIntegralForm { get; }

    /// <summary>
    /// Gets the fallback transform applied when a locale does not define an exact data-unit resource.
    /// </summary>
    public FormatterDataUnitFallbackTransform DataUnitFallbackTransform { get; }

    /// <summary>
    /// Gets the mode that controls whether a locale injects a preposition placeholder.
    /// </summary>
    public FormatterPrepositionMode PrepositionMode { get; }

    /// <summary>
    /// Gets the mode that controls whether a locale injects a secondary placeholder.
    /// </summary>
    public FormatterSecondaryPlaceholderMode SecondaryPlaceholderMode { get; }

    /// <summary>
    /// Gets the optional grammatical genders used when converting units to words.
    /// </summary>
    public FrozenDictionary<TimeUnit, GrammaticalGender> UnitGenders { get; }
}

/// <summary>
/// Keeps the common singular/dual/paucal/plural suffix bundle together for generated profiles.
/// </summary>
readonly record struct FormatterSuffixMap
{
    /// <summary>
    /// Initializes the suffix map for all supported grammatical forms.
    /// </summary>
    /// <param name="singular">The suffix used for singular forms.</param>
    /// <param name="dual">The suffix used for dual forms.</param>
    /// <param name="paucal">The suffix used for paucal forms.</param>
    /// <param name="plural">The suffix used for plural forms.</param>
    public FormatterSuffixMap(string singular, string dual, string paucal, string plural)
    {
        Singular = singular;
        Dual = dual;
        Paucal = paucal;
        Plural = plural;
    }

    /// <summary>
    /// Gets the suffix used for singular forms.
    /// </summary>
    public string Singular { get; }

    /// <summary>
    /// Gets the suffix used for dual forms.
    /// </summary>
    public string Dual { get; }

    /// <summary>
    /// Gets the suffix used for paucal forms.
    /// </summary>
    public string Paucal { get; }

    /// <summary>
    /// Gets the suffix used for plural forms.
    /// </summary>
    public string Plural { get; }

    /// <summary>
    /// Gets a value indicating whether all suffixes are empty.
    /// </summary>
    public bool IsEmpty =>
        Singular.Length == 0 &&
        Dual.Length == 0 &&
        Paucal.Length == 0 &&
        Plural.Length == 0;

    /// <summary>
    /// Gets the suffix for the requested grammatical form.
    /// </summary>
    /// <param name="form">The grammatical form selected by the detector.</param>
    /// <returns>The suffix associated with <paramref name="form"/>.</returns>
    public string GetSuffix(FormatterNumberForm form) =>
        form switch
        {
            FormatterNumberForm.Singular => Singular,
            FormatterNumberForm.Dual => Dual,
            FormatterNumberForm.Paucal => Paucal,
            FormatterNumberForm.Plural => Plural,
            _ => string.Empty
        };
}

/// <summary>
/// Describes an exact-number override for a generated resource key.
/// </summary>
/// <remarks>
/// Resource overrides let a profile pin exact exceptional keys without turning the shared
/// formatter engine into a locale-specific if/else chain.
/// </remarks>
readonly record struct FormatterResourceKeyOverride
{
    /// <summary>
    /// Initializes an exact override for a specific number.
    /// </summary>
    /// <param name="number">The number that triggers the override.</param>
    /// <param name="suffix">The suffix appended when the override matches.</param>
    /// <param name="exactKeys">Exact resource keys that match this override.</param>
    /// <param name="keyPrefixes">Resource-key prefixes that match this override.</param>
    public FormatterResourceKeyOverride(int number, string suffix, string[] exactKeys, string[] keyPrefixes)
    {
        Number = number;
        Suffix = suffix;
        ExactKeys = exactKeys;
        KeyPrefixes = keyPrefixes;
    }

    /// <summary>
    /// Gets the number that triggers the override.
    /// </summary>
    public int Number { get; }

    /// <summary>
    /// Gets the suffix appended when this override matches.
    /// </summary>
    public string Suffix { get; }

    /// <summary>
    /// Gets the exact resource keys that match this override.
    /// </summary>
    public string[] ExactKeys { get; }

    /// <summary>
    /// Gets the resource-key prefixes that match this override.
    /// </summary>
    public string[] KeyPrefixes { get; }

    /// <summary>
    /// Determines whether the override applies to the given key and number.
    /// </summary>
    /// <param name="resourceKey">The canonical resource key being requested.</param>
    /// <param name="number">The numeric value being formatted.</param>
    /// <returns><c>true</c> when the override applies; otherwise, <c>false</c>.</returns>
    public bool IsMatch(string resourceKey, int number)
    {
        if (number != Number)
        {
            return false;
        }

        foreach (var key in ExactKeys)
        {
            if (resourceKey == key)
            {
                return true;
            }
        }

        foreach (var prefix in KeyPrefixes)
        {
            if (resourceKey.StartsWith(prefix, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }
}
