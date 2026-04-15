namespace Benchmarks;

/// <summary>
/// Benchmarks the generated formatter/profile pipeline: registry resolution, date humanization,
/// timespan humanization, and profile-driven data-unit formatting.
/// </summary>
[MemoryDiagnoser]
public class FormatterBenchmarks
{
    static readonly CultureInfo arabicCulture = new("ar");
    static readonly CultureInfo romanianCulture = new("ro");
    static readonly CultureInfo russianCulture = new("ru");
    static readonly DateTime referenceUtc = new(2026, 3, 31, 12, 0, 0, DateTimeKind.Utc);
    static readonly DateTime twoHoursEarlierUtc = referenceUtc.AddHours(-2);
    static readonly TimeSpan russianSample = TimeSpan.FromDays(3) + TimeSpan.FromHours(4) + TimeSpan.FromMinutes(12);
    static readonly IFormatter arabicFormatter = Configurator.Formatters.ResolveForCulture(arabicCulture);

    [Benchmark(Description = "Formatter registry first-touch")]
    public IFormatter FormatterRegistryFirstTouch() =>
        new FormatterRegistry().ResolveForCulture(romanianCulture);

    [Benchmark(Description = "Romanian Date Humanize")]
    public string RomanianDateHumanize() =>
        twoHoursEarlierUtc.Humanize(dateToCompareAgainst: referenceUtc, culture: romanianCulture);

    [Benchmark(Description = "Russian TimeSpan Humanize")]
    public string RussianTimeSpanHumanize() =>
        russianSample.Humanize(culture: russianCulture);

    [Benchmark(Description = "Arabic DataUnitHumanize")]
    public string ArabicDataUnitHumanize() =>
        arabicFormatter.DataUnitHumanize(DataUnit.Gigabyte, 2, toSymbol: false);
}