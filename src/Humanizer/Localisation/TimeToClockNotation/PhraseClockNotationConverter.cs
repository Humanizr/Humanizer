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
        // When compactMinuteWords is active (CJK locales), omit the space between filler and word.
        var minuteWords = normalizedMinutes is > 0 and < 10 && profile.ZeroFiller.Length > 0
            ? string.Concat(profile.ZeroFiller, profile.CompactMinuteWords ? "" : " ", rawMinuteWords)
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

        // Pre-compute the day-period string for possible inline use via {dayPeriod} placeholder.
        var dayPeriod = GetDayPeriod(hour);

        // Check minute-bucket template first (exact 5-minute intervals).
        var template = GetBucketTemplate(normalizedMinutes);
        if (template.Length > 0)
        {
            var minuteSuffix = ResolveMinuteSuffixDirect(normalizedMinutes);
            var result = ExpandTemplate(template, hourWords, nextHourWords, minuteWords, reverseMinuteWords, halfMinuteWords, article, nextArticle, minuteSuffix, dayPeriod);
            return ApplyDayPeriodIfNeeded(result, template, hour, normalizedMinutes);
        }

        // Try range-based templates for non-bucketed minutes.
        // Range templates use relative minute counts (e.g., "minutes to half" or "minutes to next hour"),
        // so the suffix must reflect the range-relative count, not the absolute minute value.
        var rangeTemplate = GetRangeTemplate(normalizedMinutes);
        if (rangeTemplate.Length > 0)
        {
            var minuteSuffix = ResolveMinuteSuffixForRange(normalizedMinutes);
            var result = ExpandTemplate(rangeTemplate, hourWords, nextHourWords, minuteWords, reverseMinuteWords, halfMinuteWords, article, nextArticle, minuteSuffix, dayPeriod);
            return ApplyDayPeriodIfNeeded(result, rangeTemplate, hour, normalizedMinutes);
        }

        // Fall to default template — use the actual minute count for suffix resolution.
        if (profile.DefaultTemplate.Length > 0)
        {
            var minuteSuffix = ResolveMinuteSuffixDirect(normalizedMinutes);
            var result = ExpandTemplate(profile.DefaultTemplate, hourWords, nextHourWords, minuteWords, reverseMinuteWords, halfMinuteWords, article, nextArticle, minuteSuffix, dayPeriod);
            return ApplyDayPeriodIfNeeded(result, profile.DefaultTemplate, hour, normalizedMinutes);
        }

        // Absolute fallback: "{hour} {minutes}".
        var fallback = minuteWords.Length > 0 ? hourWords + " " + minuteWords : hourWords;
        return ApplyDayPeriod(fallback, hour, usesNextHour: false);
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

        // Check the explicit hour-words map first (e.g., Polish ordinal feminine forms).
        if (profile.HourWordsMap.Length > hourValue && profile.HourWordsMap[hourValue].Length > 0)
        {
            return profile.HourWordsMap[hourValue];
        }

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
                _ => hourValue.ToWords(GrammaticalGender.Masculine)
            };
        }

        // Apply hour suffix (singular/paucal/plural variants).
        if (profile.HourSuffixSingular.Length > 0 || profile.HourSuffixPlural.Length > 0)
        {
            var suffix = ResolveSlavicSuffix(hourValue, profile.HourSuffixSingular, profile.HourSuffixPaucal, profile.HourSuffixPlural, profile.PaucalLowOnly);
            return string.Concat(baseWord, " ", suffix);
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

        // Check the explicit minute-words map first (e.g., Filipino Spanish-derived clock numbers).
        if (profile.MinuteWordsMap.Length > minutes && profile.MinuteWordsMap[minutes].Length > 0)
        {
            return profile.MinuteWordsMap[minutes];
        }

        var words = profile.MinuteGender switch
        {
            GrammaticalGender.Feminine => minutes.ToWords(GrammaticalGender.Feminine),
            GrammaticalGender.Neuter => minutes.ToWords(GrammaticalGender.Neuter),
            _ => minutes.ToWords()
        };

        // Slovak and similar locales write compound tens+units without spaces in clock notation
        // (e.g., "dvadsaťtri" instead of "dvadsať tri").
        if (profile.CompactMinuteWords)
        {
            return RemoveSpaces(words);
        }

        // Arabic-style conjunction compaction: " و " → " و" (attaches conjunction to the
        // following word so "ثلاث و عشرون" becomes "ثلاث وعشرون").
        if (profile.CompactConjunction.Length > 0)
        {
            return CompactConjunctionInline(words, profile.CompactConjunction);
        }

        return words;
    }

    /// <summary>
    /// Removes all spaces from <paramref name="input"/> using a stack buffer.
    /// </summary>
    static string RemoveSpaces(string input)
    {
        Span<char> buf = stackalloc char[input.Length];
        var pos = 0;
        foreach (var c in input)
        {
            if (c != ' ')
            {
                buf[pos++] = c;
            }
        }

        return new string(buf[..pos]);
    }

    /// <summary>
    /// Replaces " {conj} " with " {conj}" (removes trailing space after conjunction)
    /// using a stack buffer to avoid intermediate string allocations.
    /// </summary>
    static string CompactConjunctionInline(string input, string conjunction)
    {
        // Pattern: " {conj} " → " {conj}" (remove one trailing space).
        var patternLen = conjunction.Length + 2; // " " + conj + " "
        var idx = 0;
        while (idx <= input.Length - patternLen)
        {
            if (input[idx] == ' '
                && input.AsSpan(idx + 1, conjunction.Length).SequenceEqual(conjunction)
                && input[idx + 1 + conjunction.Length] == ' ')
            {
                // Found the pattern — build result removing the trailing space.
                return string.Concat(
                    input.AsSpan(0, idx + 1 + conjunction.Length),
                    input.AsSpan(idx + patternLen));
            }

            idx++;
        }

        return input;
    }

    /// <summary>
    /// Resolves the correct suffix form for Slavic-style grammatical number.
    /// When <paramref name="paucalLowOnly"/> is false (South Slavic default):
    ///   singular (1, 21, 31…), paucal (2-4, 22-24…), plural (5-20, 25-30…).
    /// When <paramref name="paucalLowOnly"/> is true (West Slavic — Czech, Slovak):
    ///   singular (1), paucal (2-4 only), plural (5+, including 21-24, 31-34…).
    /// When paucal is empty, falls back to singular/plural only.
    /// </summary>
    static string ResolveSlavicSuffix(int count, string singular, string paucal, string plural, bool paucalLowOnly = false)
    {
        if (paucal.Length > 0)
        {
            if (paucalLowOnly)
            {
                // West Slavic: paucal only for the exact values 2, 3, 4.
                if (count is > 1 and < 5)
                {
                    return paucal;
                }

                return count == 1 && singular.Length > 0 ? singular : plural;
            }

            // South Slavic: paucal for units 2-4, excluding teens.
            var tens = count % 100 / 10;
            if (tens != 1) // teens (11-19) are always plural
            {
                var units = count % 10;
                if (units == 1)
                {
                    return singular.Length > 0 ? singular : plural;
                }

                if (units is > 1 and < 5)
                {
                    return paucal;
                }
            }

            return plural;
        }

        // No paucal form — simple singular/plural.
        return count == 1 && singular.Length > 0 ? singular : plural;
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
    /// Resolves the minute suffix using the actual (absolute) minute count.
    /// Used for bucket templates and the default template where the suffix should
    /// agree with the minute value as spoken (e.g., bg "двадесет и три минути" = plural).
    /// </summary>
    string ResolveMinuteSuffixDirect(int normalizedMinutes)
    {
        if (profile.MinuteSuffixSingular.Length == 0 && profile.MinuteSuffixPlural.Length == 0)
        {
            return "";
        }

        return ResolveSlavicSuffix(normalizedMinutes, profile.MinuteSuffixSingular, profile.MinuteSuffixPaucal, profile.MinuteSuffixPlural, profile.PaucalLowOnly);
    }

    /// <summary>
    /// Resolves the minute suffix based on the range-relative minute count.
    /// The count matches the minute value used in the range template: minutes for pastHour,
    /// minutesFromHalf for beforeHalf/afterHalf, minutesReverse for beforeNext.
    /// For Lb: "Minutt" (singular) when count == 1, "Minutten" (plural) otherwise.
    /// </summary>
    string ResolveMinuteSuffixForRange(int normalizedMinutes)
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

        return ResolveSlavicSuffix(relevantCount, profile.MinuteSuffixSingular, profile.MinuteSuffixPaucal, profile.MinuteSuffixPlural, profile.PaucalLowOnly);
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
        string article, string nextArticle, string minuteSuffix, string dayPeriod)
    {
        // Single-pass expansion: scan the template once, resolve placeholders inline,
        // skip double spaces, and trim — producing only the final return string.
        var maxLen = template.Length
            + hour.Length + nextHour.Length + minutes.Length
            + minutesReverse.Length + minutesFromHalf.Length
            + article.Length + nextArticle.Length + minuteSuffix.Length + dayPeriod.Length;

        Span<char> buf = stackalloc char[maxLen];
        var pos = 0;
        var i = 0;

        while (i < template.Length)
        {
            if (template[i] == '{')
            {
                var close = template.IndexOf('}', i);
                if (close < 0)
                {
                    AppendChar(buf, ref pos, template[i++]);
                    continue;
                }

                var name = template.AsSpan(i + 1, close - i - 1);
                var replacement = ResolveTemplatePlaceholder(
                    name, template, close + 1,
                    hour, nextHour, minutes, minutesReverse, minutesFromHalf,
                    article, nextArticle, minuteSuffix, dayPeriod);

                replacement.CopyTo(buf[pos..]);
                pos += replacement.Length;
                i = close + 1;
            }
            else
            {
                AppendChar(buf, ref pos, template[i++]);
            }
        }

        // Trim leading/trailing spaces from the result span.
        var result = buf[..pos];
        result = result.Trim();
        return new string(result);
    }

    static void AppendChar(Span<char> buf, ref int pos, char c)
    {
        // Collapse double spaces inline so no post-processing pass is needed.
        if (c == ' ' && pos > 0 && buf[pos - 1] == ' ')
        {
            return;
        }

        buf[pos++] = c;
    }

    /// <summary>
    /// Resolves a template placeholder by name, applying the Eifeler rule for number placeholders.
    /// Returns the replacement value as a <see cref="ReadOnlySpan{T}"/> to avoid allocations.
    /// </summary>
    ReadOnlySpan<char> ResolveTemplatePlaceholder(
        ReadOnlySpan<char> name, string template, int afterIndex,
        string hour, string nextHour, string minutes, string minutesReverse, string minutesFromHalf,
        string article, string nextArticle, string minuteSuffix, string dayPeriod)
    {
        // Non-number placeholders: return directly without Eifeler processing.
        if (name.SequenceEqual("article"))
        {
            return article;
        }

        if (name.SequenceEqual("nextArticle"))
        {
            return nextArticle;
        }

        if (name.SequenceEqual("minuteSuffix"))
        {
            return minuteSuffix;
        }

        if (name.SequenceEqual("dayPeriod"))
        {
            return dayPeriod;
        }

        // Number placeholders: apply Eifeler rule when enabled.
        string value;
        if (name.SequenceEqual("hour"))
        {
            value = hour;
        }
        else if (name.SequenceEqual("nextHour"))
        {
            value = nextHour;
        }
        else if (name.SequenceEqual("minutes"))
        {
            value = minutes;
        }
        else if (name.SequenceEqual("minutesReverse"))
        {
            value = minutesReverse;
        }
        else if (name.SequenceEqual("minutesFromHalf"))
        {
            value = minutesFromHalf;
        }
        else
        {
            return ReadOnlySpan<char>.Empty;
        }

        return ApplyEifelerToValue(value, template, afterIndex, article, nextArticle, minuteSuffix, dayPeriod);
    }

    /// <summary>
    /// Applies the Eifeler rule to a number-word value when enabled.
    /// The rule trims trailing 'n' from the last word of the resolved number value
    /// when the first word immediately after it starts with a non-blocking character.
    /// Non-number placeholder values are passed so the lookahead can resolve them inline.
    /// </summary>
    ReadOnlySpan<char> ApplyEifelerToValue(
        string value, string template, int afterIndex,
        string article, string nextArticle, string minuteSuffix, string dayPeriod)
    {
        if (!profile.ApplyEifelerRule || value.Length == 0)
        {
            return value;
        }

        var nextWord = ExtractNextWordResolvingPlaceholders(template, afterIndex, article, nextArticle, minuteSuffix, dayPeriod);
        if (nextWord.Length == 0)
        {
            return value;
        }

        // Apply the Eifeler rule to the last word of the resolved number value.
        var lastSpace = value.LastIndexOf(' ');
        if (lastSpace >= 0)
        {
            var lastWord = value[(lastSpace + 1)..];
            var eifeled = EifelerRule.ApplyIfNeeded(lastWord, nextWord);
            if (ReferenceEquals(eifeled, lastWord))
            {
                return value;
            }

            return string.Concat(value.AsSpan(0, lastSpace + 1), eifeled);
        }

        return EifelerRule.ApplyIfNeeded(value, nextWord);
    }

    /// <summary>
    /// Extracts the first word after the given position in the template.
    /// When a non-number placeholder is encountered, resolves it to its value and
    /// returns its first word, so the Eifeler rule sees the actual following word.
    /// </summary>
    static string ExtractNextWordResolvingPlaceholders(
        string template, int startIndex,
        string article, string nextArticle, string minuteSuffix, string dayPeriod)
    {
        var i = startIndex;
        while (i < template.Length)
        {
            // Skip spaces.
            while (i < template.Length && template[i] == ' ')
            {
                i++;
            }

            if (i >= template.Length)
            {
                return "";
            }

            if (template[i] == '{')
            {
                var closeBrace = template.IndexOf('}', i);
                if (closeBrace < 0)
                {
                    return "";
                }

                var name = template.AsSpan(i + 1, closeBrace - i - 1);

                // Resolve non-number placeholders to get their first word.
                var resolved = ResolveNonNumberPlaceholder(name, article, nextArticle, minuteSuffix, dayPeriod);
                if (resolved.Length > 0)
                {
                    // Return the first word of the resolved value.
                    var spaceIdx = resolved.IndexOf(' ');
                    return spaceIdx >= 0 ? resolved[..spaceIdx] : resolved;
                }

                // Skip number placeholders and continue looking.
                i = closeBrace + 1;
                continue;
            }

            // Extract the literal word until space or placeholder.
            var wordStart = i;
            while (i < template.Length && template[i] != ' ' && template[i] != '{')
            {
                i++;
            }

            return template[wordStart..i];
        }

        return "";
    }

    static string ResolveNonNumberPlaceholder(
        ReadOnlySpan<char> name, string article, string nextArticle, string minuteSuffix, string dayPeriod)
    {
        if (name.SequenceEqual("article"))
        {
            return article;
        }

        if (name.SequenceEqual("nextArticle"))
        {
            return nextArticle;
        }

        if (name.SequenceEqual("minuteSuffix"))
        {
            return minuteSuffix;
        }

        if (name.SequenceEqual("dayPeriod"))
        {
            return dayPeriod;
        }

        return "";
    }

    /// <summary>
    /// Decides whether to append/prepend day-period or skip (if template already had {dayPeriod}).
    /// Templates that reference {nextHour} shift the period to hour+1 for minutes >= 35;
    /// templates that use {hour} always base the period on the current hour.
    /// </summary>
    string ApplyDayPeriodIfNeeded(string expandedPhrase, string rawTemplate, int hour, int normalizedMinutes)
    {
        // If the template already placed the day-period inline, don't append/prepend it again.
        if (rawTemplate.Contains("{dayPeriod}"))
        {
            return expandedPhrase;
        }

        var usesNextHour = rawTemplate.Contains("{nextHour}") || rawTemplate.Contains("{nextArticle}");
        return ApplyDayPeriod(expandedPhrase, hour, usesNextHour);
    }

    string ApplyDayPeriod(string basePhrase, int hour, bool usesNextHour)
    {
        if (profile.EarlyMorning.Length == 0 && profile.Morning.Length == 0 &&
            profile.Afternoon.Length == 0 && profile.Night.Length == 0)
        {
            return basePhrase;
        }

        // For relative-hour style locales (ca, es), templates that reference {nextHour}
        // mean the phrasing is relative to the next hour (e.g., "twenty to five"),
        // so the day period should be based on the next hour's time slot.
        // For locales that say "current hour + minutes" (ar, fa, ku), the period
        // must always reflect the current hour.
        var periodHour = usesNextHour ? hour + 1 : hour;
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
    string minuteSuffixPlural,
    string hourSuffixPaucal,
    string minuteSuffixPaucal,
    string[] hourWordsMap,
    string[] minuteWordsMap,
    bool compactMinuteWords,
    bool paucalLowOnly,
    string compactConjunction)
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
    /// <summary>Gets the paucal hour suffix for Slavic locales (2-4 form, e.g., Czech "hodiny").</summary>
    public string HourSuffixPaucal { get; } = hourSuffixPaucal;
    /// <summary>Gets the paucal minute suffix for Slavic locales (2-4 form, e.g., Czech "minuty").</summary>
    public string MinuteSuffixPaucal { get; } = minuteSuffixPaucal;
    /// <summary>Gets an optional map of hour values (0-23) to explicit word strings, bypassing <c>ToWords()</c>.</summary>
    public string[] HourWordsMap { get; } = hourWordsMap;
    /// <summary>Gets an optional map of minute values (0-59) to explicit word strings, bypassing <c>ToWords()</c>.</summary>
    public string[] MinuteWordsMap { get; } = minuteWordsMap;
    /// <summary>Gets whether spaces should be removed from minute words (e.g., Slovak "dvadsaťtri" instead of "dvadsať tri").</summary>
    public bool CompactMinuteWords { get; } = compactMinuteWords;
    /// <summary>Gets whether paucal forms apply only to the exact values 2-4 (West Slavic), vs units 2-4 excluding teens (South Slavic default).</summary>
    public bool PaucalLowOnly { get; } = paucalLowOnly;
    /// <summary>Gets a conjunction word (e.g., Arabic "و") to compact in minute words. When non-empty, " {word} " is replaced with " {word}" (attaching the conjunction to the following word).</summary>
    public string CompactConjunction { get; } = compactConjunction;
}

#endif