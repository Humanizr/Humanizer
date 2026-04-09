# DateTime to Ordinal Words

The `ToOrdinalWords` extension method converts `DateTime` and `DateOnly` values into a human-readable ordinal date format. The output varies by culture, producing natural-sounding dates like "1st January 2015" (en-GB) or "January 1st, 2015" (en-US).

## Basic Usage

```csharp
using Humanizer;

new DateTime(2015, 1, 1).ToOrdinalWords()
    // => "January 1st, 2015" (en-US)

new DateTime(2015, 3, 22).ToOrdinalWords()
    // => "22nd March 2015" (en-GB)

new DateTime(2015, 2, 12).ToOrdinalWords()
    // => "February 12th, 2015" (en-US)
```

## DateOnly Support

On .NET 6.0 and later, `ToOrdinalWords` also works with `DateOnly`:

```csharp
new DateOnly(2015, 1, 1).ToOrdinalWords()
    // => "January 1st, 2015" (en-US)

new DateOnly(2015, 1, 1).ToOrdinalWords()
    // => "1st January 2015" (en-GB)
```

## Grammatical Case

Languages with grammatical case systems (such as Russian and Polish) can produce different output depending on the case. Pass a `GrammaticalCase` value to control this:

```csharp
new DateTime(2015, 1, 1).ToOrdinalWords(GrammaticalCase.Nominative)
new DateTime(2015, 1, 1).ToOrdinalWords(GrammaticalCase.Genitive)
```

Available cases:

- `GrammaticalCase.Nominative` - Subject of a verb
- `GrammaticalCase.Genitive` - Possessor of another noun
- `GrammaticalCase.Dative` - Indirect object of a verb
- `GrammaticalCase.Accusative` - Direct object of a verb
- `GrammaticalCase.Instrumental` - Object used in performing an action
- `GrammaticalCase.Prepositional` - Object of a preposition

For languages without a case system (like English), the grammatical case parameter has no effect.

The `DateOnly` overload also accepts `GrammaticalCase` on .NET 6.0+:

```csharp
new DateOnly(2015, 1, 1).ToOrdinalWords(GrammaticalCase.Genitive)
```

## Culture-Specific Formats

The output format is determined by the current culture. Here are some examples:

```csharp
// English (US)
new DateTime(2015, 1, 1).ToOrdinalWords() // => "January 1st, 2015"

// English (GB)
new DateTime(2015, 1, 1).ToOrdinalWords() // => "1st January 2015"

// French
new DateTime(2015, 1, 1).ToOrdinalWords() // => "1er janvier 2015"
new DateTime(2020, 3, 2).ToOrdinalWords() // => "2 mars 2020"

// German
new DateTime(2015, 1, 1).ToOrdinalWords() // => "1. Januar 2015"

// Spanish
new DateTime(2022, 1, 25).ToOrdinalWords() // => "25 de enero de 2022"
```

## Related Topics

- [DateTime Humanization](datetime-humanization.md) - Humanize DateTime to relative time
- [Fluent Date](fluent-date.md) - Fluent syntax for building dates and time spans
- [TimeSpan Humanization](timespan-humanization.md) - Humanize TimeSpan to readable text
