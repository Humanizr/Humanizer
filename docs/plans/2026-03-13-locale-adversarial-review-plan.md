# Plan: Locale-by-Locale Adversarial Translation Review

**Generated**: 2026-03-13

## Overview
Create exhaustive native-standard translation review artifacts for every localized resource file under `src/Humanizer/Properties/Resources.<locale>.resx`. This is a report-only pass. Every locale task must review all translated keys, propose replacements for every non-`ok` item, and emit a per-locale review JSON file. Child-culture fallback review is owned by the parent locale task.

## Prerequisites
- `tools/Initialize-LocaleReviewArtifacts.ps1`
- `tools/Build-LocaleReviewReport.ps1`
- `docs/reviews/locales/*.review.json`

## Dependency Graph

```text
T1 -> T2 -> T3..T53 -> T54 -> T55
```

## Tasks

### T1: Initialize review inventory and scaffolding
- **depends_on**: []
- **location**: `tools/Initialize-LocaleReviewArtifacts.ps1`, `docs/reviews/**/*`
- **description**: Generate per-locale review JSON files with one record per translated key, plus aggregate review placeholders.
- **validation**: `docs/reviews/locales/*.review.json` exists for every locale resource file.
- **status**: Completed
- **log**: Added initializer tooling that generates one review JSON per locale, a machine-readable aggregate manifest, and a markdown placeholder report. The generated scaffold now covers all 51 localized resource files and establishes one record per translated key.
- **files edited/created**: `tools/Initialize-LocaleReviewArtifacts.ps1`, `docs/reviews/2026-03-13-locale-adversarial-review.json`, `docs/reviews/2026-03-13-locale-adversarial-review.md`, `docs/reviews/locales/*.review.json`

### T2: Freeze review rubric in the plan and scaffolding
- **depends_on**: [T1]
- **location**: `docs/plans/2026-03-13-locale-adversarial-review-plan.md`, `docs/reviews/**/*`
- **description**: Lock the native-speaker review standard, required JSON fields, and parent-child fallback ownership before locale review begins.
- **validation**: Locale reviewers can work from the generated JSON and this plan without inventing fields or scope.
- **status**: Completed
- **log**: Added the execution plan and aggregate report builder, locking the native-speaker review standard, required JSON fields, parent-child fallback ownership, and final aggregation path before locale review begins.
- **files edited/created**: `docs/plans/2026-03-13-locale-adversarial-review-plan.md`, `tools/Build-LocaleReviewReport.ps1`

### T3: Review af locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.af.resx`, `tests/Humanizer.Tests/Localisation/af/**/*`, `docs/reviews/locales/af.review.json`
- **description**: Review every translated key for `af` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/af.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T4: Review ar locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.ar.resx`, `tests/Humanizer.Tests/Localisation/ar/**/*`, `docs/reviews/locales/ar.review.json`
- **description**: Review every translated key for `ar` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/ar.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T5: Review az locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.az.resx`, `tests/Humanizer.Tests/Localisation/az/**/*`, `docs/reviews/locales/az.review.json`
- **description**: Review every translated key for `az` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/az.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T6: Review bg locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.bg.resx`, `tests/Humanizer.Tests/Localisation/bg/**/*`, `docs/reviews/locales/bg.review.json`
- **description**: Review every translated key for `bg` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/bg.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T7: Review bn locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.bn.resx`, `tests/Humanizer.Tests/Localisation/bn-BD/**/*`, `docs/reviews/locales/bn.review.json`
- **description**: Review every translated key for `bn` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/bn.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T8: Review ca locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.ca.resx`, `tests/Humanizer.Tests/Localisation/ca/**/*`, `docs/reviews/locales/ca.review.json`
- **description**: Review every translated key for `ca` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/ca.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T9: Review cs locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.cs.resx`, `tests/Humanizer.Tests/Localisation/cs/**/*`, `docs/reviews/locales/cs.review.json`
- **description**: Review every translated key for `cs` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/cs.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T10: Review da locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.da.resx`, `tests/Humanizer.Tests/Localisation/da/**/*`, `docs/reviews/locales/da.review.json`
- **description**: Review every translated key for `da` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/da.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T11: Review de locale and child cultures
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.de.resx`, `tests/Humanizer.Tests/Localisation/de/**/*`, `tests/Humanizer.Tests/Localisation/de-CH/**/*`, `tests/Humanizer.Tests/Localisation/de-LI/**/*`, `docs/reviews/locales/de.review.json`
- **description**: Review every translated key for `de` and owned child-culture fallback usage.
- **validation**: `docs/reviews/locales/de.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T12: Review el locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.el.resx`, `tests/Humanizer.Tests/Localisation/el/**/*`, `docs/reviews/locales/el.review.json`
- **description**: Review every translated key for `el` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/el.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T13: Review es locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.es.resx`, `tests/Humanizer.Tests/Localisation/es/**/*`, `docs/reviews/locales/es.review.json`
- **description**: Review every translated key for `es` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/es.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T14: Review fa locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.fa.resx`, `tests/Humanizer.Tests/Localisation/fa/**/*`, `docs/reviews/locales/fa.review.json`
- **description**: Review every translated key for `fa` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/fa.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T15: Review fi locale and child cultures
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.fi.resx`, `tests/Humanizer.Tests/Localisation/fi-FI/**/*`, `docs/reviews/locales/fi.review.json`
- **description**: Review every translated key for `fi` and owned child-culture fallback usage.
- **validation**: `docs/reviews/locales/fi.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T16: Review fil locale and child cultures
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.fil.resx`, `tests/Humanizer.Tests/Localisation/fil-PH/**/*`, `docs/reviews/locales/fil.review.json`
- **description**: Review every translated key for `fil` and owned child-culture fallback usage.
- **validation**: `docs/reviews/locales/fil.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T17: Review fr locale and child cultures
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.fr.resx`, `tests/Humanizer.Tests/Localisation/fr/**/*`, `tests/Humanizer.Tests/Localisation/fr-BE/**/*`, `tests/Humanizer.Tests/Localisation/fr-CH/**/*`, `docs/reviews/locales/fr.review.json`
- **description**: Review every translated key for `fr` and owned child-culture fallback usage.
- **validation**: `docs/reviews/locales/fr.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T18: Review he locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.he.resx`, `tests/Humanizer.Tests/Localisation/he/**/*`, `docs/reviews/locales/he.review.json`
- **description**: Review every translated key for `he` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/he.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T19: Review hr locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.hr.resx`, `tests/Humanizer.Tests/Localisation/hr/**/*`, `docs/reviews/locales/hr.review.json`
- **description**: Review every translated key for `hr` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/hr.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T20: Review hu locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.hu.resx`, `tests/Humanizer.Tests/Localisation/hu/**/*`, `docs/reviews/locales/hu.review.json`
- **description**: Review every translated key for `hu` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/hu.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T21: Review hy locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.hy.resx`, `tests/Humanizer.Tests/Localisation/hy/**/*`, `docs/reviews/locales/hy.review.json`
- **description**: Review every translated key for `hy` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/hy.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T22: Review id locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.id.resx`, `tests/Humanizer.Tests/Localisation/id/**/*`, `docs/reviews/locales/id.review.json`
- **description**: Review every translated key for `id` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/id.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T23: Review is locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.is.resx`, `tests/Humanizer.Tests/Localisation/is/**/*`, `docs/reviews/locales/is.review.json`
- **description**: Review every translated key for `is` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/is.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T24: Review it locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.it.resx`, `tests/Humanizer.Tests/Localisation/it/**/*`, `docs/reviews/locales/it.review.json`
- **description**: Review every translated key for `it` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/it.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T25: Review ja locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.ja.resx`, `tests/Humanizer.Tests/Localisation/ja/**/*`, `docs/reviews/locales/ja.review.json`
- **description**: Review every translated key for `ja` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/ja.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T26: Review ko locale and child cultures
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.ko.resx`, `tests/Humanizer.Tests/Localisation/ko-KR/**/*`, `docs/reviews/locales/ko.review.json`
- **description**: Review every translated key for `ko` and owned child-culture fallback usage.
- **validation**: `docs/reviews/locales/ko.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T27: Review ku locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.ku.resx`, `tests/Humanizer.Tests/Localisation/ku/**/*`, `docs/reviews/locales/ku.review.json`
- **description**: Review every translated key for `ku` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/ku.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T28: Review lb locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.lb.resx`, `tests/Humanizer.Tests/Localisation/lb/**/*`, `docs/reviews/locales/lb.review.json`
- **description**: Review every translated key for `lb` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/lb.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T29: Review lt locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.lt.resx`, `tests/Humanizer.Tests/Localisation/lt/**/*`, `docs/reviews/locales/lt.review.json`
- **description**: Review every translated key for `lt` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/lt.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T30: Review lv locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.lv.resx`, `tests/Humanizer.Tests/Localisation/lv/**/*`, `docs/reviews/locales/lv.review.json`
- **description**: Review every translated key for `lv` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/lv.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T31: Review ms locale and child cultures
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.ms.resx`, `tests/Humanizer.Tests/Localisation/ms-MY/**/*`, `docs/reviews/locales/ms.review.json`
- **description**: Review every translated key for `ms` and owned child-culture fallback usage.
- **validation**: `docs/reviews/locales/ms.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T32: Review mt locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.mt.resx`, `tests/Humanizer.Tests/Localisation/mt/**/*`, `docs/reviews/locales/mt.review.json`
- **description**: Review every translated key for `mt` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/mt.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T33: Review nb locale and child cultures
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.nb.resx`, `tests/Humanizer.Tests/Localisation/nb/**/*`, `tests/Humanizer.Tests/Localisation/nb-NO/**/*`, `docs/reviews/locales/nb.review.json`
- **description**: Review every translated key for `nb` and owned child-culture fallback usage.
- **validation**: `docs/reviews/locales/nb.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T34: Review nl locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.nl.resx`, `tests/Humanizer.Tests/Localisation/nl/**/*`, `docs/reviews/locales/nl.review.json`
- **description**: Review every translated key for `nl` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/nl.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T35: Review pl locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.pl.resx`, `tests/Humanizer.Tests/Localisation/pl/**/*`, `docs/reviews/locales/pl.review.json`
- **description**: Review every translated key for `pl` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/pl.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T36: Review pt locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.pt.resx`, `tests/Humanizer.Tests/Localisation/pt/**/*`, `docs/reviews/locales/pt.review.json`
- **description**: Review every translated key for `pt` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/pt.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T37: Review pt-BR locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.pt-BR.resx`, `tests/Humanizer.Tests/Localisation/pt-BR/**/*`, `docs/reviews/locales/pt-BR.review.json`
- **description**: Review every translated key for `pt-BR` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/pt-BR.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T38: Review ro locale and child cultures
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.ro.resx`, `tests/Humanizer.Tests/Localisation/ro-Ro/**/*`, `docs/reviews/locales/ro.review.json`
- **description**: Review every translated key for `ro` and owned child-culture fallback usage.
- **validation**: `docs/reviews/locales/ro.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T39: Review ru locale and child cultures
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.ru.resx`, `tests/Humanizer.Tests/Localisation/ru-RU/**/*`, `docs/reviews/locales/ru.review.json`
- **description**: Review every translated key for `ru` and owned child-culture fallback usage.
- **validation**: `docs/reviews/locales/ru.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T40: Review sk locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.sk.resx`, `tests/Humanizer.Tests/Localisation/sk/**/*`, `docs/reviews/locales/sk.review.json`
- **description**: Review every translated key for `sk` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/sk.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T41: Review sl locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.sl.resx`, `tests/Humanizer.Tests/Localisation/sl/**/*`, `docs/reviews/locales/sl.review.json`
- **description**: Review every translated key for `sl` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/sl.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T42: Review sr locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.sr.resx`, `tests/Humanizer.Tests/Localisation/sr/**/*`, `docs/reviews/locales/sr.review.json`
- **description**: Review every translated key for `sr` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/sr.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T43: Review sr-Latn locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.sr-Latn.resx`, `tests/Humanizer.Tests/Localisation/sr-Latn/**/*`, `docs/reviews/locales/sr-Latn.review.json`
- **description**: Review every translated key for `sr-Latn` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/sr-Latn.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T44: Review sv locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.sv.resx`, `tests/Humanizer.Tests/Localisation/sv/**/*`, `docs/reviews/locales/sv.review.json`
- **description**: Review every translated key for `sv` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/sv.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T45: Review th locale and child cultures
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.th.resx`, `tests/Humanizer.Tests/Localisation/th-TH/**/*`, `docs/reviews/locales/th.review.json`
- **description**: Review every translated key for `th` and owned child-culture fallback usage.
- **validation**: `docs/reviews/locales/th.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T46: Review tr locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.tr.resx`, `tests/Humanizer.Tests/Localisation/tr/**/*`, `docs/reviews/locales/tr.review.json`
- **description**: Review every translated key for `tr` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/tr.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T47: Review uk locale and child cultures
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.uk.resx`, `tests/Humanizer.Tests/Localisation/uk-UA/**/*`, `docs/reviews/locales/uk.review.json`
- **description**: Review every translated key for `uk` and owned child-culture fallback usage.
- **validation**: `docs/reviews/locales/uk.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T48: Review uz-Cyrl-UZ locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.uz-Cyrl-UZ.resx`, `tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/**/*`, `docs/reviews/locales/uz-Cyrl-UZ.review.json`
- **description**: Review every translated key for `uz-Cyrl-UZ` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/uz-Cyrl-UZ.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T49: Review uz-Latn-UZ locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.uz-Latn-UZ.resx`, `tests/Humanizer.Tests/Localisation/uz-Latn-UZ/**/*`, `docs/reviews/locales/uz-Latn-UZ.review.json`
- **description**: Review every translated key for `uz-Latn-UZ` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/uz-Latn-UZ.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T50: Review vi locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.vi.resx`, `tests/Humanizer.Tests/Localisation/vi/**/*`, `docs/reviews/locales/vi.review.json`
- **description**: Review every translated key for `vi` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/vi.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T51: Review zh-CN locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.zh-CN.resx`, `tests/Humanizer.Tests/Localisation/zh-CN/**/*`, `docs/reviews/locales/zh-CN.review.json`
- **description**: Review every translated key for `zh-CN` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/zh-CN.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T52: Review zh-Hans locale
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.zh-Hans.resx`, `tests/Humanizer.Tests/Localisation/zh-Hans/**/*`, `docs/reviews/locales/zh-Hans.review.json`
- **description**: Review every translated key for `zh-Hans` and update its review JSON with final dispositions and proposed replacements.
- **validation**: `docs/reviews/locales/zh-Hans.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T53: Review zh-Hant locale and child cultures
- **depends_on**: [T1, T2]
- **location**: `src/Humanizer/Properties/Resources.zh-Hant.resx`, `tests/Humanizer.Tests/Localisation/zh-Hant/**/*`, `tests/Humanizer.Tests/Localisation/zh-HK/**/*`, `docs/reviews/locales/zh-Hant.review.json`
- **description**: Review every translated key for `zh-Hant` and owned child-culture fallback usage.
- **validation**: `docs/reviews/locales/zh-Hant.review.json` has no pending records.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T54: Cross-locale adversarial consistency pass
- **depends_on**: [T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24, T25, T26, T27, T28, T29, T30, T31, T32, T33, T34, T35, T36, T37, T38, T39, T40, T41, T42, T43, T44, T45, T46, T47, T48, T49, T50, T51, T52, T53]
- **location**: `docs/reviews/locales/*.review.json`
- **description**: Review all locale outputs for duplicated machine-translation artifacts, inconsistent unit and heading conventions, suspicious English leakage, and replacement inconsistencies across related locales.
- **validation**: Cross-locale issues are recorded in the final aggregate artifacts.
- **status**: Not Completed
- **log**:
- **files edited/created**:

### T55: Aggregate final review report
- **depends_on**: [T54]
- **location**: `tools/Build-LocaleReviewReport.ps1`, `docs/reviews/2026-03-13-locale-adversarial-review.md`, `docs/reviews/2026-03-13-locale-adversarial-review.json`
- **description**: Build the final markdown and JSON reports from all per-locale review JSON files.
- **validation**: Aggregate reports exist and counts match per-locale artifacts.
- **status**: Not Completed
- **log**:
- **files edited/created**:

## Parallel Execution Groups

| Wave | Tasks | Can Start When |
|------|-------|----------------|
| 1 | T1 | Immediately |
| 2 | T2 | T1 complete |
| 3 | T3-T53 | T1, T2 complete |
| 4 | T54 | T3-T53 complete |
| 5 | T55 | T54 complete |
