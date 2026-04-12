# fn-8-add-urdu-ur-locale-with-full-language.10 Islamic (Hijri) calendar native mode for Urdu dates

## Description

Add Islamic (Hijri) calendar coverage for Urdu dates using the contract decided in `.1` Decision 3 (with feasibility proof). Extend the `calendar` YAML surface with calendar-keyed month arrays — schema extension is **expected**, not conditional. Update `OrdinalDatePattern` (and any converters touched by the chosen contract) to index the right month array for the active calendar. Generator and runtime tests are mandatory.

Schema documentation delta is recorded for `.8` — this task does NOT commit doc edits.

**Size:** M
**Files (expected):**
- `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs` (edit — AddCalendarFeatures accepts new keys)
- `src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs` (edit — ResolveCalendar parses new shape)
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalDateProfileCatalogInput.cs` (edit — emit both month arrays)
- `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs` (edit — select array by active calendar)
- IF contract C chosen: `src/Humanizer/Localisation/DateToOrdinalWords/Default*Converter.cs`, `src/Humanizer/Localisation/DateToOrdinalWords/IDateToOrdinalWordConverter.cs`, `src/Humanizer/Localisation/DateToOrdinalWords/IDateOnlyToOrdinalWordConverter.cs`, `src/Humanizer/DateHumanizeExtensions.cs` (or the public `ToOrdinalWords` extension file), XML docs, public-API approval tests if the repo has them
- `src/Humanizer/Locales/ur.yml` (edit — add Hijri month table under `calendar`; set `calendarMode` under `ordinal.date` / `ordinal.dateOnly` if contract A/B was chosen)
- `tests/Humanizer.SourceGenerators.Tests/…` (new tests for calendar-keyed shape)

## Approach

1. Re-anchor on `.1` Decision 3 contract (A/B/C) AND its feasibility proof:
   - For **A** (culture-calendar): `OptionalCalendars` survey for `ur`/`ur-PK`/`ur-IN` must show the Urdu culture(s) can accept `HijriCalendar` or `UmAlQuraCalendar`.
   - For **B** (explicit `calendarMode`): the public caller mechanism (sibling locale, thread-local mode, or public API) must be specified concretely.
   - For **C** (new overload): the full call chain that stops relying on `CurrentCulture` in `OrdinalDatePattern` must be documented.
   If the feasibility proof is missing, escalate to epic re-plan — do NOT paper over with a shaky contract.
2. **Schema extension** (mandatory). Current state: `CanonicalLocaleAuthoring.AddCalendarFeatures` accepts only `months` / `monthsGenitive`. Add one of:
   - **Option I**: new key `calendar.hijriMonths` (12 entries). Simpler diff; easy backward compat.
   - **Option II**: calendar-keyed map `calendar.months: { gregorian: [...], hijri: [...] }`. Cleaner generalization but requires compat shim for existing flat-array authoring.
   Prefer Option I unless `.1` Decision 3 explicitly calls for Option II. Preserve backward compat for every existing locale either way.
3. **Generator plumbing**:
   - `CanonicalLocaleAuthoring.AddCalendarFeatures` accepts the new key.
   - `LocaleYamlCatalog.ResolveCalendar` validates array length (12), no duplicates, no U+200F / U+200E / U+061C, no Arabic-only letters (ي ه ك) in Urdu Hijri names.
   - `OrdinalDateProfileCatalogInput` extracts BOTH arrays and emits them.
4. **Runtime selection** (per the chosen contract):
   - **A**: `OrdinalDatePattern` reads `CultureInfo.CurrentCulture.DateTimeFormat.Calendar`; if it's `HijriCalendar` / `UmAlQuraCalendar` AND the locale profile has a Hijri array, index Hijri.
   - **B**: `OrdinalDatePattern` reads `calendarMode` from the profile; if `Islamic`, index Hijri array. Callers switch mode per the public mechanism specified in `.1` (or use a sibling locale variant).
   - **C**: new overload accepts `Calendar calendar`; `OrdinalDatePattern` receives the Calendar and picks the array; no CurrentCulture read.
5. **Native date-component extraction** (project-memory pitfall): on the Hijri path, day/month/year come from the active `Calendar`'s `GetDayOfMonth` / `GetMonth` / `GetYear`, never from raw `DateTime.Day` / `.Month` / `.Year`.
6. **Urdu Hijri month names** (plain-alif Urdu spelling):
   - محرم، صفر، ربیع الاول، ربیع الثانی، جمادی الاول، جمادی الثانی، رجب، شعبان، رمضان، شوال، ذوالقعدہ، ذوالحجہ.
   - Reviewer confirm: plain ا (U+0627), not آ (U+0622) or أ (U+0623) at start of compound.
7. **Tests (local sanity; full coverage in `.6`)**:
   - Generator tests: calendar-keyed shape accepted; wrong length rejected; existing locales unaffected.
   - Runtime tests: chosen contract yields Hijri month names; Gregorian path unchanged; variants inherit via `variantOf`.

## Investigation targets

**Required**:
- Parity map `.1` Decision 3 + feasibility proof
- `/Users/claire/dev/Humanizer/src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs:AddCalendarFeatures`
- `/Users/claire/dev/Humanizer/src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs:ResolveCalendar`
- `/Users/claire/dev/Humanizer/src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalDateProfileCatalogInput.cs`
- `/Users/claire/dev/Humanizer/src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs`
- `/Users/claire/dev/Humanizer/src/Humanizer/Localisation/DateToOrdinalWords/DefaultDateToOrdinalWordConverter.cs`
- `.flow/memory/pitfalls.md`

**Optional**:
- https://github.com/unicode-org/cldr-json `cldr-cal-islamic-full/main/ur/ca-islamic.json`

## Key context

- Schema extension is **expected**, not conditional.
- `ur-PK` / `ur-IN` must inherit Urdu's Hijri month table via `variantOf` merge semantics.
- Native-calendar output uses native `Calendar` getters throughout.
- Docs for the new calendar surface are edited in `.8` — this task records the delta for `.8` to pick up.

## Acceptance

- [ ] Feasibility proof from `.1` Decision 3 confirmed before any implementation.
- [ ] Calendar YAML surface extended; `CanonicalLocaleAuthoring`, `LocaleYamlCatalog.ResolveCalendar`, `OrdinalDateProfileCatalogInput` plumb the new shape end-to-end.
- [ ] Existing locales unaffected: every previously shipped `calendar.months` continues to work; no regression.
- [ ] 12 Urdu Hijri month names authored in `ur.yml` with plain-alif Urdu spelling (parity-map-frozen).
- [ ] Runtime selection per `.1` Decision 3 contract yields Hijri month names for Hijri path; Gregorian path unchanged.
- [ ] Hijri path uses `Calendar.GetDayOfMonth` / `GetMonth` / `GetYear`; no raw `DateTime.*` extraction.
- [ ] 12 per-month exact-output tests pass.
- [ ] Round-trip test: constructing a date via the Hijri contract → converter call → native Hijri month name.
- [ ] Variants (`ur-PK`, `ur-IN`) inherit the Hijri table through `variantOf` — verified in a test.
- [ ] Generator tests: calendar-keyed shape accepted, wrong-length rejected, existing-locale backward compat proven.
- [ ] `rg -P '\x{200E}|\x{200F}|\x{061C}'` on all Hijri output returns no matches.
- [ ] Schema-doc delta recorded for `.8` in the parity map (task `.10` does not commit doc edits).
- [ ] Parity map updated with per-Hijri-month audit entries.

## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
