# Sample Impact Analysis Summary

This is an example of what the IMPACT_SUMMARY.md report looks like after running the analysis.

---

# Humanizer v3 Namespace Consolidation - Impact Analysis Report

**Analysis Date:** 2025-10-17 06:35:00 UTC

## Executive Summary

- **Total Repositories Analyzed:** 347
- **Total Namespace Matches:** 1,248
- **Total NuGet Dependent Packages:** 89
- **Total NuGet Downloads (Dependents):** 15,234,567

## Top 10 Most Impacted Repositories and Packages

### 1. Example.Framework/Core

- **Type:** Repository
- **Impact Category:** Critical
- **Impact Score:** 99.8th percentile
- **Stars:** 12,456
- **Forks:** 1,234
- **Used By:** 567 repositories
- **Namespace Occurrences:** 47
- **Last Updated:** 12 days ago
- **Public API Exposure:** 100%
- **Classification:** LibraryPublicApi

**Recommended Actions:** Immediate coordination required. Contact maintainers to plan coordinated v3 release. Offer Roslyn analyzer with automatic code fix. Consider temporary compatibility adapter package.

**Sample Matches:**
- [src/Services/HumanizerService.cs](https://github.com/Example.Framework/Core/blob/main/src/Services/HumanizerService.cs#L15)
  ```
  using System;
  using Humanizer.Bytes;
  using Humanizer.Localisation;
  ```

### 2. Popular.Library/Utilities

- **Type:** Package
- **Impact Category:** Critical
- **Impact Score:** 98.5th percentile
- **Total Downloads:** 8,456,789
- **Package Dependents:** 234
- **Namespace Occurrences:** 23
- **Last Updated:** 45 days ago
- **Public API Exposure:** 100%
- **Classification:** LibraryPublicApi

**Recommended Actions:** Immediate coordination required. Contact maintainers to plan coordinated v3 release. Offer Roslyn analyzer with automatic code fix. Consider temporary compatibility adapter package.

### 3. MegaCorp/InternalFramework

- **Type:** Repository
- **Impact Category:** High
- **Impact Score:** 95.2th percentile
- **Stars:** 456
- **Forks:** 89
- **Used By:** 123 repositories
- **Namespace Occurrences:** 34
- **Last Updated:** 8 days ago
- **Public API Exposure:** 50%
- **Classification:** LibraryInternal

**Recommended Actions:** Distribute Roslyn analyzer with FixAll support. Provide detailed migration guide. Consider opening issue with migration steps.

---

## Top 25 Impacted Entities

**11. ProjectX/Formatter** (Repository, High)
   - Distribute Roslyn analyzer with FixAll support. Provide detailed migration guide. Consider opening issue with migration steps.

**12. AcmeLogger** (Package, High)
   - Distribute Roslyn analyzer with FixAll support. Provide detailed migration guide. Consider opening issue with migration steps.

**13. DevTeam/API** (Repository, Medium)
   - Include in migration documentation. Provide find/replace patterns and migration script. Analyzer will catch issues during build.

---

## Recommended Migration Strategy

### For Library Maintainers (Critical/High Impact)

1. **Immediate Coordination Required** - Contact maintainers of top 10 libraries
2. **Roslyn Analyzer + Code Fix** - Develop and distribute analyzer with automatic namespace fix
3. **Compatibility Shims** - Consider temporary type-forwarding adapter package
4. **Documentation** - Provide clear migration guide with before/after examples

### For Application Developers (Medium/Low Impact)

1. **Migration Script** - Provide PowerShell/batch script for automated namespace replacement
2. **Find/Replace Guide** - Document simple regex patterns for manual migration
3. **CI/CD Integration** - Add analyzer to catch issues during build

### For Package Consumers

1. **Version Pinning** - Pin to Humanizer v2.x until dependencies update
2. **Multi-targeting** - Support both v2 and v3 during transition period
3. **Adapter Package** - Use compatibility adapter if needed

## Impact by Namespace

| Namespace | Occurrences |
|-----------|-------------|
| Humanizer.Bytes | 456 |
| Humanizer.Localisation | 389 |
| Humanizer.Configuration | 178 |
| Humanizer.Inflections | 134 |
| Humanizer.DateTimeHumanizeStrategy | 91 |

## Methodology

**Analysis Date:** 2025-10-17 06:35:00 UTC

**Search Queries Used:**
- `"using Humanizer.Bytes" language:C#`
- `"using Humanizer.Localisation" language:C#`
- `"Humanizer.Bytes." language:C#`
- `"Humanizer.Localisation." language:C#`
- ...

**Scoring Formula:**
```
Repository: R * (0.30*LS(Dg) + 0.20*LS(Nu) + 0.15*LS(S) + 0.15*LS(O) + 0.12*P + 0.08*C)
Package: R * (0.40*LS(TD) + 0.30*LS(PD) + 0.15*LS(RO) + 0.10*LS(RD) + 0.05*C)
where LS(x) = log10(x+1)
```

## Confidence and Limitations

- **GitHub API Rate Limits:** Some repositories may have been missed due to rate limiting
- **Private Code:** Analysis only covers public repositories and packages
- **Public API Detection:** Automated detection may not catch all public API exposures
- **NuGet Dependency Data:** Reverse dependency counts may be incomplete
- **Code Context:** Limited context lines may not fully capture usage patterns

## Prioritized Action Items

1. Develop Roslyn analyzer with automatic namespace migration (FixAll support)
2. Create PowerShell/Bash migration script for automated find/replace
3. Write comprehensive migration guide with examples and common patterns
4. Immediate outreach to 8 critical-impact maintainers for coordinated releases
5. Contact 15 high-impact library maintainers with migration PRs
6. Consider publishing temporary compatibility adapter package (Humanizer.Migration)
7. Update Humanizer documentation with prominent v3 migration section
8. Create GitHub Discussions thread for community migration support
9. Monitor top 25 repositories for migration completion
10. Prepare blog post announcing v3 with migration timeline and resources

---

*This report was generated automatically by the Humanizer Impact Analysis Tool.*
