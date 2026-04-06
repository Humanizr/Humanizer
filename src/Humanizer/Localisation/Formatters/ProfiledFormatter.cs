using System.Diagnostics;

namespace Humanizer;

/// <summary>
/// Describes how a profile detects grammatical-number forms.
/// </summary>
enum FormatterNumberDetectorKind
{
    /// <summary>
    /// Use the default grammatical form without applying a number-specific override.
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

[Flags]
enum FormatterTimeUnitMask
{
    None = 0,
    Millisecond = 1 << 0,
    Second = 1 << 1,
    Minute = 1 << 2,
    Hour = 1 << 3,
    Day = 1 << 4,
    Week = 1 << 5,
    Month = 1 << 6,
    Year = 1 << 7,
    All = Millisecond | Second | Minute | Hour | Day | Week | Month | Year
}

[Flags]
enum FormatterTenseMask
{
    None = 0,
    Past = 1 << 0,
    Future = 1 << 1,
    Both = Past | Future
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
    /// Converts numbers using a gender-aware rule when the profile provides one.
    /// </summary>
    protected override string NumberToWords(TimeUnit unit, int number, CultureInfo culture) =>
        profile.UnitGenders.TryGetValue(unit, out var gender)
            ? number.ToWords(gender, culture)
            : base.NumberToWords(unit, number, culture);

    internal override FormatterNumberForm GetDatePhraseForm(TimeUnit unit, Tense tense, int number)
        => TryGetExactDateForm(unit, tense, number, out var form)
            ? form
            : DetectNumberForm(number, profile.PhraseDetector);

    internal override FormatterNumberForm GetTimeSpanPhraseForm(TimeUnit unit, int number, bool toWords)
        => TryGetExactTimeSpanForm(unit, number, out var form)
            ? form
            : DetectNumberForm(number, profile.PhraseDetector);

    internal override FormatterNumberForm GetDataUnitPhraseForm(DataUnit dataUnit, double count) =>
        DetectDataUnitForm(count, profile.DataUnitDetector, profile.DataUnitNonIntegralForm);

    internal override string ResolveDatePhraseForms(LocalizedPhraseForms forms, FormatterNumberForm form) =>
        ResolveProfiledPhraseForms(forms, form, profile.PhraseDetector);

    internal override string ResolveTimeSpanPhraseForms(LocalizedPhraseForms forms, FormatterNumberForm form) =>
        ResolveProfiledPhraseForms(forms, form, profile.PhraseDetector);

    internal override string ResolveDataUnitPhraseForms(LocalizedPhraseForms forms, FormatterNumberForm form) =>
        ResolveProfiledPhraseForms(forms, form, profile.DataUnitDetector);

    internal override bool ShouldUseDatePhraseTable(TimeUnit unit, Tense tense, int count, LocalizedDatePhrase phrase) =>
        true;

    internal override bool ShouldUseTimeSpanPhraseTable(TimeUnit unit, int count, bool toWords, LocalizedTimeSpanPhrase phrase) =>
        true;

    internal override bool ShouldUseDatePhraseTemplate(TimeUnit unit, Tense tense, int count, LocalizedDatePhrase phrase)
        => count == 2 &&
            phrase.Template is { Name: "two" } &&
            HasExactDateTwoForm(unit, tense, count);

    internal override bool ShouldAppendImplicitDataUnitPluralSuffix(DataUnit dataUnit, double count, FormatterNumberForm form, LocalizedPhraseForms forms, PhraseTemplate? template) =>
        base.ShouldAppendImplicitDataUnitPluralSuffix(dataUnit, count, form, forms, template);

    internal override string TransformDataUnitResult(DataUnit dataUnit, double count, FormatterNumberForm form, string result, LocalizedPhraseForms forms, PhraseTemplate? template) =>
        profile.DataUnitFallbackTransform == FormatterDataUnitFallbackTransform.None ||
        profile.DataUnitDetector != FormatterNumberDetectorKind.None
            ? result
            : ApplyFallbackTransform(result, count, profile.DataUnitFallbackTransform);

    internal override string GetDatePhraseSecondaryPlaceholder(TimeUnit unit, Tense tense, int count) =>
        GetSecondaryPlaceholder(unit, count);

    internal override string GetTimeSpanPhraseSecondaryPlaceholder(TimeUnit unit, int count, bool toWords) =>
        GetSecondaryPlaceholder(unit, count);

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

    static string ResolveProfiledPhraseForms(LocalizedPhraseForms forms, FormatterNumberForm form, FormatterNumberDetectorKind detector) =>
        detector switch
        {
            FormatterNumberDetectorKind.Between2And4Paucal or FormatterNumberDetectorKind.SouthSlavic => form switch
            {
                FormatterNumberForm.Paucal => forms.Paucal ?? forms.Singular ?? forms.Dual ?? forms.Plural ?? forms.Default,
                FormatterNumberForm.Dual => forms.Dual ?? forms.Singular ?? forms.Paucal ?? forms.Default,
                _ => forms.Resolve(form)
            },
            FormatterNumberDetectorKind.Slovenian => form switch
            {
                FormatterNumberForm.Dual => forms.Dual ?? forms.Singular ?? forms.Default,
                FormatterNumberForm.Paucal => forms.Paucal ?? forms.Default,
                _ => forms.Resolve(form)
            },
            FormatterNumberDetectorKind.Russian => form switch
            {
                FormatterNumberForm.Paucal => forms.Paucal ?? forms.Dual ?? forms.Default,
                _ => forms.Resolve(form)
            },
            _ => forms.Resolve(form)
        };

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

    bool TryGetExactDateForm(TimeUnit unit, Tense tense, int number, out FormatterNumberForm form)
    {
        foreach (var rule in profile.ExactDateForms)
        {
            if (rule.AppliesTo(unit, tense, number))
            {
                form = rule.Form;
                return true;
            }
        }

        form = default;
        return false;
    }

    bool TryGetExactTimeSpanForm(TimeUnit unit, int number, out FormatterNumberForm form)
    {
        foreach (var rule in profile.ExactTimeSpanForms)
        {
            if (rule.AppliesTo(unit, number))
            {
                form = rule.Form;
                return true;
            }
        }

        form = default;
        return false;
    }

    /// <summary>
    /// Determines whether Romanian needs the <c>de</c> preposition for the current value.
    /// </summary>
    static bool ShouldUseRomanianPreposition(int number)
    {
        var numeral = Math.Abs(number % 100);
        return numeral is < 1 or > 19;
    }

    string GetSecondaryPlaceholder(TimeUnit unit, int number) =>
        (profile.PrepositionMode, profile.SecondaryPlaceholderMode) switch
        {
            (FormatterPrepositionMode.None, FormatterSecondaryPlaceholderMode.None) => string.Empty,
            (FormatterPrepositionMode.RomanianDe, FormatterSecondaryPlaceholderMode.None) =>
                ShouldUseRomanianPreposition(number) ? " de" : string.Empty,
            (FormatterPrepositionMode.None, FormatterSecondaryPlaceholderMode.LuxembourgishEifelerN) =>
                EifelerRule.DoesApply(NumberToWords(unit, number, Culture).AsSpan()) ? string.Empty : LuxembourgishEifelerSuffix.ToString(),
            _ => throw new UnreachableException()
        };

    bool HasExactDateTwoForm(TimeUnit unit, Tense tense, int number)
    {
        foreach (var rule in profile.ExactDateForms)
        {
            if (rule.AppliesTo(unit, tense, number))
            {
                return true;
            }
        }

        return false;
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
    /// <param name="phraseDetector">The detector used for date and time-span grammatical forms.</param>
    /// <param name="exactDateForms">Exact-number date form rules that take precedence over the generic detector.</param>
    /// <param name="exactTimeSpanForms">Exact-number time-span form rules that take precedence over the generic detector.</param>
    /// <param name="dataUnitDetector">The detector used for data-unit word forms.</param>
    /// <param name="dataUnitNonIntegralForm">The form to use for non-integral data-unit counts.</param>
    /// <param name="dataUnitFallbackTransform">The fallback transform applied when an exact data-unit resource is missing.</param>
    /// <param name="prepositionMode">The mode that controls locale-specific preposition placeholders.</param>
    /// <param name="secondaryPlaceholderMode">The mode that controls locale-specific secondary placeholders.</param>
    /// <param name="unitGenders">Optional grammatical genders used when converting units to words.</param>
    public FormatterProfile(
        FormatterNumberDetectorKind phraseDetector,
        FormatterDateFormRule[] exactDateForms,
        FormatterTimeSpanFormRule[] exactTimeSpanForms,
        FormatterNumberDetectorKind dataUnitDetector,
        FormatterNumberForm dataUnitNonIntegralForm,
        FormatterDataUnitFallbackTransform dataUnitFallbackTransform,
        FormatterPrepositionMode prepositionMode,
        FormatterSecondaryPlaceholderMode secondaryPlaceholderMode,
        FrozenDictionary<TimeUnit, GrammaticalGender>? unitGenders = null)
    {
        PhraseDetector = phraseDetector;
        ExactDateForms = exactDateForms;
        ExactTimeSpanForms = exactTimeSpanForms;
        DataUnitDetector = dataUnitDetector;
        DataUnitNonIntegralForm = dataUnitNonIntegralForm;
        DataUnitFallbackTransform = dataUnitFallbackTransform;
        PrepositionMode = prepositionMode;
        SecondaryPlaceholderMode = secondaryPlaceholderMode;
        UnitGenders = unitGenders ?? FrozenDictionary<TimeUnit, GrammaticalGender>.Empty;
    }

    /// <summary>
    /// Gets the detector used for date and time-span grammatical forms.
    /// </summary>
    public FormatterNumberDetectorKind PhraseDetector { get; }

    /// <summary>
    /// Gets the exact-number date overrides that take precedence over the generic detector.
    /// </summary>
    public FormatterDateFormRule[] ExactDateForms { get; }

    /// <summary>
    /// Gets the exact-number time-span overrides that take precedence over the generic detector.
    /// </summary>
    public FormatterTimeSpanFormRule[] ExactTimeSpanForms { get; }

    /// <summary>
    /// Gets the detector used for data-unit word forms.
    /// </summary>
    public FormatterNumberDetectorKind DataUnitDetector { get; }

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
/// Describes an exact-number date form rule for a generated formatter profile.
/// </summary>
readonly record struct FormatterDateFormRule
{
    /// <summary>
    /// Initializes an exact-number date form rule.
    /// </summary>
    /// <param name="number">The number that triggers the rule.</param>
    /// <param name="units">The units covered by the rule.</param>
    /// <param name="tenses">The tenses covered by the rule.</param>
    /// <param name="form">The form selected when the rule matches.</param>
    public FormatterDateFormRule(int number, FormatterTimeUnitMask units, FormatterTenseMask tenses, FormatterNumberForm form)
    {
        Number = number;
        Units = units;
        Tenses = tenses;
        Form = form;
    }

    /// <summary>
    /// Gets the number that triggers the rule.
    /// </summary>
    public int Number { get; }

    /// <summary>
    /// Gets the units covered by the rule.
    /// </summary>
    public FormatterTimeUnitMask Units { get; }

    /// <summary>
    /// Gets the tenses covered by the rule.
    /// </summary>
    public FormatterTenseMask Tenses { get; }

    /// <summary>
    /// Gets the form selected when the rule matches.
    /// </summary>
    public FormatterNumberForm Form { get; }

    /// <summary>
    /// Determines whether the rule applies to the given date phrase.
    /// </summary>
    /// <param name="unit">The time unit being formatted.</param>
    /// <param name="tense">The tense being formatted.</param>
    /// <param name="number">The count being formatted.</param>
    /// <returns><c>true</c> when the rule applies; otherwise, <c>false</c>.</returns>
    public bool AppliesTo(TimeUnit unit, Tense tense, int number) =>
        number == Number &&
        (Units & GetTimeUnitMask(unit)) != 0 &&
        (Tenses & GetTenseMask(tense)) != 0;

    /// <summary>
    /// Maps runtime time units onto the generated rule mask.
    /// </summary>
    internal static FormatterTimeUnitMask GetTimeUnitMask(TimeUnit unit) =>
        unit switch
        {
            TimeUnit.Millisecond => FormatterTimeUnitMask.Millisecond,
            TimeUnit.Second => FormatterTimeUnitMask.Second,
            TimeUnit.Minute => FormatterTimeUnitMask.Minute,
            TimeUnit.Hour => FormatterTimeUnitMask.Hour,
            TimeUnit.Day => FormatterTimeUnitMask.Day,
            TimeUnit.Week => FormatterTimeUnitMask.Week,
            TimeUnit.Month => FormatterTimeUnitMask.Month,
            TimeUnit.Year => FormatterTimeUnitMask.Year,
            _ => FormatterTimeUnitMask.None
        };

    static FormatterTenseMask GetTenseMask(Tense tense) =>
        tense == Tense.Future
            ? FormatterTenseMask.Future
            : FormatterTenseMask.Past;
}

/// <summary>
/// Describes an exact-number time-span form rule for a generated formatter profile.
/// </summary>
readonly record struct FormatterTimeSpanFormRule
{
    /// <summary>
    /// Initializes an exact-number time-span form rule.
    /// </summary>
    /// <param name="number">The number that triggers the rule.</param>
    /// <param name="units">The units covered by the rule.</param>
    /// <param name="form">The form selected when the rule matches.</param>
    public FormatterTimeSpanFormRule(int number, FormatterTimeUnitMask units, FormatterNumberForm form)
    {
        Number = number;
        Units = units;
        Form = form;
    }

    /// <summary>
    /// Gets the number that triggers the rule.
    /// </summary>
    public int Number { get; }

    /// <summary>
    /// Gets the units covered by the rule.
    /// </summary>
    public FormatterTimeUnitMask Units { get; }

    /// <summary>
    /// Gets the form selected when the rule matches.
    /// </summary>
    public FormatterNumberForm Form { get; }

    /// <summary>
    /// Determines whether the rule applies to the given key and number.
    /// </summary>
    /// <param name="unit">The time unit being formatted.</param>
    /// <param name="number">The numeric value being formatted.</param>
    /// <returns><c>true</c> when the rule applies; otherwise, <c>false</c>.</returns>
    public bool AppliesTo(TimeUnit unit, int number) =>
        number == Number &&
        (Units & FormatterDateFormRule.GetTimeUnitMask(unit)) != 0;
}