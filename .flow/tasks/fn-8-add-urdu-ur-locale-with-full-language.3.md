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
   - Engine is `indian-grouping-gendered` (locked in `.1` Decision 2). YAML shape uses `denseUnitsMap` (100-entry string array, 0–99), `hundredsMap`, `thousandsMap`, `lakhWord`, `singleLakhWord`, `croreWord`, `negativeWord`, `zeroWord` per the locked parity-map schema. See `artifacts/2026-04-12-ur-parity-map.md` Decision 2 for the full shape.
   <!-- Updated by plan-sync: fn-8-add-urdu-ur-locale-with-full-language.1 chose indian-grouping-gendered engine with denseUnitsMap, not conjunctional-scale with unitsMap/scales -->
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

- [ ] `surfaces.number.words` authored per `.1` Decision 2; 0–99 `denseUnitsMap` distinct + South Asian scale fields (`lakhWord`, `croreWord`, `hundredsMap`, `thousandsMap`, etc.).
<!-- Updated by plan-sync: fn-8-add-urdu-ur-locale-with-full-language.1 locked denseUnitsMap not unitsMap, and individual scale fields not a scales tuple array -->
- [ ] Strings match parity map verbatim. Passing: `NumberToWords(0, "ur") == "صفر"`, `(21) == "اکیس"`, `(99) == "ننانوے"`, `(100) == "ایک سو"`, `(100000) == "ایک لاکھ"`, `(10000000) == "ایک کروڑ"`, `(1234567)` matches parity-map string.
- [ ] `surfaces.number.parse` round-trip: `NumberToWords(n, "ur") |> WordsToNumber(_, "ur") == n` for 21, 101, 1001, 100000, 1234567.
- [ ] `surfaces.number.formatting` authored with `.` / `,` / `-`.
- [ ] If Decision 1b owned by `.3`: gendered ordinal payload authored; `5.ToOrdinalWords(Masculine, "ur")` + `(Feminine)` return parity-map-frozen strings.
- [ ] `rg -P '\x{200E}|\x{200F}|\x{061C}' src/Humanizer/Locales/ur.yml` returns no matches.
- [ ] No Arabic-only letters (ي ه ك).
- [ ] All cardinal + scale terms recorded in parity map with reviewer approval.
- [ ] `NumberWordPhraseTests` pass on net10.0.

## Done summary
## Task .3 Summary: Author number surfaces (words, parse, formatting overrides)

### Delivered
1. **New `indian-grouping-gendered` engine**: Created `IndianGroupingGenderedNumberToWordsConverter` extending `GenderedNumberToWordsConverter` with dense 0-99 lookup and South Asian scale decomposition (lakh/crore/arab/kharab).
2. **Engine contract**: Added `indian-grouping-gendered` schema to `EngineContractCatalog.cs` with `denseUnitsMap`, scalar scale words, and nested ordinal gendered block.
3. **`number.words` surface**: 100 lexically distinct Urdu number words (0-99) authored in `denseUnitsMap`, plus scale words, ordinal gender suffixes with exactReplacements for 1-3.
4. **`number.parse` surface**: Token-map engine with all 100 cardinal words + 5 scale tokens, `useHundredMultiplier: true`, negative prefix `منفی`.
5. **`number.formatting` surface**: Overrides for `.` / `,` / `-` (stripping ICU's U+200E LRM from native negative sign).
6. **Test data**: Added 28 cardinal, 6 ordinal, and 5 words-to-number round-trip test cases to `LocaleNumberTheoryData`.

### Verification
- Build succeeds on all 4 TFMs (net10.0, net8.0, net48, netstandard2.0)
- 10,515 NumberWordPhraseTests pass on net10.0 and net8.0
- 58 source generator tests pass
- No bidi control characters (U+200E/200F/061C) in ur.yml
- No Arabic-only letters (ي ه ك) in ur.yml
## Evidence
- Commits:
- Tests:
- PRs:
