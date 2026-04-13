# Fill code coverage gaps toward 95% line / 88% branch

## Overview

The merged multi-TFM coverage report at `artifacts/ci-coverage-report/Summary.txt` (2026-04-10, "Parser: MultiReport (3x Cobertura)") shows 91.3% line / 80.4% branch / 71.3% method across 874 classes in three assemblies (`Humanizer`, `Humanizer.Analyzers`, `Humanizer.SourceGenerators`). This epic closes the genuine-logic gaps, excludes trivial T4-generated FluentDate sugar via targeted attribute emission, and enforces the result as a CI gate via a dedicated script that parses ReportGenerator's `Summary.xml`.

Dependency on `fn-8-add-urdu-ur-locale-with-full-language`: fn-8.12–14 audit gender-bearing locale tests (cs, pl, ru, ar, he, hi). Because fn-9 contains no deferrals, fn-9 starts only after fn-8 closes — there is then no hold list; every shipping locale is fair game.

**Branching model (stacked PR).** fn-9 ships as a separate pull request stacked on fn-8. Branch creation:

```bash
git fetch origin
git checkout feat/urdu-locale              # fn-8's branch
git checkout -b feat/coverage-gaps         # fn-9's branch
```

The fn-9 PR targets `feat/urdu-locale` as its base (not `main`). If fn-8 merges into `main` before fn-9 is ready, rebase onto main:

```bash
git fetch origin
git rebase origin/main                     # after fn-8 merges
```

Until then, the PR diff shows only fn-9 changes on top of fn-8.

Baseline infrastructure already in place: xUnit v3 + Microsoft Testing Platform, `UseCultureAttribute`, Verify snapshots, `CSharpAnalyzerTest<,>` / `CSharpCodeFixTest<,,>` verifiers, `CSharpGeneratorDriver` pattern in source-generator tests, per-project `testconfig.json` listing four attribute exclusions (`DebuggerHiddenAttribute`, `DebuggerNonUserCodeAttribute`, `GeneratedCodeAttribute`, `ExcludeFromCodeCoverageAttribute`) and a cobertura module-path include filter. PR #1715 deliberately kept generated YAML tables in coverage — preserve that direction.

## Scope

**In scope**
- Emit `[ExcludeFromCodeCoverage(Justification = "Generated trivial DateTime factories")]` from the FluentDate `.tt` templates at (a) each fully-generated nested static class and (b) each generated member of the outer `public partial class` declarations, preserving hand-authored `In.TheYear` / `InDate.TheYear` coverage and leaving `PrepositionsExtensions.cs` untouched.
- Add `coverage.runsettings` (Microsoft Code Coverage / MTP schema — NOT coverlet) preserving the effective current `testconfig.json` configuration (DeterministicReport, ExcludeAssembliesWithoutSources=None, Format=cobertura, ModulePaths.Include for `Humanizer`/`Humanizer.Analyzers`/`Humanizer.SourceGenerators`, four attribute exclusions).
- Produce a baseline artifact in task .1 at `artifacts/fn-9-baseline/` listing uncovered lines per class per assembly.
- Author real tests for every gap bucket.
- Write a dedicated **gate script** parsing ReportGenerator's `Summary.xml` (XmlSummary) and applying per-assembly thresholds.
- Wire the gate into `azure-pipelines.yml`; add `XmlSummary` to `reporttypes`.
- Contributor docs (`CLAUDE.md`, `AGENTS.md`, `docs/adding-a-locale.md`) reference the gate-script header as the canonical source of threshold numbers — no numeric duplication in docs.

**Out of scope**
- Stryker.NET mutation testing.
- Any behavior change outside tests + T4 attribute emission + runsettings + gate script. No code deletions, no dead-code removal, no refactors.
- Gate-level line-range subtraction or per-class carve-outs. Unreachable-branch lines are absorbed in threshold headroom (total size <0.1% of coverable lines).
- `Benchmarks/` project coverage.

## Approach

- **Trivial DSL exclusion.** Edit the six FluentDate `.tt` templates (`In.Months.tt`, `In.SomeTimeFrom.tt`, `InDate.Months.tt`, `InDate.SomeTimeFrom.tt`, `On.Days.tt`, `OnDate.Days.tt`) so each regenerated `.cs` output emits `[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "Generated trivial DateTime factories")]` on every generated nested type AND on every generated member of the outer partial class. Hand-authored `In.cs`, `InDate.cs`, `PrepositionsExtensions.cs` are untouched. Attribute emission is the sole FluentDate exclusion mechanism.
- **Baseline artifact.** Task .1 runs the full merged coverage on the authoritative OS target — Windows, where `net48` can run. Commits `artifacts/fn-9-baseline/Summary.txt`, `Summary.xml`, `uncovered.json` with per-class uncovered lines/branches + reachability notes for the three declared-unreachable branches.
- **Default fallback converters.** Instantiate directly; clone `CultureInfo` with `ShortDatePattern` embedding literal bidi marks for OS-independent mark-stripping tests.
- **Registry default-factory path.** Use `eo` (Esperanto) — the known-unregistered culture used by `HeadingTests.ToHeadingFallsBackToEnglishForUnknownCultures:208`.
- **Branch-level gaps.** Consume `uncovered.json`; author targeted `[Theory]` / `[Fact]` inputs.
- **Analyzer duality.** Task `.7` wires dual-Roslyn instrumentation (infrastructure only); task `.16` writes branch tests on top. No escape hatch: if dual instrumentation proves infeasible, `.7` halts and escalates to this epic for scope amendment.
- **Source generator error branches.** Synthetic YAML fixtures; `.8` owns `Fixtures/CanonicalDiagnostics/` + shared `FixtureLoader`; `.9` consumes `FixtureLoader` and adds `Fixtures/EngineContracts/`; factories fall through to conventional expressions on unknown kinds (no diagnostic) — assertions target generated source.
- **Gate.** Task `.11` parses `Summary.xml` for per-assembly thresholds; unit-tested against synthesized fixtures.

## Quick commands

```bash
# Full merged coverage locally (Windows for authoritative; macOS/Linux informational)
dotnet test Humanizer.slnx -c Release /p:EmitCompilerGeneratedFiles=true \
  -- --coverage --coverage-output-format cobertura \
  --coverage-settings coverage.runsettings --xunit-info

# Regenerate merged ReportGenerator output (XmlSummary required for gate)
reportgenerator \
  -reports:"**/*.cobertura.xml" \
  -targetdir:artifacts/local-coverage \
  -reporttypes:"TextSummary;Cobertura;XmlSummary;Html"

# Apply the gate locally
pwsh scripts/coverage-gate.ps1 -SummaryXmlPath artifacts/local-coverage/Summary.xml
```

## Acceptance

- [ ] FluentDate `.tt` templates emit `[ExcludeFromCodeCoverage(Justification = "Generated trivial DateTime factories")]` on each regenerated nested type and each regenerated member of the outer partial classes; hand-authored `In.TheYear`, `InDate.TheYear`, `PrepositionsExtensions.*` remain instrumented and tested.
- [ ] Generated nested FluentDate classes (e.g. `Humanizer.In.January`, `Humanizer.In.Eight`, `Humanizer.InDate.Five`, `Humanizer.On.April`, `Humanizer.OnDate.September`) are absent from the merged coverage report. The outer partial classes `Humanizer.In`, `Humanizer.InDate`, `Humanizer.On`, `Humanizer.OnDate` remain present with only hand-authored members counted.
- [ ] `coverage.runsettings` exists at repo root using Microsoft Code Coverage schema, replicating `testconfig.json` effective config (DeterministicReport, ExcludeAssembliesWithoutSources=None, Format=cobertura, ModulePaths.Include, four attribute exclusions).
- [ ] `azure-pipelines.yml` passes `--coverage-settings coverage.runsettings` to `dotnet test` AND includes `XmlSummary` in ReportGenerator's `reporttypes`.
- [ ] `CLAUDE.md`, `AGENTS.md`, `docs/adding-a-locale.md` reference `--coverage-settings coverage.runsettings` in `dotnet test` examples (from .1). For threshold numbers, these docs reference the gate-script header as canonical source — no numeric duplication (from .11).
- [ ] `artifacts/fn-9-baseline/` (`Summary.txt`, `Summary.xml`, `uncovered.json`) is produced by .1, authoritative on Windows CI with all three test projects × all supported TFMs.
- [ ] All three `Default*` fallback converters reach ≥95% line / ≥90% branch.
- [ ] `TokenMapWordsToNumberOrdinalMapBuilder` exercises all three `TokenMapOrdinalGenderVariant` cases across both `Build` overloads.
- [ ] `WordsToNumberExtension.TryToNumber` overloads reach ≥95% line; tests match actual signatures and behaviors (no fabricated null/empty contract).
- [ ] `SuffixScaleWordsToNumberConverter`, `OrdinalDatePattern` (reachable branches — every day-mode arm covered), `LocalePhraseTable`, `DelimitedCollectionFormatter` (six generic `Humanize<T>` overloads), `CliticCollectionFormatter`, `NoMatchFoundException` (three public ctors; no serialization ctor) reach ≥90% line / ≥85% branch.
- [ ] `Humanizer.Analyzers.*` classes reach ≥95% line / ≥85% branch in the merged report with BOTH Roslyn arms instrumented via dual-Roslyn infrastructure from .7.
- [ ] `Humanizer.SourceGenerators.*` internal types reach ≥90% line / ≥80% branch as task-level acceptance in .8 / .9; the CI gate does NOT enforce thresholds on `Humanizer.SourceGenerators` (report-only).
- [ ] Final gate script enforces `Humanizer` ≥95 line / ≥88 branch / ≥85 method; `Humanizer.Analyzers` ≥95 line / ≥85 branch.
- [ ] Gate script has unit tests against synthesized `Summary.xml` fixtures; no smoke-commit requirement.
- [ ] `azure-pipelines.yml` invokes the gate; build fails on gate failure.
- [ ] No deferrals, no hold lists, no escape hatches in task acceptance. If a task encounters technical infeasibility, it returns to this epic for explicit scope amendment.

## Declared-unreachable branches (informational appendix)

These are documented by .1 as unreachable via the current public API. They are **not** gate-filtered; they are **absorbed** in the aggregate thresholds (combined size <0.1% of coverable lines).

- `Humanizer.HeadingTableCatalog.Invariant` / `invariantCache` fallback — English heading data always matches `ResolveCore("en")`.
- `OrdinalDatePattern.GetPatternCulture` `ArgumentOutOfRangeException` fallback at `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs:270-285`.
- `WordsToNumberMigrationCodeFixProvider` `root is null` / `semanticModel is null` guards.

If `.1`'s baseline proves any reachable, the owning task extends to cover them and the appendix entry is removed. Tasks may NOT add new Boundaries items unilaterally — any new item requires epic-scope amendment.

## Early proof point

Task `.1`: T4 attribute emission + `coverage.runsettings` + full merged Windows-CI run producing the baseline artifact. If FluentDate nested classes still appear in the merged report, or if any test project fails to emit cobertura, re-evaluate the exclusion mechanism before starting `.2+`.

## Requirement coverage

| Req | Description | Task(s) | Gap justification |
|-----|-------------|---------|-------------------|
| R1  | Runsettings + FluentDate T4 attribute + baseline artifact + command docs | .1 | — |
| R2  | Default fallback converter tests | .2 | depends on .1 |
| R3  | Registry default-factory paths (via `eo`) | .3 | depends on .1 |
| R4  | WordsToNumber surface + ordinal-map builder (3×2 matrix) + SuffixScale | .4 | depends on .1 |
| R5  | OrdinalDatePattern (every reachable branch) + NoMatchFoundException | .5 | depends on .1 |
| R6  | Formatter branches (LocalePhraseTable, Delimited 6 overloads, Clitic) | .6 | depends on .1 |
| R7  | Dual-Roslyn instrumentation infrastructure | .7 | depends on .1 |
| R8  | SG canonical/diff/catalog diagnostic branches + shared FixtureLoader | .8 | depends on .1 |
| R9  | SG engine-contract-factory dispatch + member-kind + fallthrough | .9 | depends on .1, .8 |
| R10 | Converter tail: core extensions (5 classes) | .10 | depends on .1 |
| R11 | Gate script + CI wiring + threshold docs | .11 | depends on .1–.10, .12–.16 |
| R12 | Converter tail: ByteSize + Truncators (6 classes) | .12 | depends on .1 |
| R13 | Converter tail: NumberToWords scale/gender family (8 classes) | .13 | depends on .1 |
| R14 | Converter tail: WordsToNumber (5 classes) + TimeSpan | .14 | depends on .1 |
| R15 | Converter tail: ordinal NumberToWords + PhraseClockNotation + WordFormTemplateOrdinalizer (4 classes) | .15 | depends on .1 |
| R16 | Analyzer branch tests on dual-Roslyn matrix | .16 | depends on .1, .7 |
