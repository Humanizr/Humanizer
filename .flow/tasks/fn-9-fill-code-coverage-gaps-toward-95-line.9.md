## Description
Cover the actual uncovered branch surfaces of the three engine-contract factories: `WordsToNumberEngineContractFactory` (77.9% line / ~45.4% branch), `TimeOnlyToClockNotationEngineContractFactory` (82.0% / ~71.4%), `NumberToWordsEngineContractFactory` (88.1%). Branch surfaces include `EngineContractMember.Kind` switches, builder arms (`harmony-ordinal-scale-array`, `triad-scale-array`, `hyphenated-scale`, etc.), `MissingValue == "empty"` branches, optional/default/fallback arms, and unsupported-member exception paths. No task-level escape hatch: every reachable uncovered branch from `.1/uncovered.json` is exercised. If a branch is believed unreachable, the task escalates to the epic for scope amendment — it does NOT satisfy acceptance via Done-summary justification.

Depends on .8 for shared `FixtureLoader` + `Fixtures/` conventions.

**Size:** M
**Files:**
- `tests/Humanizer.SourceGenerators.Tests/Fixtures/EngineContracts/` (new subdirectory)
- `tests/Humanizer.SourceGenerators.Tests/SourceGenerators/EngineContractFactoryTests.cs` (new)

## Approach
- Reuse `FixtureLoader` from .8.
- Consume `artifacts/fn-9-local-coverage/uncovered.json` as the authoritative list. For each uncovered branch in the three factories and their `EngineContractCatalog` + `EngineContractMember` neighbours, author a synthetic YAML fixture that selects that branch.
- Target surfaces:
  - Factory `Kind` dispatch arms (e.g. `WordsToNumberEngineContractFactory.Create` `else if (kind == "X")`)
  - Builder arms in `NumberToWordsEngineContractFactory` (`harmony-ordinal-scale-array`, `triad-scale-array`, `hyphenated-scale`, etc.)
  - `EngineContractMember.Kind` switch arms in catalog helpers
  - `MissingValue == "empty"` / default / fallback branches
  - Unsupported-member exception paths (reachable via crafted fixture)
  - Unknown-kind conventional fallthrough — assert on GENERATED SOURCE CONTENT (reference to conventional expression class name); factories do NOT emit diagnostics, they fall through to `CreateConventional<X>Expression`
- Fixture size: only fields required by the targeted branch.

## Investigation targets
**Required:**
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/WordsToNumberEngineContractFactory.cs` (full, ~complexity 135)
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/TimeOnlyToClockNotationEngineContractFactory.cs` (full)
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/NumberToWordsEngineContractFactory.cs` (full)
- `src/Humanizer.SourceGenerators/Common/EngineContractModels.cs` — `EngineContractMember.Kind`
- `tests/Humanizer.SourceGenerators.Tests/SourceGenerators/FixtureLoader.cs` (from .8)
- `artifacts/fn-9-local-coverage/uncovered.json`
- `src/Humanizer/Locales/*.yml` — reference shapes for each engine kind

## Acceptance
- [ ] `WordsToNumberEngineContractFactory` reaches ≥90% line / ≥80% branch.
- [ ] `TimeOnlyToClockNotationEngineContractFactory` reaches ≥90% line / ≥80% branch.
- [ ] `NumberToWordsEngineContractFactory` reaches ≥95% line / ≥85% branch.
- [ ] EVERY reachable uncovered branch in `uncovered.json` attributed to these three factories + `EngineContractMember.Kind` helpers is exercised by at least one fixture. A branch believed unreachable triggers an epic-scope-amendment escalation, NOT a Done-summary justification.
- [ ] Unknown-kind fallthrough asserts on generated source content (NOT on diagnostics).
- [ ] Fixtures live under `Fixtures/EngineContracts/`; `FixtureLoader` from .8 consumed unmodified.
- [ ] SG thresholds are task-level; NOT gated by CI.

## Done summary
Added 6 YAML engine-contract fixtures and 35 test methods (171 -> 206) covering uncovered branches in WordsToNumberEngineContractFactory, TimeOnlyToClockNotationEngineContractFactory, NumberToWordsEngineContractFactory, and EngineContractUtilities. Covers nullable-member present/absent paths, multi-member constructor dispatch, null-RuntimeType fallback, CreateObjectValue with/without TypeName, value-resolution fallback/default branches, and all defensive exception paths.
## Evidence
- Commits: fc82ea591d27e45b2e7abe36bac3f8b14f24c303, a9bc447b8a4394be4597410c347edbc1015075e8
- Tests: dotnet test --project tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj --framework net10.0 (206 passed)
- PRs: