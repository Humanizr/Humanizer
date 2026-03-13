# Locale Translation Completion Implementation Plan

> **For Codex:** Execute in parallel by locale batch after the support matrix is locked. Do not expand to neutral `.resx` parity; fill only intended localized surfaces and validate wording for idiom/correctness.

**Goal:** Complete missing translations on Humanizer's already-supported locale/capability surfaces, validate existing wording for modern idiom/correctness, and add regression tests so supported locales stop silently falling back to English.

**Architecture:** First derive a repo-grounded support matrix from registries, direct resource consumers, and existing locale tests. Then update locale resources and any required localization wiring in parallel by locale batch, followed by targeted locale regression tests and full framework verification.

**Tech Stack:** C#, .resx resources, xUnit v3, .NET 10 SDK, Humanizer localization registries and locale-specific tests.

---

## Tasks

### T1: Lock the support matrix and fallback gaps

**Files:**
- Modify: `src/Humanizer/Properties/Resources.*.resx`
- Modify: `src/Humanizer/Configuration/*.cs`
- Modify: `src/Humanizer/Localisation/**/*.cs`
- Modify: `tests/Humanizer.Tests/Localisation/**/*`

- **depends_on**: `[]`
- **description**: Audit the repo to decide, per locale, which capabilities are intended to be localized today. Use registries, direct resource consumers, and existing locale tests to separate intentional inheritance from actual missing localization. Produce the working locale batch list and identify any shared wiring defects that must be fixed outside locale files.
- **validation**: A written matrix in the task log naming locale batches, supported surfaces per batch, and any shared-code fixes required before or alongside locale work.
- **status**: Completed
- **log**: Locked the support matrix from registry registrations, resource-family presence, and locale test folders. Confirmed that most locales intentionally localize `DateHumanize` and `TimeSpanHumanize`, while only a smaller subset currently intends `TimeUnit`, `DataUnit`, `Heading`, `CollectionFormatter`, `ClockNotation`, and `Bytes` surfaces. No broad shared wiring defect is required up front; the main expected shared work is regression guardrails if locale batches expose silent fallback paths.
- **files edited/created**: `docs/plans/2026-03-12-locale-translation-completion-plan.md`

### T2: Germanic and Nordic locale batch

**Files:**
- Modify: `src/Humanizer/Properties/Resources.af.resx`
- Modify: `src/Humanizer/Properties/Resources.da.resx`
- Modify: `src/Humanizer/Properties/Resources.de.resx`
- Modify: `src/Humanizer/Properties/Resources.fi.resx`
- Modify: `src/Humanizer/Properties/Resources.is.resx`
- Modify: `src/Humanizer/Properties/Resources.nb.resx`
- Modify: `src/Humanizer/Properties/Resources.nl.resx`
- Modify: `src/Humanizer/Properties/Resources.sv.resx`
- Modify: `tests/Humanizer.Tests/Localisation/{af,da,de,fi-FI,is,nb,nb-NO,nl,sv}/**/*`

- **depends_on**: `[T1]`
- **description**: Audit and correct the Germanic/Nordic locales for missing intended translations and outdated wording. Add missing resource keys only for intended localized surfaces, modernize idioms where needed, and add or expand tests covering each touched capability.
- **validation**: Touched locale tests fail before the fix, pass after the fix, and include regression assertions proving those locales no longer fall back to English on the supported surface that was corrected.
- **status**: Completed
- **log**: Filled missing week, millisecond, and `Never` `DateHumanize` entries for Afrikaans, Danish, German, Norwegian Bokmal, Dutch, and Swedish so these cultures stop falling back to English on supported date surfaces. Corrected the Swedish singular hour wording, fixed the Danish collection formatter registration from `dk` to `da`, and added regression coverage for Danish headings and collection formatting plus the Swedish time-span wording change. Extended the shared `DateHumanize` test helper to generate week deltas so the new locale week assertions exercise the intended resources.
- **files edited/created**: `src/Humanizer/Configuration/CollectionFormatterRegistry.cs`, `src/Humanizer/Properties/Resources.af.resx`, `src/Humanizer/Properties/Resources.da.resx`, `src/Humanizer/Properties/Resources.de.resx`, `src/Humanizer/Properties/Resources.nb.resx`, `src/Humanizer/Properties/Resources.nl.resx`, `src/Humanizer/Properties/Resources.sv.resx`, `tests/Humanizer.Tests/DateHumanize.cs`, `tests/Humanizer.Tests/Localisation/da/CollectionFormatterTests.cs`, `tests/Humanizer.Tests/Localisation/da/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/sv/TimeSpanHumanizeTests.cs`

### T3: Romance locale batch

**Files:**
- Modify: `src/Humanizer/Properties/Resources.ca.resx`
- Modify: `src/Humanizer/Properties/Resources.es.resx`
- Modify: `src/Humanizer/Properties/Resources.fr.resx`
- Modify: `src/Humanizer/Properties/Resources.it.resx`
- Modify: `src/Humanizer/Properties/Resources.pt.resx`
- Modify: `src/Humanizer/Properties/Resources.pt-BR.resx`
- Modify: `src/Humanizer/Properties/Resources.ro.resx`
- Modify: `tests/Humanizer.Tests/Localisation/{ca,es,fr,fr-BE,fr-CH,it,pt,pt-BR,ro-Ro}/**/*`

- **depends_on**: `[T1]`
- **description**: Audit and correct the Romance locales for missing intended translations and outdated wording. Cover date/time humanization, byte/unit strings, ordinals, date-to-ordinal wording, collection formatting, clock notation, and any other supported surface in the batch.
- **validation**: Touched locale tests fail before the fix, pass after the fix, and include regression assertions proving those locales no longer fall back to English on the supported surface that was corrected.
- **status**: Completed
- **log**: Added regression coverage for already-supported Romance surfaces that previously had no direct protection: Catalan and Spanish headings, `TimeUnit` symbols, and full-word byte strings; Portuguese and Brazilian Portuguese headings and byte wording. Corrected the Spanish `DataUnit_Gigabyte` typo so Spanish full-word byte output no longer emits the misspelled fallback string. Tightened the new heading parser assertions to pass an explicit culture so the locale regressions are deterministic under the parallel xUnit runner.
- **files edited/created**: `src/Humanizer/Properties/Resources.es.resx`, `tests/Humanizer.Tests/Localisation/ca/Bytes/ToFullWordsTests.cs`, `tests/Humanizer.Tests/Localisation/ca/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/ca/TimeUnitToSymbolTests.cs`, `tests/Humanizer.Tests/Localisation/es/Bytes/ToFullWordsTests.cs`, `tests/Humanizer.Tests/Localisation/es/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/es/TimeUnitToSymbolTests.cs`, `tests/Humanizer.Tests/Localisation/pt/Bytes/ToFullWordsTests.cs`, `tests/Humanizer.Tests/Localisation/pt/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/pt-BR/Bytes/ToFullWordsTests.cs`, `tests/Humanizer.Tests/Localisation/pt-BR/HeadingTests.cs`

### T4: Slavic, Baltic, and Balkan locale batch

**Files:**
- Modify: `src/Humanizer/Properties/Resources.bg.resx`
- Modify: `src/Humanizer/Properties/Resources.cs.resx`
- Modify: `src/Humanizer/Properties/Resources.hr.resx`
- Modify: `src/Humanizer/Properties/Resources.lb.resx`
- Modify: `src/Humanizer/Properties/Resources.lt.resx`
- Modify: `src/Humanizer/Properties/Resources.lv.resx`
- Modify: `src/Humanizer/Properties/Resources.pl.resx`
- Modify: `src/Humanizer/Properties/Resources.ru.resx`
- Modify: `src/Humanizer/Properties/Resources.sk.resx`
- Modify: `src/Humanizer/Properties/Resources.sl.resx`
- Modify: `src/Humanizer/Properties/Resources.sr.resx`
- Modify: `src/Humanizer/Properties/Resources.sr-Latn.resx`
- Modify: `src/Humanizer/Properties/Resources.uk.resx`
- Modify: `tests/Humanizer.Tests/Localisation/{bg,cs,hr,lb,lt,lv,pl,ru-RU,sk,sl,sr,sr-Latn,uk-UA}/**/*`

- **depends_on**: `[T1]`
- **description**: Audit and correct the Slavic/Baltic/Balkan locales for missing intended translations and morphology-sensitive wording. Keep formatter and ordinalizer logic aligned with resource keys; update shared locale code only when a resource-only change cannot satisfy the supported behavior.
- **validation**: Touched locale tests fail before the fix, pass after the fix, and include regression assertions proving those locales no longer fall back to English on the supported surface that was corrected.
- **status**: Completed
- **log**: Fixed Serbian hour/second morphology for values ending in `2-4` versus the `12-14` exceptions by correcting the formatter’s paucal selection logic and adding regression coverage in both Cyrillic and Latin scripts. These tests now prove the supported Serbian date/time humanization surfaces stay on the Serbian resources instead of silently falling back to English-like forms.
- **files edited/created**: `src/Humanizer/Localisation/Formatters/SerbianFormatter.cs`, `tests/Humanizer.Tests/Localisation/sr/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/sr/TimeSpanHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/sr-Latn/DateHumanizeTests.cs`, `tests/Humanizer.Tests/Localisation/sr-Latn/TimeSpanHumanizeTests.cs`

### T5: RTL, Asian, and other remaining locale batch

**Files:**
- Modify: `src/Humanizer/Properties/Resources.ar.resx`
- Modify: `src/Humanizer/Properties/Resources.az.resx`
- Modify: `src/Humanizer/Properties/Resources.bn.resx`
- Modify: `src/Humanizer/Properties/Resources.el.resx`
- Modify: `src/Humanizer/Properties/Resources.fa.resx`
- Modify: `src/Humanizer/Properties/Resources.fil.resx`
- Modify: `src/Humanizer/Properties/Resources.he.resx`
- Modify: `src/Humanizer/Properties/Resources.hu.resx`
- Modify: `src/Humanizer/Properties/Resources.hy.resx`
- Modify: `src/Humanizer/Properties/Resources.id.resx`
- Modify: `src/Humanizer/Properties/Resources.ja.resx`
- Modify: `src/Humanizer/Properties/Resources.ko.resx`
- Modify: `src/Humanizer/Properties/Resources.ku.resx`
- Modify: `src/Humanizer/Properties/Resources.ms.resx`
- Modify: `src/Humanizer/Properties/Resources.mt.resx`
- Modify: `src/Humanizer/Properties/Resources.ta.resx`
- Modify: `src/Humanizer/Properties/Resources.th.resx`
- Modify: `src/Humanizer/Properties/Resources.tr.resx`
- Modify: `src/Humanizer/Properties/Resources.uz-Latn-UZ.resx`
- Modify: `src/Humanizer/Properties/Resources.uz-Cyrl-UZ.resx`
- Modify: `src/Humanizer/Properties/Resources.vi.resx`
- Modify: `src/Humanizer/Properties/Resources.zh-CN.resx`
- Modify: `src/Humanizer/Properties/Resources.zh-Hans.resx`
- Modify: `src/Humanizer/Properties/Resources.zh-Hant.resx`
- Modify: `tests/Humanizer.Tests/Localisation/{ar,az,bn-BD,el,fa,fil-PH,he,hu,hy,id,ja,ko-KR,ku,ms-MY,mt,ta,th-TH,tr,uz-Latn-UZ,uz-Cyrl-UZ,vi,zh-CN,zh-Hans,zh-Hant,zh-HK}/**/*`

- **depends_on**: `[T1]`
- **description**: Audit and correct the remaining locales for missing intended translations and outdated wording. Include RTL-specific phrasing checks, scripts/orthography validation, and any supported number-to-words or ordinal logic already present in the repo.
- **validation**: Touched locale tests fail before the fix, pass after the fix, and include regression assertions proving those locales no longer fall back to English on the supported surface that was corrected.
- **status**: Completed
- **log**: Filled the missing Hungarian `TimeUnit_Millisecond` symbol and added regression coverage for Hungarian headings and `TimeUnit` symbols. Corrected Maltese heading resources by separating the full `ESE` phrase from its abbreviated symbol and added heading coverage for that locale. The heading parser regressions also now assert an explicit culture for locale-specific digraph abbreviations.
- **files edited/created**: `src/Humanizer/Properties/Resources.hu.resx`, `src/Humanizer/Properties/Resources.mt.resx`, `tests/Humanizer.Tests/Localisation/hu/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/hu/TimeUnitToSymbolTests.cs`, `tests/Humanizer.Tests/Localisation/mt/HeadingTests.cs`

### T6: Shared localization wiring and regression guardrails

**Files:**
- Modify: `src/Humanizer/Configuration/*.cs`
- Modify: `src/Humanizer/Localisation/**/*.cs`
- Modify: `tests/Humanizer.Tests/Localisation/**/*.cs`
- Modify: `tests/Humanizer.Tests/ResourceKeyTests.cs`

- **depends_on**: `[T1]`
- **description**: Fix any shared wiring defects discovered in the audit, such as missing registry registrations, wrong resource-key selection, or missing test helpers. Add shared regression guardrails only where they prevent future silent English fallback without forcing unsupported locales into parity.
- **validation**: New or updated shared tests fail before the fix, pass after the fix, and any shared code change is exercised by at least one locale regression.
- **status**: Completed
- **log**: Applied two shared localization wiring fixes uncovered by the locale audit: the Danish collection formatter registry now registers the actual `da` culture, and heading parsing now compares localized abbreviations case-insensitively instead of uppercasing input through culture-specific casing rules. The shared week support in the `DateHumanize` test helper lets locale regressions verify week strings directly, and the new locale heading/collection tests exercise the shared paths end to end.
- **files edited/created**: `src/Humanizer/Configuration/CollectionFormatterRegistry.cs`, `src/Humanizer/HeadingExtensions.cs`, `tests/Humanizer.Tests/DateHumanize.cs`, `tests/Humanizer.Tests/Localisation/da/CollectionFormatterTests.cs`, `tests/Humanizer.Tests/Localisation/ca/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/es/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/hu/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/pt/HeadingTests.cs`, `tests/Humanizer.Tests/Localisation/pt-BR/HeadingTests.cs`

### T7: Full verification

**Files:**
- Modify: `readme.md`

- **depends_on**: `[T2, T3, T4, T5, T6]`
- **description**: Run the required test and build verification for the combined changes, review any remaining fallbacks for intentional inheritance, and update documentation only if supported locale behavior changed materially.
- **validation**: `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0`, `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0`, and `dotnet build src/Humanizer/Humanizer.csproj -c Release /t:PackNuSpecs /p:PackageOutputPath=<temp path>` all succeed.
- **status**: Completed
- **log**: Verified the combined locale batch on the required targets. `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0` passed with 15,772/15,772 tests. `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0` passed with 15,772/15,772 tests. `dotnet build Humanizer/Humanizer.csproj -c Release /t:PackNuSpecs /p:PackageOutputPath=E:\Dev\Humanizer-locale\artifacts\locale-packages` succeeded from `src` with 0 warnings and 0 errors. No `readme.md` update was required because the supported locale behavior changes were limited to existing localized surfaces and regression coverage.
- **files edited/created**: `docs/plans/2026-03-12-locale-translation-completion-plan.md`
