# Time Unit Symbols

Humanizer exposes extensions for converting `TimeUnit` values into localized abbreviations. This is useful when summarizing durations in tables, dashboards, or logs.

```csharp
using Humanizer;
using Humanizer.Localisation;

var timeUnit = TimeUnit.Minute;
string symbol = timeUnit.ToSymbol();   // "min"
```

Supported units include milliseconds through years. Symbols follow language-specific conventions when you supply a `CultureInfo`.

```csharp
TimeUnit.Hour.ToSymbol(culture: new CultureInfo("fr")); // "h"
TimeUnit.Day.ToSymbol(culture: new CultureInfo("de"));  // "T"
```

Combine `ToSymbol()` with `TimeSpan.Humanize()` to generate compact summaries:

```csharp
var duration = TimeSpan.FromMinutes(90);
var text = duration.Humanize(precision: 2); // "an hour, 30 minutes"
var symbol = TimeUnit.Minute.ToSymbol();   // "min"
```

> [!TIP]
> When presenting mixed units (e.g., `TimeSpan.Humanize(precision: 3)`), consider rendering the symbol for the smallest unit used to keep the output consistent.
