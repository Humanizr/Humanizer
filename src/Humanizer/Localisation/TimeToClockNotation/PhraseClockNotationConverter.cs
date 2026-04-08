#if NET6_0_OR_GREATER

namespace Humanizer;

/// <summary>
/// Provides clock notation using a unified phrase-based profile that covers all locale patterns
/// through YAML-driven configuration: bucket phrases, hour modes, day periods, range templates,
/// hour/minute suffixes, article selection, and Eifeler rule post-processing.
/// </summary>
class PhraseClockNotationConverter(PhraseClockNotationProfile profile) : ITimeOnlyToClockNotationConverter
{
    readonly PhraseClockNotationProfile profile = profile;

    /// <summary>
    /// Converts the given time using the phrase-clock profile.
    /// </summary>
    /// <returns>The localized clock-notation string.</returns>
    public string Convert(TimeOnly time, ClockNotationRounding roundToNearestFive)
    {
        var normalizedMinutes = (int)(roundToNearestFive == ClockNotationRounding.NearestFiveMinutes
            ? 5 * Math.Round(time.Minute / 5.0)
            : time.Minute);

        // When rounding pushes minutes to 60, advance to the next hour so the bucket switch
        // does not need a second normalization pass.
        int hour;
        if (normalizedMinutes == 60)
        {
            hour = time.Hour + 1;
            normalizedMinutes = 0;
        }
        else
        {
            hour = time.Hour;
        }

        // Midnight and noon are fixed phrases that bypass all template machinery.
        if (hour % 24 == 0 && normalizedMinutes == 0 && profile.Midnight.Length > 0)
        {
            return profile.Midnight;
        }

        if (hour % 24 == 12 && normalizedMinutes == 0 && profile.Midday.Length > 0)
        {
            return profile.Midday;
        }

        var hourWords = ResolveHourExpression(hour);
        var nextHourWords = ResolveHourExpression(hour + 1);
        var rawMinuteWords = ResolveMinuteWords(normalizedMinutes);

        // When a zero-filler is configured and minutes are 1-9, prepend the filler word
        // so templates like "{hour} {minutes}" produce "et nul fem" instead of "et fem".
        var minuteWords = normalizedMinutes is > 0 and < 10 && profile.ZeroFiller.Length > 0
            ? profile.ZeroFiller + " " + rawMinuteWords
            : rawMinuteWords;

        var reverseMinuteWords = normalizedMinutes > 0 ? ResolveMinuteWords(60 - normalizedMinutes) : "";

        string halfMinuteWords;
        if (normalizedMinutes < 30)
        {
            halfMinuteWords = ResolveMinuteWords(30 - normalizedMinutes);
        }
        else if (normalizedMinutes > 30)
        {
            halfMinuteWords = ResolveMinuteWords(normalizedMinutes - 30);
        }
        else
        {
            halfMinuteWords = "";
        }

        // Resolve the article for locales that have singular/plural articles (ca, es).
        var article = ResolveArticle(hour);
        var nextArticle = ResolveArticle(hour + 1);

        // Resolve minute suffix for range templates (Lb "Minutt"/"Minutten").
        var minuteSuffix = ResolveMinuteSuffix(normalizedMinutes);

        // Check minute-bucket template first (exact 5-minute intervals).
        var template = GetBucketTemplate(normalizedMinutes);
        if (template.Length > 0)
        {
            var result = ExpandTemplate(template, hourWords, nextHourWords, minuteWords, reverseMinuteWords, halfMinuteWords, article, nextArticle, minuteSuffix);
            return ApplyDayPeriod(result, hour, normalizedMinutes);
        }

        // Try range-based templates for non-bucketed minutes.
        var rangeTemplate = GetRangeTemplate(normalizedMinutes);
        if (rangeTemplate.Length > 0)
        {
            var result = ExpandTemplate(rangeTemplate, hourWords, nextHourWords, minuteWords, reverseMinuteWords, halfMinuteWords, article, nextArticle, minuteSuffix);
            return ApplyDayPeriod(result, hour, normalizedMinutes);
        }

        // Fall to default template.
        if (profile.DefaultTemplate.Length > 0)
        {
            var result = ExpandTemplate(profile.DefaultTemplate, hourWords, nextHourWords, minuteWords, reverseMinuteWords, halfMinuteWords, article, nextArticle, minuteSuffix);
            return ApplyDayPeriod(result, hour, normalizedMinutes);
        }

        // Absolute fallback: "{hour} {minutes}".
        var fallback = minuteWords.Length > 0 ? hourWords + " " + minuteWords : hourWords;
        return ApplyDayPeriod(fallback, hour, normalizedMinutes);
    }

    int ResolveHourValue(int hour)
    {
        return profile.HourMode switch
        {
            PhraseClockHourMode.H12 => hour % 12 != 0 ? hour % 12 : 12,
            PhraseClockHourMode.H24 => hour % 24,
            PhraseClockHourMode.Numeric => hour % 24,
            _ => hour % 12
        };
    }

    string ResolveHourExpression(int rawHour)
    {
        var hourValue = ResolveHourValue(rawHour);

        // Check for fixed hour-word overrides (e.g., French "minuit" for 0, "midi" for 12,
        // Norwegian "ett" for 1 when the number engine produces a different form).
        if (hourValue == 0 && profile.HourZeroWord.Length > 0)
        {
            return profile.HourZeroWord;
        }

        if (hourValue == 1 && profile.HourOneWord.Length > 0)
        {
            return profile.HourOneWord;
        }

        if (hourValue == 12 && profile.HourTwelveWord.Length > 0)
        {
            return profile.HourTwelveWord;
        }

        string baseWord;
        if (profile.HourMode == PhraseClockHourMode.Numeric)
        {
            baseWord = hourValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
        else
        {
            baseWord = profile.HourGender switch
            {
                GrammaticalGender.Feminine => hourValue.ToWords(GrammaticalGender.Feminine),
                GrammaticalGender.Neuter => hourValue.ToWords(GrammaticalGender.Neuter),
                _ => hourValue.ToWords()
            };
        }

        // Apply hour suffix (singular/plural variants for locales like French).
        if (profile.HourSuffixSingular.Length > 0 || profile.HourSuffixPlural.Length > 0)
        {
            var suffix = hourValue == 1 && profile.HourSuffixSingular.Length > 0
                ? profile.HourSuffixSingular
                : profile.HourSuffixPlural;
            return baseWord + " " + suffix;
        }

        return baseWord;
    }

    string ResolveMinuteWords(int minutes)
    {
        // In numeric mode, always produce a digit string -- even for zero -- so templates
        // like "{hour}時{minutes}分" correctly expand to "0時0分".
        if (profile.HourMode == PhraseClockHourMode.Numeric)
        {
            return minutes.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        if (minutes == 0)
        {
            return "";
        }

        return profile.MinuteGender switch
        {
            GrammaticalGender.Feminine => minutes.ToWords(GrammaticalGender.Feminine),
            GrammaticalGender.Neuter => minutes.ToWords(GrammaticalGender.Neuter),
            _ => minutes.ToWords()
        };
    }

    string ResolveArticle(int rawHour)
    {
        if (profile.SingularArticle.Length == 0 && profile.PluralArticle.Length == 0)
        {
            return "";
        }

        var hourValue = ResolveHourValue(rawHour);
        return hourValue == 1 ? profile.SingularArticle : profile.PluralArticle;
    }

    /// <summary>
    /// Resolves the minute suffix based on the relevant minute count for the current range.
    /// The count matches the minute value used in the range template: minutes for pastHour,
    /// minutesFromHalf for beforeHalf/afterHalf, minutesReverse for beforeNext.
    /// For Lb: "Minutt" (singular) when count == 1, "Minutten" (plural) otherwise.
    /// </summary>
    string ResolveMinuteSuffix(int normalizedMinutes)
    {
        if (profile.MinuteSuffixSingular.Length == 0 && profile.MinuteSuffixPlural.Length == 0)
        {
            return "";
        }

        // Determine the relevant count based on the minute range, matching GetRangeTemplate.
        int relevantCount;
        if (normalizedMinutes is > 0 and < 25)
        {
            relevantCount = normalizedMinutes;
        }
        else if (normalizedMinutes is > 25 and < 30)
        {
            relevantCount = 30 - normalizedMinutes;
        }
        else if (normalizedMinutes is > 30 and < 35)
        {
            relevantCount = normalizedMinutes - 30;
        }
        else if (normalizedMinutes is > 35 and < 60)
        {
            relevantCount = 60 - normalizedMinutes;
        }
        else
        {
            // Bucket positions (0, 5, 10, ..., 55) — suffix not typically used but default to plural.
            relevantCount = normalizedMinutes;
        }

        return relevantCount == 1 && profile.MinuteSuffixSingular.Length > 0
            ? profile.MinuteSuffixSingular
            : profile.MinuteSuffixPlural;
    }

    string GetBucketTemplate(int minutes)
    {
        return minutes switch
        {
            0 => profile.Min0,
            5 => profile.Min5,
            10 => profile.Min10,
            15 => profile.Min15,
            20 => profile.Min20,
            25 => profile.Min25,
            30 => profile.Min30,
            35 => profile.Min35,
            40 => profile.Min40,
            45 => profile.Min45,
            50 => profile.Min50,
            55 => profile.Min55,
            _ => ""
        };
    }

    string GetRangeTemplate(int minutes)
    {
        // Range boundaries are aligned with the 5-minute bucket positions so that
        // range templates fill the gaps between buckets rather than overlapping them.
        return minutes switch
        {
            > 0 and < 25 when profile.PastHourTemplate.Length > 0 => profile.PastHourTemplate,
            > 25 and < 30 when profile.BeforeHalfTemplate.Length > 0 => profile.BeforeHalfTemplate,
            > 30 and < 35 when profile.AfterHalfTemplate.Length > 0 => profile.AfterHalfTemplate,
            > 35 and < 60 when profile.BeforeNextTemplate.Length > 0 => profile.BeforeNextTemplate,
            _ => ""
        };
    }

    string ExpandTemplate(
        string template, string hour, string nextHour,
        string minutes, string minutesReverse, string minutesFromHalf,
        string article, string nextArticle, string minuteSuffix)
    {
        var result = template;

        // First pass: replace non-number placeholders so the Eifeler rule can see
        // the resolved suffix and article words when processing number placeholders.
        if (result.Contains("{article}"))
        {
            result = result.Replace("{article}", article);
        }

        if (result.Contains("{nextArticle}"))
        {
            result = result.Replace("{nextArticle}", nextArticle);
        }

        if (result.Contains("{minuteSuffix}"))
        {
            result = result.Replace("{minuteSuffix}", minuteSuffix);
        }

        // Second pass: replace number placeholders with Eifeler awareness.
        result = ReplaceNumberPlaceholder(result, "{hour}", hour);
        result = ReplaceNumberPlaceholder(result, "{nextHour}", nextHour);
        result = ReplaceNumberPlaceholder(result, "{minutes}", minutes);
        result = ReplaceNumberPlaceholder(result, "{minutesReverse}", minutesReverse);
        result = ReplaceNumberPlaceholder(result, "{minutesFromHalf}", minutesFromHalf);

        // Collapse double spaces left by empty placeholders and trim edges so templates
        // like "{hour} {minutes}" produce "tretten" instead of "tretten " at :00.
        while (result.Contains("  "))
        {
            result = result.Replace("  ", " ");
        }

        return result.Trim();
    }

    /// <summary>
    /// Replaces a number-word placeholder, applying the Eifeler rule when enabled.
    /// The rule trims trailing 'n' from the last word of the resolved number value
    /// when the first word immediately after it starts with a non-blocking character.
    /// </summary>
    string ReplaceNumberPlaceholder(string template, string placeholder, string value)
    {
        var index = template.IndexOf(placeholder, StringComparison.Ordinal);
        if (index < 0)
        {
            return template;
        }

        var resolvedValue = value;
        if (profile.ApplyEifelerRule && resolvedValue.Length > 0)
        {
            // Find the first word after the placeholder in the (partially expanded) template.
            var afterIndex = index + placeholder.Length;
            var nextWord = ExtractNextLiteralWord(template, afterIndex);
            if (nextWord.Length > 0)
            {
                // Apply the Eifeler rule to the last word of the resolved number value.
                var lastSpace = resolvedValue.LastIndexOf(' ');
                if (lastSpace >= 0)
                {
                    var lastWord = resolvedValue[(lastSpace + 1)..];
                    var eifeled = EifelerRule.ApplyIfNeeded(lastWord, nextWord);
                    resolvedValue = resolvedValue[..(lastSpace + 1)] + eifeled;
                }
                else
                {
                    resolvedValue = EifelerRule.ApplyIfNeeded(resolvedValue, nextWord);
                }
            }
        }

        return template.Replace(placeholder, resolvedValue);
    }

    /// <summary>
    /// Extracts the first literal word after the given position in the template.
    /// Skips spaces and any remaining placeholders (curly-brace tokens).
    /// </summary>
    static string ExtractNextLiteralWord(string template, int startIndex)
    {
        var i = startIndex;
        // Skip leading spaces.
        while (i < template.Length && template[i] == ' ')
        {
            i++;
        }

        if (i >= template.Length)
        {
            return "";
        }

        // If we hit a remaining placeholder, skip it and try the next word.
        if (template[i] == '{')
        {
            var closeBrace = template.IndexOf('}', i);
            if (closeBrace >= 0)
            {
                return ExtractNextLiteralWord(template, closeBrace + 1);
            }

            return "";
        }

        // Extract the word until space or placeholder.
        var wordStart = i;
        while (i < template.Length && template[i] != ' ' && template[i] != '{')
        {
            i++;
        }

        return template[wordStart..i];
    }

    string ApplyDayPeriod(string basePhrase, int hour, int normalizedMinutes)
    {
        if (profile.EarlyMorning.Length == 0 && profile.Morning.Length == 0 &&
            profile.Afternoon.Length == 0 && profile.Night.Length == 0)
        {
            return basePhrase;
        }

        // For relative-hour style locales (ca, es), minutes >= 35 reference the NEXT hour,
        // so the day period should be based on the next hour's time slot.
        var periodHour = normalizedMinutes >= 35 ? hour + 1 : hour;
        var period = GetDayPeriod(periodHour);
        if (period.Length == 0)
        {
            return basePhrase;
        }

        return profile.DayPeriodPosition == PhraseClockDayPeriodPosition.Prefix
            ? period + " " + basePhrase
            : basePhrase + " " + period;
    }

    string GetDayPeriod(int hour)
    {
        var normalized = hour % 24;
        if (normalized is >= 1 and < 6)
        {
            return profile.EarlyMorning;
        }

        if (normalized is >= 6 and < 12)
        {
            return profile.Morning;
        }

        if (normalized is >= 12 and < 21)
        {
            return profile.Afternoon;
        }

        return profile.Night;
    }
}

/// <summary>
/// Defines the hour rendering mode for <see cref="PhraseClockNotationConverter"/>.
/// </summary>
enum PhraseClockHourMode
{
    /// <summary>12-hour clock (1-12).</summary>
    H12,
    /// <summary>24-hour clock (0-23).</summary>
    H24,
    /// <summary>Numeric digits instead of words.</summary>
    Numeric
}

/// <summary>
/// Defines day-period attachment position for <see cref="PhraseClockNotationConverter"/>.
/// </summary>
enum PhraseClockDayPeriodPosition
{
    /// <summary>Day period appears after the time phrase.</summary>
    Suffix,
    /// <summary>Day period appears before the time phrase.</summary>
    Prefix
}

/// <summary>
/// Stores the YAML-driven configuration for <see cref="PhraseClockNotationConverter"/>.
/// All fields are source-generated from locale YAML and cached as static instances.
/// </summary>
sealed class PhraseClockNotationProfile(
    PhraseClockHourMode hourMode,
    GrammaticalGender hourGender,
    GrammaticalGender minuteGender,
    string midnight,
    string midday,
    string min0,
    string min5,
    string min10,
    string min15,
    string min20,
    string min25,
    string min30,
    string min35,
    string min40,
    string min45,
    string min50,
    string min55,
    string defaultTemplate,
    string zeroFiller,
    string earlyMorning,
    string morning,
    string afternoon,
    string night,
    PhraseClockDayPeriodPosition dayPeriodPosition,
    string hourZeroWord,
    string hourOneWord,
    string hourTwelveWord,
    string hourSuffixSingular,
    string hourSuffixPlural,
    string singularArticle,
    string pluralArticle,
    bool applyEifelerRule,
    string pastHourTemplate,
    string beforeHalfTemplate,
    string afterHalfTemplate,
    string beforeNextTemplate,
    string minuteSuffixSingular,
    string minuteSuffixPlural)
{
    /// <summary>Gets the hour rendering mode.</summary>
    public PhraseClockHourMode HourMode { get; } = hourMode;
    /// <summary>Gets the grammatical gender for hour number words.</summary>
    public GrammaticalGender HourGender { get; } = hourGender;
    /// <summary>Gets the grammatical gender for minute number words.</summary>
    public GrammaticalGender MinuteGender { get; } = minuteGender;
    /// <summary>Gets the fixed phrase for midnight.</summary>
    public string Midnight { get; } = midnight;
    /// <summary>Gets the fixed phrase for midday.</summary>
    public string Midday { get; } = midday;
    /// <summary>Gets the template for on-the-hour (0 minutes).</summary>
    public string Min0 { get; } = min0;
    /// <summary>Gets the template for 5 minutes past.</summary>
    public string Min5 { get; } = min5;
    /// <summary>Gets the template for 10 minutes past.</summary>
    public string Min10 { get; } = min10;
    /// <summary>Gets the template for 15 minutes past.</summary>
    public string Min15 { get; } = min15;
    /// <summary>Gets the template for 20 minutes past.</summary>
    public string Min20 { get; } = min20;
    /// <summary>Gets the template for 25 minutes past.</summary>
    public string Min25 { get; } = min25;
    /// <summary>Gets the template for 30 minutes past.</summary>
    public string Min30 { get; } = min30;
    /// <summary>Gets the template for 35 minutes past.</summary>
    public string Min35 { get; } = min35;
    /// <summary>Gets the template for 40 minutes past.</summary>
    public string Min40 { get; } = min40;
    /// <summary>Gets the template for 45 minutes past.</summary>
    public string Min45 { get; } = min45;
    /// <summary>Gets the template for 50 minutes past.</summary>
    public string Min50 { get; } = min50;
    /// <summary>Gets the template for 55 minutes past.</summary>
    public string Min55 { get; } = min55;
    /// <summary>Gets the fallback template for non-bucketed minutes.</summary>
    public string DefaultTemplate { get; } = defaultTemplate;
    /// <summary>Gets the zero-filler word inserted when minutes are 1-9 (e.g., "noll", "nul").</summary>
    public string ZeroFiller { get; } = zeroFiller;
    /// <summary>Gets the day-period word for early morning (1:00-5:59).</summary>
    public string EarlyMorning { get; } = earlyMorning;
    /// <summary>Gets the day-period word for morning (6:00-11:59).</summary>
    public string Morning { get; } = morning;
    /// <summary>Gets the day-period word for afternoon (12:00-20:59).</summary>
    public string Afternoon { get; } = afternoon;
    /// <summary>Gets the day-period word for night (21:00-0:59).</summary>
    public string Night { get; } = night;
    /// <summary>Gets the position of day-period words relative to the time phrase.</summary>
    public PhraseClockDayPeriodPosition DayPeriodPosition { get; } = dayPeriodPosition;
    /// <summary>Gets the fixed hour word for hour value 0 (e.g., French "minuit").</summary>
    public string HourZeroWord { get; } = hourZeroWord;
    /// <summary>Gets the fixed hour word for hour value 1 when the number engine produces an unsuitable form (e.g., Norwegian "ett").</summary>
    public string HourOneWord { get; } = hourOneWord;
    /// <summary>Gets the fixed hour word for hour value 12 (e.g., French "midi").</summary>
    public string HourTwelveWord { get; } = hourTwelveWord;
    /// <summary>Gets the hour suffix for singular hours (e.g., French "heure").</summary>
    public string HourSuffixSingular { get; } = hourSuffixSingular;
    /// <summary>Gets the hour suffix for plural hours (e.g., French "heures").</summary>
    public string HourSuffixPlural { get; } = hourSuffixPlural;
    /// <summary>Gets the article for singular hours (e.g., Catalan "la").</summary>
    public string SingularArticle { get; } = singularArticle;
    /// <summary>Gets the article for plural hours (e.g., Catalan "les").</summary>
    public string PluralArticle { get; } = pluralArticle;
    /// <summary>Gets whether the Luxembourgish Eifeler rule should be applied as post-processing.</summary>
    public bool ApplyEifelerRule { get; } = applyEifelerRule;
    /// <summary>Gets the range template for minutes 1-14 (past the hour).</summary>
    public string PastHourTemplate { get; } = pastHourTemplate;
    /// <summary>Gets the range template for minutes 16-29 (before the half).</summary>
    public string BeforeHalfTemplate { get; } = beforeHalfTemplate;
    /// <summary>Gets the range template for minutes 31-44 (after the half).</summary>
    public string AfterHalfTemplate { get; } = afterHalfTemplate;
    /// <summary>Gets the range template for minutes 46-59 (before the next hour).</summary>
    public string BeforeNextTemplate { get; } = beforeNextTemplate;
    /// <summary>Gets the singular minute suffix (e.g., Lb "Minutt").</summary>
    public string MinuteSuffixSingular { get; } = minuteSuffixSingular;
    /// <summary>Gets the plural minute suffix (e.g., Lb "Minutten").</summary>
    public string MinuteSuffixPlural { get; } = minuteSuffixPlural;
}

#endif