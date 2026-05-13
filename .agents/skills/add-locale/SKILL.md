---
name: add-locale
description: Use when adding a new Humanizer locale or bringing an existing shipped locale to full parity. Trigger when work touches src/Humanizer/Locales, localization registries, shared locale engines, generator locale profiles, or culture-specific localization tests, and the locale must end with intentional coverage for every shipped localized surface with no English fallback, unsupported-locale gaps, generic fallback dependency, or partial completion.
---

# Adding Humanizer Locale Parity

## Purpose

Treat locale work as parity work, not translation work. A locale is complete only when every shipped localized surface has intentional behavior for that locale and the repo verifies that behavior.

Never shrink the goal to match the existing YAML shape. If parity requires generator, runtime, registry, documentation, or test changes, do that work instead of accepting a partial locale.

A shipped locale surface is supported only when it is locale-owned or inherited from a same-language parent that genuinely produces natural output. English fallback, unsupported-locale behavior, and generic/default runtime fallback are not parity.

## Load Order And Token Discipline

Load only what the phase needs:

1. Always read this file and `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs` before planning.
2. For new locales, read `docs/adding-a-locale.md` and `docs/locale-yaml-how-to.md`. Read `docs/locale-yaml-reference.md` when schema, engine, inheritance, or generator behavior is unclear.
3. Read the target locale YAML and every parent in the `variantOf` chain before classifying support.
4. Read `references/parity-checklist.md` when building the parity map, choosing files/tests, or closing out. It is the detailed source for surfaces, proof rows, file maps, and validation commands.
5. Read `references/urdu-epic-learnings.md` only when a red flag applies: Arabic-script locale, Indic/South Asian numbers, plural-rule uncertainty, ordinals, Hijri/non-Gregorian calendars, no-delta variants, optional YAML fields, schema/generator changes, shared-engine changes, or PR hygiene issues.
6. Use `references/goal-prompt-template.md` when drafting a reusable `/goal` prompt for locale implementation.

Do not copy large checklist tables into your working context unless you are filling them. Use searches and targeted reads first.

## Non-Negotiables

- Account for every canonical surface from `CanonicalLocaleAuthoring.cs`.
- Eliminate `unknown` before implementation starts.
- Do not rely on English fallback, unsupported-locale exceptions, or generic culture-aware runtime output.
- Use `variantOf` only for genuine same-language inheritance with natural output.
- Prefer locale-owned YAML data plus shared structural engines. Add or extend shared generator/runtime contracts when the rule family is reusable.
- Keep locale-specific runtime leaves only when the behavior is genuinely procedural.
- Add or update xUnit proof for every functional change.
- Add XML documentation for new or modified public APIs.

## Fast-Fail Checks Before Editing

Run these checks during preflight. Before editing, identify applicable risks and add the required proof obligations to the plan. The proof itself must exist before closeout. If any applicable risk has no proof plan, stop implementation and resolve the plan first.

| Risk | Fast-fail probe | Required proof |
| --- | --- | --- |
| Artifact evidence | Is the parity map under `artifacts/` being treated as PR evidence? | Keep it local scratch; durable proof must be committed tests/docs plus final/PR summary copied from the map. |
| Ordinal collapse | Does the plan merge `number.words.ordinal`, `ordinal.numeric`, `ordinal.date`, or `ordinal.dateOnly`? | Separate ownership, implementation, and tests for each surface. |
| Word ordinal engine | Would a digit suffix engine produce unnatural output such as digit+suffix when words are required? | Use or build a word-ordinal engine; prove irregular stems separately from cardinals. |
| Culture binding | Does a shared ordinal/number engine call `NumberToWords` or parse through another culture-aware service? | Prove target culture is passed intrinsically, including a test where current culture differs from target culture. |
| Hijri/calendar | Does behavior depend on a `DateTime`/`DateOnly` construction calendar or optional calendars? | Prove culture/default-calendar runtime behavior per supported TFM/platform; do not assume ICU and NLS accept the same calendars. |
| Optional YAML field | Is a new optional field or omitted section being added? | Cover parser, resolver/inheritance merge, generated profile/emitter, runtime consumer, migration/default behavior, and source-generator tests. |
| Script/codepoints | Does the locale share a script with visually similar neighboring languages? | Check code points for lookalikes; Arabic-script locales should compare against the Urdu lessons. |
| Plural rules and scales | Are plural rules or number scales copied from a nearby language? | Check CLDR-style plural family and natural scale system, such as lakh/crore for South Asian locales. |
| No-delta variant | Is a regional file present only for matrix coverage? | It must have an explicit same-language parent and omit `surfaces` unless real overrides exist. |
| Shared engine masking | Did a shared engine change only get tested with real locale data? | Add synthetic or sentinel tests that force divergent branches and edge cases. |
| Repo hygiene | Are there local lock files, build outputs, unrelated edits, or namespace mismatches? | Clean status before closeout; test namespaces must match culture folders. |

## Phase 0: Preflight Gap Report

Before editing implementation files:

1. Identify target culture, regional variants, parent chain, and whether each file is locale-owned or inherited.
2. Build `artifacts/YYYY-MM-DD-<locale>-parity-map.md` using `references/parity-checklist.md`.
3. For every canonical surface and required proof subrow, record current state as one of `locale-owned`, `same-language inherited`, `missing`, `english-fallback`, `unsupported`, or `unknown`.
4. Record the full ownership path, including the terminal owner for inherited surfaces.
5. Remove every `unknown` before implementation starts.

The parity map is gitignored scratch. Do not commit it. If reviewers need durable evidence, copy the closeout summary into the PR/final response and rely on committed tests/docs for proof.

## Phase 1: Choose The Smallest Honest Implementation

For each unresolved surface, choose in this order:

1. Same-language inheritance, when output is genuinely natural.
2. Existing shared engine with locale-owned data.
3. Shared engine or generator contract extension for reusable structure.
4. Locale-specific runtime implementation only for genuinely procedural behavior.

Do not add locale-specific leaves just to avoid reusable generator/runtime work. When adding no-delta regional variants for first-class matrix coverage, omit `surfaces` entirely unless the variant owns real overrides; `surfaces: null` remains invalid.

## Phase 2: Derive And Review Locale Terms

When introducing locale-owned words, phrases, parser tokens, grammatical forms, or composed outputs:

1. Use a proposer subagent to suggest natural terms for the exact Humanizer surface and runtime context.
2. Use an independent native-speaker reviewer subagent to reject literal, awkward, wrong-register, or grammatically incorrect terms.
3. Re-review representative composed runtime outputs, not just isolated words.
4. Record reviewer confidence, limitations, rejected alternatives, and final rationale in the parity map.

If the reviewer cannot credibly judge native or near-native naturalness, or proposer/reviewer disagreement remains unresolved, completion is blocked.

## Phase 3: Implement End-To-End

Update the real ownership path:

- locale YAML under `src/Humanizer/Locales`
- source generator inputs/contracts when profile generation, inheritance, schema, or registry wiring changes
- shared runtime kernels or residual locale-specific implementations under `src/Humanizer/Localisation`
- registry, fallback, exact-output, and culture-specific tests
- docs when authoring behavior or public behavior changes

Keep `formatter` and `phrases` as separate surfaces. Keep `number.words`, `number.parse`, `ordinal.numeric`, `ordinal.date`, `ordinal.dateOnly`, and `clock` separate during implementation and proof.

## Phase 4: Prove Parity

Use exact-output tests for grammar-sensitive behavior and sweep/registry tests to prove the locale is not riding fallback.

Minimum proof expectations:

- `clock`: prove locale-specific exact and rounded cases; generic time formatting is not support.
- `ordinal.date` and `ordinal.dateOnly`: prove actual ordinal-day behavior with exact outputs.
- `number.words` and `number.parse`: prove the locale numbering system, including representative large values and parse/word branches.
- `formatter` and `phrases`: prove localized representative outputs, not generic English-derived resources.
- shared engines: include synthetic/sentinel coverage for branch behavior that real locale data might mask.
- generator/schema changes: update `tests/Humanizer.SourceGenerators.Tests`.

Use `references/parity-checklist.md` for the complete proof-row list and file/test map. Do not mark a parity-map row `proved` until it names a concrete proving file/assertion and verification command.

## Validation

Run focused tests while iterating. Before claiming parity, run the real repo validation:

```bash
dotnet test tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj --framework net10.0
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0
dotnet pack src/Humanizer/Humanizer.csproj -c Release -o artifacts/locale-parity-validation
```

Run `net48` tests only on Windows hosts when the test project includes that TFM. Do not add the old package verification step for locale parity closeout.

If you change a hot shared kernel or registry dispatch path, run the relevant focused benchmark or performance coverage before closeout.

## Completion Gate

Do not say the locale is complete until all are true:

- every canonical surface and required proof subrow is accounted for in the parity map
- no row remains `missing`, `english-fallback`, `unsupported`, `unknown`, or `proof kind: generic fallback`
- conditional proof subrows marked `not applicable` are justified
- every shipped localized surface has `support state: supported`
- every surface is intentionally locale-owned or same-language inherited with the full ownership chain recorded
- exact-output and sweep/registry tests cover the locale's custom, inherited, or grammar-sensitive behavior
- term review is complete for all new locale-owned wording and composed outputs
- source-generator tests cover schema, inheritance, profile generation, or registry changes when those changed
- validation commands pass without new warnings or errors
- git status contains no accidental local state, build residue, unrelated edits, or committed scratch artifacts

## Final Response Contract

If claiming completion, include:

- parity map artifact path
- preflight gap summary
- before/after parity delta with an empty final unresolved set
- effective-gap summary stating no canonical surface remains `missing`, `english-fallback`, or `unsupported`
- one closeout line per canonical surface and required proof subrow with final ownership, support state, proof kind, and proving test/assertion
- exact validation commands run

If that evidence is unavailable, report remaining gaps instead of claiming completion.

## Stop Conditions

Stop and keep working when any of these remain true:

- a surface still depends on fallback, unsupported behavior, or ambiguous ownership
- preflight still contains `unknown`
- locale-owned terms lack proposer plus independent reviewer evidence
- representative runtime outputs were not reviewed after new wording was introduced
- same-language inheritance looks linguistically suspicious
- tests only prove execution or generic fallback
- a shared engine lacks synthetic coverage for edge branches
- any parity-map row lacks concrete proof
- the final validation commands have not passed
