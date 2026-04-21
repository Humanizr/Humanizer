---
name: humanizer
description: Humanizer best practices and conventions for .NET. Use when transforming strings, enums, dates, times, timespans, numbers, and quantities into human-friendly text. Keeps Humanizer usage clean and idiomatic.
---

# Humanizer

Official Humanizer skill for writing clean, idiomatic code that transforms programmatic data into human-readable formats.

## Installation

```bash
# All languages
dotnet add package Humanizer

# English only (smaller package)
dotnet add package Humanizer.Core

# Specific language (e.g., French)
dotnet add package Humanizer.Core.fr
```

**Supported frameworks:** net10.0, net8.0, net48

## String Humanization

Transform PascalCase, camelCase, underscored_strings, and dash-separated-strings into readable sentences.

```csharp
"PascalCaseInputString".Humanize() => "Pascal case input string"
"Underscored_input_string".Humanize() => "Underscored input string"
"dash-separated-string".Humanize() => "Dash separated string"
```

### Control Letter Casing

```csharp
"CanReturnTitleCase".Humanize(LetterCasing.Title) => "Can Return Title Case"
"CanReturnLowerCase".Humanize(LetterCasing.LowerCase) => "can return lower case"
"CanHumanizeIntoUpperCase".Humanize(LetterCasing.AllCaps) => "CAN HUMANIZE INTO UPPER CASE"
"some string".Humanize(LetterCasing.Sentence) => "Some string"
```

### Dehumanize Back to PascalCase

```csharp
"Pascal case input string".Dehumanize() => "PascalCaseInputString"
```

## String Transformations

Use `Transform` for flexible, chainable transformations:

```csharp
"Sentence casing".Transform(To.LowerCase) => "sentence casing"
"Sentence casing".Transform(To.TitleCase) => "Sentence Casing"
"Sentence casing".Transform(To.UpperCase) => "SENTENCE CASING"
"Sentence casing".Transform(To.SentenceCase) => "Sentence casing"
```

## Truncation

Truncate strings intelligently with multiple strategies:

```csharp
// Fixed length (default) - uses … character
"Long text to truncate".Truncate(10) => "Long text…"

// Custom truncation string
"Long text to truncate".Truncate(10, "---") => "Long te---"

// Fixed number of words
"Long text to truncate".Truncate(2, Truncator.FixedNumberOfWords) => "Long text…"

// Truncate from the left
"Long text to truncate".Truncate(10, Truncator.FixedLength, TruncateFrom.Left) => "… truncate"
```

## Enum Humanization

Convert enum values to readable text without needing `DescriptionAttribute` on every member:

```csharp
public enum UserType
{
    [Description("Custom description")]  // Optional: takes precedence when present
    MemberWithDescription,
    MemberWithoutDescription,
    ALLCAPITALS
}

UserType.MemberWithDescription.Humanize() => "Custom description"
UserType.MemberWithoutDescription.Humanize() => "Member without description"
```

### Dehumanize Enums

```csharp
"Member without description".DehumanizeTo<UserType>() => UserType.MemberWithoutDescription

// Handle missing matches gracefully
"Invalid".DehumanizeTo<UserType>(OnNoMatch.ReturnsNull) => null
```

## DateTime Humanization

Get relative time descriptions:

```csharp
DateTime.UtcNow.AddHours(-2).Humanize() => "2 hours ago"
DateTime.UtcNow.AddHours(-30).Humanize() => "yesterday"
DateTime.UtcNow.AddHours(2).Humanize() => "2 hours from now"
DateTime.UtcNow.AddHours(30).Humanize() => "tomorrow"

// DateTimeOffset also supported
DateTimeOffset.UtcNow.AddHours(1).Humanize() => "an hour from now"
```

### With Culture

```csharp
// Arabic
DateTime.UtcNow.AddDays(-1).Humanize(culture: new CultureInfo("ar")) => "أمس"

// Russian
DateTime.UtcNow.AddMinutes(-2).Humanize(culture: new CultureInfo("ru-RU")) => "2 минуты назад"
```

### Precision Strategy

For more precise relative time descriptions:

```csharp
Configurator.DateTimeHumanizeStrategy = new PrecisionDateTimeHumanizeStrategy(precision: .75);
```

## TimeSpan Humanization

```csharp
TimeSpan.FromMilliseconds(1).Humanize() => "1 millisecond"
TimeSpan.FromDays(1).Humanize() => "1 day"
TimeSpan.FromDays(16).Humanize() => "2 weeks"
```

### Precision Parameter

Control how many time units to include:

```csharp
TimeSpan.FromDays(16).Humanize(2) => "2 weeks, 2 days"
TimeSpan.FromMilliseconds(1299630020).Humanize(3) => "2 weeks, 1 day, 1 hour"
TimeSpan.FromMilliseconds(1299630020).Humanize(5) => "2 weeks, 1 day, 1 hour, 30 seconds, 20 milliseconds"
```

### Min/Max Units

```csharp
// Minimum unit (avoid small units)
TimeSpan.FromMilliseconds(122500).Humanize(minUnit: TimeUnit.Second) => "2 minutes, 2 seconds"

// Maximum unit (avoid large units)
TimeSpan.FromDays(7).Humanize(maxUnit: TimeUnit.Day) => "7 days"
```

### Words Instead of Numbers

```csharp
TimeSpan.FromMilliseconds(1299630020).Humanize(3, toWords: true) => "two weeks, one day, one hour"
```

### Age Expression

```csharp
TimeSpan.FromDays(750).ToAge() => "2 years old"
```

## Collection Humanization

Format collections as human-readable lists:

```csharp
new[] { "apple", "orange", "banana" }.Humanize() => "apple, orange, and banana"

// Custom separator
new[] { "one", "two", "three" }.Humanize("or") => "one, two, or three"

// With formatting function
items.Humanize(item => item.Name)
```

## Number to Words

```csharp
1.ToWords() => "one"
10.ToWords() => "ten"
11.ToWords() => "eleven"
122.ToWords() => "one hundred and twenty-two"
1234.ToWords() => "one thousand two hundred and thirty-four"

// Ordinals
1.ToOrdinalWords() => "first"
2.ToOrdinalWords() => "second"
11.ToOrdinalWords() => "eleventh"
```

## Ordinal Numbers

```csharp
1.Ordinalize() => "1st"
2.Ordinalize() => "2nd"
3.Ordinalize() => "3rd"
11.Ordinalize() => "11th"
```

## Quantities

Pluralize and singularize with quantities:

```csharp
"person".ToQuantity(0) => "0 people"
"person".ToQuantity(1) => "1 person"
"person".ToQuantity(2) => "2 people"

// Without number
"person".ToQuantity(2, ShowQuantityAs.None) => "people"

// With words
"person".ToQuantity(2, ShowQuantityAs.Words) => "two people"
```

## Pluralization and Singularization

```csharp
"person".Pluralize() => "people"
"people".Singularize() => "person"

"men".Pluralize() => "men"  // Already plural
"man".Singularize() => "man"  // Already singular

// Force even if already in target form
"men".Pluralize(inputIsKnownToBeSingular: false) => "men"
```

## Byte Size

```csharp
(1024L).Bytes().Humanize() => "1 KB"
(1048576L).Bytes().Humanize() => "1 MB"

// Specific units
(1024L).Kilobytes().Humanize() => "1 MB"
(1.5).Megabytes().Humanize() => "1.5 MB"
```

## Roman Numerals

```csharp
1.ToRoman() => "I"
4.ToRoman() => "IV"
9.ToRoman() => "IX"
2024.ToRoman() => "MMXXIV"

"IV".FromRoman() => 4
"MMXXIV".FromRoman() => 2024
```

## Metric Numbers

```csharp
1000.ToMetric() => "1k"
1000000.ToMetric() => "1M"
1000000000.ToMetric() => "1G"

"1k".FromMetric() => 1000
```

## Heading/Compass

```csharp
0.ToHeading() => "N"
90.ToHeading() => "E"
180.ToHeading() => "S"
270.ToHeading() => "W"

45.ToHeading() => "NE"
```

## Best Practices

### Use Culture-Aware Methods

Always specify culture when displaying to users in non-English locales:

```csharp
// Good: explicit culture
DateTime.UtcNow.AddHours(-2).Humanize(culture: CultureInfo.CurrentUICulture)

// Good: thread culture will be used automatically
TimeSpan.FromDays(1).Humanize()  // Uses CurrentUICulture
```

### Prefer Extension Methods

Humanizer is designed around extension methods. Prefer them over static utility classes:

```csharp
// Good
"SomeString".Humanize()
myEnum.Humanize()
dateTime.Humanize()

// Avoid
Humanizer.StringHumanizeExtensions.Humanize("SomeString")
```

### Handle Edge Cases

```csharp
// Zero TimeSpan
TimeSpan.Zero.Humanize() => "0 milliseconds"
TimeSpan.Zero.Humanize(toWords: true) => "no time"

// Empty/null strings
string.Empty.Humanize() => ""  // Returns empty string
```

### Use Appropriate Precision

Don't over-specify precision for TimeSpan:

```csharp
// Good: appropriate precision for context
TimeSpan.FromDays(16).Humanize(2) => "2 weeks, 2 days"

// Avoid: excessive precision
TimeSpan.FromDays(16).Humanize(7)  // Usually unnecessary
```

## Do Not

- Do not use `Humanize` on dates when you need the exact date - it's lossy
- Do not assume all languages have the same pluralization rules
- Do not hardcode separators - use locale-aware collection humanization
- Do not manually implement what Humanizer provides (e.g., ordinal suffixes)
