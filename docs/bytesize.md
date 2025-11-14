# ByteSize Utilities

`ByteSize` is a struct that represents storage quantities in a human-friendly way. It supports arithmetic, parsing, conversion between units, and formatted output.

## Creating byte sizes

Humanizer exposes fluent helpers for constructing `ByteSize` instances from numeric literals:

```csharp
using Humanizer;

var download = 750.Megabytes();
var cache = 2.Gigabytes();
var total = download + cache;     // 2.732421875 GB
```

You can also create a `ByteSize` from primitive numbers directly:

```csharp
var size = ByteSize.FromKilobytes(512);
size.Bytes;        // 524288
size.Kilobytes;    // 512
size.Megabytes;    // 0.5
```

## Formatting output

`ByteSize` implements both `ToString()` and `Humanize()` with customizable formatting.

```csharp
using Humanizer.Bytes;

var backup = 10.5.Gigabytes();
backup.ToString();                    // "10.5 GB"
backup.ToString("#.## MB");          // "10752 MB"
backup.Humanize("0.00 TB");          // "0.01 TB"
backup.ToFullWords();                 // "ten point five gigabytes"
```

Use `Humanize(string format = null, string symbol = null)` to control the numeric format and the target symbol (`B`, `KB`, `MB`, etc.).

## Parsing input

Convert textual sizes back into strongly typed values with `Parse` or `TryParse`.

```csharp
if (ByteSize.TryParse("1.25 GB", out var value))
{
    // value.Gigabytes == 1.25
}
```

Parsing accepts both SI and IEC-style abbreviations as well as mixed casing (`kb`, `KB`). Humanizer rejects partial bits, so the string `"1.5 b"` is invalid because bits cannot be fractional.

## Calculating transfer rates

Use `Per` to compute throughput from a byte size and a time interval. The result is a `ByteRate` which you can humanize in different units.

```csharp
var payload = 1.5.Gigabytes();
var duration = TimeSpan.FromSeconds(3);
var rate = payload.Per(duration);

rate.Humanize();                       // "512 MB/s"
rate.Humanize(TimeUnit.Minute);        // "30.00 GB/min"
rate.Humanize("#.## TB/hour");        // "1.76 TB/hour"
```

## Tips for production use

- **Avoid double rounding:** Format the output as late as possible, preferably at the presentation layer.
- **Leverage pluralization:** Combine `ByteSize` with `ToQuantity` or `Humanize(LetterCasing)` to integrate into larger sentences.
- **Culture-aware formatting:** Passing a `CultureInfo` to numeric methods ensures decimal separators match user expectations.
