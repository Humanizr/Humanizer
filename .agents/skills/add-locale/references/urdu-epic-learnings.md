# Learnings from the Urdu (ur) Epic

Reference notes for future locale work, captured from the fn-8 Urdu epic.

## Arabic-Script Letter Subset Differences

Urdu uses a distinct subset of Arabic-script letters that differ from Arabic:

- Urdu `ہ` (U+06C1 Heh Goal) vs Arabic `ه` (U+0647 Heh)
- Urdu `ی` (U+06CC Farsi Yeh) vs Arabic `ي` (U+064A Yeh)
- Urdu `ک` (U+06A9 Keheh) vs Arabic `ك` (U+0643 Kaf)

Do not copy-paste from Arabic locale data without substituting these characters. Visually similar but distinct Unicode code points will produce incorrect output for native speakers.

## CLDR Plural Rule for Indo-Aryan Languages

Urdu uses `singular-plural` (`one`/`other`) plural rules, not `arabic-like`. The `arabic-like` detector produces incorrect resource keys for Urdu because Arabic has `zero`, `one`, `two`, `few`, `many`, `other` categories. Use `singular-plural` for Urdu, Hindi, and other Indo-Aryan languages.

## South Asian Lakh/Crore Number Scales

Urdu (and Hindi, Bengali, etc.) use the South Asian grouping system: lakh (100,000), crore (10,000,000), arab (1,000,000,000), kharab (100,000,000,000). The `indian-grouping` engine handles this. When adding future Indic locales, use `en-IN.yml` and `ur.yml` as templates for the scale structure.

## Word-Ordinal Engine Gap

Numeric-suffix ordinalizer engines (e.g., `suffix`, `template`) produce digit-based ordinals like `5واں`. For languages that require full word ordinals (e.g., `پانچواں`), the `number-word-suffix` engine was created. It internally calls the locale's `INumberToWordsConverter` and appends gendered suffixes, with exact replacements for irregular low ordinals. Future locales needing word ordinals should use this engine rather than creating locale-specific leaves.

## Hijri Calendar Contract

`DateTime` and `DateOnly` do not retain the `Calendar` used to construct them, so the runtime cannot infer the calendar from the value alone. The chosen contract: when `calendarMode: 'Native'` is set on `ordinal.date`/`ordinal.dateOnly` and the culture's default calendar is `HijriCalendar` or `UmAlQuraCalendar`, the runtime uses the `hijriMonths` array from the YAML `calendar` surface. This is a culture-identity-based contract, not a value-based contract.

## Regional Variant File Exception for Matrix Coverage

Parity epics may ship minimum-valid no-delta variant files (`locale:` + `variantOf:` + `surfaces: {}`) when first-class matrix/sweep coverage requires the regional culture to be explicit in `LocaleRegistrySweepTests` and `LocaleTheoryMatrixCompletenessTests`. These files inherit all surfaces from the parent and produce identical output. Examples: `ur-PK.yml` and `ur-IN.yml`.

## Dense Number Maps Need exactReplacements for Irregular Ordinals

Even with a dense 0-99 cardinal map, ordinals for numbers like 4th, 6th, and 9th may have different stems than their cardinals (e.g., Urdu cardinal `چار` but ordinal `چوتھا`). Always check for stem changes in ordinals and use `exactReplacements` for irregular entries.
