# Humanizer 3.0 Breaking Changes - Quick Reference

**For:** @clairernovotny  
**Date:** October 17, 2025  
**Status:** Analysis Complete ✅

---

## TL;DR - Key Findings

### ✅ Good News

1. **No critical breaking changes** - Both flagged critical issues were false positives
2. **Most changes are namespace consolidation** - ~80% of reported changes
3. **Binary compatibility is achievable** - Use `[TypeForwardedTo]` attributes
4. **API surface grew** - Net +20 APIs added (529 → 549)

### ⚠️ Action Required

1. **Add type forwarding attributes** for binary compatibility
2. **Create migration guide** documenting namespace changes
3. **Verify ~80 high-severity APIs** (likely false positives, but worth spot-checking)
4. **Update documentation** for v3.0 release

---

## The Main Change: Namespace Consolidation

**Commit:** `00bdc00b` by Simon Cropp (Feb 16, 2024)  
**PR:** #1351

### What Changed

**11 sub-namespaces merged into root `Humanizer` namespace:**

```
Humanizer.Bytes                         → Humanizer (30 members)
Humanizer.Localisation                  → Humanizer (17 members)
Humanizer.Localisation.Formatters       → Humanizer (10 members)
Humanizer.Localisation.NumberToWords    → Humanizer  (9 members)
Humanizer.DateTimeHumanizeStrategy      → Humanizer  (8 members)
[... 6 more namespaces with 19 total members]
```

### User Impact

**Before (v2.x):**
```csharp
using Humanizer;
using Humanizer.Bytes;
using Humanizer.Localisation;
```

**After (v3.0):**
```csharp
using Humanizer;  // That's it!
```

---

## Analysis Results Summary

| Category | Count | % of API |
|----------|-------|----------|
| **Total APIs (v2.14.1)** | 529 | 100% |
| **Total APIs (main)** | 549 | 104% |
| **Namespace-only changes** | 93 | 17.6% |
| **Reported "breaking changes"** | 343 | 64.8% |
| **Likely false positives** | ~275 | ~52% |
| **True breaking changes** | ~68* | ~13% |

\* Estimated after accounting for namespace reference changes in signatures

---

## Severity Breakdown

| Severity | Raw Count | Est. After Verification |
|----------|-----------|------------------------|
| Critical | 2 | 0 (verified as false positives) |
| High | 100 | ~20 (most are namespace ref changes) |
| Medium | 249 | ~40 (most are namespace ref changes) |
| Low | 86 | ~8 (minimal impact) |

---

## Recommended Actions (Priority Order)

### 1. Binary Compatibility (High Priority)

Add `[TypeForwardedTo]` attributes for all moved types:

```csharp
// In Humanizer.Bytes namespace (old location)
[assembly: TypeForwardedTo(typeof(Humanizer.ByteSize))]
[assembly: TypeForwardedTo(typeof(Humanizer.ByteRate))]

// In Humanizer.Localisation namespace
[assembly: TypeForwardedTo(typeof(Humanizer.TimeUnit))]
[assembly: TypeForwardedTo(typeof(Humanizer.IFormatter))]
// ... etc for all 93 moved types
```

**Impact:** Allows existing compiled assemblies to work without recompilation

### 2. Migration Guide (High Priority)

Create document covering:
- Namespace consolidation changes
- Simple find/replace for using statements
- Example before/after code
- Notes on nullable reference types

### 3. Spot-Check High-Severity APIs (Medium Priority)

Verify these commonly-used extension methods:
- `string.Humanize()` overloads
- `DateTime.Humanize()` overloads  
- `TimeSpan.Humanize()` overloads
- `int.ToWords()` / `ToOrdinalWords()`
- `enum.Humanize()`
- String inflectors (`Pluralize`, `Singularize`, etc.)

**Expected:** All still present, just with updated namespace references

### 4. Release Notes (High Priority)

Document:
- Namespace consolidation (breaking for source, not binary with forwarding)
- Nullable reference types enabled
- Net +20 new APIs
- Migration guide link

---

## Files Provided

1. **`BREAKING_CHANGES_ANALYSIS.md`** (14 KB)
   - Comprehensive analysis report
   - Detailed methodology
   - Full findings and recommendations

2. **`breaking_changes_detailed_output.txt`** (35 KB)
   - Raw console output from analysis tool
   - Complete list of all flagged changes
   - Grouped by severity and type

---

## Questions for Review

1. **Type forwarding:** Approve adding `[TypeForwardedTo]` attributes?
2. **Migration timeline:** When should migration guide be ready?
3. **Preview releases:** RC/beta for community feedback before 3.0?
4. **Compatibility level:** Accept source-breaking but maintain binary compat?
5. **Verification:** Want manual spot-check of specific high-severity APIs?

---

## Next Steps

After review and approval:

1. ✅ Analysis complete (this document)
2. ⬜ Add `[TypeForwardedTo]` attributes (separate PR)
3. ⬜ Create migration guide
4. ⬜ Update release notes
5. ⬜ Spot-check high-priority APIs if requested
6. ⬜ Beta/RC release for community testing

---

## Analysis Methodology

**Tool:** Custom Python script comparing API approval test files  
**Source Data:**
- v2.14.1: `src/Humanizer.Tests/ApiApprover/PublicApiApprovalTest.approve_public_api.approved.txt`
- main: `src/Humanizer.Tests/ApiApprover/PublicApiApprovalTest.Approve_Public_Api.DotNet10_0.verified.txt`

**Limitations:**
- Signature comparison flagged namespace reference changes as "removed API"
- Manual verification required to distinguish true removals from false positives
- Nullable annotation differences treated as signature changes

**Validation:**
- Manual verification of critical items (ByteRate.Humanize methods) confirmed false positives
- Git history analysis identified key commit (#1351 - namespace consolidation)

---

**End of Quick Reference**
