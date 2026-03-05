# Migrating from Humanizer 2.14.1 to 3.0.6

This guide is for teams upgrading from `2.14.1` to `3.0.6`.

This document was created to address [#1656](https://github.com/Humanizr/Humanizer/issues/1656) (undocumented v3 breaking changes), then updated as fixes landed in later v3 patches.

## Quick Checklist

1. Upgrade package/tooling prerequisites first.
2. Run namespace migration (`using Humanizer;`).
3. Rebuild all projects that reference Humanizer (binary compatibility changed in v3).
4. Validate behavior around `Pascalize`, `Dehumanize`, and `Titleize`.

## Breaking Changes and Related Issues

### Namespace Consolidation (source-breaking)

All sub-namespaces were collapsed to root `Humanizer`.

Related:
- [PR #1351](https://github.com/Humanizr/Humanizer/pull/1351)
- Upgrade impact tracking: [#1656](https://github.com/Humanizr/Humanizer/issues/1656)

Before:

```csharp
using Humanizer.Bytes;
using Humanizer.Localisation;
```

After:

```csharp
using Humanizer;
```

### Removed APIs

1. `StringExtensions.FormatWith(...)` removed
   Related: [PR #1395](https://github.com/Humanizr/Humanizer/pull/1395)

2. Obsolete `ToMetric` overloads removed:
   - `ToMetric(this int, bool hasSpace, bool useSymbol = true, int? decimals = null)`
   - `ToMetric(this double, bool hasSpace, bool useSymbol = true, int? decimals = null)`
   Related: [PR #1389](https://github.com/Humanizr/Humanizer/pull/1389)

3. `ToQuantity(this string, int, ...)` overloads were removed in early v3, then restored in `3.0.6`.
   Related: [PR #1338](https://github.com/Humanizr/Humanizer/pull/1338), [#1652](https://github.com/Humanizr/Humanizer/issues/1652), [PR #1679](https://github.com/Humanizr/Humanizer/pull/1679)

4. `EnglishArticles` enum removed
   Related: [PR #1443](https://github.com/Humanizr/Humanizer/pull/1443)

5. `Configurator.EnumDescriptionPropertyLocator` property removed
   Use `Configurator.UseEnumDescriptionPropertyLocator(...)` at startup.
   Related: [#1656](https://github.com/Humanizr/Humanizer/issues/1656)

### Enum Signature Changes

Enum APIs moved from `Enum` receiver signatures to constrained generics (`where T : struct, Enum`).

Related:
- Upgrade impact tracking: [#1656](https://github.com/Humanizr/Humanizer/issues/1656)

### Extensibility Surface Changes

If you implement custom formatters:

- `IFormatter` gained `TimeSpanHumanize_Age()`
- `DefaultFormatter` override surface changed (time-unit-aware `Format` overload)

Related:
- [PR #1068](https://github.com/Humanizr/Humanizer/pull/1068)
- Upgrade impact tracking: [#1656](https://github.com/Humanizr/Humanizer/issues/1656)

## Framework and Packaging Changes

### TFM Support Changes in v3

Compared to `2.14.1`, v3 removed:

- `netstandard1.0`
- `net462`
- `net472`

Related:
- [PR #1322](https://github.com/Humanizr/Humanizer/pull/1322)
- [PR #1482](https://github.com/Humanizr/Humanizer/pull/1482)

### Locale Package ID Normalization

Notable package ID changes include:

- `Humanizer.Core.bn-BD` -> `Humanizer.Core.bn`
- `Humanizer.Core.fi-FI` -> `Humanizer.Core.fi`
- `Humanizer.Core.ko-KR` -> `Humanizer.Core.ko`
- `Humanizer.Core.ms-MY` -> `Humanizer.Core.ms`
- `Humanizer.Core.th-TH` -> `Humanizer.Core.th`

Related:
- [commit 7b14ef6f](https://github.com/Humanizr/Humanizer/commit/7b14ef6f)
- Upgrade impact tracking: [#1656](https://github.com/Humanizr/Humanizer/issues/1656)

## Behavior Changes and Fixes

### Behavioral changes introduced in early v3

1. `Pascalize` treats `-` as a delimiter.
2. `Dehumanize` output differences can occur because it depends on `Humanize().Pascalize()`.
3. `Humanize`/`Titleize` preserve input with no recognized letters.

Related:
- [#1282](https://github.com/Humanizr/Humanizer/issues/1282), [PR #1299](https://github.com/Humanizr/Humanizer/pull/1299)
- [#1668](https://github.com/Humanizr/Humanizer/issues/1668)
- [#385](https://github.com/Humanizr/Humanizer/issues/385), [PR #1611](https://github.com/Humanizr/Humanizer/pull/1611)

### Fixed in v3 patch releases

- Analyzer loading compatibility fixes were delivered in `3.0.4`:
  [#1655](https://github.com/Humanizr/Humanizer/issues/1655),
  [#1665](https://github.com/Humanizr/Humanizer/issues/1665),
  [#1672](https://github.com/Humanizr/Humanizer/issues/1672),
  via [PR #1676](https://github.com/Humanizr/Humanizer/pull/1676).
- `ToQuantity(int, ...)` compatibility regression fixed for v3 patch line in `3.0.6` via [PR #1679](https://github.com/Humanizr/Humanizer/pull/1679) (issue [#1652](https://github.com/Humanizr/Humanizer/issues/1652)).
- `TitleCase` first-word article/conjunction/preposition handling fixed in `3.0.6` via [PR #1678](https://github.com/Humanizr/Humanizer/pull/1678) (issue [#1658](https://github.com/Humanizr/Humanizer/issues/1658)).

## Remaining Open Upgrade Issue (as of March 5, 2026)

| Issue | Status | Notes |
| --- | --- | --- |
| [#1668](https://github.com/Humanizr/Humanizer/issues/1668) | Open | `Dehumanize` differences involving underscores/digits remain under discussion. |
