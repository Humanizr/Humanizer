# Troubleshooting

Use this guide to diagnose common issues that arise when installing, configuring, or running Humanizer in production applications.

## Missing locale resources

**Symptoms**
- `MissingManifestResourceException`
- Humanizer falls back to English despite specifying another culture

**Resolution**
- Install the corresponding `Humanizer.Core.<culture>` package or use the `Humanizer` meta-package that bundles all locales.
- Confirm the satellite assembly exists in the publish output (`dotnet publish` or your build artifacts).
- In trimming scenarios, add `<TrimmerRootAssembly Include="Humanizer" />` to prevent the linker from removing resource types.

## Restore failures with old SDKs

**Symptoms**
- `NU1004` errors mentioning unrecognized locales during `dotnet restore`

**Resolution**
- Upgrade to .NET SDK 9.0.200 or newer, or use Visual Studio/MSBuild builds that contain the locale parsing fix.
- As a workaround, reference `Humanizer.Core` plus specific `Humanizer.Core.<culture>` packages instead of the `Humanizer` meta-package.

## Unexpected English output

**Symptoms**
- `DateTime.Humanize()` or `ToWords()` returns English text even after setting the culture.

**Resolution**
- Ensure both `CultureInfo.CurrentCulture` **and** `CultureInfo.CurrentUICulture` are set before calling Humanizer.
- When running parallel tasks, set the culture inside each task; culture flow is not automatic across all thread pools.
- Verify that the target culture is supported. Consult [Localization](localization.md) for the feature matrix.

## `NotImplementedException` for language features

**Symptoms**
- Calling `ToOrdinalWords()` or `ToWords()` throws `NotImplementedException` for a specific language.

**Resolution**
- Some languages intentionally ship with partial feature support. Check whether ordinal or cardinal conversions exist for the target culture.
- Consider contributing the missing implementation (see [Contributing](../.github/CONTRIBUTING.md)).
- Provide a fallback by wrapping calls in `try/catch` and defaulting to English or a neutral culture.

## Configuration side effects

**Symptoms**
- Humanizer behaves differently across requests or test runs.

**Resolution**
- Configuration is global; set strategies and formatters during application startup only.
- In tests, capture the original configuration values, assign overrides for the test, and restore the originals during teardown.
- Avoid mutating `Configurator` inside request handlers or controllers.

## Trimming or AOT issues

**Symptoms**
- `MissingMethodException` or runtime failures when publishing with trimming or Native AOT.

**Resolution**
- Humanizer relies on reflection to discover certain converters. Add `DynamicDependency` attributes or `TrimmerRootAssembly` entries targeting `Humanizer` and `Humanizer.Core`.
- Validate trimming results by running `dotnet publish -c Release -p:PublishTrimmed=true` and executing integration tests against the output.

## DocFX build failures

**Symptoms**
- `docfx metadata` reports unresolved cross references or missing files.

**Resolution**
- Run `dotnet tool restore` to ensure the DocFX CLI is installed.
- Verify that new Markdown files are listed in `docs/toc.yml` and that links use the correct relative paths.
- Rebuild with `dotnet tool run docfx build docs/docfx.json` after fixing broken references.

## Checklist before release

- [ ] Restore succeeds on supported SDKs and build agents.
- [ ] Application includes the correct satellite packages for each culture.
- [ ] Global configuration executes once and is covered by tests.
- [ ] Humanized output verified for supported cultures.
- [ ] Documentation builds cleanly with DocFX.

## Related topics

- [Installation](installation.md)
- [Localization](localization.md)
- [Configuration](configuration.md)
- [Testing and Quality Assurance](testing.md)
- [Performance and Optimization](performance.md)
