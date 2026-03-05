# Application Integration

Humanizer ships as a set of extension methods and helper types that you can call from any .NET application. This guide highlights common integration patterns across web, desktop, background, and tooling workloads so you can produce human-friendly output without sacrificing reliability or performance.

## Project setup

```csharp
// Global usings in a .NET project file (Directory.Build.props) or Program.cs
global using Humanizer;
```

- Install the appropriate NuGet package (see [Installation](installation.md)) and import the `Humanizer` namespace to access the core extension methods.
- For reusable libraries, expose the humanized result as a separate property or method so the raw value remains available for programmatic use.

## ASP.NET Core and web APIs

Humanizer works seamlessly inside controllers, Razor views, and Minimal APIs. Ensure the request culture matches your users before calling `Humanize()` or `ToWords()`.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en-US", "fr-FR", "es-ES" };
    options.SetDefaultCulture("en-US");
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
});

var app = builder.Build();
app.UseRequestLocalization();

app.MapGet("/orders/{id}", (Order order) => new
{
    order.Id,
    LastUpdated = order.LastUpdated.Humanize(),
    Payload = order.Size.ToMetric()
});

app.Run();
```

### Razor, MVC, and Blazor

- Inject `IStringLocalizer` alongside Humanizer helpers to combine translated templates with humanized values.
- In Razor views, humanize directly inside expressions: `@Model.LastUpdated.Humanize(culture: ViewContext.HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture)`.
- For Blazor, call Humanizer from components or `@code` blocks. Set `CultureInfo.CurrentCulture` during `Program.cs` startup for WebAssembly and keep it in sync with the UI culture.

## ASP.NET MVC 4.x

- Humanize display names by replacing the default metadata provider with a subclass that calls `Humanize()` when no `DisplayNameAttribute` or `DisplayAttribute` is present. For example:

    ```csharp
    public sealed class HumanizingMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(
            IEnumerable<Attribute> attributes,
            Type containerType,
            Func<object> modelAccessor,
            Type modelType,
            string propertyName)
        {
            var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            if (string.IsNullOrEmpty(metadata.DisplayName) && !string.IsNullOrEmpty(propertyName))
            {
                metadata.DisplayName = propertyName.Humanize();
            }

            return metadata;
        }
    }
    ```

- Register the provider during application start-up:

    ```csharp
    ModelMetadataProviders.Current = new HumanizingMetadataProvider();
    ```

- When using Humanizer in Razor views compiled by the legacy ASP.NET MVC pipeline, add the portable class library references (such as `System.Runtime`, `System.Globalization`) to `web.config` so the view compiler can resolve them. See the [Stack Overflow guidance](https://stackoverflow.com/a/19942274/738188) for the exact `<compilation>` entry.

## Background services and job processors

Worker services, hosted services, and message processors often run outside the scope of an HTTP request. Before humanizing values:

1. Set `CultureInfo.CurrentCulture` and `CultureInfo.CurrentUICulture` explicitly (for example, to the tenant or customer culture you are processing).
2. Register global strategies once at startup (see [Configuration](configuration.md)).
3. Avoid humanizing in tight loops; cache repeated phrases or humanize lazily when emitting notifications.

```csharp
public class InvoiceReminderHandler : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-GB");
        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-GB");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
```

## Desktop, mobile, and XAML apps

- Use Humanizer in data-binding scenarios by exposing humanized properties or value converters.
- For XAML, create an `IValueConverter` that wraps calls to `Humanize()`, allowing you to reuse the logic in bindings while keeping culture resolution centralized.
- In MAUI or Xamarin, set the culture per thread whenever the UI switches languages so that future humanization calls pick up the new culture.

## Command-line tools and automation

CLI utilities often balance machine-readability with human context. Recommended patterns:

- Provide a `--humanize` switch that toggles between exact values and humanized summaries.
- Compose humanized text with ANSI color output to highlight changes, but keep a verbose mode for scripting.
- Log both raw and humanized values for diagnostics: `logger.LogInformation("Processed {Count} ({Humanized}) files", count, count.ToQuantity("file"));`.

## Logging and telemetry

Humanized messages can help during incident response, but avoid losing the original data:

- Emit structured logs with both machine (`count`) and human (`count.ToMetric()`) fields.
- In dashboards or customer-facing status pages, humanize aggregate metrics such as durations or byte counts.
- When building alerts, humanize the threshold in the message while keeping numeric comparisons in code.

## Packaging and deployment

- Satellite language packages (`Humanizer.Core.<culture>`) must be deployed alongside your application binaries. For ASP.NET Core, confirm they are copied to the publish output (`dotnet publish`) by checking the `wwwroot` or deployment artifact.
- Verify that trimming or Native AOT scenarios keep the required resources. Add `<TrimmerRootAssembly Include="Humanizer" />` if you are trimming and rely on reflection-based discovery.
- For containerized workloads, install a recent .NET SDK or runtime image that includes the locale fixes described in [Installation](installation.md).

## Checklist

- [ ] Culture is set explicitly or inferred via ASP.NET Core localization middleware.
- [ ] Humanizer configuration runs once during startup and is reset in unit tests.
- [ ] Logs retain raw values when emitting humanized output.
- [ ] Language satellite assemblies are present in publish artifacts.
- [ ] UI strings combine localization and humanization consistently.

## Related topics

- [Configuration](configuration.md)
- [Localization](localization.md)
- [Numbers](numbers.md)
- [Dates & Times](dates-and-times.md)
- [Troubleshooting](troubleshooting.md)
