# fn-13-prepare-for-net-11-add-net110-tfm-audit.4 Resolve net11 analyzer/warnings-as-errors regressions

## Description
Resolve any new analyzer warnings or default-enabled analyzer rules that net11 introduces, so `TreatWarningsAsErrors=true` + `EnforceCodeStyleInBuild=true` (`Directory.Build.props:8,17`) don't block the build.

**Size:** S/M (depends on how noisy the net11 analyzer defaults turn out to be)
**Files:**
- `Directory.Build.props` (analyzer / NoWarn settings)
- `src/Humanizer/**/*.cs` (minimal per-site suppressions or fixes)
- potentially `.editorconfig`

## Approach

- Run `dotnet build -c Release -f net11.0` and let warnings surface.
- Prefer code fixes over `NoWarn` suppressions.
- When suppressing, suppress at the narrowest scope (per-file or per-line) with a `// justification` comment.

## Investigation targets

**Required:**
- `Directory.Build.props:8,17,36-50` — warnings-as-errors + enforce-code-style + TFM gates
## Acceptance
- [ ] `dotnet build -c Release -f net11.0` and `dotnet build Humanizer.slnx -c Release` complete with zero warnings
- [ ] No blanket `NoWarn` additions; any suppression is narrowly scoped with justification
- [ ] net8/net10 builds continue to pass without regression
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
