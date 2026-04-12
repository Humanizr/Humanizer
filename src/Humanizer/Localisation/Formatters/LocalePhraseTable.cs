using System.Globalization;

namespace Humanizer;

enum PhraseCountPlacement
{
    None,
    BeforeForm,
    AfterForm
}

readonly record struct PhraseTemplate(string? Name, string Template);

readonly record struct LocalizedPhraseForms(
    string Default,
    string? Singular = null,
    string? Dual = null,
    string? Paucal = null,
    string? Plural = null)
{
    public string Resolve(FormatterNumberForm form) =>
        form switch
        {
            FormatterNumberForm.Singular => Singular ?? Default,
            FormatterNumberForm.Dual => Dual ?? Default,
            FormatterNumberForm.Paucal => Paucal ?? Default,
            FormatterNumberForm.Plural => Plural ?? Default,
            _ => Default
        };
}

readonly record struct LocalizedCountedPhrase(
    LocalizedPhraseForms Forms,
    PhraseCountPlacement CountPlacement,
    string? BeforeCountText = null,
    string? AfterCountText = null,
    PhraseTemplate? Template = null);

readonly record struct LocalizedDatePhrase(
    string? Single = null,
    LocalizedCountedPhrase? Multiple = null,
    PhraseTemplate? Template = null);

readonly record struct LocalizedTimeSpanPhrase(
    string? Single = null,
    string? SingleWordsVariant = null,
    LocalizedCountedPhrase? Multiple = null,
    LocalizedCountedPhrase? MultipleWordsVariant = null,
    PhraseTemplate? Template = null);

readonly record struct LocalizedUnitPhrase(
    LocalizedPhraseForms? Forms = null,
    string? Symbol = null,
    PhraseTemplate? Template = null);

sealed class LocalePhraseTable(
    string? dateNow,
    string? dateNever,
    string? timeSpanZero,
    string? timeSpanAge,
    LocalizedDatePhrase?[] datePast,
    LocalizedDatePhrase?[] dateFuture,
    LocalizedTimeSpanPhrase?[] timeSpanUnits,
    LocalizedUnitPhrase?[] dataUnits,
    LocalizedUnitPhrase?[] timeUnits)
{
    readonly LocalizedDatePhrase?[] datePast = datePast;
    readonly LocalizedDatePhrase?[] dateFuture = dateFuture;
    readonly LocalizedTimeSpanPhrase?[] timeSpanUnits = timeSpanUnits;
    readonly LocalizedUnitPhrase?[] dataUnits = dataUnits;
    readonly LocalizedUnitPhrase?[] timeUnits = timeUnits;

    public string? DateNow { get; } = dateNow;
    public string? DateNever { get; } = dateNever;
    public string? TimeSpanZero { get; } = timeSpanZero;
    public string? TimeSpanAge { get; } = timeSpanAge;
    public string? DateHumanizeNow => DateNow;
    public string? DateHumanizeNever => DateNever;

    public bool TryGetDatePhrase(TimeUnit unit, Tense tense, out LocalizedDatePhrase phrase)
    {
        var value = (tense == Tense.Future ? dateFuture : datePast)[(int)unit];
        if (value is { } found)
        {
            phrase = found;
            return true;
        }

        phrase = default;
        return false;
    }

    public bool TryGetTimeSpanPhrase(TimeUnit unit, out LocalizedTimeSpanPhrase phrase)
    {
        var value = timeSpanUnits[(int)unit];
        if (value is { } found)
        {
            phrase = found;
            return true;
        }

        phrase = default;
        return false;
    }

    public bool TryGetDataUnitPhrase(DataUnit unit, out LocalizedUnitPhrase phrase)
    {
        var value = dataUnits[(int)unit];
        if (value is { } found)
        {
            phrase = found;
            return true;
        }

        phrase = default;
        return false;
    }

    public bool TryGetTimeUnitPhrase(TimeUnit unit, out LocalizedUnitPhrase phrase)
    {
        var value = timeUnits[(int)unit];
        if (value is { } found)
        {
            phrase = found;
            return true;
        }

        phrase = default;
        return false;
    }

    public LocalizedDatePhrase? GetDateHumanize(TimeUnit unit, Tense tense) =>
        TryGetDatePhrase(unit, tense, out var phrase) ? phrase : null;

    public LocalizedTimeSpanPhrase? GetTimeSpan(TimeUnit unit) =>
        TryGetTimeSpanPhrase(unit, out var phrase) ? phrase : null;

    public LocalizedUnitPhrase? GetDataUnit(DataUnit unit) =>
        TryGetDataUnitPhrase(unit, out var phrase) ? phrase : null;

    public LocalizedUnitPhrase? GetTimeUnit(TimeUnit unit) =>
        TryGetTimeUnitPhrase(unit, out var phrase) ? phrase : null;

    public string? GetTimeUnitSymbol(TimeUnit unit) =>
        GetTimeUnit(unit)?.Symbol;
}

static partial class LocalePhraseTableCatalog
{
    public static LocalePhraseTable? Resolve(CultureInfo culture)
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

        return ResolveCore("en");
    }

    internal static partial LocalePhraseTable? ResolveCore(string localeCode);
}