# Humanizer Benchmarks

This project contains comprehensive benchmarks for the Humanizer library, demonstrating performance improvements from recent optimizations.

## Running the Benchmarks

### Locally

Run all benchmarks:
```bash
cd src/Benchmarks
dotnet run -c Release -- --filter *
```

Run specific benchmarks:
```bash
dotnet run -c Release -- --filter *StringHumanize*
```

### Baseline Comparison (GitHub Actions)

A manual GitHub Actions workflow is available to compare performance between a baseline NuGet package version and the current source code:

1. Go to **Actions** → **Benchmark Baseline vs Current**
2. Click **Run workflow**
3. Enter the baseline version (default: 2.14.1)
4. The workflow will:
   - Run benchmarks against the baseline package in parallel
   - Run benchmarks against the current source code in parallel
   - Compare results using ResultsComparer
   - Publish detailed reports as artifacts
   - Display results and comparison in the job summary

**How it works:**
- **Baseline run**: Builds benchmarks with `UseBaselinePackage=true` to reference the NuGet package
- **Current run**: Builds benchmarks with `UseBaselinePackage=false` to reference the local source code
- **Comparison**: Downloads both JSON results and uses `dotnet/performance` ResultsComparer tool to generate a diff table

**Artifacts available after each run:**
- `humanizer-bdn-baseline-json` - Full JSON results from baseline
- `humanizer-bdn-current-json` - Full JSON results from current code
- `humanizer-bdn-baseline-all` - Complete BenchmarkDotNet artifacts (baseline)
- `humanizer-bdn-current-all` - Complete BenchmarkDotNet artifacts (current)
- `humanizer-bdn-comparison` - ResultsComparer diff report

## Benchmark Suites

### 1. StringHumanizeBenchmarks
**Optimizations tested**: Source-generated regex (NET7+)

Tests the performance of string humanization operations which use regex patterns to split and transform strings:
- PascalCase → "Pascal case"
- underscore_case → "Underscore case"
- Mixed formats with special handling

**Expected improvements**: 5-10x faster on .NET 7+ with zero allocations

### 2. InflectorBenchmarks
**Optimizations tested**: Source-generated regex (NET7+)

Tests various string transformation methods that rely on regex patterns:
- `Pascalize()` - converts to PascalCase
- `Camelize()` - converts to camelCase
- `Underscore()` - converts to underscore_case
- `Kebaberize()` - converts to kebab-case
- `Titleize()` - converts to Title Case
- `Pluralize()` / `Singularize()` - word pluralization

**Expected improvements**: 5-10x faster on .NET 7+ with zero allocations

### 3. RomanNumeralBenchmarks
**Optimizations tested**: Source-generated regex (NET7+)

Tests conversion between integers and Roman numerals:
- `ToRoman()` - converts integers to Roman numerals
- `FromRoman()` - converts Roman numerals to integers (with regex validation)

**Expected improvements**: 5-10x faster validation on .NET 7+

### 4. NumberToWordsBenchmarks
**Optimizations tested**: FrozenDictionary (all frameworks)

Tests number-to-words conversion across multiple locales:
- English, Turkish, Greek, Korean, Finnish locales
- Small and large numbers
- Ordinal words

**Expected improvements**: 2-3x faster lookups with reduced memory usage

### 5. WordsToNumberBenchmarks
**Optimizations tested**: FrozenDictionary + source-generated regex

Tests converting word representations to numbers:
- Simple words ("five")
- Compound words ("forty-two")
- Complex numbers ("one thousand two hundred thirty-four")

**Expected improvements**: Combined benefits of both optimizations

### 6. MetricNumeralBenchmarks
**Optimizations tested**: FrozenDictionary (all frameworks)

Tests metric prefix conversions:
- `ToMetric()` - converts numbers to metric notation (1000 → "1k")
- `FromMetric()` - parses metric notation back to numbers

**Expected improvements**: 2-3x faster lookups

### 7. OrdinalBenchmarks
**Optimizations tested**: SearchValues (NET8+), FrozenDictionary

Tests ordinal number generation across locales:
- Dutch (uses SearchValues for character set matching)
- Turkish, Greek, Finnish (use FrozenDictionary)

**Expected improvements**: 
- Dutch: 2-5x faster on .NET 8+ (hardware-accelerated)
- Others: 2-3x faster lookups

### 8. VocabularyBenchmarks
**Optimizations tested**: Source-generated regex (NET7+)

Tests pluralization/singularization with vocabulary rules:
- Batch operations on multiple words
- Common and irregular words

**Expected improvements**: 5-10x faster on .NET 7+

### 9. EnumBenchmarks (existing)
Tests enum humanization and dehumanization

### 10. EnglishArticleBenchmarks (existing)
Tests English article sorting operations

### 11. EnglishToWordsBenchmark (existing)
Tests English number-to-words conversion

### 12. TransformersBenchmarks (existing)
Tests string transformation operations

## Performance Optimization Summary

| Optimization | Target Framework | Typical Improvement | Benchmarks |
|--------------|------------------|---------------------|------------|
| Source-Generated Regex | .NET 7+ | 5-10x faster, zero allocations | StringHumanize, Inflector, RomanNumeral, Vocabulary, WordsToNumber |
| FrozenDictionary | All (via polyfill) | 2-3x faster lookups | NumberToWords, MetricNumeral, Ordinal, WordsToNumber |
| SearchValues | .NET 8+ | 2-5x faster (hardware-accelerated) | Ordinal (Dutch) |

## Baseline Comparison

To compare against the pre-optimization performance, you can:

1. Check out the commit before optimizations: `git checkout 97432f62`
2. Run benchmarks and save results
3. Return to current branch: `git checkout -`
4. Run benchmarks again and compare

Or use BenchmarkDotNet's baseline feature:

```csharp
[Benchmark(Baseline = true)]
public string OldImplementation() { ... }

[Benchmark]
public string NewImplementation() { ... }
```

## Memory Diagnostics

All benchmarks include `[MemoryDiagnoser]` to track allocations. The optimizations significantly reduce or eliminate allocations in hot paths:

- Regex operations: Zero allocations on .NET 7+ (source-generated)
- Dictionary lookups: Reduced memory footprint (FrozenDictionary)
- Character searches: Zero allocations on .NET 8+ (SearchValues)

## Notes

- Benchmarks run on .NET 10.0 to demonstrate maximum performance
- Performance improvements are most significant on modern frameworks
- Older frameworks (.NET 4.8, netstandard2.0) still benefit from FrozenDictionary via polyfills
- Results may vary based on hardware and workload characteristics
