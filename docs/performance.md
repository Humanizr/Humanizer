# Performance and Optimization

Humanizer focuses on readability, but many applications call it in performance-sensitive scenarios such as dashboards, messaging systems, and live telemetry. Use the guidance below to keep humanization fast and predictable.

## General principles

- **Defer formatting to the presentation layer.** Store and transport raw values (dates, numbers, counts) and humanize only when rendering to users.
- **Avoid tight loops.** If you must humanize inside loops or high-frequency timers, precompute values or cache results.
- **Prefer invariant data paths.** Do not humanize values that will be parsed back by machines; keep a machine-readable alternative.

## Caching strategies

### Caching considerations

Humanizer already applies internal caching for culture-specific resources. Adding your own memoization rarely delivers additional wins and can easily serve the wrong culture if you ignore `CultureInfo`. If you discover a real hotspot that still needs memoization, key the cache with both the value **and** the culture (for example, `(value, CultureInfo.CurrentUICulture.Name)`) so French output is not reused for English callers.

> [!NOTE]
> If you implement custom caching for humanized output in multi-threaded scenarios, use a thread-safe collection such as `ConcurrentDictionary` to avoid race conditions and data corruption.

### Minimize culture switches

Group similar operations to avoid thrashing `CultureInfo`:

```csharp
var originalCulture = CultureInfo.CurrentCulture;
var originalUICulture = CultureInfo.CurrentUICulture;

try
{
    var culture = CultureInfo.GetCultureInfo("fr-FR");
    CultureInfo.CurrentCulture = culture;
    CultureInfo.CurrentUICulture = culture;

    foreach (var item in items)
    {
        item.DisplayAge = item.Created.Humanize();
        // ToQuantity expects a singular word and handles pluralization automatically
        item.DisplayCount = item.Count.ToQuantity("item");
    }
}
finally
{
    CultureInfo.CurrentCulture = originalCulture;
    CultureInfo.CurrentUICulture = originalUICulture;
}
```

> [!TIP]
> Always restore the previous culture after you temporarily change it. Wrapping the pattern in a small `IDisposable` helper keeps call sites tidy, but the snippet above shows the core logic.

## TimeSpan and DateTime strategies

- Choose `PrecisionDateTimeHumanizeStrategy` or implement your own to reduce extra calculations for intermediate units you never display (for example, show hours but not minutes).
- For `TimeSpan.Humanize`, specify `minUnit`/`maxUnit` to avoid building longer strings than necessary.

```csharp
TimeSpan duration = TimeSpan.FromSeconds(75);
string concise = duration.Humanize(maxUnit: TimeUnit.Minute); // "1 minute"
```

## String truncation and transformations

- Prefer truncators that honor word boundaries (for example, `Truncator.DynamicLengthAndPreserveWords`) or implement your own `ITruncator` when the built-ins are insufficient.
- Chain string transformers sparingly; each transformer runs sequentially, so remove redundant steps from the pipeline you pass to `Transform`.

## Large data sets

- Humanize data lazily when paging through results instead of precomputing for the entire set.
- Use asynchronous pipelines to offload expensive calculations from request threads.
- Consider formatting heavy collections in background jobs and storing the humanized result alongside the raw data if generating on demand is too slow.

## Logging and telemetry

- Write structured logs that capture the raw value. Render the humanized text only in message templates:

```csharp
logger.LogInformation(
    "Processed {Count} items ({HumanizedCount}) in {Elapsed}",
    count,
    count.ToQuantity("item"),
    elapsed.Humanize(maxUnit: TimeUnit.Second));
```

- In Application Insights or similar backends, keep the original numeric field for querying. Use the humanized message for dashboards and alerts.

## Measuring performance

- Add `BenchmarkDotNet` benchmarks to validate your configuration decisions. Focus on allocations (`Allocated`) and mean execution time (`Mean`).
- Profile string-heavy workloads with the .NET profiler or `dotnet trace` to ensure Humanizer is not dominating CPU time.
- When optimizing for startup, pre-warm caches (for example, humanize known enum values) during application boot and measure the effect.

## Troubleshooting slow output

| Symptom | Likely cause | Mitigation |
| --- | --- | --- |
| Humanization spikes CPU in ASP.NET Core | Culture switching per request | Cache `CultureInfo` instances or use middleware to set culture once per request |
| Frequent allocations during truncation | Using the default truncator on large strings | Implement a custom `ITruncator` that reuses buffers or trims at word boundaries |
| `ToWords()` slow for large numbers | Recomputing rarely used values | Cache or precompute results, especially when rendering static assets |
| `DateTime.Humanize()` inconsistent | Multiple threads modifying `Configurator` | Configure once at startup and guard writes with locks |

## Related topics

- [Configuration](configuration.md)
- [Testing and Quality Assurance](testing.md)
- [Application Integration](application-integration.md)
- [Troubleshooting](troubleshooting.md)
