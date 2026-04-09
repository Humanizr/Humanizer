# Fluent Date

Fluent date methods make time-based code dramatically more readable. Instead of verbose `DateTime.Now.AddDays(2).AddHours(3)` calls, you can write `DateTime.Now + 2.Days() + 3.Hours()`. This improves code clarity and reduces errors.

## Basic Usage

```csharp
using Humanizer;

2.Days() // => TimeSpan.FromDays(2)
3.Hours() // => TimeSpan.FromHours(3)

In.January // => January 1st of the current year
On.January.The4th // => January 4th of the current year

DateTime.Now + 2.Days() + 3.Hours() - 5.Minutes()
```

## TimeSpan Extension Methods

Numeric types gain extension methods that create `TimeSpan` values. These methods work on `int`, `double`, `long`, and other numeric types:

```csharp
2.Milliseconds() // => TimeSpan.FromMilliseconds(2)
5.Seconds() // => TimeSpan.FromSeconds(5)
3.Minutes() // => TimeSpan.FromMinutes(3)
4.Hours() // => TimeSpan.FromHours(4)
7.Days() // => TimeSpan.FromDays(7)
2.Weeks() // => TimeSpan.FromDays(14)
```

Use these with arithmetic operators for readable date math:

```csharp
// Instead of this:
DateTime.Now.AddDays(2).AddHours(3).AddMinutes(-5)

// Write this:
DateTime.Now + 2.Days() + 3.Hours() - 5.Minutes()
```

Double values work too, for fractional time units:

```csharp
1.5.Hours() // => TimeSpan representing 1 hour and 30 minutes
0.5.Days() // => TimeSpan representing 12 hours
```

## DateTime Construction with In

The `In` class provides a natural way to construct `DateTime` values by month name:

```csharp
In.TheYear(2010) // => new DateTime(2010, 1, 1)

In.January // => January 1st of the current year
In.February // => February 1st of the current year
In.March // => March 1st of the current year
// ... all twelve months are available

In.JanuaryOf(2009) // => new DateTime(2009, 1, 1)
In.FebruaryOf(2009) // => new DateTime(2009, 2, 1)
In.MarchOf(2009) // => new DateTime(2009, 3, 1)
// ... all twelve months have an Of(year) variant
```

## Relative Time Construction with In

The `In` class also provides named number subclasses (from `One` through `Ten`) for constructing dates relative to now or a given base date:

```csharp
In.One.Hour // => DateTime.UtcNow.AddHours(1)
In.Two.Days // => DateTime.UtcNow.AddDays(2)
In.Three.Weeks // => DateTime.UtcNow.AddDays(21)
In.Five.Months // => DateTime.UtcNow.AddMonths(5)
In.Ten.Years // => DateTime.UtcNow.AddYears(10)
```

Each also has a `From` method for computing relative to a specific date:

```csharp
var baseDate = On.January.The21st;
In.Five.DaysFrom(baseDate) // => baseDate.AddDays(5)
In.One.MonthFrom(baseDate) // => baseDate.AddMonths(1)
In.Two.YearsFrom(baseDate) // => baseDate.AddYears(2)
```

Available time units: `Second`/`Seconds`, `Minute`/`Minutes`, `Hour`/`Hours`, `Day`/`Days`, `Week`/`Weeks`, `Month`/`Months`, `Year`/`Years` (singular forms are used on `In.One`).

## DateTime Construction with On

The `On` class creates `DateTime` values for specific days of specific months:

```csharp
On.January.The4th // => January 4th of the current year
On.February.The(12) // => February 12th of the current year
On.March.The1st // => March 1st of the current year
On.December.The25th // => December 25th of the current year
```

Ordinal day properties are available from `The1st` through `The31st`, and the `The(int dayNumber)` method accepts any valid day number.

## DateTime Manipulation

Extension methods on `DateTime` allow you to change individual components of an existing date:

```csharp
var date = new DateTime(2011, 2, 10, 5, 25, 45, 125);

date.In(2008) // => new DateTime(2008, 2, 10, 5, 25, 45, 125)
date.At(2, 20, 15) // => new DateTime(2011, 2, 10, 2, 20, 15, 0)
date.AtNoon() // => new DateTime(2011, 2, 10, 12, 0, 0, 0)
date.AtMidnight() // => new DateTime(2011, 2, 10, 0, 0, 0, 0)
```

The `At` method accepts hour, minute (default 0), second (default 0), and millisecond (default 0) parameters:

```csharp
date.At(14) // => 2:00 PM, keeping the same date
date.At(14, 30) // => 2:30 PM
date.At(14, 30, 45) // => 2:30:45 PM
date.At(14, 30, 45, 500) // => 2:30:45.500 PM
```

## Related Topics

- [DateTime Humanization](datetime-humanization.md) - Humanize DateTime values into relative time expressions
- [TimeSpan Humanization](timespan-humanization.md) - Humanize TimeSpan values into readable durations
