# Fix for Issue #385: Titleize returns empty if text contains no known letters

## Problem

When `Titleize()` was called on strings containing no ASCII letters (e.g., Cyrillic "Майк", special characters "@@"), the method would either throw an `InvalidOperationException` or return an empty string.

## Root Cause

The `FromPascalCase()` method uses a regex to match word patterns. When the input contains only characters that don't match any pattern (like `@@` or `?`), the regex returns no matches, resulting in an empty string after the `string.Join()`.

## Solution

Added a check in `FromPascalCase()` to return the original input when no regex matches are found (when `result.Length == 0`). This ensures that inputs with no recognized letters are preserved as-is.

## Changes

### 1. Fixed `StringHumanizeExtensions.cs`
```csharp
// If no matches found, return the original input
if (result.Length == 0)
{
    return input;
}
```

### 2. Updated existing tests in `StringHumanizeTests.cs`
Changed expectations for special-character-only inputs to preserve the original:
- `"?)@"` now returns `"?)@"` instead of `""`
- `"?"` now returns `"?"` instead of `""`

### 3. Added new tests in `InflectorTests.cs`
New test method `TitleizeShouldPreserveUnrecognizedCharacters()` validates:
- Cyrillic: `"Майк".Titleize()` → `"Майк"`
- Special chars: `"@@".Titleize()` → `"@@"`
- Single char: `"?".Titleize()` → `"?"`
- Numbers: `"123".Titleize()` → `"123"`

## Validation

Created and ran comprehensive standalone tests verifying:
- ✅ Cyrillic text is preserved
- ✅ Arabic text is preserved
- ✅ Special characters are preserved when they're the only content
- ✅ Numbers are preserved
- ✅ Existing PascalCase functionality still works correctly
- ✅ All caps are still preserved
- ✅ Special chars are still stripped when mixed with letters

## Backward Compatibility

This change modifies the behavior for inputs with no recognized letters:
- **Before**: `"@@".Humanize()` → `""` (empty string)
- **After**: `"@@".Humanize()` → `"@@"` (original preserved)

As discussed in issue #385, the consensus among maintainers was that "any unrecognized characters should be left as is" (@mexx, @MehdiK). Since the previous behavior was considered a bug, this fix improves the library's behavior without breaking legitimate use cases.
