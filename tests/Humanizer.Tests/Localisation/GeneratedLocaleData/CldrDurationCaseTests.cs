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