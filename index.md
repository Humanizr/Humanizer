---
layout: default
title: Humanizer - .NET library for humanizing data
---

# Welcome to Humanizer

Humanizer meets all your .NET needs for manipulating and displaying strings, enums, dates, times, timespans, numbers and quantities.

## Quick Start

Install Humanizer via NuGet:

```bash
dotnet add package Humanizer
```

## Features

### Humanize Strings

```csharp
"PascalCaseInputStringIsTurnedIntoSentence".Humanize() => "Pascal case input string is turned into sentence"
"Underscored_input_string_is_turned_into_sentence".Humanize() => "Underscored input string is turned into sentence"
"Heading".Humanize() => "Heading"
```

### Humanize DateTime

```csharp
DateTime.UtcNow.AddHours(-30).Humanize() => "yesterday"
DateTime.UtcNow.AddHours(-2).Humanize() => "2 hours ago"
DateTime.UtcNow.AddHours(30).Humanize() => "tomorrow"
DateTime.UtcNow.AddHours(2).Humanize() => "2 hours from now"
```

### Humanize TimeSpan

```csharp
TimeSpan.FromMilliseconds(1299630020).Humanize() => "2 weeks"
TimeSpan.FromMilliseconds(1299630020).Humanize(3) => "2 weeks, 1 day, 1 hour"
```

### Number to Words

```csharp
1.ToWords() => "one"
10.ToWords() => "ten"
11.ToWords() => "eleven"
122.ToWords() => "one hundred and twenty-two"
3501.ToWords() => "three thousand five hundred and one"
```

### Number to Ordinal Words

```csharp
1.ToOrdinalWords() => "first"
2.ToOrdinalWords() => "second"
3.ToOrdinalWords() => "third"
```

## Documentation

For complete documentation, examples, and more features, visit the [GitHub repository](https://github.com/Humanizr/Humanizer) or check out the [README](https://github.com/Humanizr/Humanizer/blob/main/readme.md).

## Get Involved

- **GitHub**: [https://github.com/Humanizr/Humanizer](https://github.com/Humanizr/Humanizer)
- **NuGet**: [https://www.nuget.org/packages/Humanizer](https://www.nuget.org/packages/Humanizer)
- **Issues**: [Report bugs or request features](https://github.com/Humanizr/Humanizer/issues)
- **Contributing**: [Contribution guidelines](https://github.com/Humanizr/Humanizer/blob/main/.github/CONTRIBUTING.md)

## License

Humanizer is licensed under the [MIT License](https://github.com/Humanizr/Humanizer/blob/main/license.txt).
