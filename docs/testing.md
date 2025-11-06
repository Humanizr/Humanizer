# Testing and Quality Assurance

A reliable Humanizer integration includes automated tests that lock down localized output, configuration, and regression scenarios. This guide shows how to exercise Humanizer features with unit tests, integration checks, and continuous integration (CI) pipelines.

## Unit testing patterns

### Pin cultures with `UseCulture`

Create a reusable attribute that switches `CultureInfo.CurrentCulture` and `CultureInfo.CurrentUICulture` for the duration of a test. Humanizer ships a sample implementation inside the test project (see [`UseCultureAttribute.cs` on GitHub](https://github.com/Humanizr/Humanizer/blob/main/src/Humanizer.Tests/UseCultureAttribute.cs)); you can copy the approach:

```csharp
public sealed class UseCultureAttribute : BeforeAfterTestAttribute
{
    private readonly string _culture;
    private CultureInfo? _originalCulture;
    private CultureInfo? _originalUICulture;

    public UseCultureAttribute(string culture) => _culture = culture;

    public override void Before(MethodInfo methodUnderTest)
    {
        _originalCulture = CultureInfo.CurrentCulture;
        _originalUICulture = CultureInfo.CurrentUICulture;

        var cultureInfo = CultureInfo.GetCultureInfo(_culture);
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
    }

    public override void After(MethodInfo methodUnderTest)
    {
        CultureInfo.CurrentCulture = _originalCulture!;
        CultureInfo.CurrentUICulture = _originalUICulture!;
    }
}
```

Use the attribute to isolate tests:

```csharp
public class DateHumanizerTests
{
    [Theory, UseCulture("en-US")]
    [InlineData(-1, "yesterday")]
    [InlineData(0, "today")]
    [InlineData(1, "tomorrow")]
    public void HumanizeDate(int offsetDays, string expected)
    {
        var value = DateTime.Today.AddDays(offsetDays);
        Assert.Equal(expected, value.Humanize(dateToCompareAgainst: DateTime.Today));
    }
}
```

### Reset configuration between tests

Humanizer configuration is global. Capture the original value, assign your test override, and restore the previous value inside `Dispose`:

```csharp
public sealed class HumanizerConfigurationTests : IDisposable
{
    private readonly IDateTimeHumanizeStrategy _originalStrategy = Configurator.DateTimeHumanizeStrategy;

    public HumanizerConfigurationTests()
    {
        Configurator.DateTimeHumanizeStrategy = new PrecisionDateTimeHumanizeStrategy(precision: 0.25m);
    }

    [Fact]
    public void Uses_custom_strategy()
    {
        var comparison = DateTime.UtcNow;
        var value = comparison.AddMinutes(-90);

        var text = value.Humanize(dateToCompareAgainst: comparison);

        Assert.Equal("an hour ago", text);
    }

    public void Dispose()
    {
        Configurator.DateTimeHumanizeStrategy = _originalStrategy;
    }
}
```

### Snapshot and golden tests

- For APIs that return humanized strings, consider snapshot testing to detect wording changes.
- Normalize whitespace and punctuation before assertions to avoid brittle comparisons across cultures.
- When testing `ToNumber()` and other parsers, assert both success and failure paths to cover diagnostics.

## Localization verification

1. Install the relevant `Humanizer.Core.<culture>` package into your test project.
2. Use `[Theory]` + `[UseCulture]` to provide expectations per culture.
3. For languages with gender or grammatical cases, assert each variation:

```csharp
[Theory]
[InlineData(GrammaticalGender.Masculine, "primero")]
[InlineData(GrammaticalGender.Feminine, "primera")]
[UseCulture("es")]
public void Ordinals_respect_gender(GrammaticalGender gender, string expected)
{
    Assert.Equal(expected, 1.ToOrdinalWords(gender));
}
```

4. Run locale-specific tests in CI only when the corresponding satellite package is present to avoid false negatives on agents that lack ICU data.

## Integration tests

- Exercise full request pipelines by humanizing values inside controllers or Razor pages.
- Verify logging: assert that structured log events include both the machine-readable value and the humanized string.
- Confirm DTOs exposed over HTTP still contain raw numeric or date values in addition to humanized descriptions.

## Performance regression checks

- Record baseline timings for hot code paths that call Humanizer. A simple benchmark using `BenchmarkDotNet` can catch regressions introduced by configuration changes.
- Consider caching heavy transformations (for example, large enum mappings) and add unit tests demonstrating the cache policy.

## Continuous integration

Include Humanizer coverage in your CI workflow:

```bash
# Run the Humanizer test suite across supported frameworks
dotnet test src/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
dotnet test src/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0

# Regenerate documentation as part of verification
dotnet tool restore
dotnet tool run docfx build docs/docfx.json
```

> [!TIP]
> If your application supports additional frameworks (for example, net9.0 or net48), add them to your test matrix to validate cultural behavior consistently across targets.

## Manual QA checklist

- [ ] Confirm localized output for each supported culture.
- [ ] Verify humanized strings in UI screens, emails, and notifications.
- [ ] Validate logging and telemetry contain both raw values and humanized summaries.
- [ ] Rebuild DocFX docs and skim feature guides for outdated snippets.
- [ ] Review `Configurator` usage for thread safety and ensure tests restore any overridden settings.

## Related topics

- [Localization](localization.md)
- [Configuration](configuration.md)
- [Troubleshooting](troubleshooting.md)
