# Metric Numerals

Humanizer can convert numbers to metric notation (e.g., `1k`, `3.5M`) and parse metric strings back to numbers. This covers the full range of SI prefixes from yocto (10^-24) to yotta (10^24).

## Basic Usage

```csharp
using Humanizer;

1230d.ToMetric()  // => "1.23k"
1000d.ToMetric()  // => "1k"
0.001.ToMetric()  // => "1m"
1E6.ToMetric()    // => "1M"
1E9.ToMetric()    // => "1G"

"1.23k".FromMetric() // => 1230
"100m".FromMetric()  // => 0.1
"1M".FromMetric()    // => 1000000
```

## ToMetric

The `ToMetric` extension method is available on `double`, `int`, and `long`. It accepts optional `formats` and `decimals` parameters:

```csharp
123d.ToMetric()    // => "123"
1230d.ToMetric()   // => "1.23k"
-123456d.ToMetric() // => "-123.456k"
```

### Controlling Decimal Places

Use the `decimals` parameter to control rounding:

```csharp
123456.ToMetric(decimals: 0)  // => "123k"
123456.ToMetric(decimals: 1)  // => "123.5k"
123456.ToMetric(decimals: 2)  // => "123.46k"
123456.ToMetric(decimals: 3)  // => "123.456k"
```

### MetricNumeralFormats

The `formats` parameter is a `[Flags]` enum that controls the output style:

- `MetricNumeralFormats.WithSpace` - Adds a space between the number and the symbol
- `MetricNumeralFormats.UseName` - Uses the SI prefix name instead of the symbol (e.g., "kilo" instead of "k")
- `MetricNumeralFormats.UseShortScaleWord` - Uses the short scale word (e.g., "thousand", "million")
- `MetricNumeralFormats.UseLongScaleWord` - Uses the long scale word (e.g., "thousand", "milliard")

```csharp
1000d.ToMetric(MetricNumeralFormats.WithSpace)
    // => "1 k"

1E-3.ToMetric(MetricNumeralFormats.UseName)
    // => "1milli"

1300000d.ToMetric(MetricNumeralFormats.UseShortScaleWord)
    // => "1.3million"

1300000d.ToMetric(MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord)
    // => "1.3 million"

1230d.ToMetric(MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord)
    // => "1.23 thousand"
```

Flags can be combined for the desired output:

```csharp
1E24.ToMetric(MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseName)
    // => "1 yotta"

1E24.ToMetric(MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseShortScaleWord)
    // => "1 septillion"

1E24.ToMetric(MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseLongScaleWord)
    // => "1 quadrillion"
```

## FromMetric

The `FromMetric` extension method parses a metric string back to a `double`. It accepts both symbol and name formats, with or without spaces:

```csharp
"1.23k".FromMetric()   // => 1230
"1 k".FromMetric()     // => 1000
"1 kilo".FromMetric()  // => 1000
"1milli".FromMetric()  // => 0.001
"100m".FromMetric()    // => 0.1
```

## Supported Symbols

| Symbol | Name   | Short Scale Word | Example           |
|--------|--------|------------------|-------------------|
| k      | kilo   | thousand         | 10^3              |
| M      | mega   | million          | 10^6              |
| G      | giga   | billion          | 10^9              |
| T      | tera   | trillion         | 10^12             |
| P      | peta   | quadrillion      | 10^15             |
| E      | exa    | quintillion      | 10^18             |
| Z      | zetta  | sextillion       | 10^21             |
| Y      | yotta  | septillion       | 10^24             |
| m      | milli  | thousandth       | 10^-3             |
| u      | micro  | millionth        | 10^-6             |
| n      | nano   | billionth        | 10^-9             |
| p      | pico   | trillionth       | 10^-12            |
| f      | femto  | quadrillionth    | 10^-15            |
| a      | atto   | quintillionth    | 10^-18            |
| z      | zepto  | sextillionth     | 10^-21            |
| y      | yocto  | septillionth     | 10^-24            |

## Error Handling

`ToMetric` throws an `ArgumentOutOfRangeException` if the absolute value exceeds 10^27 or falls below 10^-27 (excluding zero):

```csharp
1E+27.ToMetric() // throws ArgumentOutOfRangeException
1E-27.ToMetric() // throws ArgumentOutOfRangeException
```

`FromMetric` throws an `ArgumentException` for empty or invalid metric strings, and an `ArgumentNullException` for null input.

## Related Topics

- [Roman Numerals](roman-numerals.md) - Convert numbers to Roman numerals
- [Number to Numbers](number-to-numbers.md) - Fluent numeric multiplier methods
- [Number to Words](number-to-words.md) - Convert numbers to English words
