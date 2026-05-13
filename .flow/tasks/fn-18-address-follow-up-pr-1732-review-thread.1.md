# fn-18-address-follow-up-pr-1732-review-thread.1 Patch Regex word join-control parity

## Description
TBD

## Acceptance
- [ ] TBD

## Done summary
Addressed the follow-up PR #1732 review thread for Regex word-character parity by including U+200C ZERO WIDTH NON-JOINER and U+200D ZERO WIDTH JOINER in ArticlePrefixSort.IsRegexWordCharacter, and added regression coverage for both join-control characters.
## Evidence
- Commits:
- Tests:
- PRs: