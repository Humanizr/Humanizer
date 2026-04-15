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
dotnet test --project tests/Humanizer.Analyzers.Tests/Humanizer.Analyzers.Tests.csproj

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

<!-- BEGIN FLOW-NEXT -->
## Flow-Next

This project uses Flow-Next for task tracking. Use `.flow/bin/flowctl` instead of markdown TODOs or TodoWrite.

**Quick commands:**
```bash
.flow/bin/flowctl list                # List all epics + tasks
.flow/bin/flowctl epics               # List all epics
.flow/bin/flowctl tasks --epic fn-N   # List tasks for epic
.flow/bin/flowctl ready --epic fn-N   # What's ready
.flow/bin/flowctl show fn-N.M         # View task
.flow/bin/flowctl start fn-N.M        # Claim task
.flow/bin/flowctl done fn-N.M --summary-file s.md --evidence-json e.json
```

**Creating a spec** ("create a spec", "spec out X", "write a spec for X"):

A spec = an epic. Create one directly — do NOT use `/flow-next:plan` (that breaks specs into tasks).

```bash
.flow/bin/flowctl epic create --title "Short title" --json
.flow/bin/flowctl epic set-plan <epic-id> --file - --json <<'EOF'
# Title

## Goal & Context
Why this exists, what problem it solves.

## Architecture & Data Models
System design, data flow, key components.

## API Contracts
Endpoints, interfaces, input/output shapes.

## Edge Cases & Constraints
Failure modes, limits, performance requirements.

## Acceptance Criteria
- [ ] Testable criterion 1
- [ ] Testable criterion 2

## Boundaries
What's explicitly out of scope.

## Decision Context
Why this approach over alternatives.
EOF
```

After creating a spec, choose next step:
- `/flow-next:plan <epic-id>` — research + break into tasks
- `/flow-next:interview <epic-id>` — deep Q&A to refine the spec

**Rules:**
- Use `.flow/bin/flowctl` for ALL task tracking
- Do NOT create markdown TODOs or use TodoWrite
- Re-anchor (re-read spec + status) before every task

**More info:** `.flow/bin/flowctl --help` or read `.flow/usage.md`
<!-- END FLOW-NEXT -->
