namespace Humanizer;

/// <summary>
/// Default implementation of IFormatter interface.
/// </summary>
public class DefaultFormatter(CultureInfo culture) : IFormatter
{
    protected CultureInfo Culture { get; } = culture;

    public DefaultFormatter(string localeCode)
        : this(new CultureInfo(localeCode))
    {
    }

    public virtual string DateHumanize_Now() =>
        GetResourceForDate(TimeUnit.Millisecond, Tense.Past, 0);

    public virtual string DateHumanize_Never() =>
        Format(ResourceKeys.DateHumanize.Never);

    /// <summary>
    /// Returns the string representation of the provided DateTime
    /// </summary>
    public virtual string DateHumanize(TimeUnit timeUnit, Tense timeUnitTense, int unit) =>
        GetResourceForDate(timeUnit, timeUnitTense, unit);

    /// <summary>
    /// 0 seconds
    /// </summary>
    /// <returns>Returns 0 seconds as the string representation of Zero TimeSpan</returns>
    public virtual string TimeSpanHumanize_Zero() =>
        GetResourceForTimeSpan(TimeUnit.Millisecond, 0, true);

    /// <summary>
    /// Returns the string representation of the provided TimeSpan
    /// </summary>
    /// <param name="timeUnit">A time unit to represent.</param>
    /// <exception cref="System.ArgumentOutOfRangeException">Is thrown when timeUnit is larger than TimeUnit.Week</exception>
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

    /// <inheritdoc cref="IFormatter.DataUnitHumanize(DataUnit, double, bool)"/>
    public virtual string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true)
    {
        var resourceKey = (toSymbol, dataUnit) switch
        {
            (true, DataUnit.Bit) => "DataUnit_BitSymbol",
            (true, DataUnit.Byte) => "DataUnit_ByteSymbol",
            (true, DataUnit.Kilobyte) => "DataUnit_KilobyteSymbol",
            (true, DataUnit.Megabyte) => "DataUnit_MegabyteSymbol",
            (true, DataUnit.Gigabyte) => "DataUnit_GigabyteSymbol",
            (true, DataUnit.Terabyte) => "DataUnit_TerabyteSymbol",
            (true, _) => $"DataUnit_{dataUnit}Symbol",

            (false, DataUnit.Bit) => "DataUnit_Bit",
            (false, DataUnit.Byte) => "DataUnit_Byte",
            (false, DataUnit.Kilobyte) => "DataUnit_Kilobyte",
            (false, DataUnit.Megabyte) => "DataUnit_Megabyte",
            (false, DataUnit.Gigabyte) => "DataUnit_Gigabyte",
            (false, DataUnit.Terabyte) => "DataUnit_Terabyte",
            (false, _) => $"DataUnit_{dataUnit}",
        };

        var resourceValue = Format(resourceKey);

        if (!toSymbol && count > 1)
        {
            resourceValue += "s";
        }

        return resourceValue;
    }

    /// <inheritdoc />
    public virtual string TimeUnitHumanize(TimeUnit timeUnit)
    {
        var resourceKey = ResourceKeys.TimeUnitSymbol.GetResourceKey(timeUnit);
        return Format(resourceKey);
    }

    string GetResourceForDate(TimeUnit unit, Tense timeUnitTense, int count)
    {
        var resourceKey = ResourceKeys.DateHumanize.GetResourceKey(unit, timeUnitTense: timeUnitTense, count: count);
        return count == 1 ? Format(resourceKey) : Format(unit, resourceKey, count);
    }

    string GetResourceForTimeSpan(TimeUnit unit, int count, bool toWords = false)
    {
        var resourceKey = ResourceKeys.TimeSpanHumanize.GetResourceKey(unit, count, toWords);
        return count == 1 ? Format(resourceKey + (toWords ? "_Words" : "")) : Format(unit, resourceKey, count, toWords);
    }

    /// <summary>
    /// Formats the specified resource key.
    /// </summary>
    /// <param name="resourceKey">The resource key.</param>
    /// <exception cref="ArgumentException">If the resource not exists on the specified culture.</exception>
    protected virtual string Format(string resourceKey)
    {
        var resolvedKey = GetResourceKey(resourceKey);
        return Resources.GetResource(resolvedKey, Culture);
    }

    /// <summary>
    /// Formats the specified resource key.
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="resourceKey">The resource key.</param>
    /// <param name="number">The number.</param>
    /// <param name="toWords"></param>
    /// <exception cref="ArgumentException">If the resource not exists on the specified culture.</exception>
    protected virtual string Format(TimeUnit unit, string resourceKey, int number, bool toWords = false)
    {
        var resolvedKey = GetResourceKey(resourceKey, number);
        var resourceString = Resources.GetResource(resolvedKey, Culture);

        return string.Format(resourceString, toWords ? NumberToWords(unit, number, Culture) : number);
    }

    protected virtual string NumberToWords(TimeUnit unit, int number, CultureInfo culture) =>
        number.ToWords(culture);

    /// <summary>
    /// Override this method if your locale has complex rules around multiple units; e.g. Arabic, Russian
    /// </summary>
    /// <param name="resourceKey">The resource key that's being in formatting</param>
    /// <param name="number">The number of the units being used in formatting</param>
    protected virtual string GetResourceKey(string resourceKey, int number) =>
        resourceKey;

    protected virtual string GetResourceKey(string resourceKey) =>
        resourceKey;
}