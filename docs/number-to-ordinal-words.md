# Number to Ordinal Words

Humanizer can convert numbers to their ordinal word form, turning `1` into "first", `2` into "second", and so on. This is useful for generating human-readable rankings, positions, or sequences.

## Basic Usage

```csharp
using Humanizer;

0.ToOrdinalWords() // => "zeroth"
1.ToOrdinalWords() // => "first"
2.ToOrdinalWords() // => "second"
10.ToOrdinalWords() // => "tenth"
21.ToOrdinalWords() // => "twenty-first"
121.ToOrdinalWords() // => "hundred and twenty-first"
1000.ToOrdinalWords() // => "thousandth"
```

## Grammatical Gender

Some languages produce different ordinal words depending on grammatical gender. Pass a `GrammaticalGender` value along with a `CultureInfo`:

```csharp
using System.Globalization;

// Brazilian Portuguese
1.ToOrdinalWords(GrammaticalGender.Masculine, new CultureInfo("pt-BR"))
    // => "primeiro"
1.ToOrdinalWords(GrammaticalGender.Feminine, new CultureInfo("pt-BR"))
    // => "primeira"
```

## WordForm (Locale-Specific Variations)

Some locales have abbreviated or alternate forms for ordinal words. Use `WordForm` to select them:

```csharp
// Spanish
1.ToOrdinalWords(WordForm.Normal, new CultureInfo("es")) // => "primero"
3.ToOrdinalWords(WordForm.Abbreviation, new CultureInfo("es")) // => "tercer"
```

You can combine `WordForm` with `GrammaticalGender` for full control:

```csharp
// Spanish
3.ToOrdinalWords(GrammaticalGender.Masculine, WordForm.Normal, new CultureInfo("es"))
    // => "tercero"
3.ToOrdinalWords(GrammaticalGender.Masculine, WordForm.Abbreviation, new CultureInfo("es"))
    // => "tercer"
3.ToOrdinalWords(GrammaticalGender.Feminine, WordForm.Normal, new CultureInfo("es"))
    // => "tercera"
```

## Culture Support

Pass a `CultureInfo` to generate ordinal words in a specific language. If no culture is provided, the current thread's UI culture is used:

```csharp
1021.ToOrdinalWords(new CultureInfo("en-US")) // => "thousand and twenty-first"
21.ToOrdinalWords(new CultureInfo("ar")) // => "الحادي و العشرون"
1112.ToOrdinalWords(new CultureInfo("ru")) // => "одна тысяча сто двенадцатый"
```

## API Summary

| Method | Description |
|--------|-------------|
| `int.ToOrdinalWords()` | Convert to ordinal words using current culture |
| `int.ToOrdinalWords(CultureInfo)` | Convert to ordinal words in a specific culture |
| `int.ToOrdinalWords(GrammaticalGender, CultureInfo)` | Gender-aware ordinal conversion |
| `int.ToOrdinalWords(WordForm, CultureInfo)` | Locale-specific word form |
| `int.ToOrdinalWords(GrammaticalGender, WordForm, CultureInfo)` | Full control over gender and form |

## Related Topics

- [Number to Words](number-to-words.md) - Convert numbers to "one", "two", "three", etc.
- [Words to Number](words-to-number.md) - Convert word representations back to numbers
- [Ordinalization](ordinalization.md) - Convert numbers to "1st", "2nd", "3rd", etc.
- [Roman Numerals](roman-numerals.md) - Convert to and from Roman numerals
- [Metric Numerals](metric-numerals.md) - Convert to metric representations
