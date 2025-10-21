# Quick Start Guide

Get started with Humanizer in minutes.

## Installation

Install the Humanizer NuGet package:

```bash
dotnet add package Humanizer
```

For English-only:

```bash
dotnet add package Humanizer.Core
```

See [Installation](installation.md) for more options.

## Basic Examples

### Humanize Strings

```csharp
using Humanizer;

"PascalCaseString".Humanize(); 
// => "Pascal case string"

"some_property_name".Humanize(); 
// => "Some property name"
```

### Humanize DateTimes

```csharp
DateTime.UtcNow.AddHours(-2).Humanize(); 
// => "2 hours ago"

DateTime.UtcNow.AddDays(1).Humanize(); 
// => "tomorrow"
```

### Humanize TimeSpans

```csharp
TimeSpan.FromDays(1).Humanize(); 
// => "1 day"

TimeSpan.FromMinutes(90).Humanize(); 
// => "an hour"
```

### Humanize Enums

```csharp
public enum PaymentStatus
{
    PendingApproval,
    Approved,
    Declined
}

PaymentStatus.PendingApproval.Humanize(); 
// => "Pending approval"
```

### Pluralization

```csharp
"person".Pluralize(); 
// => "people"

"case".ToQuantity(5); 
// => "5 cases"
```

### Number Conversions

```csharp
1234.ToWords(); 
// => "one thousand two hundred and thirty-four"

1.Ordinalize(); 
// => "1st"

21.Ordinalize(); 
// => "21st"
```

### Fluent Dates

```csharp
In.January; 
// => January 1st of current year

2.Days() + 3.Hours(); 
// => TimeSpan of 2 days and 3 hours

DateTime.Now + 2.Weeks(); 
// => DateTime 2 weeks from now
```

### Collections

```csharp
var items = new[] { "apple", "banana", "cherry" };
items.Humanize(); 
// => "apple, banana, and cherry"
```

### Truncation

```csharp
"Long text that needs truncating".Truncate(10); 
// => "Long text…"
```

## Common Patterns

### Display Property Names in UI

```csharp
public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}

// Generate labels automatically
nameof(Person.FirstName).Humanize(); 
// => "First name"

nameof(Person.DateOfBirth).Humanize(); 
// => "Date of birth"
```

### Relative Time Display

```csharp
var postDate = DateTime.UtcNow.AddHours(-3);
$"Posted {postDate.Humanize()}"; 
// => "Posted 3 hours ago"
```

### Format Numbers for Display

```csharp
var count = 1234567;
$"Downloaded {count.ToMetric()} times"; 
// => "Downloaded 1.23M times"
```

### Pluralize Based on Count

```csharp
var itemCount = 5;
$"You have {itemCount} {"item".ToQuantity(itemCount)}"; 
// => "You have 5 items"

var singleItem = 1;
$"You have {singleItem} {"item".ToQuantity(singleItem)}"; 
// => "You have 1 item"
```

## Next Steps

Explore the documentation for detailed information on each feature:

- [String Humanization](string-humanization.md)
- [DateTime Humanization](datetime-humanization.md)
- [Number Conversions](number-to-words.md)
- [Pluralization](pluralization.md)
- [All Features](index.md)
