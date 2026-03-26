# To Quantity

Combine a number with a word, automatically pluralizing or singularizing it to match the quantity.

## Basic Usage

```csharp
using Humanizer;

"case".ToQuantity(0)  // => "0 cases"
"case".ToQuantity(1)  // => "1 case"
"case".ToQuantity(5)  // => "5 cases"
"man".ToQuantity(2)   // => "2 men"
```

The input word can be singular or plural. `ToQuantity` adjusts it based on the number:

```csharp
"processes".ToQuantity(1) // => "1 process"
"process".ToQuantity(2)   // => "2 processes"
"men".ToQuantity(1)       // => "1 man"
```

Negative quantities are handled correctly:

```csharp
"case".ToQuantity(-1) // => "-1 case"
"case".ToQuantity(-5) // => "-5 cases"
```

## ShowQuantityAs

Control how the quantity is displayed using the `ShowQuantityAs` enum.

### Numeric (Default)

The quantity is shown as a number:

```csharp
"case".ToQuantity(5, ShowQuantityAs.Numeric) // => "5 cases"
```

### Words

The quantity is written out as words:

```csharp
"case".ToQuantity(1, ShowQuantityAs.Words)    // => "one case"
"case".ToQuantity(5, ShowQuantityAs.Words)    // => "five cases"
"process".ToQuantity(1200, ShowQuantityAs.Words)
    // => "one thousand two hundred processes"
```

### None

Only the properly pluralized or singularized word is returned, with no quantity prefix:

```csharp
"case".ToQuantity(0, ShowQuantityAs.None) // => "cases"
"case".ToQuantity(1, ShowQuantityAs.None) // => "case"
"case".ToQuantity(5, ShowQuantityAs.None) // => "cases"
```

## Custom Format Strings

Pass a standard or custom .NET numeric format string to control how the number is rendered:

```csharp
"case".ToQuantity(123456, "N0")  // => "123,456 cases"
"dollar".ToQuantity(2, "C0", new CultureInfo("en-US"))
    // => "$2 dollars"
"case".ToQuantity(123456, "N2")  // => "123,456.00 cases"
```

You can also provide an `IFormatProvider` to use culture-specific formatting:

```csharp
"case".ToQuantity(1234567, "N0", new CultureInfo("it-IT"))
    // => "1.234.567 cases"
```

## Double Quantities

`ToQuantity` also accepts `double` values:

```csharp
"hour".ToQuantity(0.5)   // => "0.5 hours"
"hour".ToQuantity(1.0)   // => "1 hour"
"hour".ToQuantity(22.4)  // => "22.4 hours"
```

Only values that are exactly 1 or -1 (with no fractional part) are treated as singular.

## Related Topics

- [Pluralization](pluralization.md) - Pluralize individual words
- [Singularization](singularization.md) - Singularize individual words
- [Collection Humanization](collection-humanization.md) - Format lists of items
- [Custom Vocabularies](custom-vocabularies.md) - Add irregular or uncountable words
