namespace Humanizer;

/// <summary>
/// Provides the standard formatter implementation for Humanizer locales.
/// </summary>
public class DefaultFormatter : IFormatter
{
    /// <summary>
    /// Gets the culture used to resolve resources and localized number words.
    /// </summary>
    protected CultureInfo Culture { get; }

    /// <summary>
    /// Initializes a new formatter for the specified culture.
    /// </summary>
    /// <param name="culture">The culture used to resolve resources and localized number words.</param>
    public DefaultFormatter(CultureInfo culture) => Culture = culture;

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
        GetResourceForDate(TimeUnit.Millisecond, Tense.Past, 0);

    /// <inheritdoc/>
    public virtual string DateHumanize_Never() =>
        Format(ResourceKeys.DateHumanize.Never);

    /// <inheritdoc/>
    public virtual string DateHumanize(TimeUnit timeUnit, Tense timeUnitTense, int unit) =>
        GetResourceForDate(timeUnit, timeUnitTense, unit);

    /// <inheritdoc/>
    public virtual string TimeSpanHumanize_Zero() =>
        GetResourceForTimeSpan(TimeUnit.Millisecond, 0, true);

    /// <inheritdoc/>
    public virtual string TimeSpanHumanize(TimeUnit timeUnit, int unit, bool toWords = false) =>
        GetResourceForTimeSpan(timeUnit, unit, toWords);

    /// <inheritdoc/>
    public virtual string TimeSpanHumanize_Age()
    {
        if (Resources.TryGetResource("TimeSpanHumanize_Age", Culture, out var ageFormat))
        {
            return ageFormat;
        }

        return "{0}";
    }

    /// <inheritdoc/>
    public virtual string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true)
    {
        var resourceKey = DataUnitResourceKeys.GetResourceKey(dataUnit, toSymbol);
        var resourceValue = Format(resourceKey);

        if (!toSymbol && count > 1)
        {
            resourceValue += "s";
        }

        return resourceValue;
    }

    /// <inheritdoc/>
    public virtual string TimeUnitHumanize(TimeUnit timeUnit)
    {
        var resourceKey = ResourceKeys.TimeUnitSymbol.GetResourceKey(timeUnit);
        return Format(resourceKey);
    }

    string GetResourceForDate(TimeUnit unit, Tense timeUnitTense, int count)
    {
        // Singular relative-date phrases are often separate resource strings, while plural phrases need the
        // count injected into a format string. Keep the lookup branch split so locales can use either shape.
        var resourceKey = ResourceKeys.DateHumanize.GetResourceKey(unit, timeUnitTense: timeUnitTense, count: count);
        return count == 1 ? Format(resourceKey) : Format(unit, resourceKey, count);
    }

    string GetResourceForTimeSpan(TimeUnit unit, int count, bool toWords = false)
    {
        // Zero-word handling has its own resource key; other counts still follow the singular/plural lookup path.
        var resourceKey = ResourceKeys.TimeSpanHumanize.GetResourceKey(unit, count, toWords);
        return count == 1 ? Format(resourceKey + (toWords ? "_Words" : "")) : Format(unit, resourceKey, count, toWords);
    }

    /// <summary>
    /// Formats the specified resource key.
    /// </summary>
    /// <param name="resourceKey">The canonical resource key.</param>
    /// <returns>The localized resource string.</returns>
    /// <exception cref="ArgumentException">Thrown when the resource is missing for the configured culture.</exception>
    protected virtual string Format(string resourceKey)
    {
        var resolvedKey = GetResourceKey(resourceKey);
        return Resources.GetResource(resolvedKey, Culture);
    }

    /// <summary>
    /// Formats the specified resource key with a numeric value.
    /// </summary>
    /// <param name="unit">The unit being formatted.</param>
    /// <param name="resourceKey">The canonical resource key.</param>
    /// <param name="number">The numeric value to inject into the resource string.</param>
    /// <param name="toWords">Whether the number should be rendered as words.</param>
    /// <returns>The localized formatted string.</returns>
    /// <exception cref="ArgumentException">Thrown when the resource is missing for the configured culture.</exception>
    protected virtual string Format(TimeUnit unit, string resourceKey, int number, bool toWords = false)
    {
        var resolvedKey = GetResourceKey(resourceKey, number);
        var resourceString = Resources.GetResource(resolvedKey, Culture);

        // The resource string decides the grammatical shape; only the injected value changes between numeric and word form.
        return string.Format(resourceString, toWords ? NumberToWords(unit, number, Culture) : number);
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

    /// <summary>
    /// Returns the resource-key suffix for the specified number.
    /// </summary>
    /// <param name="resourceKey">The resource key being formatted.</param>
    /// <param name="number">The number of units being formatted.</param>
    /// <returns>The resource key to use for the number-specific form.</returns>
    /// <remarks>
    /// Override this method for locales that use number-dependent key suffixes.
    /// </remarks>
    protected virtual string GetResourceKey(string resourceKey, int number) =>
        resourceKey;

    /// <summary>
    /// Returns the resource key to use when no number-specific suffix is needed.
    /// </summary>
    /// <param name="resourceKey">The canonical resource key.</param>
    /// <returns>The resource key to use for lookup.</returns>
    protected virtual string GetResourceKey(string resourceKey) =>
        resourceKey;
}
