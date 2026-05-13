# Add Urdu (ur) locale with full language support

## Overview

Add Urdu (`ur`) as a new neutral locale in Humanizer plus two regional variants (`ur-PK`, `ur-IN`), with parity coverage across every canonical YAML surface (list, formatter, phrases, number, ordinal, clock, compass, calendar). Cover the Islamic (Hijri) calendar. And — because the epic's first real difficulty (word-ordinal generation with grammatical gender) has ripple effects across the library — audit every existing locale for grammatical-gender gaps and fill them so "all grammatical forms for all locales" is met.

"Full language support" is taken literally: every language-bearing Humanizer feature must produce linguistically correct Urdu output; every grammatical form (masculine / feminine / neuter, where a locale requires it) must be reachable through the public API; source-generator and runtime-engine code changes are in scope wherever existing engines fall short.

All work happens on branch `feat/urdu-locale`.

## Up-front contract decisions (decided in fn-8.1)

Three decisions are made in task `.1` before any Urdu content is authored, so downstream tasks are implementation-ready:

1. **Ordinal API path matrix for Urdu** — specify the expected output for each of:
   - `5.ToOrdinalWords(GrammaticalGender, "ur")` via `INumberToWordsConverter`
   - `5.Ordinalize(GrammaticalGender, "ur")` via `IOrdinalizer`
   - `"5".Ordinalize(GrammaticalGender, "ur")` (string overload)
   All three must produce Urdu word ordinals (`پانچواں` / `پانچویں`) — not digits-plus-suffix like `5واں`. This eliminates the ambiguity between the two runtime paths.
2. **Number-to-words engine for Urdu** — inspect `ConjunctionalScaleNumberToWordsConverter` and `IndianGroupingNumberToWordsConverter` (if present); choose the engine that supports dense irregular 0-99 lookup with lakh/crore scales. If no existing engine works, define the minimal new engine spec.
3. **Hijri calendar contract** — pick one of:
   - **(A) Culture-calendar contract**: Hijri output selected when `CultureInfo.CurrentCulture.DateTimeFormat.Calendar` is a Hijri/UmAlQura calendar AND YAML `ordinal.date.calendarMode: Native` / `ordinal.dateOnly.calendarMode: Native`. (`calendarMode` lives on the ordinal-date profile today — see `OrdinalDateProfileCatalogInput` reading `calendarMode` from `ordinal.date` / `ordinal.dateOnly` — NOT on the `calendar` surface.)
   - **(B) Explicit `calendarMode` contract**: YAML `ordinal.date.calendarMode: Islamic` triggers Hijri tables regardless of `CurrentCulture.Calendar`; callers set the mode via the public mechanism specified in Decision 3.
   - **(C) New public overload**: `ToOrdinalWords(Calendar calendar, CultureInfo culture)` plumbed through `IDateToOrdinalWordConverter`; Calendar carried alongside the DateTime.
   
   `DateTime` / `DateOnly` do **not** retain the `Calendar` used to construct them, so "caller supplies HijriCalendar-derived DateTime" is not observable by the runtime. One of A/B/C must be chosen and documented before .10 implementation.

## Scope

**In scope**
- `src/Humanizer/Locales/ur.yml` covering all eight canonical surfaces.
- `src/Humanizer/Locales/ur-PK.yml` and `src/Humanizer/Locales/ur-IN.yml` with `variantOf: 'ur'` and minimum shape `locale: '…'` + `variantOf: 'ur'` + `surfaces: {}` (canonical parser requires `surfaces`).
- Islamic (Hijri) calendar coverage using the contract chosen in .1 — native month names (محرم، صفر، ربیع الاول، ربیع الثانی، جمادی الاول، جمادی الثانی، رجب، شعبان، رمضان، شوال، ذوالقعدہ، ذوالحجہ) selected by the agreed mechanism. Schema extension for a calendar-keyed months shape (`calendar.hijriMonths` or `calendar.months: { gregorian, hijri }`) is expected.
- Word-ordinal generation for Urdu via the public `Ordinalize` and `ToOrdinalWords` APIs with grammatical gender dispatch. Existing generator already emits `masculine` / `feminine` / `neuter` fields for `suffix` / `template` / `word-form-template` ordinalizer engines; the gap is producing **word** ordinals (`پانچواں`) instead of numeric-suffix ordinals (`5واں`). Define or extend an ordinalizer engine (e.g., a `number-word-suffix` engine that calls `NumberToWords` internally) to close this gap.
- `Neuter → Masculine` fallback for Urdu where the matrix test enumerates all enum genders per locale.
- Regional-variant matrix coverage: `ur-PK` / `ur-IN` appear in **every** dataset enumerated by `LocaleTheoryMatrixCompletenessTests.ShippedLocaleRows` (rows are computed from every shipped YAML file, not "where distinguished").
- Cross-locale grammatical-gender audit across every shipped locale with grammatical gender. Produce `artifacts/2026-04-12-grammatical-gender-audit.md` (committed). Fill work is **pre-split** into `.12` (Romance / Germanic fills + the audit itself), `.13` (Slavic fills), and `.14` (Semitic / Indic / other gender-bearing fills). Fill every gap whose resolution needs no research beyond CLDR Ordinal Rules + one authoritative published grammar. Only gaps requiring research *beyond* that evidence standard escape to follow-up tasks OUTSIDE this epic — their IDs are recorded in the audit.
- **Source-generator and runtime-engine changes** are in scope wherever existing engines fall short. Keep ordinalizer profile data in `LocaleFeature.ProfileRoot` consistent with the existing catalog architecture — do **not** add typed per-gender properties to `ResolvedLocaleDefinition`.
- Follow the repo's own `.agents/skills/add-locale/SKILL.md` workflow: preflight gap report → parity map artifact → proposer+reviewer term derivation → exact-output proofs → empty-unresolved-set completion gate. Every accepted term must be frozen in the parity map with the exact string the test will assert; "native-accurate" is process, not an acceptance criterion.
- Update docs, release notes, `.agents/skills/add-locale/`, `.flow/memory/`, and user memory (`/Users/claire/.claude/projects/-Users-claire-dev-Humanizer/memory/`).
- Credit community PR #1683 author (@iamahsanmehmood) in the PR description.

**Out of scope**
- Reopening, rebasing, or merging community PR #1683.
- Silent deferral of any acceptance item. Items that end up outside the epic must have explicit follow-up Flow task IDs recorded in the audit / parity artifacts.

## Approach

Primary deliverable is driven by the source-generator pipeline. Dropping YAML into `src/Humanizer/Locales/` is sufficient for generator discovery via `src/Humanizer/Humanizer.csproj:18` (`<AdditionalFiles Include="Locales\*.yml" />`). The generator auto-produces all registry registrations (`LocaleRegistryInput.cs:36–46`). Regional variants use `variantOf` for merge-on-top semantics.

Structural template selection:
- **YAML shape**: derive from `src/Humanizer/Locales/fa.yml` (Perso-Arabic script, similar phrase structure).
- **Numbers**: engine choice deferred to .1 discovery.
- **Do NOT derive from `ar.yml`**: different plural shape, different number engine, different script subset.

Plural rule: `formatter.pluralRule: 'singular-plural'` and `formatter.dataUnitPluralRule: 'singular-plural'`. Honor CLDR one/other distinctions (`گھنٹہ`/`گھنٹے`, `دن`/`دنوں`, `مہینہ`/`مہینے`).

Digits / formatting: pin cross-platform via `number.formatting` overrides (`decimalSeparator: '.'`, `groupSeparator: ','`, `negativeSign: '-'`).

Directionality: no tests currently sweep every locale's output for bidi control characters. Urdu tests must include an explicit helper asserting absence of U+200F (RLM), U+200E (LRM), and U+061C (ALM) in any authored output.

Dependency order (implementation-ready):

1. `.1` preflight + scaffold + up-front contract decisions
2. `.2 → .3 → .4 → .5` base Urdu surfaces (sequential because all edit `ur.yml`)
3. `.9` gendered word-ordinal implementation (depends on .4)
4. `.10` Hijri calendar implementation using contract from .1 (depends on .5)
5. `.11` regional variants (depends on .5)
6. `.6` matrix rows + exact-output tests for ur + variants + gender + Hijri (depends on .5, .9, .10, .11)
7. `.12` cross-locale grammatical-gender audit + Romance/Germanic fills (depends on .9 for plumbing, .6 for stable test infra)
8. `.13` Slavic grammatical-gender fills (depends on .12 audit)
9. `.14` Semitic / Indic / other grammatical-gender fills (depends on .12 audit)
10. `.7` cross-platform verification (depends on .12, .13, .14 — all content stable)
11. `.8` docs + release notes + memory (depends on .7)

Quick-command updates: probe has TWO runners — `tools/locale-probe.cs` (file-based app for net10+) and `tools/locale-probe-net48/` (project-based for net48 on Windows).

## Quick commands

```bash
git checkout -b feat/urdu-locale
dotnet build src/Humanizer/Humanizer.csproj -c Release
dotnet test tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj --framework net10.0
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0
dotnet run tools/locale-probe.cs -- ur ur-PK ur-IN                  # net10 file-based app
dotnet run --project tools/locale-probe-net48 -- ur ur-PK ur-IN     # net48, Windows host
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 \
  --filter "FullyQualifiedName~LocaleRegistrySweepTests|LocaleTheoryMatrixCompletenessTests"
dotnet pack src/Humanizer/Humanizer.csproj -c Release -o artifacts/urdu-validation
```

## Acceptance

- [ ] `src/Humanizer/Locales/ur.yml` exists and covers all eight canonical surfaces.
- [ ] `src/Humanizer/Locales/ur-PK.yml` and `src/Humanizer/Locales/ur-IN.yml` exist with minimum `locale: '…'` + `variantOf: 'ur'` + `surfaces: {}`.
- [ ] All three culture identifiers pass `LocaleRegistrySweepTests`.
- [ ] Source-generator build succeeds with no diagnostics.
- [ ] `LocaleRegistrySweepTests` and `LocaleTheoryMatrixCompletenessTests` pass. **Variant rows added to every dataset in `LocaleTheoryMatrixCompletenessTests.ShippedLocaleRows`**, not a subset.
- [ ] `tests/Humanizer.Tests/Localisation/ur/` contains exact-output proofs for every surface Urdu owns — including gendered word-ordinals, Hijri dates, regional-variant resolution — with strings frozen in the parity map.
- [ ] `NumberToWords(100000, "ur")` returns `ایک لاکھ`; South Asian scale names present. Engine choice proven by passing tests for 21 → `اکیس`, 99 → `ننانوے`, 1234567 → reviewer-approved exact string.
- [ ] Cardinal table covers 0–99 as distinct entries.
- [ ] All three ordinal API paths return Urdu word ordinals (not digits-plus-suffix):
  - `5.ToOrdinalWords(GrammaticalGender.Masculine, "ur") == "پانچواں"`
  - `5.ToOrdinalWords(GrammaticalGender.Feminine, "ur") == "پانچویں"`
  - `5.Ordinalize(GrammaticalGender.Masculine, "ur") == "پانچواں"`
  - `5.Ordinalize(GrammaticalGender.Feminine, "ur") == "پانچویں"`
  - `"5".Ordinalize(GrammaticalGender.Masculine, "ur") == "پانچواں"`
- [ ] `Neuter` falls back to `Masculine` for Urdu; documented in the audit artifact.
- [ ] Plural stems distinct between one and other counts (`گھنٹہ`/`گھنٹے`, `دن`/`دنوں`, `مہینہ`/`مہینے`).
- [ ] Hijri month names authored; runtime selection by the contract chosen in .1; exact-output tests per month.
- [ ] Urdu tests assert absence of U+200F, U+200E, U+061C in authored output via a shared helper.
- [ ] Calendar surface carries calendar-keyed month arrays (`hijriMonths` or `months: { gregorian, hijri }`); `OrdinalDatePattern` indexes the right array for the active calendar; existing locales unaffected (generator + runtime tests prove backward compat).
- [ ] `number.formatting` overrides `decimalSeparator`, `groupSeparator`, `negativeSign`.
- [ ] Cross-platform byte-identical output across net48 (Windows host) / net8 / net10 for every Urdu test.
- [ ] Cross-locale gender-audit artifact `artifacts/2026-04-12-grammatical-gender-audit.md` exists (committed). Every non-research-bound gap is filled in this epic across the pre-split tasks `.12` (Romance/Germanic), `.13` (Slavic), `.14` (Semitic/Indic/other). Research-beyond-CLDR-plus-one-grammar gaps may have follow-up tasks OUTSIDE the epic — their IDs are listed in the audit.
- [ ] Any source-generator / runtime-engine changes (ordinalizer engine extension, calendar surface extension) include dedicated generator tests and runtime tests.
- [ ] Docs updated to **65** shipped locale files (3 new files added: `ur`, `ur-PK`, `ur-IN`). Wording distinguishes "language" from "shipped locale file / culture profile" where appropriate.
- [ ] `release_notes.md` entry added announcing Urdu + variants + Hijri + cross-locale gender audit.
- [ ] `.agents/skills/add-locale/` updated with any new reference notes for future Hijri / gendered-ordinal / regional-variant work.
- [ ] `.flow/memory/` + user memory (`/Users/claire/.claude/projects/-Users-claire-dev-Humanizer/memory/`) capture durable lessons.
- [ ] Parity map `artifacts/2026-04-12-ur-parity-map.md` (committed) has an empty unresolved-questions section; every authored string matches the parity map verbatim.
- [ ] PR description credits PR #1683 author.
- [ ] No acceptance item marked "deferred"; external follow-ups have explicit Flow IDs.
- [ ] All commits on branch `feat/urdu-locale`.

## Early proof point

Task `.1` validates the YAML-pipeline assumption AND locks three contract decisions (ordinal API matrix, number engine, Hijri contract). If any of the three decisions surfaces a larger rewrite than the plan anticipates, the epic re-plans BEFORE launching `.2`+ content tasks. This is the epic's cheapest exit before authoring linguistic content against an unstable foundation.

## Requirement coverage

| Req | Description | Task(s) | Gap justification |
|-----|-------------|---------|-------------------|
| R1  | `ur.yml` covers all eight canonical surfaces | .1, .2, .3, .4, .5 | — |
| R2  | Source-generator build passes after every content task | .1–.5, .9, .10, .11 | — |
| R3  | Registry + matrix completeness sweeps pass for `ur`, `ur-PK`, `ur-IN` across every `ShippedLocaleRows` dataset | .6 | — |
| R4  | Locale-specific exact-output proofs with frozen strings | .6 | — |
| R5  | Numbers use engine proven in .1, with South Asian lakh/crore scale | .3 | — |
| R6  | Cardinal table 0–99 (.3) + full `ordinal.numeric` masculine + feminine authoring + word-ordinal engine (.9); `.4` authors only `ordinal.date` / `.dateOnly` / `clock` to avoid referencing an engine not yet implemented | .3, .9 | — |
| R7  | Singular vs oblique-plural stems in phrases | .2 | — |
| R8  | Clock + compass + calendar surfaces | .4, .5 | — |
| R9  | No U+200F / U+200E / U+061C in authored output (explicit helper) | .6, extended through .7 | — |
| R10 | Explicit `calendar.months` + `number.formatting` overrides | .5 | — |
| R11 | Cross-platform byte-identical output | .7 (runs after all content stable) | — |
| R12 | Doc count 62→65 + language vs locale-file wording + release notes | .8 | — |
| R13 | Feedback loop: skill + memory updates | .8 | — |
| R14 | Parity map committed with empty unresolved set | .1 creates + .7 final-clears | — |
| R15 | Branch `feat/urdu-locale` used exclusively | all tasks | — |
| R16 | Gendered word-ordinals end-to-end for Urdu, no deferral at any task boundary | .9 | — |
| R17 | Hijri calendar coverage using contract chosen in .1 | .1 (contract), .10 (implementation) | — |
| R18 | Regional variants `ur-PK`, `ur-IN` with matrix rows in every shipped-locale dataset | .11, .6 | — |
| R19 | Engine / source-generator changes scoped to ordinalizer + calendar surface, data held in `LocaleFeature.ProfileRoot` (no typed per-gender properties on `ResolvedLocaleDefinition`) | .9, .10 | — |
| R20 | PR #1683 credit (no reopen / rebase) | .8 | — |
| R21 | Cross-locale gender audit + fills; only research-beyond-CLDR-plus-one-grammar gaps may escape to follow-up tasks outside the epic | .12 (audit + Romance/Germanic), .13 (Slavic), .14 (Semitic/Indic/other) | — |
