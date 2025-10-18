using BenchmarkDotNet.Attributes;
using Humanizer;

namespace Benchmarks;

/// <summary>
/// Benchmarks for StringHumanizeExtensions - demonstrates source-generated regex performance
/// </summary>
[MemoryDiagnoser]
public class StringHumanizeBenchmarks
{
    [Benchmark(Description = "Humanize PascalCase")]
    public string HumanizePascalCase() =>
        "PascalCaseInputStringToBeHumanized".Humanize();

    [Benchmark(Description = "Humanize with underscore")]
    public string HumanizeUnderscore() =>
        "Underscored_input_string_is_turned_INTO_sentence".Humanize();

    [Benchmark(Description = "Humanize with dashes")]
    public string HumanizeDashes() =>
        "Dashed-input-string-is-turned-INTO-sentence".Humanize();

    [Benchmark(Description = "Humanize with casing")]
    public string HumanizeWithCasing() =>
        "PascalCaseInputString".Humanize(LetterCasing.AllCaps);

    [Benchmark(Description = "Humanize mixed format")]
    public string HumanizeMixed() =>
        "HTML_to_JSON_Converter".Humanize();
}
