namespace Humanizer.Tests.Localisation;

static class LocaleResourcePhraseAssertions
{
    public static void Verify(string localeName, string caseName, string expected)
    {
        var culture = CultureInfo.GetCultureInfo(localeName);

        switch (caseName)
        {
            case "DatePastSecond1": DateHumanize.Verify(expected, 1, TimeUnit.Second, Tense.Past, culture: culture); return;
            case "DateFutureSecond2": DateHumanize.Verify(expected, 2, TimeUnit.Second, Tense.Future, culture: culture); return;
            case "DatePastMinute1": DateHumanize.Verify(expected, 1, TimeUnit.Minute, Tense.Past, culture: culture); return;
            case "DateFutureMinute2": DateHumanize.Verify(expected, 2, TimeUnit.Minute, Tense.Future, culture: culture); return;
            case "DatePastHour1": DateHumanize.Verify(expected, 1, TimeUnit.Hour, Tense.Past, culture: culture); return;
            case "DateFutureHour2": DateHumanize.Verify(expected, 2, TimeUnit.Hour, Tense.Future, culture: culture); return;
            case "DatePastDay1": DateHumanize.Verify(expected, 1, TimeUnit.Day, Tense.Past, culture: culture); return;
            case "DateFutureDay1": DateHumanize.Verify(expected, 1, TimeUnit.Day, Tense.Future, culture: culture); return;
            case "DatePastDay2": DateHumanize.Verify(expected, 2, TimeUnit.Day, Tense.Past, culture: culture); return;
            case "DateFutureDay2": DateHumanize.Verify(expected, 2, TimeUnit.Day, Tense.Future, culture: culture); return;
            case "DatePastMonth1": DateHumanize.Verify(expected, 1, TimeUnit.Month, Tense.Past, culture: culture); return;
            case "DateFutureMonth2": DateHumanize.Verify(expected, 2, TimeUnit.Month, Tense.Future, culture: culture); return;
            case "DatePastYear1": DateHumanize.Verify(expected, 1, TimeUnit.Year, Tense.Past, culture: culture); return;
            case "DateFutureYear2": DateHumanize.Verify(expected, 2, TimeUnit.Year, Tense.Future, culture: culture); return;
            case "DateNow": DateHumanize.Verify(expected, 0, TimeUnit.Second, Tense.Future, culture: culture); return;
            case "DateNever": Assert.Equal(expected, ((DateTime?)null).Humanize(culture: culture)); return;
            case "SpanSecond1": Assert.Equal(expected, TimeSpan.FromSeconds(1).Humanize(culture: culture)); return;
            case "SpanSecond2": Assert.Equal(expected, TimeSpan.FromSeconds(2).Humanize(culture: culture)); return;
            case "SpanMinute1": Assert.Equal(expected, TimeSpan.FromMinutes(1).Humanize(culture: culture)); return;
            case "SpanMinute2": Assert.Equal(expected, TimeSpan.FromMinutes(2).Humanize(culture: culture)); return;
            case "SpanHour1": Assert.Equal(expected, TimeSpan.FromHours(1).Humanize(culture: culture)); return;
            case "SpanHour2": Assert.Equal(expected, TimeSpan.FromHours(2).Humanize(culture: culture)); return;
            case "SpanDay1": Assert.Equal(expected, TimeSpan.FromDays(1).Humanize(culture: culture)); return;
            case "SpanDay2": Assert.Equal(expected, TimeSpan.FromDays(2).Humanize(culture: culture)); return;
            case "SpanZero": Assert.Equal(expected, TimeSpan.Zero.Humanize(culture: culture)); return;
            case "SpanZeroWords": Assert.Equal(expected, TimeSpan.Zero.Humanize(culture: culture, toWords: true)); return;
            default: throw new Xunit.Sdk.XunitException($"Unknown locale resource phrase case '{caseName}'.");
        }
    }
}
