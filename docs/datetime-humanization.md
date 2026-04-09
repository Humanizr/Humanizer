# DateTime Humanization

DateTime humanization converts `DateTime` and `DateTimeOffset` values into human-readable relative time expressions like "2 hours ago" or "tomorrow". This is useful for displaying timestamps in a natural way that users can quickly understand.

## Basic Usage

```csharp
using Humanizer;

DateTime.UtcNow.AddHours(-2).Humanize() // => "2 hours ago"
DateTime.UtcNow.AddHours(-30).Humanize() // => "yesterday"
DateTime.UtcNow.AddHours(2).Humanize() // => "2 hours from now"
DateTime.UtcNow.AddHours(30).Humanize() // => "tomorrow"

DateTimeOffset.UtcNow.AddHours(1).Humanize() // => "an hour from now"
```

## Method Signatures

```csharp
public static string Humanize(this DateTime input,
    bool? utcDate = null,
    DateTime? dateToCompareAgainst = null,
    CultureInfo? culture = null)

public static string Humanize(this DateTimeOffset input,
    DateTimeOffset? dateToCompareAgainst = null,
    CultureInfo? culture = null)
```

## Relative Time Expressions

The default strategy produces these relative time descriptions:

```csharp
// Past
DateTime.UtcNow.AddSeconds(-1).Humanize() // => "one second ago"
DateTime.UtcNow.AddSeconds(-10).Humanize() // => "10 seconds ago"
DateTime.UtcNow.AddMinutes(-1).Humanize() // => "a minute ago"
DateTime.UtcNow.AddMinutes(-10).Humanize() // => "10 minutes ago"
DateTime.UtcNow.AddHours(-1).Humanize() // => "an hour ago"
DateTime.UtcNow.AddHours(-10).Humanize() // => "10 hours ago"
DateTime.UtcNow.AddDays(-1).Humanize() // => "yesterday"
DateTime.UtcNow.AddDays(-10).Humanize() // => "10 days ago"
DateTime.UtcNow.AddMonths(-1).Humanize() // => "one month ago"
DateTime.UtcNow.AddYears(-1).Humanize() // => "one year ago"

// Future
DateTime.UtcNow.AddSeconds(1).Humanize() // => "one second from now"
DateTime.UtcNow.AddMinutes(1).Humanize() // => "a minute from now"
DateTime.UtcNow.AddHours(1).Humanize() // => "an hour from now"
DateTime.UtcNow.AddDays(1).Humanize() // => "tomorrow"
DateTime.UtcNow.AddMonths(1).Humanize() // => "one month from now"
DateTime.UtcNow.AddYears(1).Humanize() // => "one year from now"

// Present
DateTime.UtcNow.Humanize() // => "now"
```

## UTC Handling

By default, if the input `DateTime` is not `DateTimeKind.Local`, it is treated as UTC. You can override this with the `utcDate` parameter:

```csharp
// Explicit UTC
myDate.Humanize(utcDate: true)

// Explicit local time
myDate.Humanize(utcDate: false)
```

When using `DateTimeOffset`, UTC handling is automatic based on the offset information in the value.

## Custom Comparison Date

By default, the input is compared against `DateTime.UtcNow` (or `DateTimeOffset.UtcNow`). You can provide a custom comparison date:

```csharp
var baseDate = new DateTime(2024, 1, 15, 12, 0, 0, DateTimeKind.Utc);
var input = new DateTime(2024, 1, 14, 12, 0, 0, DateTimeKind.Utc);

input.Humanize(dateToCompareAgainst: baseDate) // => "yesterday"
```

## Precision Strategy

There are two humanization strategies: the default strategy and a precision-based strategy. The default strategy uses fixed thresholds for transitions between time units. The precision strategy allows finer control over when the description rolls over to the next unit.

To use the precision strategy, configure it globally:

```csharp
Configurator.DateTimeHumanizeStrategy = new PrecisionDateTimeHumanizeStrategy(precision: .75);
Configurator.DateTimeOffsetHumanizeStrategy = new PrecisionDateTimeOffsetHumanizeStrategy(precision: .75);
```

The default precision is 0.75. With this setting:

```csharp
// 44 seconds => "44 seconds ago"
// 45 seconds => "one minute ago"
// 104 seconds => "one minute ago"
// 105 seconds => "two minutes ago"
// 25 days => "a month ago"
```

## Nullable Support

Calling `Humanize` on a nullable `DateTime?` or `DateTimeOffset?` returns "never" when the value is null:

```csharp
DateTime? never = null;
never.Humanize() // => "never"

DateTime? hasValue = new DateTime(2024, 1, 15);
hasValue.Humanize() // same as hasValue.Value.Humanize()
```

## Culture Support

You can specify a culture explicitly. If omitted, the current thread's UI culture is used:

```csharp
DateTime.UtcNow.AddDays(-1).Humanize(culture: new CultureInfo("ar"))
    // => "أمس"

DateTime.UtcNow.AddDays(-2).Humanize(culture: new CultureInfo("ar"))
    // => "منذ يومين"

DateTime.UtcNow.AddMinutes(-1).Humanize(culture: new CultureInfo("ru-RU"))
    // => "минуту назад"

DateTime.UtcNow.AddMinutes(-2).Humanize(culture: new CultureInfo("ru-RU"))
    // => "2 минуты назад"

DateTime.UtcNow.AddDays(-2).Humanize(culture: new CultureInfo("sv-SE"))
    // => "för 2 dagar sedan"
```

Many localizations are available, including Arabic, Russian, Swedish, German, French, and many more.

## DateOnly Support (.NET 6+)

On .NET 6 and later, `DateOnly` and `DateOnly?` are also supported:

```csharp
DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1).Humanize()
    // => "yesterday"

DateOnly? nullDate = null;
nullDate.Humanize() // => "never"
```

## Related Topics

- [TimeSpan Humanization](timespan-humanization.md) - Humanize durations instead of relative dates
- [Fluent Date](fluent-date.md) - Fluent APIs for constructing and manipulating dates
