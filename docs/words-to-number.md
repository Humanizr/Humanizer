# Words to Number

Humanizer can convert spelled-out number strings back to their integer representation. This is the reverse of `ToWords` and is currently supported for English only.

## Basic Usage

```csharp
using Humanizer;
using System.Globalization;

"one".ToNumber(new CultureInfo("en")) // => 1
"twenty".ToNumber(new CultureInfo("en")) // => 20
"one hundred and five".ToNumber(new CultureInfo("en")) // => 105
"three thousand two hundred".ToNumber(new CultureInfo("en")) // => 3200
```

A `CultureInfo` parameter is always required.

## Negative Numbers

Prefix the input with "minus" or "negative" to parse negative values:

```csharp
"minus five".ToNumber(new CultureInfo("en")) // => -5
"negative one hundred and five".ToNumber(new CultureInfo("en")) // => -105
```

## Ordinal Words

`ToNumber` also handles ordinal word forms and numeric ordinals:

```csharp
"seventeenth".ToNumber(new CultureInfo("en")) // => 17
"thirty-first".ToNumber(new CultureInfo("en")) // => 31
"one hundred and third".ToNumber(new CultureInfo("en")) // => 103
"17th".ToNumber(new CultureInfo("en")) // => 17
"31st".ToNumber(new CultureInfo("en")) // => 31
"203rd".ToNumber(new CultureInfo("en")) // => 203
```

## Safe Parsing with TryToNumber

Use `TryToNumber` when the input might be invalid. It returns `true` on success without throwing exceptions:

```csharp
if ("forty-two".TryToNumber(out var number, new CultureInfo("en")))
{
    Console.WriteLine(number); // => 42
}
```

An overload provides the first unrecognized word on failure, which is useful for error messages:

```csharp
if (!"tenn".TryToNumber(out var result, new CultureInfo("en"), out var badWord))
{
    Console.WriteLine($"Unrecognized word: {badWord}");
    // => "Unrecognized word: tenn"
}
```

## Error Handling

`ToNumber` throws a `FormatException` when the input contains unrecognized words:

```csharp
// Throws FormatException
"twenty nine hello".ToNumber(new CultureInfo("en"));
```

It throws an `ArgumentNullException` when the input is null:

```csharp
// Throws ArgumentNullException
((string)null!).ToNumber(new CultureInfo("en"));
```

## Language Support

Only English (`en`) is currently supported. Passing a non-English culture throws `NotSupportedException`:

```csharp
// Throws NotSupportedException
"veinte".ToNumber(new CultureInfo("es-ES"));
"vingt".ToNumber(new CultureInfo("fr-FR"));
```

## API Summary

| Method | Description |
|--------|-------------|
| `string.ToNumber(CultureInfo)` | Convert words to int, throws on failure |
| `string.TryToNumber(out int, CultureInfo)` | Safe conversion, returns bool |
| `string.TryToNumber(out int, CultureInfo, out string?)` | Safe conversion with unrecognized word output |

## Related Topics

- [Number to Words](number-to-words.md) - Convert numbers to "one", "two", "three", etc.
- [Number to Ordinal Words](number-to-ordinal-words.md) - Convert numbers to "first", "second", etc.
- [Ordinalization](ordinalization.md) - Convert numbers to "1st", "2nd", "3rd", etc.
- [Roman Numerals](roman-numerals.md) - Convert to and from Roman numerals
