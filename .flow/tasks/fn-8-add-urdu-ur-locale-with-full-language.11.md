# fn-8-add-urdu-ur-locale-with-full-language.11 Regional variants: ur-PK and ur-IN YAML files with variantOf merge semantics

## Description

Author regional variants `ur-PK.yml` and `ur-IN.yml` with the minimum valid shape. The canonical parser (`CanonicalLocaleAuthoring.Parse`) requires a `surfaces` key, so the minimum is `locale:` + `variantOf:` + `surfaces: {}`. Variant-file existence is deliberate so both cultures appear in `LocaleRegistrySweepTests` and every `ShippedLocaleRows` matrix dataset. Matrix rows for the variants live in `.6`, not here.

The `docs/locale-yaml-reference.md` Regional Variant Checklist exception is recorded by `.8`.

**Size:** S
**Files:**
- `src/Humanizer/Locales/ur-PK.yml` (new)
- `src/Humanizer/Locales/ur-IN.yml` (new)

## Approach

1. **Identify CLDR deltas** via `.1` probe output + `cldr-json` `ur_PK` / `ur_IN`. Common deltas:
   - `ur-PK`: currency `₨`, first-day-of-week.
   - `ur-IN`: currency `₹`, week-of-year rule, possibly first-day-of-week.
   Urdu's current Humanizer feature set likely has no runtime-visible deltas unless a surface exposes currency or first-day-of-week.
2. **Author minimum-valid variant YAML**:
   ```yaml
   locale: 'ur-PK'
   variantOf: 'ur'
   surfaces: {}
   ```
   If genuine runtime-visible deltas exist, override only the specific fields that differ.
3. **Do NOT omit `surfaces`** — `CanonicalLocaleAuthoring.Parse` raises if absent.
4. Build + run source-generator tests to confirm both variant files are accepted.

## Investigation targets

**Required**:
- `/Users/claire/dev/Humanizer/src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs:Parse` — `surfaces` requirement
- `/Users/claire/dev/Humanizer/docs/locale-yaml-reference.md:41-54` — merge rules
- Probe output from `.1`
- `/Users/claire/dev/Humanizer/src/Humanizer/Configuration/LocaliserRegistry.cs` — CultureInfo.Parent walking

**Optional**:
- Existing no-delta variants in the repo for shape precedent

## Key context

- Minimum valid shape per review:
  ```yaml
  locale: 'ur-PK'
  variantOf: 'ur'
  surfaces: {}
  ```
- Variants deliberately shipped even with no runtime-visible deltas so `ur-PK` / `ur-IN` are first-class in `LocaleRegistrySweepTests` and `ShippedLocaleRows`.
- Matrix rows in `.6`.
- Docs-exception reconciliation in `.8`.

## Acceptance

- [ ] `src/Humanizer/Locales/ur-PK.yml` exists with at minimum `locale: 'ur-PK'`, `variantOf: 'ur'`, `surfaces: {}` (plus genuine deltas if probe shows any).
- [ ] `src/Humanizer/Locales/ur-IN.yml` exists with at minimum `locale: 'ur-IN'`, `variantOf: 'ur'`, `surfaces: {}` (plus genuine deltas if any).
- [ ] Source-generator build accepts both files; generator tests pass.
- [ ] `LocaleRegistrySweepTests` passes — both variants swept and resolve through the registries.
- [ ] `rg -P '\x{200E}|\x{200F}|\x{061C}'` on variant files returns no matches.
- [ ] No separate variant-resolution test file — `.6` covers resolution via matrix + sweep tests.
- [ ] Parity map updated noting genuine deltas (or "none — shipped for matrix coverage").

## Done summary
Added ur-PK and ur-IN regional variant YAML files with minimum-valid variantOf shape (surfaces: {}). Probe data confirmed no runtime-visible deltas between variants and neutral ur for any Humanizer surface. Updated parity map with comprehensive delta analysis documenting that variants are shipped for matrix coverage.
## Evidence
- Commits: 86662019275aaaa2a80f396a2ff27c39b85f066d
- Tests: dotnet test --project tests/Humanizer.SourceGenerators.Tests (63/63 passed), dotnet build src/Humanizer/Humanizer.csproj -c Release (0 warnings, 0 errors), dotnet test --project tests/Humanizer.Tests --filter-class LocaleRegistrySweepTests (12493/12494 passed, 1 pre-existing Uzbek failure), rg bidi control check (no matches)
- PRs: