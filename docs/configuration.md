# Configuration

Humanizer exposes configuration points that control how strings, numbers, dates, collections, and quantities are rendered. Because these settings are global, register them once during application startup (for example, in `Program.cs` or a dependency-injection bootstrapper).

```csharp
using Humanizer;
using Humanizer.Configuration;
using Humanizer.DateTimes;

Configurator.DateTimeHumanizeStrategy = new PrecisionDateTimeHumanizeStrategy(precision: 0.75M);
Configurator.DateTimeOffsetHumanizeStrategy = new PrecisionDateTimeOffsetHumanizeStrategy(precision: 0.75M);
Configurator.RegisterFormatter(TimeUnit.Second, new CustomTimeUnitFormatter());
Configurator.CollectionFormatters.Register(typeof(YourCollection), new YourCollectionFormatter());
```

## Strategy configuration

- `IDateTimeHumanizeStrategy` and `IDateTimeOffsetHumanizeStrategy` control the thresholds for "yesterday", "2 hours ago", and similar phrases. Swap in `PrecisionDateTimeHumanizeStrategy` or implement your own for custom rounding.
- `ITimeSpanHumanizeStrategy` changes how durations roll up into compound units.
- `ITruncator` implementations can be registered via `Truncator.Register<T>()` to enforce consistent string truncation rules.

## Formatters

- `Configurator.RegisterFormatter(TimeUnit unit, IFormatter formatter)` overrides the text used for `TimeSpan.Humanize()`.
- `Configurator.CollectionFormatters` contains formatters keyed by collection type (see [Collection Humanization](collection-humanization.md)).
- Number and byte-size formatters can be swapped to align with domain-specific abbreviations.

## String transformers

`Configurator.StringTransformers` lets you compose custom casing or transliteration logic with `Transform()`. Register additional `IStringTransformer` implementations in the order you want them executed.

```csharp
Configurator.StringTransformers.Add(new SlugifyTransformer());
Configurator.StringTransformers.Add(new RemoveDiacriticsTransformer());
```

## Resetting and testing

Use `Configurator.Reset()` in unit tests to restore the default settings after each run. This prevents side effects from carrying across different test cases.

```csharp
public class HumanizerConfigurationTests : IDisposable
{
	public HumanizerConfigurationTests()
	{
		Configurator.StringTransformers.Add(new SlugifyTransformer());
	}

	public void Dispose() => Configurator.Reset();
}
```

## Guidelines for production apps

- **Register once:** Configuration is global, so set it during startup and avoid per-request changes.
- **Document decisions:** Keep a record of the registered strategies/formatters so the behavior remains discoverable to new contributors.
- **Prefer specificity:** Register formatters for the narrowest applicable type (e.g., a specific collection interface) to avoid unintended matches.
For the full list of options, review the `Humanizer.Configuration.Configurator` class or generate the API reference with DocFX.
