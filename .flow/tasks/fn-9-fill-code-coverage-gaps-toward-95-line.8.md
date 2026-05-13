## Description
Cover source-generator diagnostic and diff branches in `Humanizer.SourceGenerators`. Classes at 70-88%: `LocaleSemanticDiff` (72.6%), `CanonicalLocaleAuthoring` (80.1%), `LocaleCatalogInput` (75.5%), `LocaleDefinitionFile` (73.3%). These only light up when YAML fixtures exercise malformed/unknown-key/conflict paths.

Also owns the **shared fixture helper + csproj resource inclusion** that task .9 will build on.

**Size:** M
**Files:**
- `tests/Humanizer.SourceGenerators.Tests/Fixtures/CanonicalDiagnostics/` (new subdirectory) — one YAML fixture per branch
- `tests/Humanizer.SourceGenerators.Tests/SourceGenerators/LocaleCanonicalFixtureTests.cs` (new)
- `tests/Humanizer.SourceGenerators.Tests/SourceGenerators/FixtureLoader.cs` (new) — shared helper that reads fixtures from disk (or embedded resource) and wraps them as `AdditionalText`
- `tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj` — embed fixtures as content/resources so they reach the build output

## Approach
- Follow existing `CSharpGeneratorDriver` + `AdditionalText` pattern from `tests/Humanizer.SourceGenerators.Tests/SourceGenerators/HumanizerSourceGeneratorTests.cs`.
- One `[Theory]` per fixture with `[MemberData]` enumerating the fixtures under `Fixtures/CanonicalDiagnostics/`.
- `LocaleSemanticDiff.Compare` returns **human-readable strings**, not typed diff kinds. Assert on **exact substring or exact string match** of the diff message text as produced by the current implementation.
- Target every reachable branch in:
  - `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs:LocaleSemanticDiff` (complexity ~80)
  - `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs:CanonicalLocaleAuthoring` (complexity ~173)
  - `src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs:LocaleCatalogInput` (complexity ~268)
  - `src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs:LocaleDefinitionFile`
- Fixture files target one branch each; filename matches the branch description. Size discipline: **only the YAML fields required by that branch** — no arbitrary line-count cap. For diff tests that need two YAML blobs, put them in a `pair/` subfolder alongside each other.
- Shared `FixtureLoader` helper owns: reading fixtures from disk, wrapping as `AdditionalText`, and exposing a `TheoryData<string, string>` enumerator. `.9` will reuse this helper, not reimplement it.

## Investigation targets
**Required:**
- `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs` (full)
- `src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs` (full)
- `tests/Humanizer.SourceGenerators.Tests/SourceGenerators/HumanizerSourceGeneratorTests.cs:1-400` — driver pattern, in-memory `AdditionalText` setup
- `tests/Humanizer.SourceGenerators.Tests/SourceGenerators/CanonicalLocaleSchemaTests.cs` — existing schema-test shape
- `tests/Humanizer.SourceGenerators.Tests/SourceGenerators/LocalePhraseNormalizationTests.cs` — normalization-test shape
- `artifacts/fn-9-baseline/uncovered.json` (from .1)

## Acceptance
- [ ] `LocaleSemanticDiff` reaches ≥90% line and ≥80% branch.
- [ ] `CanonicalLocaleAuthoring` reaches ≥90% line and ≥80% branch.
- [ ] `LocaleCatalogInput` reaches ≥90% line and ≥80% branch.
- [ ] `LocaleDefinitionFile` reaches ≥90% line and ≥80% branch.
- [ ] Each new fixture file targets exactly one named branch; filename describes the branch.
- [ ] `FixtureLoader` helper is reusable (`.9` consumes it unmodified).
- [ ] Assertions use exact or substring match on the actual string output of `LocaleSemanticDiff.Compare` — no fabricated "diff kind" enums.
- [ ] No modification to any shipped YAML in `src/Humanizer/Locales/`.
- [ ] SG coverage thresholds are task-level acceptance only — **not gated by CI** (the gate script in .11 excludes `Humanizer.SourceGenerators`).

## Done summary
Added source-generator schema diagnostics coverage for unsupported canonical properties, invalid number formatting, calendar override exception branches, list template validation, legacy migration edge cases, and semantic diff left/right-only locales. CanonicalLocaleAuthoring reached 91.02% line / 90.37% branch and LocaleYamlCatalog reached 93.75% line / 87.58% branch in the captured coverage report.
## Evidence
- Commits:
- Tests: dotnet test tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj -c Release -- --coverage --coverage-output-format cobertura, dotnet test tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj -c Release
- PRs: