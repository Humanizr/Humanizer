# fn-8-add-urdu-ur-locale-with-full-language.3 Author number surfaces (words, parse, formatting overrides)

## Description

Author the `number` surface using the engine chosen in `.1` Decision 2. Author number-to-words (cardinals 0-99, scale words up to کھرب), words-to-number parse, and `number.formatting` overrides. If `.1` Decision 1b designated `.3` as the owner of gendered-ordinal `NumberToWords` output, author that here too; otherwise that work lives in `.9`.

**Size:** M
**Files:**
- `src/Humanizer/Locales/ur.yml` (edit)
- `artifacts/2026-04-12-ur-parity-map.md` (update)
- If Decision 1b routed to `.3`: `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/NumberToWordsProfileCatalogInput.cs` + the selected runtime number-to-words converter (edit)

## Ownership split (per `.1` Decisions 1a, 1b)

- **`.3` owns**:
  - Urdu cardinal `NumberToWords` output for all three culture IDs.
  - Urdu `WordsToNumber` cardinal parse (token-map).
  - `number.formatting` overrides.
  - IF `.1` Decision 1b said the chosen number-words engine already supports per-gender word ordinals: author the gendered ordinal payload here.
- **`.9` owns**:
  - `IOrdinalizer` word-ordinal engine for `(int|string).Ordinalize` paths.
  - IF `.1` Decision 1b said `NumberToWords` engine must be EXTENDED for per-gender word ordinals: that extension (profile-catalog + runtime) lives in `.9`, not `.3`.

Record the concrete split in the parity map so `.9`'s acceptance is unambiguous.

## Approach

1. Re-anchor on parity-map `.1` Decision 2 (number engine) and Decision 1b (ordinal-owner split). If recorded choice turns out wrong in practice, update parity map and post a re-plan note — do not silently switch engines.
2. **`number.words`** per chosen engine:
   - If `conjunctional-scale` proved sufficient: `unitsMap` 0–99 (all distinct) + `scales` (1000 ہزار, 100000 لاکھ, 10000000 کروڑ, 1000000000 ارب, 100000000000 کھرب).
   - Otherwise follow the chosen engine's shape.
   - Cross-check 0–99 vocabulary against PR #1683 resx and `forzagreen/n2words` `ur-PK.js` `BELOW_HUNDRED`. Discrepancies via proposer+reviewer.
3. **`number.parse`** — `engine: 'token-map'`, `cardinalMap` covering 0-99 + scales; `negativePrefixes: ['منفی']`; `ignoredTokens: ['اعشاریہ']`; `useHundredMultiplier: true`.
4. **`number.formatting`** — `decimalSeparator: '.'`, `groupSeparator: ','`, `negativeSign: '-'`. No LRM on signs.
5. If Decision 1b routed here: author per-gender ordinal payload per the engine's shape.
6. Run:
   ```bash
   dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 \
     --filter "FullyQualifiedName~NumberWordPhraseTests"
   ```

## Investigation targets

**Required**:
- Parity map `.1` Decisions 1b, 2
- `/Users/claire/dev/Humanizer/src/Humanizer/Locales/en-IN.yml`
- `/Users/claire/dev/Humanizer/docs/locale-yaml-reference.md:57-80`
- `/Users/claire/dev/Humanizer/src/Humanizer/Localisation/NumberToWords/ConjunctionalScaleNumberToWordsConverter.cs` + engine named in parity map

**Optional**:
- https://github.com/forzagreen/n2words/blob/main/src/ur-PK.js
- Humanizer PR #1683 resx

## Key context

- No Arabic-Indic digit output; `{count}` yields Latin digits per CLDR `defaultNumberingSystem: latn`.
- No `appended-group` engine (Urdu has no dual).
- No million/billion — Urdu uses lakh/crore.
- All authored strings match parity-map-frozen reviewer-approved values.
- Content directionality sweep: `rg -P '\x{200E}|\x{200F}|\x{061C}' src/Humanizer/Locales/ur.yml` must return no matches.

## Acceptance

- [ ] `surfaces.number.words` authored per `.1` Decision 2; 0–99 `unitsMap` distinct + South Asian scales.
- [ ] Strings match parity map verbatim. Passing: `NumberToWords(0, "ur") == "صفر"`, `(21) == "اکیس"`, `(99) == "ننانوے"`, `(100) == "ایک سو"`, `(100000) == "ایک لاکھ"`, `(10000000) == "ایک کروڑ"`, `(1234567)` matches parity-map string.
- [ ] `surfaces.number.parse` round-trip: `NumberToWords(n, "ur") |> WordsToNumber(_, "ur") == n` for 21, 101, 1001, 100000, 1234567.
- [ ] `surfaces.number.formatting` authored with `.` / `,` / `-`.
- [ ] If Decision 1b owned by `.3`: gendered ordinal payload authored; `5.ToOrdinalWords(Masculine, "ur")` + `(Feminine)` return parity-map-frozen strings.
- [ ] `rg -P '\x{200E}|\x{200F}|\x{061C}' src/Humanizer/Locales/ur.yml` returns no matches.
- [ ] No Arabic-only letters (ي ه ك).
- [ ] All cardinal + scale terms recorded in parity map with reviewer approval.
- [ ] `NumberWordPhraseTests` pass on net10.0.

## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
