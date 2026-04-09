# TimeOnly to Clock Notation

The `ToClockNotation` extension method converts `TimeOnly` values into natural-language clock notation like "three o'clock", "half past four", or "a quarter to six". This feature requires .NET 6.0 or later.

## Basic Usage

```csharp
using Humanizer;

new TimeOnly(3, 0).ToClockNotation()   // => "three o'clock"
new TimeOnly(12, 0).ToClockNotation()  // => "noon"
new TimeOnly(0, 0).ToClockNotation()   // => "midnight"
new TimeOnly(14, 30).ToClockNotation() // => "half past two"
new TimeOnly(8, 15).ToClockNotation()  // => "a quarter past eight"
new TimeOnly(17, 45).ToClockNotation() // => "a quarter to six"
```

## Rounding

By default, `ToClockNotation` uses exact minutes. For times that do not fall on common intervals, the output uses a direct numeric style:

```csharp
new TimeOnly(5, 1).ToClockNotation()  // => "five one"
new TimeOnly(20, 59).ToClockNotation() // => "eight fifty-nine"
```

Pass `ClockNotationRounding.NearestFiveMinutes` to round the time to the nearest five-minute mark:

```csharp
new TimeOnly(5, 1).ToClockNotation(ClockNotationRounding.NearestFiveMinutes)
    // => "five o'clock"

new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes)
    // => "twenty-five past one"

new TimeOnly(14, 32).ToClockNotation(ClockNotationRounding.NearestFiveMinutes)
    // => "half past two"

new TimeOnly(20, 59).ToClockNotation(ClockNotationRounding.NearestFiveMinutes)
    // => "nine o'clock"
```

Available rounding options:

- `ClockNotationRounding.None` - Use exact minutes (default)
- `ClockNotationRounding.NearestFiveMinutes` - Round to the nearest five minutes

## Culture Support

The output adapts to the current culture. Several languages have dedicated converters with natural phrasing:

```csharp
// English
new TimeOnly(14, 30).ToClockNotation() // => "half past two"
new TimeOnly(17, 45).ToClockNotation() // => "a quarter to six"

// Brazilian Portuguese
new TimeOnly(3, 0).ToClockNotation()   // => "três em ponto"
new TimeOnly(14, 30).ToClockNotation() // => "duas e meia"

// Spanish
new TimeOnly(12, 0).ToClockNotation()  // => "mediodía"
new TimeOnly(17, 45).ToClockNotation() // => "las seis menos cuarto de la tarde"

// German, French, Catalan, Luxembourgish, and Portuguese
// also have culture-specific converters.
```

## Related Topics

- [DateTime Humanization](datetime-humanization.md) - Humanize DateTime to relative time
- [TimeSpan Humanization](timespan-humanization.md) - Humanize TimeSpan to readable text
- [DateTime to Ordinal Words](datetime-ordinal-words.md) - Convert dates to ordinal word format
- [Fluent Date](fluent-date.md) - Fluent syntax for building dates and time spans
