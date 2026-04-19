# TimeSpan Humanization

TimeSpan humanization converts `TimeSpan` values into human-readable duration strings like "2 weeks, 1 day" or "one hour, three seconds". This is useful for displaying elapsed time, countdowns, or durations in a way that users can quickly parse.

## Basic Usage

```csharp
using Humanizer;

TimeSpan.FromMilliseconds(1).Humanize() // => "1 millisecond"
TimeSpan.FromMilliseconds(2).Humanize() // => "2 milliseconds"
TimeSpan.FromDays(1).Humanize() // => "1 day"
TimeSpan.FromDays(16).Humanize() // => "2 weeks"
```

## Method Signature

```csharp
public static string Humanize(this TimeSpan timeSpan,
    int precision = 1,
    CultureInfo? culture = null,
    TimeUnit maxUnit = TimeUnit.Week,
    TimeUnit minUnit = TimeUnit.Millisecond,
    string? collectionSeparator = ", ",
    bool toWords = false)
```

A second overload accepts a `countEmptyUnits` parameter:

```csharp
public static string Humanize(this TimeSpan timeSpan,
    int precision,
    bool countEmptyUnits,
    CultureInfo? culture = null,
    TimeUnit maxUnit = TimeUnit.Week,
    TimeUnit minUnit = TimeUnit.Millisecond,
    string? collectionSeparator = ", ",
    bool toWords = false)
```

## Precision

The `precision` parameter controls the maximum number of time units returned. The default is 1, which returns only the largest unit:

```csharp
TimeSpan.FromDays(16).Humanize() // => "2 weeks"
TimeSpan.FromDays(16).Humanize(2) // => "2 weeks, 2 days"

TimeSpan.FromMilliseconds(1299630020).Humanize() // => "2 weeks"
TimeSpan.FromMilliseconds(1299630020).Humanize(3) // => "2 weeks, 1 day, 1 hour"
TimeSpan.FromMilliseconds(1299630020).Humanize(4) // => "2 weeks, 1 day, 1 hour, 30 seconds"
TimeSpan.FromMilliseconds(1299630020).Humanize(5)
    // => "2 weeks, 1 day, 1 hour, 30 seconds, 20 milliseconds"
```

By default, empty time units are not counted towards the precision. If you want empty units to count, use the `countEmptyUnits` parameter:

```csharp
TimeSpan.FromMilliseconds(3603001).Humanize(3)
    // => "1 hour, 3 seconds, 1 millisecond"

TimeSpan.FromMilliseconds(3603001).Humanize(3, countEmptyUnits: true)
    // => "1 hour, 3 seconds"
    // (the empty minutes slot counts as one of the 3 precision slots)
```

## Maximum Unit

The `maxUnit` parameter sets an upper bound on the largest time unit displayed. The default is `TimeUnit.Week`:

```csharp
TimeSpan.FromDays(7).Humanize(maxUnit: TimeUnit.Day) // => "7 days"
TimeSpan.FromMilliseconds(2000).Humanize(maxUnit: TimeUnit.Millisecond)
    // => "2000 milliseconds"
```

You can increase `maxUnit` to `TimeUnit.Month` or `TimeUnit.Year` for longer durations. These give approximations based on 365.2425 days per year and 30.4369 days per month:

```csharp
TimeSpan.FromDays(486).Humanize(maxUnit: TimeUnit.Year, precision: 7)
    // => "1 year, 3 months, 29 days"

TimeSpan.FromDays(517).Humanize(maxUnit: TimeUnit.Year, precision: 7)
    // => "1 year, 4 months, 30 days"
```

## Minimum Unit

The `minUnit` parameter sets a lower bound to avoid rolling down to smaller units:

```csharp
TimeSpan.FromMilliseconds(122500).Humanize(minUnit: TimeUnit.Second)
    // => "2 minutes, 2 seconds"
    // (instead of "2 minutes, 2 seconds, 500 milliseconds")

TimeSpan.FromHours(25).Humanize(minUnit: TimeUnit.Day)
    // => "1 day"
    // (instead of "1 day, 1 hour")
```

## Collection Separator

By default, time parts are joined with `", "`. You can provide a custom separator:

```csharp
TimeSpan.FromMilliseconds(1299630020).Humanize(3, collectionSeparator: " - ")
    // => "2 weeks - 1 day - 1 hour"
```

Pass `null` to use the current culture's collection formatter, which adds a conjunction before the last item:

```csharp
// in en-US culture
TimeSpan.FromMilliseconds(1299630020).Humanize(3, collectionSeparator: null)
    // => "2 weeks, 1 day, and 1 hour"

// in de-DE culture
TimeSpan.FromMilliseconds(1299630020).Humanize(3, collectionSeparator: null)
    // => "2 Wochen, 1 Tag und 1 Stunde"
```

## Words Instead of Numbers

Set `toWords: true` to convert numeric values to their word equivalents:

```csharp
TimeSpan.FromMilliseconds(1299630020).Humanize(3, toWords: true)
    // => "two weeks, one day, one hour"

TimeSpan.FromDays(1).Humanize(toWords: true)
    // => "one day"
```

When the `TimeSpan` is zero and `toWords` is true, the result is "no time" instead of "0 milliseconds":

```csharp
TimeSpan.Zero.Humanize() // => "0 milliseconds"
TimeSpan.Zero.Humanize(toWords: true) // => "no time"
TimeSpan.Zero.Humanize(minUnit: TimeUnit.Second) // => "0 seconds"
```

## ToAge

The `ToAge` method expresses a `TimeSpan` as an age. For cultures that define an age expression format, the result includes the appropriate suffix. The default `maxUnit` for `ToAge` is `TimeUnit.Year`:

```csharp
// in en-US culture
TimeSpan.FromDays(750).ToAge() // => "2 years old"
TimeSpan.FromDays(4).ToAge() // => "4 days old"
TimeSpan.FromDays(64).ToAge() // => "2 months old"

// in fr culture
TimeSpan.FromDays(750).ToAge() // => "2 ans"
```

You can also convert the numbers to words:

```csharp
TimeSpan.FromDays(367).ToAge(toWords: true) // => "one year old"
TimeSpan.FromDays(750).ToAge(toWords: true) // => "two years old"
```

## Culture Support

You can specify a culture explicitly. If omitted, the current thread's UI culture is used:

```csharp
TimeSpan.FromDays(1).Humanize(culture: new CultureInfo("ru-RU"))
    // => "1 день"

TimeSpan.FromDays(1).Humanize(culture: new CultureInfo("de-DE"))
    // => "1 Tag"

TimeSpan.FromMilliseconds(1).Humanize(culture: new CultureInfo("sk-SK"))
    // => "1 milisekunda"
```

## Related Topics

- [DateTime Humanization](datetime-humanization.md) - Humanize relative dates instead of durations
- [Fluent Date](fluent-date.md) - Fluent APIs for creating TimeSpan and DateTime values
