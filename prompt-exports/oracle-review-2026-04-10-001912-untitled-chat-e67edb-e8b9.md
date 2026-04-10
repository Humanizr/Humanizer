# Oracle Review

## Summary

These changes correctly tighten fn-5’s spec back to “no deferral,” record the local net8.0 test pass in the human-readable task docs, and update AGENTS.md plus fn-5.7’s markdown narrative to reflect the shipped `#if NET5_0_OR_GREATER` fix instead of the abandoned Polyfill approach. The main gaps are around Flow state consistency: the markdown now says tasks are done, but the task JSON metadata still says otherwise, and the re-recorded fn-5.5 JSON evidence is still materially thinner than the markdown claims.

## Major

- **File: `.flow/tasks/fn-5-locale-parity-sign-off-verify-code.7.json:11-12`, `.flow/tasks/fn-5-locale-parity-sign-off-verify-code.8.json:14-16`**  
  **Problem:** The task tracker JSON is still out of sync with the markdown you just updated. `.7.json` still has the old Polyfill-based title, and both `.7.json` and `.8.json` still show `status: "todo"` even though `.7.md` and `.8.md` now contain completed done summaries/evidence. Any Flow consumer will still treat these tasks as unfinished and `.7` as the wrong implementation.  
  **Suggestion:** Update the JSON task records alongside the markdown: set the correct shipped title for `.7.json`, mark `.7`/`.8` done if they are complete, and keep `updated_at`/evidence fields aligned with the markdown.

- **File: `.flow/tasks/fn-5-locale-parity-sign-off-verify-code.5.json:19-42` vs `.flow/tasks/fn-5-locale-parity-sign-off-verify-code.5.md:218-223`**  
  **Problem:** The task’s machine-readable evidence and human-readable evidence are not actually in sync. The markdown says fn-5.5 ran and passed scans 2a–2i, plus build/test/format, but the JSON only records `net10_0`, `net8_0`, `format_check`, and `sdk_proof`. That leaves the gate evidence incomplete for any automation or later audit that reads the JSON.  
  **Suggestion:** Either add the missing scan/build evidence to `.5.json` so it matches the markdown claims, or explicitly narrow the markdown to only what is present in JSON and document where the authoritative full scan evidence lives.

## Minor

- **File: `.flow/tasks/fn-5-locale-parity-sign-off-verify-code.7.md:16-39`**  
  **Problem:** The task was supposed to be “reconciled to the shipped `#if` guard implementation,” but the `## Approach` section still leads with “Preferred path — add Polyfill reference to the test project.” That makes the task read like the Polyfill path is still the intended implementation rather than a rejected attempt.  
  **Suggestion:** Rewrite the approach to lead with the shipped `#if NET5_0_OR_GREATER` fix, and move the Polyfill attempt into a short “rejected approach / context” note.

<verdict>NEEDS_WORK</verdict>