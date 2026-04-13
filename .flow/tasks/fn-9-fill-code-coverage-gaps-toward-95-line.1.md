## Description
Establish the honest coverage baseline. Emit `[ExcludeFromCodeCoverage]` from FluentDate T4 templates (targeted at T4-generated types and members only — hand-authored `In.TheYear` / `InDate.TheYear` / `PrepositionsExtensions.*` remain instrumented). Add `coverage.runsettings` preserving the effective current `testconfig.json` configuration. Run merged coverage across all three test projects on Windows CI (authoritative) and commit a baseline artifact. Epic's early proof point.

**Size:** M
**Files:**
- `coverage.runsettings` (new, repo root) — Microsoft Code Coverage `.runsettings` (NOT coverlet)
- `src/Humanizer/FluentDate/In.Months.tt`, `In.SomeTimeFrom.tt`, `InDate.Months.tt`, `InDate.SomeTimeFrom.tt`, `On.Days.tt`, `OnDate.Days.tt` (the six T4 templates) — emit attribute on each generated nested type and each generated member of the outer partial class
- Regenerated `.cs` outputs (`In.Months.cs`, `In.SomeTimeFrom.cs`, `InDate.Months.cs`, `InDate.SomeTimeFrom.cs`, `On.Days.cs`, `OnDate.Days.cs`) — committed
- `azure-pipelines.yml:71-92` — pass `--coverage-settings coverage.runsettings` to `dotnet test`; add `XmlSummary` to the ReportGenerator `reporttypes` list (task .11 consumes it for the gate, but `.1` makes the output available)
- `CLAUDE.md`, `AGENTS.md`, `docs/adding-a-locale.md` — `dotnet test` command examples (command only; threshold numbers live in .11)
- `artifacts/fn-9-baseline/Summary.txt`, `Summary.xml`, `uncovered.json` (new, committed)

## Approach
- **Attribute emission (primary exclusion).** The six FluentDate `.tt` files produce generated code that either (a) adds members to the outer `public partial class In` / `InDate` / `On` / `OnDate` or (b) declares nested static classes (e.g. `In.January`, `On.April`, `InDate.Five`). Update each `.tt` to emit `[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "Generated trivial DateTime factories")]`:
  - on every generated **nested static class**
  - on every generated **static member** (property, method) added to the outer partial class
  Do NOT emit the attribute on the outer `partial class` declaration — that would cover hand-authored `TheYear` (In.cs, InDate.cs) and remove it from coverage. Partial-class attribute semantics: attribute on any partial applies to the entire class type since .NET 5, so attaching to the outer declaration anywhere would wipe the hand-authored coverage too.
  Do NOT modify `In.cs`, `InDate.cs`, `PrepositionsExtensions.cs`.
- **coverage.runsettings (secondary).** Uses Microsoft Code Coverage `<Configuration><CodeCoverage>` schema. Must replicate the effective content of current per-project `testconfig.json`:
  - `DeterministicReport=true`
  - `ExcludeAssembliesWithoutSources=None`
  - `Format=cobertura`
  - `ModulePaths.Include`: `.*Humanizer(\\.(Analyzers|SourceGenerators))?\\.dll$`
  - `Attributes.Exclude`: `^System\\.Diagnostics\\.DebuggerHiddenAttribute$`, `^System\\.Diagnostics\\.DebuggerNonUserCodeAttribute$`, `^System\\.CodeDom\\.Compiler\\.GeneratedCodeAttribute$`, `^System\\.Diagnostics\\.CodeAnalysis\\.ExcludeFromCodeCoverageAttribute$`
  Adding `CompilerGeneratedAttribute` is **not** required — current config does not list it and source-gen YAML tables are kept in coverage per PR #1715.
  No source-path globs for FluentDate — attribute emission handles that.
  No per-assembly gate exclusion in the runsettings — the gate script in .11 owns gating.
- **Baseline collection.**
  - Authoritative: Windows CI run (the only OS that executes `net48`). Commit the Windows-CI-produced artifact as the baseline for downstream tasks.
  - Informational: local macOS/Linux runs at net10.0 + net8.0 are useful for iteration but are NOT the source of truth. The baseline artifact header includes the OS + TFM list it covers.
  - Produce: `artifacts/fn-9-baseline/Summary.txt` (ReportGenerator TextSummary), `Summary.xml` (XmlSummary), `uncovered.json` (per-class uncovered lines/branches with optional `reachability` annotations for the three suspect branches).
- **Suspect-unreachable investigation.** Record in `uncovered.json` under a `notes` field for each of:
  - `HeadingTableCatalog.Invariant` / `invariantCache`
  - `OrdinalDatePattern.GetPatternCulture` AOORE fallback at `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs:270-285`
  - `WordsToNumberMigrationCodeFixProvider` `root is null` and `semanticModel is null` guards
  Either "reachable via <public API call>" (extend the owning task to cover) or "unreachable — <justification>" (recorded in the epic's declared-unreachable appendix). No action on source required; the gate in .11 absorbs these in the aggregate threshold headroom.

## Investigation targets
**Required:**
- `artifacts/ci-coverage-report/Summary.txt` — prior baseline
- `tests/Humanizer.Tests/testconfig.json:1-40` and sibling testconfig.json files in Analyzers.Tests / SourceGenerators.Tests — coverage configuration to preserve
- `azure-pipelines.yml:71-92` — current test + ReportGenerator steps; `reporttypes` line to amend
- Six `.tt` templates listed above
- `src/Humanizer/FluentDate/In.cs`, `InDate.cs`, `PrepositionsExtensions.cs` — hand-authored files to leave alone

**Optional:**
- `https://learn.microsoft.com/visualstudio/test/customizing-code-coverage-analysis` — Microsoft Code Coverage `.runsettings` schema
- `https://github.com/App-vNext/Polly/blob/main/eng/Test.targets` — precedent for attribute-driven exclusion

## Acceptance
- [ ] Only the six T4 templates listed above are modified; each regenerated `.cs` output emits `[ExcludeFromCodeCoverage(Justification = "Generated trivial DateTime factories")]` on every generated nested type and every generated member of the outer partial class.
- [ ] `In.cs`, `InDate.cs`, `PrepositionsExtensions.cs` are unchanged; `In.TheYear`, `InDate.TheYear`, `PrepositionsExtensions.*` remain in the coverage report (and covered).
- [ ] `coverage.runsettings` exists at repo root using Microsoft Code Coverage schema and replicates the current `testconfig.json` effective coverage configuration (DeterministicReport, ExcludeAssembliesWithoutSources, Format=cobertura, ModulePaths.Include filter, and the four attribute exclusions — DebuggerHidden, DebuggerNonUserCode, GeneratedCode, ExcludeFromCodeCoverage).
- [ ] `azure-pipelines.yml` passes `--coverage-settings coverage.runsettings` to `dotnet test` AND includes `XmlSummary` in ReportGenerator's `reporttypes`.
- [ ] `CLAUDE.md`, `AGENTS.md`, `docs/adding-a-locale.md` reference `--coverage-settings coverage.runsettings` in their `dotnet test` examples (threshold numbers added by .11).
- [ ] Authoritative Windows-CI run produces `artifacts/fn-9-baseline/Summary.txt`, `Summary.xml`, `uncovered.json` covering all three test projects and all supported TFMs (`net10.0`, `net8.0`, `net48`); the artifact header names the OS + TFM set.
- [ ] `uncovered.json` includes `notes` entries for the three suspect branches documenting reachability.
- [ ] Merged report contains zero `Humanizer.In.*`, `Humanizer.InDate.*`, `Humanizer.On.*`, `Humanizer.OnDate.*` nested-type classes; the outer `Humanizer.In` / `Humanizer.InDate` / `Humanizer.On` / `Humanizer.OnDate` appear with only their hand-authored members counted.
- [ ] `Humanizer` assembly line coverage in the new baseline is measurably higher than the prior 91.3% (instrumentation change alone).
- [ ] No behavior change outside T4 attribute emission and the runsettings file.

## Done summary
_To be filled on completion._

## Evidence
- Commits:
- Tests:
- PRs:
