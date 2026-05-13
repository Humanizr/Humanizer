# fn-13-prepare-for-net-11-add-net110-tfm-audit.1 Add net11.0 TFM + preview SDK pin + CI preview slot

## Description
Add `net11.0` to Humanizer's TFM matrix and pin a .NET 11 preview SDK locally so preview builds don't leak to unrelated contributors. Wire a CI preview slot (Linux/macOS/Windows) that exercises `dotnet build` + `dotnet test --framework net11.0` against the pinned SDK.

**Size:** M
**Files:**
- `src/Humanizer/Humanizer.csproj` (TFM list)
- any other `.csproj` with an explicit `<TargetFrameworks>` list (grep first)
- `global.json` (add `sdk.paths`, bump `version` to preview or rely on `allowPrerelease`)
- `.github/workflows/benchmarks-baseline-vs-current.yml` (explicit SDK list at `:39-45`, `:125`, `:165`)
- potentially a new `.github/workflows/net11-preview.yml` (separate workflow so net11 failures don't block the main matrix)

## Approach

- TFM list source of truth is `src/Humanizer/Humanizer.csproj:3` (`net10.0;net8.0;net48;netstandard2.0`).
- `copilot-setup-steps.yml:34-36` and `codeql.yml:48-50` use `global-json-file` → auto-track `global.json`. `benchmarks-baseline-vs-current.yml` uses an explicit SDK list and must be updated.
- Pin the preview SDK via `global.json` `paths` per the prerelease guide so the repo doesn't require every contributor to install the preview globally.
- Start the net11 preview workflow as `continue-on-error: true` or a separate optional workflow — we don't want preview churn blocking main PRs.

## Investigation targets

**Required:**
- `src/Humanizer/Humanizer.csproj:1-10` — TFM list
- `global.json` — current pin
- `.github/workflows/benchmarks-baseline-vs-current.yml:39-45,125,165` — explicit SDK list that needs the net11 entry
- `Directory.Build.props:36-50` — Polyfill / AOT / trim gates keyed off TFM

**Optional:**
- `.github/workflows/copilot-setup-steps.yml:34-36`
- `.github/workflows/codeql.yml:48-50`

## Key context

The preview SDK strategy is "install locally via global.json paths" — do **not** set `allowPrerelease: true` without a `paths` pin, since that makes every contributor build with whichever preview happens to be on their machine.
## Acceptance
- [ ] net11.0 added to Humanizer.csproj and any other project that enumerates TFMs
- [ ] global.json pins a .NET 11 preview SDK via sdk.paths (or equivalent) without disrupting net10/net8
- [ ] CI preview workflow runs net11.0 build + tests on Linux, macOS, Windows
- [ ] `dotnet build -f net11.0` produces a green binary for Humanizer.csproj
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
