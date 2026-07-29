namespace Humanizer;

/// <summary>
/// Humanizes TimeSpan into human readable form
/// </summary>
public static class TimeSpanHumanizeExtensions
{
    const int DaysInAWeek = 7;
    const double DaysInAYear = 365.2425; // see https://en.wikipedia.org/wiki/Gregorian_calendar
    const double DaysInAMonth = DaysInAYear / 12;

    static readonly TimeUnit[] TimeUnits = [.. Enumerable.Reverse(Enum.GetValues<TimeUnit>())];

    /// <summary>
    /// Turns a TimeSpan into a human readable form. E.g. 1 day.
    /// </summary>
    /// <param name="precision">The maximum number of time units to return. Defaulted is 1 which means the largest unit is returned</param>
    /// <param name="culture">Culture to use. If null, current thread's culture is used.</param>
    /// <param name="maxUnit">The maximum unit of time to output. The default value is <see cref="TimeUnit.Week"/>. The time units <see cref="TimeUnit.Month"/> and <see cref="TimeUnit.Year"/> will give approximations for time spans bigger 30 days by calculating with 365.2425 days a year and 30.4369 days a month.</param>
    /// <param name="minUnit">The minimum unit of time to output.</param>
    /// <param name="collectionSeparator">The separator to use when combining humanized time parts. If null, the default collection formatter for the current culture is used.</param>
    /// <param name="toWords">Uses words instead of numbers if true. E.g. one day.</param>
    public static string Humanize(this TimeSpan timeSpan, int precision = 1, CultureInfo? culture = null, TimeUnit maxUnit = TimeUnit.Week, TimeUnit minUnit = TimeUnit.Millisecond, string? collectionSeparator = ", ", bool toWords = false) =>
        Configurator.TimeSpanHumanizeStrategy.Humanize(timeSpan, precision, false, culture, maxUnit, minUnit, collectionSeparator, toWords, false);

    /// <summary>
    /// Turns a <see cref="TimeSpan"/> into a human readable form using localized unit symbols.
    /// </summary>
    /// <param name="timeSpan">The time span to humanize.</param>
    /// <param name="precision">The maximum number of time units to return.</param>
    /// <param name="culture">Culture to use. If null, current thread's culture is used.</param>
    /// <param name="maxUnit">The maximum unit of time to output. The default value is <see cref="TimeUnit.Week"/>.</param>
    /// <param name="minUnit">The minimum unit of time to output.</param>
    /// <param name="collectionSeparator">The separator to use when combining humanized time parts. If null, the default collection formatter for the current culture is used.</param>
    public static string HumanizeToSymbols(
        this TimeSpan timeSpan,
        int precision = 1,
        CultureInfo? culture = null,
        TimeUnit maxUnit = TimeUnit.Week,
        TimeUnit minUnit = TimeUnit.Millisecond,
        string? collectionSeparator = ", ") =>
        Configurator.TimeSpanHumanizeStrategy.Humanize(timeSpan, precision, false, culture, maxUnit, minUnit, collectionSeparator, false, true);

    /// <summary>
    /// Turns a TimeSpan into a human readable form. E.g. 1 day.
    /// </summary>
    /// <param name="precision">The maximum number of time units to return.</param>
    /// <param name="countEmptyUnits">Controls whether empty time units should be counted towards maximum number of time units. Leading empty time units never count.</param>
    /// <param name="culture">Culture to use. If null, current thread's culture is used.</param>
    /// <param name="maxUnit">The maximum unit of time to output. The default value is <see cref="TimeUnit.Week"/>. The time units <see cref="TimeUnit.Month"/> and <see cref="TimeUnit.Year"/> will give approximations for time spans bigger than 30 days by calculating with 365.2425 days a year and 30.4369 days a month.</param>
    /// <param name="minUnit">The minimum unit of time to output.</param>
    /// <param name="collectionSeparator">The separator to use when combining humanized time parts. If null, the default collection formatter for the current culture is used.</param>
    /// <param name="toWords">Uses words instead of numbers if true. E.g. one day.</param>
    public static string Humanize(this TimeSpan timeSpan, int precision, bool countEmptyUnits, CultureInfo? culture = null, TimeUnit maxUnit = TimeUnit.Week, TimeUnit minUnit = TimeUnit.Millisecond, string? collectionSeparator = ", ", bool toWords = false)
        => Configurator.TimeSpanHumanizeStrategy.Humanize(timeSpan, precision, countEmptyUnits, culture, maxUnit, minUnit, collectionSeparator, toWords, false);

    /// <summary>
    /// Turns a <see cref="TimeSpan"/> into a bare duration using locale-authored unit-case phrases.
    /// No preposition is added.
    /// </summary>
    /// <param name="timeSpan">The time span to humanize.</param>
    /// <param name="grammaticalCase">The grammatical case used to select each unit phrase.</param>
    /// <param name="precision">The maximum number of time units to return.</param>
    /// <param name="culture">Culture to use. If null, the current thread's culture is used.</param>
    /// <param name="maxUnit">The maximum unit of time to output.</param>
    /// <param name="minUnit">The minimum unit of time to output.</param>
    /// <param name="collectionSeparator">The separator used to combine time parts. If null, the culture's default collection formatter is used.</param>
    /// <returns>The locale-authored unit-case phrase. The count may be written explicitly or encoded by the unit form.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="grammaticalCase"/> is not defined.</exception>
    /// <exception cref="NotSupportedException">The configured strategy, selected formatter, locale, or duration unit does not support the requested case.</exception>
    public static string HumanizeWithCase(
        this TimeSpan timeSpan,
        GrammaticalCase grammaticalCase,
        int precision = 1,
        CultureInfo? culture = null,
        TimeUnit maxUnit = TimeUnit.Week,
        TimeUnit minUnit = TimeUnit.Millisecond,
        string? collectionSeparator = ", ") =>
        HumanizeWithCase(
            timeSpan,
            grammaticalCase,
            precision,
            false,
            culture,
            maxUnit,
            minUnit,
            collectionSeparator);

    /// <summary>
    /// Turns a <see cref="TimeSpan"/> into a bare duration using locale-authored unit-case phrases.
    /// No preposition is added.
    /// </summary>
    /// <param name="timeSpan">The time span to humanize.</param>
    /// <param name="grammaticalCase">The grammatical case used to select each unit phrase.</param>
    /// <param name="precision">The maximum number of time units to return.</param>
    /// <param name="countEmptyUnits">Whether empty time units count toward <paramref name="precision"/>.</param>
    /// <param name="culture">Culture to use. If null, the current thread's culture is used.</param>
    /// <param name="maxUnit">The maximum unit of time to output.</param>
    /// <param name="minUnit">The minimum unit of time to output.</param>
    /// <param name="collectionSeparator">The separator used to combine time parts. If null, the culture's default collection formatter is used.</param>
    /// <returns>The locale-authored unit-case phrase. The count may be written explicitly or encoded by the unit form.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="grammaticalCase"/> is not defined.</exception>
    /// <exception cref="NotSupportedException">The configured strategy, selected formatter, locale, or duration unit does not support the requested case.</exception>
    public static string HumanizeWithCase(
        this TimeSpan timeSpan,
        GrammaticalCase grammaticalCase,
        int precision,
        bool countEmptyUnits,
        CultureInfo? culture = null,
        TimeUnit maxUnit = TimeUnit.Week,
        TimeUnit minUnit = TimeUnit.Millisecond,
        string? collectionSeparator = ", ")
    {
        ValidateGrammaticalCase(grammaticalCase);

        if (Configurator.TimeSpanHumanizeStrategy is not IGrammaticalCaseTimeSpanHumanizeStrategy strategy)
        {
            throw new NotSupportedException(
                $"The configured {nameof(ITimeSpanHumanizeStrategy)} does not support grammatical-case-aware durations.");
        }

        return strategy.Humanize(
            timeSpan,
            precision,
            countEmptyUnits,
            culture,
            maxUnit,
            minUnit,
            collectionSeparator,
            false,
            grammaticalCase);
    }

    /// <summary>
    /// Turns a <see cref="TimeSpan"/> into a human readable form using localized unit symbols.
    /// </summary>
    /// <param name="timeSpan">The time span to humanize.</param>
    /// <param name="precision">The maximum number of time units to return.</param>
    /// <param name="countEmptyUnits">Controls whether empty time units should be counted towards the maximum number of time units. Leading empty time units never count.</param>
    /// <param name="culture">Culture to use. If null, current thread's culture is used.</param>
    /// <param name="maxUnit">The maximum unit of time to output. The default value is <see cref="TimeUnit.Week"/>.</param>
    /// <param name="minUnit">The minimum unit of time to output.</param>
    /// <param name="collectionSeparator">The separator to use when combining humanized time parts. If null, the default collection formatter for the current culture is used.</param>
    public static string HumanizeToSymbols(
        this TimeSpan timeSpan,
        int precision,
        bool countEmptyUnits,
        CultureInfo? culture = null,
        TimeUnit maxUnit = TimeUnit.Week,
        TimeUnit minUnit = TimeUnit.Millisecond,
        string? collectionSeparator = ", ") =>
        Configurator.TimeSpanHumanizeStrategy.Humanize(timeSpan, precision, countEmptyUnits, culture, maxUnit, minUnit, collectionSeparator, false, true);

    /// <summary>
    /// Turns a <see cref="TimeSpan"/> into a human-readable form with fractional seconds.
    /// </summary>
    /// <param name="timeSpan">The time span to humanize.</param>
    /// <param name="precision">The maximum number of time units to return.</param>
    /// <param name="maxFractionalDigits">The maximum number of fractional-second digits, from 0 through 7.</param>
    /// <param name="roundingMode">The midpoint rounding mode. Only <see cref="MidpointRounding.ToEven"/> and <see cref="MidpointRounding.AwayFromZero"/> are supported.</param>
    /// <param name="culture">Culture to use. If null, the current culture is used.</param>
    /// <param name="maxUnit">The maximum unit of time to output.</param>
    /// <param name="collectionSeparator">The separator used to combine time parts. If null, the culture's default collection formatter is used.</param>
    /// <returns>The human-readable time span, using seconds as its minimum unit.</returns>
    public static string HumanizeWithFractionalSeconds(
        this TimeSpan timeSpan,
        int precision,
        int maxFractionalDigits,
        MidpointRounding roundingMode,
        CultureInfo? culture,
        TimeUnit maxUnit,
        string? collectionSeparator = ", ") =>
        HumanizeWithFractionalSeconds(
            timeSpan,
            precision,
            false,
            maxFractionalDigits,
            roundingMode,
            culture,
            maxUnit,
            collectionSeparator);

    /// <summary>
    /// Turns a <see cref="TimeSpan"/> into a human-readable form with fractional seconds.
    /// </summary>
    /// <param name="timeSpan">The time span to humanize.</param>
    /// <param name="precision">The maximum number of time units to return.</param>
    /// <param name="countEmptyUnits">Whether empty time units count toward <paramref name="precision"/>.</param>
    /// <param name="maxFractionalDigits">The maximum number of fractional-second digits, from 0 through 7.</param>
    /// <param name="roundingMode">The midpoint rounding mode. Only <see cref="MidpointRounding.ToEven"/> and <see cref="MidpointRounding.AwayFromZero"/> are supported.</param>
    /// <param name="culture">Culture to use. If null, the current culture is used.</param>
    /// <param name="maxUnit">The maximum unit of time to output.</param>
    /// <param name="collectionSeparator">The separator used to combine time parts. If null, the culture's default collection formatter is used.</param>
    /// <returns>The human-readable time span, using seconds as its minimum unit.</returns>
    public static string HumanizeWithFractionalSeconds(
        this TimeSpan timeSpan,
        int precision,
        bool countEmptyUnits,
        int maxFractionalDigits,
        MidpointRounding roundingMode,
        CultureInfo? culture,
        TimeUnit maxUnit,
        string? collectionSeparator = ", ") =>
        HumanizeWithFractionalSecondsCore(
            timeSpan,
            precision,
            countEmptyUnits,
            maxFractionalDigits,
            roundingMode,
            culture,
            maxUnit,
            collectionSeparator,
            false);

    /// <summary>
    /// Turns a <see cref="TimeSpan"/> into a human-readable form using localized unit symbols and fractional seconds.
    /// </summary>
    /// <param name="timeSpan">The time span to humanize.</param>
    /// <param name="precision">The maximum number of time units to return.</param>
    /// <param name="maxFractionalDigits">The maximum number of fractional-second digits, from 0 through 7.</param>
    /// <param name="roundingMode">The midpoint rounding mode. Only <see cref="MidpointRounding.ToEven"/> and <see cref="MidpointRounding.AwayFromZero"/> are supported.</param>
    /// <param name="culture">Culture to use. If null, the current culture is used.</param>
    /// <param name="maxUnit">The maximum unit of time to output.</param>
    /// <param name="collectionSeparator">The separator used to combine time parts. If null, the culture's default collection formatter is used.</param>
    /// <returns>The human-readable time span, using seconds as its minimum unit.</returns>
    public static string HumanizeToSymbolsWithFractionalSeconds(
        this TimeSpan timeSpan,
        int precision,
        int maxFractionalDigits,
        MidpointRounding roundingMode,
        CultureInfo? culture,
        TimeUnit maxUnit,
        string? collectionSeparator = ", ") =>
        HumanizeToSymbolsWithFractionalSeconds(
            timeSpan,
            precision,
            false,
            maxFractionalDigits,
            roundingMode,
            culture,
            maxUnit,
            collectionSeparator);

    /// <summary>
    /// Turns a <see cref="TimeSpan"/> into a human-readable form using localized unit symbols and fractional seconds.
    /// </summary>
    /// <param name="timeSpan">The time span to humanize.</param>
    /// <param name="precision">The maximum number of time units to return.</param>
    /// <param name="countEmptyUnits">Whether empty time units count toward <paramref name="precision"/>.</param>
    /// <param name="maxFractionalDigits">The maximum number of fractional-second digits, from 0 through 7.</param>
    /// <param name="roundingMode">The midpoint rounding mode. Only <see cref="MidpointRounding.ToEven"/> and <see cref="MidpointRounding.AwayFromZero"/> are supported.</param>
    /// <param name="culture">Culture to use. If null, the current culture is used.</param>
    /// <param name="maxUnit">The maximum unit of time to output.</param>
    /// <param name="collectionSeparator">The separator used to combine time parts. If null, the culture's default collection formatter is used.</param>
    /// <returns>The human-readable time span, using seconds as its minimum unit.</returns>
    public static string HumanizeToSymbolsWithFractionalSeconds(
        this TimeSpan timeSpan,
        int precision,
        bool countEmptyUnits,
        int maxFractionalDigits,
        MidpointRounding roundingMode,
        CultureInfo? culture,
        TimeUnit maxUnit,
        string? collectionSeparator = ", ") =>
        HumanizeWithFractionalSecondsCore(
            timeSpan,
            precision,
            countEmptyUnits,
            maxFractionalDigits,
            roundingMode,
            culture,
            maxUnit,
            collectionSeparator,
            true);

    static string HumanizeWithFractionalSecondsCore(
        TimeSpan timeSpan,
        int precision,
        bool countEmptyUnits,
        int maxFractionalDigits,
        MidpointRounding roundingMode,
        CultureInfo? culture,
        TimeUnit maxUnit,
        string? collectionSeparator,
        bool toSymbols)
    {
        ValidateFractionalSecondArguments(maxFractionalDigits, roundingMode, maxUnit);

        var strategy = Configurator.TimeSpanHumanizeStrategy;
        if (strategy.GetType() == typeof(DefaultTimeSpanHumanizeStrategy))
        {
            return ((DefaultTimeSpanHumanizeStrategy)strategy).HumanizeWithFractionalSeconds(
                timeSpan,
                precision,
                countEmptyUnits,
                culture,
                maxUnit,
                collectionSeparator,
                maxFractionalDigits,
                roundingMode,
                toSymbols);
        }

        if (strategy is IFractionalTimeSpanHumanizeStrategy fractionalStrategy)
        {
            return fractionalStrategy.HumanizeWithFractionalSeconds(
                timeSpan,
                precision,
                countEmptyUnits,
                culture,
                maxUnit,
                collectionSeparator,
                maxFractionalDigits,
                roundingMode,
                toSymbols);
        }

        var roundedTimeSpan = RoundToFractionalSecondPrecision(timeSpan, maxFractionalDigits, roundingMode);
        if (HasVisibleFractionalSeconds(roundedTimeSpan, precision, countEmptyUnits, maxUnit))
        {
            throw new InvalidOperationException(
                $"The configured {nameof(ITimeSpanHumanizeStrategy)} does not support fractional seconds. " +
                $"Implement {nameof(IFractionalTimeSpanHumanizeStrategy)} to handle genuinely fractional results.");
        }

        return strategy.Humanize(
            roundedTimeSpan.ToTimeSpan(),
            precision,
            countEmptyUnits,
            culture,
            maxUnit,
            TimeUnit.Second,
            collectionSeparator,
            false,
            toSymbols);
    }

    internal static void ValidateFractionalSecondArguments(
        int maxFractionalDigits,
        MidpointRounding roundingMode,
        TimeUnit maxUnit)
    {
        if (maxFractionalDigits is < 0 or > 7)
        {
            throw new ArgumentOutOfRangeException(nameof(maxFractionalDigits));
        }

        if (roundingMode is not (MidpointRounding.ToEven or MidpointRounding.AwayFromZero))
        {
            throw new ArgumentOutOfRangeException(
                nameof(roundingMode),
                roundingMode,
                "Fractional seconds support only midpoint rounding ToEven and AwayFromZero.");
        }

        if (maxUnit is < TimeUnit.Second or > TimeUnit.Year)
        {
            throw new ArgumentOutOfRangeException(nameof(maxUnit));
        }
    }

    static RoundedTimeSpan RoundToFractionalSecondPrecision(
        TimeSpan timeSpan,
        int maxFractionalDigits,
        MidpointRounding roundingMode)
    {
        var magnitudeTicks = Math.Abs((decimal)timeSpan.Ticks);
        var quantumTicks = DecimalPowersOfTen[7 - maxFractionalDigits];
        var roundedMagnitudeTicks = decimal.Round(magnitudeTicks / quantumTicks, 0, roundingMode) * quantumTicks;
        return new(roundedMagnitudeTicks, timeSpan.Ticks < 0);
    }

    static bool HasVisibleFractionalSeconds(
        RoundedTimeSpan roundedTimeSpan,
        int precision,
        bool countEmptyUnits,
        TimeUnit maxUnit) =>
        CreateFractionalSecondParts(roundedTimeSpan, precision, countEmptyUnits, maxUnit)
            .Any(static part => part.Unit == TimeUnit.Second && decimal.Truncate(part.Value) != part.Value);

    internal static string DefaultHumanizeWithFractionalSeconds(
        TimeSpan timeSpan,
        int precision,
        bool countEmptyUnits,
        CultureInfo? culture,
        TimeUnit maxUnit,
        string? collectionSeparator,
        int maxFractionalDigits,
        MidpointRounding roundingMode,
        bool toSymbols)
    {
        ValidateFractionalSecondArguments(maxFractionalDigits, roundingMode, maxUnit);

        var roundedTimeSpan = RoundToFractionalSecondPrecision(timeSpan, maxFractionalDigits, roundingMode);
        var parts = CreateFractionalSecondParts(roundedTimeSpan, precision, countEmptyUnits, maxUnit);
        var formatter = Configurator.GetFormatter(culture);
        var formattedParts = new List<string>(parts.Count);
        foreach (var part in parts)
        {
            if (part.Unit != TimeUnit.Second)
            {
                formattedParts.Add(FormatTimePart(
                    formatter,
                    part.Unit,
                    decimal.ToInt32(part.Value),
                    culture,
                    false,
                    toSymbols,
                    null));
                continue;
            }

            if (IsBuiltInFractionalTimeSpanFormatter(formatter))
            {
                ((DefaultFormatter)formatter).ValidateFractionalSecondGrammar(part.Value, toSymbols);
            }

            if (decimal.Truncate(part.Value) == part.Value &&
                part.Value is >= int.MinValue and <= int.MaxValue)
            {
                formattedParts.Add(FormatTimePart(
                    formatter,
                    TimeUnit.Second,
                    SaturateToInt(part.Value),
                    culture,
                    false,
                    toSymbols,
                    null));
                continue;
            }

            if (formatter.GetType() == typeof(DefaultFormatter))
            {
                formattedParts.Add(((DefaultFormatter)formatter).TimeSpanHumanizeWithFractionalSeconds(part.Value, toSymbols));
                continue;
            }

            if (formatter is not IFractionalTimeSpanFormatter fractionalFormatter)
            {
                throw new InvalidOperationException(
                    $"The configured {nameof(IFormatter)} for '{culture?.Name ?? CultureInfo.CurrentCulture.Name}' " +
                    $"does not support fractional seconds. Implement {nameof(IFractionalTimeSpanFormatter)}.");
            }

            formattedParts.Add(fractionalFormatter.TimeSpanHumanizeWithFractionalSeconds(part.Value, toSymbols));
        }

        return ConcatenateTimeSpanParts(formattedParts, culture, collectionSeparator);
    }

    static bool IsBuiltInFractionalTimeSpanFormatter(IFormatter formatter) =>
        formatter.GetType() == typeof(DefaultFormatter) || formatter is ProfiledFormatter;
    static List<FractionalSecondPart> CreateFractionalSecondParts(
        RoundedTimeSpan roundedTimeSpan,
        int precision,
        bool countEmptyUnits,
        TimeUnit maxUnit)
    {
        if (precision <= 0)
        {
            return [];
        }

        var parts = new List<FractionalSecondPart>(Math.Min(precision, TimeUnits.Length));
        var firstValueFound = false;
        var countedUnits = 0;

        foreach (var timeUnit in TimeUnits)
        {
            if (timeUnit > maxUnit || timeUnit < TimeUnit.Second)
            {
                continue;
            }

            var value = timeUnit == TimeUnit.Second
                ? GetFractionalSeconds(roundedTimeSpan, maxUnit)
                : GetRoundedTimeUnitNumericalValue(timeUnit, roundedTimeSpan.MagnitudeTicks, maxUnit);
            if (value == 0 && !firstValueFound)
            {
                continue;
            }

            firstValueFound = true;
            if (countEmptyUnits)
            {
                countedUnits++;
                if (value != 0)
                {
                    parts.Add(new(timeUnit, value));
                }

                if (countedUnits >= precision)
                {
                    return parts;
                }
            }
            else if (value != 0)
            {
                parts.Add(new(timeUnit, value));
                if (parts.Count >= precision)
                {
                    return parts;
                }
            }
        }

        if (parts.Count == 0)
        {
            parts.Add(new(TimeUnit.Second, 0));
        }

        return parts;
    }

    static decimal GetFractionalSeconds(RoundedTimeSpan timeSpan, TimeUnit maxUnit) =>
        maxUnit == TimeUnit.Second
            ? timeSpan.MagnitudeTicks / TimeSpan.TicksPerSecond
            : timeSpan.MagnitudeTicks % TimeSpan.TicksPerMinute / TimeSpan.TicksPerSecond;

    static decimal GetRoundedTimeUnitNumericalValue(
        TimeUnit timeUnit,
        decimal magnitudeTicks,
        TimeUnit maximumTimeUnit)
    {
        var wholeDays = decimal.ToInt32(decimal.Truncate(magnitudeTicks / TimeSpan.TicksPerDay));
        var isMaximum = timeUnit == maximumTimeUnit;

        return timeUnit switch
        {
            TimeUnit.Minute => isMaximum
                ? SaturateToInt(magnitudeTicks / TimeSpan.TicksPerMinute)
                : decimal.Truncate(magnitudeTicks / TimeSpan.TicksPerMinute) % 60,
            TimeUnit.Hour => isMaximum
                ? SaturateToInt(magnitudeTicks / TimeSpan.TicksPerHour)
                : decimal.Truncate(magnitudeTicks / TimeSpan.TicksPerHour) % 24,
            TimeUnit.Day => maximumTimeUnit switch
            {
                TimeUnit.Day => wholeDays,
                TimeUnit.Week => wholeDays % DaysInAWeek,
                _ => (int)(wholeDays % DaysInAMonth) % DaysInAWeek
            },
            TimeUnit.Week => maximumTimeUnit == TimeUnit.Week
                ? wholeDays / DaysInAWeek
                : (int)(wholeDays % DaysInAMonth) / DaysInAWeek,
            TimeUnit.Month => isMaximum
                ? (int)(wholeDays / DaysInAMonth)
                : (int)((wholeDays % DaysInAYear) / DaysInAMonth),
            TimeUnit.Year => (int)(wholeDays / DaysInAYear),
            _ => 0
        };
    }

    static int SaturateToInt(decimal value) =>
        value >= int.MaxValue ? int.MaxValue : decimal.ToInt32(decimal.Truncate(value));

    static readonly decimal[] DecimalPowersOfTen =
    [
        1m,
        10m,
        100m,
        1000m,
        10000m,
        100000m,
        1000000m,
        10000000m
    ];

    readonly record struct FractionalSecondPart(TimeUnit Unit, decimal Value);

    readonly record struct RoundedTimeSpan(decimal MagnitudeTicks, bool IsNegative)
    {
        public TimeSpan ToTimeSpan()
        {
            var ticks = IsNegative ? -MagnitudeTicks : MagnitudeTicks;
            if (ticks >= long.MaxValue)
            {
                return TimeSpan.MaxValue;
            }

            if (ticks <= long.MinValue)
            {
                return TimeSpan.MinValue;
            }

            return TimeSpan.FromTicks(decimal.ToInt64(ticks));
        }
    }

    internal static string DefaultHumanize(
        TimeSpan timeSpan,
        int precision,
        bool countEmptyUnits,
        CultureInfo? culture,
        TimeUnit maxUnit,
        TimeUnit minUnit,
        string? collectionSeparator,
        bool toWords,
        bool toSymbols)
        => DefaultHumanizeCore(
            timeSpan,
            precision,
            countEmptyUnits,
            culture,
            maxUnit,
            minUnit,
            collectionSeparator,
            toWords,
            toSymbols,
            null);

    internal static string DefaultHumanizeWithCase(
        TimeSpan timeSpan,
        int precision,
        bool countEmptyUnits,
        CultureInfo? culture,
        TimeUnit maxUnit,
        TimeUnit minUnit,
        string? collectionSeparator,
        bool toSymbols,
        GrammaticalCase grammaticalCase)
        => DefaultHumanizeCore(
            timeSpan,
            precision,
            countEmptyUnits,
            culture,
            maxUnit,
            minUnit,
            collectionSeparator,
            false,
            toSymbols,
            grammaticalCase);

    static string DefaultHumanizeCore(
        TimeSpan timeSpan,
        int precision,
        bool countEmptyUnits,
        CultureInfo? culture,
        TimeUnit maxUnit,
        TimeUnit minUnit,
        string? collectionSeparator,
        bool toWords,
        bool toSymbols,
        GrammaticalCase? grammaticalCase)
    {
        if (grammaticalCase is { } requestedCase)
        {
            ValidateGrammaticalCase(requestedCase);
        }

        if (toSymbols && grammaticalCase is not null)
        {
            throw new NotSupportedException("Grammatical case is not supported for time-unit symbols.");
        }

        if (precision == 1 && !countEmptyUnits)
        {
            return HumanizeSinglePart(timeSpan, culture, maxUnit, minUnit, toWords, toSymbols, grammaticalCase);
        }

        var timeParts = CreatePrecisionLimitedTimeParts(
            timeSpan,
            culture,
            maxUnit,
            minUnit,
            precision,
            countEmptyUnits,
            toWords,
            toSymbols,
            grammaticalCase);

        return ConcatenateTimeSpanParts(timeParts, culture, collectionSeparator);
    }

    /// <summary>
    /// Turns a TimeSpan into an age expression, e.g. "40 years old"
    /// </summary>
    /// <param name="timeSpan">Elapsed time</param>
    /// <param name="culture">Culture to use. If null, current thread's culture is used.</param>
    /// <param name="maxUnit">The maximum unit of time to output. The default value is <see cref="TimeUnit.Year"/>.</param>
    /// <param name="toWords">Uses words instead of numbers if true. E.g. "forty years old".</param>
    /// <returns>Age expression in the given culture/language</returns>
    public static string ToAge(this TimeSpan timeSpan, CultureInfo? culture = null, TimeUnit maxUnit = TimeUnit.Year, bool toWords = false)
    {
        var timeSpanExpression = timeSpan.Humanize(culture: culture, maxUnit: maxUnit, toWords: toWords);

        var cultureFormatter = Configurator.GetFormatter(culture);
        var ageFormat = cultureFormatter.TimeSpanHumanize_Age();

        return FormatAge(timeSpanExpression, ageFormat);
    }

    internal static string FormatAge(string value, string ageFormat)
    {
        if (ageFormat == "{0} old" || ageFormat == "{value} old")
        {
            return string.Concat(value, " old");
        }

        if (ageFormat.Contains("{value}", StringComparison.Ordinal))
        {
            return ageFormat.Replace("{value}", value);
        }

        return string.Format(ageFormat, value);
    }

    static List<string> CreatePrecisionLimitedTimeParts(
        TimeSpan timespan,
        CultureInfo? culture,
        TimeUnit maxUnit,
        TimeUnit minUnit,
        int precision,
        bool countEmptyUnits,
        bool toWords,
        bool toSymbols,
        GrammaticalCase? grammaticalCase)
    {
        if (precision <= 0)
        {
            return [];
        }

        var cultureFormatter = Configurator.GetFormatter(culture);
        var firstValueFound = false;
        var countedUnits = 0;
        var timeParts = new List<string>(Math.Min(precision, TimeUnits.Length));

        foreach (var timeUnit in TimeUnits)
        {
            var timePart = GetTimeUnitPart(
                timeUnit,
                timespan,
                maxUnit,
                minUnit,
                cultureFormatter,
                culture,
                toWords,
                toSymbols,
                grammaticalCase);

            if (timePart == null && !firstValueFound)
            {
                continue;
            }

            firstValueFound = true;
            if (countEmptyUnits)
            {
                countedUnits++;
                if (timePart != null)
                {
                    timeParts.Add(timePart);
                }

                if (countedUnits >= precision)
                {
                    return timeParts;
                }
            }
            else if (timePart != null)
            {
                timeParts.Add(timePart);
                if (timeParts.Count >= precision)
                {
                    return timeParts;
                }
            }
        }

        if (timeParts.Count == 0)
        {
            var noTimeValueCultureFormatted = grammaticalCase is null && toWords
                    ? cultureFormatter.TimeSpanHumanize_Zero()
                    : FormatTimePart(
                        cultureFormatter,
                        minUnit,
                        0,
                        culture,
                        toWords,
                        toSymbols,
                        grammaticalCase);
            timeParts.Add(noTimeValueCultureFormatted);
        }

        return timeParts;
    }

    static string? GetTimeUnitPart(
        TimeUnit timeUnitToGet,
        TimeSpan timespan,
        TimeUnit maximumTimeUnit,
        TimeUnit minimumTimeUnit,
        IFormatter cultureFormatter,
        CultureInfo? culture,
        bool toWords,
        bool toSymbols,
        GrammaticalCase? grammaticalCase)
    {
        if (timeUnitToGet <= maximumTimeUnit && timeUnitToGet >= minimumTimeUnit)
        {
            var numberOfTimeUnits = GetTimeUnitNumericalValue(timeUnitToGet, timespan, maximumTimeUnit);
            return BuildFormatTimePart(
                cultureFormatter,
                timeUnitToGet,
                numberOfTimeUnits,
                culture,
                toWords,
                toSymbols,
                grammaticalCase);
        }

        return null;
    }

    static int GetTimeUnitNumericalValue(TimeUnit timeUnitToGet, TimeSpan timespan, TimeUnit maximumTimeUnit)
    {
        var isTimeUnitToGetTheMaximumTimeUnit = timeUnitToGet == maximumTimeUnit;
        return timeUnitToGet switch
        {
            TimeUnit.Millisecond => GetNormalCaseTimeAsInteger(timespan.Milliseconds, timespan.TotalMilliseconds, isTimeUnitToGetTheMaximumTimeUnit),
            TimeUnit.Second => GetNormalCaseTimeAsInteger(timespan.Seconds, timespan.TotalSeconds, isTimeUnitToGetTheMaximumTimeUnit),
            TimeUnit.Minute => GetNormalCaseTimeAsInteger(timespan.Minutes, timespan.TotalMinutes, isTimeUnitToGetTheMaximumTimeUnit),
            TimeUnit.Hour => GetNormalCaseTimeAsInteger(timespan.Hours, timespan.TotalHours, isTimeUnitToGetTheMaximumTimeUnit),
            TimeUnit.Day => GetSpecialCaseDaysAsInteger(timespan, maximumTimeUnit),
            TimeUnit.Week => GetSpecialCaseWeeksAsInteger(timespan, maximumTimeUnit),
            TimeUnit.Month => GetSpecialCaseMonthAsInteger(timespan, isTimeUnitToGetTheMaximumTimeUnit),
            TimeUnit.Year => GetSpecialCaseYearAsInteger(timespan),
            _ => 0
        };
    }

    static int GetSpecialCaseMonthAsInteger(TimeSpan timespan, bool isTimeUnitToGetTheMaximumTimeUnit)
    {
        if (isTimeUnitToGetTheMaximumTimeUnit)
        {
            return (int)(timespan.Days / DaysInAMonth);
        }

        var remainingDays = timespan.Days % DaysInAYear;
        return (int)(remainingDays / DaysInAMonth);
    }

    static int GetSpecialCaseYearAsInteger(TimeSpan timespan) =>
        (int)(timespan.Days / DaysInAYear);

    static int GetSpecialCaseWeeksAsInteger(TimeSpan timespan, TimeUnit maximumTimeUnit)
    {
        if (maximumTimeUnit == TimeUnit.Week)
        {
            return timespan.Days / DaysInAWeek;
        }

        return (int)(timespan.Days % DaysInAMonth) / DaysInAWeek;
    }

    static int GetSpecialCaseDaysAsInteger(TimeSpan timespan, TimeUnit maximumTimeUnit)
    {
        if (maximumTimeUnit == TimeUnit.Day)
        {
            return timespan.Days;
        }

        if (maximumTimeUnit == TimeUnit.Week)
        {
            var remainingDays = timespan.Days % DaysInAWeek;
            return remainingDays;
        }

        return (int)(timespan.Days % DaysInAMonth) % DaysInAWeek;
    }

    static int GetNormalCaseTimeAsInteger(int timeNumberOfUnits, double totalTimeNumberOfUnits, bool isTimeUnitToGetTheMaximumTimeUnit)
    {
        if (isTimeUnitToGetTheMaximumTimeUnit)
        {
            return (int)Math.Max(-int.MaxValue, Math.Min(totalTimeNumberOfUnits, int.MaxValue));
        }

        return timeNumberOfUnits;
    }

    static string? BuildFormatTimePart(
        IFormatter cultureFormatter,
        TimeUnit timeUnitType,
        int amountOfTimeUnits,
        CultureInfo? culture,
        bool toWords,
        bool toSymbols,
        GrammaticalCase? grammaticalCase) =>
        // Always use positive units to account for negative timespans
        amountOfTimeUnits != 0
            ? FormatTimePart(
                cultureFormatter,
                timeUnitType,
                Math.Abs(amountOfTimeUnits),
                culture,
                toWords,
                toSymbols,
                grammaticalCase)
            : null;

    internal static string FormatTimePart(
        IFormatter cultureFormatter,
        TimeUnit timeUnit,
        int amount,
        CultureInfo? culture,
        bool toWords,
        bool toSymbols,
        GrammaticalCase? grammaticalCase) =>
        toSymbols
            ? string.Concat(amount.ToString(culture ?? CultureInfo.CurrentCulture), cultureFormatter.TimeUnitHumanize(timeUnit))
            : grammaticalCase is null
                ? cultureFormatter.TimeSpanHumanize(timeUnit, amount, toWords)
                : cultureFormatter is IGrammaticalCaseTimeSpanFormatter caseFormatter
                    ? caseFormatter.TimeSpanHumanize(timeUnit, amount, grammaticalCase.Value)
                    : throw new NotSupportedException(
                        $"The formatter for '{culture?.Name ?? CultureInfo.CurrentCulture.Name}' does not support grammatical-case-aware durations.");

    static string HumanizeSinglePart(
        TimeSpan timeSpan,
        CultureInfo? culture,
        TimeUnit maxUnit,
        TimeUnit minUnit,
        bool toWords,
        bool toSymbols,
        GrammaticalCase? grammaticalCase)
    {
        var cultureFormatter = Configurator.GetFormatter(culture);
        foreach (var timeUnit in TimeUnits)
        {
            var timePart = GetTimeUnitPart(
                timeUnit,
                timeSpan,
                maxUnit,
                minUnit,
                cultureFormatter,
                culture,
                toWords,
                toSymbols,
                grammaticalCase);
            if (timePart is not null)
            {
                return timePart;
            }
        }

        return grammaticalCase is null && toWords
                ? cultureFormatter.TimeSpanHumanize_Zero()
                : FormatTimePart(
                    cultureFormatter,
                    minUnit,
                    0,
                    culture,
                    toWords,
                    toSymbols,
                    grammaticalCase);
    }

    static void ValidateGrammaticalCase(GrammaticalCase grammaticalCase)
    {
        if ((uint)grammaticalCase > (uint)GrammaticalCase.Causal)
        {
            throw new ArgumentOutOfRangeException(nameof(grammaticalCase), grammaticalCase, "Unsupported grammatical case.");
        }
    }

    static string ConcatenateTimeSpanParts(List<string> timeSpanParts, CultureInfo? culture, string? collectionSeparator)
    {
        if (collectionSeparator == null)
        {
            return Configurator
                .CollectionFormatters.ResolveForCulture(culture)
                .Humanize(timeSpanParts);
        }

        return string.Join(collectionSeparator, timeSpanParts);
    }
}