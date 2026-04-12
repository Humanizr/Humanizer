# fn-8-add-urdu-ur-locale-with-full-language.1 Preflight: parity map, scaffolding, generator acceptance proof

## Description

Establish the branch, produce the preflight parity map, scaffold a minimal `src/Humanizer/Locales/ur.yml`, AND lock three up-front contract decisions so downstream tasks are implementation-ready. This task is the epic's single cheapest exit if any of the foundations proves unstable.

**Size:** M
**Files:**
- `src/Humanizer/Locales/ur.yml` (new — `surfaces: {}` only)
- `artifacts/2026-04-12-ur-parity-map.md` (new — committed)
- `.agents/skills/add-locale/SKILL.md` or its `references/` (edit — update parity-map source-control convention)

## Up-front contract decisions (all three must land in this task)

### Decision 1 — Ordinal API path matrix for Urdu

Document which public API path produces word ordinals for Urdu. Fill the table in the parity map with expected Urdu strings by reading the current runtime code.

| Public API | Registry | Expected Urdu (Masc) | Expected Urdu (Fem) |
|---|---|---|---|
| `5.ToOrdinalWords(gender, "ur")` | `NumberToWordsConverters` | `پانچواں` | `پانچویں` |
| `5.Ordinalize(gender, "ur")` | `Ordinalizers` | `پانچواں` | `پانچویں` |
| `"5".Ordinalize(gender, "ur")` | `Ordinalizers` | `پانچواں` | `پانچویں` |

Current state (per review):
- `OrdinalizeExtensions` passes numeric strings (`"5"`) into ordinalizers.
- `suffix`/`template`/`word-form-template` ordinalizer engines already expose `masculine`/`feminine`/`neuter` fields via `OrdinalizerProfileCatalogInput`.
- `ToOrdinalWords` routes through `INumberToWordsConverter`, selected by `number.words`, which is a separate catalog.

The plan must own BOTH paths:
- Decision .1a — pick a strategy for `IOrdinalizer` word-ordinal generation (implemented in .9): (a) extend `word-form-template`, (b) new `number-word-suffix` engine, (c) exact replacement table.
- Decision .1b — pick a strategy for `INumberToWordsConverter` gendered ordinal output (implemented in .3 or extended in .9): the chosen number engine must either already emit per-gender word ordinals, or be extended to do so. Record which task owns the extension.

Record both in the parity map with rationale.

### Decision 2 — Number-to-words engine for Urdu

Inspect `ConjunctionalScaleNumberToWordsConverter` and `IndianGroupingNumberToWordsConverter` (if present). Prove or disprove dense irregular 0–99 lookup with lakh/crore scales and per-gender ordinal emission. Required runtime tests must eventually pass:

- `21 → "اکیس"`, `99 → "ننانوے"`, `100 → "ایک سو"`, `100000 → "ایک لاکھ"`, `1234567 → <reviewer-approved>`
- Gendered ordinal output coherent with Decision 1 via `ToOrdinalWords(gender)`.

Record engine choice + citations in the parity map.

### Decision 3 — Hijri calendar contract (feasibility-gated)

`DateTime` / `DateOnly` do not retain the `Calendar` used to construct them. `OrdinalDatePattern` derives calendar behavior from `CultureInfo.CurrentCulture.DateTimeFormat.Calendar`, not from a supplied `Calendar`. Pick ONE, with feasibility proof:

- **(A) Culture-calendar contract**: Hijri selected when `CurrentCulture.DateTimeFormat.Calendar` is `HijriCalendar` / `UmAlQuraCalendar` AND YAML `ordinal.date.calendarMode: Native` / `ordinal.dateOnly.calendarMode: Native`. (`calendarMode` is an ordinal-date-profile field per `OrdinalDateProfileCatalogInput`, NOT a `calendar`-surface field.) **Feasibility gate**: record `OptionalCalendars` for `ur`, `ur-PK`, `ur-IN` across net10/net8/net48. If none of the Urdu cultures accepts assigning `HijriCalendar` / `UmAlQuraCalendar` to `DateTimeFormat.Calendar`, contract A is **rejected** (callers would need to clone a different culture, which is a worse UX than B or C).

- **(B) Explicit `calendarMode` contract**: YAML `ordinal.date.calendarMode: Islamic` (and `ordinal.dateOnly.calendarMode: Islamic`) triggers Hijri tables. **Feasibility gate**: specify the public caller mechanism — a static YAML mode alone cannot let callers pick Gregorian vs Hijri per call. Require a public API surface (e.g. `ToOrdinalWords(calendar, culture)` or a thread-local mode). Adding a fourth Urdu sibling locale (`ur-IS`, etc.) is **out of scope** under this epic — the shipped-locale set is fixed at `ur` + `ur-PK` + `ur-IN` (65 shipped YAML files). If Decision 3 ends up requiring a new sibling, post a re-plan note; do NOT silently add a fourth file.

- **(C) New public overload**: add `ToOrdinalWords(Calendar calendar, CultureInfo culture)` plumbed through `IDateToOrdinalWordConverter`. **Feasibility gate**: the full call chain must stop relying on `CurrentCulture` inside `OrdinalDatePattern`; document every site that today reads `culture.DateTimeFormat.Calendar` and describe its replacement.

Record the chosen option + feasibility evidence + schema extension shape (`calendar.hijriMonths` new key, or `calendar.months: { gregorian, hijri }` calendar-keyed map).

## Approach

1. Create and check out branch `feat/urdu-locale` from `main`.
2. Run the cross-platform probes and save output; `tools/` contains TWO probes — the net10+ file-based app `tools/locale-probe.cs` and the net48 project under `tools/locale-probe-net48/`:
   ```bash
   dotnet run tools/locale-probe.cs -- ur ur-PK ur-IN                     # net10 via file-based app
   dotnet run --project tools/locale-probe-net48 -- ur ur-PK ur-IN        # net48 (Windows host)
   ```
3. Read `.agents/skills/add-locale/SKILL.md` + `references/parity-checklist.md`, `OrdinalizerProfileCatalogInput.cs`, `OrdinalizeExtensions.cs`, `ConjunctionalScaleNumberToWordsConverter.cs`, `NumberToWordsProfileCatalogInput.cs`, `OrdinalDatePattern.cs`, and `CanonicalLocaleAuthoring.AddCalendarFeatures` before writing anything.
4. Produce the preflight gap report in the parity map (committed). Include: all 8 surfaces as unresolved; filled Decisions 1a/1b, 2, 3 with rationale + citations + feasibility proofs; probe output inline for both net10 and net48 runs (or a pointer to CI if net48 is unavailable locally).
5. Author `src/Humanizer/Locales/ur.yml` with EXACTLY:
   ```yaml
   locale: 'ur'
   surfaces: {}
   ```
   No per-surface stubs.
6. Build. Proof point: generator accepts the canonical `ur.yml`. `LocaleTheoryMatrixCompletenessTests` failure output is captured as the `.6` drive-list. Do NOT claim full registry coverage from this task.
7. **Update `.agents/skills/add-locale/SKILL.md`** (or a new `references/parity-artifacts.md`) to reflect this epic's convention: parity maps and audit artifacts under `artifacts/` are committed. This aligns the skill with `.1`'s committed artifact and unblocks `.8` from re-writing the same note.
8. Commit referencing this task and the three contract decisions.

## Investigation targets

**Required**:
- `/Users/claire/dev/Humanizer/.agents/skills/add-locale/SKILL.md`
- `/Users/claire/dev/Humanizer/.agents/skills/add-locale/references/parity-checklist.md`
- `/Users/claire/dev/Humanizer/docs/adding-a-locale.md:62-144,188-270,333-346,400-424`
- `/Users/claire/dev/Humanizer/src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs`
- `/Users/claire/dev/Humanizer/src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalizerProfileCatalogInput.cs`
- `/Users/claire/dev/Humanizer/src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/NumberToWordsProfileCatalogInput.cs`
- `/Users/claire/dev/Humanizer/src/Humanizer/OrdinalizeExtensions.cs`
- `/Users/claire/dev/Humanizer/src/Humanizer/Localisation/NumberToWords/ConjunctionalScaleNumberToWordsConverter.cs`
- `/Users/claire/dev/Humanizer/src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs`
- `/Users/claire/dev/Humanizer/src/Humanizer/Humanizer.csproj:18`
- `/Users/claire/dev/Humanizer/tools/locale-probe.cs`
- `/Users/claire/dev/Humanizer/tools/locale-probe-net48/`

## Key context

- Scaffold is deliberately minimal (`surfaces: {}`). Empty per-surface stubs may fail profile generation and mislead registry coverage claims.
- Re-plan trigger: any contract decision exposing a >~5-file blast radius STOPS the epic and posts a re-plan note before `.2`.
- This epic commits parity artifacts under `artifacts/`; the add-locale skill previously said they should NOT be source-controlled. This task updates the skill to reflect the new convention; `.8` verifies the skill update landed.
- This task is the epic's cheapest exit.

## Acceptance

- [ ] Branch `feat/urdu-locale` created from `main` and checked out.
- [ ] `artifacts/2026-04-12-ur-parity-map.md` committed. Contains:
  - Preflight gap report (all 8 surfaces unresolved).
  - Probe output for `ur`, `ur-PK`, `ur-IN` on net10 + net48 (or CI URL if net48 local host unavailable).
  - **Decision 1a** filled: `IOrdinalizer` engine strategy + rationale.
  - **Decision 1b** filled: `INumberToWordsConverter` gendered ordinal strategy + the task that owns the extension (.3 or .9).
  - **Decision 2** filled: number-to-words engine choice + inspection notes for dense 0–99 + gendered-ordinal emission.
  - **Decision 3** filled: Hijri contract A/B/C + feasibility proof (`OptionalCalendars` survey for A; public caller mechanism for B; call-chain removal plan for C) + schema-extension shape.
- [ ] `src/Humanizer/Locales/ur.yml` contains exactly `locale: 'ur'` + `surfaces: {}` — no per-surface stubs.
- [ ] `dotnet build src/Humanizer/Humanizer.csproj -c Release` succeeds with no diagnostic attributable to `ur.yml`.
- [ ] `dotnet test tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj --framework net10.0` passes.
- [ ] `LocaleTheoryMatrixCompletenessTests` output captured in the parity map as the `.6` driver list.
- [ ] No claim of full registry coverage in this task.
- [ ] `.agents/skills/add-locale/SKILL.md` updated to reflect parity artifacts being committed under `artifacts/`.
- [ ] Re-plan trigger: any contract decision with >~5-file blast radius posts a re-plan note on the epic BEFORE `.2` begins.

## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
