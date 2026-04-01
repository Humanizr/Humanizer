using System.Diagnostics;

namespace Humanizer;

enum FormatterNumberDetectorKind
{
    None,
    SingularPlural,
    ArabicLike,
    Between2And4Paucal,
    SouthSlavic,
    Slovenian,
    Russian,
    Lithuanian
}

enum FormatterNumberForm
{
    Default,
    Singular,
    Dual,
    Paucal,
    Plural
}

enum FormatterDataUnitFallbackTransform
{
    None,
    TrimTrailingS,
    Latvian
}

enum FormatterPrepositionMode
{
    None,
    RomanianDe
}

enum FormatterSecondaryPlaceholderMode
{
    None,
    LuxembourgishEifelerN
}

// Shared formatter kernel used whenever a locale can be expressed as declarative resource-key,
// suffix, preposition, and gender-selection rules. The generator builds FormatterProfile
// instances from locale-owned YAML so runtime formatting stays branch-light and parse-free.
sealed class ProfiledFormatter(CultureInfo culture, FormatterProfile profile) : DefaultFormatter(culture)
{
    const char LuxembourgishEifelerSuffix = 'n';
    readonly FormatterProfile profile = profile;

    public override string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true)
    {
        if (toSymbol)
        {
            return base.DataUnitHumanize(dataUnit, count, toSymbol);
        }

        if (profile.DataUnitDetector == FormatterNumberDetectorKind.None && profile.DataUnitSuffixes.IsEmpty)
        {
            return ApplyFallbackTransform(base.DataUnitHumanize(dataUnit, count, toSymbol), count, profile.DataUnitFallbackTransform);
        }

        var resourceKey = DataUnitResourceKeys.GetResourceKey(dataUnit, false);
        var numberForm = DetectDataUnitForm(count, profile.DataUnitDetector, profile.DataUnitNonIntegralForm);
        var suffix = profile.DataUnitSuffixes.GetSuffix(numberForm);

        if (suffix.Length != 0 &&
            Resources.TryGetResourceWithFallback(resourceKey + suffix, Culture, out var exactForm))
        {
            return exactForm;
        }

        if (Resources.TryGetResourceWithFallback(resourceKey, Culture, out var resource))
        {
            return resource;
        }

        return ApplyFallbackTransform(base.DataUnitHumanize(dataUnit, count, toSymbol), count, profile.DataUnitFallbackTransform);
    }

    protected override string NumberToWords(TimeUnit unit, int number, CultureInfo culture) =>
        profile.UnitGenders.TryGetValue(unit, out var gender)
            ? number.ToWords(gender, culture)
            : base.NumberToWords(unit, number, culture);

    protected override string Format(TimeUnit unit, string resourceKey, int number, bool toWords = false)
    {
        var resolvedKey = GetResourceKey(resourceKey, number);
        var resourceString = Resources.GetResource(resolvedKey, Culture);
        var numberAsWord = NumberToWords(unit, number, Culture);
        object value = toWords ? numberAsWord : number;

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

    protected override string GetResourceKey(string resourceKey, int number)
    {
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

    static FormatterNumberForm DetectDataUnitForm(double count, FormatterNumberDetectorKind detector, FormatterNumberForm nonIntegralForm)
    {
        var absoluteCount = Math.Abs(count);
        if (absoluteCount % 1 != 0)
        {
            return nonIntegralForm;
        }

        return DetectNumberForm((int)absoluteCount, detector);
    }

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

    static FormatterNumberForm DetectSouthSlavicForm(int absoluteNumber)
    {
        var mod10 = absoluteNumber % 10;
        var mod100 = absoluteNumber % 100;

        if (mod10 == 1 && mod100 != 11)
        {
            return FormatterNumberForm.Singular;
        }

        return mod10 is > 1 and < 5 && mod100 is not 12 and not 13 and not 14
            ? FormatterNumberForm.Paucal
            : FormatterNumberForm.Default;
    }

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

    static bool ShouldUseRomanianPreposition(int number)
    {
        var numeral = Math.Abs(number % 100);
        return numeral is < 1 or > 19;
    }
}

// Compact generated rule object for ProfiledFormatter. Each field corresponds to one structural
// formatting concern rather than to a locale name so new locales usually stay data-only.
sealed class FormatterProfile(
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
    public FormatterNumberDetectorKind ResourceKeyDetector { get; } = resourceKeyDetector;
    public FormatterSuffixMap ResourceSuffixes { get; } = resourceSuffixes;
    public FormatterResourceKeyOverride[] ResourceKeyOverrides { get; } = resourceKeyOverrides;
    public FormatterNumberDetectorKind DataUnitDetector { get; } = dataUnitDetector;
    public FormatterSuffixMap DataUnitSuffixes { get; } = dataUnitSuffixes;
    public FormatterNumberForm DataUnitNonIntegralForm { get; } = dataUnitNonIntegralForm;
    public FormatterDataUnitFallbackTransform DataUnitFallbackTransform { get; } = dataUnitFallbackTransform;
    public FormatterPrepositionMode PrepositionMode { get; } = prepositionMode;
    public FormatterSecondaryPlaceholderMode SecondaryPlaceholderMode { get; } = secondaryPlaceholderMode;
    public FrozenDictionary<TimeUnit, GrammaticalGender> UnitGenders { get; } = unitGenders ?? FrozenDictionary<TimeUnit, GrammaticalGender>.Empty;
}

// Keeps the common singular/dual/paucal/plural suffix bundle together so the generator can emit
// formatter rule tables without duplicating boilerplate constructor calls at every use site.
readonly record struct FormatterSuffixMap(
    string Singular,
    string Dual,
    string Paucal,
    string Plural)
{
    public bool IsEmpty =>
        Singular.Length == 0 &&
        Dual.Length == 0 &&
        Paucal.Length == 0 &&
        Plural.Length == 0;

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

// Resource overrides let a profile pin exact exceptional keys without turning the shared
// formatter engine into a locale-specific if/else chain.
readonly record struct FormatterResourceKeyOverride(
    int Number,
    string Suffix,
    string[] ExactKeys,
    string[] KeyPrefixes)
{
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
