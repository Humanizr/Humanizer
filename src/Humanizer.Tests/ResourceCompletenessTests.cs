namespace Humanizer.Tests.Localisation;

public class ResourceCompletenessTests
{
    /// <summary>
    /// All supported cultures with their corresponding .resx files
    /// </summary>
    private static readonly string[] SupportedCultures = {
        "af", "ar", "az", "bg", "bn-BD", "cs", "da", "de", "el", "es", "fa", 
        "fi-FI", "fil-PH", "fr-BE", "fr", "he", "hr", "hu", "hy", "id", "is", 
        "it", "ja", "ko-KR", "ku", "lb", "lt", "lv", "ms-MY", "mt", "nb-NO", 
        "nb", "nl", "pl", "pt-BR", "pt", "ro", "ru", "sk", "sl", "sr-Latn", 
        "sr", "sv", "th-TH", "tr", "uk", "uz-Cyrl-UZ", "uz-Latn-UZ", "vi", 
        "zh-CN", "zh-Hans", "zh-Hant"
    };

    /// <summary>
    /// All resource keys that should be present in every .resx file
    /// </summary>
    private static readonly string[] RequiredResourceKeys = {
        "DateHumanize_SingleSecondAgo", "DateHumanize_MultipleSecondsAgo", "DateHumanize_SingleMinuteAgo",
        "DateHumanize_MultipleMinutesAgo", "DateHumanize_SingleHourAgo", "DateHumanize_MultipleHoursAgo",
        "DateHumanize_SingleDayAgo", "DateHumanize_MultipleDaysAgo", "DateHumanize_SingleMonthAgo",
        "DateHumanize_MultipleMonthsAgo", "DateHumanize_SingleYearAgo", "DateHumanize_MultipleYearsAgo",
        "TimeSpanHumanize_MultipleDays", "TimeSpanHumanize_MultipleHours", "TimeSpanHumanize_MultipleMilliseconds",
        "TimeSpanHumanize_MultipleMinutes", "TimeSpanHumanize_MultipleSeconds", "TimeSpanHumanize_SingleDay",
        "TimeSpanHumanize_SingleHour", "TimeSpanHumanize_SingleMillisecond", "TimeSpanHumanize_SingleMinute",
        "TimeSpanHumanize_SingleSecond", "TimeSpanHumanize_Zero", "TimeSpanHumanize_MultipleWeeks",
        "TimeSpanHumanize_SingleWeek", "DateHumanize_MultipleDaysFromNow", "DateHumanize_MultipleHoursFromNow",
        "DateHumanize_MultipleMinutesFromNow", "DateHumanize_MultipleMonthsFromNow", "DateHumanize_MultipleSecondsFromNow",
        "DateHumanize_MultipleYearsFromNow", "DateHumanize_Now", "DateHumanize_SingleDayFromNow",
        "DateHumanize_SingleHourFromNow", "DateHumanize_SingleMinuteFromNow", "DateHumanize_SingleMonthFromNow",
        "DateHumanize_SingleSecondFromNow", "DateHumanize_SingleYearFromNow", "DateHumanize_MultipleHoursFromNow_Dual",
        "DateHumanize_MultipleMinutesFromNow_Plural", "DateHumanize_MultipleSecondsAgo_Dual", "DateHumanize_MultipleSecondsFromNow_Dual",
        "DateHumanize_MultipleYearsFromNow_Dual", "TimeSpanHumanize_MultipleDays_Dual", "TimeSpanHumanize_MultipleDays_Plural",
        "TimeSpanHumanize_MultipleHours_Plural", "TimeSpanHumanize_MultipleMilliseconds_Dual", "TimeSpanHumanize_MultipleMinutes_Dual",
        "TimeSpanHumanize_MultipleMinutes_Plural", "TimeSpanHumanize_MultipleSeconds_Plural", "TimeSpanHumanize_MultipleMilliseconds_Plural",
        "DateHumanize_MultipleDaysAgo_Dual", "DateHumanize_MultipleDaysAgo_Plural", "DateHumanize_MultipleDaysAgo_Singular",
        "DateHumanize_MultipleDaysFromNow_Dual", "DateHumanize_MultipleDaysFromNow_Plural", "DateHumanize_MultipleDaysFromNow_Singular",
        "DateHumanize_MultipleHoursAgo_Dual", "DateHumanize_MultipleHoursAgo_Plural", "DateHumanize_MultipleHoursAgo_Singular",
        "DateHumanize_MultipleHoursFromNow_Plural", "DateHumanize_MultipleHoursFromNow_Singular", "DateHumanize_MultipleMinutesAgo_Dual",
        "DateHumanize_MultipleMinutesAgo_Plural", "DateHumanize_MultipleMinutesAgo_Singular", "DateHumanize_MultipleMinutesFromNow_Dual",
        "DateHumanize_MultipleMinutesFromNow_Singular", "DateHumanize_MultipleMonthsAgo_Dual", "DateHumanize_MultipleMonthsAgo_Plural",
        "DateHumanize_MultipleMonthsAgo_Singular", "DateHumanize_MultipleMonthsFromNow_Dual", "DateHumanize_MultipleMonthsFromNow_Plural",
        "DateHumanize_MultipleMonthsFromNow_Singular", "DateHumanize_MultipleSecondsAgo_Plural", "DateHumanize_MultipleSecondsAgo_Singular",
        "DateHumanize_MultipleSecondsFromNow_Plural", "DateHumanize_MultipleSecondsFromNow_Singular", "DateHumanize_MultipleYearsAgo_Dual",
        "DateHumanize_MultipleYearsAgo_Plural", "DateHumanize_MultipleYearsAgo_Singular", "DateHumanize_MultipleYearsFromNow_Plural",
        "DateHumanize_MultipleYearsFromNow_Singular", "TimeSpanHumanize_MultipleDays_Singular", "TimeSpanHumanize_MultipleHours_Dual",
        "TimeSpanHumanize_MultipleHours_Singular", "TimeSpanHumanize_MultipleMilliseconds_Singular", "TimeSpanHumanize_MultipleMinutes_Singular",
        "TimeSpanHumanize_MultipleSeconds_Dual", "TimeSpanHumanize_MultipleSeconds_Singular", "TimeSpanHumanize_MultipleWeeks_Dual",
        "TimeSpanHumanize_MultipleWeeks_Plural", "TimeSpanHumanize_MultipleWeeks_Singular", "TimeSpanHumanize_MultipleMonths",
        "TimeSpanHumanize_MultipleYears", "TimeSpanHumanize_SingleMonth", "TimeSpanHumanize_SingleYear", "DateHumanize_Never",
        "TimeSpanHumanize_MultipleMonths_Dual", "TimeSpanHumanize_MultipleMonths_Plural", "TimeSpanHumanize_MultipleMonths_Singular",
        "TimeSpanHumanize_MultipleYears_Dual", "TimeSpanHumanize_MultipleYears_Plural", "TimeSpanHumanize_MultipleYears_Singular",
        "TimeSpanHumanize_SingleDay_Words", "TimeSpanHumanize_SingleHour_Words", "TimeSpanHumanize_SingleMillisecond_Words",
        "TimeSpanHumanize_SingleMinute_Words", "TimeSpanHumanize_SingleMonth_Words", "TimeSpanHumanize_SingleSecond_Words",
        "TimeSpanHumanize_SingleWeek_Words", "TimeSpanHumanize_SingleYear_Words", "N", "NNE", "NE", "ENE", "E", "ESE",
        "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW", "N_Short", "NNE_Short", "NE_Short", "ENE_Short",
        "E_Short", "ESE_Short", "SE_Short", "SSE_Short", "S_Short", "SSW_Short", "SW_Short", "WSW_Short", "W_Short",
        "WNW_Short", "NW_Short", "NNW_Short", "DataUnit_Bit", "DataUnit_BitSymbol", "DataUnit_Byte", "DataUnit_ByteSymbol",
        "DataUnit_Kilobyte", "DataUnit_KilobyteSymbol", "DataUnit_Megabyte", "DataUnit_MegabyteSymbol", "DataUnit_Gigabyte",
        "DataUnit_GigabyteSymbol", "DataUnit_Terabyte", "DataUnit_TerabyteSymbol", "TimeUnit_Millisecond", "TimeUnit_Second",
        "TimeUnit_Minute", "TimeUnit_Hour", "TimeUnit_Day", "TimeUnit_Week", "TimeUnit_Month", "TimeUnit_Year",
        "DateHumanize_MultipleDaysAgo_Paucal", "DateHumanize_MultipleDaysFromNow_Paucal", "DateHumanize_MultipleHoursAgo_Paucal",
        "DateHumanize_MultipleHoursFromNow_Paucal", "DateHumanize_MultipleMinutesAgo_Paucal", "DateHumanize_MultipleMinutesFromNow_Paucal",
        "DateHumanize_MultipleMonthsAgo_Paucal", "DateHumanize_MultipleMonthsFromNow_Paucal", "DateHumanize_MultipleSecondsAgo_Paucal",
        "DateHumanize_MultipleSecondsFromNow_Paucal", "DateHumanize_MultipleYearsAgo_Paucal", "DateHumanize_MultipleYearsFromNow_Paucal",
        "DateHumanize_TwoDaysAgo", "DateHumanize_TwoDaysFromNow", "TimeSpanHumanize_MultipleDays_Paucal", "TimeSpanHumanize_MultipleHours_Paucal",
        "TimeSpanHumanize_MultipleMilliseconds_Paucal", "TimeSpanHumanize_MultipleMinutes_Paucal", "TimeSpanHumanize_MultipleMonths_Paucal",
        "TimeSpanHumanize_MultipleSeconds_Paucal", "TimeSpanHumanize_MultipleWeeks_Paucal", "TimeSpanHumanize_MultipleYears_Paucal",
        "TimeSpanHumanize_Age"
    };

    [Theory]
    [InlineData("af")]
    [InlineData("ar")]
    [InlineData("az")]
    [InlineData("bg")]
    [InlineData("bn-BD")]
    [InlineData("cs")]
    [InlineData("da")]
    [InlineData("de")]
    [InlineData("el")]
    [InlineData("es")]
    [InlineData("fa")]
    [InlineData("fi-FI")]
    [InlineData("fil-PH")]
    [InlineData("fr-BE")]
    [InlineData("fr")]
    [InlineData("he")]
    [InlineData("hr")]
    [InlineData("hu")]
    [InlineData("hy")]
    [InlineData("id")]
    [InlineData("is")]
    [InlineData("it")]
    [InlineData("ja")]
    [InlineData("ko-KR")]
    [InlineData("ku")]
    [InlineData("lb")]
    [InlineData("lt")]
    [InlineData("lv")]
    [InlineData("ms-MY")]
    [InlineData("mt")]
    [InlineData("nb-NO")]
    [InlineData("nb")]
    [InlineData("nl")]
    [InlineData("pl")]
    [InlineData("pt-BR")]
    [InlineData("pt")]
    [InlineData("ro")]
    [InlineData("ru")]
    [InlineData("sk")]
    [InlineData("sl")]
    [InlineData("sr-Latn")]
    [InlineData("sr")]
    [InlineData("sv")]
    [InlineData("th-TH")]
    [InlineData("tr")]
    [InlineData("uk")]
    [InlineData("uz-Cyrl-UZ")]
    [InlineData("uz-Latn-UZ")]
    [InlineData("vi")]
    [InlineData("zh-CN")]
    [InlineData("zh-Hans")]
    [InlineData("zh-Hant")]
    public void AllResourceKeysExistForCulture(string cultureName)
    {
        // Arrange
        var culture = new CultureInfo(cultureName);
        var missingKeys = new List<string>();

        // Act - Check each required key exists for the culture
        foreach (var key in RequiredResourceKeys)
        {
            try
            {
                var resource = Resources.GetResource(key, culture);
                if (string.IsNullOrEmpty(resource))
                {
                    missingKeys.Add(key);
                }
            }
            catch (Exception)
            {
                missingKeys.Add(key);
            }
        }

        // Assert
        Assert.Empty(missingKeys.Select(k => $"Missing key '{k}' for culture '{cultureName}'"));
    }

    [Fact]
    public void AllSupportedCulturesHaveCompleteResources()
    {
        // Arrange
        var incompleteCultures = new List<string>();

        // Act
        foreach (var cultureName in SupportedCultures)
        {
            var culture = new CultureInfo(cultureName);
            var missingKeysCount = 0;

            foreach (var key in RequiredResourceKeys)
            {
                try
                {
                    var resource = Resources.GetResource(key, culture);
                    if (string.IsNullOrEmpty(resource))
                    {
                        missingKeysCount++;
                    }
                }
                catch (Exception)
                {
                    missingKeysCount++;
                }
            }

            if (missingKeysCount > 0)
            {
                incompleteCultures.Add($"{cultureName} (missing {missingKeysCount} keys)");
            }
        }

        // Assert
        Assert.Empty(incompleteCultures.Select(c => $"Incomplete culture: {c}"));
    }

    [Fact]
    public void AllResourceKeysAreNonEmptyForDefaultCulture()
    {
        // Arrange
        var emptyKeys = new List<string>();

        // Act - Check each key has a non-empty value in the default culture
        foreach (var key in RequiredResourceKeys)
        {
            try
            {
                var resource = Resources.GetResource(key);
                if (string.IsNullOrWhiteSpace(resource))
                {
                    emptyKeys.Add(key);
                }
            }
            catch (Exception)
            {
                emptyKeys.Add(key);
            }
        }

        // Assert
        Assert.Empty(emptyKeys.Select(k => $"Empty or missing key '{k}' in default culture"));
    }

    [Theory]
    [MemberData(nameof(GetCultureAndKeyPairs))]
    public void ResourceKeyExistsAndIsNotEmpty(string cultureName, string key)
    {
        // Arrange
        var culture = new CultureInfo(cultureName);

        // Act
        var resource = Resources.GetResource(key, culture);

        // Assert
        Assert.NotNull(resource);
        Assert.NotEmpty(resource);
    }

    public static IEnumerable<object[]> GetCultureAndKeyPairs()
    {
        foreach (var culture in SupportedCultures)
        {
            foreach (var key in RequiredResourceKeys)
            {
                yield return new object[] { culture, key };
            }
        }
    }
}