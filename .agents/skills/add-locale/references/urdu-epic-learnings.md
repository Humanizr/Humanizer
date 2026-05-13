# Urdu Epic Learnings

Use this as a tagged failure-mode catalog for future locale parity work. Do not load it for every locale by default; load it when the target locale or touched subsystem matches a tag below.

Each lesson is operational: identify the failure, run the fast-fail probe, and leave the required proof behind in tests or the parity map.

## Fast Index

| Tags | Lesson |
| --- | --- |
| `arabic-script`, `ur` | Urdu uses different Arabic-script code points than Arabic. |
| `plural-rules`, `ur`, `indic` | Urdu uses singular/plural resource categories, not Arabic-like plural categories. |
| `indic`, `south-asian`, `number.words`, `number.parse` | South Asian scale words must cover lakh/crore/arab/kharab boundaries. |
| `ordinal`, `number.words` | Word ordinals need a word-aware engine and irregular stem replacements. |
| `ordinal`, `culture`, `shared-engine` | Engines that call `NumberToWords` must bind the target culture intrinsically. |
| `calendar`, `hijri`, `icu`, `nls` | Date values do not retain construction calendar and optional calendars differ by platform. |
| `variant`, `schema` | No-delta regional variants should omit `surfaces` and prove same-language inheritance. |
| `schema`, `generator` | Optional fields must survive parser, resolver, emitter, migration, and runtime consumption. |
| `engine-tests` | Shared engine bugs can be masked by real locale data; add synthetic divergent tests. |
| `hygiene`, `pr` | Remove local agent state, build residue, namespace mismatches, and unrelated edits before closeout. |

## Arabic-Script Letter Subset Differences

**Tags:** `arabic-script`, `ur`

**Failure observed:** Copying Arabic locale data can introduce visually similar but wrong Unicode code points:

- Urdu `ہ` (U+06C1 Heh Goal), not Arabic `ه` (U+0647 Heh)
- Urdu `ی` (U+06CC Farsi Yeh), not Arabic `ي` (U+064A Yeh)
- Urdu `ک` (U+06A9 Keheh), not Arabic `ك` (U+0643 Kaf)

**Fast-fail probe:** Search generated outputs, YAML terms, parser tokens, and tests for Arabic lookalike code points when adding Urdu or related Arabic-script locale data.

**Required proof:** Exact-output tests must contain the target-locale letters, and the parity map should record the codepoint sanity check when script lookalikes are a risk.

**Reusable rule:** Do not copy from a neighboring script-family locale without validating actual code points.

## CLDR Plural Rule Family

**Tags:** `plural-rules`, `ur`, `indic`

**Failure observed:** Urdu uses `singular-plural` (`one`/`other`) resource categories, not Arabic-like six-category plural rules. The Arabic-like detector produces the wrong resource keys for Urdu.

**Fast-fail probe:** Before authoring formatter/phrase resources, identify the locale's plural-rule family instead of copying from a nearby language.

**Required proof:** Formatter/phrase tests must hit the resource categories the locale actually uses.

**Reusable rule:** Proximity in script or region is not plural-rule evidence.

## South Asian Number Scales

**Tags:** `indic`, `south-asian`, `number.words`, `number.parse`

**Failure observed:** Urdu uses South Asian grouping and scale words: lakh (100,000), crore (10,000,000), arab (1,000,000,000), and kharab (100,000,000,000). Western thousand/million-only assumptions miss natural output and parser coverage.

**Fast-fail probe:** For South Asian locales, decide whether the natural number system uses lakh/crore-style scale boundaries before selecting a number engine.

**Required proof:** Add word and parse tests around each meaningful scale boundary.

**Reusable rule:** Use `en-IN.yml`, `hi`, and `ur` patterns as starting points for South Asian scale structure, not generic English large-number assumptions.

## Word Ordinal Engine Gap

**Tags:** `ordinal`, `number.words`

**Failure observed:** Numeric suffix ordinalizer engines such as `suffix` or `template` produce digit-based output like `5واں`. Urdu requires word ordinals like `پانچواں`.

**Fast-fail probe:** Ask whether users naturally expect digit+suffix or full word ordinals for `Ordinalize`, `ToOrdinalWords`, and date ordinals. Keep the API paths separate.

**Required proof:** Exact-output tests for `number.words.ordinal`, `ordinal.numeric`, `ordinal.date`, and `ordinal.dateOnly`, including irregular stems. Even with dense cardinal maps, irregular low ordinals may need `exactReplacements`; do not assume cardinal stems compose correctly.

**Reusable rule:** For locales that require word ordinals, use or extend a word-aware ordinal engine instead of a digit-suffix engine.

## Ordinalizer Culture Binding

**Tags:** `ordinal`, `culture`, `shared-engine`

**Failure observed:** The `number-word-suffix` ordinalizer calls the locale's `INumberToWordsConverter`; it must receive the target culture. Relying on ambient/current culture or author-controlled YAML flags can produce wrong-language output.

**Fast-fail probe:** When a shared engine calls a culture-aware service, identify how the generated registry passes the target culture.

**Required proof:** Add or identify a test where current culture differs from the target culture and the output still uses the target locale.

**Reusable rule:** Culture binding for engine dependencies should be intrinsic in generator/runtime contract metadata, not a fragile YAML authoring toggle.

## Hijri Calendar Contract

**Tags:** `calendar`, `hijri`, `icu`, `nls`

**Failure observed:** `DateTime` and `DateOnly` do not retain the `Calendar` used to construct them. Runtime cannot infer Hijri identity from the value alone. Urdu also exposed a platform trap: `ur-IN` accepts `HijriCalendar` on .NET Framework/NLS but can reject it on ICU net8/net10 because optional calendars differ.

**Fast-fail probe:** Before adding non-Gregorian behavior, check whether the culture's default/optional calendars support the intended calendar on the target TFM/platforms.

**Required proof:** Capability-gated exact-output tests should keep the inheritance/locale proof strong where the calendar is supported and skip only unsupported platform combinations.

**Reusable rule:** `calendarMode: Native` is a culture-identity/default-calendar contract, not a value-construction-calendar contract.

## Regional Variant Files

**Tags:** `variant`, `schema`

**Failure observed:** `ur-PK` and `ur-IN` existed for first-class matrix/sweep coverage even though they inherited from `ur`. Empty `surfaces: {}` created schema noise and follow-up cleanup.

**Fast-fail probe:** For a regional file, decide whether it has real overrides or exists only to make the culture explicit in tests/registries.

**Required proof:** If there are no real overrides, omit `surfaces`, record the same-language parent chain, and add inheritance proof where the variant is matrix-visible.

**Reusable rule:** No-delta variants are allowed for explicit culture coverage, but they must not pretend to own surfaces.

## Optional Field Propagation

**Tags:** `schema`, `generator`

**Failure observed:** Optional fields can parse correctly but still be dropped by resolver, generated profile input, emitter, migration, or runtime consumer paths. Urdu's `hijriMonths` and no-delta `surfaces` cleanup exposed this risk.

**Fast-fail probe:** For every new optional YAML field or omitted section, trace parser → resolver/inheritance merge → profile/emitter → migration/default behavior → runtime consumer.

**Required proof:** Source-generator tests must prove the optional field is preserved or the omitted field defaults correctly through the full pipeline.

**Reusable rule:** Parsing is not enough; profile emission and migration paths are part of the contract.

## Shared Engine Masking

**Tags:** `engine-tests`, `shared-engine`

**Failure observed:** Real Urdu data did not expose every branch in shared ordinal logic. Positive/negative irregular replacement paths needed a synthetic divergent test to prove the engine, not just the locale data.

**Fast-fail probe:** When a shared engine has multiple data paths, ask whether real locale data makes those paths coincidentally identical.

**Required proof:** Add synthetic or sentinel tests with deliberately divergent data for the engine branch.

**Reusable rule:** Shared engine correctness needs engine-shaped tests, not only locale-shaped tests.

## PR Hygiene And Cross-Locale Drift

**Tags:** `hygiene`, `pr`

**Failure observed:** Urdu PR feedback included removing a local `.claude` lock file and fixing an unrelated Icelandic test namespace. Locale parity work can touch broad test/generator surfaces, making accidental edits easy to miss.

**Fast-fail probe:** Before closeout, inspect git status and changed file list for local agent state, build artifacts, unrelated cultures, namespace/folder mismatches, and generated residue.

**Required proof:** Clean diff with intentional files only; targeted tests for any non-target culture touched.

**Reusable rule:** Broad locale sweeps do not justify unrelated drift; either prove and explain the edit or remove it.
