---
name: add-locale
description: Use when adding a new Humanizer locale or bringing an existing shipped locale to full parity. Trigger when work touches src/Humanizer/Locales, localization registries, shared locale engines, or culture-specific localization tests, and the locale must end with intentional coverage for every shipped localized surface with no English fallback, unsupported-locale gaps, or partial completion.
---

# Adding Humanizer Locale Parity

## Overview

Treat locale work as parity work, not translation work. A locale is not done when a YAML file exists; it is done only when every shipped localized surface has intentional behavior for that locale and the repo verifies that behavior.

If existing YAML and shared kernels cannot reach parity, expand the work into generator, runtime, registry, documentation, and test changes until parity is real. Do not land a partial locale.

For parity purposes, support is a behavioral question: does the feature work correctly for the locale? Ownership style, profile shape, and inheritance chain are maintainer details, not support categories.

## Hard Rules

1. Start with the repo docs, not guesswork: read `docs/adding-a-locale.md` and `docs/locale-yaml-how-to.md`, then read `docs/locale-yaml-reference.md` when the surface shape or engine options are unclear.
2. Treat `src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs` as the source of truth for canonical locale surfaces.
3. Treat both new locales and existing shipped locales the same way: the end state is full parity across shipped localized features.
4. Do not rely on English fallback or unsupported-locale behavior for any shipped locale.
5. There is no shipped-locale exemption list in this repo. If any canonical surface is unresolved, parity is incomplete.
6. Use `variantOf` only for genuine same-language inheritance that still produces natural output.
7. Prefer locale-owned YAML data plus shared structural engines. Keep locale-specific runtime leaves only when the behavior is genuinely procedural.
8. If a locale cannot be expressed with current shared engines, add or extend the generator contract and shared runtime kernel instead of reducing scope.
9. Do not mark the work complete until you have verified that full parity exists in code and tests. "Good enough", "most surfaces", or "we can follow up later" are failures.
10. Generic runtime fallback does not count as support. A locale surface is only supported when it has correct locale-specific behavior, either authored directly or inherited from a same-language parent that genuinely provides the right behavior for that surface.

## Anti-Rationalization Rules

These are failure modes, not acceptable interpretations:

- "This locale never had that feature before" is not parity.
- "Inherited from the parent, probably fine" is not verification.
- "The YAML compiles" is not completion.
- "The tests I happened to touch pass" is not parity if sweep coverage still leaves gaps.
- "Most surfaces are done" is failure.
- "We can follow up on the remaining surfaces later" is failure.
- "The output is understandable" is failure if it is unnatural for native speakers.
- "A locale-specific leaf is easier" is not justification for avoiding a reusable shared engine.
- "The registry resolved something" is not proof of parity.
- "The method returned a non-empty string" is not proof of parity.
- "The output is culture-aware" is not proof of parity if it came from a generic/default fallback path.
- "The locale works because generic formatting returned something reasonable" is not parity for a canonical surface that is supposed to be linguistically modeled.

If the work needed for parity grows into generator, runtime, registry, docs, or tests, expand the work. Do not shrink the goal.

## Workflow

### 1. Produce The Preflight Gap Report

Inspect the target locale YAML, related parent locales, current registries, and existing tests before editing anything.

Before making any implementation changes, write a preflight gap report that lists every canonical surface for the locale with one of these statuses:

- `locale-owned`
- `same-language inherited`
- `missing`
- `english-fallback`
- `unsupported`
- `unknown`

Use the canonical surfaces. Canonical authoring surfaces under `surfaces` are exactly `list`, `formatter`, `phrases`, `number`, `ordinal`, `clock`, `compass`, and `calendar`. Canonical nested members are `number.words`, `number.parse`, `number.formatting`, `ordinal.numeric`, `ordinal.date`, `ordinal.dateOnly`, `calendar.months`, `calendar.monthsGenitive`, and `calendar.hijriMonths`.

The preflight report must identify the full current ownership path for each surface, including the inheritance chain to the terminal owner when inheritance is involved. `unknown` is allowed only during initial inspection and must be eliminated before implementation starts.

### 2. Build The Parity Map

Create a parity map artifact at:

`artifacts/YYYY-MM-DD-<locale>-parity-map.md`

The `artifacts/` directory is local-only and gitignored. Parity map and audit artifacts live there as working scratch files for the duration of the epic. Do not commit them. Anything that needs to outlive the epic belongs in `docs/`, the spec, or the test suite as proof.

Build the parity map as a concrete table, not notes. Use at least these columns:

- `surface`
- `ownership path`
- `current state`
- `target state`
- `support state`
- `proof kind`
- `files to change`
- `tests proving parity`
- `proof file/assertion`
- `verification command`
- `verification exit status`
- `verified at`
- `status`
- `term review status`

The parity map must cover the canonical surfaces explicitly. Use top-level rows plus required proof subrows for:

- `list`
- `formatter`
- `phrases.relativeDate`
- `phrases.duration`
- `phrases.dataUnits`
- `phrases.timeUnits`
- `number.words`
- `number.words.cardinal`
- `number.words.ordinal`
- `number.parse`
- `number.parse.cardinal`
- `number.parse.ordinal`
- `number.formatting.decimalSeparator` (only when the locale authors a `number.formatting` override; mark "inherited from parent" or "not applicable" otherwise)
- `ordinal.numeric`
- `ordinal.date`
- `ordinal.dateOnly`
- `clock`
- `compass`
- `calendar.months` (only when the locale authors a `calendar` override; mark "inherited from parent" or "not applicable" otherwise)
- `calendar.monthsGenitive` (only when the locale authors a `calendar` override with a genitive array; mark "inherited from parent" or "not applicable" otherwise)
- `calendar.hijriMonths` (only when the locale authors or inherits a `calendar.hijriMonths` override; mark "inherited from parent" or "not applicable" otherwise)

Add more `number.words.*` or `number.parse.*` proof rows whenever the selected engine owns additional meaningful branches such as tuple handling, gendered variants, abbreviation parsing, or special composition paths. Do not collapse multiple behavior families into one green bucket.

Allowed values for `current state` and `target state`:

- `locale-owned`
- `same-language inherited`
- `missing`
- `english-fallback`
- `unsupported`

Allowed values for `support state`:

- `supported`
- `not supported`

Allowed values for `proof kind`:

- `locale-owned exact-output`
- `same-language inherited exact-output`
- `locale-owned structural assertion`
- `same-language inherited structural assertion`
- `generic fallback`
- `unsupported`

Any row with `proof kind: generic fallback` fails parity and blocks completion.

Do not start implementation until every shipped localized surface for the locale appears in the parity map.

Use `references/parity-checklist.md` for the surface inventory, repo file map, and surface-to-files matrix.

Also include a short effective-gap summary below the table listing any surfaces that still resolve as `missing`, `english-fallback`, or `unsupported`. The implementation is not ready to claim parity until that summary is empty.

Use `status` values such as:

- `not-started`
- `blocked`
- `in-progress`
- `proved`

Do not mark a row `proved` until the row has a concrete proving test or assertion, not just intended coverage. "suite passed" is not valid row-level proof.

The actual parity verdict for the locale must be expressed only in `support state` terms. Do not use implementation-shaped verdicts such as "broadly available", "intentionally resolved", or other wording that hides whether the feature truly works.

### 3. Derive Locale Terms With Independent Review

When you need new locale-owned words, phrases, grammatical forms, parser tokens, or other translated terms, do not invent them in a single pass.

Use two subagents:

- a proposer subagent to suggest the most language-appropriate terms for the target surface and usage context
- a reviewer subagent acting as a native speaker to review those suggestions for naturalness, correctness, register, and edge-case awkwardness

Use prompts shaped like this:

Proposer:

```text
You are deriving locale-owned Humanizer terms for <locale>.
Target surface: <surface>.
Behavior and usage context: <examples and expected runtime behavior>.
Suggest the most natural locale terms, including any grammar/inflection notes and parser-token implications.
Call out alternatives you rejected and why.
```

Reviewer:

```text
You are a native-speaker reviewer for <locale>.
Target surface: <surface>.
Expected behavior: <examples and expected runtime behavior>.
Proposed terms: <candidate terms from proposer>.
Review for naturalness, correctness, register, grammar, inflection, and whether native speakers would actually say this in this context.
Reject anything that is literal, awkward, or only technically understandable.
```

Keep the reviewer independent. Do not ask it to rubber-stamp the first proposal. Give it the target behavior and the proposed terms, and require it to call out unnatural phrasing, literal translations, grammar mistakes, or terms that sound technically correct but are not what native speakers would actually say.

Do not accept terms unless both subagents support the final choice and the parity map records the resolution. If they disagree, keep iterating until you have a justified set of locale terms that both semantic correctness and native-speaker naturalness support. Do not land locale-owned wording that only one pass has seen.

After term selection, run the reviewer again against representative runtime outputs, not just isolated terms. A term that looks acceptable in isolation is still wrong if the composed Humanizer output is unnatural. Record reviewer confidence, reviewer limitations, rejected alternatives, and the reviewed runtime outputs in the parity map. If the reviewer cannot credibly claim native-level or near-native confidence for judging naturalness, completion is blocked.

### 4. Choose The Smallest Honest Implementation

For each missing or unacceptable surface:

- reuse an existing engine if the behavior is the same algorithm with locale-owned words or tables
- extend a shared kernel and generator contract if the behavior is structurally reusable
- keep or add a locale-specific runtime implementation only when forcing it into YAML/shared kernels would create imperative hooks or an exception bucket

Do not hide algorithm gaps behind translation-only changes.

### 5. Implement The Locale End-To-End

Update the locale through the real ownership path:

- locale YAML in `src/Humanizer/Locales`
- source generator wiring when profile generation or registry input changes
- runtime kernels or residual locale-specific implementations under `src/Humanizer/Localisation`
- registry coverage and exact-output tests
- contributor docs when the supported surface set or authoring guidance changes

When adding a regional variant, override only real differences. When adding a neutral locale, own the full behavior directly or through explicit shared abstractions.

### 6. Prove Parity, Not Just Compilation

Add or update tests that prove the locale's actual output and registry presence. Favor exact-output tests for grammar-sensitive behavior and keep sweep tests strong enough to catch accidental fallback. Treat `formatter` and `phrases` as separate surfaces when auditing and proving parity.

Proof must distinguish real support from fallback:

- registry presence is not enough
- non-empty output is not enough
- culture-aware formatting is not enough
- generic runtime fallback is not enough

For every surface that claims parity, add at least one proof that the locale is not merely riding a generic fallback path when such a fallback exists.

Feature-specific minimum bars:

- `clock`
  Do not count `TimeOnly.ToClockNotation()` as supported merely because the default converter returned `ToString("t", culture)` or another generic time format. Prove locale-specific clock behavior with exact-output tests or same-language inherited exact-output tests for both exact and rounded cases.
- `ordinal.date` / `ordinal.dateOnly`
  Do not count date ordinal support as complete merely because a culture date format happened to place the day naturally. Prove the actual ordinal-day behavior with exact outputs.
- `number.words` / `number.parse`
  Round-trip and representative large-number cases must prove the locale's own numbering system rather than an English or generic parser fallback.
- `formatter` / `phrases`
  Do not accept generic resources or default English-derived strings as parity. Use exact localized outputs for representative tense and unit cases.

At minimum, review and extend the locale parity checks in:

- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs`
- `tests/Humanizer.Tests/Localisation/LocaleFallbackSweepTests.cs`
- `tests/Humanizer.Tests/Localisation/LocaleRegistrySweepTests.cs`
- `tests/Humanizer.Tests/Localisation/GeneratedLocaleData/GeneratedFormatterRuntimeTests.cs`
- `tests/Humanizer.Tests/Localisation/ExactLocaleDateAndTimeRegistryTests.cs`
- the target culture folder under `tests/Humanizer.Tests/Localisation/<culture>`

When generator contracts, inheritance rules, or locale schema behavior change, also update `tests/Humanizer.SourceGenerators.Tests`.

Update the parity map artifact as you go. The final artifact should show no remaining `missing`, `english-fallback`, or `unsupported` rows for shipped localized surfaces.

Before claiming completion, add a closeout line for every canonical surface and required proof subrow showing:

- final ownership path
- whether it is `locale-owned` or `same-language inherited`
- full inheritance chain to the terminal owner when inherited
- final `support state`
- final `proof kind`
- the proving test or assertion
- whether native-speaker review was needed

Also record a before/after parity delta:

- initial unresolved surfaces from the preflight gap report
- final unresolved surfaces after implementation

The final unresolved set must be empty.

## Validation

Run the real repo validation for locale work:

```
dotnet test tests/Humanizer.SourceGenerators.Tests/Humanizer.SourceGenerators.Tests.csproj --framework net10.0
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0
dotnet pack src/Humanizer/Humanizer.csproj -c Release -o artifacts/locale-parity-validation
```

Do not add the old package verification step. The completion gate is full parity verification in code and tests, not satellite-package validation.

If you touch a hot shared kernel or registry dispatch path, run the relevant benchmark coverage before claiming parity.

## Completion Gate

Do not say the locale is complete until all of these are true:

- every shipped localized surface for the locale is intentionally locale-owned or intentionally inherited from a same-language parent
- every shipped localized surface for the locale has `support state: supported`
- no shipped surface for the locale depends on English fallback
- no shipped surface for the locale throws because the locale is unsupported
- no shipped surface for the locale is only "working" because a generic/default runtime fallback returned something culture-aware
- new locale-owned terms were reviewed by both a proposer subagent and a native-speaker reviewer subagent, and any disagreements were resolved before landing
- representative composed runtime outputs were reviewed for naturalness when new locale-owned wording was introduced
- the reviewer evidence records confidence and limitations, and the reviewer did not disclaim the ability to judge native naturalness
- the parity map artifact exists locally under `artifacts/` (gitignored, not committed) and every shipped localized surface for the locale is accounted for there
- the preflight gap report exists and the final before/after parity delta shows an empty unresolved set
- every canonical surface and required proof subrow has a concrete closeout entry with proof, not just an implementation note
- no canonical surface or required proof subrow has `proof kind: generic fallback`
- exact-output tests cover the locale's custom or grammar-sensitive behavior
- sweep tests and registry tests include the locale wherever applicable
- source-generator tests cover schema, inheritance, or generated wiring changes when those changed
- `dotnet test` passes for `tests/Humanizer.SourceGenerators.Tests` on `net10.0`
- `dotnet test` passes for `tests/Humanizer.Tests` on `net10.0` and `net8.0`
- `dotnet pack` succeeds without warnings or errors

If any item above is false, the locale is not at parity yet.

## Final Response Contract

Do not present locale work as complete unless the final response includes all of these:

- the parity map artifact path
- the preflight gap report summary
- the before/after parity delta
- the empty effective-gap summary
- one line per canonical surface stating final ownership and proof
- one line per canonical surface stating final `support state`
- the exact validation commands run
- an explicit statement that no canonical surface remains `missing`, `english-fallback`, or `unsupported`

If the final response cannot include that evidence, report the remaining gaps instead of claiming completion.

## Stop Conditions

Stop and keep working if any of these are true:

- the parity map still contains any `missing`, `english-fallback`, or `unsupported` row for a shipped localized surface
- the preflight gap report still contains any `unknown` surface
- locale-owned terms were written without both subagent passes
- representative runtime outputs were not reviewed for naturalness after new locale-owned wording was introduced
- same-language inheritance looks linguistically suspicious or unnatural, even if technically functional
- tests only prove execution, not natural output for grammar-sensitive or locale-specific behavior
- tests only prove the generic fallback path executes
- runtime fallback is still doing work that the locale should own explicitly
- the implementation compiles but the ownership path is still ambiguous
- any canonical surface or required proof subrow lacks a concrete proving test or assertion
