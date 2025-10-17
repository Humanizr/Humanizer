# Breaking Changes in Humanizer 3.0

This document describes breaking changes between Humanizer v2.x and v3.0, and provides guidance on how to migrate your code.

## Table of Contents
- [Overview](#overview)
- [Namespace Consolidation](#namespace-consolidation)
- [Extension Methods](#extension-methods)
- [Nullable Reference Types](#nullable-reference-types)
- [Migration Guide](#migration-guide)
- [FAQ](#faq)

---

## Overview

Humanizer 3.0 introduces a major simplification of the namespace structure by consolidating 11 sub-namespaces into the root `Humanizer` namespace. This is a **source-breaking change** that requires updating `using` statements in your code.

### Key Changes

- **Namespace consolidation**: 93 types moved from sub-namespaces to root `Humanizer` namespace
- **Nullable reference types**: Enabled across the codebase for better null safety
- **API surface growth**: Net +20 new APIs added (529 → 549 members, +3.8%)
- **No critical API removals**: All major APIs remain available with updated signatures

### Impact Assessment

| Change Type | Count | Impact Level |
|-------------|-------|--------------|
| Namespace-only changes | 93 types | **Source-breaking** (requires using statement updates) |
| Nullable annotations | Various APIs | **Warning-level** (improves null safety, may produce warnings) |
| Binary compatibility | Maintained via facade | **Compatible** (recompilation optional) |

---

## Namespace Consolidation

### What Changed

In v2.x, Humanizer used multiple sub-namespaces to organize types. In v3.0, all types have been moved to the root `Humanizer` namespace for simplicity.

### Affected Namespaces

The following namespaces have been consolidated into `Humanizer`:

| Old Namespace (v2.x) | New Namespace (v3.0) | Types Affected |
|---------------------|----------------------|----------------|
| `Humanizer.Bytes` | `Humanizer` | ByteRate, ByteSize |
| `Humanizer.Configuration` | `Humanizer` | LocaliserRegistry<T> |
| `Humanizer.DateTimeHumanizeStrategy` | `Humanizer` | 12 strategy classes and interfaces |
| `Humanizer.Inflections` | `Humanizer` | Vocabulary |
| `Humanizer.Localisation` | `Humanizer` | DataUnit, Tense, TimeUnit |
| `Humanizer.Localisation.CollectionFormatters` | `Humanizer` | ICollectionFormatter |
| `Humanizer.Localisation.DateToOrdinalWords` | `Humanizer` | IDateOnlyToOrdinalWordConverter, IDateToOrdinalWordConverter |
| `Humanizer.Localisation.Formatters` | `Humanizer` | DefaultFormatter, IFormatter |
| `Humanizer.Localisation.NumberToWords` | `Humanizer` | INumberToWordsConverter |
| `Humanizer.Localisation.Ordinalizers` | `Humanizer` | IOrdinalizer |
| `Humanizer.Localisation.TimeToClockNotation` | `Humanizer` | ITimeOnlyToClockNotationConverter |

### Complete Type List

#### Humanizer.Bytes → Humanizer
- `ByteRate` (class)
- `ByteSize` (struct)

#### Humanizer.Configuration → Humanizer
- `LocaliserRegistry<TLocaliser>` (class)

#### Humanizer.DateTimeHumanizeStrategy → Humanizer
- `DefaultDateOnlyHumanizeStrategy` (class)
- `DefaultDateTimeHumanizeStrategy` (class)
- `DefaultDateTimeOffsetHumanizeStrategy` (class)
- `DefaultTimeOnlyHumanizeStrategy` (class)
- `IDateOnlyHumanizeStrategy` (interface)
- `IDateTimeHumanizeStrategy` (interface)
- `IDateTimeOffsetHumanizeStrategy` (interface)
- `ITimeOnlyHumanizeStrategy` (interface)
- `PrecisionDateOnlyHumanizeStrategy` (class)
- `PrecisionDateTimeHumanizeStrategy` (class)
- `PrecisionDateTimeOffsetHumanizeStrategy` (class)
- `PrecisionTimeOnlyHumanizeStrategy` (class)

#### Humanizer.Inflections → Humanizer
- `Vocabulary` (class)

#### Humanizer.Localisation → Humanizer
- `DataUnit` (enum)
- `Tense` (enum)
- `TimeUnit` (enum)

#### Humanizer.Localisation.CollectionFormatters → Humanizer
- `ICollectionFormatter` (interface)

#### Humanizer.Localisation.DateToOrdinalWords → Humanizer
- `IDateOnlyToOrdinalWordConverter` (interface)
- `IDateToOrdinalWordConverter` (interface)

#### Humanizer.Localisation.Formatters → Humanizer
- `DefaultFormatter` (class)
- `IFormatter` (interface)

#### Humanizer.Localisation.NumberToWords → Humanizer
- `INumberToWordsConverter` (interface)

#### Humanizer.Localisation.Ordinalizers → Humanizer
- `IOrdinalizer` (interface)

#### Humanizer.Localisation.TimeToClockNotation → Humanizer
- `ITimeOnlyToClockNotationConverter` (interface)

---

## Extension Methods

Extension methods (static extension classes) remain in the `Humanizer` namespace and are **not affected** by this change. Your existing code using extension methods will continue to work without modification once you update your `using` statements.

### Examples of Extension Methods (No Breaking Changes)

```csharp
using Humanizer;  // Only this using is needed

// All extension methods work as before
var text = "PascalCaseString".Humanize();
var plural = "cat".Pluralize();
var ordinal = 1.Ordinalize();
var words = 123.ToWords();
var humanized = DateTime.Now.Humanize();
var bytes = 1024.Bytes();
var timespan = 3.Hours();
```

---

## Nullable Reference Types

Humanizer 3.0 has nullable reference types (NRT) enabled. This means:

- Reference type parameters and return values may be annotated with `?` for nullable
- Your code may generate new nullability warnings when compiled with NRT enabled
- These are **compiler warnings only** - code functionality is unchanged
- This improves code safety by making null expectations explicit

### Example

```csharp
// v2.x
public static string Humanize(this string input, string format) { }

// v3.0
public static string? Humanize(this string? input, string? format = null) { }
```

### Handling Nullable Warnings

If you see new warnings after upgrading:

1. **Enable nullable reference types** in your project to match Humanizer's nullability contracts
2. **Suppress warnings** if you're not ready to adopt NRT: `#pragma warning disable CS8600`
3. **Update your code** to handle nulls appropriately where Humanizer indicates nullability

---

## Migration Guide

### Step 1: Update Using Statements

The simplest and most common migration is updating your `using` statements.

#### Before (v2.x)

```csharp
using Humanizer;
using Humanizer.Bytes;
using Humanizer.Localisation;
using Humanizer.Localisation.Formatters;
using Humanizer.Configuration;
```

#### After (v3.0)

```csharp
using Humanizer;  // That's all you need!
```

### Step 2: Update Type References

If you have fully-qualified type references, update them:

#### Before (v2.x)

```csharp
Humanizer.Bytes.ByteSize size = 1024.Bytes();
Humanizer.Localisation.TimeUnit unit = Humanizer.Localisation.TimeUnit.Second;
var formatter = new Humanizer.Localisation.Formatters.DefaultFormatter(culture);
```

#### After (v3.0)

```csharp
Humanizer.ByteSize size = 1024.Bytes();
Humanizer.TimeUnit unit = Humanizer.TimeUnit.Second;
var formatter = new Humanizer.DefaultFormatter(culture);
```

### Step 3: Recompile Your Application

**Good News**: Assemblies compiled against Humanizer v2.x will work with v3.0 **without recompilation**. Binary compatibility is maintained through a facade assembly (`Humanizer.Core.dll`) that contains TypeForwardedTo attributes redirecting old namespace references to the new `Humanizer.dll` assembly.

### Step 4: Address Nullable Warnings (Optional)

If you have nullable reference types enabled, review and address any new warnings:

```csharp
// If Humanizer indicates a parameter can be null:
string? result = someString.Humanize();

// Add null checks if needed:
if (result != null)
{
    Console.WriteLine(result);
}
```

### Automated Migration

#### Find and Replace

You can use find-and-replace in your IDE:

**Find**: `using Humanizer.(Bytes|Configuration|Inflections|Localisation|DateTimeHumanizeStrategy);`  
**Replace**: `// Removed - types now in Humanizer namespace`  
**Use Regular Expressions**: ✓

Then manually review and ensure you have `using Humanizer;` at the top.

#### Example PowerShell Script

```powershell
# Find all C# files with old namespace usings
Get-ChildItem -Recurse -Filter *.cs | ForEach-Object {
    $content = Get-Content $_.FullName -Raw
    $updated = $content `
        -replace 'using Humanizer\.Bytes;', '// using Humanizer.Bytes; // Moved to Humanizer in v3.0' `
        -replace 'using Humanizer\.Configuration;', '// using Humanizer.Configuration; // Moved to Humanizer in v3.0' `
        -replace 'using Humanizer\.Localisation.*;', '// using Humanizer.Localisation.*; // Moved to Humanizer in v3.0' `
        -replace 'using Humanizer\.Inflections;', '// using Humanizer.Inflections; // Moved to Humanizer in v3.0' `
        -replace 'using Humanizer\.DateTimeHumanizeStrategy;', '// using Humanizer.DateTimeHumanizeStrategy; // Moved to Humanizer in v3.0'
    
    if ($content -ne $updated) {
        Set-Content -Path $_.FullName -Value $updated -NoNewline
        Write-Host "Updated: $($_.FullName)"
    }
}
```

---

## FAQ

### Q: Do I need to update my code if I only use extension methods?

**A:** Yes, but only to update your `using` statements. Change from multiple `using` statements to just `using Humanizer;`. The extension methods themselves haven't changed.

### Q: Will my compiled application work with Humanizer 3.0 without recompiling?

**A:** Yes! Binary compatibility is maintained through a facade assembly. The Humanizer.Core package includes:
- `Humanizer.dll` - Main implementation with all types in the root `Humanizer` namespace
- `Humanizer.Core.dll` - Facade assembly with TypeForwardedTo attributes that redirect old namespace references

Applications compiled against v2.x will work without recompilation. However, recompiling is still recommended to take advantage of updated signatures and nullable reference types.

### Q: How does the TypeForwardedTo facade work?

**A:** The solution uses a clever assembly naming strategy:

1. The main assembly is named `Humanizer.dll` (contains all implementations)
2. A facade assembly named `Humanizer.Core.dll` contains only TypeForwardedTo attributes
3. Both assemblies are included in the Humanizer.Core NuGet package

When v2.x compiled code references types like `[Humanizer.Core]Humanizer.Bytes.ByteSize`:
- The runtime loads `Humanizer.Core.dll`
- Finds the TypeForwardedTo attribute: `[assembly: TypeForwardedTo(typeof(Humanizer.ByteSize))]`
- Redirects to `[Humanizer]Humanizer.ByteSize` in the main assembly
- ✅ Binary compatibility achieved!

This approach maintains the same package name (Humanizer.Core) while leveraging the assembly boundary needed for TypeForwardedTo to function.

### Q: Are there any API removals I should know about?

**A:** The analysis found no critical API removals. All commonly-used extension methods like `Humanize()`, `Pluralize()`, `ToWords()`, etc. remain available. Some signatures may have nullable annotations added, but functionality is preserved.

### Q: What about custom implementations of Humanizer interfaces?

**A:** If you have custom implementations of Humanizer interfaces (like `IFormatter`, `INumberToWordsConverter`, etc.):

1. Update your `using` statements to `using Humanizer;`
2. Update your interface references to use the `Humanizer` namespace
3. Review nullable annotations on interface methods and update your implementations accordingly
4. Recompile your code

Example:

```csharp
// v2.x
using Humanizer.Localisation.Formatters;

public class MyFormatter : IFormatter { /* implementation */ }

// v3.0
using Humanizer;

public class MyFormatter : IFormatter { /* implementation with updated nullable annotations */ }
```

### Q: How can I test my migration before fully committing?

**A:** 

1. Create a test branch in your source control
2. Update Humanizer to v3.0
3. Use IDE's "Find All References" to locate all Humanizer usages
4. Update `using` statements
5. Recompile and run your tests
6. Address any nullable warnings
7. Once tests pass, merge to your main branch

### Q: Where can I get help if I encounter issues?

**A:** 

- **GitHub Issues**: [https://github.com/Humanizr/Humanizer/issues](https://github.com/Humanizr/Humanizer/issues)
- **Documentation**: [https://github.com/Humanizr/Humanizer](https://github.com/Humanizr/Humanizer)
- **Stack Overflow**: Tag your questions with `humanizer`

---

## Version History

- **v3.0.0**: Initial release with namespace consolidation
- **v2.14.1**: Last v2.x release before namespace consolidation

## Related Commits

- Namespace consolidation: [PR #1351](https://github.com/Humanizr/Humanizer/pull/1351) (commit `00bdc00b`)
- Author: Simon Cropp
- Date: February 16, 2024

---

## Summary

Humanizer 3.0's namespace consolidation simplifies the API surface and reduces the cognitive load of remembering which namespace each type belongs to. While this is a breaking change, the migration is straightforward:

**✓ Update your `using` statements from multiple namespaces to just `using Humanizer;`**  
**✓ Recompile your application**  
**✓ Address any nullable warnings (optional)**  
**✓ Test thoroughly**

The benefits of a cleaner, simpler namespace structure outweigh the one-time migration effort for this major version release.
