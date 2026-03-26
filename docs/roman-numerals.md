# Roman Numerals

Humanizer can convert integers to Roman numeral strings and parse Roman numeral strings back to integers. The supported range is 1 through 3999, covering the standard Roman numeral system.

## Basic Usage

```csharp
using Humanizer;

1.ToRoman()    // => "I"
4.ToRoman()    // => "IV"
10.ToRoman()   // => "X"
1990.ToRoman() // => "MCMXC"
3999.ToRoman() // => "MMMCMXCIX"

"XIV".FromRoman()   // => 14
"MCMXC".FromRoman() // => 1990
```

## Converting Integers to Roman Numerals

The `ToRoman` extension method works on `int` values in the range 1 to 3999:

```csharp
1.ToRoman()    // => "I"
5.ToRoman()    // => "V"
9.ToRoman()    // => "IX"
40.ToRoman()   // => "XL"
90.ToRoman()   // => "XC"
400.ToRoman()  // => "CD"
500.ToRoman()  // => "D"
100.ToRoman()  // => "C"
3999.ToRoman() // => "MMMCMXCIX"
```

Subtractive notation is used automatically for 4, 9, 40, 90, 400, and 900.

## Parsing Roman Numerals

The `FromRoman` extension method converts a Roman numeral string back to an integer. It is case-insensitive:

```csharp
"I".FromRoman()        // => 1
"IV".FromRoman()       // => 4
"XII".FromRoman()      // => 12
"XL".FromRoman()       // => 40
"XC".FromRoman()       // => 90
"CD".FromRoman()       // => 400
"D".FromRoman()        // => 500
"MMMCMXCIX".FromRoman() // => 3999
```

A `ReadOnlySpan<char>` overload is also available for allocation-free parsing:

```csharp
"XIV".AsSpan().FromRoman() // => 14
```

## Error Handling

`ToRoman` throws an `ArgumentOutOfRangeException` for values outside the 1 to 3999 range:

```csharp
0.ToRoman()    // throws ArgumentOutOfRangeException
4000.ToRoman() // throws ArgumentOutOfRangeException
```

`FromRoman` throws an `ArgumentException` for empty or invalid Roman numeral strings, and an `ArgumentNullException` for null input:

```csharp
"".FromRoman()    // throws ArgumentException
"ABC".FromRoman() // throws ArgumentException
```

## Related Topics

- [Number to Words](number-to-words.md) - Convert numbers to English words
- [Metric Numerals](metric-numerals.md) - Convert numbers to metric notation
- [Tupleize](tupleize.md) - Convert numbers to tuple names
