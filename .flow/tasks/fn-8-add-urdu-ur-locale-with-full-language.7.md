# fn-8-add-urdu-ur-locale-with-full-language.7 Cross-platform verification: net10/net8/net48 byte parity + clear parity map

## Description

Run the full Humanizer test suite on every target framework and verify byte-identical Urdu output across net10.0, net8.0, and net48. Apply explicit YAML overrides for any drift uncovered. Clear the parity map and audit artifact unresolved sets. Runs AFTER `.12`/`.13`/`.14` so all content is stable before final verification.

**Size:** S-M
**Files:**
- `src/Humanizer/Locales/ur.yml` (edit only if drift requires additional overrides)
- `artifacts/2026-04-12-ur-parity-map.md` (final reconciliation — unresolved set empty)
- `artifacts/2026-04-12-grammatical-gender-audit.md` (mark gaps resolved or follow-ups tracked)

## Approach

1. Run the full matrix:
   ```bash
   dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
   dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0
   # net48 runtime requires Windows host:
   dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net48
   ```
2. If on macOS / Linux, flag that net48 runtime verification must happen on Windows CI or dev environment. Do NOT mark net48 verified based on compile success alone.
3. Compare outputs across frameworks for every test in `tests/Humanizer.Tests/Localisation/ur/`. Any drift → identify the responsible `CultureInfo` field and add an explicit YAML override. Re-run.
4. Re-run `locale-probe.cs` (and `tools/locale-probe-net48/` on Windows) and confirm Urdu outputs match the baseline captured in `.1`'s parity map (or an explicitly updated baseline).
5. Full suite: zero new failures (not just `ur`-scoped).
6. Run `dotnet pack src/Humanizer/Humanizer.csproj -c Release -o artifacts/urdu-validation` for packaging sanity. **Do NOT run `pwsh tests/verify-packages.ps1`** — the add-locale skill explicitly says not to add the old package verification step.
7. **SKILL.md completion gate** — confirm:
   - Every canonical surface has reviewer-approved terms in parity map.
   - Unresolved-questions section empty.
   - Proposer+reviewer audit trail present.
   - Cross-locale gender audit artifact: every gap either filled in-epic (in `.12`, `.13`, or `.14`) or tracked by an explicit follow-up Flow task ID outside the epic.
8. Commit on branch `feat/urdu-locale`.

## Investigation targets

**Required**:
- `/Users/claire/dev/Humanizer/docs/adding-a-locale.md:400-424`
- `/Users/claire/dev/Humanizer/.agents/skills/add-locale/SKILL.md`
- `/Users/claire/dev/Humanizer/tools/locale-probe.cs`
- `/Users/claire/dev/Humanizer/tools/locale-probe-net48/` (Windows host probe)
- `.flow/memory/pitfalls.md` — "claim strength must match evidence"

## Key context

- User-memory: "No deferrals or spec-loosening." Tests fail → fix YAML; never weaken acceptance.
- net48 runtime needs Windows. If locally unavailable, acceptance stays open until CI runs it; CI build URL recorded in parity map.
- `verify-packages.ps1` NOT run here (per add-locale skill).

## Acceptance

- [ ] Full `Humanizer.Tests` suite passes on net10.0.
- [ ] Full `Humanizer.Tests` suite passes on net8.0.
- [ ] Full `Humanizer.Tests` suite runtime-verified on net48 (Windows host). If unavailable locally, CI build URL recorded in parity map; item stays open until CI passes.
- [ ] Byte-identical Urdu output confirmed across net10 / net8 / net48 for every test in `tests/Humanizer.Tests/Localisation/ur/`.
- [ ] `UrduBidiControlSweep` passes across all frameworks (no U+200F / U+200E / U+061C).
- [ ] Any platform drift produced an explicit YAML override (never a test-side workaround).
- [ ] `SourceGenerators.Tests` passes on net10.0.
- [ ] `dotnet pack` succeeds into `artifacts/urdu-validation`. `pwsh verify-packages.ps1` NOT run.
- [ ] Parity map `artifacts/2026-04-12-ur-parity-map.md` unresolved-questions section empty.
- [ ] Cross-locale audit `artifacts/2026-04-12-grammatical-gender-audit.md`: every gap filled in-epic (`.12` / `.13` / `.14`) OR has an explicit follow-up Flow task ID.
- [ ] All commits on branch `feat/urdu-locale`.

## Done summary
Cross-platform verification: net10.0 and net8.0 fully verified (40,619 tests each, byte-identical output); net48 compile-verified only (runtime requires Windows CI — URL to be recorded in parity map when available). Source generator tests pass (71), dotnet pack succeeds. Fixed pre-existing uz-Latn-UZ apostrophe mismatch (U+2019 to U+2018). Parity map finalized with all canonical surfaces proved on net10/net8, unresolved-questions section empty, gender audit confirmed complete. Updated probe tools to include ur/ur-IN/ur-PK in the 65-locale list.
## Evidence
- Commits: 449554124dc9e97f2bfc48aad79b553950dcc3cd
- Tests: dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 (40619 passed), dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0 (40619 passed), dotnet test --project tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj --framework net10.0 (71 passed), dotnet build tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net48 -c Release (build pass), dotnet pack src/Humanizer/Humanizer.csproj -c Release -o artifacts/urdu-validation (pack pass)
- PRs: