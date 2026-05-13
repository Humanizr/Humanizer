# Fix CI code coverage reporting

## Overview

Azure DevOps CI pipeline (`azure-pipelines.yml`) never produces a code coverage report — not even when all tests pass. The primary root cause is that `--coverage` and `--xunit-info` are MTP (Microsoft Testing Platform) runner arguments that must go after a `--` separator in the `dotnet test` command, but the pipeline passes them without the separator. Without `--`, these flags are never forwarded to the test runner, so coverage is never collected. A secondary defect is that test failures (exit code 2) would also skip all downstream steps including reportgenerator.

Additionally, the DevSkim security scanner (GitHub Actions) produces false positives on `.flow/` task metadata, flagging commit SHAs as hardcoded credentials (rule DS173237).

## Scope

1. **Missing `--` separator in test arguments (primary)** — MTP arguments (`--coverage`, `--coverage-output-format`, `--xunit-info`) are not being forwarded to the test runner because they appear before the `--` separator (which is absent entirely). Adding `--` and also `--results-directory $(Agent.TempDirectory)` (a `dotnet test` arg, before `--`) ensures coverage files land where `reportgenerator@5` expects them.

2. **No `continueOnError` on test steps (secondary)** — Even once coverage is collected, test failures would skip the reportgenerator and publish steps. Adding `continueOnError: true` + `condition: succeededOrFailed()` + a fail-gate step ensures coverage is always reported while preserving correct build failure semantics.

3. **DevSkim DS173237 false positives** — Commit SHAs in `.flow/` task metadata are flagged as hardcoded credentials. The `.flow/` directory should be excluded from DevSkim scanning.

## Quick commands

```bash
# Verify coverage output locally (check for .cobertura.xml files)
dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj -c Release --framework net10.0 -- --coverage --coverage-output-format cobertura
find . -name "*.cobertura.xml" -type f

# Validate pipeline YAML syntax
# (push to a branch and check Azure DevOps for syntax errors)
```

## Acceptance

- [ ] Coverage `.cobertura.xml` files are actually collected (MTP args forwarded via `--` separator)
- [ ] Coverage files land in a path that matches the reportgenerator glob pattern
- [ ] `reportgenerator@5` step generates and publishes a coverage report on a green build (all tests pass)
- [ ] `reportgenerator@5` step also generates and publishes coverage when test steps have failures (exit code 2)
- [ ] Build status is red (Failed) when tests fail — not yellow (SucceededWithIssues)
- [ ] CodeSign stage does NOT execute when tests fail (gated correctly on build failure)
- [ ] Build packages artifact is still published even when tests fail (for diagnostic purposes)
- [ ] DevSkim no longer flags `.flow/` files with DS173237 false positives
- [ ] Pipeline still works correctly on the green path (all tests pass, coverage reported, CodeSign runs)

## Early proof point

Task fn-7-fix-ci-code-coverage-reporting-when.1 is the sole task — push the pipeline fix and verify coverage appears in the Azure DevOps build results page on a green build first. The `--` separator fix is the most critical change; if coverage still doesn't appear after that, re-investigate the exact MTP argument syntax for CodeCoverage 18.5.2.

## Requirement coverage

| Req | Description | Task(s) | Gap justification |
|-----|-------------|---------|-------------------|
| R1  | MTP args forwarded via -- separator | fn-7-fix-ci-code-coverage-reporting-when.1 | — |
| R2  | Coverage files land in correct path | fn-7-fix-ci-code-coverage-reporting-when.1 | — |
| R3  | Coverage report on green build | fn-7-fix-ci-code-coverage-reporting-when.1 | — |
| R4  | Coverage report on red build | fn-7-fix-ci-code-coverage-reporting-when.1 | — |
| R5  | Build stays red on test failure | fn-7-fix-ci-code-coverage-reporting-when.1 | — |
| R6  | CodeSign gated correctly | fn-7-fix-ci-code-coverage-reporting-when.1 | — |
| R7  | Packages still published | fn-7-fix-ci-code-coverage-reporting-when.1 | — |
| R8  | DevSkim DS173237 suppressed for .flow/ | fn-7-fix-ci-code-coverage-reporting-when.1 | — |
| R9  | Green path end-to-end | fn-7-fix-ci-code-coverage-reporting-when.1 | — |
