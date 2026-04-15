# fn-14-add-windows-nls-backend-usenls-ci-slot.1 Add Windows UseNls CI slot for net8.0 + net10.0

## Description
Wire a Windows-only CI slot that builds and runs `tests/Humanizer.Tests` with the NLS globalization backend enabled on net8.0 and net10.0. Pick one of two activation mechanisms:

1. `runtimeconfig.template.json` in the test project with `{"configProperties": {"System.Globalization.UseNls": true}}` — baked into test binaries for that slot.
2. `DOTNET_SYSTEM_GLOBALIZATION_USENLS=1` as a workflow env var — zero repo changes, purely CI.

Option (2) is preferred since it doesn't affect other slots. Option (1) is needed only if the runtime config has to apply to deps outside the env var's reach.

**Size:** M
**Files:**
- `.github/workflows/ci.yml` or equivalent test workflow — add a `windows-latest` matrix entry with the env var set
- optional `tests/Humanizer.Tests/runtimeconfig.template.json` (only if env-var path is insufficient)

## Approach

- Scope to Windows only — NLS is not available on Linux/macOS.
- Start the NLS slot as non-blocking (`continue-on-error: true`) until fn-14.2 and fn-14.3 catch any new divergences.
- Confirm NLS activation at runtime via a probe (e.g., a smoke test asserting `new CultureInfo("ur-IN").OptionalCalendars` contains `HijriCalendar`).

## Investigation targets

**Required:**
- `.github/workflows/*.yml` — identify the main test workflow (this repo's CI entry point)
- `tests/Humanizer.Tests/Humanizer.Tests.csproj`

## Key context

Since .NET 5 (Windows 10) and .NET 7 (Windows Server 2019), ICU is the Windows default. `UseNls=true` restores the classic backend — Windows only.
## Acceptance
- [ ] Windows CI slot runs test suite on net8.0 and net10.0 with NLS backend active
- [ ] NLS activation verified at runtime (e.g., probe on OptionalCalendars or DTFI behavior)
- [ ] Slot runs non-blocking until NLS divergences in fn-14.2/fn-14.3 are resolved
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
