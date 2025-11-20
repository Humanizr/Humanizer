# Collection Humanization

Use `Humanize()` on `IEnumerable` sequences to produce natural language lists. Humanizer automatically inserts the correct separators, supports Oxford commas, and lets you inject custom formatting logic.

```csharp
using Humanizer;

var items = new[] { "apples", "bananas", "pears" };
items.Humanize();                   // "apples, bananas, and pears"
items.Humanize("or");             // "apples, bananas, or pears"
```

## Custom item formatting

Pass a formatter to control how each element is rendered. This is useful when humanizing complex objects.

```csharp
record Invoice(int Id, decimal Total);

var invoices = new[]
{
    new Invoice(1, 120.50m),
    new Invoice(2, 75m),
    new Invoice(3, 430m)
};

string summary = invoices.Humanize(
    formatter: invoice => $"#{invoice.Id} ({invoice.Total:C})");
// "#1 ($120.50), #2 ($75.00), and #3 ($430.00)"
```

## Combining with `ToQuantity`

When presenting quantities, humanize the collection first and then apply `ToQuantity` for pluralization.

```csharp
var beverages = new[] { "coffee", "tea", "water" };
string text = beverages.Humanize();          // "coffee, tea, and water"
text.ToQuantity(3, ShowQuantityAs.Words);    // "three coffee, tea, and water"
```

## Custom collection formatters

To change separators or inject domain-specific rules globally, register an `ICollectionFormatter` implementation for the relevant culture.

```csharp
Configurator.CollectionFormatters.Register(
    "en-GB",
    _ => new OxfordCommaWithSemicolonFormatter());

public sealed class OxfordCommaWithSemicolonFormatter : ICollectionFormatter
{
    public string Humanize<T>(IEnumerable<T> collection) =>
        Humanize(collection, item => item?.ToString());

    public string Humanize<T>(IEnumerable<T> collection, Func<T, string?> formatter) =>
        Humanize(collection, formatter, "; and");

    public string Humanize<T>(IEnumerable<T> collection, Func<T, object?> formatter) =>
        Humanize(collection, item => formatter(item)?.ToString());

    public string Humanize<T>(IEnumerable<T> collection, string separator) =>
        Humanize(collection, item => item?.ToString(), separator);

    public string Humanize<T>(IEnumerable<T> collection, Func<T, string?> formatter, string separator)
    {
        var values = collection
            .Select(formatter)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(value => value!.Trim())
            .ToArray();

        return values.Length switch
        {
            0 => string.Empty,
            1 => values[0],
            _ => $"{string.Join("; ", values, 0, values.Length - 1)}{separator} {values[^1]}"
        };
    }

    public string Humanize<T>(IEnumerable<T> collection, Func<T, object?> formatter, string separator) =>
        Humanize(collection, item => formatter(item)?.ToString(), separator);
}
```

This configuration executes process-wide, so perform the registration during application startup (for example, in ASP.NET dependency configuration).

## Handling null or empty entries

Humanizer trims whitespace and skips null or empty items by default. Verify that your formatter does the same if you need custom trimming. For inputs that may contain duplicates or placeholder values, clean the data before calling `Humanize()` to avoid unexpected output.
