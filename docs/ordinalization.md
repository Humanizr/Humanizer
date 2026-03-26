# Ordinalization

Humanizer can turn numbers into ordinal strings such as "1st", "2nd", "3rd", and "4th". The `Ordinalize` extension method works on both `int` and `string` types and supports grammatical gender, word forms, and culture-specific output.

## Basic Usage

```csharp
using Humanizer;

1.Ordinalize() // => "1st"
2.Ordinalize() // => "2nd"
3.Ordinalize() // => "3rd"
5.Ordinalize() // => "5th"
21.Ordinalize() // => "21st"

// Works on strings too
"1".Ordinalize() // => "1st"
"21".Ordinalize() // => "21st"
"103".Ordinalize() // => "103rd"
```

## Grammatical Gender

Some languages produce different ordinal suffixes depending on grammatical gender. Pass a `GrammaticalGender` value to get the correct form:

```csharp
using System.Globalization;

// Brazilian Portuguese
1.Ordinalize(GrammaticalGender.Masculine) // => "1º"
1.Ordinalize(GrammaticalGender.Feminine) // => "1ª"

// String overloads work the same way
"1".Ordinalize(GrammaticalGender.Masculine) // => "1º"
"1".Ordinalize(GrammaticalGender.Feminine) // => "1ª"
```

In English, gender has no effect on the output. Both masculine and feminine produce the same result.

## WordForm (Locale-Specific Variations)

Some locales support alternate ordinal forms. Use `WordForm` to select them:

```csharp
// Spanish
1.Ordinalize(new CultureInfo("es-ES"), WordForm.Normal) // => "1.º"
1.Ordinalize(new CultureInfo("es-ES"), WordForm.Abbreviation) // => "1.er"
```

You can combine `WordForm` with `GrammaticalGender`:

```csharp
// Spanish
1.Ordinalize(GrammaticalGender.Masculine, new CultureInfo("es-ES"), WordForm.Normal)
    // => "1.º"
1.Ordinalize(GrammaticalGender.Masculine, new CultureInfo("es-ES"), WordForm.Abbreviation)
    // => "1.er"
1.Ordinalize(GrammaticalGender.Feminine, new CultureInfo("es-ES"), WordForm.Normal)
    // => "1.ª"
```

In English, `WordForm` has no effect. Both normal and abbreviated forms produce the same result.

## Culture Support

Pass a `CultureInfo` to generate ordinals for a specific locale. If no culture is provided, the current thread's UI culture is used:

```csharp
1.Ordinalize(new CultureInfo("en-US")) // => "1st"
1.Ordinalize(new CultureInfo("nl-NL")) // => "1e"

// String overloads
"1".Ordinalize(new CultureInfo("en-US")) // => "1st"
"1".Ordinalize(new CultureInfo("nl-NL")) // => "1e"
```

## API Summary

| Method | Description |
|--------|-------------|
| `int.Ordinalize()` | Ordinalize using current culture |
| `int.Ordinalize(CultureInfo)` | Ordinalize in a specific culture |
| `int.Ordinalize(GrammaticalGender)` | Gender-aware ordinalization |
| `int.Ordinalize(GrammaticalGender, CultureInfo)` | Gender-aware with specific culture |
| `int.Ordinalize(CultureInfo, WordForm)` | Culture-specific word form |
| `int.Ordinalize(GrammaticalGender, CultureInfo, WordForm)` | Full control |
| `string.Ordinalize()` | Ordinalize a numeric string |
| `string.Ordinalize(CultureInfo)` | Ordinalize a numeric string in a specific culture |
| `string.Ordinalize(GrammaticalGender)` | Gender-aware string ordinalization |
| `string.Ordinalize(GrammaticalGender, CultureInfo)` | Gender-aware with specific culture |
| `string.Ordinalize(CultureInfo, WordForm)` | Culture-specific word form |
| `string.Ordinalize(GrammaticalGender, CultureInfo, WordForm)` | Full control |

## Related Topics

- [Number to Words](number-to-words.md) - Convert numbers to "one", "two", "three", etc.
- [Number to Ordinal Words](number-to-ordinal-words.md) - Convert numbers to "first", "second", etc.
- [Words to Number](words-to-number.md) - Convert word representations back to numbers
- [Roman Numerals](roman-numerals.md) - Convert to and from Roman numerals
- [Metric Numerals](metric-numerals.md) - Convert to metric representations
