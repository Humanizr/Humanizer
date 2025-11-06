# Configuration

Humanizer exposes configuration points that control how strings, numbers, dates, collections, and quantities are rendered. Because these settings are global, perform registration during application startup (for example, in `Program.cs`).

```csharp
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Humanizer;

Configurator.DateTimeHumanizeStrategy = new PrecisionDateTimeHumanizeStrategy(precision: 0.75m);
Configurator.DateTimeOffsetHumanizeStrategy = new PrecisionDateTimeOffsetHumanizeStrategy(precision: 0.75m);

Configurator.Formatters.Register(
	"en-GB",
	cultureInfo => new CustomTimeUnitFormatter(cultureInfo));

Configurator.CollectionFormatters.Register(
	"en-GB",
	_ => new BritishOxfordCollectionFormatter());
```

```csharp
public sealed class CustomTimeUnitFormatter(CultureInfo culture) : DefaultFormatter(culture)
{
	public override string TimeUnitHumanize(TimeUnit timeUnit)
		=> base.TimeUnitHumanize(timeUnit).ToUpperInvariant();
}

public sealed class BritishOxfordCollectionFormatter : ICollectionFormatter
{
	public string Humanize<T>(IEnumerable<T> collection)
	{
		var items = collection.ToList();
		return items.Count switch
		{
			0 => string.Empty,
			1 => items[0]?.ToString() ?? string.Empty,
			_ => $"{string.Join(", ", items.Take(items.Count - 1))}, and {items[^1]}"
		};
	}
}
```

## Strategy configuration

- `IDateTimeHumanizeStrategy` and `IDateTimeOffsetHumanizeStrategy` control the thresholds for phrases such as "yesterday" or "2 hours ago". Swap in `PrecisionDateTimeHumanizeStrategy` or implement your own strategy to adjust rounding.
- On .NET 6+, `Configurator.DateOnlyHumanizeStrategy` and `Configurator.TimeOnlyHumanizeStrategy` let you customize `DateOnly.Humanize()` and `TimeOnly.Humanize()`.
- Use `Configurator.UseEnumDescriptionPropertyLocator` to change which attribute property supplies descriptions for `Enum.Humanize()`.

## Localized formatters and converters

`Configurator` exposes a set of `LocaliserRegistry<T>` instances. Call `Register` before the registry is used to add or replace localizations.

- `Configurator.Formatters` controls the phrases returned by `TimeSpan.Humanize` and related members.
- `Configurator.CollectionFormatters` determines how collections are joined (see [Collection Humanization](collection-humanization.md)).
- `Configurator.NumberToWordsConverters` and `Configurator.Ordinalizers` let you plug in custom `INumberToWordsConverter` or `IOrdinalizer` implementations.

```csharp
Configurator.NumberToWordsConverters.Register("custom", new CustomNumberToWordsConverter());

public sealed class CustomNumberToWordsConverter : INumberToWordsConverter
{
	public string Convert(long number) => number.ToString(CultureInfo.InvariantCulture);
	public string Convert(long number, WordForm wordForm) => Convert(number);
	public string Convert(long number, bool addAnd) => Convert(number);
	public string Convert(long number, bool addAnd, WordForm wordForm) => Convert(number);
	public string Convert(long number, GrammaticalGender gender, bool addAnd = true) => Convert(number);
	public string Convert(long number, WordForm wordForm, GrammaticalGender gender, bool addAnd = true) => Convert(number);
	public string ConvertToOrdinal(int number) => $"{number}th";
	public string ConvertToOrdinal(int number, WordForm wordForm) => ConvertToOrdinal(number);
	public string ConvertToOrdinal(int number, GrammaticalGender gender) => ConvertToOrdinal(number);
	public string ConvertToOrdinal(int number, GrammaticalGender gender, WordForm wordForm) => ConvertToOrdinal(number);
	public string ConvertToTuple(int number) => number switch
	{
		1 => "single",
		2 => "double",
		_ => $"{number}-tuple"
	};
}
```

Once a registry is accessed it becomes immutable for the remainder of the process, so ensure registrations happen before any humanization call executes.

## Testing considerations

- Store original strategy instances before replacing them and restore the previous values in `Dispose` for repeatable tests.
- Isolate culture-dependent changes by setting `CultureInfo.CurrentUICulture` inside test scopes.

## Guidelines for production apps

- **Register once:** Configuration is global, so set it during startup and avoid per-request changes.
- **Document decisions:** Keep a record of registered strategies or converters so new contributors understand the behavior.
- **Prefer specificity:** Register formatters using the appropriate locale code to avoid unexpectedly replacing other cultures.

For the full list of options, review the `Humanizer.Configurator` class or generate the API reference with DocFX.
