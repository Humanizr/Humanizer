namespace Humanizer;

/// <summary>
/// Provides the standard formatter implementation for Humanizer locales.
/// </summary>
public class DefaultFormatter : IFormatter
{
    readonly LocalePhraseTable phraseTable;
    private protected LocalePhraseTable PhraseTable => phraseTable;

    /// <summary>
    /// Gets the culture used to resolve resources and localized number words.
    /// </summary>
    protected CultureInfo Culture { get; }

    /// <summary>
    /// Initializes a new formatter for the specified culture.
    /// </summary>
    /// <param name="culture">The culture used to resolve resources and localized number words.</param>
    public DefaultFormatter(CultureInfo culture)
    {
        Culture = culture;
        phraseTable = LocalePhraseTableCatalog.Resolve(culture)
            ?? throw new InvalidOperationException("The generated locale phrase tables are missing the required English fallback.");
    }

    /// <summary>
    /// Initializes a new formatter for the specified locale code.
    /// </summary>
    /// <param name="localeCode">The locale code used to construct the formatter culture.</param>
    public DefaultFormatter(string localeCode)
        : this(new CultureInfo(localeCode))
    {
    }

    /// <inheritdoc/>
    public virtual string DateHumanize_Now() =>
        phraseTable.DateNow ?? "now";

    /// <inheritdoc/>
    public virtual string DateHumanize_Never() =>
        phraseTable.DateNever ?? "never";

    /// <inheritdoc/>
    public virtual string DateHumanize(TimeUnit timeUnit, Tense timeUnitTense, int unit) =>
        TryFormatDateFromPhraseTable(timeUnit, timeUnitTense, unit, out var result)
            ? result
            : throw new InvalidOperationException($"Missing generated relative-date phrase for '{Culture.Name}' and unit '{timeUnit}'.");

    /// <inheritdoc/>
    public virtual string TimeSpanHumanize_Zero() =>
        phraseTable.TimeSpanZero ?? "no time";

    /// <inheritdoc/>
    public virtual string TimeSpanHumanize(TimeUnit timeUnit, int unit, bool toWords = false) =>
        TryFormatTimeSpanFromPhraseTable(timeUnit, unit, toWords, out var result)
            ? result
            : throw new InvalidOperationException($"Missing generated time-span phrase for '{Culture.Name}' and unit '{timeUnit}'.");

    /// <inheritdoc/>
    public virtual string TimeSpanHumanize_Age()
    {
        return phraseTable.TimeSpanAge ?? "{0}";
    }

    /// <inheritdoc/>
    public virtual string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true)
    {
        if (TryFormatDataUnitFromPhraseTable(dataUnit, count, toSymbol, out var generated))
        {
            return generated;
        }

        throw new InvalidOperationException($"Missing generated data-unit phrase for '{Culture.Name}' and unit '{dataUnit}'.");
    }

    /// <inheritdoc/>
    public virtual string TimeUnitHumanize(TimeUnit timeUnit)
    {
        if (phraseTable.TryGetTimeUnitPhrase(timeUnit, out var generatedPhrase) &&
            generatedPhrase.Symbol is { } generatedSymbol)
        {
            return generatedSymbol;
        }

        throw new InvalidOperationException($"Missing generated time-unit phrase for '{Culture.Name}' and unit '{timeUnit}'.");
    }

    /// <summary>
    /// Converts a number to words for the current culture.
    /// </summary>
    /// <param name="unit">The unit being formatted.</param>
    /// <param name="number">The numeric value to convert.</param>
    /// <param name="culture">The culture to use when generating the words.</param>
    /// <returns>The number rendered as words for the configured culture.</returns>
    protected virtual string NumberToWords(TimeUnit unit, int number, CultureInfo culture) =>
        number.ToWords(culture);

    internal virtual FormatterNumberForm GetDatePhraseForm(TimeUnit unit, Tense tense, int number) =>
        Math.Abs(number) == 1 ? FormatterNumberForm.Singular : FormatterNumberForm.Default;

    internal virtual FormatterNumberForm GetTimeSpanPhraseForm(TimeUnit unit, int number, bool toWords) =>
        Math.Abs(number) == 1 ? FormatterNumberForm.Singular : FormatterNumberForm.Default;

    internal virtual FormatterNumberForm GetDataUnitPhraseForm(DataUnit dataUnit, double count) =>
        Math.Abs(count) == 1d ? FormatterNumberForm.Singular : FormatterNumberForm.Default;

    internal virtual string ResolveDatePhraseForms(LocalizedPhraseForms forms, FormatterNumberForm form) =>
        forms.Resolve(form);

    internal virtual string ResolveTimeSpanPhraseForms(LocalizedPhraseForms forms, FormatterNumberForm form) =>
        forms.Resolve(form);

    internal virtual string ResolveDataUnitPhraseForms(LocalizedPhraseForms forms, FormatterNumberForm form) =>
        forms.Resolve(form);

    internal virtual bool ShouldUseDatePhraseTable(TimeUnit unit, Tense tense, int count, LocalizedDatePhrase phrase) =>
        true;

    internal virtual bool ShouldUseTimeSpanPhraseTable(TimeUnit unit, int count, bool toWords, LocalizedTimeSpanPhrase phrase) =>
        true;

    internal virtual bool ShouldUseDatePhraseTemplate(TimeUnit unit, Tense tense, int count, LocalizedDatePhrase phrase) =>
        false;

    internal virtual bool ShouldAppendImplicitDataUnitPluralSuffix(DataUnit dataUnit, double count, FormatterNumberForm form, LocalizedPhraseForms forms, PhraseTemplate? template) =>
        count > 1 &&
        form == FormatterNumberForm.Default &&
        forms.Singular is null &&
        template is null;

    internal virtual string TransformDataUnitResult(DataUnit dataUnit, double count, FormatterNumberForm form, string result, LocalizedPhraseForms forms, PhraseTemplate? template) =>
        result;

    internal virtual string GetDatePhraseSecondaryPlaceholder(TimeUnit unit, Tense tense, int count) =>
        string.Empty;

    internal virtual string GetTimeSpanPhraseSecondaryPlaceholder(TimeUnit unit, int count, bool toWords) =>
        string.Empty;

    private protected bool TryFormatDataUnitFromPhraseTable(DataUnit dataUnit, double count, bool toSymbol, out string result)
    {
        result = null!;
        if (!phraseTable.TryGetDataUnitPhrase(dataUnit, out var phrase))
        {
            return false;
        }

        if (toSymbol)
        {
            if (phrase.Symbol is null)
            {
                return false;
            }

            result = phrase.Symbol;
            return true;
        }

        if (phrase.Forms is not { } forms)
        {
            return false;
        }

        var form = GetDataUnitPhraseForm(dataUnit, count);
        result = ResolveDataUnitPhraseForms(forms, form);
        if (phrase.Template is { } template)
        {
            result = RenderTemplate(template, FormatCountValue(count), result, string.Empty);
        }
        else if (ShouldAppendImplicitDataUnitPluralSuffix(dataUnit, count, form, forms, phrase.Template))
        {
            result += "s";
        }

        result = TransformDataUnitResult(dataUnit, count, form, result, forms, phrase.Template);

        return true;
    }

    bool TryFormatDateFromPhraseTable(TimeUnit unit, Tense tense, int count, out string result)
    {
        result = null!;
        if (count == 0 && phraseTable.DateNow is { } now)
        {
            result = now;
            return true;
        }

        if (!phraseTable.TryGetDatePhrase(unit, tense, out var phrase))
        {
            return false;
        }

        if (!ShouldUseDatePhraseTable(unit, tense, count, phrase))
        {
            return false;
        }

        if (count == 1 && phrase.Single is { } single)
        {
            result = single;
            return true;
        }

        if (count == 2 &&
            phrase.Template is { Name: "two", Template: { } exactTwo } &&
            ShouldUseDatePhraseTemplate(unit, tense, count, phrase))
        {
            result = exactTwo;
            return true;
        }

        if (phrase.Multiple is not { } multiple)
        {
            return false;
        }

        var form = GetDatePhraseForm(unit, tense, count);
        result = RenderCountedPhrase(
            multiple,
            ResolveDatePhraseForms(multiple.Forms, form),
            count.ToString(CultureInfo.CurrentCulture),
            GetDatePhraseSecondaryPlaceholder(unit, tense, count));
        return true;
    }

    bool TryFormatTimeSpanFromPhraseTable(TimeUnit unit, int count, bool toWords, out string result)
    {
        result = null!;
        if (count == 0 && toWords && phraseTable.TimeSpanZero is { } zero)
        {
            result = zero;
            return true;
        }

        if (!phraseTable.TryGetTimeSpanPhrase(unit, out var phrase))
        {
            return false;
        }

        if (!ShouldUseTimeSpanPhraseTable(unit, count, toWords, phrase))
        {
            return false;
        }

        if (count == 1)
        {
            var single = toWords ? phrase.SingleWordsVariant ?? phrase.Single : phrase.Single;
            if (single is not null)
            {
                result = single;
                return true;
            }
        }

        var multiple = toWords ? phrase.MultipleWordsVariant ?? phrase.Multiple : phrase.Multiple;
        if (multiple is not { } countedPhrase)
        {
            return false;
        }

        var form = GetTimeSpanPhraseForm(unit, count, toWords);
        result = RenderCountedPhrase(
            countedPhrase,
            ResolveTimeSpanPhraseForms(countedPhrase.Forms, form),
            FormatCountValue(unit, count, toWords),
            GetTimeSpanPhraseSecondaryPlaceholder(unit, count, toWords));
        return true;
    }

    string FormatCountValue(TimeUnit unit, int number, bool toWords) =>
        toWords
            ? NumberToWords(unit, number, Culture)
            : number.ToString(CultureInfo.CurrentCulture);

    static string FormatCountValue(double number) =>
        number.ToString(CultureInfo.CurrentCulture);

    static string RenderCountedPhrase(LocalizedCountedPhrase phrase, string unitText, string countValue, string secondaryPlaceholder)
    {
        if (phrase.CountPlacement == PhraseCountPlacement.None)
        {
            return RenderPattern(unitText, countValue, secondaryPlaceholder);
        }

        if (phrase.Template is { } template)
        {
            return RenderTemplate(template, countValue, unitText, secondaryPlaceholder);
        }

        var beforeCount = RenderPattern(phrase.BeforeCountText, countValue, secondaryPlaceholder);
        var renderedUnit = RenderPattern(unitText, countValue, secondaryPlaceholder);
        var afterCount = RenderPattern(phrase.AfterCountText, countValue, secondaryPlaceholder);

        return phrase.CountPlacement switch
        {
            PhraseCountPlacement.BeforeForm => JoinPhraseParts(beforeCount, countValue, renderedUnit, afterCount),
            PhraseCountPlacement.AfterForm => JoinPhraseParts(beforeCount, renderedUnit, countValue, afterCount),
            _ => RenderPattern(renderedUnit, countValue, secondaryPlaceholder)
        };
    }

    static string RenderTemplate(PhraseTemplate template, string countValue, string unitText, string secondaryPlaceholder) =>
        template.Template
            .Replace("{count}", countValue)
            .Replace("{unit}", unitText)
            .Replace("{prep}", secondaryPlaceholder)
            .Replace("{value}", unitText)
            .Replace("{0}", countValue);

    static string RenderPattern(string? pattern, string countValue, string secondaryPlaceholder)
    {
        if (string.IsNullOrEmpty(pattern))
        {
            return pattern ?? string.Empty;
        }

        var resolvedPattern = pattern!;
        return resolvedPattern
            .Replace("{count}", countValue)
            .Replace("{prep}", secondaryPlaceholder)
            .Replace("{0}", countValue)
            .Replace("{1}", secondaryPlaceholder)
            .Trim();
    }

    static string JoinPhraseParts(string? first, string? second, string? third, string? fourth)
    {
        var partCount = 0;
        var totalLength = 0;
        MeasurePhrasePart(first, ref partCount, ref totalLength);
        MeasurePhrasePart(second, ref partCount, ref totalLength);
        MeasurePhrasePart(third, ref partCount, ref totalLength);
        MeasurePhrasePart(fourth, ref partCount, ref totalLength);

        if (partCount == 0)
        {
            return string.Empty;
        }

        if (partCount == 1)
        {
            return FirstPhrasePart(first, second, third, fourth);
        }

        var builder = new StringBuilder(totalLength + partCount - 1);
        AppendPhrasePart(builder, first);
        AppendPhrasePart(builder, second);
        AppendPhrasePart(builder, third);
        AppendPhrasePart(builder, fourth);
        return builder.ToString();
    }

    static void MeasurePhrasePart(string? part, ref int partCount, ref int totalLength)
    {
        if (string.IsNullOrWhiteSpace(part))
        {
            return;
        }

        partCount++;
        totalLength += part!.Length;
    }

    static string FirstPhrasePart(string? first, string? second, string? third, string? fourth)
    {
        if (!string.IsNullOrWhiteSpace(first))
        {
            return first!;
        }

        if (!string.IsNullOrWhiteSpace(second))
        {
            return second!;
        }

        return !string.IsNullOrWhiteSpace(third) ? third! : fourth!;
    }

    static void AppendPhrasePart(StringBuilder builder, string? part)
    {
        if (string.IsNullOrWhiteSpace(part))
        {
            return;
        }

        if (builder.Length > 0)
        {
            builder.Append(' ');
        }

        builder.Append(part);
    }
}