# Dates and Times

Humanizer offers helpers for presenting dates, times, and durations in natural language. This includes relative descriptions ("2 hours ago"), ordinal dates ("January 1st"), fluent time arithmetic, and clock notation.

## Relative time: `DateTime` and `DateTimeOffset`

```csharp
using Humanizer;

DateTime.UtcNow.AddHours(-2).Humanize();               // "2 hours ago"
DateTime.Now.AddDays(1).Humanize();                    // "tomorrow"
DateTimeOffset.UtcNow.AddMinutes(45).Humanize();       // "45 minutes from now"
DateTime.Now.Humanize(culture: new CultureInfo("fr")); // "aujourd’hui"
```

### Comparison base and precision

Provide a comparison base to control relativity:

```csharp
var reference = new DateTime(2025, 05, 01);
var scheduled = reference.AddDays(7);
string result = scheduled.Humanize(dateToCompareAgainst: reference); // "in 7 days"
```

Fine-tune rounding by replacing the default strategy:

```csharp
Configurator.DateTimeHumanizeStrategy =
    new PrecisionDateTimeHumanizeStrategy(precision: 0.75M);
Configurator.DateTimeOffsetHumanizeStrategy =
    new PrecisionDateTimeOffsetHumanizeStrategy(precision: 0.75M);
```

A higher precision moves to the next unit earlier (e.g., 45 seconds → "a minute ago").

## Duration output: `TimeSpan.Humanize`

`TimeSpan.Humanize` converts intervals into readable phrases. Control the level of detail with `precision` and restrict the minimum or maximum units.

```csharp
TimeSpan.FromMilliseconds(1299630020).Humanize(precision: 3);
// "2 weeks, 1 day, 1 hour"

TimeSpan.FromDays(1).Humanize(culture: new CultureInfo("ru"));
// "один день"

TimeSpan.FromHours(25).Humanize(minUnit: TimeUnit.Day); // "1 day"
TimeSpan.FromDays(7).Humanize(maxUnit: TimeUnit.Day);   // "7 days"
```

When you need domain-specific output, adjust the separator or format via the optional parameters:

```csharp
var summary = TimeSpan.FromHours(42).Humanize(
    precision: 2,
    collectionSeparator: " • ",
    toWords: true);
// "two days • eighteen hours"
```

## Ordinal dates

`ToOrdinalWords()` turns `DateTime` and `DateOnly` values into phrases like "January 1st, 2025".

```csharp
new DateTime(2025, 1, 1).ToOrdinalWords();        // "January 1st, 2025"
new DateOnly(2025, 3, 14).ToOrdinalWords();       // "March 14th, 2025"
new DateTime(2025, 1, 1).ToOrdinalWords(GrammaticalCase.Genitive);
```

## Clock notation with `TimeOnly`

Convert `TimeOnly` values into phrases such as "quarter past three" using `ToClockNotation()`.

```csharp
new TimeOnly(3, 0).ToClockNotation();                     // "three o'clock"
new TimeOnly(14, 30).ToClockNotation();                   // "half past two"
new TimeOnly(15, 7).ToClockNotation(ClockNotationRounding.NearestFiveMinutes);
// "five past three"
```

## Fluent date and time construction

Use the `In` and `On` helpers to create dates fluently:

```csharp
DateTime meeting = In.January.The(15);     // January 15th of the current year
DateTime launch = On.April.The5th.At(9, 30); // April 5th, 09:30 of the current year
```

Time calculations are equally expressive:

```csharp
var deadline = DateTime.UtcNow + 2.Days() + 3.Hours() - 15.Minutes();
```

## Testing and localization tips

- Wrap tests with `[UseCulture]` to assert locale-specific output.
- Set `CultureInfo.CurrentUICulture` in web or background services before humanizing time values.
- Combine with [Time Unit Symbols](time-unit-symbols.md) when you need compact tabular output.
