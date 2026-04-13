# Grammatical Gender Audit — Shipped Locales

**Date:** 2026-04-12
**Task:** fn-8-add-urdu-ur-locale-with-full-language.12

## Methodology

Enumerated all `src/Humanizer/Locales/*.yml` files (65 shipped locales). Cross-referenced each locale's:
- Linguistic gender system (from CLDR Ordinal Rules + WALS 30A)
- Ordinalizer YAML configuration (engine type, gender suffix/template coverage)
- Test matrix coverage (`LocaleOrdinalizerMatrixData` gender rows)
- NumberToWords gender support

## Summary

| Family | Locale | Gender System | Ordinal Engine | Gender Coverage | Gap Owner | Notes |
|--------|--------|---------------|----------------|-----------------|-----------|-------|
| **Romance** | es | masc/fem | word-form-template | ✅ Full (M/F/N) | — | Neuter→Masculine fallback |
| | fr | masc/fem | template | ✅ Full (M/F/N) | — | 1er/1ère distinction |
| | fr-BE | masc/fem | (inherits fr) | ✅ Full | — | No ordinal override |
| | fr-CH | masc/fem | (inherits fr) | ✅ Full | — | No ordinal override |
| | it | masc/fem | suffix | ✅ Full (M/F/N) | — | °/ª |
| | pt | masc/fem | suffix | ✅ Full (M/F/N) | — | º/ª |
| | pt-BR | masc/fem | (inherits pt) | ✅ Full | — | No numeric ordinal override |
| | ro | masc/fem/neuter | template | ✅ Full (M/F/N) | — | al X-lea / a X-a / primul/prima |
| | ca | masc/fem | template | ✅ Full (M/F/N) | — | -r/-n/-è (masc) / -a (fem) |
| **Germanic** | de | masc/fem/neuter | suffix | ✅ Full (M/F/N) | .12 filled | All genders use "." — explicit F/N added |
| | de-CH | masc/fem/neuter | (inherits de) | ✅ Full via inheritance | — | Inherits de complete coverage |
| | de-LI | masc/fem/neuter | (inherits de) | ✅ Full via inheritance | — | Inherits de complete coverage |
| | nl | common/neuter | suffix | ✅ Full (M/F/N) | .12 filled | All genders use "e" — explicit F/N added |
| | sv | common/neuter | modulo-suffix | ✅ No gender keys | — | ":e"/":a" by digit, not gender |
| | da | common/neuter | suffix | ✅ Full (M/F/N) | .12 filled | All genders use "." — explicit F/N added |
| | nb | masc/fem/neuter | suffix | ✅ Full (M/F/N) | .12 filled | All genders use "." — explicit F/N added |
| | nn | masc/fem/neuter | (no ordinal section) | ✅ via inheritance | — | Inherits nb/default |
| | is | masc/fem/neuter | suffix | ✅ Full (M/F/N) | .12 filled | All genders use "." — explicit F/N added |
| | lb | masc/fem/neuter | suffix | ✅ Full (M/F/N) | .12 filled | All genders use "." — explicit F/N added |
| | af | none (modern) | modulo-suffix | ✅ No gender keys | — | Afrikaans lost grammatical gender |
| | en | none | modulo-suffix | ✅ No gender keys | — | th/st/nd/rd by digit |
| | en-GB | none | (inherits en) | ✅ | — | |
| | en-IN | none | (inherits en) | ✅ | — | |
| | en-US | none | (overrides en) | ✅ | — | Uses en ordinal pattern |
| **Slavic** | bg | masc/fem/neuter | suffix | ✅ Full (M/F/N) | .13 filled | All genders use "." — explicit F/N added |
| | cs | masc/fem/neuter | suffix | ✅ Full (M/F/N) | .13 filled | All genders use "." — explicit F/N added |
| | hr | masc/fem | suffix | ✅ Full (M/F/N) | .13 filled | All genders use "." — explicit F/N added |
| | pl | masc/fem/neuter | suffix | ✅ Full (M/F/N) | .13 filled | All genders use "." — explicit F/N added |
| | ru | masc/fem/neuter | suffix | ✅ Full (M/F/N) | — | -й/-я/-е |
| | sk | masc/fem/neuter | suffix | ✅ Full (M/F/N) | .13 filled | All genders use "." — explicit F/N added |
| | sl | masc/fem/neuter | suffix | ✅ Full (M/F/N) | .13 filled | All genders use "." — explicit F/N added |
| | sr | masc/fem/neuter | suffix | ✅ Full (M/F/N) | .13 filled | All genders use "." — explicit F/N added |
| | sr-Latn | masc/fem/neuter | suffix | ✅ Full (M/F/N) | .13 filled | All genders use "." — explicit F/N added |
| | uk | masc/fem/neuter | template | ✅ Full (M/F/N) | — | Per-gender template sections |
| **Semitic** | ar | masc/fem | suffix | ✅ Full (M/F/N) | .14 filled | All genders use "" (empty) — explicit F/N added |
| | he | masc/fem | suffix | ✅ Full (M/F/N) | .14 filled | All genders use "" (empty) — explicit F/N added |
| | mt | masc/fem | suffix | ✅ Full (M/F/N) | .14 filled | All genders use "" (empty) — explicit F/N added |
| **Indic** | bn | none (ordinals) | template | ✅ Full | — | Gender-invariant ordinals |
| | ta | none (ordinals) | template | ✅ Full | — | Gender-invariant ordinals |
| | ur | masc/fem | number-word-suffix | ✅ Full (M/F) | — | Gendered via .9 plumbing |
| | ur-IN | masc/fem | (inherits ur) | ✅ | — | |
| | ur-PK | masc/fem | (inherits ur) | ✅ | — | |
| **Turkic** | az | none (ordinals) | suffix | ✅ | — | "." for all |
| | tr | none (ordinals) | suffix | ✅ | — | "." for all |
| | uz-Cyrl-UZ | none (ordinals) | suffix | ✅ | — | "-чи" for all |
| | uz-Latn-UZ | none (ordinals) | suffix | ✅ | — | "-chi" for all |
| **Uralic** | fi | none | suffix | ✅ | — | "." for all |
| | hu | none | suffix | ✅ | — | "." for all |
| **Baltic** | lt | masc/fem | suffix | ✅ Full (M/F/N) | .13 filled | All genders use "." — explicit F/N added |
| | lv | masc/fem | suffix | ✅ Full (M/F/N) | .13 filled | All genders use "." — explicit F/N added |
| **Hellenic** | el | masc/fem/neuter | suffix | ✅ Full (M/F/N) | — | ος/η/ο |
| **Armenian** | hy | none (ordinals) | modulo-suffix | ✅ | — | |
| **Iranian** | fa | none (ordinals) | suffix | ✅ | — | "م" for all |
| | ku | none (ordinals) | suffix | ✅ | — | "ەم" for all |
| **Austronesian** | id | none | template | ✅ | — | |
| | ms | none | template | ✅ | — | |
| | fil | none | template | ✅ | — | |
| **Tai** | th | none | template | ✅ | — | |
| **Koreanic** | ko | none | template | ✅ | — | |
| **Japonic** | ja | none | template | ✅ | — | |
| **Sinitic** | zh-CN | none | (inherits zh-Hans) | ✅ | — | |
| | zh-Hans | none | template | ✅ | — | |
| | zh-Hant | none | template | ✅ | — | |
| **Vietnamese** | vi | none | template | ✅ | — | |
| **Bantu** | zu-ZA | none (ordinals) | suffix | ✅ | — | No suffix (empty string) |

## Category A Gaps (In-Scope Fills)

All gaps below are locales where the suffix engine has only `masculineSuffix` defined but the language has grammatical gender. In every case, the numeric ordinal suffix is gender-invariant (same value for all genders), so the fix is adding explicit `feminineSuffix` and `neuterSuffix` with the same value.

### .12 — Romance + Germanic

| Locale | masculineSuffix | feminineSuffix (add) | neuterSuffix (add) | Reference |
|--------|----------------|---------------------|-------------------|-----------|
| de | '.' | '.' | '.' | DUDEN §793; numeric ordinals are gender-invariant |
| nl | 'e' | 'e' | 'e' | ANS §6.3.3; "1e", "2e" for all genders |
| da | '.' | '.' | '.' | DDO; "1.", "2." for common/neuter |
| nb | '.' | '.' | '.' | Nynorsk/Bokmål standard; "1.", "2." for all genders |
| is | '.' | '.' | '.' | Íslensk málfræði §4.2; "1.", "2." for all genders |
| lb | '.' | '.' | '.' | LOD; "1.", "2." for all genders |

de-CH and de-LI inherit from de — no direct YAML changes needed.

### .13 — Slavic + Baltic

| Locale | masculineSuffix | feminineSuffix (added) | neuterSuffix (added) | Reference |
|--------|----------------|---------------------|-------------------|-----------|
| bg | '.' | '.' | '.' | Bulgarian Academy grammar; numeric ordinals "1.", "2." are gender-invariant |
| cs | '.' | '.' | '.' | Pravidla českého pravopisu; "1.", "2." for all genders |
| hr | '.' | '.' | '.' | Hrvatski pravopis; "1.", "2." for all genders |
| pl | '.' | '.' | '.' | Słownik ortograficzny PWN; "1.", "2." for all genders |
| sk | '.' | '.' | '.' | Pravidlá slovenského pravopisu; "1.", "2." for all genders |
| sl | '.' | '.' | '.' | Slovenski pravopis; "1.", "2." for all genders |
| sr | '.' | '.' | '.' | Pravopis srpskoga jezika; "1.", "2." for all genders |
| sr-Latn | '.' | '.' | '.' | Same as sr (Latin script variant) |
| lt | '.' | '.' | '.' | Lietuvių kalbos rašyba ir skyryba; "1.", "2." for all genders |
| lv | '.' | '.' | '.' | Latviešu valodas pareizrakstības vārdnīca; "1.", "2." for all genders |

### .14 — Semitic/Indic/Other

| Locale | masculineSuffix | feminineSuffix (added) | neuterSuffix (added) | Reference |
|--------|----------------|---------------------|-------------------|-----------|
| ar | '' | '' | '' | Arabic numeric ordinals have no suffix; CLDR Ordinal Rules: Arabic `other` rule only |
| he | '' | '' | '' | Hebrew numeric ordinals have no suffix; CLDR Ordinal Rules: Hebrew `other` rule only |
| mt | '' | '' | '' | Maltese numeric ordinals have no suffix; CLDR Ordinal Rules: Maltese `other` rule only |

Note: Greek (el) already had full M/F/N coverage (ος/η/ο) — no changes needed.
Bengali (bn) and Tamil (ta) have gender-invariant ordinals — no changes needed.
Hindi (hi) is not shipped — out of scope per epic spec.

## Category B Gaps (Research-Bound)

None identified. All shipped locales' numeric ordinals are well-documented in CLDR and standard grammars. No locale requires research beyond one authoritative source.

## Proposer + Reviewer Audit Trail

| Locale | Gendered Term | Proposer Evidence | Reviewer Cross-check |
|--------|---------------|-------------------|---------------------|
| de | feminineSuffix: '.' / neuterSuffix: '.' | DUDEN §793 — numeric ordinals "1., 2., 3." are invariant across der/die/das | CLDR Ordinal Rules: German `other` rule only |
| nl | feminineSuffix: 'e' / neuterSuffix: 'e' | ANS (Algemene Nederlandse Spraakkunst) §6.3.3 — "1e, 2e" for de-words and het-words alike | CLDR: Dutch `other` rule only |
| da | feminineSuffix: '.' / neuterSuffix: '.' | DDO (Den Danske Ordbog) — "1., 2." for both fælleskon and intetkøn | CLDR: Danish `other` rule only |
| nb | feminineSuffix: '.' / neuterSuffix: '.' | Nynorskordlista / Bokmålsordlista — "1., 2." standard notation | CLDR: Norwegian Bokmål `other` rule only |
| is | feminineSuffix: '.' / neuterSuffix: '.' | Íslensk málfræði — "1., 2." dot notation is gender-invariant | CLDR: Icelandic `other` rule only |
| lb | feminineSuffix: '.' / neuterSuffix: '.' | LOD (Lëtzebuerger Online Dictionnaire) — "1., 2." notation | CLDR: Luxembourgish `other` rule only |
| bg | feminineSuffix: '.' / neuterSuffix: '.' | Bulgarian Academy grammar — numeric ordinals "1., 2." are gender-invariant | CLDR: Bulgarian `other` rule only |
| cs | feminineSuffix: '.' / neuterSuffix: '.' | Pravidla českého pravopisu — "1., 2." for all genders | CLDR: Czech `other` rule only |
| hr | feminineSuffix: '.' / neuterSuffix: '.' | Hrvatski pravopis — "1., 2." for masculine/feminine | CLDR: Croatian `other` rule only |
| pl | feminineSuffix: '.' / neuterSuffix: '.' | Słownik ortograficzny PWN — "1., 2." for all genders | CLDR: Polish `other` rule only |
| sk | feminineSuffix: '.' / neuterSuffix: '.' | Pravidlá slovenského pravopisu — "1., 2." for all genders | CLDR: Slovak `other` rule only |
| sl | feminineSuffix: '.' / neuterSuffix: '.' | Slovenski pravopis — "1., 2." for all genders | CLDR: Slovenian `other` rule only |
| sr | feminineSuffix: '.' / neuterSuffix: '.' | Pravopis srpskoga jezika — "1., 2." for all genders | CLDR: Serbian `other` rule only |
| sr-Latn | feminineSuffix: '.' / neuterSuffix: '.' | Same as sr (Latin script variant of Serbian) | CLDR: Serbian `other` rule only |
| lt | feminineSuffix: '.' / neuterSuffix: '.' | Lietuvių kalbos rašyba ir skyryba — "1., 2." for masculine/feminine | CLDR: Lithuanian `other` rule only |
| lv | feminineSuffix: '.' / neuterSuffix: '.' | Latviešu valodas pareizrakstības vārdnīca — "1., 2." for masculine/feminine | CLDR: Latvian `other` rule only |
| ar | feminineSuffix: '' / neuterSuffix: '' | Arabic numeric ordinals are written as bare digits without suffix; no gendered ordinal markers in standard Arabic numeral notation | CLDR: Arabic `other` rule only; no ordinal gender distinction in digit form |
| he | feminineSuffix: '' / neuterSuffix: '' | Hebrew numeric ordinals are written as bare digits without suffix; no gendered ordinal markers in standard Hebrew numeral notation | CLDR: Hebrew `other` rule only; no ordinal gender distinction in digit form |
| mt | feminineSuffix: '' / neuterSuffix: '' | Maltese numeric ordinals are written as bare digits without suffix; no gendered ordinal markers in standard Maltese numeral notation | CLDR: Maltese `other` rule only; no ordinal gender distinction in digit form |

## Backward Compatibility

All non-gendered locales (en, tr, ja, zh, ko, fi, hu, id, ms, fil, th, vi, af, az, uz, fa, ku, hy, zu-ZA, bn, ta) continue to emit identical output. Adding explicit feminine/neuter suffixes with the same value as masculine changes no runtime behavior — it only makes the YAML self-documenting instead of relying on engine fallback.
