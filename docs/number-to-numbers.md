# Number to Numbers

Humanizer provides fluent extension methods that multiply a number by a power of ten, making large numeric literals easier to read and write in code.

## Basic Usage

```csharp
using Humanizer;

3.Thousands() // => 3000
5.Millions()  // => 5000000
2.Billions()  // => 2000000000
4.Hundreds()  // => 400
1.Tens()      // => 10
```

## Available Methods

Each method multiplies the input by the corresponding factor:

| Method        | Factor        |
|---------------|---------------|
| `Tens()`      | x 10          |
| `Hundreds()`  | x 100         |
| `Thousands()` | x 1,000       |
| `Millions()`  | x 1,000,000   |
| `Billions()`  | x 1,000,000,000 |

```csharp
5.Tens()      // => 50
2.Hundreds()  // => 200
3.Thousands() // => 3000
4.Millions()  // => 4000000
1.Billions()  // => 1000000000
```

## Chaining

Methods can be chained together to build composite values:

```csharp
3.Hundreds().Thousands() // => 300000
```

## Supported Numeric Types

Every method is defined for five numeric types, so you can use them on any common number:

- `int`
- `uint`
- `long`
- `ulong`
- `double`

```csharp
3.Thousands()    // => 3000 (int)
3U.Thousands()   // => 3000 (uint)
3L.Thousands()   // => 3000 (long)
3UL.Thousands()  // => 3000 (ulong)
1.25.Billions()  // => 1250000000.0 (double)
```

Using `double` is especially useful for expressing fractional multipliers:

```csharp
1.5.Millions()  // => 1500000.0
2.5.Thousands() // => 2500.0
```

## Related Topics

- [Number to Words](number-to-words.md) - Convert numbers to English words
- [Metric Numerals](metric-numerals.md) - Convert numbers to metric notation
- [Roman Numerals](roman-numerals.md) - Convert numbers to Roman numerals
