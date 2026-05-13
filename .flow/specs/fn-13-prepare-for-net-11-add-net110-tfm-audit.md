# Prepare for .NET 11 (add net11.0 TFM, audit ICU 78 breakage)

## Overview

Add `net11.0` to Humanizer's TFM matrix and surface any .NET 11 / ICU 78 regressions before the SDK flips. We currently ship `net10.0;net8.0;net48;netstandard2.0` from `src/Humanizer/Humanizer.csproj:3`, pinned via `global.json:3` (`version 10.0.100`, `rollForward: latestFeature`).

## Scope

In: net11.0 TFM, CI preview slot, byte-parity re-verification on .NET 11 preview, targeted audit of known ICU 78 / .NET 11 breaking changes, docs (CLAUDE.md test commands).

Out: Windows NLS backend (tracked separately as a sibling epic), net48 changes, unrelated test-suite work.

## Known breaking changes to audit

1. **ICU 78 Japanese Meiji era** start date shift (ICU CLDR refresh). No `ja-*` fixtures exist in this repo and no code reads `JapaneseCalendar` era tables directly, so risk is low — confirm by running ja suites on the preview.
2. **ur-IN `NativeDigits`** converges to extended Arabic-Indic (U+06F0–U+06F9) on Apple platforms. `grep NativeDigits src/Humanizer` returns **zero hits** — Humanizer never reads the property. Risk is confined to any test that asserts platform `ToString()` output.
3. **ISO 8601 24:00 end-of-day parsing** ([runtime #124142](https://github.com/dotnet/runtime/pull/124142)). No `"24:00"` fixtures exist today. Spot-check date-parse adjacent tests.
4. **`decimal` NumberStyles validation tightening**. `decimal.Parse`/`TryParse` returns zero hits in src/tests. `ByteSize` uses NumberStyles with `double`, not `decimal` (`src/Humanizer/Bytes/ByteSize.cs:26,478`). Low risk.
5. **fn-3 byte-parity assertions** (bn, fa, he, ku, zu-ZA, ta, ar, fr-CH) ran only against current ICU. Re-run on .NET 11 preview and refresh `calendar:` / `number.formatting:` YAML overrides if any drift appears.

## Risks

- `TreatWarningsAsErrors=true` + `EnforceCodeStyleInBuild=true` (`Directory.Build.props:8,17`) mean new net11 analyzer defaults will break the build. Allocate a warning-cleanup task.
- Preview SDKs rev mid-epic; pin via `global.json` `paths` + `version` per the [test-prerelease-sdk-locally guide](https://learn.microsoft.com/en-us/dotnet/core/tools/test-prerelease-sdk-locally).

## Quick commands

```bash
# Build net11 (once TFM added)
dotnet build src/Humanizer/Humanizer.csproj -c Release -f net11.0

# Run test suite on net11
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj -f net11.0

# Compare suite health across TFMs
for tfm in net8.0 net10.0 net11.0; do dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj -f $tfm --no-build; done
```

## Acceptance

- [ ] `net11.0` added to `src/Humanizer/Humanizer.csproj` TFM list (and other projects that enumerate TFMs)
- [ ] `global.json` updated to a .NET 11 preview (pinned via `paths`) without blocking net10/net8 builds
- [ ] CI preview slot runs net11.0 on Linux, macOS, Windows; failures are triaged (not silently accepted)
- [ ] Full `tests/Humanizer.Tests` suite passes on net11.0 across all three OS runners
- [ ] fn-3 byte-parity locales (bn, fa, he, ku, zu-ZA, ta, ar, fr-CH) verified byte-identical on .NET 11; any drift resolved via YAML overrides
- [ ] No new warnings-as-errors regressions from net11 analyzer defaults
- [ ] `CLAUDE.md` "Quick Commands" section lists net11.0 alongside existing TFMs

## Early proof point

Task fn-13.1 adds the net11.0 TFM + preview SDK pin + CI slot. If that task's smoke build doesn't produce a green net11.0 binary, the ICU/analyzer audit tasks can't begin — re-evaluate whether to wait for a later preview or use a nightly SDK channel.

## Requirement coverage

| Req | Description | Task(s) | Gap justification |
|-----|-------------|---------|-------------------|
| R1  | net11.0 TFM + preview SDK + CI slot | fn-13.1 | — |
| R2  | Full suite passes on net11 (all 3 OS) | fn-13.2 | — |
| R3  | fn-3 byte-parity re-verification + YAML refresh | fn-13.3 | — |
| R4  | net11 analyzer/warning cleanup | fn-13.4 | — |
| R5  | Targeted breaking-change sweep (Meiji, 24:00, decimal, NativeDigits) | fn-13.5 | — |
| R6  | Docs + CLAUDE.md updated | fn-13.6 | — |

## Dependencies

- **fn-8** (Urdu locale) — Epic A re-verifies ur/ur-PK/ur-IN under ICU 78. fn-8 must be closed.
- **fn-12** (calendar schema refactor) — Japanese calendar dispatch slot lives downstream of fn-12; ICU 78 Meiji audit is cleaner after fn-12 lands.

## References

- [Breaking changes in .NET 11](https://learn.microsoft.com/en-us/dotnet/core/compatibility/11)
- [test-prerelease-sdk-locally](https://learn.microsoft.com/en-us/dotnet/core/tools/test-prerelease-sdk-locally)
- [global.json rollForward + allowPrerelease](https://learn.microsoft.com/en-us/dotnet/core/tools/global-json)
- [runtime #124142 — ISO 8601 24:00 parsing](https://github.com/dotnet/runtime/pull/124142)
- [ICU release notes](https://icu.unicode.org/download)
