## Description
Enforce the hard coverage gate in CI via a dedicated gate script that parses ReportGenerator's `Summary.xml` (XmlSummary report type). Add a Coverage-gate section to contributor docs referencing the gate-script header as the canonical source of threshold numbers (no numeric duplication in docs). Depends on .1–.10 + .12–.16 closing.

**Size:** M
**Files:**
- `scripts/coverage-gate.ps1` (new, PowerShell 7+) — or `scripts/coverage-gate.cs` as a file-based `.NET 10 dotnet run` app. Pick one; prefer whichever matches repo convention.
- `scripts/coverage-gate.Tests.ps1` or `scripts/coverage-gate.Tests.cs` (new) — unit tests
- `scripts/coverage-gate-fixtures/` (new) — synthesized `Summary.xml` inputs: one that passes, one that fails each axis (line, branch, method, missing-assembly, malformed-xml)
- `azure-pipelines.yml` — gate step after ReportGenerator; verify `XmlSummary` is in `reporttypes` (added in .1 but re-confirm)
- `CLAUDE.md`, `AGENTS.md` — "Coverage gate" section referencing the gate-script header (canonical thresholds live there) and the gate-script path; no numeric duplication
- `docs/adding-a-locale.md` — contributor note referencing the gate
- `readme.md` — optional coverage badge (skip if no public coverage service wired)

## Approach
- **Gate mechanism.** ReportGenerator's `XmlSummary` produces a per-assembly summary with `linecoverage`, `branchcoverage`, `methodcoverage`, `fullmethodcoverage` attributes. The gate script parses that XML, selects the elements for `Humanizer` and `Humanizer.Analyzers`, and applies:
  - `Humanizer`: line ≥95, branch ≥88, method ≥85
  - `Humanizer.Analyzers`: line ≥95, branch ≥85 (method waived — code-fix providers are short)
  - `Humanizer.SourceGenerators`: **report-only** (not gated)
- The gate consumes the published ReportGenerator numbers exactly as `Summary.xml` reports them — no line-range subtraction, no per-class filter. The epic absorbs a handful of unreachable lines into threshold headroom (their impact on aggregate percentage is <0.1%).
- **Unit tests.** Fabricate `Summary.xml` inputs covering each failure axis. Assert the script exits 0 on pass and non-zero with an explicit reason on fail. No reliance on production coverage numbers; no "intentional smoke commit" acceptance.
- **Pipeline integration.** Add a `- task: PowerShell@2` step (or a `- script:` step running `dotnet run scripts/coverage-gate.cs`) after the `reportgenerator@5` step. Pass `-SummaryXmlPath $(Build.ArtifactStagingDirectory)/coverageReports/Summary.xml`. Step's `failOnStderr: true` or explicit `exit $LASTEXITCODE`.
- **Docs.** The canonical threshold list lives in the gate-script header comment. `CLAUDE.md`, `AGENTS.md`, `docs/adding-a-locale.md` link to that header and do not duplicate numbers.

## Investigation targets
**Required:**
- `azure-pipelines.yml:71-102` — current test / ReportGenerator / fail-on-issue wiring
- `artifacts/fn-9-local-coverage/Summary.xml` (produced by .1) — canonical format of ReportGenerator `XmlSummary`
- `https://reportgenerator.io/usage` — `XmlSummary` report type reference
- Final fresh `Summary.xml` produced after .2–.10 + .12–.15 — source of truth for achievable thresholds
- `CLAUDE.md:Quick Commands` — existing format conventions

## Acceptance
- [ ] Gate script exists and exits non-zero with an explicit reason when any of these fails:
  - `Humanizer` line <95 or branch <88 or method <85
  - `Humanizer.Analyzers` line <95 or branch <85
  - `Summary.xml` missing or malformed or missing a required assembly element
- [ ] `Humanizer.SourceGenerators` is read from `Summary.xml` and reported to stdout but never fails the gate.
- [ ] Gate-script unit tests cover every failure axis using synthesized `Summary.xml` fixtures; no intentional smoke-commit test.
- [ ] `azure-pipelines.yml` invokes the gate after ReportGenerator; build fails on gate failure.
- [ ] `CLAUDE.md`, `AGENTS.md`, `docs/adding-a-locale.md` cite the canonical threshold list by referencing the gate-script header comment (no duplicated numbers elsewhere).
- [ ] Final merged CI run: `Humanizer` ≥95 line / ≥88 branch / ≥85 method; `Humanizer.Analyzers` ≥95 line / ≥85 branch.
- [ ] No `[ExcludeFromCodeCoverage]` added beyond .1's FluentDate T4 changes.
- [ ] No gate-level line-range subtraction, no per-class carve-out list; the gate is exactly the numbers `Summary.xml` publishes.

## Done summary
_To be filled on completion._

## Evidence
- Commits:
- Tests:
- PRs:
