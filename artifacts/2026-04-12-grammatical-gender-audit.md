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
| **Germanic** | de | masc/fem/neuter | suffix | ⚠️ masculineSuffix only | .12 (A) | All genders use "." — add explicit F/N |
| | de-CH | masc/fem/neuter | (inherits de) | ⚠️ via inheritance | .12 (A) | Inherits de gap |
| | de-LI | masc/fem/neuter | (inherits de) | ⚠️ via inheritance | .12 (A) | Inherits de gap |
| | nl | common/neuter | suffix | ⚠️ masculineSuffix only | .12 (A) | All genders use "e" — add explicit F/N |
| | sv | common/neuter | modulo-suffix | ✅ No gender keys | — | ":e"/":a" by digit, not gender |
| | da | common/neuter | suffix | ⚠️ masculineSuffix only | .12 (A) | All genders use "." — add explicit F/N |
| | nb | masc/fem/neuter | suffix | ⚠️ masculineSuffix only | .12 (A) | All genders use "." — add explicit F/N |
| | nn | masc/fem/neuter | (no ordinal section) | ✅ via inheritance | — | Inherits nb/default |
| | is | masc/fem/neuter | suffix | ⚠️ masculineSuffix only | .12 (A) | All genders use "." — add explicit F/N |
| | lb | masc/fem/neuter | suffix | ⚠️ masculineSuffix only | .12 (A) | All genders use "." — add explicit F/N |
| | af | none (modern) | modulo-suffix | ✅ No gender keys | — | Afrikaans lost grammatical gender |
| | en | none | modulo-suffix | ✅ No gender keys | — | th/st/nd/rd by digit |
| | en-GB | none | (inherits en) | ✅ | — | |
| | en-IN | none | (inherits en) | ✅ | — | |
| | en-US | none | (overrides en) | ✅ | — | Uses en ordinal pattern |
| **Slavic** | bg | masc/fem/neuter | suffix | ⚠️ masculineSuffix only | .13 | "." for all genders |
| | cs | masc/fem/neuter | suffix | ⚠️ masculineSuffix only | .13 | "." for all genders |
| | hr | masc/fem | suffix | ⚠️ masculineSuffix only | .13 | "." for all genders |
| | pl | masc/fem/neuter | suffix | ⚠️ masculineSuffix only | .13 | "." for all genders |
| | ru | masc/fem/neuter | suffix | ✅ Full (M/F/N) | — | -й/-я/-е |
| | sk | masc/fem/neuter | suffix | ⚠️ masculineSuffix only | .13 | "." for all genders |
| | sl | masc/fem/neuter | suffix | ⚠️ masculineSuffix only | .13 | "." for all genders |
| | sr | masc/fem/neuter | suffix | ⚠️ masculineSuffix only | .13 | "." for all genders |
| | sr-Latn | masc/fem/neuter | suffix | ⚠️ masculineSuffix only | .13 | "." for all genders |
| | uk | masc/fem/neuter | template | ✅ Full (M/F/N) | — | Per-gender template sections |
| **Semitic** | ar | masc/fem | suffix | ⚠️ masculineSuffix only | .14 | No suffix (empty string) |
| | he | masc/fem | suffix | ⚠️ masculineSuffix only | .14 | No suffix (empty string) |
| | mt | masc/fem | suffix | ⚠️ masculineSuffix only | .14 | No suffix (empty string) |
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
| | et | — | — | — | — | Not shipped |
| **Baltic** | lt | masc/fem | suffix | ⚠️ masculineSuffix only | .13 | "." for all |
| | lv | masc/fem | suffix | ⚠️ masculineSuffix only | .13 | "." for all |
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

### .13 — Slavic (separate task)

bg, cs, hr, pl, sk, sl, sr, sr-Latn, lt, lv — all use "." suffix for all genders.

### .14 — Semitic/Indic/Other (separate task)

ar, he, mt — all use empty-string suffix for all genders.

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

## Backward Compatibility

All non-gendered locales (en, tr, ja, zh, ko, fi, hu, et, lv, id, ms, fil, th, vi, af, az, uz, fa, ku, hy, zu-ZA, bn, ta) continue to emit identical output. Adding explicit feminine/neuter suffixes with the same value as masculine changes no runtime behavior — it only makes the YAML self-documenting instead of relying on engine fallback.
