# Add Windows NLS backend (UseNls) CI slot + restore NLS-only test coverage

## Overview

Add a Windows CI matrix slot that runs net8.0/net10.0 with `<RuntimeHostConfigurationOption Include="System.Globalization.UseNls" Value="true"/>` (or `DOTNET_SYSTEM_GLOBALIZATION_USENLS=1`) and restore tests that are currently gated behind `#if NET48` because their scenarios only exist under the classic Windows NLS backend — starting with `HijriCalendar_UrIn_InheritsHijriMonths` (`tests/Humanizer.Tests/Localisation/ur/UrduHijriDateTests.cs:96`).

## Background

Since .NET 5 (Windows 10) and .NET 7 (Server 2019), .NET defaults to ICU on Windows. `ur-IN.OptionalCalendars` on ICU does **not** list `HijriCalendar`, so `DateTimeFormat.Calendar = new HijriCalendar()` throws for cultures whose ICU data lacks the entry. Under NLS, the list differs and the scenario is reachable. Today this reachability matches exactly one TFM (`net48`, Windows-only, always NLS), so we guard with `#if NET48`. An NLS slot generalizes that coverage to net8/net10 and ensures we don't regress the dispatch path when ICU data shifts again.

## Scope

In: a Windows-only CI job that builds the test project with `UseNls=true` and runs the suite on net8.0 + net10.0; restoration of NLS-only test paths currently ifdef'd to `NET48`; documentation of the matrix in CLAUDE.md.

Out: non-Windows NLS (not a supported configuration); changes to the ICU default on any runner; production library code changes (the library is already NLS-compatible by virtue of reading `DateTimeFormat.Calendar` at runtime).

## Quick commands

```bash
# Local one-off: NLS via environment
DOTNET_SYSTEM_GLOBALIZATION_USENLS=1 dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj -f net10.0

# Or via runtimeconfig.template.json in the test project:
# { "configProperties": { "System.Globalization.UseNls": true } }
```

## Acceptance

- [ ] Windows CI job runs `tests/Humanizer.Tests` with `UseNls=true` on net8.0 and net10.0
- [ ] The NLS slot is green (or failing tests are triaged with filed follow-ups)
- [ ] `HijriCalendar_UrIn_InheritsHijriMonths` runs on the NLS slot without `#if NET48` restriction (or a broader guard covers NLS on net8/net10)
- [ ] Any other ur/fa/he/ar test paths that differ under NLS are identified and covered
- [ ] CLAUDE.md documents the NLS vs ICU matrix and how to reproduce locally

## Early proof point

Task fn-14.1 wires the NLS CI slot and runs the existing suite. If that surfaces a flood of unexpected NLS-vs-ICU divergences beyond the ur-IN case, the scope of fn-14.3 (extending NLS coverage) will need to grow before fn-14.2 can relax the `#if NET48` guard cleanly.

## Requirement coverage

| Req | Description | Task(s) | Gap justification |
|-----|-------------|---------|-------------------|
| R1  | CI slot runs net8/net10 with UseNls=true on Windows | fn-14.1 | — |
| R2  | Restore ur-IN + HijriCalendar coverage beyond net48 | fn-14.2 | — |
| R3  | Identify + cover other NLS-only divergences | fn-14.3 | — |
| R4  | Document matrix + local repro | fn-14.4 | — |

## Dependencies

- **fn-8** (Urdu locale) — ur-IN + HijriCalendar test is the primary re-gain target; fn-8 must be closed.
- **fn-10** (net48 Urdu byte parity) — generalize that work into a proper NLS slot, rather than duplicating.

## References

- [Globalization runtime config settings (UseNls)](https://learn.microsoft.com/en-us/dotnet/core/runtime-config/globalization)
- [Breaking change: Globalization APIs use ICU on Windows 10 (.NET 5)](https://learn.microsoft.com/en-us/dotnet/core/compatibility/globalization/5.0/icu-globalization-api)
- [Breaking change: ICU on Windows Server 2019 (.NET 7)](https://learn.microsoft.com/en-us/dotnet/core/compatibility/globalization/7.0/icu-globalization-api)
