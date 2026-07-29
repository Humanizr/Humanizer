namespace GeneratedLocaleData;

[UseCulture("en-US")]
public class CldrDurationCaseTests
{
    static readonly int[] RepresentativeCounts = [0, 1, 2, 3, 5, 11, 21, 100];

    [Theory]
    [InlineData("da-DK", GrammaticalCase.Genitive, TimeUnit.Day, 1, "1 dags")]
    [InlineData("sv-SE", GrammaticalCase.Genitive, TimeUnit.Day, 1, "1 dygns")]
    [InlineData("nn-NO", GrammaticalCase.Genitive, TimeUnit.Week, 1, "1 vekes")]
    [InlineData("nb-NO", GrammaticalCase.Genitive, TimeUnit.Week, 1, "1 ukes")]
    [InlineData("ro-RO", GrammaticalCase.Genitive, TimeUnit.Day, 1, "unei zile")]
    [InlineData("am-ET", GrammaticalCase.Accusative, TimeUnit.Day, 1, "አንድ ቀን")]
    [InlineData("hi-IN", GrammaticalCase.Oblique, TimeUnit.Day, 1, "1 दिन")]
    [InlineData("pa-IN", GrammaticalCase.Oblique, TimeUnit.Week, 1, "1 ਹਫ਼ਤੇ")]
    [InlineData("az", GrammaticalCase.Dative, TimeUnit.Day, 1, "1 günə")]
    public void PinnedCldrDurationCasesAreCultureSpecific(
        string cultureName,
        GrammaticalCase grammaticalCase,
        TimeUnit unit,
        int count,
        string expected)
    {
        var duration = unit == TimeUnit.Week
            ? TimeSpan.FromDays(count * 7)
            : TimeSpan.FromDays(count);

        Assert.Equal(
            expected,
            duration.HumanizeWithCase(
                grammaticalCase,
                culture: new(cultureName),
                maxUnit: unit,
                minUnit: unit));
    }

    [Fact]
    public void ContextDependentSomaliCaseFailsClearly()
    {
        var exception = Assert.Throws<NotSupportedException>(
            () => TimeSpan.FromDays(1).HumanizeWithCase(
                GrammaticalCase.Nominative,
                culture: new("so"),
                maxUnit: TimeUnit.Day,
                minUnit: TimeUnit.Day));

        Assert.Contains("does not apply", exception.Message);
    }

    [Theory]
    [InlineData(GrammaticalCase.Nominative, TimeUnit.Hour, 0, "0 ساعة")]
    [InlineData(GrammaticalCase.Nominative, TimeUnit.Hour, 1, "ساعة واحدة")]
    [InlineData(GrammaticalCase.Nominative, TimeUnit.Hour, 2, "ساعتان")]
    [InlineData(GrammaticalCase.Genitive, TimeUnit.Hour, 0, "0 ساعة")]
    [InlineData(GrammaticalCase.Genitive, TimeUnit.Hour, 2, "ساعتين")]
    [InlineData(GrammaticalCase.Accusative, TimeUnit.Day, 0, "0 يوم")]
    [InlineData(GrammaticalCase.Accusative, TimeUnit.Hour, 2, "ساعتين")]
    [InlineData(GrammaticalCase.Accusative, TimeUnit.Day, 1, "يوما واحدا")]
    [InlineData(GrammaticalCase.Nominative, TimeUnit.Day, 11, "11 يوما")]
    public void ArabicCaseAndNumeralAgreementUsesAuthoredWholePhrases(
        GrammaticalCase grammaticalCase,
        TimeUnit unit,
        int count,
        string expected)
    {
        var duration = unit == TimeUnit.Hour
            ? TimeSpan.FromHours(count)
            : TimeSpan.FromDays(count);

        Assert.Equal(
            expected,
            duration.HumanizeWithCase(
                grammaticalCase,
                culture: new("ar"),
                maxUnit: unit,
                minUnit: unit));
    }

    [Theory]
    [InlineData(GrammaticalCase.Oblique, TimeUnit.Millisecond, 1, "1 ملي ثانیې")]
    [InlineData(GrammaticalCase.Oblique, TimeUnit.Millisecond, 2, "2 ملي ثانیو")]
    [InlineData(GrammaticalCase.Oblique, TimeUnit.Second, 1, "1 ثانیې")]
    [InlineData(GrammaticalCase.Oblique, TimeUnit.Second, 2, "2 ثانیو")]
    [InlineData(GrammaticalCase.Oblique, TimeUnit.Minute, 1, "1 دقیقې")]
    [InlineData(GrammaticalCase.Oblique, TimeUnit.Minute, 2, "2 دقیقو")]
    [InlineData(GrammaticalCase.Oblique, TimeUnit.Hour, 1, "1 ساعت")]
    [InlineData(GrammaticalCase.Oblique, TimeUnit.Hour, 2, "2 ساعتونو")]
    [InlineData(GrammaticalCase.Oblique, TimeUnit.Day, 1, "1 ورځې")]
    [InlineData(GrammaticalCase.Oblique, TimeUnit.Day, 2, "2 ورځو")]
    [InlineData(GrammaticalCase.Oblique, TimeUnit.Week, 1, "1 اونۍ")]
    [InlineData(GrammaticalCase.Oblique, TimeUnit.Week, 2, "2 اونیو")]
    [InlineData(GrammaticalCase.Oblique, TimeUnit.Month, 1, "1 میاشتې")]
    [InlineData(GrammaticalCase.Oblique, TimeUnit.Month, 2, "2 میاشتو")]
    [InlineData(GrammaticalCase.Oblique, TimeUnit.Year, 1, "1 کال")]
    [InlineData(GrammaticalCase.Oblique, TimeUnit.Year, 2, "2 کالو")]
    [InlineData(GrammaticalCase.Ablative, TimeUnit.Millisecond, 1, "1 ملي ثانیې")]
    [InlineData(GrammaticalCase.Ablative, TimeUnit.Millisecond, 2, "2 ملي ثانیو")]
    [InlineData(GrammaticalCase.Ablative, TimeUnit.Second, 1, "1 ثانیې")]
    [InlineData(GrammaticalCase.Ablative, TimeUnit.Second, 2, "2 ثانیو")]
    [InlineData(GrammaticalCase.Ablative, TimeUnit.Minute, 1, "1 دقیقې")]
    [InlineData(GrammaticalCase.Ablative, TimeUnit.Minute, 2, "2 دقیقو")]
    [InlineData(GrammaticalCase.Ablative, TimeUnit.Hour, 1, "1 ساعته")]
    [InlineData(GrammaticalCase.Ablative, TimeUnit.Hour, 2, "2 ساعتونو")]
    [InlineData(GrammaticalCase.Ablative, TimeUnit.Day, 1, "1 ورځې")]
    [InlineData(GrammaticalCase.Ablative, TimeUnit.Day, 2, "2 ورځو")]
    [InlineData(GrammaticalCase.Ablative, TimeUnit.Week, 1, "1 اونۍ")]
    [InlineData(GrammaticalCase.Ablative, TimeUnit.Week, 2, "2 اونیو")]
    [InlineData(GrammaticalCase.Ablative, TimeUnit.Month, 1, "1 میاشتې")]
    [InlineData(GrammaticalCase.Ablative, TimeUnit.Month, 2, "2 میاشتو")]
    [InlineData(GrammaticalCase.Ablative, TimeUnit.Year, 1, "1 کاله")]
    [InlineData(GrammaticalCase.Ablative, TimeUnit.Year, 2, "2 کالو")]
    public void PashtoObliqueAndAblativeUseTheirDistinctNounForms(
        GrammaticalCase grammaticalCase,
        TimeUnit unit,
        int count,
        string expected)
    {
        IGrammaticalCaseTimeSpanFormatter formatter = new DefaultFormatter("ps");

        Assert.Equal(
            expected,
            formatter.TimeSpanHumanize(unit, count, grammaticalCase));
    }

    [Theory]
    [InlineData(TimeUnit.Millisecond, "2 ملی سیکنڈ")]
    [InlineData(TimeUnit.Second, "2 سیکنڈ")]
    [InlineData(TimeUnit.Minute, "2 منٹ")]
    [InlineData(TimeUnit.Hour, "2 گھنٹے")]
    [InlineData(TimeUnit.Day, "2 دن")]
    [InlineData(TimeUnit.Week, "2 ہفتے")]
    [InlineData(TimeUnit.Month, "2 مہینے")]
    [InlineData(TimeUnit.Year, "2 سال")]
    public void UrduExplicitlyCountedTimeWordsDoNotTakeTheGenericObliquePlural(
        TimeUnit unit,
        string expected)
    {
        IGrammaticalCaseTimeSpanFormatter formatter = new DefaultFormatter("ur");

        Assert.Equal(
            expected,
            formatter.TimeSpanHumanize(unit, 2, GrammaticalCase.Oblique));
    }

    [Theory]
    [InlineData(GrammaticalCase.Genitive, TimeUnit.Millisecond, "1 মিলিসেকেন্ডের")]
    [InlineData(GrammaticalCase.Genitive, TimeUnit.Second, "1 সেকেন্ডের")]
    [InlineData(GrammaticalCase.Genitive, TimeUnit.Minute, "1 মিনিটের")]
    [InlineData(GrammaticalCase.Genitive, TimeUnit.Hour, "1 ঘণ্টার")]
    [InlineData(GrammaticalCase.Genitive, TimeUnit.Day, "1 দিনের")]
    [InlineData(GrammaticalCase.Genitive, TimeUnit.Week, "1 সপ্তাহের")]
    [InlineData(GrammaticalCase.Genitive, TimeUnit.Month, "1 মাসের")]
    [InlineData(GrammaticalCase.Genitive, TimeUnit.Year, "1 বছরের")]
    [InlineData(GrammaticalCase.Locative, TimeUnit.Millisecond, "1 মিলিসেকেন্ডে")]
    [InlineData(GrammaticalCase.Locative, TimeUnit.Second, "1 সেকেন্ডে")]
    [InlineData(GrammaticalCase.Locative, TimeUnit.Minute, "1 মিনিটে")]
    [InlineData(GrammaticalCase.Locative, TimeUnit.Hour, "1 ঘণ্টায়")]
    [InlineData(GrammaticalCase.Locative, TimeUnit.Day, "1 দিনে")]
    [InlineData(GrammaticalCase.Locative, TimeUnit.Week, "1 সপ্তাহে")]
    [InlineData(GrammaticalCase.Locative, TimeUnit.Month, "1 মাসে")]
    [InlineData(GrammaticalCase.Locative, TimeUnit.Year, "1 বছরে")]
    public void BanglaProductiveCaseAllomorphsMatchEveryDurationStem(
        GrammaticalCase grammaticalCase,
        TimeUnit unit,
        string expected)
    {
        IGrammaticalCaseTimeSpanFormatter formatter = new DefaultFormatter("bn");

        Assert.Equal(
            expected,
            formatter.TimeSpanHumanize(unit, 1, grammaticalCase));
    }

    [Theory]
    [InlineData("be", GrammaticalCase.Genitive, TimeUnit.Day, 1, "1 дня")]
    [InlineData("bs", GrammaticalCase.Instrumental, TimeUnit.Week, 1, "1 sedmicom")]
    [InlineData("et", GrammaticalCase.Inessive, TimeUnit.Day, 1, "1 päevas")]
    [InlineData("eu", GrammaticalCase.Ergative, TimeUnit.Day, 1, "1 egunek")]
    [InlineData("ga", GrammaticalCase.Genitive, TimeUnit.Day, 1, "1 lae")]
    [InlineData("gu", GrammaticalCase.Locative, TimeUnit.Day, 1, "એક દિવસે")]
    [InlineData("gu", GrammaticalCase.Locative, TimeUnit.Week, 1, "એક અઠવાડિયામાં")]
    [InlineData("ka", GrammaticalCase.Ergative, TimeUnit.Day, 1, "1 დღემ")]
    [InlineData("bn", GrammaticalCase.Genitive, TimeUnit.Minute, 1, "1 মিনিটের")]
    [InlineData("ne", GrammaticalCase.Ablative, TimeUnit.Day, 1, "एक दिनबाट")]
    [InlineData("or", GrammaticalCase.Dative, TimeUnit.Minute, 1, "ଏକ ମିନିଟ୍‌କୁ")]
    [InlineData("or", GrammaticalCase.Ablative, TimeUnit.Minute, 1, "ଏକ ମିନିଟ୍‌ରୁ")]
    [InlineData("or", GrammaticalCase.Locative, TimeUnit.Minute, 1, "ଏକ ମିନିଟ୍‌ରେ")]
    [InlineData("si", GrammaticalCase.Dative, TimeUnit.Day, 1, "එක් දිනයකට")]
    [InlineData("sq", GrammaticalCase.Dative, TimeUnit.Day, 1, "1 dite")]
    [InlineData("sr", GrammaticalCase.Instrumental, TimeUnit.Day, 1, "1 даном")]
    [InlineData("sr-Latn", GrammaticalCase.Instrumental, TimeUnit.Day, 1, "1 danom")]
    [InlineData("ur", GrammaticalCase.Oblique, TimeUnit.Hour, 2, "2 گھنٹے")]
    public void SourcedAuthoredCaseFamiliesUseExactNativeForms(
        string cultureName,
        GrammaticalCase grammaticalCase,
        TimeUnit unit,
        int count,
        string expected)
    {
        IGrammaticalCaseTimeSpanFormatter formatter = new DefaultFormatter(cultureName);

        Assert.Equal(
            expected,
            formatter.TimeSpanHumanize(unit, count, grammaticalCase));
    }

    [Theory]
    [InlineData("az", TimeUnit.Year, "bir il")]
    [InlineData("cs", TimeUnit.Hour, "jedna hodina")]
    [InlineData("el", TimeUnit.Hour, "μία ώρα")]
    [InlineData("el", TimeUnit.Month, "ένας μήνας")]
    [InlineData("el", TimeUnit.Year, "ένα έτος")]
    [InlineData("hr", TimeUnit.Year, "jedna godina")]
    [InlineData("lv", TimeUnit.Month, "viens mēnesis")]
    [InlineData("pl", TimeUnit.Day, "jeden dzień")]
    [InlineData("sk", TimeUnit.Minute, "jedna minúta")]
    [InlineData("sl", TimeUnit.Year, "eno leto")]
    [InlineData("sr-Latn", TimeUnit.Week, "jedna nedelja")]
    [InlineData("sr", TimeUnit.Millisecond, "једна милисекунда")]
    [InlineData("sv", TimeUnit.Week, "en vecka")]
    [InlineData("uz-Cyrl-UZ", TimeUnit.Second, "бир сония")]
    [InlineData("uz-Latn-UZ", TimeUnit.Second, "bir sekund")]
    public void CitationCaseSingularsNeverFallBackToEnglish(
        string cultureName,
        TimeUnit unit,
        string expected)
    {
        IGrammaticalCaseTimeSpanFormatter formatter = new DefaultFormatter(cultureName);

        Assert.Equal(
            expected,
            formatter.TimeSpanHumanize(unit, 1, GrammaticalCase.Nominative));
    }

    [Theory]
    [InlineData("bs", TimeUnit.Millisecond, "21 milisekunda")]
    [InlineData("bs", TimeUnit.Second, "21 sekunda")]
    [InlineData("bs", TimeUnit.Minute, "21 minuta")]
    [InlineData("bs", TimeUnit.Hour, "21 sat")]
    [InlineData("bs", TimeUnit.Day, "21 dan")]
    [InlineData("bs", TimeUnit.Week, "21 sedmica")]
    [InlineData("bs", TimeUnit.Month, "21 mjesec")]
    [InlineData("bs", TimeUnit.Year, "21 godina")]
    [InlineData("hr", TimeUnit.Millisecond, "21 milisekunda")]
    [InlineData("hr", TimeUnit.Second, "21 sekunda")]
    [InlineData("hr", TimeUnit.Minute, "21 minuta")]
    [InlineData("hr", TimeUnit.Hour, "21 sat")]
    [InlineData("hr", TimeUnit.Day, "21 dan")]
    [InlineData("hr", TimeUnit.Week, "21 tjedan")]
    [InlineData("hr", TimeUnit.Month, "21 mjesec")]
    [InlineData("hr", TimeUnit.Year, "21 godina")]
    [InlineData("sr", TimeUnit.Millisecond, "21 милисекунда")]
    [InlineData("sr", TimeUnit.Second, "21 секунда")]
    [InlineData("sr", TimeUnit.Minute, "21 минут")]
    [InlineData("sr", TimeUnit.Hour, "21 сат")]
    [InlineData("sr", TimeUnit.Day, "21 дан")]
    [InlineData("sr", TimeUnit.Week, "21 недеља")]
    [InlineData("sr", TimeUnit.Month, "21 месец")]
    [InlineData("sr", TimeUnit.Year, "21 година")]
    [InlineData("sr-Latn", TimeUnit.Millisecond, "21 milisekunda")]
    [InlineData("sr-Latn", TimeUnit.Second, "21 sekunda")]
    [InlineData("sr-Latn", TimeUnit.Minute, "21 minut")]
    [InlineData("sr-Latn", TimeUnit.Hour, "21 sat")]
    [InlineData("sr-Latn", TimeUnit.Day, "21 dan")]
    [InlineData("sr-Latn", TimeUnit.Week, "21 nedelja")]
    [InlineData("sr-Latn", TimeUnit.Month, "21 mesec")]
    [InlineData("sr-Latn", TimeUnit.Year, "21 godina")]
    public void SouthSlavicCitationCasesUseNominativeFormsForCountsEndingInOne(
        string cultureName,
        TimeUnit unit,
        string expected)
    {
        IGrammaticalCaseTimeSpanFormatter formatter = new DefaultFormatter(cultureName);

        Assert.Equal(
            expected,
            formatter.TimeSpanHumanize(unit, 21, GrammaticalCase.Nominative));
    }

    [Theory]
    [InlineData("bs", TimeUnit.Hour, 1, "jedan sat")]
    [InlineData("bs", TimeUnit.Hour, 2, "2 sata")]
    [InlineData("bs", TimeUnit.Hour, 4, "4 sata")]
    [InlineData("bs", TimeUnit.Hour, 5, "5 sati")]
    [InlineData("bs", TimeUnit.Hour, 11, "11 sati")]
    [InlineData("bs", TimeUnit.Hour, 21, "21 sat")]
    [InlineData("bs", TimeUnit.Hour, 22, "22 sata")]
    [InlineData("bs", TimeUnit.Hour, 25, "25 sati")]
    [InlineData("bs", TimeUnit.Hour, 101, "101 sat")]
    [InlineData("hr", TimeUnit.Hour, 1, "jedan sat")]
    [InlineData("hr", TimeUnit.Hour, 2, "2 sata")]
    [InlineData("hr", TimeUnit.Hour, 4, "4 sata")]
    [InlineData("hr", TimeUnit.Hour, 5, "5 sati")]
    [InlineData("hr", TimeUnit.Hour, 11, "11 sati")]
    [InlineData("hr", TimeUnit.Hour, 21, "21 sat")]
    [InlineData("hr", TimeUnit.Hour, 22, "22 sata")]
    [InlineData("hr", TimeUnit.Hour, 25, "25 sati")]
    [InlineData("hr", TimeUnit.Hour, 101, "101 sat")]
    [InlineData("hr", TimeUnit.Day, 2, "2 dana")]
    [InlineData("hr", TimeUnit.Day, 4, "4 dana")]
    [InlineData("hr", TimeUnit.Day, 22, "22 dana")]
    [InlineData("sr", TimeUnit.Month, 1, "један месец")]
    [InlineData("sr", TimeUnit.Month, 2, "2 месеца")]
    [InlineData("sr", TimeUnit.Month, 4, "4 месеца")]
    [InlineData("sr", TimeUnit.Month, 5, "5 месеци")]
    [InlineData("sr", TimeUnit.Month, 11, "11 месеци")]
    [InlineData("sr", TimeUnit.Month, 21, "21 месец")]
    [InlineData("sr", TimeUnit.Month, 22, "22 месеца")]
    [InlineData("sr", TimeUnit.Month, 25, "25 месеци")]
    [InlineData("sr", TimeUnit.Month, 101, "101 месец")]
    [InlineData("sr", TimeUnit.Minute, 2, "2 минута")]
    [InlineData("sr", TimeUnit.Minute, 4, "4 минута")]
    [InlineData("sr", TimeUnit.Minute, 22, "22 минута")]
    [InlineData("sr", TimeUnit.Day, 2, "2 дана")]
    [InlineData("sr", TimeUnit.Day, 4, "4 дана")]
    [InlineData("sr", TimeUnit.Day, 22, "22 дана")]
    [InlineData("sr-Latn", TimeUnit.Second, 1, "jedna sekunda")]
    [InlineData("sr-Latn", TimeUnit.Second, 2, "2 sekunde")]
    [InlineData("sr-Latn", TimeUnit.Second, 4, "4 sekunde")]
    [InlineData("sr-Latn", TimeUnit.Second, 5, "5 sekundi")]
    [InlineData("sr-Latn", TimeUnit.Second, 11, "11 sekundi")]
    [InlineData("sr-Latn", TimeUnit.Second, 21, "21 sekunda")]
    [InlineData("sr-Latn", TimeUnit.Second, 22, "22 sekunde")]
    [InlineData("sr-Latn", TimeUnit.Second, 25, "25 sekundi")]
    [InlineData("sr-Latn", TimeUnit.Second, 101, "101 sekunda")]
    [InlineData("sr-Latn", TimeUnit.Minute, 2, "2 minuta")]
    [InlineData("sr-Latn", TimeUnit.Minute, 4, "4 minuta")]
    [InlineData("sr-Latn", TimeUnit.Minute, 22, "22 minuta")]
    [InlineData("sr-Latn", TimeUnit.Day, 2, "2 dana")]
    [InlineData("sr-Latn", TimeUnit.Day, 4, "4 dana")]
    [InlineData("sr-Latn", TimeUnit.Day, 22, "22 dana")]
    public void SouthSlavicCitationCasesUseModuloNumberForms(
        string cultureName,
        TimeUnit unit,
        int count,
        string expected)
    {
        IGrammaticalCaseTimeSpanFormatter formatter = new DefaultFormatter(cultureName);

        Assert.Equal(
            expected,
            formatter.TimeSpanHumanize(unit, count, GrammaticalCase.Nominative));
    }

    [Theory]
    [InlineData(TimeUnit.Millisecond, 3, "3 milisekunde")]
    [InlineData(TimeUnit.Millisecond, 5, "5 milisekundi")]
    [InlineData(TimeUnit.Millisecond, 21, "21 milisekundom")]
    [InlineData(TimeUnit.Second, 3, "3 sekunde")]
    [InlineData(TimeUnit.Second, 5, "5 sekundi")]
    [InlineData(TimeUnit.Second, 21, "21 sekundom")]
    [InlineData(TimeUnit.Minute, 3, "3 minute")]
    [InlineData(TimeUnit.Minute, 5, "5 minuta")]
    [InlineData(TimeUnit.Minute, 21, "21 minutom")]
    [InlineData(TimeUnit.Hour, 3, "3 sata")]
    [InlineData(TimeUnit.Hour, 5, "5 sati")]
    [InlineData(TimeUnit.Hour, 21, "21 satom")]
    [InlineData(TimeUnit.Day, 3, "3 dana")]
    [InlineData(TimeUnit.Day, 5, "5 dana")]
    [InlineData(TimeUnit.Day, 21, "21 danom")]
    [InlineData(TimeUnit.Week, 3, "3 tjedna")]
    [InlineData(TimeUnit.Week, 5, "5 tjedana")]
    [InlineData(TimeUnit.Week, 21, "21 tjednom")]
    [InlineData(TimeUnit.Month, 3, "3 mjeseca")]
    [InlineData(TimeUnit.Month, 5, "5 mjeseci")]
    [InlineData(TimeUnit.Month, 21, "21 mjesecom")]
    [InlineData(TimeUnit.Year, 3, "3 godine")]
    [InlineData(TimeUnit.Year, 5, "5 godina")]
    [InlineData(TimeUnit.Year, 21, "21 godinom")]
    public void CroatianInstrumentalUsesNumeralGovernedForms(
        TimeUnit unit,
        int count,
        string expected)
    {
        IGrammaticalCaseTimeSpanFormatter formatter = new DefaultFormatter("hr");

        Assert.Equal(
            expected,
            formatter.TimeSpanHumanize(unit, count, GrammaticalCase.Instrumental));
    }

    [Theory]
    [InlineData("sr", TimeUnit.Hour, 3, "3 сата")]
    [InlineData("sr", TimeUnit.Hour, 5, "5 сати")]
    [InlineData("sr", TimeUnit.Hour, 21, "21 сатом")]
    [InlineData("sr", TimeUnit.Day, 3, "3 дана")]
    [InlineData("sr", TimeUnit.Day, 5, "5 дана")]
    [InlineData("sr", TimeUnit.Day, 21, "21 даном")]
    [InlineData("sr-Latn", TimeUnit.Hour, 3, "3 sata")]
    [InlineData("sr-Latn", TimeUnit.Hour, 5, "5 sati")]
    [InlineData("sr-Latn", TimeUnit.Hour, 21, "21 satom")]
    [InlineData("sr-Latn", TimeUnit.Day, 3, "3 dana")]
    [InlineData("sr-Latn", TimeUnit.Day, 5, "5 dana")]
    [InlineData("sr-Latn", TimeUnit.Day, 21, "21 danom")]
    public void SerbianInstrumentalUsesNumeralGovernedForms(
        string cultureName,
        TimeUnit unit,
        int count,
        string expected)
    {
        IGrammaticalCaseTimeSpanFormatter formatter = new DefaultFormatter(cultureName);

        Assert.Equal(
            expected,
            formatter.TimeSpanHumanize(unit, count, GrammaticalCase.Instrumental));
    }

    [Fact]
    public void EverySupportedLocaleCaseAndUnitFormatsEveryReachableCountFamily()
    {
        foreach (var localeCode in LocaleDurationCaseTableCatalog.LocaleCodes)
        {
            var table = LocaleDurationCaseTableCatalog.ResolveCore(localeCode)!;
            if (table.Classification is LocaleDurationCaseClassification.NotApplicable)
            {
                continue;
            }

            IGrammaticalCaseTimeSpanFormatter formatter = new DefaultFormatter(localeCode);
            foreach (var grammaticalCase in Enum.GetValues<GrammaticalCase>())
            {
                if (!table.TryGetCase(grammaticalCase, out var durationCase))
                {
                    continue;
                }

                for (var unitIndex = 0; unitIndex < durationCase.Units.Length; unitIndex++)
                {
                    var unit = durationCase.Units[unitIndex];
                    Assert.NotEqual(LocalizedDurationCaseUnitKind.Unsupported, unit.Kind);
                    if (unit.Kind is LocalizedDurationCaseUnitKind.NotApplicable)
                    {
                        continue;
                    }

                    foreach (var count in RepresentativeCounts)
                    {
                        var result = formatter.TimeSpanHumanize(
                            (TimeUnit)unitIndex,
                            count,
                            grammaticalCase);

                        Assert.False(string.IsNullOrWhiteSpace(result));
                        Assert.DoesNotContain("{count}", result, StringComparison.Ordinal);
                        if (!localeCode.Equals("en", StringComparison.OrdinalIgnoreCase))
                            Assert.DoesNotContain("one ", result, StringComparison.OrdinalIgnoreCase);
                    }
                }
            }
        }
    }
}