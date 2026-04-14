# fn-11-fix-urdu-locale-ci-pr-feedback-rebase.4 Make 'surfaces:' optional when empty; strip from ur-PK/ur-IN/zh-CN

## Description
Make the YAML `surfaces:` block optional when a variant locale has no overrides (empty map), and remove the redundant `surfaces: {}` declaration from the three locales that currently carry only that noise: `ur-PK.yml`, `ur-IN.yml`, `zh-CN.yml`.

**Size:** S
**Files:**
- `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs` (schema/validation — relax the `surfaces:` required check when `variantOf:` is set and the file has no override keys)
- `src/Humanizer/Locales/ur-PK.yml` (delete `surfaces: {}` line)
- `src/Humanizer/Locales/ur-IN.yml` (delete `surfaces: {}` line)
- `src/Humanizer/Locales/zh-CN.yml` (delete `surfaces: {}` line)
- `docs/locale-yaml-reference.md` / `docs/adding-a-locale.md` (note that `surfaces:` is optional for no-delta variants)

## Approach
1. Verify current behavior: does the generator reject a variant file that omits `surfaces:` entirely? Run the generator tests after removing `surfaces: {}` from one file to see what fails.
2. Relax the validation in `CanonicalLocaleAuthoring.cs` so that when `variantOf:` is present and no override keys exist, `surfaces:` can be omitted (or explicitly null) without a schema error. If `surfaces:` is absent, treat it as empty.
3. Remove `surfaces: {}` from the three files.
4. Update the two doc pages to mention that `surfaces:` is optional for variants with no overrides.

Do NOT:
- Touch variant files that already have content under `surfaces:` (e.g. `en-GB.yml`, `fr-BE.yml`).
- Change what `variantOf:` means or how fallback works at runtime.
- Remove the `surfaces:` block from non-variant locales (where it's the root of all authored data).

## Investigation targets
**Required:**
- `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs` — current validation of `surfaces:` (search for how the key is required).
- `src/Humanizer/Locales/en-GB.yml` (7 lines — has meaningful surfaces content) for contrast.
- `tests/Humanizer.SourceGenerators.Tests/` — any test that asserts on the presence of `surfaces:` shape.

**Optional:**
- `docs/locale-yaml-reference.md` — existing phrasing for the `surfaces:` section.

## Key context
- Three files currently carry `surfaces: {}` with no overrides: `ur-PK`, `ur-IN`, `zh-CN`. They exist purely as variant markers (`variantOf:` pointing at the parent). Empty `surfaces:` adds noise and can mislead readers into thinking overrides are intended.
- This task depends on fn-11.1 (rebase) so the change applies to a clean base.
- After this change: creating a new variant locale with no overrides becomes a two-line YAML (`locale:` + `variantOf:`).
## Acceptance
- [ ] `surfaces:` key is optional when `variantOf:` is set and no override content exists — schema validator accepts its absence.
- [ ] `ur-PK.yml`, `ur-IN.yml`, `zh-CN.yml` no longer contain `surfaces: {}`; each file is exactly two content lines (`locale:`, `variantOf:`).
- [ ] All Urdu and zh-CN tests still pass on net10.0 and net8.0 (no runtime behavior change).
- [ ] Source generator tests pass.
- [ ] `docs/locale-yaml-reference.md` (and/or `docs/adding-a-locale.md`) notes that `surfaces:` is optional for no-delta variants.
- [ ] `dotnet format Humanizer.slnx --verify-no-changes` passes.
## Done summary
TBD

## Evidence
- Commits:
- Tests:
- PRs:
