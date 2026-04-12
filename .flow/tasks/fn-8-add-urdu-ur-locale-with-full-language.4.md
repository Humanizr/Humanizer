# fn-8-add-urdu-ur-locale-with-full-language.4 Author ordinal.date / ordinal.dateOnly + clock surfaces

## Description

Author the **date-side ordinal patterns** (`ordinal.date`, `ordinal.dateOnly`) and the full `clock` surface in `src/Humanizer/Locales/ur.yml`. **All `ordinal.numeric` authoring is owned by `.9`** — if `.1` Decision 1a selects an engine that doesn't yet exist (e.g. a new `number-word-suffix`), authoring `ordinal.numeric` here would reference missing runtime types. Keep `.4` strictly to date patterns + clock to avoid that coupling.

**Size:** M
**Files:**
- `src/Humanizer/Locales/ur.yml` (edit — add `ordinal.date`, `ordinal.dateOnly`, `clock` blocks)
- `artifacts/2026-04-12-ur-parity-map.md` (update)

## Approach

1. **`ordinal.date`** and **`ordinal.dateOnly`**:
   - Pattern chosen via reviewer, recorded in parity map (e.g. `'{day} MMMM، y'` using U+060C OR `'{day} MMMM yyyy'` ASCII-space).
   - `dayMode: 'Numeric'`.
   - `calendarMode` sits under `ordinal.date` / `ordinal.dateOnly` (per current schema; see `OrdinalDateProfileCatalogInput.cs` reading `GetOptionalString(profile.Root, "calendarMode")`). Authoring for the Hijri contract chosen in `.1` Decision 3 happens here when the contract uses `calendarMode: 'Islamic'` (contract B), or `calendarMode: 'Native'` (contract A). Contract C touches the runtime API; YAML stays Gregorian by default.
2. **`clock`** (unified `phrase-clock` engine):
   - `engine: 'phrase-clock'`, `hourMode: 'h12'`, `hourGender: 'masculine'`, `minuteGender: 'masculine'`.
   - `hourWordsMap` 1–12 using `.3`'s cardinal strings (no drift).
   - `min0`: `'{hour} بجے'`; `defaultTemplate`: `'{hour} بجے {minute} منٹ'`.
   - Day-period fields use the **supported** `phrase-clock` shape per `docs/locale-yaml-reference.md` and `ar.yml`/`fa.yml` precedent: `earlyMorning`, `morning`, `afternoon`, `night`. **Do NOT author an undocumented `evening` field.**
   - Day-period labels linguistically distinct (project-memory pitfall: do not reuse a word across ranges). Suggested Urdu labels:
     - `earlyMorning: 'صبح سویرے'` (pre-dawn / early morning)
     - `morning: 'صبح'`
     - `afternoon: 'دوپہر'`
     - `night: 'رات'`
     Exact reviewer-approved strings live in the parity map.
   - Bucket templates MUST NOT embed day-period words (project-memory pitfall).
   - AM/PM: reviewer-decided; recorded in parity map.
3. Run:
   ```bash
   dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 \
     --filter "FullyQualifiedName~ExactLocaleDateAndTimeRegistryTests|GeneratedFormatterRuntimeTests"
   ```

## Investigation targets

**Required**:
- Parity map `.1` Decision 3 (Hijri contract — determines `calendarMode` value here)
- `/Users/claire/dev/Humanizer/docs/locale-yaml-reference.md` ordinal + clock sections
- `/Users/claire/dev/Humanizer/src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalDateProfileCatalogInput.cs`
- `/Users/claire/dev/Humanizer/src/Humanizer/Locales/ar.yml` clock block (RTL + `phrase-clock` reference)
- `/Users/claire/dev/Humanizer/src/Humanizer/Locales/fa.yml` clock block (Perso-Arabic reference)
- `.flow/memory/pitfalls.md`

## Key context

- `ordinal.numeric` is NOT authored in this task. `.9` owns the full masculine + feminine `ordinal.numeric` payload AFTER the engine lands.
- `calendarMode` is an **`ordinal.date` / `ordinal.dateOnly` field**, not a `calendar`-surface field. Do not author `calendar.calendarMode`.
- Supported clock day-period fields: `earlyMorning`, `morning`, `afternoon`, `night`. No `evening`.
- `minuteGender: 'masculine'` explicit.
- `hourWordsMap` reuses `.3`'s cardinal strings verbatim.
- Content directionality sweep: `rg -P '\x{200E}|\x{200F}|\x{061C}' src/Humanizer/Locales/ur.yml` returns no matches.
- All exact strings match parity-map-frozen reviewer-approved values.

## Acceptance

- [ ] `surfaces.ordinal.date` and `surfaces.ordinal.dateOnly` authored with pattern + `dayMode: 'Numeric'` + `calendarMode` per `.1` Decision 3 (where applicable).
- [ ] `surfaces.clock.engine: 'phrase-clock'` with `hourMode: 'h12'`, `hourGender: 'masculine'`, `minuteGender: 'masculine'` explicit.
- [ ] `hourWordsMap` strings identical to `.3`'s cardinal strings.
- [ ] Day-period fields use supported shape (`earlyMorning`, `morning`, `afternoon`, `night`). No `evening` field authored.
- [ ] Day-period labels distinct; bucket templates contain no day-period words.
- [ ] Clock renders correctly for parity-map cases: 1:00 PM, 7:05 AM, 12:00, 12:30, one bucket edge.
- [ ] `ExactLocaleDateAndTimeRegistryTests` pass on net10.0 for clock and date-ordinal cases (gendered numeric ordinals arrive in `.9`).
- [ ] `rg -P '\x{200E}|\x{200F}|\x{061C}' src/Humanizer/Locales/ur.yml` returns no matches.
- [ ] Every authored term matches the parity-map-frozen reviewer-approved string.
- [ ] `ordinal.numeric` is NOT authored here (`.9` owns it).

## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
