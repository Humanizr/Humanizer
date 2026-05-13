# Humanizer

.NET library for humanizing numbers, dates, times, enums, quantities, and more across 65 locale files.

## Quick Commands

```bash
# Build the main library (all target frameworks)
dotnet build src/Humanizer/Humanizer.csproj -c Release

# Build entire solution
dotnet build Humanizer.slnx -c Release

# Pack NuGet package
dotnet pack src/Humanizer/Humanizer.csproj -c Release -o artifacts

# Run tests (all three TFMs build on every platform; net48 test execution requires a Windows host)
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net11.0
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net48  # Windows only

# Run analyzer tests
dotnet test --project tests/Humanizer.Analyzers.Tests/Humanizer.Analyzers.Tests.Roslyn38.csproj
dotnet test --project tests/Humanizer.Analyzers.Tests/Humanizer.Analyzers.Tests.Roslyn48.csproj
dotnet test --project tests/Humanizer.Analyzers.Tests/Humanizer.Analyzers.Tests.Roslyn414.csproj

# Run source generator tests
dotnet test --project tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj

# Lint (verify formatting without changes)
dotnet format Humanizer.slnx --verify-no-changes --verbosity diagnostic

# Format (auto-fix)
dotnet format Humanizer.slnx

# Verify package structure after packing
pwsh tests/verify-packages.ps1 -PackagePath artifacts
```

## Project Structure

```
src/
  Humanizer/                    # Main library (net10.0, net8.0, net48, netstandard2.0)
    Locales/                    # 65 YAML locale definition files
    Configuration/              # Formatter/converter registries
  Humanizer.SourceGenerators/   # Roslyn source generators (YAML -> C# tables, library build-time only, not shipped)
  Humanizer.Analyzers/          # Roslyn analyzers shipped in NuGet package (namespace migration, API guidance)
  Benchmarks/                   # BenchmarkDotNet performance benchmarks
tests/
  Humanizer.Tests/              # Primary test suite (xUnit v3, 40k+ tests)
    Localisation/               # Culture-specific test folders
  Humanizer.Analyzers.Tests/    # Analyzer unit tests
  Humanizer.SourceGenerators.Tests/  # Source generator tests
  fixtures/                     # Package smoke tests (Console, Blazor, WebApi)
docs/                           # Jekyll documentation site
```

## Code Conventions

- Follow `.editorconfig` strictly: 4-space indentation, file-scoped namespaces, `var` for obvious types
- `EnforceCodeStyleInBuild=true` and `TreatWarningsAsErrors=true` are enabled globally
- Naming: camelCase private fields, PascalCase public members/constants/static readonly
- Use `System.*` usings first; prefer existing global usings
- Add XML documentation for new/modified public APIs
- No `try/catch` around imports, no unnecessary `this.`, no redundant braces on one-line blocks

## Testing

- Framework: xUnit v3 with Microsoft Testing Platform
- Use `UseCulture` attribute for culture-specific tests
- Place locale tests under `tests/Humanizer.Tests/Localisation/{culture}/`
- Snapshot testing via Verify library for golden-file comparisons

## Localization

- Locale data defined in YAML files under `src/Humanizer/Locales/`
- Source generators transform YAML into C# lookup tables at build time
- To add a locale: duplicate a YAML file and translate it; the source generator wires all registries automatically (see `docs/adding-a-locale.md`)
- When ICU-supplied data (month names, decimal separators) differs across platforms, author explicit overrides in `calendar:` and/or `number.formatting:` YAML surfaces
- See `docs/adding-a-locale.md` and `docs/locale-yaml-reference.md` for the full guide

## Key Config Files

- `global.json` - .NET SDK version (11.0.100-preview.3)
- `Directory.Build.props` - Shared MSBuild properties (nullable, warnings-as-errors, analyzers)
- `Directory.Packages.props` - Central package management (all NuGet versions)
- `version.json` - Nerdbank.GitVersioning semver config
- `.editorconfig` - Code style rules enforced at build time
