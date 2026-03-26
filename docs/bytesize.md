# ByteSize

Humanizer includes a port of the [ByteSize](https://github.com/omar/ByteSize) library for representing and manipulating byte quantities. It provides a fluent API for creating, formatting, and performing arithmetic on data sizes from bits to terabytes.

## Basic Usage

```csharp
using Humanizer;

var fileSize = (10).Kilobytes();

fileSize.Bits      // => 81920
fileSize.Bytes     // => 10240
fileSize.Kilobytes // => 10
fileSize.Megabytes // => 0.009765625
fileSize.Gigabytes // => 9.53674316e-6
fileSize.Terabytes // => 9.31322575e-9
```

## Creating ByteSize Instances

Extension methods on numeric types convert numbers into `ByteSize` instances:

```csharp
3.Bits();
5.Bytes();
(10.5).Kilobytes();
(2.5).Megabytes();
(10.2).Gigabytes();
(4.7).Terabytes();
```

You can also use static factory methods:

```csharp
ByteSize.FromBits(81920);
ByteSize.FromBytes(10240);
ByteSize.FromKilobytes(10);
ByteSize.FromMegabytes(0.5);
ByteSize.FromGigabytes(1.2);
ByteSize.FromTerabytes(4.7);
```

The extension methods work on `byte`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, and `double` types.

## Properties

A `ByteSize` instance exposes the value in every unit simultaneously:

| Property | Type | Description |
|----------|------|-------------|
| `Bits` | `long` | Value in bits |
| `Bytes` | `double` | Value in bytes |
| `Kilobytes` | `double` | Value in kilobytes |
| `Megabytes` | `double` | Value in megabytes |
| `Gigabytes` | `double` | Value in gigabytes |
| `Terabytes` | `double` | Value in terabytes |

Two convenience properties return the largest unit where the value is at least 1:

```csharp
var maxFileSize = (10).Kilobytes();

maxFileSize.LargestWholeNumberSymbol // => "KB"
maxFileSize.LargestWholeNumberValue  // => 10
```

## Arithmetic

Use `+` and `-` operators or `Add` and `Subtract` methods to combine `ByteSize` values:

```csharp
var total = (10).Gigabytes() + (512).Megabytes() - (2.5).Gigabytes();
total.Subtract((2500).Kilobytes()).Add((25).Megabytes());
```

The `++` and `--` operators increment and decrement by one byte. Comparison operators (`==`, `!=`, `<`, `<=`, `>`, `>=`) are also supported.

## ToString and Humanize

Call `ToString()` or `Humanize()` to get a human-readable string. Both methods behave identically:

```csharp
7.Bits().ToString();           // => "7 b"
8.Bits().ToString();           // => "1 B"
(.5).Kilobytes().Humanize();   // => "512 B"
(1000).Kilobytes().ToString(); // => "1000 KB"
(1024).Kilobytes().Humanize(); // => "1 MB"
(.5).Gigabytes().Humanize();   // => "512 MB"
(1024).Gigabytes().ToString(); // => "1 TB"
```

### Custom Format Strings

Provide a format string to control the output. The formatter can contain a unit symbol (`b`, `B`, `KB`, `MB`, `GB`, `TB`) to force a specific unit. The number portion uses the standard [`double.ToString`](https://docs.microsoft.com/dotnet/api/system.double.tostring) format, with `#.##` as the default:

```csharp
var b = (10.505).Kilobytes();

// Default number format is #.##
b.ToString("KB");         // => "10.52 KB"
b.Humanize("MB");         // => ".01 MB"
b.Humanize("b");          // => "86057 b"

// Default symbol is the largest metric prefix value >= 1
b.ToString("#.#");        // => "10.5 KB"

// All valid double.ToString formats are acceptable
b.ToString("0.0000");     // => "10.5050 KB"
b.Humanize("000.00");     // => "010.51 KB"

// Combine number format and symbol
b.ToString("#.#### MB");  // => ".0103 MB"
b.Humanize("0.00 GB");    // => "0 GB"
b.Humanize("#.## B");     // => "10757.12 B"
```

## ToFullWords

Call `ToFullWords()` for a string representation using full unit names instead of symbols:

```csharp
7.Bits().ToFullWords();           // => "7 bits"
8.Bits().ToFullWords();           // => "1 byte"
(.5).Kilobytes().ToFullWords();   // => "512 bytes"
(1000).Kilobytes().ToFullWords(); // => "1000 kilobytes"
(1024).Kilobytes().ToFullWords(); // => "1 megabyte"
(.5).Gigabytes().ToFullWords();   // => "512 megabytes"
(1024).Gigabytes().ToFullWords(); // => "1 terabyte"
```

## Parse and TryParse

Convert string representations back into `ByteSize` instances using `Parse` or `TryParse`. There is no `Dehumanize` method, but these static methods serve the same purpose:

```csharp
ByteSize.TryParse("1.5mb", out var output);

// Valid inputs
ByteSize.Parse("5b");
ByteSize.Parse("1.55B");
ByteSize.Parse("1.55KB");
ByteSize.Parse("1.55 kB ");  // spaces are trimmed
ByteSize.Parse("1.55 kb");
ByteSize.Parse("1.55 MB");
ByteSize.Parse("1.55 mB");
ByteSize.Parse("1.55 mb");
ByteSize.Parse("1.55 GB");
ByteSize.Parse("1.55 gB");
ByteSize.Parse("1.55 gb");
ByteSize.Parse("1.55 TB");
ByteSize.Parse("1.55 tB");
ByteSize.Parse("1.55 tb");

// Invalid: partial bits are not allowed
ByteSize.Parse("1.5 b"); // throws FormatException
```

`TryParse` returns a `bool` indicating success and outputs the result via the `out` parameter. `Parse` throws a `FormatException` on invalid input. Both methods accept an optional `IFormatProvider` for culture-specific number parsing.

## ByteRate

To calculate the rate at which bytes are transferred, use the `Per` extension method on a `ByteSize` instance. It accepts a `TimeSpan` representing the measurement interval and returns a `ByteRate`.

```csharp
var size = ByteSize.FromMegabytes(10);
var interval = TimeSpan.FromSeconds(1);

size.Per(interval).Humanize();                  // => "10 MB/s"
size.Per(interval).Humanize(TimeUnit.Minute);   // => "600 MB/min"
size.Per(interval).Humanize(TimeUnit.Hour);     // => "35.15625 GB/hour"
```

You can specify a format string for the bytes portion of the output:

```csharp
19854651984.Bytes().Per(1.Seconds()).Humanize("#.##");
// => "18.49 GB/s"
```

Valid time units for `Humanize` are `TimeUnit.Second`, `TimeUnit.Minute`, and `TimeUnit.Hour`. The default is `TimeUnit.Second`.

## Related Topics

- [Time Unit Symbols](time-unit-symbols.md) - TimeUnit.ToSymbol used in ByteRate output
- [Localization](localization.md) - Culture-specific formatting
