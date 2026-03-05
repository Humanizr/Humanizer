# Migrating from Humanizer 2.14.1 to 3.0.8

This guide is for teams upgrading directly from `2.14.1` to `3.0.8`.

This document was added to address [issue #1656](https://github.com/Humanizr/Humanizer/issues/1656) (undocumented v3 breaking changes).

Validated against:
- Git tag `v2.14.1`
- v3 breaking-change commits through `v3.0.1`
- v3 patch-line fixes included in `3.0.8`:
  - Roslyn analyzer compatibility fixes in [PR #1676](https://github.com/Humanizr/Humanizer/pull/1676)
  - `ToQuantity(int, ...)` compatibility fix in [PR #1679](https://github.com/Humanizr/Humanizer/pull/1679)
  - `TitleCase` first-word capitalization fix in [PR #1678](https://github.com/Humanizr/Humanizer/pull/1678)

## Quick Upgrade Checklist

1. Update package/tooling prerequisites first (framework and restore requirements).
2. If you are on `3.0.1`, upgrade to `3.0.8` to pick up patch-line compatibility fixes ([#1655](https://github.com/Humanizr/Humanizer/issues/1655), [#1665](https://github.com/Humanizr/Humanizer/issues/1665), [#1672](https://github.com/Humanizr/Humanizer/issues/1672), [#1652](https://github.com/Humanizr/Humanizer/issues/1652), [#1658](https://github.com/Humanizr/Humanizer/issues/1658)).
3. Run the namespace migration analyzer and replace old `using Humanizer.*` directives.
4. Replace removed APIs (`FormatWith`, obsolete `ToMetric` overloads, etc.).
5. Rebuild all assemblies that reference Humanizer (binary compatibility changed in a few APIs).
6. If you implement extensibility points (`IFormatter`, `DefaultFormatter`), update those implementations.
7. Run behavioral regression tests for `Titleize`, `Pascalize`, `Dehumanize`, and enum humanization.

## Breaking Changes

### Namespace Consolidation (source-breaking)

All Humanizer APIs were consolidated into the root `Humanizer` namespace.

Related:
- [PR #1351](https://github.com/Humanizr/Humanizer/pull/1351)
- Upgrade impact report: [issue #1656](https://github.com/Humanizr/Humanizer/issues/1656)

Before:

```csharp
using Humanizer.Bytes;
using Humanizer.Localisation;
using Humanizer.Configuration;
```

After:

```csharp
using Humanizer;
```

Use the built-in analyzer (`HUMANIZER001`) to automate this migration.

See also: [Namespace-only migration guide](v3-namespace-migration.md).

### Removed APIs

1. `StringExtensions.FormatWith(...)` was removed.

Related:
- [PR #1395](https://github.com/Humanizr/Humanizer/pull/1395)
- Upgrade impact report: [issue #1656](https://github.com/Humanizr/Humanizer/issues/1656)

Before:

```csharp
"{0:N2}".FormatWith(culture, value);
```

After:

```csharp
string.Format(culture, "{0:N2}", value);
```

2. Obsolete `ToMetric` overloads were removed:

- `ToMetric(this int input, bool hasSpace, bool useSymbol = true, int? decimals = null)`
- `ToMetric(this double input, bool hasSpace, bool useSymbol = true, int? decimals = null)`

Use `MetricNumeralFormats` instead:

```csharp
value.ToMetric(MetricNumeralFormats.WithSpace | MetricNumeralFormats.UseName, decimals: 2);
```

Related:
- [PR #1389](https://github.com/Humanizr/Humanizer/pull/1389)
- Upgrade impact report: [issue #1656](https://github.com/Humanizr/Humanizer/issues/1656)

3. `ToQuantity(this string, int, ...)` overloads were removed in early v3 and later restored in the patch line (`3.0.6+`, including `3.0.8`).

Related:
- [PR #1338](https://github.com/Humanizr/Humanizer/pull/1338)
- Follow-up request and resolution: [issue #1652](https://github.com/Humanizr/Humanizer/issues/1652), [PR #1679](https://github.com/Humanizr/Humanizer/pull/1679)

4. `EnglishArticles` enum was removed.

Related:
- [PR #1443](https://github.com/Humanizr/Humanizer/pull/1443)

5. `Configurator.EnumDescriptionPropertyLocator` public property was removed.

Use `Configurator.UseEnumDescriptionPropertyLocator(...)` instead, and call it early during startup:

```csharp
Configurator.UseEnumDescriptionPropertyLocator(p => p.Name == "Info");
```

`UseEnumDescriptionPropertyLocator(...)` now throws if you call it after enum humanization has already occurred.

Related:
- Upgrade impact report: [issue #1656](https://github.com/Humanizr/Humanizer/issues/1656)

### Enum API Signature Changes

Enum APIs moved from `Enum`-based extension signatures to constrained generics:

Before (`2.14.1`):

```csharp
public static string Humanize(this Enum input)
public static string Humanize(this Enum input, LetterCasing casing)
public static TTargetEnum DehumanizeTo<TTargetEnum>(this string input)
    where TTargetEnum : struct, IComparable, IFormattable
```

After (`3.0.8`):

```csharp
public static string Humanize<T>(this T input) where T : struct, Enum
public static string Humanize<T>(this T input, LetterCasing casing) where T : struct, Enum
public static TTargetEnum DehumanizeTo<TTargetEnum>(this string input)
    where TTargetEnum : struct, Enum
```

Impact:

- Code that stores values as `Enum` (not a concrete enum type) and then calls `.Humanize()` no longer compiles.
- Generic callers with non-enum constraints for `DehumanizeTo<T>` no longer compile.

Related:
- Upgrade impact report: [issue #1656](https://github.com/Humanizr/Humanizer/issues/1656)

### Extensibility Breaks (`IFormatter` / `DefaultFormatter`)

If you implement or subclass formatting infrastructure, update your code:

1. `IFormatter` now requires:

```csharp
string TimeSpanHumanize_Age();
```

2. `DefaultFormatter` override surface changed:

Before:

```csharp
protected virtual string Format(string resourceKey, int number, bool toWords = false)
```

After:

```csharp
protected virtual string Format(TimeUnit unit, string resourceKey, int number, bool toWords = false)
```

If you had custom `DefaultFormatter` subclasses overriding the old signature, they must be migrated.

Related:
- API introduction that expanded formatter contract: [PR #1068](https://github.com/Humanizr/Humanizer/pull/1068)
- Upgrade impact report: [issue #1656](https://github.com/Humanizr/Humanizer/issues/1656)

## Framework, Packaging, and Tooling Breaks

### Target Framework Support Changes

Compared to `2.14.1`, v3 removed support for:

- `netstandard1.0`
- `net462`
- `net472`
- dedicated `net6.0` assets (consumers on `net6.0`/`net7.0` now resolve `netstandard2.0` assets)

`3.0.8` package assets target:

- `netstandard2.0`
- `net48`
- `net8.0`
- `net10.0`

Related:
- `netstandard1.0` removal: [PR #1322](https://github.com/Humanizr/Humanizer/pull/1322)
- `net462`/`net472` removal: [PR #1482](https://github.com/Humanizr/Humanizer/pull/1482)
- Upgrade impact report: [issue #1656](https://github.com/Humanizr/Humanizer/issues/1656)

### Metapackage Restore Requirement

The `Humanizer` metapackage requires NuGet locale parsing support from newer tooling.

You need:

- .NET SDK `9.0.200+`, or
- Visual Studio/MSBuild that includes the same NuGet locale parsing fix.

On older tooling, restore can fail for the metapackage. Workaround: reference `Humanizer.Core` directly and install needed locale packages explicitly.

Related:
- NuGet locale parsing fix dependency note: [NuGet.Client discussion](https://github.com/NuGet/NuGet.Client/pull/6124#issuecomment-3391090183)

### Locale Package ID Changes

Several locale package IDs changed between `2.14.1` and `3.0.8`:

| `2.14.1` package | `3.0.8` package |
| --- | --- |
| `Humanizer.Core.bn-BD` | `Humanizer.Core.bn` |
| `Humanizer.Core.fi-FI` | `Humanizer.Core.fi` |
| `Humanizer.Core.ko-KR` | `Humanizer.Core.ko` |
| `Humanizer.Core.ms-MY` | `Humanizer.Core.ms` |
| `Humanizer.Core.th-TH` | `Humanizer.Core.th` |

Removed from the metapackage dependency list (no direct one-to-one replacement):

- `Humanizer.Core.fr-BE`
- `Humanizer.Core.nb-NO`

Related:
- Locale ID normalization change: [commit 7b14ef6f](https://github.com/Humanizr/Humanizer/commit/7b14ef6f)
- Upgrade impact report: [issue #1656](https://github.com/Humanizr/Humanizer/issues/1656)

## Behavior Changes to Validate

1. `Pascalize` now treats hyphens (`-`) as delimiters.
2. `Dehumanize` output can differ because it is based on `Humanize().Pascalize()` and inherits `Pascalize` changes.
3. `Humanize` / `Titleize` preserve strings with no recognized letters instead of returning empty string.

Related:
- `Pascalize` hyphen behavior change: [issue #1282](https://github.com/Humanizr/Humanizer/issues/1282), [PR #1299](https://github.com/Humanizr/Humanizer/pull/1299)
- `Dehumanize`/spacing impact reports: [issue #1656](https://github.com/Humanizr/Humanizer/issues/1656), [issue #1668](https://github.com/Humanizr/Humanizer/issues/1668)
- `Titleize` no-letter preservation: [issue #385](https://github.com/Humanizr/Humanizer/issues/385), [PR #1611](https://github.com/Humanizr/Humanizer/pull/1611)
- `TitleCase` first-word casing regression: [issue #1658](https://github.com/Humanizr/Humanizer/issues/1658)

## Roslyn Analyzer Fixes (Included in 3.0.8 via 3.0.4)

`3.0.8` includes analyzer loading compatibility fixes from [PR #1676](https://github.com/Humanizr/Humanizer/pull/1676), originally delivered in `3.0.4`.

| Issue | Status | Impact in 3.0.1 | 3.0.8 result |
| --- | --- | --- | --- |
| [#1655](https://github.com/Humanizr/Humanizer/issues/1655) | Closed | Analyzer could fail to load on .NET 8 SDK hosts. | Fixed |
| [#1665](https://github.com/Humanizr/Humanizer/issues/1665) | Closed | Analyzer load failure due to `System.Memory` binding mismatch. | Fixed |
| [#1672](https://github.com/Humanizr/Humanizer/issues/1672) | Closed | Analyzer load failure due to `System.Collections.Immutable` dependency mismatch. | Fixed |

## Compatibility Fixes Included in 3.0.8

| Issue | Status | Patch-line fix |
| --- | --- | --- |
| [#1652](https://github.com/Humanizr/Humanizer/issues/1652) | Closed | `ToQuantity(int, ...)` compatibility restored via [PR #1679](https://github.com/Humanizr/Humanizer/pull/1679) (in `3.0.6+`, included in `3.0.8`). |
| [#1658](https://github.com/Humanizr/Humanizer/issues/1658) | Closed | `TitleCase` first-word capitalization fixed via [PR #1678](https://github.com/Humanizr/Humanizer/pull/1678) (in `3.0.6+`, included in `3.0.8`). |

## Remaining Known Upgrade Issue (as of March 5, 2026)

| Issue | Status | Impact | Suggested mitigation |
| --- | --- | --- | --- |
| [#1668](https://github.com/Humanizr/Humanizer/issues/1668) | Open | Some `Dehumanize()` cases retain underscore before digits (for example `everything_0`). | Pre-normalize affected inputs before `Dehumanize()`, or use custom conversion logic for these patterns. |

## Recommended Validation Pass

After migration:

1. Full clean restore with your actual CI SDK image.
2. Full rebuild of all projects that reference Humanizer.
3. Integration tests around string casing, enum formatting/dehumanization, and quantity formatting.
4. Spot-check localized output if you depended on renamed locale packages.
