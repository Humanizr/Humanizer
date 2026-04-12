# fn-8-add-urdu-ur-locale-with-full-language.2 Author formatter + phrases surfaces (relativeDate, duration, timeUnits, dataUnits)

## Description
Author the `formatter` surface and all four `phrases` tables (`relativeDate`, `duration`, `timeUnits`, `dataUnits`) in `src/Humanizer/Locales/ur.yml`. Every term goes through the proposer+reviewer subagent flow per `.agents/skills/add-locale/SKILL.md`. Honor CLDR's one/other distinction — do not flatten `گھنٹہ` to cover both counts.

**Size:** M
**Files:**
- `src/Humanizer/Locales/ur.yml` (edit)
- `artifacts/2026-04-12-ur-parity-map.md` (update: mark formatter + phrases as resolved)

## Approach

1. **Formatter block** (small): set `engine: 'profiled'`, `pluralRule: 'singular-plural'`, `dataUnitPluralRule: 'singular-plural'`. Do NOT use `arabic-like` — CLDR `ur` is two-form (one when `i=1 and v=0`, else other). Set `dataUnitFallbackTransform` if needed for byte-size unit derivation.
2. **phrases.relativeDate** (CLDR `dateFields.json` for `ur` is the source of truth):
   - past: `پہلے` suffix (NOT `قبل`)
   - future: `بعد` / `میں` suffix per CLDR
   - Singular vs plural stems: سال (both) / ماہ (sg)-مہینے (pl) / ہفتہ (sg)-ہفتے (pl) / دن (sg)-دنوں (pl) / گھنٹہ (sg)-گھنٹے (pl) / منٹ (both) / سیکنڈ (both)
   - Special single-unit forms: "now" = `ابھی`, "yesterday" = `گزشتہ کل`, "tomorrow" = `آئندہ کل` (per CLDR disambiguated forms, not vernacular `کل` for both)
3. **phrases.duration**: mirror relativeDate plural-form decisions for each unit.
4. **phrases.timeUnits**: CLDR short + narrow are identical to long for Urdu. Use the same plural-stem table throughout.
5. **phrases.dataUnits**: byte-size unit names in Urdu (بائٹ, کلوبائٹ, میگابائٹ, گیگابائٹ, ٹیرابائٹ). Suffix scheme per `dataUnitFallbackTransform`.
6. **Proposer+reviewer per SKILL.md**: for each distinct term, emit a proposer draft + reviewer check (native speaker). Record proposed/reviewed tokens in the parity map artifact. Empty unresolved entries as each term lands.
7. Build and run targeted tests to exercise the phrases:
   ```bash
   dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 \
     --filter "FullyQualifiedName~GeneratedFormatterRuntimeTests|ResourceBackedPhraseTests"
   ```

## Investigation targets

**Required**:
- `/Users/claire/dev/Humanizer/src/Humanizer/Locales/fa.yml:1-60` — closest structural template (Perso-Arabic, clitic list, similar phrase shape)
- `/Users/claire/dev/Humanizer/src/Humanizer/Locales/he.yml:1-20` — shows the legacy `resourceKeyDetector`/`dataUnitDetector` aliases if needed
- `/Users/claire/dev/Humanizer/docs/locale-yaml-reference.md:88-123,1-200` — `phrases.*` shape, singular/plural/forms/afterCount pattern
- `/Users/claire/dev/Humanizer/src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs:141-142` — formatter grammar keys
- `/Users/claire/dev/Humanizer/src/Humanizer/Localisation/Formatters/ProfiledFormatter.cs:245-287` — enum semantics behind `pluralRule`

**Optional** (for cross-checking vocabulary):
- CLDR JSON `cldr-dates-full/main/ur/dateFields.json` for official relative-time strings
- `moment/locale/ur.js` / `dayjs/locale/ur.js` for secondary confirmation

## Key context

- **Do not copy Arabic strings**. Urdu uses Perso-Arabic script with distinct letters: ہ (U+06C1, not ه U+0647), ی (U+06CC, not ي U+064A), ک (U+06A9, not ك U+0643), plus retroflex ٹ ڈ ڑ and ں (noon ghunna).
- **Do not use `pluralRule: 'arabic-like'`** — that's 6 forms (zero/one/two/few/many/other). Urdu is 2 forms.
- **No U+200F / U+200E / U+061C**. Never type directionality controls into YAML. `.6` adds an explicit `UrduBidiControlSweep` helper asserting absence across runtime outputs.
- **ASCII digits**. Templates use `{count}` placeholders which the formatter renders as Latin digits per CLDR `defaultNumberingSystem: latn`.
- **Arabic comma U+060C (،)** is legitimate in Urdu date patterns; don't replace with `,`.
## Acceptance
- [ ] `surfaces.formatter` authored with `engine: 'profiled'`, `pluralRule: 'singular-plural'`, `dataUnitPluralRule: 'singular-plural'`.
- [ ] `surfaces.phrases.relativeDate` authored with singular and plural entries for the full `TimeUnitOrder` slot list — `millisecond`, `second`, `minute`, `hour`, `day`, `week`, `month`, `year` (closed generator order per `DataUnit.cs`); past uses پہلے; future uses بعد / میں; zero/one-step forms use CLDR `ابھی`/`گزشتہ کل`/`آئندہ کل`.
- [ ] Stems differ between singular and plural where CLDR requires it (at minimum: گھنٹہ vs گھنٹے, دن vs دنوں, مہینہ vs مہینے, ہفتہ vs ہفتے).
- [ ] `surfaces.phrases.duration` authored for the full `TimeUnitOrder`: `millisecond`, `second`, `minute`, `hour`, `day`, `week`, `month`, `year` with matching singular/plural shape.
- [ ] `surfaces.phrases.timeUnits` authored for the full `TimeUnitOrder` (same 8 slots). Short/narrow == long per CLDR.
- [ ] `surfaces.phrases.dataUnits` authored for the full `DataUnitOrder`: `bit`, `byte`, `kilobyte`, `megabyte`, `gigabyte`, `terabyte` (Urdu: بٹ / بائٹ / کلوبائٹ / میگابائٹ / گیگابائٹ / ٹیرابائٹ) with plural forms per `dataUnitPluralRule`.
- [ ] `rg -P '\x{200E}|\x{200F}|\x{061C}' src/Humanizer/Locales/ur.yml` returns no matches.
- [ ] No Arabic-script letters mis-used: grep confirms ه (U+0647), ي (U+064A), ك (U+0643) do not appear in `ur.yml`; only ہ ی ک are used.
- [ ] Proposer + reviewer entries recorded for every authored term in `artifacts/2026-04-12-ur-parity-map.md`.
- [ ] Build passes: `dotnet build src/Humanizer/Humanizer.csproj -c Release`.
- [ ] Phrase-level runtime tests pass on net10.0 (`GeneratedFormatterRuntimeTests` + `ResourceBackedPhraseTests`).
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
