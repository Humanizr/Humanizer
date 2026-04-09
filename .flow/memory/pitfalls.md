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
When a verification gate depends on a known external blocker (e.g., net48 test suite blocked by Enum.GetValues<T>), either make it a tracked dependency or explicitly downgrade to documented follow-up — never leave a hard gate that cannot close

## 2026-04-09 manual [pitfall]
When a decision document chooses values that contradict downstream task specs, explicitly flag each contradiction with the spec text that must be corrected -- do not leave the contradiction for implementers to discover
