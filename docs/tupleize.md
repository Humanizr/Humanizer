# Tupleize

Humanizer can convert integers into their tuple names. For example, 1 becomes "single", 2 becomes "double", and 3 becomes "triple". Numbers without a specific name fall back to the "n-tuple" format.

## Basic Usage

```csharp
using Humanizer;

1.Tupleize()   // => "single"
2.Tupleize()   // => "double"
3.Tupleize()   // => "triple"
10.Tupleize()  // => "decuple"
100.Tupleize() // => "centuple"
42.Tupleize()  // => "42-tuple"
```

## Named Tuples

The following values have specific named tuple representations:

| Value | Tuple Name  |
|-------|-------------|
| 1     | single      |
| 2     | double      |
| 3     | triple      |
| 4     | quadruple   |
| 5     | quintuple   |
| 6     | sextuple    |
| 7     | septuple    |
| 8     | octuple     |
| 9     | nonuple     |
| 10    | decuple     |
| 100   | centuple    |
| 1000  | milluple    |

```csharp
4.Tupleize()    // => "quadruple"
7.Tupleize()    // => "septuple"
1000.Tupleize() // => "milluple"
```

## N-Tuple Fallback

Any integer that does not have a named tuple returns the format `"{n}-tuple"`. This includes zero, negative numbers, and any positive number not in the named list:

```csharp
0.Tupleize()    // => "0-tuple"
11.Tupleize()   // => "11-tuple"
42.Tupleize()   // => "42-tuple"
(-1).Tupleize() // => "-1-tuple"
```

## Related Topics

- [Number to Words](number-to-words.md) - Convert numbers to English words
- [Ordinalization](ordinalization.md) - Convert numbers to ordinal strings
- [Roman Numerals](roman-numerals.md) - Convert numbers to Roman numerals
