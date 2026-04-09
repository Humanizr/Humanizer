# Number to Words

Humanizer can convert numbers to their English word representation. This works with both `int` and `long` types, supports grammatical gender for languages that require it, and lets you control whether "and" appears in the output.

## Basic Usage

```csharp
using Humanizer;

1.ToWords() // => "one"
10.ToWords() // => "ten"
122.ToWords() // => "one hundred and twenty-two"
3501.ToWords() // => "three thousand five hundred and one"
```

Negative numbers are handled automatically:

```csharp
(-1).ToWords() // => "minus one"
```

## Large Numbers with long

The `ToWords` extension method also works on `long` values, covering numbers up to quintillions:

```csharp
1000000L.ToWords() // => "one million"
1000000000L.ToWords() // => "one billion"
1111111111111L.ToWords()
    // => "one trillion one hundred and eleven billion one hundred and eleven million one hundred and eleven thousand one hundred and eleven"
```

## Controlling "and"

By default, "and" is inserted before the final component (e.g., "five hundred and one"). Pass `addAnd: false` to omit it:

```csharp
3501.ToWords() // => "three thousand five hundred and one"
3501.ToWords(addAnd: false) // => "three thousand five hundred one"
```

## Grammatical Gender

Some languages produce different words depending on grammatical gender. Pass a `GrammaticalGender` value along with a `CultureInfo` to get the correct form:

```csharp
using System.Globalization;

// Russian
1.ToWords(GrammaticalGender.Masculine, new CultureInfo("ru")) // => "один"
1.ToWords(GrammaticalGender.Feminine, new CultureInfo("ru")) // => "одна"
```

The `long` overloads support gender as well:

```csharp
1L.ToWords(GrammaticalGender.Masculine, new CultureInfo("ru")) // => "один"
1L.ToWords(GrammaticalGender.Feminine, new CultureInfo("ru")) // => "одна"
```

## WordForm (Locale-Specific Variations)

Some locales have abbreviated or alternate word forms. Use `WordForm` to select them:

```csharp
// Spanish
21.ToWords(WordForm.Normal, new CultureInfo("es")) // => "veintiuno"
21.ToWords(WordForm.Abbreviation, new CultureInfo("es")) // => "veintiun"
```

You can combine `WordForm` with `GrammaticalGender`:

```csharp
21.ToWords(WordForm.Normal, GrammaticalGender.Feminine, new CultureInfo("es"))
    // => "veintiuna"
```

## Culture Support

Pass a `CultureInfo` to generate words in a specific language. If no culture is provided, the current thread's UI culture is used:

```csharp
11.ToWords(new CultureInfo("en-US")) // => "eleven"
22.ToWords(new CultureInfo("ar")) // => "اثنان و عشرون"
40.ToWords(new CultureInfo("ru")) // => "сорок"
```

## API Summary

| Method | Description |
|--------|-------------|
| `int.ToWords()` | Convert int to words using current culture |
| `int.ToWords(CultureInfo)` | Convert int to words in a specific culture |
| `int.ToWords(bool addAnd)` | Control "and" insertion |
| `int.ToWords(GrammaticalGender, CultureInfo)` | Gender-aware conversion |
| `int.ToWords(WordForm, CultureInfo)` | Locale-specific word form |
| `long.ToWords(CultureInfo, bool)` | Convert long to words |
| `long.ToWords(GrammaticalGender, CultureInfo)` | Gender-aware long conversion |
| `long.ToWords(WordForm, GrammaticalGender, CultureInfo)` | Full control over long conversion |

## Related Topics

- [Number to Ordinal Words](number-to-ordinal-words.md) - Convert numbers to "first", "second", etc.
- [Words to Number](words-to-number.md) - Convert word representations back to numbers
- [Ordinalization](ordinalization.md) - Convert numbers to "1st", "2nd", "3rd", etc.
- [Roman Numerals](roman-numerals.md) - Convert to and from Roman numerals
- [Metric Numerals](metric-numerals.md) - Convert to metric representations
