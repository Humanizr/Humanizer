# Time Unit Symbols

Humanizer can convert `TimeUnit` enum values into their standard abbreviated symbols.

## Basic Usage

```csharp
using Humanizer;

TimeUnit.Day.ToSymbol();  // => "d"
TimeUnit.Week.ToSymbol(); // => "week"
TimeUnit.Year.ToSymbol(); // => "y"
```

## All Symbols

The following table lists every `TimeUnit` value and its corresponding symbol in the `en-US` culture:

| TimeUnit | Symbol |
|----------|--------|
| `TimeUnit.Millisecond` | `ms` |
| `TimeUnit.Second` | `s` |
| `TimeUnit.Minute` | `min` |
| `TimeUnit.Hour` | `h` |
| `TimeUnit.Day` | `d` |
| `TimeUnit.Week` | `week` |
| `TimeUnit.Month` | `mo` |
| `TimeUnit.Year` | `y` |

```csharp
TimeUnit.Millisecond.ToSymbol(); // => "ms"
TimeUnit.Second.ToSymbol();      // => "s"
TimeUnit.Minute.ToSymbol();      // => "min"
TimeUnit.Hour.ToSymbol();        // => "h"
TimeUnit.Day.ToSymbol();         // => "d"
TimeUnit.Week.ToSymbol();        // => "week"
TimeUnit.Month.ToSymbol();       // => "mo"
TimeUnit.Year.ToSymbol();        // => "y"
```

## Culture Support

`ToSymbol` accepts an optional `CultureInfo` parameter. When provided, the symbol is resolved using the culture-specific formatter:

```csharp
TimeUnit.Day.ToSymbol();                            // => "d" (current culture)
TimeUnit.Day.ToSymbol(new CultureInfo("en-US"));    // => "d"
```

Symbols may vary across cultures depending on the localization resources available.

## Related Topics

- [ByteSize](bytesize.md) - ByteRate uses time unit symbols in its output
- [Localization](localization.md) - Culture-specific formatting
