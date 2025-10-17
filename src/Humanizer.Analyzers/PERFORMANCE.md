# Performance Optimizations for Humanizer.Analyzers

## Summary of Changes

This document outlines the performance optimizations made to the Humanizer.Analyzers project to improve analyzer and code fix performance.

## Optimizations Applied

### 1. **Static Methods** (Analyzer)
- Changed `AnalyzeUsingDirective` and `AnalyzeQualifiedName` from instance methods to `static`
- **Benefit**: Avoids unnecessary allocations and potential closure captures

### 2. **StringComparer.Ordinal in ImmutableHashSet**
- Added `StringComparer.Ordinal` to `ImmutableHashSet.Create()`
- **Benefit**: Uses faster ordinal comparison instead of culture-aware comparison

### 3. **Eliminated String Concatenation in Hot Path**
- Replaced `fullName.StartsWith(oldNamespace + ".")` with span-based comparison
- **Benefit**: Avoids allocating a new string for every comparison

### 4. **Span-Based String Matching**
- Implemented `IsNamespaceMatch()` method using `AsSpan()` and indexed access
- Uses `fullName[oldNamespace.Length] == '.'` check before span comparison
- **Benefit**: Zero-allocation namespace prefix matching

### 5. **Collection Initializer Syntax** (CodeFixProvider)
- Used modern collection expression syntax `[...]` for array initialization
- **Benefit**: More readable and potentially better JIT optimization

### 6. **Lambda Optimization**
- Changed lambda in `Any()` to `static` lambda
- **Benefit**: Prevents closure allocation when lambda doesn't capture variables

### 7. **Reduced Allocations in GetReplacementName**
- Simplified string building logic
- Uses single concatenation instead of multiple operations
- **Benefit**: Minimizes temporary string allocations

### 8. **Polyfill Package**
- Added `Polyfill` package reference with `PrivateAssets="all"`
- **Benefit**: Enables use of modern APIs like `AsSpan()` on netstandard2.0 target

### 9. **Pre-Ordered Namespace Array**
- Ordered namespaces by length (longest first) in CodeFixProvider
- **Benefit**: Ensures most specific namespaces are matched first, avoiding incorrect replacements

## Performance Impact

### Before Optimizations:
- String allocations in every qualified name check
- Culture-aware string comparisons
- Closure allocations in lambdas

### After Optimizations:
- Zero-allocation namespace matching using spans
- Fast ordinal comparisons
- Static methods and lambdas avoid closures
- Minimal string allocations

## Target Framework

While multi-targeting to net10.0 was considered, Roslyn analyzers must target `netstandard2.0` only (per RS1041). However, we still achieve significant performance gains using:
- Modern C# features (via `LangVersion>latest`)
- Polyfill package for span APIs on netstandard2.0
- Optimized algorithms and data structures

## Testing

All optimizations were verified to:
1. ✅ Build successfully with 0 warnings/errors
2. ✅ Maintain backward compatibility
3. ✅ Work correctly with existing test infrastructure
4. ✅ Properly package into Humanizer.Core NuGet

## Code Quality

- Maintained code readability
- Added inline comments explaining optimization techniques
- Followed existing code style and conventions
- Added `nullable` enable for better null safety
