# Numbers

Humanizer includes helpers for presenting numbers as readable phrases, handling ordinals, generating quantities, and converting between specialized representations such as metric or Roman numerals.

## Words for cardinal numbers

`ToWords()` converts integers and decimals into text that respects cultural conventions and grammatical gender where available.

```csharp
using Humanizer;

1234.ToWords();                             // "one thousand two hundred and thirty-four"
3501.ToWords(addAnd: false);                // "three thousand five hundred one"
(-42).ToWords();                            // "minus forty-two"
1.ToWords(GrammaticalGender.Feminine, new("pt")); // "uma"
12.5m.ToWords();                            // "twelve point five"
```

### Tips

- Pass `addAnd: false` to omit "and" in English-style output.
- Provide a `CultureInfo` to switch languages; ensure the corresponding satellite package is referenced (e.g., `Humanizer.Core.fr`).

## Ordinal words

`ToOrdinalWords()` expresses positions such as "first" or "twenty-second". Gender and culture arguments are supported where applicable.

```csharp
42.ToOrdinalWords();                          // "forty-second"
1.ToOrdinalWords(GrammaticalGender.Masculine, new("es")); // "primero"
```

Use `Ordinalize()` for numeric abbreviations (`1st`, `2nd`, etc.).

```csharp
21.Ordinalize();                              // "21st"
"car".ToQuantity(2).Ordinalize();            // "2nd cars"
```

## Quantities

`ToQuantity()` adds the correct pluralization for a noun based on the number provided.

```csharp
"case".ToQuantity(0);                        // "0 cases"
"case".ToQuantity(1);                        // "1 case"
"case".ToQuantity(2, ShowQuantityAs.Words);  // "two cases"
"case".ToQuantity(5, "N0");                 // "5 cases"
```

Combine with [Pluralization](pluralization.md) to override irregular nouns if necessary.

## Words to numbers

Convert human-friendly text back into numeric values with `ToNumber()` or `TryToNumber()`.

```csharp
"forty-two".ToNumber(new CultureInfo("en"));                     // 42

if ("three thousand".TryToNumber(out var value, new CultureInfo("en")))
{
    // value == 3000
}
```

When validation fails, `TryToNumber` returns `false` and surfaces the offending token. Use this in user input workflows to provide targeted error messages.

## Metric numerals

`ToMetric()` shortens numbers using metric prefixes, while `FromMetric()` parses them back.

```csharp
1230d.ToMetric();                            // "1.23k"
0.00042d.ToMetric(decimals: 2);              // "0.42m"
"1.23k".FromMetric();                        // 1230
```

## Roman numerals

Convert between integers and Roman numerals with `ToRoman()` and `FromRoman()`.

```csharp
1990.ToRoman();                              // "MCMXC"
"XIV".FromRoman();                           // 14
```

Input must be between 1 and 3999—the valid range for standard Roman numerals.

## Tupleize and large-number helpers

- `Tupleize()` describes repetition counts (`1.Tupleize()` → "single", `3.Tupleize()` → "triple").
- Use fluent helpers such as `5.Millions()` or `3.Hundreds()` to build numbers expressively:

```csharp
3.Thousands();                               // 3000
5.Millions() + 250.Thousands();              // 5,250,000
```

## Guidelines

- Normalize inputs (trim whitespace, remove punctuation) before parsing with `ToNumber()`.
- Unit-test localized number phrasing, especially where gender or plural forms vary.
- Combine these helpers with [Localization](localization.md) to ensure the correct satellite packages are included for each culture.
