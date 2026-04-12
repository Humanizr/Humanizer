## Description

Update three user-facing files that describe the current post-fn-3 state at various levels of detail:

1. **`release_notes.md` `vNext` section (around lines 51-56)** — currently covers the YAML locale pipeline, long-based `ToNumber` breaking change, and analyzer/package layout. Missing entries for cross-platform-visible fn-1/fn-3 deliverables:
   - **Unified `phrase-clock` engine** — replaces the former `phrase-hour`, `relative-hour`, and four handwritten locale leaf converters (German, French, Luxembourgish, Japanese). Note that `DefaultTimeOnlyToClockNotationConverter` is also removed — all 62 shipped locales are now YAML-authored clock notation.
   - **New `calendar:` surface** with `months` and `monthsGenitive` — locale-authored month-name overrides threaded through `PatternDateToOrdinalWordsConverter` and `PatternDateOnlyToOrdinalWordsConverter`. Bn, fa, he, ku currently use this (and possibly ta/zu-ZA per fn-5.1 outcome — check fn-5.1 task done-evidence before writing this bullet).
   - **New `number.formatting.decimalSeparator` sub-block** — locale-authored decimal-separator overrides threaded through `ByteSize.ToString` and `MetricNumeralExtensions`. Ar, fr-CH, ku currently use this. Note that caller-supplied custom `NumberFormatInfo`/`IFormatProvider` is preserved as-is (not overridden).
   - **Cross-platform consistency** — date/time month names and decimal separators for overridden locales are now byte-identical across macOS, Linux, and Windows (both .NET 10 and .NET Framework 4.8 NLS).

2. **`readme.md:32-36` Locale Data section** — currently enumerates 8 canonical surfaces using the pre-fn-3 form ("list formatting, formatter phrases, number words, number parsing, numeric ordinals, date ordinals, clock notation, and compass headings"). This omits `calendar` (month-name overrides) and `number.formatting` (decimal-separator overrides). Options:
   - (a) Extend the enumeration to include `calendar:` and `number.formatting:`; OR
   - (b) Replace the enumeration with a single link to `docs/localization.md` as authoritative.
   Choose (a) if you can keep it one sentence; choose (b) if the enumeration would bloat.

3. **`ARCHITECTURE.md` generator table (lines 40-51) and prose summary (line 88)**:
   - Generator table row for `LocaleRegistryInput`: currently says "master locale registry" — extend to mention it also emits `LocaleNumberFormattingOverrides.g.cs` (the per-locale decimal-separator override registry consumed by `ByteSize.ToString` and `MetricNumeralExtensions`).
   - Generator table row for `OrdinalDateProfileCatalogInput`: currently says "Ordinal date formatting per locale" — extend to mention it also consumes `calendar.months` / `calendar.monthsGenitive` and embeds them into generated `OrdinalDatePattern` instances for month-name substitution.
   - Prose summary at line 88 ("Each YAML file defines a locale's capabilities: phrase tables for date/time humanization, number-to-words rules, ordinalizer patterns, heading labels, and clock notation preferences") — enumerate all 8 canonical surfaces (add `calendar`-derived month overrides and `number.formatting`-derived decimal-separator overrides) OR rephrase to a less-enumerative summary.

**Size:** M
**Files:**
- `release_notes.md` — extend `vNext` section with 4 new bullets
- `readme.md` — update Locale Data section (single line edit)
- `ARCHITECTURE.md` — update 2 generator table rows + 1 prose summary line

## Investigation targets

**Required:**
- `release_notes.md:1-70` — full `vNext` section to match style of existing entries
- `readme.md:30-45` — Locale Data section context
- `ARCHITECTURE.md:35-95` — generator table + prose summary
- `docs/localization.md:70-71` — the canonical surface list we're aligning to
- `docs/locale-yaml-reference.md:380-454` — authoritative `calendar:` and `number.formatting:` section (for terminology precision)
- `src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs` — for the generator table extension (what `LocaleNumberFormattingOverrides.g.cs` actually contains). Note: this file is under `Generators/`, not `Common/` — the historic path reference was stale.
- `src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalDateProfileCatalogInput.cs` — for the generator table extension (how `calendar.months` is embedded). Note: this file is under `Generators/ProfileCatalogs/`, not `Common/`.
- `.flow/tasks/fn-5-locale-parity-sign-off-verify-code.1.md` (done evidence) — to read the fn-5.1 decision before writing the `calendar:` release-notes bullet

**Optional:**
- `src/Humanizer/Localisation/NumberToWords/LocaleNumberFormattingOverrides.g.cs` (or wherever the generated file lands in obj/) — to cite the exact emitted type name

## Approach

- Follow the existing bullet style in `release_notes.md` `vNext` — concise, user-facing, one line per item.
- For `readme.md`, prefer option (b) (link to `docs/localization.md`) if the enumeration would run past one readable sentence — the README is a landing page, not a reference.
- For `ARCHITECTURE.md`, extend existing rows rather than adding new rows (both new data paths are consumed by existing generator inputs, not new ones).
- Do NOT add new sections or rewrite surrounding content.
- Do NOT add a migration guide for the deleted converters — they were internal implementation classes with no public-API consumers (see `tests/Humanizer.Tests/ApiApprover/*.verified.txt` for confirmation that only the `ITimeOnlyToClockNotationConverter` interface was public).

## Key context

- fn-5.1 outcome affects one bullet in `release_notes.md`: if ta/zu-ZA got overrides, the `calendar:` bullet should name six locales — otherwise four locales. This task must read the fn-5.1 resolution (task done-evidence file) before finalizing the bullet. Both outcomes are equally valid; the release-notes bullet just needs to match reality.
- The `release_notes.md` existing breaking-changes format: skim the current entries and match their structure (bullet, short phrase, no "we" voice).
- The deleted converter list is intentionally not enumerated in release notes — call them out as "four handwritten residual leaf converters" rather than naming each one, since the user never invoked them directly.

## Acceptance

- [ ] `release_notes.md` `vNext` section has a bullet for the unified `phrase-clock` engine (mentions `DefaultTimeOnlyToClockNotationConverter` removal and the four residual leaf removals by category, not by FQN)
- [ ] `release_notes.md` `vNext` section has a bullet for the new `calendar:` surface with `months`/`monthsGenitive`, naming the locales that currently author overrides (matches fn-5.1 outcome, read from the fn-5.1 task done-evidence file)
- [ ] `release_notes.md` `vNext` section has a bullet for the new `number.formatting.decimalSeparator` sub-block, naming the locales that currently author overrides (ar, fr-CH, ku)
- [ ] `release_notes.md` `vNext` section has a bullet describing cross-platform byte-identical output for overridden locales
- [ ] `readme.md:32-45` Locale Data section either enumerates all 8 canonical surfaces (including `calendar:` and `number.formatting:`) OR links to `docs/localization.md` as authoritative
- [ ] `ARCHITECTURE.md` generator table `LocaleRegistryInput` row mentions `LocaleNumberFormattingOverrides.g.cs` output
- [ ] `ARCHITECTURE.md` generator table `OrdinalDateProfileCatalogInput` row mentions `calendar.months`/`calendar.monthsGenitive` consumption and embedding
- [ ] `ARCHITECTURE.md:88` prose summary enumerates all 8 canonical surfaces OR omits the enumeration with a link to `docs/localization.md`
- [ ] `dotnet format Humanizer.slnx --verify-no-changes` still passes (markdown edits only, but confirm)
- [ ] No other content in `release_notes.md`, `readme.md`, or `ARCHITECTURE.md` modified

## Done summary
Updated release_notes.md vNext with 4 new bullets for fn-3 deliverables (unified phrase-clock engine, calendar surface with months/monthsGenitive for bn/fa/he/ku/ta/zu-ZA, number.formatting.decimalSeparator for ar/fr-CH/ku, cross-platform byte-identical output). Updated readme.md Locale Data section to enumerate the 8 canonical surfaces by name with link to docs/localization.md. Updated ARCHITECTURE.md generator table rows for LocaleRegistryInput (LocaleNumberFormattingOverrides.g.cs output) and OrdinalDateProfileCatalogInput (calendar.months/monthsGenitive consumption), and updated prose summary to name all 8 canonical surfaces with links to reference docs.
## Evidence
- Commits: a972cd73, abb45588
- Tests: dotnet format Humanizer.slnx --verify-no-changes
- PRs: