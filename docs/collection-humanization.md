# Collection Humanization

Convert collections into human-readable comma-separated lists with proper conjunction placement and Oxford comma support.

## Basic Usage

```csharp
using Humanizer;

new[] { "Alice", "Bob", "Charlie" }.Humanize()
    // => "Alice, Bob, and Charlie"

new[] { "Alice", "Bob" }.Humanize()
    // => "Alice and Bob"

new[] { "Alice" }.Humanize()
    // => "Alice"
```

## Custom Separator

By default, the conjunction used is "and". You can pass a different separator:

```csharp
new[] { "Alice", "Bob", "Charlie" }.Humanize("or")
    // => "Alice, Bob, or Charlie"

new[] { "Alice", "Bob" }.Humanize("or")
    // => "Alice or Bob"
```

## Oxford Comma

When a collection has three or more items, the Oxford comma is included before the final conjunction:

```csharp
new[] { "A", "B", "C" }.Humanize()
    // => "A, B, and C"

new[] { "A", "B", "C" }.Humanize("or")
    // => "A, B, or C"
```

With only two items, no comma is used:

```csharp
new[] { "A", "B" }.Humanize()
    // => "A and B"
```

## Custom Display Formatters

By default, `ToString()` is called on each element. You can provide a formatter function to control how each item is displayed.

### String Formatter

```csharp
var people = new[]
{
    new { Name = "Alice", Age = 30 },
    new { Name = "Bob", Age = 25 },
    new { Name = "Charlie", Age = 35 }
};

people.Humanize(p => p.Name)
    // => "Alice, Bob, and Charlie"

people.Humanize(p => $"{p.Name} ({p.Age})")
    // => "Alice (30), Bob (25), and Charlie (35)"
```

### Object Formatter

You can also return an object, which will be converted to a string via `ToString()`:

```csharp
new[] { 1, 2, 3 }.Humanize(n => n * 10)
    // => "10, 20, and 30"
```

### Formatter with Custom Separator

Both formatter overloads accept a separator parameter:

```csharp
new[] { 1, 2, 3 }.Humanize(n => n.Ordinalize(), "or")
    // => "1st, 2nd, or 3rd"
```

## Trimming and Blank Items

Items are automatically trimmed, and blank (null or whitespace) items are skipped. This keeps the comma punctuation clean:

```csharp
new[] { "A", "  B  ", "C" }.Humanize()
    // => "A, B, and C"

new[] { "A", " ", "C" }.Humanize()
    // => "A and C"
```

When using a custom formatter, trimming and blank-skipping apply to the formatter's output rather than the original input.

## Custom Collection Formatter

You can provide your own collection formatting logic by implementing `ICollectionFormatter` and registering it with `Configurator.CollectionFormatters`. This is useful if you need locale-specific list formatting or a different comma style.

## Related Topics

- [To Quantity](to-quantity.md) - Combine numbers with properly pluralized words
- [Pluralization](pluralization.md) - Pluralize individual words
- [Singularization](singularization.md) - Singularize individual words
