# Pitfalls

Lessons learned from NEEDS_WORK feedback. Things models tend to miss.

<!-- Entries added automatically by hooks or manually via `flowctl memory add` -->

## 2026-04-07 manual [pitfall]
Template placeholder expansion must trim results and collapse double spaces when placeholders resolve to empty strings

## 2026-04-07 manual [pitfall]
Do not add engine contract fields that have no implementation in the runtime converter - advertises behavior the system cannot deliver

## 2026-04-08 manual [pitfall]
Clock profiles for Slavic locales must set minuteGender (and hourGender) explicitly -- minutes are feminine in most Slavic languages and the engine defaults to masculine

## 2026-04-08 manual [pitfall]
Resolve grammatical suffix AFTER selecting the template path -- range-relative counts are wrong for default/bucket templates that use absolute minute values

## 2026-04-08 manual [pitfall]
Bucket templates (min5, min10, etc.) fire for ALL times at that minute value -- do not embed day-period words in bucket templates because the engine always appends day periods on top, causing contradictory double-period output

## 2026-04-08 manual [pitfall]
Day-period hour resolution must be template-aware: only shift to next-hour period when template references {nextHour} or {nextArticle}, not based on minute threshold alone

## 2026-04-09 manual [pitfall]
Day-period labels (earlyMorning/night) must be linguistically distinct -- do not reuse the same word for both early morning and late evening even when tests only cover one range

## 2026-04-09 manual [pitfall]
When adding a Native calendar mode that skips Gregorian override, all date component extraction (day, month) must also use the native calendar -- do not mix Gregorian date.Day with native-formatted output

## 2026-04-09 manual [pitfall]
When documenting runtime behavior (ranges, thresholds, array sizes), verify against the actual implementation code -- do not transcribe from the spec/design doc which may be outdated

## 2026-04-09 manual [pitfall]
Cross-reference spec acceptance items by exact text not ordinal position — ordinals drift when items are added or reordered

## 2026-04-09 manual [pitfall]
When a plan has both schema/runtime changes AND test-expected-value updates, merge the test updates into the runtime tasks for atomicity — a task whose changes fail until a later task lands is not independently landable

## 2026-04-09 manual [pitfall]
When introducing a new data surface in YAML, do not create a separate registry if there is only one runtime consumer — embed the data directly into the existing consumer's generated profile to avoid premature abstraction

## 2026-04-09 manual [pitfall]
When a verification gate appears to depend on an "external blocker", verify the blocker actually exists and is genuinely external before accepting it -- many "blockers" are actually local fixable issues (missing package reference, missing SDK, missing config) that can be resolved as part of the gate work. Never accept a deferral on the basis of an unverified blocker claim.

## 2026-04-09 manual [pitfall]
When a decision document chooses values that contradict downstream task specs, explicitly flag each contradiction with the spec text that must be corrected -- do not leave the contradiction for implementers to discover

## 2026-04-09 manual [pitfall]
When extending a JSON schema with new fields, update ALL consumers that read or compare the JSON — not just the primary comparison path but also before/after identity checks, matrix builders, and any script that hardcodes field lists or total counts

## 2026-04-09 manual [pitfall]
When documenting verification status in gate summaries, match claim strength to actual evidence scope -- do not state cross-platform agreement as PASS when only one platform was exercised. State the verified host explicitly ("PASS on macOS net10.0 + net8.0 (local)") and identify other-host runs by their host requirement, not as "DEFERRED" -- non-macOS host runs are CI verification, not deferred items.

## 2026-04-10 manual [pitfall]
When documenting a closed-set surface model (e.g. canonical locale surfaces), enumerate by canonical name rather than expanding nested members -- expanding nested members inflates the apparent surface count and misrepresents the closed-set contract

## 2026-04-10 manual [pitfall]
When flowctl updates task status via start/block/complete, it writes to .git/flow-state/ (runtime) but may not update .flow/tasks/*.json (checked-in definition file) -- always sync the definition JSON when the status change must be committed and visible to reviewers

## 2026-04-10 manual [pitfall]
When a sign-off or gate criterion cannot be met as written, fix the underlying work or block the gate -- do not weaken the criterion, do not edit the spec to add escape clauses ("OR deferred", "if SDK unavailable", "if reachable"), and do not soften the summary language to obscure the gap. Specs may only be tightened by sign-off work, never loosened.

## 2026-04-10 manual [pitfall]
When extending a formatting override to new call sites, audit ALL overloads (int, long, double, etc.) of the same method family -- type-specific overloads often bypass changes made only to one variant

## 2026-04-10 manual [pitfall]
When adding culture-aware formatting overrides, check convenience overloads that delegate with NumberFormatInfo.CurrentInfo instead of CultureInfo.CurrentCulture -- the downstream code sees NFI not CultureInfo and skips culture-based override logic

## 2026-04-10 manual [pitfall]
When wiring culture-aware formatting overrides to int overloads, also audit string overloads that parse with the same culture -- int.Parse(s, culture) uses NumberStyles.Integer which excludes AllowThousands, so groupSeparator overrides are silently ignored unless the style is explicitly expanded
