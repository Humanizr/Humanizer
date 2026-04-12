# fn-8-add-urdu-ur-locale-with-full-language.5 Author list, compass, and calendar.months surfaces

## Description
Close out the remaining three canonical surfaces: `list` (sentence conjunction), `compass` (16 heading labels), and `calendar.months` (12 Gregorian month names). This is also where explicit calendar overrides pin cross-platform output against net48 NLS vs net8+/ICU drift.

**Size:** S (borderline M — three small blocks, all reference data)
**Files:**
- `src/Humanizer/Locales/ur.yml` (edit)
- `artifacts/2026-04-12-ur-parity-map.md` (update)

## Approach

1. **`list`**:
   - Urdu `اور` is a separate conjunction word. Use `engine: 'conjunction'` with `value: 'اور'` per the list-engine catalog documented in `docs/locale-yaml-reference.md` (`clitic` / `conjunction` / `delimited`).
   - Do NOT use `clitic` (that's Persian/Arabic `و` attached without space). Do NOT invent `oxford` — not a documented list engine in this repo.
   - `fa.yml` uses `clitic` with `'و'` — Persian/Arabic convention, NOT Urdu.
2. **`compass`**:
   - 16 full heading labels + 16 short labels in Urdu:
     - Cardinals: شمال (N), جنوب (S), مشرق (E), مغرب (W)
     - Ordinals: شمال مشرق (NE), جنوب مشرق (SE), جنوب مغرب (SW), شمال مغرب (NW)
     - Plus intercardinal variants (NNE, ENE, ESE, SSE, SSW, WSW, WNW, NNW) — standard Urdu compound forms.
   - Short labels: per CLDR/common usage, same as full (Urdu has no single-letter compass abbreviations). If a short form is required by the schema, use the two-word compound minus connectors, reviewer-approved.
3. **`calendar.months`**:
   - 12 Gregorian month names exactly: جنوری، فروری، مارچ، اپریل، مئی، جون، جولائی، اگست، ستمبر، اکتوبر، نومبر، دسمبر.
   - Author as an explicit override — contributor checklist at `docs/adding-a-locale.md:424` requires explicit YAML when ICU-supplied data differs across platforms. Confirm with probe output from fn-8.1 whether ICU on net10 and NLS on net48 agree; if they don't, the explicit override fixes the drift.
   - `calendar.monthsGenitive`: Urdu does not inflect month names; only author this if the schema requires it for parity. Otherwise omit.

## Investigation targets

**Required**:
- `/Users/claire/dev/Humanizer/docs/locale-yaml-reference.md` sections on `list`, `compass`, `calendar`
- `/Users/claire/dev/Humanizer/src/Humanizer/Locales/en.yml` — example list surface (note: Urdu uses `engine: 'conjunction'`, not English's Oxford-style)
- `/Users/claire/dev/Humanizer/src/Humanizer/Locales/ar.yml` compass block — RTL compass layout reference
- Probe output from fn-8.1 — decide whether `calendar.months` override is required or optional for byte identity

**Optional**:
- `cldr-json/cldr-dates-full/main/ur/ca-gregorian.json` — CLDR authoritative month names (confirms جنوری/فروری/etc.)

## Key context

- `اور` is a separate word — never attach it with no space.
- Compass: CLDR does not publish 16-entry Urdu compass labels; this is a locale-author deliverable. Reviewer MUST verify the intercardinal compounds are native-accurate.
- If `calendar.monthsGenitive` is a required surface (not just optional), author the 12 entries identical to `months` — Urdu has no genitive inflection.
## Acceptance
- [ ] `surfaces.list` authored with `engine: 'conjunction'` and `value: 'اور'`. Exact-output tests for two-item and three-item lists in `.6`. Clitic engine NOT used.
- [ ] `ToQuantity`-style list output: `["الف", "ب", "ج"].Humanize("ur")` produces the parity-map-frozen reviewer-approved three-element list string.
- [ ] `surfaces.compass.full` has all 16 Urdu heading labels.
- [ ] `surfaces.compass.short` has 16 entries (same as full if no short-form convention).
- [ ] `surfaces.calendar.months` has exactly 12 entries: جنوری، فروری، مارچ، اپریل، مئی، جون، جولائی، اگست، ستمبر، اکتوبر، نومبر، دسمبر.
- [ ] If the schema requires `surfaces.calendar.monthsGenitive`, the 12 entries match `months` (no Urdu inflection).
- [ ] No Arabic-only letter mis-use (ه ي ك absent from these sections).
- [ ] `rg -P '\x{200E}|\x{200F}|\x{061C}' src/Humanizer/Locales/ur.yml` returns no matches.
- [ ] Parity map artifact shows all 8 canonical surfaces now have at least one reviewed term; unresolved-surfaces set reduced to zero (moving to content-polish phase).
- [ ] `dotnet build` + `LocaleRegistrySweepTests` still pass.
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
