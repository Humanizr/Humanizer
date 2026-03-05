# Migrating from Humanizer 2.14.1 to 3.0.1

This guide is for teams upgrading directly from `2.14.1` to `3.0.1`.

Validated against:
- Git tags `v2.14.1` and `v3.0.1`
- Public issue reports through **March 5, 2026**

## Quick Upgrade Checklist

1. Update package/tooling prerequisites first (framework and restore requirements).
2. Run the namespace migration analyzer and replace old `using Humanizer.*` directives.
3. Replace removed APIs (`FormatWith`, obsolete `ToMetric` overloads, etc.).
4. Rebuild all assemblies that reference Humanizer (binary compatibility changed in a few APIs).
5. If you implement/extensibility points (`IFormatter`, `DefaultFormatter`), update those implementations.
6. Run behavioral regression tests for `Titleize`, `Pascalize`, `Dehumanize`, and enum humanization.

## Breaking Changes

### Namespace Consolidation (source-breaking)

All Humanizer APIs were consolidated into the root `Humanizer` namespace.

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

3. `ToQuantity(this string, int, ...)` overloads were removed.

- `long` and `double` overloads remain.
- This is a binary compatibility break for already-compiled assemblies that referenced the `int` signatures.

4. `EnglishArticles` enum was removed.

5. `Configurator.EnumDescriptionPropertyLocator` public property was removed.

Use `Configurator.UseEnumDescriptionPropertyLocator(...)` instead, and call it early during startup:

```csharp
Configurator.UseEnumDescriptionPropertyLocator(p => p.Name == "Info");
```

`UseEnumDescriptionPropertyLocator(...)` now throws if you call it after enum humanization has already occurred.

### Enum API Signature Changes

Enum APIs moved from `Enum`-based extension signatures to constrained generics:

Before (`2.14.1`):

```csharp
public static string Humanize(this Enum input)
public static string Humanize(this Enum input, LetterCasing casing)
public static TTargetEnum DehumanizeTo<TTargetEnum>(this string input)
    where TTargetEnum : struct, IComparable, IFormattable
```

After (`3.0.1`):

```csharp
public static string Humanize<T>(this T input) where T : struct, Enum
public static string Humanize<T>(this T input, LetterCasing casing) where T : struct, Enum
public static TTargetEnum DehumanizeTo<TTargetEnum>(this string input)
    where TTargetEnum : struct, Enum
```

Impact:

- Code that stores values as `Enum` (not a concrete enum type) and then calls `.Humanize()` no longer compiles.
- Generic callers with non-enum constraints for `DehumanizeTo<T>` no longer compile.

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

## Framework, Packaging, and Tooling Breaks

### Target Framework Support Changes

Compared to `2.14.1`, `3.0.1` removed support for:

- `netstandard1.0`
- `net462`
- `net472`
- dedicated `net6.0` assets (consumers on `net6.0`/`net7.0` now resolve `netstandard2.0` assets)

`3.0.1` package assets target:

- `netstandard2.0`
- `net48`
- `net8.0`
- `net10.0`

### Metapackage Restore Requirement

The `Humanizer` metapackage requires NuGet locale parsing support from newer tooling.

You need:

- .NET SDK `9.0.200+`, or
- Visual Studio/MSBuild that includes the same NuGet locale parsing fix.

On older tooling, restore can fail for the metapackage. Workaround: reference `Humanizer.Core` directly and install needed locale packages explicitly.

### Locale Package ID Changes

Several locale package IDs changed between `2.14.1` and `3.0.1`:

| `2.14.1` package | `3.0.1` package |
| --- | --- |
| `Humanizer.Core.bn-BD` | `Humanizer.Core.bn` |
| `Humanizer.Core.fi-FI` | `Humanizer.Core.fi` |
| `Humanizer.Core.ko-KR` | `Humanizer.Core.ko` |
| `Humanizer.Core.ms-MY` | `Humanizer.Core.ms` |
| `Humanizer.Core.th-TH` | `Humanizer.Core.th` |

Removed from the metapackage dependency list (no direct one-to-one replacement):

- `Humanizer.Core.fr-BE`
- `Humanizer.Core.nb-NO`

## Behavior Changes to Validate

1. `Pascalize` now treats hyphens (`-`) as delimiters.
2. `Dehumanize` output can differ because it is based on `Humanize().Pascalize()` and inherits `Pascalize` changes.
3. `Humanize` / `Titleize` preserve strings with no recognized letters instead of returning empty string.

## Known Regressions in 3.0.1 (status as of March 5, 2026)

| Issue | Status | Impact | Suggested mitigation |
| --- | --- | --- | --- |
| [#1652](https://github.com/Humanizr/Humanizer/issues/1652) | Open | `ToQuantity(int, ...)` overload removal is still an upgrade pain point (especially binary compatibility and exact-signature call sites). | Rebuild all dependent assemblies; switch call sites/delegates to `long` overloads explicitly. |
| [#1655](https://github.com/Humanizr/Humanizer/issues/1655) | Open | `Enum`-typed variables no longer support `.Humanize()` extension dispatch with the new generic signature. | Keep enum values strongly typed as concrete enum types where possible. |
| [#1658](https://github.com/Humanizr/Humanizer/issues/1658) | Open | `Transform(To.TitleCase)` can leave first-word articles/conjunctions/prepositions lowercase. | Add app-level casing normalization for the first word if this output matters. |
| [#1668](https://github.com/Humanizr/Humanizer/issues/1668) | Open | Some `Dehumanize()` cases retain underscore before digits (for example `everything_0`). | Pre-normalize affected inputs before `Dehumanize()`, or use custom conversion logic for these patterns. |
| [#1654](https://github.com/Humanizr/Humanizer/issues/1654) / [#1659](https://github.com/Humanizr/Humanizer/issues/1659) | Closed after `3.0.1` | Analyzer loading warnings/errors on some Roslyn/SDK combinations. | Upgrade SDK/Visual Studio toolchain; if needed, disable analyzers temporarily in build until tooling is updated. |

## Recommended Validation Pass

After migration:

1. Full clean restore with your actual CI SDK image.
2. Full rebuild of all projects that reference Humanizer.
3. Integration tests around string casing, enum formatting/dehumanization, and quantity formatting.
4. Spot-check localized output if you depended on renamed locale packages.
