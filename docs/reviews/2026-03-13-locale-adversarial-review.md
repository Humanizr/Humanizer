# Locale Adversarial Review

Generated: 2026-03-13

## Summary
- Total locales reviewed: 51
- Defects: 203
- Suspicious items: 57

## Cross-Locale Themes
- `TimeSpanHumanize_Zero`: 12 locales (az, bn, cs, el, fr, hr, hy, id, pt, pt-BR, uz-Latn-UZ, vi)
- `DateHumanize_SingleMinuteFromNow`: 6 locales (el, id, lv, sr, th, uz-Latn-UZ)
- `TimeSpanHumanize_SingleMillisecond`: 6 locales (af, el, fa, fil, pt-BR, vi)
- `DateHumanize_SingleMonthFromNow`: 5 locales (el, id, pt, th, uz-Latn-UZ)
- `DateHumanize_TwoDaysFromNow`: 5 locales (bg, hu, pt, tr, uz-Latn-UZ)
- `DateHumanize_SingleSecondFromNow`: 5 locales (el, id, sr, th, uz-Latn-UZ)
- `TimeSpanHumanize_MultipleMilliseconds`: 5 locales (ar, fa, fil, pt-BR, vi)
- `DateHumanize_MultipleMonthsFromNow_Singular`: 4 locales (ca, es, pt, th)
- `DateHumanize_TwoDaysAgo`: 4 locales (bg, da, hu, uz-Latn-UZ)
- `DateHumanize_SingleHourFromNow`: 4 locales (el, id, th, uz-Latn-UZ)
- `DateHumanize_SingleYearFromNow`: 4 locales (el, id, th, uz-Latn-UZ)
- `DateHumanize_MultipleMinutesFromNow`: 4 locales (id, th, uz-Cyrl-UZ, uz-Latn-UZ)
- `DateHumanize_MultipleSecondsFromNow`: 4 locales (id, th, uz-Cyrl-UZ, uz-Latn-UZ)
- `DateHumanize_MultipleDaysFromNow`: 4 locales (bg, id, th, uz-Latn-UZ)
- `DateHumanize_MultipleDaysFromNow_Paucal`: 4 locales (bg, id, th, uz-Latn-UZ)
- `TimeSpanHumanize_SingleMillisecond_Words`: 3 locales (af, fil, pt-BR)
- `DateHumanize_MultipleSecondsFromNow_Dual`: 3 locales (ca, es, th)
- `DateHumanize_MultipleSecondsFromNow_Plural`: 3 locales (ca, es, th)
- `DateHumanize_MultipleMinutesFromNow_Singular`: 3 locales (ca, es, th)
- `DateHumanize_MultipleMonthsFromNow_Dual`: 3 locales (ca, es, th)
- `DateHumanize_MultipleMonthsFromNow_Plural`: 3 locales (ca, es, th)
- `DateHumanize_MultipleSecondsFromNow_Singular`: 3 locales (ca, es, th)
- `DateHumanize_MultipleYearsFromNow_Singular`: 3 locales (ca, es, th)
- `DateHumanize_SingleSecondAgo`: 3 locales (fi, pt, sr)
- `DataUnit_Byte`: 3 locales (it, nb, sv)
- `DateHumanize_MultipleYearsFromNow_Dual`: 3 locales (ca, es, th)
- `DataUnit_Gigabyte`: 3 locales (it, nb, sv)
- `DateHumanize_MultipleYearsFromNow_Plural`: 3 locales (ca, es, th)
- `DateHumanize_MultipleMinutesFromNow_Plural`: 3 locales (ca, es, th)
- `DateHumanize_MultipleHoursFromNow`: 3 locales (id, th, uz-Latn-UZ)
- `DateHumanize_MultipleDaysFromNow_Singular`: 3 locales (ca, es, th)
- `DateHumanize_MultipleDaysFromNow_Dual`: 3 locales (ca, es, th)
- `DateHumanize_MultipleMonthsFromNow`: 3 locales (id, th, uz-Latn-UZ)
- `DateHumanize_MultipleDaysFromNow_Plural`: 3 locales (ca, es, th)
- `DateHumanize_MultipleYearsFromNow`: 3 locales (id, th, uz-Latn-UZ)
- `TimeSpanHumanize_MultipleMilliseconds_Dual`: 3 locales (ar, fil, pt-BR)
- `TimeSpanHumanize_SingleMonth`: 3 locales (ar, bn, pt)
- `TimeSpanHumanize_MultipleMilliseconds_Plural`: 3 locales (ar, fil, pt-BR)
- `DateHumanize_MultipleMinutesFromNow_Dual`: 3 locales (ca, es, th)
- `TimeSpanHumanize_MultipleMonths_Singular`: 2 locales (is, pt)
- `DateHumanize_MultipleMinutesAgo`: 2 locales (pt, uz-Cyrl-UZ)
- `DateHumanize_MultipleSecondsAgo`: 2 locales (pt, uz-Cyrl-UZ)
- `DateHumanize_SingleMinuteAgo`: 2 locales (pt, sr)
- `DateHumanize_MultipleYearsAgo_Paucal`: 2 locales (ms, pt)
- `TimeSpanHumanize_Age`: 2 locales (fil, it)
- `TimeUnit_Week`: 2 locales (pl, zh-Hant)
- `DateHumanize_SingleYearAgo`: 2 locales (ar, pt)
- `DateHumanize_MultipleSecondsAgo_Plural`: 2 locales (is, pt)
- `DateHumanize_MultipleHoursFromNow_Paucal`: 2 locales (es, th)
- `DateHumanize_MultipleHoursFromNow_Plural`: 2 locales (es, th)
- `DateHumanize_MultipleHoursFromNow_Singular`: 2 locales (es, th)
- `DateHumanize_MultipleHoursFromNow_Dual`: 2 locales (es, th)
- `TimeSpanHumanize_SingleSecond`: 2 locales (af, uz-Cyrl-UZ)
- `DateHumanize_MultipleDaysAgo`: 2 locales (bg, pt)
- `DateHumanize_MultipleDaysAgo_Paucal`: 2 locales (bg, pt)
- `DateHumanize_MultipleMinutesFromNow_Paucal`: 2 locales (es, th)
- `DateHumanize_MultipleYearsFromNow_Paucal`: 2 locales (es, th)
- `TimeSpanHumanize_MultipleMilliseconds_Paucal`: 2 locales (fil, pt-BR)
- `TimeSpanHumanize_MultipleMilliseconds_Singular`: 2 locales (fil, pt-BR)
- `DateHumanize_MultipleSecondsFromNow_Paucal`: 2 locales (es, th)
- `DateHumanize_MultipleMonthsAgo_Dual`: 2 locales (es, pt)
- `DateHumanize_MultipleMonthsAgo_Singular`: 2 locales (es, pt)
- `DateHumanize_MultipleMonthsFromNow_Paucal`: 2 locales (es, th)

## Master Findings
- [af] `TimeSpanHumanize_SingleMillisecond`
  Current: 1 millisekond
  Proposed: 1 millisekonde
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Die korrekte Afrikaans enkelvoud is millisekonde. Millisekond is nie standaardvorm nie.
  Evidence: src/Humanizer/Properties/Resources.af.resx: TimeSpanHumanize_SingleMillisecond is currently 1 millisekond while plural is millisekondes.; src/Humanizer/Properties/Resources.af.resx: DateHumanize_SingleMillisecondAgo uses 1 millisekonde terug.
  Notes: Align singular form with established Afrikaans spelling.
- [af] `TimeSpanHumanize_SingleMillisecond_Words`
  Current: een millisekond
  Proposed: een millisekonde
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Millisekond is ortografies onvanpas in Afrikaans; millisekonde is die natuurlike enkelvoud.
  Evidence: src/Humanizer/Properties/Resources.af.resx: TimeSpanHumanize_SingleMillisecond_Words is currently een millisekond.; Date/multi forms in the same locale consistently use millisekonde/millisekondes pattern.
  Notes: Word-form variant should preserve standard singular morphology.
- [af] `TimeSpanHumanize_SingleSecond`
  Current: 1 sekond
  Proposed: 1 sekonde
  Status: defect / Severity: P2 / Confidence: high
  Rationale: In standaard Afrikaans is die enkelvoud sekonde, nie sekond nie. Die huidige vorm is onidiomaties en ortografies afwykend.
  Evidence: src/Humanizer/Properties/Resources.af.resx: TimeSpanHumanize_SingleSecond is currently 1 sekond while plural is sekondes.; tests/Humanizer.Tests/Localisation/af/DateHumanizeTests.cs: singular second form is 1 sekonde terug.
  Notes: Normalize singular noun morphology to standard Afrikaans.
- [af] `TimeSpanHumanize_SingleSecond_Words`
  Current: een sekond
  Proposed: een sekonde
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Sekond is nie die standaard geskrewe enkelvoud in Afrikaans nie; sekonde is korrek en konsekwent met ander tydfrases.
  Evidence: src/Humanizer/Properties/Resources.af.resx: TimeSpanHumanize_SingleSecond_Words is currently een sekond.; src/Humanizer/Properties/Resources.af.resx: DateHumanize_SingleSecondAgo uses 1 sekonde terug.
  Notes: Word-form variant should mirror numeric singular form.
- [ar] `DateHumanize_SingleYearAgo`
  Current: Ã˜Â§Ã™â€žÃ˜Â¹Ã˜Â§Ã™â€¦ Ã˜Â§Ã™â€žÃ˜Â³Ã˜Â§Ã˜Â¨Ã™â€š
  Proposed: Ù…Ù†Ø° Ø³Ù†Ø© ÙˆØ§Ø­Ø¯Ø©
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: "Ø§Ù„Ø¹Ø§Ù… Ø§Ù„Ø³Ø§Ø¨Ù‚" Ø£Ù‚Ø±Ø¨ Ø¥Ù„Ù‰ "last year" Ù…Ù†Ù‡ Ø¥Ù„Ù‰ "one year ago". ØµÙŠØ§ØºØ© "Ù…Ù†Ø° Ø³Ù†Ø© ÙˆØ§Ø­Ø¯Ø©" Ø£Ø¯Ù‚ Ø¯Ù„Ø§Ù„ÙŠÙ‹Ø§ ÙˆÙ…ØªØ³Ù‚Ø© Ù…Ø¹ Ø¨Ù‚ÙŠØ© ØµÙŠØº "Ù…Ù†Ø°" ÙÙŠ Ù†ÙØ³ Ø§Ù„Ù…Ø¬Ù…ÙˆØ¹Ø©.
  Evidence: src/Humanizer/Properties/Resources.ar.resx: DateHumanize_SingleYearAgo = "Ø§Ù„Ø¹Ø§Ù… Ø§Ù„Ø³Ø§Ø¨Ù‚" (comment: one year ago).; tests/Humanizer.Tests/Localisation/ar/DateHumanizeTests.cs: YearsAgo(-1) currently expects "Ø§Ù„Ø¹Ø§Ù… Ø§Ù„Ø³Ø§Ø¨Ù‚".
  Notes: ØªØ­Ø³ÙŠÙ† Ø¯Ù„Ø§Ù„ÙŠ/Ø£Ø³Ù„ÙˆØ¨ÙŠØ› Ø§Ù„Ø³Ù„Ø³Ù„Ø© Ø§Ù„Ø­Ø§Ù„ÙŠØ© Ù…ÙÙ‡ÙˆÙ…Ø© Ù„ÙƒÙ†Ù‡Ø§ Ø£Ù‚Ù„ Ù…Ø·Ø§Ø¨Ù‚Ø©Ù‹ Ù„Ù„Ù…Ø±Ø¬Ø¹.
- [ar] `TimeSpanHumanize_MultipleMilliseconds`
  Current: {0} Ã˜Â¬Ã˜Â²Ã˜Â¡ Ã™â€¦Ã™â€  Ã˜Â§Ã™â€žÃ˜Â«Ã˜Â§Ã™â€ Ã™Å Ã˜Â©
  Proposed: {0} Ù…Ù„Ù„ÙŠ Ø«Ø§Ù†ÙŠØ©
  Status: defect / Severity: P1 / Confidence: high
  Rationale: "{0} Ø¬Ø²Ø¡ Ù…Ù† Ø§Ù„Ø«Ø§Ù†ÙŠØ©" Ù„Ø§ ØªØ¹Ø¨Ù‘Ø± Ø¹Ù† milliseconds ÙƒÙˆØ­Ø¯Ø© Ù…Ø³ØªÙ‚Ù„Ø©Ø› ØªÙÙÙ‡Ù… ÙƒÙ€ fraction Ø¹Ø§Ù… Ù…Ù† Ø§Ù„Ø«Ø§Ù†ÙŠØ©. Ø§Ù„Ù…ØµØ·Ù„Ø­ Ø§Ù„Ø¹Ø±Ø¨ÙŠ Ø§Ù„ØªÙ‚Ù†ÙŠ Ø§Ù„Ø´Ø§Ø¦Ø¹ Ù‡Ùˆ "Ù…Ù„Ù„ÙŠ Ø«Ø§Ù†ÙŠØ©".
  Evidence: src/Humanizer/Properties/Resources.ar.resx: TimeUnit_Millisecond = "Ù…Ù„Ù„ÙŠ Ø«Ø§Ù†ÙŠØ©".; src/Humanizer/Properties/Resources.ar.resx: TimeSpanHumanize_MultipleMilliseconds currently uses "Ø¬Ø²Ø¡ Ù…Ù† Ø§Ù„Ø«Ø§Ù†ÙŠØ©".
  Notes: Ø®Ù„Ù„ Ù…ØµØ·Ù„Ø­ÙŠ ÙŠØ¤Ø«Ø± Ø¯Ù‚Ø© Ø§Ù„ÙˆØ­Ø¯Ø©.
- [ar] `TimeSpanHumanize_MultipleMilliseconds_Dual`
  Current: Ã˜Â¬Ã˜Â²Ã˜Â¦Ã™Å Ã™â€  Ã™â€¦Ã™â€  Ã˜Â§Ã™â€žÃ˜Â«Ã˜Â§Ã™â€ Ã™Å Ã˜Â©
  Proposed: Ù…Ù„Ù„ÙŠ Ø«Ø§Ù†ÙŠØªÙŠÙ†
  Status: defect / Severity: P1 / Confidence: high
  Rationale: "Ø¬Ø²Ø¦ÙŠÙ† Ù…Ù† Ø§Ù„Ø«Ø§Ù†ÙŠØ©" ØºÙŠØ± Ø¯Ù‚ÙŠÙ‚ ÙƒÙˆØ­Ø¯Ø© milliseconds. Ø§Ù„ØµÙŠØ§ØºØ© Ø§Ù„Ù…Ù‚ØªØ±Ø­Ø© ØªØ­ÙØ¸ Ø§Ù„ØªØ«Ù†ÙŠØ© ÙˆØªÙØ¨Ù‚ÙŠ Ø§Ø³Ù… Ø§Ù„ÙˆØ­Ø¯Ø© Ø§Ù„ØªÙ‚Ù†ÙŠØ©.
  Evidence: src/Humanizer/Properties/Resources.ar.resx: TimeSpanHumanize_MultipleMilliseconds_Dual = "Ø¬Ø²Ø¦ÙŠÙ† Ù…Ù† Ø§Ù„Ø«Ø§Ù†ÙŠØ©".; tests/Humanizer.Tests/Localisation/ar/TimeSpanHumanizeTests.cs currently asserts this phrase for 2 ms.
  Notes: Ø¨Ø¯ÙŠÙ„ Ù…Ù‚ØªØ±Ø­ ÙŠØ­Ø§ÙØ¸ Ø¹Ù„Ù‰ Ù†Ù…Ø· Ø§Ù„ØªØ«Ù†ÙŠØ© Ø§Ù„Ø­Ø§Ù„ÙŠ ÙÙŠ Ø§Ù„Ù…ÙˆØ§Ø±Ø¯.
- [ar] `TimeSpanHumanize_MultipleMilliseconds_Plural`
  Current: {0} Ã˜Â£Ã˜Â¬Ã˜Â²Ã˜Â§Ã˜Â¡ Ã™â€¦Ã™â€  Ã˜Â§Ã™â€žÃ˜Â«Ã˜Â§Ã™â€ Ã™Å Ã˜Â©
  Proposed: {0} Ù…Ù„Ù„ÙŠ Ø«Ø§Ù†ÙŠØ©
  Status: defect / Severity: P1 / Confidence: high
  Rationale: "{0} Ø£Ø¬Ø²Ø§Ø¡ Ù…Ù† Ø§Ù„Ø«Ø§Ù†ÙŠØ©" ØºÙŠØ± Ø§ØµØ·Ù„Ø§Ø­ÙŠ Ù„Ù„Ù€ ms ÙˆÙ‚Ø¯ ÙŠØ³Ø¨Ø¨ Ù„Ø¨Ø³Ù‹Ø§ Ø¯Ù„Ø§Ù„ÙŠÙ‹Ø§. "Ù…Ù„Ù„ÙŠ Ø«Ø§Ù†ÙŠØ©" Ù‡Ùˆ Ø§Ù„ØªØ¹Ø¨ÙŠØ± Ø§Ù„Ù‚ÙŠØ§Ø³ÙŠ Ù„Ù„Ù…Ù‚Ø¯Ø§Ø± Ø§Ù„Ø¹Ø¯Ø¯ÙŠ.
  Evidence: src/Humanizer/Properties/Resources.ar.resx: TimeSpanHumanize_MultipleMilliseconds_Plural currently uses "Ø£Ø¬Ø²Ø§Ø¡ Ù…Ù† Ø§Ù„Ø«Ø§Ù†ÙŠØ©".; src/Humanizer/Properties/Resources.ar.resx already uses "Ù…Ù„Ù„ÙŠ Ø«Ø§Ù†ÙŠØ©" in TimeUnit_Millisecond.
  Notes: ØªØµØ­ÙŠØ­ Ù…ØµØ·Ù„Ø­ÙŠ ÙˆØ§ØªØ³Ø§Ù‚ Ø¯Ø§Ø®Ù„ÙŠ Ù…Ø¹ TimeUnit.
- [ar] `TimeSpanHumanize_MultipleMonths`
  Current: {0} Ã˜Â£Ã˜Â´Ã™â€¡Ã˜Â±
  Proposed: {0} Ø´Ù‡Ø±
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Ø§Ù„ØµÙŠØºØ© Ø§Ù„Ø­Ø§Ù„ÙŠØ© "{0} Ø£Ø´Ù‡Ø±" ÙÙŠ Ù‡Ø°Ø§ Ø§Ù„Ù…ÙØªØ§Ø­ Ø§Ù„Ø£Ø³Ø§Ø³ÙŠ ØªÙÙ†ØªØ¬ ØªØ±ÙƒÙŠØ¨Ù‹Ø§ ØºÙŠØ± Ø³Ù„ÙŠÙ… Ù„Ù„Ø£Ø¹Ø¯Ø§Ø¯ Ø§Ù„ÙƒØ¨ÙŠØ±Ø© (Ù…Ø«Ù„ 11). Ø§Ù„ØµÙŠØºØ© Ø§Ù„Ù…ÙØ±Ø¯Ø© Ù‡Ù†Ø§ Ø£Ù†Ø³Ø¨ Ù„Ø¯ÙˆØ± Ø§Ù„Ù…ÙØªØ§Ø­ Ø§Ù„Ø£Ø³Ø§Ø³ÙŠ ÙÙŠ Ø§Ù„Ø¹Ø±Ø¨ÙŠØ©.
  Evidence: tests/Humanizer.Tests/Localisation/ar/TimeSpanHumanizeTests.cs: Months(335) expects "11 Ø£Ø´Ù‡Ø±" and is tagged [Trait("Translation", "Google")].; src/Humanizer/Properties/Resources.ar.resx: DateHumanize_MultipleMonths uses singular base "{0} Ø´Ù‡Ø±" for many-form behavior.
  Notes: ÙŠÙˆØ¬Ø¯ Ø£Ø«Ø± Ù…ØªÙˆÙ‚Ø¹ Ø¹Ù„Ù‰ Ø§Ù„Ø§Ø®ØªØ¨Ø§Ø±Ø§Øª Ø§Ù„Ø­Ø§Ù„ÙŠØ© Ø§Ù„Ù…Ø±ØªØ¨Ø·Ø© Ø¨ØªØ±Ø¬Ù…Ø© Google.
- [ar] `TimeSpanHumanize_MultipleYears_Plural`
  Current: {0} Ã˜Â³Ã™â€ Ã˜Â©
  Proposed: {0} Ø³Ù†ÙˆØ§Øª
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Ø§Ù„Ù…ÙØªØ§Ø­ Ø§Ù„Ù…ÙˆØ³ÙˆÙ… Plural ÙŠØ¬Ø¨ Ø£Ù† ÙŠØ­Ù…Ù„ Ø¬Ù…Ø¹Ù‹Ø§ Ø­Ù‚ÙŠÙ‚ÙŠÙ‹Ø§. Ø§Ù„Ù‚ÙŠÙ…Ø© Ø§Ù„Ø­Ø§Ù„ÙŠØ© "{0} Ø³Ù†Ø©" ØªÙƒØ±Ø± Ø§Ù„Ù…ÙØ±Ø¯ ÙˆØªÙÙ‚Ø¯ Ø§Ù„ØªÙØ±ÙŠÙ‚ Ø¨ÙŠÙ† Ø§Ù„ØµÙŠØº.
  Evidence: src/Humanizer/Properties/Resources.ar.resx: TimeSpanHumanize_MultipleYears_Plural currently equals "{0} Ø³Ù†Ø©".; src/Humanizer/Properties/Resources.ar.resx: DateHumanize_MultipleYearsFromNow_Plural uses proper plural "{0} Ø³Ù†ÙˆØ§Øª".
  Notes: ØªØµØ­ÙŠØ­ Ø§ØªØ³Ø§Ù‚ ØµØ±ÙÙŠ Ø¯Ø§Ø®Ù„ Ù…Ø¬Ù…ÙˆØ¹Ø© Ø§Ù„Ø³Ù†ÙˆØ§Øª.
- [ar] `TimeSpanHumanize_SingleMonth`
  Current: Ã˜Â´Ã™â€¡Ã˜Â± 1
  Proposed: Ø´Ù‡Ø± ÙˆØ§Ø­Ø¯
  Status: defect / Severity: P1 / Confidence: high
  Rationale: "Ø´Ù‡Ø± 1" ØªØ±ØªÙŠØ¨ Ù…ØªØ±Ø¬Ù… Ø¢Ù„ÙŠÙ‹Ø§ ÙˆØºÙŠØ± Ø·Ø¨ÙŠØ¹ÙŠ Ø¹Ø±Ø¨ÙŠÙ‹Ø§. Ø§Ù„ØµÙŠØ§ØºØ© Ø§Ù„Ø³Ù„ÙŠÙ…Ø©: "Ø´Ù‡Ø± ÙˆØ§Ø­Ø¯".
  Evidence: src/Humanizer/Properties/Resources.ar.resx: TimeSpanHumanize_SingleMonth = "Ø´Ù‡Ø± 1".; tests/Humanizer.Tests/Localisation/ar/TimeSpanHumanizeTests.cs marks month strings with [Trait("Translation", "Google")].
  Notes: Ø®Ù„Ù„ Ù„ØºÙˆÙŠ ÙˆØ§Ø¶Ø­ Ù‚Ø§Ø¨Ù„ Ù„Ù„Ø±ØµØ¯ Ù„Ù„Ù…Ø³ØªØ®Ø¯Ù… Ø§Ù„Ù†Ù‡Ø§Ø¦ÙŠ.
- [ar] `TimeSpanHumanize_SingleYear`
  Current: Ã˜Â§Ã™â€žÃ˜Â³Ã™â€ Ã˜Â© 1
  Proposed: Ø³Ù†Ø© ÙˆØ§Ø­Ø¯Ø©
  Status: defect / Severity: P1 / Confidence: high
  Rationale: "Ø§Ù„Ø³Ù†Ø© 1" ØµÙŠØ§ØºØ© ØºÙŠØ± Ø¹Ø±Ø¨ÙŠØ© Ø·Ø¨ÙŠØ¹ÙŠØ© ÙˆØªØ´ÙŠØ± Ø¥Ù„Ù‰ ØªØ±Ø¬Ù…Ø© Ø­Ø±ÙÙŠØ©. Ø§Ù„Ù…Ù‚Ø§Ø¨Ù„ Ø§Ù„Ø·Ø¨ÙŠØ¹ÙŠ: "Ø³Ù†Ø© ÙˆØ§Ø­Ø¯Ø©".
  Evidence: src/Humanizer/Properties/Resources.ar.resx: TimeSpanHumanize_SingleYear = "Ø§Ù„Ø³Ù†Ø© 1".; tests/Humanizer.Tests/Localisation/ar/TimeSpanHumanizeTests.cs marks year strings with [Trait("Translation", "Google")].
  Notes: Ø®Ù„Ù„ Ù„ØºÙˆÙŠ Ù…Ø¨Ø§Ø´Ø± ÙÙŠ ØµÙŠØºØ© Ø§Ù„Ù…ÙØ±Ø¯.
- [az] `TimeSpanHumanize_Zero`
  Current: zaman fÉ™rqi yoxdur
  Proposed: vaxt yoxdur
  Status: defect / Severity: P2 / Confidence: high
  Rationale: "zaman fÉ™rqi yoxdur" ifadÉ™si iki vaxt nÃ¶qtÉ™si arasÄ±nda mÃ¼qayisÉ™ mÉ™nasÄ± verir. `TimeSpan.Zero` Ã¼Ã§Ã¼n AzÉ™rbaycan dilindÉ™ daha tÉ™bii vÉ™ birbaÅŸa qarÅŸÄ±lÄ±q "vaxt yoxdur"dur.
  Evidence: src/Humanizer/Properties/Resources.az.resx: TimeSpanHumanize_Zero = zaman fÉ™rqi yoxdur; tests/Humanizer.Tests/Localisation/az/TimeSpanHumanizeTests.cs: NoTimeToWords testi bu aÃ§arÄ± yoxlayÄ±r
  Notes: MÉ™na uyÄŸunsuzluÄŸu: duration=0 Ã¼Ã§Ã¼n "fÉ™rq" deyil, "mÃ¶vcudluq/yoxluq" ifadÉ™si daha dÃ¼zgÃ¼ndÃ¼r.
- [bg] `DateHumanize_MultipleDaysAgo`
  Current: Ð¿Ñ€ÐµÐ´Ð¸ {0} Ð´ÐµÐ½Ð°
  Proposed: Ð¿Ñ€ÐµÐ´Ð¸ {0} Ð´Ð½Ð¸
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Standard written Bulgarian uses "дни" as the plural of "ден". "дена" is colloquial/regional and should not be the default UI string.
  Evidence: src/Humanizer/Properties/Resources.bg.resx:193 uses "преди {0} дена".; tests/Humanizer.Tests/Localisation/bg/DateHumanizeTests.cs:45 expects "преди 2 дена".
  Notes: Use the formal plural "дни" in date-humanized output.
- [bg] `DateHumanize_MultipleDaysAgo_Paucal`
  Current: Ð¿Ñ€ÐµÐ´Ð¸ {0} Ð´ÐµÐ½Ð°
  Proposed: Ð¿Ñ€ÐµÐ´Ð¸ {0} Ð´Ð½Ð¸
  Status: defect / Severity: P2 / Confidence: high
  Rationale: The paucal path should match standard Bulgarian plural usage. "дни" is standard; "дена" is colloquial.
  Evidence: src/Humanizer/Properties/Resources.bg.resx:196 uses "преди {0} дена".; tests/Humanizer.Tests/Localisation/bg/ResourcesTests.cs:19 asserts the same non-standard form.
  Notes: Align paucal and regular day plural to "дни".
- [bg] `DateHumanize_MultipleDaysFromNow`
  Current: ÑÐ»ÐµÐ´ {0} Ð´ÐµÐ½Ð°
  Proposed: ÑÐ»ÐµÐ´ {0} Ð´Ð½Ð¸
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Future-day phrasing should use the formal plural "дни"; current "дена" sounds colloquial.
  Evidence: src/Humanizer/Properties/Resources.bg.resx:199 uses "след {0} дена".; tests/Humanizer.Tests/Localisation/bg/DateHumanizeTests.cs:51 expects "след 2 дена".
  Notes: Prefer standard register for library output.
- [bg] `DateHumanize_MultipleDaysFromNow_Paucal`
  Current: ÑÐ»ÐµÐ´ {0} Ð´ÐµÐ½Ð°
  Proposed: ÑÐ»ÐµÐ´ {0} Ð´Ð½Ð¸
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Paucal/future form should also use formal plural "дни" for consistency and correctness.
  Evidence: src/Humanizer/Properties/Resources.bg.resx:202 uses "след {0} дена".; tests/Humanizer.Tests/Localisation/bg/ResourcesTests.cs:22 asserts the same non-standard form.
  Notes: Keep formal style across all day plural variants.
- [bg] `DateHumanize_TwoDaysAgo`
  Current: Ð¿Ñ€ÐµÐ´Ð¸ 2 Ð´ÐµÐ½Ð°
  Proposed: Ð¿Ñ€ÐµÐ´Ð¸ 2 Ð´Ð½Ð¸
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Fixed two-day idiom should still use the standard plural noun "дни" in neutral UI language.
  Evidence: src/Humanizer/Properties/Resources.bg.resx:241 uses "преди 2 дена".; tests/Humanizer.Tests/Localisation/bg/DateHumanizeTests.cs:45 expects the same.
  Notes: Use "преди 2 дни".
- [bg] `DateHumanize_TwoDaysFromNow`
  Current: ÑÐ»ÐµÐ´ 2 Ð´ÐµÐ½Ð°
  Proposed: ÑÐ»ÐµÐ´ 2 Ð´Ð½Ð¸
  Status: defect / Severity: P2 / Confidence: high
  Rationale: In standard Bulgarian, this should be "след 2 дни"; "дена" is colloquial.
  Evidence: src/Humanizer/Properties/Resources.bg.resx:244 uses "след 2 дена".; tests/Humanizer.Tests/Localisation/bg/DateHumanizeTests.cs:51 expects the same.
  Notes: Use formal plural in fixed two-day future phrase.
- [bg] `TimeSpanHumanize_MultipleDays`
  Current: {0} Ð´ÐµÐ½Ð°
  Proposed: {0} Ð´Ð½Ð¸
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Duration plural should be "дни" in standard Bulgarian. "дена" is conversational and less suitable for library defaults.
  Evidence: src/Humanizer/Properties/Resources.bg.resx:283 uses "{0} дена".; tests/Humanizer.Tests/Localisation/bg/TimeSpanHumanizeTests.cs:57 and :63 assert this colloquial form.
  Notes: Use "{0} дни" for formal, broadly acceptable output.
- [bn] `TimeSpanHumanize_SingleMonth`
  Current: এক মাসের
  Proposed: এক মাস
  Status: defect / Severity: P2 / Confidence: high
  Rationale: ‘এক মাসের’ এখানে অসম্পূর্ণ শোনায়; ‘-এর’ যোগে মালিকানা/সম্বন্ধ বোঝায় (যেমন ‘এক মাসের ছুটি’)। একক সময়সীমা হিসেবে স্বাভাবিক রূপ ‘এক মাস’।
  Evidence: src/Humanizer/Properties/Resources.bn.resx: TimeSpanHumanize_SingleMonth => এক মাসের; tests/Humanizer.Tests/Localisation/bn-BD/TimeSpanHumanizeTests.cs: [Trait("Translation", "Google")] on Months test suggests machine-translated string; Native usage: standalone duration in Bengali is 'এক মাস', not genitive 'এক মাসের'
- [bn] `TimeSpanHumanize_Zero`
  Current: শূন্য সময়
  Proposed: কোনো সময় নেই
  Status: defect / Severity: P3 / Confidence: medium
  Rationale: ‘শূন্য সময়’ বাংলা ব্যবহারে ক্যালক-ধাঁচের/অপ্রাকৃত শোনায়। ‘no time’ অর্থে প্রাকৃতিক রূপ ‘কোনো সময় নেই’।
  Evidence: src/Humanizer/Properties/Resources.bn.resx: TimeSpanHumanize_Zero => শূন্য সময়; tests/Humanizer.Tests/Localisation/bn-BD/TimeSpanHumanizeTests.cs: NoTimeToWords expects this key, with comment noting awkward phrasing; Native usage prefers absence construction for 'no time'
- [ca] `DateHumanize_MultipleDaysFromNow_Dual`
  Current: {0} dies a partir d'ara
  Proposed: d'aquÃ­ {0} dies
  Status: suspicious / Severity: P3 / Confidence: high
  Rationale: En catalÃ  estÃ ndard Ã©s mÃ©s idiomÃ tic i coherent amb la resta del fitxer fer servir la construcciÃ³ Â«d'aquÃ­ {0} ...Â» en lloc de Â«{0} ... a partir d'araÂ».
  Evidence: src/Humanizer/Properties/Resources.ca.resx: la forma base de futur per a segons/minuts/hores/dies/mesos/anys Ã©s majoritÃ riament Â«d'aquÃ­ ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament Â«d'aquÃ­ ...Â» (p. ex. Â«d'aquÃ­ 2 segonsÂ», Â«d'aquÃ­ 2 minutsÂ», Â«d'aquÃ­ 2 diesÂ»).
  Notes: Millora d'idiomaticitat i consistÃ¨ncia interna; no canvia el significat semÃ ntic.
- [ca] `DateHumanize_MultipleDaysFromNow_Plural`
  Current: {0} dies a partir d'ara
  Proposed: d'aquÃ­ {0} dies
  Status: suspicious / Severity: P3 / Confidence: high
  Rationale: En catalÃ  estÃ ndard Ã©s mÃ©s idiomÃ tic i coherent amb la resta del fitxer fer servir la construcciÃ³ Â«d'aquÃ­ {0} ...Â» en lloc de Â«{0} ... a partir d'araÂ».
  Evidence: src/Humanizer/Properties/Resources.ca.resx: la forma base de futur per a segons/minuts/hores/dies/mesos/anys Ã©s majoritÃ riament Â«d'aquÃ­ ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament Â«d'aquÃ­ ...Â» (p. ex. Â«d'aquÃ­ 2 segonsÂ», Â«d'aquÃ­ 2 minutsÂ», Â«d'aquÃ­ 2 diesÂ»).
  Notes: Millora d'idiomaticitat i consistÃ¨ncia interna; no canvia el significat semÃ ntic.
- [ca] `DateHumanize_MultipleDaysFromNow_Singular`
  Current: {0} dia a partir d'ara
  Proposed: d'aquÃ­ {0} dia
  Status: suspicious / Severity: P3 / Confidence: high
  Rationale: En catalÃ  estÃ ndard Ã©s mÃ©s idiomÃ tic i coherent amb la resta del fitxer fer servir la construcciÃ³ Â«d'aquÃ­ {0} ...Â» en lloc de Â«{0} ... a partir d'araÂ».
  Evidence: src/Humanizer/Properties/Resources.ca.resx: la forma base de futur per a segons/minuts/hores/dies/mesos/anys Ã©s majoritÃ riament Â«d'aquÃ­ ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament Â«d'aquÃ­ ...Â» (p. ex. Â«d'aquÃ­ 2 segonsÂ», Â«d'aquÃ­ 2 minutsÂ», Â«d'aquÃ­ 2 diesÂ»).
  Notes: Millora d'idiomaticitat i consistÃ¨ncia interna; no canvia el significat semÃ ntic.
- [ca] `DateHumanize_MultipleMinutesFromNow_Dual`
  Current: {0} minuts a partir d'ara
  Proposed: d'aquÃ­ {0} minuts
  Status: suspicious / Severity: P3 / Confidence: high
  Rationale: En catalÃ  estÃ ndard Ã©s mÃ©s idiomÃ tic i coherent amb la resta del fitxer fer servir la construcciÃ³ Â«d'aquÃ­ {0} ...Â» en lloc de Â«{0} ... a partir d'araÂ».
  Evidence: src/Humanizer/Properties/Resources.ca.resx: la forma base de futur per a segons/minuts/hores/dies/mesos/anys Ã©s majoritÃ riament Â«d'aquÃ­ ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament Â«d'aquÃ­ ...Â» (p. ex. Â«d'aquÃ­ 2 segonsÂ», Â«d'aquÃ­ 2 minutsÂ», Â«d'aquÃ­ 2 diesÂ»).
  Notes: Millora d'idiomaticitat i consistÃ¨ncia interna; no canvia el significat semÃ ntic.
- [ca] `DateHumanize_MultipleMinutesFromNow_Plural`
  Current: {0} minuts a partir d'ara
  Proposed: d'aquÃ­ {0} minuts
  Status: suspicious / Severity: P3 / Confidence: high
  Rationale: En catalÃ  estÃ ndard Ã©s mÃ©s idiomÃ tic i coherent amb la resta del fitxer fer servir la construcciÃ³ Â«d'aquÃ­ {0} ...Â» en lloc de Â«{0} ... a partir d'araÂ».
  Evidence: src/Humanizer/Properties/Resources.ca.resx: la forma base de futur per a segons/minuts/hores/dies/mesos/anys Ã©s majoritÃ riament Â«d'aquÃ­ ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament Â«d'aquÃ­ ...Â» (p. ex. Â«d'aquÃ­ 2 segonsÂ», Â«d'aquÃ­ 2 minutsÂ», Â«d'aquÃ­ 2 diesÂ»).
  Notes: Millora d'idiomaticitat i consistÃ¨ncia interna; no canvia el significat semÃ ntic.
- [ca] `DateHumanize_MultipleMinutesFromNow_Singular`
  Current: {0} minut a partir d'ara
  Proposed: d'aquÃ­ {0} minut
  Status: suspicious / Severity: P3 / Confidence: high
  Rationale: En catalÃ  estÃ ndard Ã©s mÃ©s idiomÃ tic i coherent amb la resta del fitxer fer servir la construcciÃ³ Â«d'aquÃ­ {0} ...Â» en lloc de Â«{0} ... a partir d'araÂ».
  Evidence: src/Humanizer/Properties/Resources.ca.resx: la forma base de futur per a segons/minuts/hores/dies/mesos/anys Ã©s majoritÃ riament Â«d'aquÃ­ ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament Â«d'aquÃ­ ...Â» (p. ex. Â«d'aquÃ­ 2 segonsÂ», Â«d'aquÃ­ 2 minutsÂ», Â«d'aquÃ­ 2 diesÂ»).
  Notes: Millora d'idiomaticitat i consistÃ¨ncia interna; no canvia el significat semÃ ntic.
- [ca] `DateHumanize_MultipleMonthsFromNow_Dual`
  Current: {0} mesos a partir d'ara
  Proposed: d'aquÃ­ {0} mesos
  Status: suspicious / Severity: P3 / Confidence: high
  Rationale: En catalÃ  estÃ ndard Ã©s mÃ©s idiomÃ tic i coherent amb la resta del fitxer fer servir la construcciÃ³ Â«d'aquÃ­ {0} ...Â» en lloc de Â«{0} ... a partir d'araÂ».
  Evidence: src/Humanizer/Properties/Resources.ca.resx: la forma base de futur per a segons/minuts/hores/dies/mesos/anys Ã©s majoritÃ riament Â«d'aquÃ­ ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament Â«d'aquÃ­ ...Â» (p. ex. Â«d'aquÃ­ 2 segonsÂ», Â«d'aquÃ­ 2 minutsÂ», Â«d'aquÃ­ 2 diesÂ»).
  Notes: Millora d'idiomaticitat i consistÃ¨ncia interna; no canvia el significat semÃ ntic.
- [ca] `DateHumanize_MultipleMonthsFromNow_Plural`
  Current: {0} mesos a partir d'ara
  Proposed: d'aquÃ­ {0} mesos
  Status: suspicious / Severity: P3 / Confidence: high
  Rationale: En catalÃ  estÃ ndard Ã©s mÃ©s idiomÃ tic i coherent amb la resta del fitxer fer servir la construcciÃ³ Â«d'aquÃ­ {0} ...Â» en lloc de Â«{0} ... a partir d'araÂ».
  Evidence: src/Humanizer/Properties/Resources.ca.resx: la forma base de futur per a segons/minuts/hores/dies/mesos/anys Ã©s majoritÃ riament Â«d'aquÃ­ ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament Â«d'aquÃ­ ...Â» (p. ex. Â«d'aquÃ­ 2 segonsÂ», Â«d'aquÃ­ 2 minutsÂ», Â«d'aquÃ­ 2 diesÂ»).
  Notes: Millora d'idiomaticitat i consistÃ¨ncia interna; no canvia el significat semÃ ntic.
- [ca] `DateHumanize_MultipleMonthsFromNow_Singular`
  Current: {0} mes a partir d'ara
  Proposed: d'aquÃ­ {0} mes
  Status: suspicious / Severity: P3 / Confidence: high
  Rationale: En catalÃ  estÃ ndard Ã©s mÃ©s idiomÃ tic i coherent amb la resta del fitxer fer servir la construcciÃ³ Â«d'aquÃ­ {0} ...Â» en lloc de Â«{0} ... a partir d'araÂ».
  Evidence: src/Humanizer/Properties/Resources.ca.resx: la forma base de futur per a segons/minuts/hores/dies/mesos/anys Ã©s majoritÃ riament Â«d'aquÃ­ ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament Â«d'aquÃ­ ...Â» (p. ex. Â«d'aquÃ­ 2 segonsÂ», Â«d'aquÃ­ 2 minutsÂ», Â«d'aquÃ­ 2 diesÂ»).
  Notes: Millora d'idiomaticitat i consistÃ¨ncia interna; no canvia el significat semÃ ntic.
- [ca] `DateHumanize_MultipleSecondsFromNow_Dual`
  Current: {0} segons a partir d'ara
  Proposed: d'aquÃ­ {0} segons
  Status: suspicious / Severity: P3 / Confidence: high
  Rationale: En catalÃ  estÃ ndard Ã©s mÃ©s idiomÃ tic i coherent amb la resta del fitxer fer servir la construcciÃ³ Â«d'aquÃ­ {0} ...Â» en lloc de Â«{0} ... a partir d'araÂ».
  Evidence: src/Humanizer/Properties/Resources.ca.resx: la forma base de futur per a segons/minuts/hores/dies/mesos/anys Ã©s majoritÃ riament Â«d'aquÃ­ ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament Â«d'aquÃ­ ...Â» (p. ex. Â«d'aquÃ­ 2 segonsÂ», Â«d'aquÃ­ 2 minutsÂ», Â«d'aquÃ­ 2 diesÂ»).
  Notes: Millora d'idiomaticitat i consistÃ¨ncia interna; no canvia el significat semÃ ntic.
- [ca] `DateHumanize_MultipleSecondsFromNow_Plural`
  Current: {0} segons a partir d'ara
  Proposed: d'aquÃ­ {0} segons
  Status: suspicious / Severity: P3 / Confidence: high
  Rationale: En catalÃ  estÃ ndard Ã©s mÃ©s idiomÃ tic i coherent amb la resta del fitxer fer servir la construcciÃ³ Â«d'aquÃ­ {0} ...Â» en lloc de Â«{0} ... a partir d'araÂ».
  Evidence: src/Humanizer/Properties/Resources.ca.resx: la forma base de futur per a segons/minuts/hores/dies/mesos/anys Ã©s majoritÃ riament Â«d'aquÃ­ ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament Â«d'aquÃ­ ...Â» (p. ex. Â«d'aquÃ­ 2 segonsÂ», Â«d'aquÃ­ 2 minutsÂ», Â«d'aquÃ­ 2 diesÂ»).
  Notes: Millora d'idiomaticitat i consistÃ¨ncia interna; no canvia el significat semÃ ntic.
- [ca] `DateHumanize_MultipleSecondsFromNow_Singular`
  Current: {0} segon a partir d'ara
  Proposed: d'aquÃ­ {0} segon
  Status: suspicious / Severity: P3 / Confidence: high
  Rationale: En catalÃ  estÃ ndard Ã©s mÃ©s idiomÃ tic i coherent amb la resta del fitxer fer servir la construcciÃ³ Â«d'aquÃ­ {0} ...Â» en lloc de Â«{0} ... a partir d'araÂ».
  Evidence: src/Humanizer/Properties/Resources.ca.resx: la forma base de futur per a segons/minuts/hores/dies/mesos/anys Ã©s majoritÃ riament Â«d'aquÃ­ ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament Â«d'aquÃ­ ...Â» (p. ex. Â«d'aquÃ­ 2 segonsÂ», Â«d'aquÃ­ 2 minutsÂ», Â«d'aquÃ­ 2 diesÂ»).
  Notes: Millora d'idiomaticitat i consistÃ¨ncia interna; no canvia el significat semÃ ntic.
- [ca] `DateHumanize_MultipleYearsFromNow_Dual`
  Current: {0} anys a partir d'ara
  Proposed: d'aquÃ­ {0} anys
  Status: suspicious / Severity: P3 / Confidence: high
  Rationale: En catalÃ  estÃ ndard Ã©s mÃ©s idiomÃ tic i coherent amb la resta del fitxer fer servir la construcciÃ³ Â«d'aquÃ­ {0} ...Â» en lloc de Â«{0} ... a partir d'araÂ».
  Evidence: src/Humanizer/Properties/Resources.ca.resx: la forma base de futur per a segons/minuts/hores/dies/mesos/anys Ã©s majoritÃ riament Â«d'aquÃ­ ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament Â«d'aquÃ­ ...Â» (p. ex. Â«d'aquÃ­ 2 segonsÂ», Â«d'aquÃ­ 2 minutsÂ», Â«d'aquÃ­ 2 diesÂ»).
  Notes: Millora d'idiomaticitat i consistÃ¨ncia interna; no canvia el significat semÃ ntic.
- [ca] `DateHumanize_MultipleYearsFromNow_Plural`
  Current: {0} anys a partir d'ara
  Proposed: d'aquÃ­ {0} anys
  Status: suspicious / Severity: P3 / Confidence: high
  Rationale: En catalÃ  estÃ ndard Ã©s mÃ©s idiomÃ tic i coherent amb la resta del fitxer fer servir la construcciÃ³ Â«d'aquÃ­ {0} ...Â» en lloc de Â«{0} ... a partir d'araÂ».
  Evidence: src/Humanizer/Properties/Resources.ca.resx: la forma base de futur per a segons/minuts/hores/dies/mesos/anys Ã©s majoritÃ riament Â«d'aquÃ­ ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament Â«d'aquÃ­ ...Â» (p. ex. Â«d'aquÃ­ 2 segonsÂ», Â«d'aquÃ­ 2 minutsÂ», Â«d'aquÃ­ 2 diesÂ»).
  Notes: Millora d'idiomaticitat i consistÃ¨ncia interna; no canvia el significat semÃ ntic.
- [ca] `DateHumanize_MultipleYearsFromNow_Singular`
  Current: {0} any a partir d'ara
  Proposed: d'aquÃ­ {0} any
  Status: suspicious / Severity: P3 / Confidence: high
  Rationale: En catalÃ  estÃ ndard Ã©s mÃ©s idiomÃ tic i coherent amb la resta del fitxer fer servir la construcciÃ³ Â«d'aquÃ­ {0} ...Â» en lloc de Â«{0} ... a partir d'araÂ».
  Evidence: src/Humanizer/Properties/Resources.ca.resx: la forma base de futur per a segons/minuts/hores/dies/mesos/anys Ã©s majoritÃ riament Â«d'aquÃ­ ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament Â«d'aquÃ­ ...Â» (p. ex. Â«d'aquÃ­ 2 segonsÂ», Â«d'aquÃ­ 2 minutsÂ», Â«d'aquÃ­ 2 diesÂ»).
  Notes: Millora d'idiomaticitat i consistÃ¨ncia interna; no canvia el significat semÃ ntic.
- [cs] `TimeSpanHumanize_Zero`
  Current: není čas
  Proposed: žádný čas
  Status: defect / Severity: P2 / Confidence: high
  Rationale: „Není čas“ v češtině znamená „nemáme čas“ (situace spěchu), ne nulovou délku trvání. Pro význam „no time“ jako nulový interval je přirozené „žádný čas“.
  Evidence: src/Humanizer/Properties/Resources.cs.resx: TimeSpanHumanize_Zero currently maps to "není čas".; tests/Humanizer.Tests/Localisation/cs/TimeSpanHumanizeTests.cs contains no zero-duration assertion, so this semantic mismatch is currently untested.
  Notes: Semantic correction for zero-duration output.
- [da] `DateHumanize_TwoDaysAgo`
  Current: forgårs
  Proposed: i forgårs
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: "forgårs" forstås, men i moderne dansk er "i forgårs" den klart mest idiomatiske og neutrale form til "two days ago".
  Evidence: src/Humanizer/Properties/Resources.da.resx: DateHumanize_TwoDaysAgo = "forgårs".; tests/Humanizer.Tests/Localisation/da/ResourcesTests.cs: asserts DateHumanize_TwoDaysAgo as "forgårs".
  Notes: Stilistisk/idiomatisk forbedring; nuværende formulering er forståelig, men lyder mere arkaiserende.
- [el] `DateHumanize_SingleHourFromNow`
  Current: Ãâ‚¬ÃÂÃŽÂ¯ÃŽÂ½ ÃŽÂ±Ãâ‚¬ÃÅ’ ÃŽÂ¼ÃŽÂ¯ÃŽÂ± ÃÅ½ÃÂÃŽÂ± ÃŽÂ±Ãâ‚¬ÃÅ’ Ãâ€žÃÅ½ÃÂÃŽÂ±
  Proposed: Î¼Î¯Î± ÏŽÏÎ± Î±Ï€ÏŒ Ï„ÏŽÏÎ±
  Status: defect / Severity: P1 / Confidence: high
  Rationale: Î— Ï„ÏÎ­Ï‡Î¿Ï…ÏƒÎ± Î±Ï€ÏŒÎ´Î¿ÏƒÎ· Ï‡ÏÎ·ÏƒÎ¹Î¼Î¿Ï€Î¿Î¹ÎµÎ¯ Ï„Î¿ Â«Ï€ÏÎ¹Î½ Î±Ï€ÏŒÂ» (Ï€Î±ÏÎµÎ»Î¸Î¿Î½Ï„Î¹ÎºÏŒÏ‚ Î´ÎµÎ¯ÎºÏ„Î·Ï‚) Î³Î¹Î± Î¼Î­Î»Î»Î¿Î½, Î¬ÏÎ± Î±Î½Ï„Î¹ÏƒÏ„ÏÎ­Ï†ÎµÎ¹ Ï„Î¿ Î½ÏŒÎ·Î¼Î± Ï„Î¿Ï… Î¼Î·Î½ÏÎ¼Î±Ï„Î¿Ï‚.
  Evidence: src/Humanizer/Properties/Resources.el.resx:DateHumanize_SingleHourFromNow=Ï€ÏÎ¯Î½ Î±Ï€ÏŒ Î¼Î¯Î± ÏŽÏÎ± Î±Ï€ÏŒ Ï„ÏŽÏÎ±; tests/Humanizer.Tests/Localisation/el/DateHumanizeTests.cs:HoursFromNow expects Ï€ÏÎ¯Î½ Î±Ï€ÏŒ Î¼Î¯Î± ÏŽÏÎ± Î±Ï€ÏŒ Ï„ÏŽÏÎ±
  Notes: Î”Î¹Î¿ÏÎ¸ÏŽÎ½ÎµÎ¹ ÎºÎ±Î¹ Ï„Î¿ Î¿ÏÎ¸Î¿Î³ÏÎ±Ï†Î¹ÎºÏŒ Â«Ï€ÏÎ¯Î½Â» -> Â«Ï€ÏÎ¹Î½Â».
- [el] `DateHumanize_SingleMinuteFromNow`
  Current: Ãâ‚¬ÃÂÃŽÂ¯ÃŽÂ½ ÃŽÂ±Ãâ‚¬ÃÅ’ ÃŽÂ­ÃŽÂ½ÃŽÂ± ÃŽÂ»ÃŽÂµÃâ‚¬Ãâ€žÃÅ’ ÃŽÂ±Ãâ‚¬ÃÅ’ Ãâ€žÃÅ½ÃÂÃŽÂ±
  Proposed: Î­Î½Î± Î»ÎµÏ€Ï„ÏŒ Î±Ï€ÏŒ Ï„ÏŽÏÎ±
  Status: defect / Severity: P1 / Confidence: high
  Rationale: Î Î±ÏÎµÎ»Î¸Î¿Î½Ï„Î¹ÎºÎ® Ï€ÏÏŒÎ¸ÎµÏƒÎ· (Â«Ï€ÏÎ¹Î½ Î±Ï€ÏŒÂ») ÏƒÎµ ÏƒÏ…Î¼Î²Î¿Î»Î¿ÏƒÎµÎ¹ÏÎ¬ Î¼ÎµÎ»Î»Î¿Î½Ï„Î¹ÎºÎ¿Ï Ï‡ÏÏŒÎ½Î¿Ï…Â· ÏƒÎ·Î¼Î±ÏƒÎ¹Î¿Î»Î¿Î³Î¹ÎºÎ¬ Î»Î±Î½Î¸Î±ÏƒÎ¼Î­Î½Î¿.
  Evidence: src/Humanizer/Properties/Resources.el.resx:DateHumanize_SingleMinuteFromNow=Ï€ÏÎ¯Î½ Î±Ï€ÏŒ Î­Î½Î± Î»ÎµÏ€Ï„ÏŒ Î±Ï€ÏŒ Ï„ÏŽÏÎ±; tests/Humanizer.Tests/Localisation/el/DateHumanizeTests.cs:MinutesFromNow expects Ï€ÏÎ¯Î½ Î±Ï€ÏŒ Î­Î½Î± Î»ÎµÏ€Ï„ÏŒ Î±Ï€ÏŒ Ï„ÏŽÏÎ±
  Notes: Î‘Ï€Î»Î® ÎºÎ±Î¹ Ï†Ï…ÏƒÎ¹ÎºÎ® Î¼ÎµÎ»Î»Î¿Î½Ï„Î¹ÎºÎ® Î´Î¹Î±Ï„ÏÏ€Ï‰ÏƒÎ·.
- [el] `DateHumanize_SingleMonthFromNow`
  Current: Ãâ‚¬ÃÂÃŽÂ¹ÃŽÂ½ ÃŽÂ±Ãâ‚¬ÃÅ’ ÃŽÂ­ÃŽÂ½ÃŽÂ±ÃŽÂ½ ÃŽÂ¼ÃŽÂ®ÃŽÂ½ÃŽÂ± ÃŽÂ±Ãâ‚¬ÃÅ’ Ãâ€žÃÅ½ÃÂÃŽÂ±
  Proposed: Î­Î½Î±Î½ Î¼Î®Î½Î± Î±Ï€ÏŒ Ï„ÏŽÏÎ±
  Status: defect / Severity: P1 / Confidence: high
  Rationale: Î¤Î¿ Â«Ï€ÏÎ¹Î½ Î±Ï€ÏŒÂ» Î´Î·Î»ÏŽÎ½ÎµÎ¹ Ï€Î±ÏÎµÎ»Î¸ÏŒÎ½ ÎºÎ±Î¹ ÏƒÏ…Î³ÎºÏÎ¿ÏÎµÏ„Î±Î¹ Î¼Îµ Ï„Î¿ ÎºÎ»ÎµÎ¹Î´Î¯ FromNow.
  Evidence: src/Humanizer/Properties/Resources.el.resx:DateHumanize_SingleMonthFromNow=Ï€ÏÎ¹Î½ Î±Ï€ÏŒ Î­Î½Î±Î½ Î¼Î®Î½Î± Î±Ï€ÏŒ Ï„ÏŽÏÎ±; tests/Humanizer.Tests/Localisation/el/DateHumanizeTests.cs:MonthsFromNow expects Ï€ÏÎ¹Î½ Î±Ï€ÏŒ Î­Î½Î±Î½ Î¼Î®Î½Î± Î±Ï€ÏŒ Ï„ÏŽÏÎ±
  Notes: Î”Î¹Î±Ï„Î·ÏÎµÎ¯ Ï„Î·Î½ ÎºÎ»Î¹Ï„Î¹ÎºÎ® Î¼Î¿ÏÏ†Î® Â«Î­Î½Î±Î½ Î¼Î®Î½Î±Â».
- [el] `DateHumanize_SingleSecondFromNow`
  Current: Ãâ‚¬ÃÂÃŽÂ¹ÃŽÂ½ ÃŽÂ±Ãâ‚¬ÃÅ’ ÃŽÂ­ÃŽÂ½ÃŽÂ± ÃŽÂ´ÃŽÂµÃâ€¦Ãâ€žÃŽÂµÃÂÃÅ’ÃŽÂ»ÃŽÂµÃâ‚¬Ãâ€žÃŽÂ¿ ÃŽÂ±Ãâ‚¬ÃÅ’ Ãâ€žÃÅ½ÃÂÃŽÂ±
  Proposed: Î­Î½Î± Î´ÎµÏ…Ï„ÎµÏÏŒÎ»ÎµÏ€Ï„Î¿ Î±Ï€ÏŒ Ï„ÏŽÏÎ±
  Status: defect / Severity: P1 / Confidence: high
  Rationale: Î§ÏÎ¿Î½Î¹ÎºÏŒÏ‚ Î´ÎµÎ¯ÎºÏ„Î·Ï‚ Ï€Î±ÏÎµÎ»Î¸ÏŒÎ½Ï„Î¿Ï‚ Î±Î½Ï„Î¯ Î³Î¹Î± Î¼Î­Î»Î»Î¿Î½, Î¬ÏÎ± Î»Î¬Î¸Î¿Ï‚ ÏƒÎ·Î¼Î±ÏƒÎ¯Î±.
  Evidence: src/Humanizer/Properties/Resources.el.resx:DateHumanize_SingleSecondFromNow=Ï€ÏÎ¹Î½ Î±Ï€ÏŒ Î­Î½Î± Î´ÎµÏ…Ï„ÎµÏÏŒÎ»ÎµÏ€Ï„Î¿ Î±Ï€ÏŒ Ï„ÏŽÏÎ±; tests/Humanizer.Tests/Localisation/el/DateHumanizeTests.cs:SecondsFromNow expects Ï€ÏÎ¹Î½ Î±Ï€ÏŒ Î­Î½Î± Î´ÎµÏ…Ï„ÎµÏÏŒÎ»ÎµÏ€Ï„Î¿ Î±Ï€ÏŒ Ï„ÏŽÏÎ±
  Notes: Î•Ï…Î¸Ï…Î³ÏÎ±Î¼Î¼Î¯Î¶ÎµÏ„Î±Î¹ Î¼Îµ Ï„Î± Ï€Î»Î·Î¸Ï…Î½Ï„Î¹ÎºÎ¬ FromNow.
- [el] `DateHumanize_SingleYearFromNow`
  Current: Ãâ‚¬ÃÂÃŽÂ¹ÃŽÂ½ ÃŽÂ±Ãâ‚¬ÃÅ’ ÃŽÂ­ÃŽÂ½ÃŽÂ±ÃŽÂ½ Ãâ€¡ÃÂÃÅ’ÃŽÂ½ÃŽÂ¿ ÃŽÂ±Ãâ‚¬ÃÅ’ Ãâ€žÃÅ½ÃÂÃŽÂ±
  Proposed: Î­Î½Î±Î½ Ï‡ÏÏŒÎ½Î¿ Î±Ï€ÏŒ Ï„ÏŽÏÎ±
  Status: defect / Severity: P1 / Confidence: high
  Rationale: Î§ÏÎ®ÏƒÎ· Â«Ï€ÏÎ¹Î½ Î±Ï€ÏŒÂ» ÏƒÎµ Î¼Î®Î½Ï…Î¼Î± Î¼Î­Î»Î»Î¿Î½Ï„Î¿Ï‚ Ï€ÏÎ¿ÎºÎ±Î»ÎµÎ¯ Î±Î½Ï„Î¹ÏƒÏ„ÏÎ¿Ï†Î® Ï‡ÏÎ¿Î½Î¹ÎºÎ®Ï‚ ÎºÎ±Ï„ÎµÏÎ¸Ï…Î½ÏƒÎ·Ï‚.
  Evidence: src/Humanizer/Properties/Resources.el.resx:DateHumanize_SingleYearFromNow=Ï€ÏÎ¹Î½ Î±Ï€ÏŒ Î­Î½Î±Î½ Ï‡ÏÏŒÎ½Î¿ Î±Ï€ÏŒ Ï„ÏŽÏÎ±; tests/Humanizer.Tests/Localisation/el/DateHumanizeTests.cs:YearsFromNow expects Ï€ÏÎ¹Î½ Î±Ï€ÏŒ Î­Î½Î±Î½ Ï‡ÏÏŒÎ½Î¿ Î±Ï€ÏŒ Ï„ÏŽÏÎ±
  Notes: Î¦Ï…ÏƒÎ¹ÎºÎ® Î¿Ï…Î´Î­Ï„ÎµÏÎ· Î±Ï€ÏŒÎ´Î¿ÏƒÎ· ÏƒÎµ Î½Î­Î± ÎµÎ»Î»Î·Î½Î¹ÎºÎ¬.
- [el] `TimeSpanHumanize_SingleMillisecond`
  Current: 1 Ãâ€¡ÃŽÂ¹ÃŽÂ»ÃŽÂ¹ÃŽÂ¿ÃÆ’tÃÅ’ Ãâ€žÃŽÂ¿Ãâ€¦ ÃŽÂ´ÃŽÂµÃâ€¦Ãâ€žÃŽÂµÃÂÃŽÂ¿ÃŽÂ»ÃŽÂ­Ãâ‚¬Ãâ€žÃŽÂ¿Ãâ€¦
  Proposed: 1 Ï‡Î¹Î»Î¹Î¿ÏƒÏ„ÏŒ Ï„Î¿Ï… Î´ÎµÏ…Ï„ÎµÏÎ¿Î»Î­Ï€Ï„Î¿Ï…
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Î¥Ï€Î¬ÏÏ‡ÎµÎ¹ Î¾Î­Î½Î¿Ï‚ Î»Î±Ï„Î¹Î½Î¹ÎºÏŒÏ‚ Ï‡Î±ÏÎ±ÎºÏ„Î®ÏÎ±Ï‚ (t) ÏƒÏ„Î· Î»Î­Î¾Î· Â«Ï‡Î¹Î»Î¹Î¿ÏƒtÏŒÂ», Ï€Î¿Ï… Î±Ï€Î¿Ï„ÎµÎ»ÎµÎ¯ Ï„Ï…Ï€Î¿Î³ÏÎ±Ï†Î¹ÎºÏŒ ÏƒÏ†Î¬Î»Î¼Î±.
  Evidence: src/Humanizer/Properties/Resources.el.resx:TimeSpanHumanize_SingleMillisecond=1 Ï‡Î¹Î»Î¹Î¿ÏƒtÏŒ Ï„Î¿Ï… Î´ÎµÏ…Ï„ÎµÏÎ¿Î»Î­Ï€Ï„Î¿Ï…; tests/Humanizer.Tests/Localisation/el/TimeSpanHumanizeTests.cs:Milliseconds expects 1 Ï‡Î¹Î»Î¹Î¿ÏƒtÏŒ Ï„Î¿Ï… Î´ÎµÏ…Ï„ÎµÏÎ¿Î»Î­Ï€Ï„Î¿Ï…
  Notes: Î‘Ï€Î»Î® Î¿ÏÎ¸Î¿Î³ÏÎ±Ï†Î¹ÎºÎ® Î±Ï€Î¿ÎºÎ±Ï„Î¬ÏƒÏ„Î±ÏƒÎ· Ï‡Ï‰ÏÎ¯Ï‚ Î±Î»Î»Î±Î³Î® Î½Î¿Î®Î¼Î±Ï„Î¿Ï‚.
- [el] `TimeSpanHumanize_Zero`
  Current: ÃŽÂ¼ÃŽÂ·ÃŽÂ´ÃŽÂ­ÃŽÂ½ Ãâ€¡ÃÂÃÅ’ÃŽÂ½ÃŽÂ¿Ãâ€š
  Proposed: Î¼Î·Î´ÎµÎ½Î¹ÎºÏŒÏ‚ Ï‡ÏÏŒÎ½Î¿Ï‚
  Status: defect / Severity: P2 / Confidence: medium
  Rationale: Î¤Î¿ Â«Î¼Î·Î´Î­Î½ Ï‡ÏÏŒÎ½Î¿Ï‚Â» ÎµÎ¯Î½Î±Î¹ Î±Î½Ï„Î¹Ï†Ï…ÏƒÎ¹ÎºÏŒ/Î±Î½Ï„Î¹Î³ÏÎ±Î¼Î¼Î±Ï„Î¹ÎºÏŒ Ï‰Ï‚ Î¿Î½Î¿Î¼Î±Ï„Î¹ÎºÎ® Ï†ÏÎ¬ÏƒÎ· ÏƒÏ„Î± ÎµÎ»Î»Î·Î½Î¹ÎºÎ¬.
  Evidence: src/Humanizer/Properties/Resources.el.resx:TimeSpanHumanize_Zero=Î¼Î·Î´Î­Î½ Ï‡ÏÏŒÎ½Î¿Ï‚; tests/Humanizer.Tests/Localisation/el/TimeSpanHumanizeTests.cs:NoTimeToWords expects Î¼Î·Î´Î­Î½ Ï‡ÏÏŒÎ½Î¿Ï‚
  Notes: Î•Î½Î±Î»Î»Î±ÎºÏ„Î¹ÎºÎ¬ Î¸Î± Î¼Ï€Î¿ÏÎ¿ÏÏƒÎµ Î½Î± Ï‡ÏÎ·ÏƒÎ¹Î¼Î¿Ï€Î¿Î¹Î·Î¸ÎµÎ¯ Â«ÎºÎ±Î¸ÏŒÎ»Î¿Ï… Ï‡ÏÏŒÎ½Î¿Ï‚Â», Î±Î»Î»Î¬ ÎµÎ¯Î½Î±Î¹ Ï€Î¹Î¿ Î¹Î´Î¹Ï‰Î¼Î±Ï„Î¹ÎºÏŒ.
- [es] `DateHumanize_MultipleDaysFromNow_Dual`
  Current: hace {0} días desde ahora
  Proposed: en {0} días
  Status: defect / Severity: P2 / Confidence: high
  Rationale: La clave es de futuro (from now), pero la cadena actual usa una construcción de pasado ('hace ...'). En español natural debe expresarse con 'en ...'.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleDaysFromNow*).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (DaysFromNow usa 'en ...').
  Notes: Se alinea con la variante base DateHumanize_MultipleDaysFromNow.
- [es] `DateHumanize_MultipleDaysFromNow_Plural`
  Current: hace {0} días desde ahora
  Proposed: en {0} días
  Status: defect / Severity: P2 / Confidence: high
  Rationale: La cadena mezcla pasado ('hace') con una clave de futuro ('from now'). La formulación idiomática es 'en {0} días'.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleDaysFromNow*).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (DaysFromNow).
  Notes: Corrección de tiempo verbal.
- [es] `DateHumanize_MultipleDaysFromNow_Singular`
  Current: {0} día desde ahora
  Proposed: en {0} día
  Status: defect / Severity: P2 / Confidence: high
  Rationale: La frase actual es forzada y poco natural en español. Para futuro inmediato en Humanizer se usa el patrón 'en ...'.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleDaysFromNow_Singular).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (futuro con 'en').
  Notes: Se normaliza al mismo patrón de las demás formas de futuro.
- [es] `DateHumanize_MultipleHoursFromNow_Dual`
  Current: hace {0} horas desde ahora
  Proposed: en {0} horas
  Status: defect / Severity: P2 / Confidence: high
  Rationale: La expresión actual contradice el sentido de futuro de la clave; 'hace' indica pasado.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleHoursFromNow_Dual).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (HoursFromNow).
  Notes: Se usa el patrón natural de futuro: 'en {0} horas'.
- [es] `DateHumanize_MultipleHoursFromNow_Paucal`
  Current: hace {0} horas desde ahora
  Proposed: en {0} horas
  Status: defect / Severity: P2 / Confidence: high
  Rationale: La forma actual está en pasado y no corresponde a una clave de futuro.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleHoursFromNow_Paucal).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (HoursFromNow).
  Notes: Corrección de tiempo verbal.
- [es] `DateHumanize_MultipleHoursFromNow_Plural`
  Current: hace {0} horas desde ahora
  Proposed: en {0} horas
  Status: defect / Severity: P2 / Confidence: high
  Rationale: La redacción actual mezcla pasado con futuro y suena no nativa.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleHoursFromNow_Plural).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (HoursFromNow).
  Notes: Consistencia con DateHumanize_MultipleHoursFromNow.
- [es] `DateHumanize_MultipleHoursFromNow_Singular`
  Current: hace {0} hora desde ahora
  Proposed: en {0} hora
  Status: defect / Severity: P2 / Confidence: high
  Rationale: La estructura 'hace ... desde ahora' es incorrecta para futuro en español.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleHoursFromNow_Singular).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (HoursFromNow).
  Notes: Se sustituye por la fórmula idiomática 'en ...'.
- [es] `DateHumanize_MultipleMinutesFromNow_Dual`
  Current: {0} minutos desde ahora
  Proposed: en {0} minutos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Aunque entendible, no sigue la forma idiomática usada en español para futuro relativo en Humanizer ('en ...').
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleMinutesFromNow_Dual).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (MinutesFromNow).
  Notes: Ajuste de estilo y consistencia.
- [es] `DateHumanize_MultipleMinutesFromNow_Paucal`
  Current: hace {0} minutos desde ahora
  Proposed: en {0} minutos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: La cadena actual usa marcador de pasado ('hace') en una clave de futuro.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleMinutesFromNow_Paucal).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (MinutesFromNow).
  Notes: Corrección de tiempo verbal.
- [es] `DateHumanize_MultipleMinutesFromNow_Plural`
  Current: hace {0} minutos desde ahora
  Proposed: en {0} minutos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: La redacción es semánticamente incorrecta para una referencia temporal futura.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleMinutesFromNow_Plural).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (MinutesFromNow).
  Notes: Se normaliza a 'en {0} minutos'.
- [es] `DateHumanize_MultipleMinutesFromNow_Singular`
  Current: hace {0} minuto desde ahora
  Proposed: en {0} minuto
  Status: defect / Severity: P2 / Confidence: high
  Rationale: La cadena combina pasado y futuro de forma antinatural; en español estándar debe ir con 'en'.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleMinutesFromNow_Singular).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (MinutesFromNow).
  Notes: Consistencia con el resto de variantes FromNow.
- [es] `DateHumanize_MultipleMonthsAgo_Dual`
  Current: hace {0} meses desde ahora
  Proposed: hace {0} meses
  Status: defect / Severity: P2 / Confidence: high
  Rationale: La clave es de pasado ('ago') y no debe incluir 'desde ahora', que expresa futuro relativo.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleMonthsAgo_Dual).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (MonthsAgo).
  Notes: Se alinea con DateHumanize_MultipleMonthsAgo.
- [es] `DateHumanize_MultipleMonthsAgo_Singular`
  Current: hace {0} meses
  Proposed: hace {0} mes
  Status: defect / Severity: P2 / Confidence: high
  Rationale: La forma singular debe usar 'mes', no 'meses'.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleMonthsAgo_Singular).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (MonthsAgo singular usa 'un mes').
  Notes: Corrección de concordancia de número.
- [es] `DateHumanize_MultipleMonthsFromNow_Dual`
  Current: hace {0} meses desde ahora
  Proposed: en {0} meses
  Status: defect / Severity: P2 / Confidence: high
  Rationale: La cadena actual usa un giro de pasado para una clave de futuro; en español debe ser 'en {0} meses'.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleMonthsFromNow_Dual).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (MonthsFromNow).
  Notes: Consistencia con DateHumanize_MultipleMonthsFromNow.
- [es] `DateHumanize_MultipleMonthsFromNow_Paucal`
  Current: hace {0} meses desde ahora
  Proposed: en {0} meses
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Expresión temporal incorrecta: marca pasado cuando la clave expresa futuro.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleMonthsFromNow_Paucal).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (MonthsFromNow).
  Notes: Corrección de tiempo verbal.
- [es] `DateHumanize_MultipleMonthsFromNow_Plural`
  Current: hace {0} meses desde ahora
  Proposed: en {0} meses
  Status: defect / Severity: P2 / Confidence: high
  Rationale: La construcción actual suena no nativa y contradice el sentido de 'from now'.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleMonthsFromNow_Plural).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (MonthsFromNow).
  Notes: Se normaliza a la forma idiomática de futuro.
- [es] `DateHumanize_MultipleMonthsFromNow_Singular`
  Current: hace {0} mes desde ahora
  Proposed: en {0} mes
  Status: defect / Severity: P2 / Confidence: high
  Rationale: La forma actual mezcla pasado y futuro. Para español natural se usa 'en {0} mes'.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleMonthsFromNow_Singular).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (MonthsFromNow).
  Notes: Consistencia con el resto de variantes FromNow.
- [es] `DateHumanize_MultipleSecondsFromNow_Dual`
  Current: hace {0} segundos desde ahora
  Proposed: en {0} segundos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: La clave requiere futuro relativo y la cadena actual está en pasado.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleSecondsFromNow_Dual).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (SecondsFromNow).
  Notes: Corrección de tiempo verbal.
- [es] `DateHumanize_MultipleSecondsFromNow_Paucal`
  Current: hace {0} segundos desde ahora
  Proposed: en {0} segundos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: La frase no es idiomática para futuro; 'hace' marca pasado.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleSecondsFromNow_Paucal).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (SecondsFromNow).
  Notes: Alineado con la variante principal de futuro.
- [es] `DateHumanize_MultipleSecondsFromNow_Plural`
  Current: hace {0} segundos desde ahora
  Proposed: en {0} segundos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: El contenido expresa pasado y la clave expresa futuro.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleSecondsFromNow_Plural).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (SecondsFromNow).
  Notes: Se corrige al patrón estándar en español.
- [es] `DateHumanize_MultipleSecondsFromNow_Singular`
  Current: hace {0} segundo desde ahora
  Proposed: en {0} segundo
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Construcción incorrecta para referencia futura; debe ser 'en {0} segundo'.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleSecondsFromNow_Singular).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (SecondsFromNow).
  Notes: Consistencia con el resto de variantes FromNow.
- [es] `DateHumanize_MultipleYearsFromNow_Dual`
  Current: hace {0} años desde ahora
  Proposed: en {0} años
  Status: defect / Severity: P2 / Confidence: high
  Rationale: La forma actual contradice la semántica de futuro de la clave.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleYearsFromNow_Dual).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (YearsFromNow).
  Notes: Corrección de tiempo verbal.
- [es] `DateHumanize_MultipleYearsFromNow_Paucal`
  Current: hace {0} años desde ahora
  Proposed: en {0} años
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Usa 'hace' (pasado) en una variante de futuro.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleYearsFromNow_Paucal).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (YearsFromNow).
  Notes: Consistencia con DateHumanize_MultipleYearsFromNow.
- [es] `DateHumanize_MultipleYearsFromNow_Plural`
  Current: hace {0} años desde ahora
  Proposed: en {0} años
  Status: defect / Severity: P2 / Confidence: high
  Rationale: La cadena no es natural en español para futuro relativo.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleYearsFromNow_Plural).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (YearsFromNow).
  Notes: Se estandariza al patrón 'en ...'.
- [es] `DateHumanize_MultipleYearsFromNow_Singular`
  Current: hace {0} año desde ahora
  Proposed: en {0} año
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Construcción gramaticalmente forzada y en tiempo verbal incorrecto para una clave de futuro.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleYearsFromNow_Singular).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (YearsFromNow).
  Notes: Corrección idiomática para español nativo.
- [fa] `TimeSpanHumanize_MultipleMilliseconds`
  Current: {0} Ù…ÛŒÙ„ÛŒ Ø«Ø§Ù†ÛŒÙ‡
  Proposed: {0} Ù…ÛŒÙ„ÛŒâ€ŒØ«Ø§Ù†ÛŒÙ‡
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Ø¯Ø± ÙØ§Ø±Ø³ÛŒ Ù…Ø¹ÛŒØ§Ø±ØŒ ØªØ±Ú©ÛŒØ¨ Â«Ù…ÛŒÙ„ÛŒâ€ŒØ«Ø§Ù†ÛŒÙ‡Â» Ø¨Ø§ Ù†ÛŒÙ…â€ŒÙØ§ØµÙ„Ù‡ Ù†ÙˆØ´ØªÙ‡ Ù…ÛŒâ€ŒØ´ÙˆØ¯. Ø´Ú©Ù„ Ø¨Ø§ ÙØ§ØµÙ„Ù‡Ù” Ú©Ø§Ù…Ù„ Ù†Ø§Ù‡Ù…Ø§Ù‡Ù†Ú¯ Ùˆ Ø§Ø² Ù†Ø¸Ø± Ù†Ú¯Ø§Ø±Ø´ÛŒ Ù†Ø§Ø¯Ø±Ø³Øª Ø§Ø³ØªØŒ Ø¨Ù‡â€ŒØ®ØµÙˆØµ ÙˆÙ‚ØªÛŒ Ù‡Ù…ÛŒÙ† ÙØ§ÛŒÙ„ Ø¨Ø±Ø§ÛŒ TimeUnit Ù‡Ù…ÛŒÙ† ÙˆØ§Ú˜Ù‡ Ø±Ø§ Ø¨Ø§ Ù†ÛŒÙ…â€ŒÙØ§ØµÙ„Ù‡ Ø¢ÙˆØ±Ø¯Ù‡ Ø§Ø³Øª.
  Evidence: src/Humanizer/Properties/Resources.fa.resx: TimeSpanHumanize_MultipleMilliseconds={0} Ù…ÛŒÙ„ÛŒ Ø«Ø§Ù†ÛŒÙ‡; src/Humanizer/Properties/Resources.fa.resx: TimeSpanHumanize_SingleMillisecond=ÛŒÚ© Ù…ÛŒÙ„ÛŒ Ø«Ø§Ù†ÛŒÙ‡; src/Humanizer/Properties/Resources.fa.resx: TimeUnit_Millisecond=Ù…ÛŒÙ„ÛŒâ€ŒØ«Ø§Ù†ÛŒÙ‡; tests/Humanizer.Tests/Localisation/fa/TimeSpanHumanizeTests.cs: expects "ÛŒÚ© Ù…ÛŒÙ„ÛŒ Ø«Ø§Ù†ÛŒÙ‡"; tests/Humanizer.Tests/Localisation/fa/TimeUnitToSymbolTests.cs: expects "Ù…ÛŒÙ„ÛŒâ€ŒØ«Ø§Ù†ÛŒÙ‡"
  Notes: Orthographic consistency issue (space vs ZWNJ) within the same locale pack.
- [fa] `TimeSpanHumanize_SingleMillisecond`
  Current: ÛŒÚ© Ù…ÛŒÙ„ÛŒ Ø«Ø§Ù†ÛŒÙ‡
  Proposed: ÛŒÚ© Ù…ÛŒÙ„ÛŒâ€ŒØ«Ø§Ù†ÛŒÙ‡
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Ø¯Ø± ÙØ§Ø±Ø³ÛŒ Ù…Ø¹ÛŒØ§Ø±ØŒ ØªØ±Ú©ÛŒØ¨ Â«Ù…ÛŒÙ„ÛŒâ€ŒØ«Ø§Ù†ÛŒÙ‡Â» Ø¨Ø§ Ù†ÛŒÙ…â€ŒÙØ§ØµÙ„Ù‡ Ù†ÙˆØ´ØªÙ‡ Ù…ÛŒâ€ŒØ´ÙˆØ¯. Ø´Ú©Ù„ Ø¨Ø§ ÙØ§ØµÙ„Ù‡Ù” Ú©Ø§Ù…Ù„ Ù†Ø§Ù‡Ù…Ø§Ù‡Ù†Ú¯ Ùˆ Ø§Ø² Ù†Ø¸Ø± Ù†Ú¯Ø§Ø±Ø´ÛŒ Ù†Ø§Ø¯Ø±Ø³Øª Ø§Ø³ØªØŒ Ø¨Ù‡â€ŒØ®ØµÙˆØµ ÙˆÙ‚ØªÛŒ Ù‡Ù…ÛŒÙ† ÙØ§ÛŒÙ„ Ø¨Ø±Ø§ÛŒ TimeUnit Ù‡Ù…ÛŒÙ† ÙˆØ§Ú˜Ù‡ Ø±Ø§ Ø¨Ø§ Ù†ÛŒÙ…â€ŒÙØ§ØµÙ„Ù‡ Ø¢ÙˆØ±Ø¯Ù‡ Ø§Ø³Øª.
  Evidence: src/Humanizer/Properties/Resources.fa.resx: TimeSpanHumanize_MultipleMilliseconds={0} Ù…ÛŒÙ„ÛŒ Ø«Ø§Ù†ÛŒÙ‡; src/Humanizer/Properties/Resources.fa.resx: TimeSpanHumanize_SingleMillisecond=ÛŒÚ© Ù…ÛŒÙ„ÛŒ Ø«Ø§Ù†ÛŒÙ‡; src/Humanizer/Properties/Resources.fa.resx: TimeUnit_Millisecond=Ù…ÛŒÙ„ÛŒâ€ŒØ«Ø§Ù†ÛŒÙ‡; tests/Humanizer.Tests/Localisation/fa/TimeSpanHumanizeTests.cs: expects "ÛŒÚ© Ù…ÛŒÙ„ÛŒ Ø«Ø§Ù†ÛŒÙ‡"; tests/Humanizer.Tests/Localisation/fa/TimeUnitToSymbolTests.cs: expects "Ù…ÛŒÙ„ÛŒâ€ŒØ«Ø§Ù†ÛŒÙ‡"
  Notes: Orthographic consistency issue (space vs ZWNJ) within the same locale pack.
- [fi] `DateHumanize_SingleSecondAgo`
  Current: sekuntti sitten
  Proposed: sekunti sitten
  Status: defect / Severity: P1 / Confidence: high
  Rationale: "Sekuntti" on kirjoitusvirhe eikÃ¤ ole norminmukainen sana. Oikea suomen kielen muoto on "sekunti", joten idiomaattinen ja oikein taivutettu kÃ¤Ã¤nnÃ¶s on "sekunti sitten".
  Evidence: src/Humanizer/Properties/Resources.fi.resx: DateHumanize_SingleSecondAgo = "sekuntti sitten"; tests/Humanizer.Tests/Localisation/fi-FI/DateHumanizeTests.cs: [InlineData(1, "sekuntti sitten")]
  Notes: Typo appears in both resource and assertion; child culture fi-FI currently inherits this incorrect value from fi.
- [fil] `TimeSpanHumanize_Age`
  Current: {0} gulang
  Proposed: {0} na gulang
  Status: suspicious / Severity: P2 / Confidence: medium
  Rationale: Mas natural sa Filipino ang may pang-angkop dito; "{0} gulang" ay nauunawaan pero tunog salin at bitin sa daloy.
  Evidence: src/Humanizer/Properties/Resources.fil.resx:465; tests/Humanizer.Tests/Localisation/fil-PH/TimeSpanHumanizeTests.cs:80-81
  Notes: Owned child-culture fallback (fil-PH) currently asserts the awkward form.
- [fil] `TimeSpanHumanize_MultipleMilliseconds`
  Current: {0} milliseconds
  Proposed: {0} milisegundo
  Status: defect / Severity: P1 / Confidence: high
  Rationale: May halatang English leakage ("milliseconds"). Karaniwang Filipino localization: "milisegundo" at hindi kailangang English plural suffix.
  Evidence: src/Humanizer/Properties/Resources.fil.resx:375-376; tests/Humanizer.Tests/Localisation/fil-PH/TimeSpanHumanizeTests.cs:63-66
  Notes: Inherited by fil-PH via fallback.
- [fil] `TimeSpanHumanize_MultipleMilliseconds_Dual`
  Current: {0} milliseconds
  Proposed: {0} milisegundo
  Status: defect / Severity: P1 / Confidence: high
  Rationale: May halatang English leakage ("milliseconds"). Dapat Filipino term at pare-pareho sa ibang plural-form keys.
  Evidence: src/Humanizer/Properties/Resources.fil.resx:378-379; tests/Humanizer.Tests/Localisation/fil-PH/TimeSpanHumanizeTests.cs:63-66
  Notes: Inherited by fil-PH via fallback.
- [fil] `TimeSpanHumanize_MultipleMilliseconds_Paucal`
  Current: {0} milliseconds
  Proposed: {0} milisegundo
  Status: defect / Severity: P1 / Confidence: high
  Rationale: May halatang English leakage ("milliseconds"). Dapat Filipino term at pare-pareho sa ibang plural-form keys.
  Evidence: src/Humanizer/Properties/Resources.fil.resx:381-382; tests/Humanizer.Tests/Localisation/fil-PH/TimeSpanHumanizeTests.cs:63-66
  Notes: Inherited by fil-PH via fallback.
- [fil] `TimeSpanHumanize_MultipleMilliseconds_Plural`
  Current: {0} milliseconds
  Proposed: {0} milisegundo
  Status: defect / Severity: P1 / Confidence: high
  Rationale: May halatang English leakage ("milliseconds"). Dapat Filipino term at pare-pareho sa ibang plural-form keys.
  Evidence: src/Humanizer/Properties/Resources.fil.resx:384-385; tests/Humanizer.Tests/Localisation/fil-PH/TimeSpanHumanizeTests.cs:63-66
  Notes: Inherited by fil-PH via fallback.
- [fil] `TimeSpanHumanize_MultipleMilliseconds_Singular`
  Current: {0} millisecond
  Proposed: {0} milisegundo
  Status: defect / Severity: P1 / Confidence: high
  Rationale: May halatang English leakage ("millisecond"). Sa Filipino, mas natural at konsistent ang "milisegundo".
  Evidence: src/Humanizer/Properties/Resources.fil.resx:387-388; tests/Humanizer.Tests/Localisation/fil-PH/TimeSpanHumanizeTests.cs:63-66
  Notes: Inherited by fil-PH via fallback.
- [fil] `TimeSpanHumanize_SingleMillisecond`
  Current: 1 millisecond
  Proposed: 1 milisegundo
  Status: defect / Severity: P1 / Confidence: high
  Rationale: English term pa rin ang gamit. Sa Filipino localization, dapat "milisegundo".
  Evidence: src/Humanizer/Properties/Resources.fil.resx:480-481; tests/Humanizer.Tests/Localisation/fil-PH/TimeSpanHumanizeTests.cs:63-66
  Notes: Inherited by fil-PH via fallback.
- [fil] `TimeSpanHumanize_SingleMillisecond_Words`
  Current: isang millisecond
  Proposed: isang milisegundo
  Status: defect / Severity: P1 / Confidence: high
  Rationale: English term pa rin ang gamit. Sa natural na Filipino phrasing, "isang milisegundo" ang mas tama.
  Evidence: src/Humanizer/Properties/Resources.fil.resx:483-484; tests/Humanizer.Tests/Localisation/fil-PH/TimeSpanHumanizeTests.cs:89-90
  Notes: Inherited by fil-PH via fallback.
- [fr] `TimeSpanHumanize_Zero`
  Current: temps nul
  Proposed: aucune durÃ©e
  Status: defect / Severity: P2 / Confidence: high
  Rationale: 'temps nul' n'est pas idiomatique en franÃ§ais produit; pour exprimer une durÃ©e nulle, 'aucune durÃ©e' est naturel et immÃ©diatement comprÃ©hensible.
  Evidence: src/Humanizer/Properties/Resources.fr.resx: TimeSpanHumanize_Zero=temps nul; tests/Humanizer.Tests/Localisation/fr/TimeSpanHumanizeTests.cs: NoTimeToWords attend actuellement temps nul; tests/Humanizer.Tests/Localisation/fr-BE/TimeSpanHumanizeTests.cs: NoTimeToWords hÃ©rite aussi de temps nul (fallback parent)
  Notes: Impact fallback: fr-BE et fr-CH hÃ©ritent cette chaÃ®ne; correction parent amÃ©liorerait les enfants sans override.
- [he] `E_Short`
  Current: מ
  Proposed: מז
  Status: defect / Severity: P1 / Confidence: high
  Rationale: הקיצור הנוכחי זהה ל-W_Short ולכן אינו מבחין בין מזרח למערב. בעברית מקובל לקצר מזרח ל"מז" ומערב ל"מע" כדי למנוע דו-משמעות.
  Evidence: src/Humanizer/Properties/Resources.he.resx: E_Short='מ' and W_Short='מ'; tests/Humanizer.Tests/Localisation/he/HeadingTests.cs: 90° and 270° both expect 'מ'
  Notes: Requires corresponding test updates in HeadingTests.
- [he] `NE_Short`
  Current: צמ
  Proposed: צמז
  Status: defect / Severity: P1 / Confidence: high
  Rationale: הקיצור הנוכחי מתנגש עם NW_Short ולכן אינו חד-משמעי. צירוף "צמז" שומר על קצרות ומבהיר שזה צפון-מזרח.
  Evidence: src/Humanizer/Properties/Resources.he.resx: NE_Short='צמ' and NW_Short='צמ'; tests/Humanizer.Tests/Localisation/he/HeadingTests.cs: 45° and 315° both expect 'צמ'
  Notes: Keep N_Short as 'צ'; disambiguate diagonal abbreviations.
- [he] `NW_Short`
  Current: צמ
  Proposed: צמע
  Status: defect / Severity: P1 / Confidence: high
  Rationale: הקיצור הנוכחי אינו מבדיל בין צפון-מערב לצפון-מזרח. "צמע" מבטא בבירור את רכיב המערב.
  Evidence: src/Humanizer/Properties/Resources.he.resx: NE_Short='צמ' and NW_Short='צמ'; tests/Humanizer.Tests/Localisation/he/HeadingTests.cs: 45° and 315° both expect 'צמ'
  Notes: Paired change with NE_Short to keep a consistent scheme.
- [he] `SE_Short`
  Current: דמ
  Proposed: דמז
  Status: defect / Severity: P1 / Confidence: high
  Rationale: הקיצור הנוכחי מתנגש עם SW_Short ולכן לא מאפשר הבדלה בין דרום-מזרח לדרום-מערב. "דמז" מציין במפורש מזרח.
  Evidence: src/Humanizer/Properties/Resources.he.resx: SE_Short='דמ' and SW_Short='דמ'; tests/Humanizer.Tests/Localisation/he/HeadingTests.cs: 135° and 225° both expect 'דמ'
  Notes: Keep S_Short as 'ד'; disambiguate diagonals only.
- [he] `SW_Short`
  Current: דמ
  Proposed: דמע
  Status: defect / Severity: P1 / Confidence: high
  Rationale: הקיצור הנוכחי זהה ל-SE_Short ולכן דו-משמעי. "דמע" מציין דרום-מערב באופן טבעי וברור.
  Evidence: src/Humanizer/Properties/Resources.he.resx: SE_Short='דמ' and SW_Short='דמ'; tests/Humanizer.Tests/Localisation/he/HeadingTests.cs: 135° and 225° both expect 'דמ'
  Notes: Paired change with SE_Short.
- [he] `W_Short`
  Current: מ
  Proposed: מע
  Status: defect / Severity: P1 / Confidence: high
  Rationale: הקיצור הנוכחי זהה ל-E_Short ולכן גורם להתנגשות סמנטית. "מע" הוא קיצור ברור של מערב ומונע בלבול ניווטי.
  Evidence: src/Humanizer/Properties/Resources.he.resx: E_Short='מ' and W_Short='מ'; tests/Humanizer.Tests/Localisation/he/HeadingTests.cs: 90° and 270° both expect 'מ'
  Notes: Requires corresponding test updates in HeadingTests.
- [hr] `DateHumanize_SingleDayAgo`
  Current: juÄer
  Proposed: jučer
  Status: defect / Severity: P1 / Confidence: high
  Rationale: Oblik "juÄer" je oštećen (mojibake). Ispravan hrvatski izraz je "jučer".
  Evidence: src/Humanizer/Properties/Resources.hr.resx: DateHumanize_SingleDayAgo = "jučer"; tests/Humanizer.Tests/Localisation/hr/DateHumanizeTests.cs: očekivanje "jučer" za -1 dan
  Notes: Potrebno uskladiti zapis u review artefaktu s izvornom lokalizacijom.
- [hr] `TimeSpanHumanize_Zero`
  Current: bez podatka o vremenu
  Proposed: bez vremena
  Status: defect / Severity: P2 / Confidence: high
  Rationale: "Bez podatka o vremenu" znači da nema podatka, a ne da je trajanje nula. Za neutralni engleski "no time" prirodnije je "bez vremena".
  Evidence: src/Humanizer/Properties/Resources.hr.resx: TimeSpanHumanize_Zero = "bez podatka o vremenu"; Neutral reference je "no time" (bez dodatnog značenja o nedostajućim podacima)
  Notes: Predloženi izraz je semantički bliži i prirodniji u hrvatskom UI kontekstu.
- [hu] `DateHumanize_TwoDaysAgo`
  Current: két éve
  Proposed: tegnapelőtt
  Status: defect / Severity: P1 / Confidence: high
  Rationale: A jelenlegi szöveg jelentése "két éve" (two years ago), ami szemantikailag hibás a "two days ago" kulcshoz. Magyarul a természetes, idiomatikus forma két nappal ezelőttre: "tegnapelőtt".
  Evidence: src/Humanizer/Properties/Resources.hu.resx: DateHumanize_TwoDaysAgo = "két éve"; tests/Humanizer.Tests/Localisation/hu/DateHumanizeTests.cs: napokra külön ellenőrzés van (1 nap: "tegnap", 10 nap: "10 napja"), de a TwoDays kulcs nincs külön lefedve
  Notes: Jelentéscsere: day -> year. Dedikált two-day teszt hiányzik, ezért a hiba észrevétlen maradt.
- [hu] `DateHumanize_TwoDaysFromNow`
  Current: két év múlva
  Proposed: holnapután
  Status: defect / Severity: P1 / Confidence: high
  Rationale: A jelenlegi szöveg jelentése "két év múlva" (two years from now), ami hibás a "two days from now" kulcshoz. Magyarul a pontos és természetes megfelelő: "holnapután".
  Evidence: src/Humanizer/Properties/Resources.hu.resx: DateHumanize_TwoDaysFromNow = "két év múlva"; tests/Humanizer.Tests/Localisation/hu/DateHumanizeTests.cs: jövő idejű napokra csak 1 és 10 nap van tesztelve, a TwoDays kulcs nincs célzottan tesztelve
  Notes: Jelentéscsere: day -> year. A két napos speciális forma jelenleg teszt nélküli.
- [hy] `TimeSpanHumanize_Zero`
  Current: ÕªÕ¡Õ´Õ¡Õ¶Õ¡Õ¯Õ¨ Õ¢Õ¡ÖÕ¡Õ¯Õ¡ÕµÕ¸Ö‚Õ´ Õ§
  Proposed: ÕªÕ¡Õ´Õ¡Õ¶Õ¡Õ¯ Õ¹Õ¯Õ¡
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Â«ÔºÕ¡Õ´Õ¡Õ¶Õ¡Õ¯Õ¨ Õ¢Õ¡ÖÕ¡Õ¯Õ¡ÕµÕ¸Ö‚Õ´ Õ§Â» Õ±Ö‡Õ¡Õ¯Õ¥Ö€ÕºÕ¸Ö‚Õ´Õ¨ Õ¡Ö€Õ°Õ¥Õ½Õ¿Õ¡Õ¯Õ¡Õ¶ Õ§ Ö‡ Õ¢Õ¡Õ¼Õ¡ÖÕ« Õ©Õ¡Ö€Õ£Õ´Õ¡Õ¶Õ¸Ö‚Õ©ÕµÕ¡Õ¶ Õ¿ÕºÕ¡Õ¾Õ¸Ö€Õ¸Ö‚Õ©ÕµÕ¸Ö‚Õ¶ Õ§ Õ©Õ¸Õ²Õ¶Õ¸Ö‚Õ´Ö‰ Ô²Õ¶Õ¡Õ¯Õ¡Õ¶ Õ­Õ¸Õ½Ö„Õ¸Ö‚Õ´ Â«no timeÂ»-Õ« Õ°Õ¡Õ´Õ¡Ö€ Õ³Õ«Õ·Õ¿Õ¶ Õ§ Â«ÕªÕ¡Õ´Õ¡Õ¶Õ¡Õ¯ Õ¹Õ¯Õ¡Â»Ö‰
  Evidence: src/Humanizer/Properties/Resources.hy.resx:267-268 uses "ÕªÕ¡Õ´Õ¡Õ¶Õ¡Õ¯Õ¨ Õ¢Õ¡ÖÕ¡Õ¯Õ¡ÕµÕ¸Ö‚Õ´ Õ§" for TimeSpanHumanize_Zero.; tests/Humanizer.Tests/Localisation/hy/TimeSpanHumanizeTests.cs:128 asserts the same awkward phrase for toWords: true.
  Notes: ÔµÕ©Õ¥ ÖƒÕ¸Õ­Õ¡Ö€Õ«Õ¶Õ¸Ö‚Õ´Õ¨ Õ¨Õ¶Õ¤Õ¸Ö‚Õ¶Õ¾Õ«, ÕºÕ¥Õ¿Ö„ Õ§ Õ°Õ¡Õ´Õ¡ÕºÕ¡Õ¿Õ¡Õ½Õ­Õ¡Õ¶Õ¡Õ¢Õ¡Ö€ Õ©Õ¡Ö€Õ´Õ¡ÖÕ¶Õ¥Õ¬ hy/TimeSpanHumanizeTests.cs-Õ« NoTimeToWords Õ©Õ¥Õ½Õ¿Õ¨Ö‰
- [id] `DateHumanize_MultipleDaysFromNow`
  Current: {0} hari dari sekarang
  Proposed: dalam {0} hari
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Frasa "{0} hari dari sekarang" dapat dipahami, tetapi terdengar terjemahan literal. Dalam bahasa Indonesia alami, bentuk yang paling lazim adalah "dalam {0} hari" untuk konteks waktu mendatang.
  Evidence: src/Humanizer/Properties/Resources.id.resx; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs; tests/Humanizer.Tests/Localisation/id/TimeSpanHumanizeTests.cs
  Notes: Perlu sinkronisasi ekspektasi pengujian jika string produksi diubah.
- [id] `DateHumanize_MultipleDaysFromNow_Paucal`
  Current: {0} hari dari sekarang
  Proposed: dalam {0} hari
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Varian paucal mewarisi masalah diksi yang sama: "dari sekarang" terdengar kaku. Bentuk "dalam {0} hari" lebih idiomatis dan umum dipakai penutur asli.
  Evidence: src/Humanizer/Properties/Resources.id.resx; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs; tests/Humanizer.Tests/Localisation/id/TimeSpanHumanizeTests.cs
  Notes: Jaga konsistensi dengan pasangan non-paucal.
- [id] `DateHumanize_MultipleHoursFromNow`
  Current: {0} jam dari sekarang
  Proposed: dalam {0} jam
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: "{0} jam dari sekarang" adalah terjemahan literal dan kurang natural dalam UI Indonesia. "dalam {0} jam" lebih ringkas dan idiomatis.
  Evidence: src/Humanizer/Properties/Resources.id.resx; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs; tests/Humanizer.Tests/Localisation/id/TimeSpanHumanizeTests.cs
  Notes: Perubahan menyentuh frase umum future tense.
- [id] `DateHumanize_MultipleMinutesFromNow`
  Current: {0} menit dari sekarang
  Proposed: dalam {0} menit
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Untuk ekspresi waktu mendatang, penutur asli cenderung memakai "dalam {0} menit", bukan "{0} menit dari sekarang".
  Evidence: src/Humanizer/Properties/Resources.id.resx; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs; tests/Humanizer.Tests/Localisation/id/TimeSpanHumanizeTests.cs
  Notes: Perlu penyelarasan dengan pola unit lain.
- [id] `DateHumanize_MultipleMonthsFromNow`
  Current: {0} bulan dari sekarang
  Proposed: dalam {0} bulan
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Struktur "dari sekarang" tidak salah makna, tetapi terasa seperti hasil terjemahan langsung. "dalam {0} bulan" lebih alami dalam bahasa Indonesia standar.
  Evidence: src/Humanizer/Properties/Resources.id.resx; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs; tests/Humanizer.Tests/Localisation/id/TimeSpanHumanizeTests.cs
  Notes: Menjaga konsistensi gaya dengan unit hari/jam/menit.
- [id] `DateHumanize_MultipleSecondsFromNow`
  Current: {0} detik dari sekarang
  Proposed: dalam {0} detik
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Bentuk natural untuk hitung mundur adalah "dalam {0} detik". Bentuk saat ini terlalu harfiah.
  Evidence: src/Humanizer/Properties/Resources.id.resx; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs; tests/Humanizer.Tests/Localisation/id/TimeSpanHumanizeTests.cs
  Notes: Penting untuk konteks notifikasi real-time.
- [id] `DateHumanize_MultipleYearsFromNow`
  Current: {0} tahun dari sekarang
  Proposed: dalam {0} tahun
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Ungkapan waktu masa depan paling lazim: "dalam {0} tahun". Frasa sekarang dapat dipahami, tetapi tidak idiomatis.
  Evidence: src/Humanizer/Properties/Resources.id.resx; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs; tests/Humanizer.Tests/Localisation/id/TimeSpanHumanizeTests.cs
  Notes: Selaraskan dengan seluruh keluarga FromNow.
- [id] `DateHumanize_SingleHourFromNow`
  Current: sejam dari sekarang
  Proposed: sejam lagi
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Untuk satuan tunggal, penutur asli jauh lebih natural memakai bentuk "... lagi" seperti "sejam lagi" ketimbang "sejam dari sekarang".
  Evidence: src/Humanizer/Properties/Resources.id.resx; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs; tests/Humanizer.Tests/Localisation/id/TimeSpanHumanizeTests.cs
  Notes: Pertahankan gaya khusus bentuk singular.
- [id] `DateHumanize_SingleMinuteFromNow`
  Current: semenit dari sekarang
  Proposed: semenit lagi
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: "semenit lagi" adalah bentuk idiomatis Indonesia. Bentuk saat ini kaku dan kurang percakapan.
  Evidence: src/Humanizer/Properties/Resources.id.resx; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs; tests/Humanizer.Tests/Localisation/id/TimeSpanHumanizeTests.cs
  Notes: Disarankan sejajar dengan "sejam lagi" dan "sedetik lagi".
- [id] `DateHumanize_SingleMonthFromNow`
  Current: sebulan dari sekarang
  Proposed: sebulan lagi
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Frasa singular masa depan yang natural adalah "sebulan lagi". Versi sekarang terlalu literal.
  Evidence: src/Humanizer/Properties/Resources.id.resx; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs; tests/Humanizer.Tests/Localisation/id/TimeSpanHumanizeTests.cs
  Notes: Konsistenkan pola singular future.
- [id] `DateHumanize_SingleSecondFromNow`
  Current: sedetik dari sekarang
  Proposed: sedetik lagi
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Dalam penggunaan nyata, "sedetik lagi" lebih alami dan ringkas daripada "sedetik dari sekarang".
  Evidence: src/Humanizer/Properties/Resources.id.resx; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs; tests/Humanizer.Tests/Localisation/id/TimeSpanHumanizeTests.cs
  Notes: Relevan untuk output cepat/konversasional.
- [id] `DateHumanize_SingleYearFromNow`
  Current: setahun dari sekarang
  Proposed: setahun lagi
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Ungkapan idiomatis yang lazim adalah "setahun lagi". Bentuk sekarang tidak salah arti, tetapi tidak natural bagi penutur asli.
  Evidence: src/Humanizer/Properties/Resources.id.resx; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs; tests/Humanizer.Tests/Localisation/id/TimeSpanHumanizeTests.cs
  Notes: Tetap pertahankan makna kuantitas 1 tahun.
- [id] `TimeSpanHumanize_Zero`
  Current: waktu kosong
  Proposed: tanpa waktu
  Status: defect / Severity: P1 / Confidence: high
  Rationale: "waktu kosong" di Indonesia lebih dekat ke makna "free time/slot kosong", bukan "no time". Ini berisiko mengubah makna fungsional output.
  Evidence: src/Humanizer/Properties/Resources.id.resx; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs; tests/Humanizer.Tests/Localisation/id/TimeSpanHumanizeTests.cs
  Notes: Alternatif lain yang mungkin: "tidak ada waktu"; pilihannya tergantung gaya produk.
- [is] `DateHumanize_MultipleSecondsAgo_Plural`
  Current: fyrir {0} sekúndu
  Proposed: fyrir {0} sekúndum
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Þetta er fleirtölulykill en núverandi strengur er í eintölu (sekúndu). Eftir forsetninguna "fyrir" þarf hér þágufall fleirtölu: "sekúndum".
  Evidence: src/Humanizer/Properties/Resources.is.resx (DateHumanize_MultipleSecondsAgo notar 'fyrir {0} sekúndum').; tests/Humanizer.Tests/Localisation/is/DateHumanizeTests.cs (SecondsAgo: 'fyrir 2 sekúndum').
  Notes: Samræmt við grunnlykilinn DateHumanize_MultipleSecondsAgo.
- [is] `TimeSpanHumanize_MultipleMonths_Singular`
  Current: {0} mánuðir
  Proposed: {0} mánuður
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Singular-lykillinn er rangur þar sem hann notar fleirtöluorðið "mánuðir". Fyrir eintölu á að vera "mánuður".
  Evidence: src/Humanizer/Properties/Resources.is.resx (TimeSpanHumanize_MultipleMonths_Singular).; tests/Humanizer.Tests/Localisation/is/TimeSpanHumanizeTests.cs (Months: '1 mánuður').
  Notes: Leiðrétt í samræmi við TimeSpanHumanize_SingleMonth.
- [it] `DataUnit_Byte`
  Current: bytes
  Proposed: byte
  Status: defect / Severity: P2 / Confidence: high
  Rationale: In italiano tecnico 'byte' resta invariabile: la forma plurale inglese 'bytes' non e idiomatica.
  Evidence: src/Humanizer/Properties/Resources.it.resx: DataUnit_Byte = 'bytes'; tests/Humanizer.Tests/Localisation/it/Bytes/ToFullWordsTests.cs: ReturnsPluralBytes expects '10 bytes' (legacy behavior to correct)
  Notes: Difetto lessicale: mantenere 'byte' sia al singolare sia al plurale.
- [it] `DataUnit_Gigabyte`
  Current: gigabytes
  Proposed: gigabyte
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Anche 'gigabyte' in uso italiano e invariabile; 'gigabytes' e un calco inglese non naturale.
  Evidence: src/Humanizer/Properties/Resources.it.resx: DataUnit_Gigabyte = 'gigabytes'; tests/Humanizer.Tests/Localisation/it/Bytes/ToFullWordsTests.cs: ReturnsPluralGigabytes expects '10 gigabytes' (legacy behavior to correct)
  Notes: Allineare singolare/plurale a 'gigabyte'.
- [it] `TimeSpanHumanize_Age`
  Current: {0} vecchio
  Proposed: {0} anni
  Status: defect / Severity: P1 / Confidence: high
  Rationale: La stringa '{0} vecchio' e grammaticalmente scorretta e semanticamente impropria per l'eta; la resa naturale e '{0} anni'.
  Evidence: src/Humanizer/Properties/Resources.it.resx: TimeSpanHumanize_Age = '{0} vecchio'; tests/Humanizer.Tests/Localisation/it/ResourcesTests.cs: HasExplicitResidualResources asserts current residual value
  Notes: Difetto di qualita alta: output innaturale in un caso utente comune.
- [it] `TimeSpanHumanize_SingleHour_Words`
  Current: una ora
  Proposed: un'ora
  Status: suspicious / Severity: P3 / Confidence: high
  Rationale: In italiano standard davanti a 'ora' si usa normalmente l'elisione: 'un'ora'. 'Una ora' e comprensibile ma marcato/non standard.
  Evidence: src/Humanizer/Properties/Resources.it.resx: TimeSpanHumanize_SingleHour_Words = 'una ora'; tests/Humanizer.Tests/Localisation/it/TimeSpanHumanizeTests.cs: Hours(toWords:true) currently expects 'una ora'
  Notes: Miglioria stilistica raccomandata; non blocca comprensione.
- [ko] `DateHumanize_Never`
  Current: 사용 안 함
  Proposed: 없음
  Status: defect / Severity: P1 / Confidence: high
  Rationale: "사용 안 함"은 설정 UI의 토글 문구(Disabled)에 가깝고, 시간 표현의 "never" 의미(한 번도 없음)를 전달하지 못합니다. DateHumanize의 null 입력 반환값으로는 한국어에서 "없음"이 가장 자연스럽고 문맥 호환성이 높습니다.
  Evidence: src/Humanizer/Properties/Resources.ko.resx: DateHumanize_Never = "사용 안 함".; src/Humanizer/DateHumanizeExtensions.cs: nullable Date/Time humanize에서 null일 때 DateHumanize_Never를 반환.; src/Humanizer/Properties에는 Resources.ko-KR.resx가 없어서 ko-KR이 ko 값을 폴백으로 상속.
  Notes: 용어 오역으로 의미가 바뀌는 결함이며, ko-KR 자식 문화권에도 동일하게 전파됩니다.
- [lv] `DateHumanize_SingleMinuteFromNow`
  Current: pÄ“c minÅ«tÄ“s
  Proposed: pēc minūtes
  Status: defect / Severity: P1 / Confidence: high
  Rationale: Latviešu valodā pēc prievārda "pēc" vajadzīgs ģenitīvs vienskaitlī: "minūtes". Forma "minūtēs" ir lokatīva daudzskaitlī un šeit ir gramatiski nepareiza.
  Evidence: src/Humanizer/Properties/Resources.lv.resx:255 (DateHumanize_SingleMinuteFromNow currently "pēc minūtēs"); src/Humanizer/Properties/Resources.lv.resx:252 (DateHumanize_SingleMinuteAgo uses correct "pirms minūtes"); tests/Humanizer.Tests/Localisation/lv/DateHumanizeTests.cs (Latvian date humanization resources are locale-owned and explicitly validated)
  Notes: Direct grammatical correction; keeps style parallel with SingleHourFromNow/SingleSecondFromNow.
- [ms] `DateHumanize_MultipleYearsAgo_Paucal`
  Current: {0} tahun dari yang lalu
  Proposed: {0} tahun yang lalu
  Status: defect / Severity: P1 / Confidence: high
  Rationale: Frasa "dari yang lalu" tidak gramatis dalam Bahasa Melayu baku dan menjejaskan kefahaman. Bentuk yang tepat dan natural ialah "{0} tahun yang lalu", selaras dengan pola tahun lain dalam locale ini.
  Evidence: src/Humanizer/Properties/Resources.ms.resx: DateHumanize_MultipleYearsAgo_Paucal = "{0} tahun dari yang lalu"; tests/Humanizer.Tests/Localisation/ms-MY/DateHumanizeTests.cs: [UseCulture("ms-MY")] with ms resource lookup indicates child-culture fallback path; tests/Humanizer.Tests/Localisation/ms-MY/**/*: ms-MY tests rely on parent ms strings; defect propagates to owned child culture fallback
  Notes: Defect is in parent ms resource and therefore inherited by owned child culture ms-MY fallback.
- [mt] `N`
  Current: XL
  Proposed: tramuntana
  Status: defect / Severity: P1 / Confidence: high
  Rationale: "XL" hija abbrevjazzjoni ta' "xlokk" (southeast), mhux l-isem sħiħ tad-direzzjoni tat-tramuntana. Għall-valur sħiħ ta' N f'Malti għandu jintuża "tramuntana".
  Evidence: src/Humanizer/Properties/Resources.mt.resx: key "N" currently uses "XL" while key "N_Short" is "T".; tests/Humanizer.Tests/Localisation/mt/HeadingTests.cs: full heading currently expects "XL" at 0°, indicating the resource is carrying an abbreviation in a full-form key.
  Notes: Wrong cardinal direction term in full heading output.
- [mt] `TimeSpanHumanize_MultipleYears_Singular`
  Current: {0} snin
  Proposed: {0} sena
  Status: defect / Severity: P1 / Confidence: high
  Rationale: Għall-kategorija singulari b'placeholder numeriku, in-nom għandu jibqa' fis-singular: "{0} sena". Il-forma attwali "{0} snin" hi plural u tagħti qbil grammatikali żbaljat.
  Evidence: src/Humanizer/Properties/Resources.mt.resx: key "TimeSpanHumanize_MultipleYears_Singular" is "{0} snin".; tests/Humanizer.Tests/Localisation/mt/TimeSpanHumanizeTests.cs: one-year output is "sena", confirming the singular stem.; src/Humanizer/Properties/Resources.mt.resx: parallel pattern exists in "TimeSpanHumanize_MultipleMinutes_Singular" as "{0} minuta" (number + singular noun).
  Notes: Singular/plural agreement defect in numeric template.
- [nb] `DataUnit_Byte`
  Current: byte
  Proposed: byte
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Selve grunnformen "byte" er akseptabel på bokmål, men gjeldende oppførsel gir engelsk flertall ("bytes") i brukerflate. Det er ikke naturlig norsk dataterminologi.
  Evidence: tests/Humanizer.Tests/Localisation/nb/Bytes/ToFullWordsTests.cs:11 expects "10 bytes"; tests/Humanizer.Tests/Localisation/nb/Bytes/ToFullWordsTests.cs:19 expects "10 gigabytes"; src/Humanizer/Properties/Resources.nb.resx defines base form "byte"
  Notes: Krever lokalisert flertallshåndtering i kodebanen for ByteSize.ToFullWords; ressursstrengen alene styrer ikke flertallsformen.
- [nb] `DataUnit_Gigabyte`
  Current: gigabyte
  Proposed: gigabyte
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Selve grunnformen "gigabyte" er akseptabel på bokmål, men engelsk flertall med -s ("gigabytes") er ikke naturlig i norsk brukerflate.
  Evidence: tests/Humanizer.Tests/Localisation/nb/Bytes/ToFullWordsTests.cs:19 expects "10 gigabytes"; src/Humanizer/Properties/Resources.nb.resx defines base form "gigabyte"
  Notes: Samme årsak som for DataUnit_Byte: flertall må håndteres lokalisert i kode, ikke via engelsk standardbøying.
- [pl] `TimeUnit_Day`
  Current: dzień
  Proposed: d
  Status: suspicious / Severity: P3 / Confidence: high
  Rationale: Klucz jest używany jako symbol jednostki czasu, a "dzień" to pełna nazwa. W polskim skrótem-symboliem jest zwykle "d".
  Evidence: src/Humanizer/Properties/Resources.pl.resx (TimeUnit_Day).; tests/Humanizer.Tests/Localisation/pl/TimeUnitToSymbolTests.cs (ToSymbol mapuje ten klucz).
  Notes: Obecna wartość działa, ale semantycznie bardziej przypomina etykietę niż symbol.
- [pl] `TimeUnit_Month`
  Current: miesiąc
  Proposed: mies.
  Status: suspicious / Severity: P3 / Confidence: high
  Rationale: Dla symbolu jednostki czasu naturalniejszy jest skrót, nie pełna forma rzeczownika. W polskim interfejsie najczęściej używa się "mies.".
  Evidence: src/Humanizer/Properties/Resources.pl.resx (TimeUnit_Month).; tests/Humanizer.Tests/Localisation/pl/TimeUnitToSymbolTests.cs (ToSymbol dla Month).
  Notes: Warto rozważyć ujednolicenie stylu symboli między Day/Week/Month/Year i h/min/s/ms.
- [pl] `TimeUnit_Week`
  Current: tydzień
  Proposed: tydz.
  Status: suspicious / Severity: P3 / Confidence: high
  Rationale: ToSymbol sugeruje skrót/symbol jednostki. Pełne "tydzień" jest zrozumiałe, ale nie wygląda jak symbol jednostki czasu.
  Evidence: src/Humanizer/Properties/Resources.pl.resx (TimeUnit_Week).; tests/Humanizer.Tests/Localisation/pl/TimeUnitToSymbolTests.cs (Week -> "tydzień").
  Notes: Ocena dotyczy naturalności symbolu, nie poprawności leksykalnej.
- [pl] `TimeUnit_Year`
  Current: rok
  Proposed: r.
  Status: suspicious / Severity: P3 / Confidence: high
  Rationale: Wartość "rok" jest pełnym słowem, a nie symbolem. Dla krótkiego oznaczenia jednostki częściej spotyka się "r." (ew. "l." zależnie od kontekstu).
  Evidence: src/Humanizer/Properties/Resources.pl.resx (TimeUnit_Year).; tests/Humanizer.Tests/Localisation/pl/TimeUnitToSymbolTests.cs (Year -> "rok").
  Notes: Wariant skrótu można doprecyzować zależnie od przyjętej konwencji produktu.
- [pt] `DateHumanize_MultipleDaysAgo`
  Current: hÃ¡ {0} dias
  Proposed: há {0} dias
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleDaysAgo_Dual`
  Current: hÃ¡ {0} dias
  Proposed: há {0} dias
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleDaysAgo_Paucal`
  Current: hÃ¡ {0} dias
  Proposed: há {0} dias
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleDaysAgo_Plural`
  Current: hÃ¡ {0} dias
  Proposed: há {0} dias
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleDaysAgo_Singular`
  Current: hÃ¡ {0} dia
  Proposed: há {0} dia
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleHoursAgo`
  Current: hÃ¡ {0} horas
  Proposed: há {0} horas
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleHoursAgo_Dual`
  Current: hÃ¡ {0} horas
  Proposed: há {0} horas
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleHoursAgo_Paucal`
  Current: hÃ¡ {0} horas
  Proposed: há {0} horas
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleHoursAgo_Plural`
  Current: hÃ¡ {0} horas
  Proposed: há {0} horas
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleHoursAgo_Singular`
  Current: hÃ¡ {0} hora
  Proposed: há {0} hora
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleMinutesAgo`
  Current: hÃ¡ {0} minutos
  Proposed: há {0} minutos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleMinutesAgo_Dual`
  Current: hÃ¡ {0} minutos
  Proposed: há {0} minutos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleMinutesAgo_Paucal`
  Current: hÃ¡ {0} minutos
  Proposed: há {0} minutos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleMinutesAgo_Plural`
  Current: hÃ¡ {0} minutos
  Proposed: há {0} minutos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleMinutesAgo_Singular`
  Current: hÃ¡ {0} minuto
  Proposed: há {0} minuto
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleMonthsAgo`
  Current: hÃ¡ {0} meses
  Proposed: há {0} meses
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleMonthsAgo_Dual`
  Current: hÃ¡ {0} meses
  Proposed: há {0} meses
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleMonthsAgo_Paucal`
  Current: hÃ¡ {0} meses
  Proposed: há {0} meses
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleMonthsAgo_Plural`
  Current: hÃ¡ {0} meses
  Proposed: há {0} meses
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleMonthsAgo_Singular`
  Current: hÃ¡ {0} mÃªs
  Proposed: há {0} mês
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleMonthsFromNow_Singular`
  Current: daqui a {0} mÃªs
  Proposed: daqui a {0} mês
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleSecondsAgo`
  Current: hÃ¡ {0} segundos
  Proposed: há {0} segundos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleSecondsAgo_Dual`
  Current: hÃ¡ {0} segundos
  Proposed: há {0} segundos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleSecondsAgo_Paucal`
  Current: hÃ¡ {0} segundos
  Proposed: há {0} segundos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleSecondsAgo_Plural`
  Current: hÃ¡ {0} segundos
  Proposed: há {0} segundos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleSecondsAgo_Singular`
  Current: hÃ¡ {0} segundo
  Proposed: há {0} segundo
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleYearsAgo`
  Current: hÃ¡ {0} anos
  Proposed: há {0} anos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleYearsAgo_Dual`
  Current: hÃ¡ {0} anos
  Proposed: há {0} anos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleYearsAgo_Paucal`
  Current: hÃ¡ {0} anos
  Proposed: há {0} anos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleYearsAgo_Plural`
  Current: hÃ¡ {0} anos
  Proposed: há {0} anos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_MultipleYearsAgo_Singular`
  Current: hÃ¡ {0} ano
  Proposed: há {0} ano
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_SingleDayFromNow`
  Current: amanhÃ£
  Proposed: amanhã
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_SingleHourAgo`
  Current: hÃ¡ uma hora
  Proposed: há uma hora
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_SingleMinuteAgo`
  Current: hÃ¡ um minuto
  Proposed: há um minuto
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_SingleMonthAgo`
  Current: hÃ¡ um mÃªs
  Proposed: há um mês
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_SingleMonthFromNow`
  Current: daqui a um mÃªs
  Proposed: daqui a um mês
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_SingleSecondAgo`
  Current: hÃ¡ um segundo
  Proposed: há um segundo
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_SingleYearAgo`
  Current: hÃ¡ um ano
  Proposed: há um ano
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `DateHumanize_TwoDaysFromNow`
  Current: depois de amanhÃ£
  Proposed: depois de amanhã
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `TimeSpanHumanize_MultipleMonths_Singular`
  Current: {0} mÃªs
  Proposed: {0} mês
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `TimeSpanHumanize_SingleMonth`
  Current: 1 mÃªs
  Proposed: 1 mês
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `TimeSpanHumanize_SingleMonth_Words`
  Current: um mÃªs
  Proposed: um mês
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt] `TimeSpanHumanize_Zero`
  Current: sem horÃ¡rio
  Proposed: sem horário
  Status: defect / Severity: P2 / Confidence: high
  Rationale: O texto atual está com mojibake (acentuação corrompida), o que torna a saída incorreta para português europeu. A forma proposta repõe a grafia nativa usada no recurso de origem.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor canónico com acentuação correta); tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserções com grafia correta); tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserções com grafia correta)
  Notes: Defeito de encoding no artefacto de revisão; tradução válida confirmada no .resx e nos testes de localização.
- [pt-BR] `TimeSpanHumanize_MultipleMilliseconds`
  Current: {0} milisegundos
  Proposed: {0} milissegundos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Em portuguÃªs do Brasil, a grafia correta Ã© â€œmilissegundoâ€ (com SS). A forma com um Ãºnico S (â€œmilisegundoâ€) estÃ¡ ortograficamente incorreta.
  Evidence: src/Humanizer/Properties/Resources.pt-BR.resx (chaves de milissegundo atualmente com "milisegundo"); tests/Humanizer.Tests/Localisation/pt-BR/TimeSpanHumanizeTests.cs (asserÃ§Ãµes atuais tambÃ©m usam a grafia incorreta)
  Notes: Recomenda-se alinhar recursos e testes para â€œmilissegundo(s)â€.
- [pt-BR] `TimeSpanHumanize_MultipleMilliseconds_Dual`
  Current: {0} milisegundos
  Proposed: {0} milissegundos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Em portuguÃªs do Brasil, a grafia correta Ã© â€œmilissegundoâ€ (com SS). A forma com um Ãºnico S (â€œmilisegundoâ€) estÃ¡ ortograficamente incorreta.
  Evidence: src/Humanizer/Properties/Resources.pt-BR.resx (chaves de milissegundo atualmente com "milisegundo"); tests/Humanizer.Tests/Localisation/pt-BR/TimeSpanHumanizeTests.cs (asserÃ§Ãµes atuais tambÃ©m usam a grafia incorreta)
  Notes: Recomenda-se alinhar recursos e testes para â€œmilissegundo(s)â€.
- [pt-BR] `TimeSpanHumanize_MultipleMilliseconds_Paucal`
  Current: {0} milisegundos
  Proposed: {0} milissegundos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Em portuguÃªs do Brasil, a grafia correta Ã© â€œmilissegundoâ€ (com SS). A forma com um Ãºnico S (â€œmilisegundoâ€) estÃ¡ ortograficamente incorreta.
  Evidence: src/Humanizer/Properties/Resources.pt-BR.resx (chaves de milissegundo atualmente com "milisegundo"); tests/Humanizer.Tests/Localisation/pt-BR/TimeSpanHumanizeTests.cs (asserÃ§Ãµes atuais tambÃ©m usam a grafia incorreta)
  Notes: Recomenda-se alinhar recursos e testes para â€œmilissegundo(s)â€.
- [pt-BR] `TimeSpanHumanize_MultipleMilliseconds_Plural`
  Current: {0} milisegundos
  Proposed: {0} milissegundos
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Em portuguÃªs do Brasil, a grafia correta Ã© â€œmilissegundoâ€ (com SS). A forma com um Ãºnico S (â€œmilisegundoâ€) estÃ¡ ortograficamente incorreta.
  Evidence: src/Humanizer/Properties/Resources.pt-BR.resx (chaves de milissegundo atualmente com "milisegundo"); tests/Humanizer.Tests/Localisation/pt-BR/TimeSpanHumanizeTests.cs (asserÃ§Ãµes atuais tambÃ©m usam a grafia incorreta)
  Notes: Recomenda-se alinhar recursos e testes para â€œmilissegundo(s)â€.
- [pt-BR] `TimeSpanHumanize_MultipleMilliseconds_Singular`
  Current: {0} milisegundo
  Proposed: {0} milissegundo
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Em portuguÃªs do Brasil, a grafia correta Ã© â€œmilissegundoâ€ (com SS). A forma com um Ãºnico S (â€œmilisegundoâ€) estÃ¡ ortograficamente incorreta.
  Evidence: src/Humanizer/Properties/Resources.pt-BR.resx (chaves de milissegundo atualmente com "milisegundo"); tests/Humanizer.Tests/Localisation/pt-BR/TimeSpanHumanizeTests.cs (asserÃ§Ãµes atuais tambÃ©m usam a grafia incorreta)
  Notes: Recomenda-se alinhar recursos e testes para â€œmilissegundo(s)â€.
- [pt-BR] `TimeSpanHumanize_SingleMillisecond`
  Current: 1 milisegundo
  Proposed: 1 milissegundo
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Em portuguÃªs do Brasil, a grafia correta Ã© â€œmilissegundoâ€ (com SS). A forma com um Ãºnico S (â€œmilisegundoâ€) estÃ¡ ortograficamente incorreta.
  Evidence: src/Humanizer/Properties/Resources.pt-BR.resx (chaves de milissegundo atualmente com "milisegundo"); tests/Humanizer.Tests/Localisation/pt-BR/TimeSpanHumanizeTests.cs (asserÃ§Ãµes atuais tambÃ©m usam a grafia incorreta)
  Notes: Recomenda-se alinhar recursos e testes para â€œmilissegundo(s)â€.
- [pt-BR] `TimeSpanHumanize_SingleMillisecond_Words`
  Current: um milisegundo
  Proposed: um milissegundo
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Em portuguÃªs do Brasil, a grafia correta Ã© â€œmilissegundoâ€ (com SS). A forma com um Ãºnico S (â€œmilisegundoâ€) estÃ¡ ortograficamente incorreta.
  Evidence: src/Humanizer/Properties/Resources.pt-BR.resx (chaves de milissegundo atualmente com "milisegundo"); tests/Humanizer.Tests/Localisation/pt-BR/TimeSpanHumanizeTests.cs (asserÃ§Ãµes atuais tambÃ©m usam a grafia incorreta)
  Notes: Recomenda-se alinhar recursos e testes para â€œmilissegundo(s)â€.
- [pt-BR] `TimeSpanHumanize_Zero`
  Current: sem horÃ¡rio
  Proposed: sem tempo
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Em pt-BR, â€œsem horÃ¡rioâ€ significa â€œsem agendamentoâ€, nÃ£o â€œsem duraÃ§Ã£o de tempoâ€. Para TimeSpan.Zero, a forma natural e semÃ¢ntica Ã© â€œsem tempoâ€.
  Evidence: src/Humanizer/Properties/Resources.pt-BR.resx (TimeSpanHumanize_Zero = "sem horÃ¡rio"); tests/Humanizer.Tests/Localisation/pt-BR/TimeSpanHumanizeTests.cs (caso NoTimeToWords atualmente valida "sem horÃ¡rio")
  Notes: SugestÃ£o linguÃ­stica: ajustar recurso e teste para refletir ausÃªncia de duraÃ§Ã£o, nÃ£o ausÃªncia de agenda.
- [sr] `DateHumanize_SingleMinuteAgo`
  Current: пре минут
  Proposed: пре минуте
  Status: defect / Severity: P2 / Confidence: high
  Rationale: У стандардном српском после предлога "пре" стоји генитив: правилно је "пре минуте", док је "пре минут" граматички неисправан и не звучи природно.
  Evidence: src/Humanizer/Properties/Resources.sr.resx: DateHumanize_SingleMinuteAgo = "пре минут"; tests/Humanizer.Tests/Localisation/sr/DateHumanizeTests.cs: [InlineData(1, "пре минут")]
  Notes: Тест тренутно потврђује неисправан облик и треба га ускладити са исправком.
- [sr] `DateHumanize_SingleMinuteFromNow`
  Current: за минут
  Proposed: за минуту
  Status: defect / Severity: P2 / Confidence: high
  Rationale: После предлога "за" у овој конструкцији иде акузатив једнине: "за минуту". Облик "за минут" је граматички неисправан за именицу "минута".
  Evidence: src/Humanizer/Properties/Resources.sr.resx: DateHumanize_SingleMinuteFromNow = "за минут"; tests/Humanizer.Tests/Localisation/sr/DateHumanizeTests.cs: [InlineData(1, "за минут")]
  Notes: Тест тренутно потврђује неисправан облик и треба га ускладити са исправком.
- [sr] `DateHumanize_SingleSecondAgo`
  Current: пре секунд
  Proposed: пре секунде
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Са предлогом "пре" мора да стоји генитив: "пре секунде". Облик "пре секунд" није природан нити правописно исправан у српском.
  Evidence: src/Humanizer/Properties/Resources.sr.resx: DateHumanize_SingleSecondAgo = "пре секунд"; tests/Humanizer.Tests/Localisation/sr/DateHumanizeTests.cs: [InlineData(1, "пре секунд")]
  Notes: Тест тренутно потврђује неисправан облик и треба га ускладити са исправком.
- [sr] `DateHumanize_SingleSecondFromNow`
  Current: за секунд
  Proposed: за секунду
  Status: defect / Severity: P2 / Confidence: high
  Rationale: У значењу будућег помераја времена исправан је акузатив: "за секунду". Облик "за секунд" је неусаглашен са падежом и делује као грешка.
  Evidence: src/Humanizer/Properties/Resources.sr.resx: DateHumanize_SingleSecondFromNow = "за секунд"; tests/Humanizer.Tests/Localisation/sr/DateHumanizeTests.cs: [InlineData(1, "за секунд")]
  Notes: Тест тренутно потврђује неисправан облик и треба га ускладити са исправком.
- [sv] `DataUnit_Byte`
  Current: bytes
  Proposed: byte
  Status: defect / Severity: P2 / Confidence: high
  Rationale: På svenska böjs normalt inte dataenheten med -s efter tal; idiomatisk form är "10 byte", inte "10 bytes".
  Evidence: src/Humanizer/Properties/Resources.sv.resx:306; tests/Humanizer.Tests/Localisation/sv/Bytes/ToFullWordsTests.cs:12
  Notes: Nuvarande test för sv förväntar sig den engelska pluralformen och bör justeras när resurssträngen rättas.
- [sv] `DataUnit_Gigabyte`
  Current: gigabytes
  Proposed: gigabyte
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Standard svensk användning är samma form i singular och plural för denna enhet: "1 gigabyte", "10 gigabyte".
  Evidence: src/Humanizer/Properties/Resources.sv.resx:315; tests/Humanizer.Tests/Localisation/sv/Bytes/ToFullWordsTests.cs:20
  Notes: Pluralformen med -s är ett engelskt mönster och låter onaturligt i svensk lokalisering.
- [th] `DateHumanize_MultipleDaysFromNow`
  Current: {0} à¸§à¸±à¸™à¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸§à¸±à¸™
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleDaysFromNow_Dual`
  Current: {0} à¸§à¸±à¸™à¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸§à¸±à¸™
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleDaysFromNow_Paucal`
  Current: {0} à¸§à¸±à¸™à¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸§à¸±à¸™
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleDaysFromNow_Plural`
  Current: {0} à¸§à¸±à¸™à¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸§à¸±à¸™
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleDaysFromNow_Singular`
  Current: {0} à¸§à¸±à¸™à¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸§à¸±à¸™
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleHoursFromNow`
  Current: {0} à¸Šà¸±à¹ˆà¸§à¹‚à¸¡à¸‡à¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸Šà¸±à¹ˆà¸§à¹‚à¸¡à¸‡
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleHoursFromNow_Dual`
  Current: {0} à¸Šà¸±à¹ˆà¸§à¹‚à¸¡à¸‡à¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸Šà¸±à¹ˆà¸§à¹‚à¸¡à¸‡
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleHoursFromNow_Paucal`
  Current: {0} à¸Šà¸±à¹ˆà¸§à¹‚à¸¡à¸‡à¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸Šà¸±à¹ˆà¸§à¹‚à¸¡à¸‡
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleHoursFromNow_Plural`
  Current: {0} à¸Šà¸±à¹ˆà¸§à¹‚à¸¡à¸‡à¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸Šà¸±à¹ˆà¸§à¹‚à¸¡à¸‡
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleHoursFromNow_Singular`
  Current: {0} à¸Šà¸±à¹ˆà¸§à¹‚à¸¡à¸‡à¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸Šà¸±à¹ˆà¸§à¹‚à¸¡à¸‡
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleMinutesFromNow`
  Current: {0} à¸™à¸²à¸—à¸µà¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸™à¸²à¸—à¸µ
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleMinutesFromNow_Dual`
  Current: {0} à¸™à¸²à¸—à¸µà¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸™à¸²à¸—à¸µ
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleMinutesFromNow_Paucal`
  Current: {0} à¸™à¸²à¸—à¸µà¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸™à¸²à¸—à¸µ
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleMinutesFromNow_Plural`
  Current: {0} à¸™à¸²à¸—à¸µà¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸™à¸²à¸—à¸µ
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleMinutesFromNow_Singular`
  Current: {0} à¸™à¸²à¸—à¸µà¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸™à¸²à¸—à¸µ
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleMonthsFromNow`
  Current: {0} à¹€à¸”à¸·à¸­à¸™à¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¹€à¸”à¸·à¸­à¸™
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleMonthsFromNow_Dual`
  Current: {0} à¹€à¸”à¸·à¸­à¸™à¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¹€à¸”à¸·à¸­à¸™
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleMonthsFromNow_Paucal`
  Current: {0} à¹€à¸”à¸·à¸­à¸™à¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¹€à¸”à¸·à¸­à¸™
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleMonthsFromNow_Plural`
  Current: {0} à¹€à¸”à¸·à¸­à¸™à¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¹€à¸”à¸·à¸­à¸™
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleMonthsFromNow_Singular`
  Current: {0} à¹€à¸”à¸·à¸­à¸™à¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¹€à¸”à¸·à¸­à¸™
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleSecondsFromNow`
  Current: {0} à¸§à¸´à¸™à¸²à¸—à¸µà¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸§à¸´à¸™à¸²à¸—à¸µ
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleSecondsFromNow_Dual`
  Current: {0} à¸§à¸´à¸™à¸²à¸—à¸µà¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸§à¸´à¸™à¸²à¸—à¸µ
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleSecondsFromNow_Paucal`
  Current: {0} à¸§à¸´à¸™à¸²à¸—à¸µà¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸§à¸´à¸™à¸²à¸—à¸µ
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleSecondsFromNow_Plural`
  Current: {0} à¸§à¸´à¸™à¸²à¸—à¸µà¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸§à¸´à¸™à¸²à¸—à¸µ
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleSecondsFromNow_Singular`
  Current: {0} à¸§à¸´à¸™à¸²à¸—à¸µà¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸§à¸´à¸™à¸²à¸—à¸µ
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleYearsFromNow`
  Current: {0} à¸›à¸µà¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸›à¸µ
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleYearsFromNow_Dual`
  Current: {0} à¸›à¸µà¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸›à¸µ
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleYearsFromNow_Paucal`
  Current: {0} à¸›à¸µà¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸›à¸µ
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleYearsFromNow_Plural`
  Current: {0} à¸›à¸µà¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸›à¸µ
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_MultipleYearsFromNow_Singular`
  Current: {0} à¸›à¸µà¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸ {0} à¸›à¸µ
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_SingleHourFromNow`
  Current: à¸«à¸™à¸¶à¹ˆà¸‡à¸Šà¸±à¹ˆà¸§à¹‚à¸¡à¸‡à¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸à¸«à¸™à¸¶à¹ˆà¸‡à¸Šà¸±à¹ˆà¸§à¹‚à¸¡à¸‡
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_SingleMinuteFromNow`
  Current: à¸«à¸™à¸¶à¹ˆà¸‡à¸™à¸²à¸—à¸µà¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸à¸«à¸™à¸¶à¹ˆà¸‡à¸™à¸²à¸—à¸µ
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_SingleMonthFromNow`
  Current: à¸«à¸™à¸¶à¹ˆà¸‡à¹€à¸”à¸·à¸­à¸™à¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸à¸«à¸™à¸¶à¹ˆà¸‡à¹€à¸”à¸·à¸­à¸™
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_SingleSecondFromNow`
  Current: à¸«à¸™à¸¶à¹ˆà¸‡à¸§à¸´à¸™à¸²à¸—à¸µà¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸à¸«à¸™à¸¶à¹ˆà¸‡à¸§à¸´à¸™à¸²à¸—à¸µ
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [th] `DateHumanize_SingleYearFromNow`
  Current: à¸«à¸™à¸¶à¹ˆà¸‡à¸›à¸µà¸ˆà¸²à¸à¸™à¸µà¹‰
  Proposed: à¸­à¸µà¸à¸«à¸™à¸¶à¹ˆà¸‡à¸›à¸µ
  Status: defect / Severity: P2 / Confidence: high
  Rationale: à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸Ÿà¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸ªà¸³à¸™à¸§à¸™à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹ƒà¸™à¸­à¸™à¸²à¸„à¸•à¸ à¸²à¸©à¸²à¹„à¸—à¸¢; à¸œà¸¹à¹‰à¹ƒà¸Šà¹‰à¹„à¸—à¸¢à¸—à¸±à¹ˆà¸§à¹„à¸›à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "à¸­à¸µà¸ ..." à¸¡à¸²à¸à¸à¸§à¹ˆà¸²à¹à¸¥à¸°à¸­à¹ˆà¸²à¸™à¸¥à¸·à¹ˆà¸™à¸à¸§à¹ˆà¸²à¹ƒà¸™ UI.
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¸•à¹ˆà¸­à¹€à¸™à¸·à¹ˆà¸­à¸‡; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¹ƒà¸Šà¹‰à¸‡à¸²à¸™à¸£à¸¹à¸›à¹à¸šà¸š FromNow à¹ƒà¸™à¸§à¸±à¸’à¸™à¸˜à¸£à¸£à¸¡à¸¥à¸¹à¸ th-TH
  Notes: th-TH fallback à¹‚à¸”à¸¢à¸£à¸§à¸¡à¸¢à¸­à¸¡à¸£à¸±à¸šà¹„à¸”à¹‰ (à¸¡à¸µà¸„à¸µà¸¢à¹Œà¹€à¸‰à¸žà¸²à¸° TwoDays/Age/Paucal à¸„à¸£à¸š) à¹à¸•à¹ˆà¸‚à¹‰à¸­à¸„à¸§à¸²à¸¡ FromNow à¸‚à¸­à¸‡ parent locale à¸¢à¸±à¸‡à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´
- [tr] `DateHumanize_TwoDaysFromNow`
  Current: yarından sonra
  Proposed: öbür gün
  Status: suspicious / Severity: P3 / Confidence: high
  Rationale: Mevcut ifade anlaşılır olsa da günlük Türkçede 'two days from now' için en doğal ve kalıp kullanım 'öbür gün'dür.
  Evidence: src/Humanizer/Properties/Resources.tr.resx: DateHumanize_TwoDaysFromNow = 'yarından sonra'; tests/Humanizer.Tests/Localisation/tr/DateHumanizeTests.cs: explicit residual key assertion uses current literal
  Notes: Üslup/doğallık düzeltmesi; anlamsal hata değil.
- [uk] `DataUnit_BitSymbol`
  Current: б
  Proposed: b
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Символ біта має бути латинською "b" за міжнародним технічним стандартом; кирилична "б" виглядає як локалізація слова, а не одиниці виміру.
  Evidence: src/Humanizer/Properties/Resources.uk.resx:123
  Notes: Inherited by uk-UA via fallback.
- [uk] `ENE`
  Current: схід-північний схід
  Proposed: східно-північний схід
  Status: suspicious / Severity: P3 / Confidence: medium
  Rationale: Для складених румбів українською нормативніше прислівникове поєднання ("східно-"), а форма з іменником "схід-" звучить кальковано.
  Evidence: src/Humanizer/Properties/Resources.uk.resx:636; tests/Humanizer.Tests/Localisation/uk-UA/HeadingTests.cs:30
  Notes: Owned child-culture fallback (uk-UA) currently asserts this form.
- [uk] `ESE`
  Current: схід-південний схід
  Proposed: східно-південний схід
  Status: suspicious / Severity: P3 / Confidence: medium
  Rationale: У технічній українській номенклатурі напрямків природніше "східно-південний", а не "схід-південний".
  Evidence: src/Humanizer/Properties/Resources.uk.resx:648; tests/Humanizer.Tests/Localisation/uk-UA/HeadingTests.cs:32
  Notes: Owned child-culture fallback (uk-UA) currently asserts this form.
- [uk] `NNE`
  Current: північ-північний схід
  Proposed: північно-північний схід
  Status: suspicious / Severity: P3 / Confidence: medium
  Rationale: Форма "північ-північний" стилістично груба; у румбах усталений прикметниково-прислівниковий компонент "північно-".
  Evidence: src/Humanizer/Properties/Resources.uk.resx:624; tests/Humanizer.Tests/Localisation/uk-UA/HeadingTests.cs:28
  Notes: Owned child-culture fallback (uk-UA) currently asserts this form.
- [uk] `NNW`
  Current: північ-північний захід
  Proposed: північно-північний захід
  Status: suspicious / Severity: P3 / Confidence: medium
  Rationale: Для складеного напрямку природніша й нормативніша форма "північно-північний захід".
  Evidence: src/Humanizer/Properties/Resources.uk.resx:708; tests/Humanizer.Tests/Localisation/uk-UA/HeadingTests.cs:42
  Notes: Owned child-culture fallback (uk-UA) currently asserts this form.
- [uk] `SSE`
  Current: південь-південний схід
  Proposed: південно-південний схід
  Status: suspicious / Severity: P3 / Confidence: medium
  Rationale: У цій позиції очікується "південно-"; чинна форма сприймається як буквальна калька, а не природна українська назва румба.
  Evidence: src/Humanizer/Properties/Resources.uk.resx:660; tests/Humanizer.Tests/Localisation/uk-UA/HeadingTests.cs:34
  Notes: Owned child-culture fallback (uk-UA) currently asserts this form.
- [uk] `SSW`
  Current: південь-південний захід
  Proposed: південно-південний захід
  Status: suspicious / Severity: P3 / Confidence: medium
  Rationale: Для складеного напряму нормативна модель: "південно-південний захід".
  Evidence: src/Humanizer/Properties/Resources.uk.resx:672; tests/Humanizer.Tests/Localisation/uk-UA/HeadingTests.cs:36
  Notes: Owned child-culture fallback (uk-UA) currently asserts this form.
- [uk] `WNW`
  Current: захід-північний захід
  Proposed: західно-північний захід
  Status: suspicious / Severity: P3 / Confidence: medium
  Rationale: Краще узгоджується зі словотвірною нормою румбів: "західно-північний", а не "захід-північний".
  Evidence: src/Humanizer/Properties/Resources.uk.resx:696; tests/Humanizer.Tests/Localisation/uk-UA/HeadingTests.cs:40
  Notes: Owned child-culture fallback (uk-UA) currently asserts this form.
- [uk] `WSW`
  Current: захід-південний захід
  Proposed: західно-південний захід
  Status: suspicious / Severity: P3 / Confidence: medium
  Rationale: Для цього румба природніша форма "західно-південний захід".
  Evidence: src/Humanizer/Properties/Resources.uk.resx:684; tests/Humanizer.Tests/Localisation/uk-UA/HeadingTests.cs:38
  Notes: Owned child-culture fallback (uk-UA) currently asserts this form.
- [uz-Cyrl-UZ] `DateHumanize_MultipleMinutesAgo`
  Current: {0} Ð¼Ð¸Ð½ÑƒÑ‚ Ð°Ð²Ð²Ð°Ð»
  Proposed: {0} Ð´Ð°Ò›Ð¸Ò›Ð° Ð°Ð²Ð²Ð°Ð»
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Ð£Ð·Ð±ÐµÐº Ð°Ð´Ð°Ð±Ð¸Ð¹ Ñ‚Ð¸Ð»Ð¸Ð´Ð° Ð²Ð°Ò›Ñ‚ Ð±Ð¸Ñ€Ð»Ð¸Ð³Ð¸ ÑƒÑ‡ÑƒÐ½ "Ð´Ð°Ò›Ð¸Ò›Ð°" Ð¼ÐµÑŠÑ‘Ñ€Ð¸Ð¹ ÑˆÐ°ÐºÐ». "{0} Ð¼Ð¸Ð½ÑƒÑ‚ Ð°Ð²Ð²Ð°Ð»" Ñ€ÑƒÑÑ‡Ð°Ð³Ð° Ð¾Ò“Ð³Ð°Ð½ Ð²Ð° ÑˆÑƒ Ð»Ð¾ÐºÐ°Ð»Ð´Ð°Ð³Ð¸ "Ð±Ð¸Ñ€ Ð´Ð°Ò›Ð¸Ò›Ð° Ð°Ð²Ð²Ð°Ð»" Ò³Ð°Ð¼Ð´Ð° TimeUnit Ñ‚Ð°Ñ€Ð¶Ð¸Ð¼Ð°Ð»Ð°Ñ€Ð¸ Ð±Ð¸Ð»Ð°Ð½ ÑƒÑÐ»ÑƒÐ±Ð¸Ð¹ Ð·Ð¸Ð´.
  Evidence: src/Humanizer/Properties/Resources.uz-Cyrl-UZ.resx: DateHumanize_MultipleMinutesAgo={0} Ð¼Ð¸Ð½ÑƒÑ‚ Ð°Ð²Ð²Ð°Ð»; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/DateHumanizeTests.cs: MinutesAgo(10) expects "10 Ð¼Ð¸Ð½ÑƒÑ‚ Ð°Ð²Ð²Ð°Ð»"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/DateHumanizeTests.cs: MinutesAgo(1) expects "Ð±Ð¸Ñ€ Ð´Ð°Ò›Ð¸Ò›Ð° Ð°Ð²Ð²Ð°Ð»"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeUnitToSymbolTests.cs: TimeUnit.Minute expects "Ð´Ð°Ò›Ð¸Ò›Ð°"
  Notes: Normalize lexical choice to meyoriy Uzbek (Ð´Ð°Ò›Ð¸Ò›Ð°) across minute forms.
- [uz-Cyrl-UZ] `DateHumanize_MultipleMinutesFromNow`
  Current: {0} Ð¼Ð¸Ð½ÑƒÑ‚Ð´Ð°Ð½ ÑÑžÐ½Ð³
  Proposed: {0} Ð´Ð°Ò›Ð¸Ò›Ð°Ð´Ð°Ð½ ÑÑžÐ½Ð³
  Status: defect / Severity: P2 / Confidence: high
  Rationale: "Ð”Ð°Ò›Ð¸Ò›Ð°" Ð¼ÐµÑŠÑ‘Ñ€Ð¸Ð¹ ÑˆÐ°ÐºÐ»; "Ð¼Ð¸Ð½ÑƒÑ‚Ð´Ð°Ð½" Ñ€ÑƒÑÑ‡Ð° ÐºÐ°Ð»ÑŒÐºÐ° Ð±ÑžÐ»Ð¸Ð±, ÑˆÑƒ Ñ„Ð°Ð¹Ð»Ð´Ð°Ð³Ð¸ "Ð±Ð¸Ñ€ Ð´Ð°Ò›Ð¸Ò›Ð°Ð´Ð°Ð½ ÑÑžÐ½Ð³" Ð±Ð¸Ð»Ð°Ð½ Ð±Ð¸Ñ€ Ð»Ð¾ÐºÐ°Ð»Ð´Ð° Ð°Ñ€Ð°Ð»Ð°Ñˆ ÑƒÑÐ»ÑƒÐ± Ò³Ð¾ÑÐ¸Ð» Ò›Ð¸Ð»ÑÐ¿Ñ‚Ð¸.
  Evidence: src/Humanizer/Properties/Resources.uz-Cyrl-UZ.resx: DateHumanize_MultipleMinutesFromNow={0} Ð¼Ð¸Ð½ÑƒÑ‚Ð´Ð°Ð½ ÑÑžÐ½Ð³; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/DateHumanizeTests.cs: MinutesFromNow(10) expects "10 Ð¼Ð¸Ð½ÑƒÑ‚Ð´Ð°Ð½ ÑÑžÐ½Ð³"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/DateHumanizeTests.cs: MinutesFromNow(1) expects "Ð±Ð¸Ñ€ Ð´Ð°Ò›Ð¸Ò›Ð°Ð´Ð°Ð½ ÑÑžÐ½Ð³"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeUnitToSymbolTests.cs: TimeUnit.Minute expects "Ð´Ð°Ò›Ð¸Ò›Ð°"
  Notes: Keep postposition pattern (Ð´Ð°Ð½ ÑÑžÐ½Ð³), only normalize time-unit lexeme.
- [uz-Cyrl-UZ] `DateHumanize_MultipleSecondsAgo`
  Current: {0} ÑÐµÐºÑƒÐ½Ð´ Ð°Ð²Ð²Ð°Ð»
  Proposed: {0} ÑÐ¾Ð½Ð¸Ñ Ð°Ð²Ð²Ð°Ð»
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Ð£Ð·Ð±ÐµÐºÑ‡Ð°Ð´Ð° "ÑÐ¾Ð½Ð¸Ñ" Ð¼ÐµÑŠÑ‘Ñ€Ð¸Ð¹; "ÑÐµÐºÑƒÐ½Ð´" Ñ€ÑƒÑÑ‡Ð° Ð²Ð°Ñ€Ð¸Ð°Ð½Ñ‚. Ð‘Ð¸Ñ€Ð»Ð¸Ðº ÑˆÐ°ÐºÐ»Ð¸ Ð±Ñƒ Ð»Ð¾ÐºÐ°Ð»Ð´Ð° Ð°Ð»Ð»Ð°Ò›Ð°Ñ‡Ð¾Ð½ "Ð±Ð¸Ñ€ ÑÐ¾Ð½Ð¸Ñ Ð°Ð²Ð²Ð°Ð»" Ð´ÐµÐ± Ð±ÐµÑ€Ð¸Ð»Ð³Ð°Ð½, ÐºÑžÐ¿Ð»Ð¸ÐºÐ´Ð° Ò³Ð°Ð¼ ÑˆÑƒ Ð±Ð°Ð·Ð° ÑÐ°Ò›Ð»Ð°Ð½Ð¸ÑˆÐ¸ ÐºÐµÑ€Ð°Ðº.
  Evidence: src/Humanizer/Properties/Resources.uz-Cyrl-UZ.resx: DateHumanize_MultipleSecondsAgo={0} ÑÐµÐºÑƒÐ½Ð´ Ð°Ð²Ð²Ð°Ð»; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/DateHumanizeTests.cs: SecondsAgo(10) expects "10 ÑÐµÐºÑƒÐ½Ð´ Ð°Ð²Ð²Ð°Ð»"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/DateHumanizeTests.cs: SecondsAgo(1) expects "Ð±Ð¸Ñ€ ÑÐ¾Ð½Ð¸Ñ Ð°Ð²Ð²Ð°Ð»"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeUnitToSymbolTests.cs: TimeUnit.Second expects "ÑÐ¾Ð½Ð¸Ñ"
  Notes: Lexical consistency with singular second and TimeUnit symbols.
- [uz-Cyrl-UZ] `DateHumanize_MultipleSecondsFromNow`
  Current: {0} ÑÐµÐºÑƒÐ½Ð´Ð´Ð°Ð½ ÑÑžÐ½Ð³
  Proposed: {0} ÑÐ¾Ð½Ð¸ÑÐ´Ð°Ð½ ÑÑžÐ½Ð³
  Status: defect / Severity: P2 / Confidence: high
  Rationale: "Ð¡Ð¾Ð½Ð¸Ñ" Ð°Ð´Ð°Ð±Ð¸Ð¹ Ð¼ÐµÑŠÑ‘Ñ€Ð³Ð° Ð¼Ð¾Ñ; "ÑÐµÐºÑƒÐ½Ð´Ð´Ð°Ð½" Ñ€ÑƒÑÑ‡Ð° Ñ‚Ð°ÑŠÑÐ¸Ñ€. "Ð‘Ð¸Ñ€ ÑÐ¾Ð½Ð¸ÑÐ´Ð°Ð½ ÑÑžÐ½Ð³" ÑˆÐ°ÐºÐ»Ð¸ Ð»Ð¾ÐºÐ°Ð»Ð´Ð° Ð¼Ð°Ð²Ð¶ÑƒÐ´, Ð´ÐµÐ¼Ð°Ðº ÐºÑžÐ¿Ð»Ð¸Ðº ÑƒÑ‡ÑƒÐ½ Ò³Ð°Ð¼ ÑˆÑƒ Ð»ÐµÐºÑÐµÐ¼Ð° Ò›ÑžÐ»Ð»Ð°Ð½Ð¸ÑˆÐ¸ ÐºÐµÑ€Ð°Ðº.
  Evidence: src/Humanizer/Properties/Resources.uz-Cyrl-UZ.resx: DateHumanize_MultipleSecondsFromNow={0} ÑÐµÐºÑƒÐ½Ð´Ð´Ð°Ð½ ÑÑžÐ½Ð³; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/DateHumanizeTests.cs: SecondsFromNow(10) expects "10 ÑÐµÐºÑƒÐ½Ð´Ð´Ð°Ð½ ÑÑžÐ½Ð³"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/DateHumanizeTests.cs: SecondsFromNow(1) expects "Ð±Ð¸Ñ€ ÑÐ¾Ð½Ð¸ÑÐ´Ð°Ð½ ÑÑžÐ½Ð³"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeUnitToSymbolTests.cs: TimeUnit.Second expects "ÑÐ¾Ð½Ð¸Ñ"
  Notes: Normalize only noun; keep tense construction intact.
- [uz-Cyrl-UZ] `TimeSpanHumanize_MultipleMinutes`
  Current: {0} Ð¼Ð¸Ð½ÑƒÑ‚
  Proposed: {0} Ð´Ð°Ò›Ð¸Ò›Ð°
  Status: defect / Severity: P2 / Confidence: high
  Rationale: "ÐœÐ¸Ð½ÑƒÑ‚" ÑžÑ€Ð½Ð¸Ð³Ð° "Ð´Ð°Ò›Ð¸Ò›Ð°" Ð¼ÐµÑŠÑ‘Ñ€Ð¸Ð¹ Ð²Ð° ÑˆÑƒ Ð»Ð¾ÐºÐ°Ð»Ð½Ð¸Ð½Ð³ TimeUnit Ò›Ð°Ñ‚Ð¾Ñ€Ð»Ð°Ñ€Ð¸ Ð±Ð¸Ð»Ð°Ð½ Ð¼Ð¾Ñ. Ò²Ð¾Ð·Ð¸Ñ€Ð³Ð¸ Ò³Ð¾Ð»Ð°Ñ‚Ð´Ð° TimeSpan Ð²Ð° TimeUnit ÑžÑ€Ñ‚Ð°ÑÐ¸Ð´Ð° Ñ‚ÐµÑ€Ð¼Ð¸Ð½Ð¾Ð»Ð¾Ð³Ð¸Ðº Ð¿Ð°Ñ€Ñ‡Ð°Ð»Ð°Ð½Ð¸Ñˆ Ð±Ð¾Ñ€.
  Evidence: src/Humanizer/Properties/Resources.uz-Cyrl-UZ.resx: TimeSpanHumanize_MultipleMinutes={0} Ð¼Ð¸Ð½ÑƒÑ‚; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeSpanHumanizeTests.cs: Minutes(2) expects "2 Ð¼Ð¸Ð½ÑƒÑ‚"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeUnitToSymbolTests.cs: TimeUnit.Minute expects "Ð´Ð°Ò›Ð¸Ò›Ð°"
  Notes: Terminology harmonization across Humanize surfaces.
- [uz-Cyrl-UZ] `TimeSpanHumanize_MultipleSeconds`
  Current: {0} ÑÐµÐºÑƒÐ½Ð´
  Proposed: {0} ÑÐ¾Ð½Ð¸Ñ
  Status: defect / Severity: P2 / Confidence: high
  Rationale: "Ð¡Ð¾Ð½Ð¸Ñ" Ð¼ÐµÑŠÑ‘Ñ€Ð¸Ð¹ ÑƒÐ·Ð±ÐµÐºÑ‡Ð° ÑˆÐ°ÐºÐ»; "ÑÐµÐºÑƒÐ½Ð´" Ñ€ÑƒÑ Ñ‚Ð¸Ð»Ð¸ Ñ‚Ð°ÑŠÑÐ¸Ñ€Ð¸. Ð¨Ñƒ Ð»Ð¾ÐºÐ°Ð»Ð´Ð° TimeUnit Ð²Ð° DateHumanize Ð±Ð¸Ñ€Ð»Ð¸Ðº ÑˆÐ°ÐºÐ»Ð»Ð°Ñ€Ð¸ "ÑÐ¾Ð½Ð¸Ñ" Ð´ÐµÐ± ÐºÐµÐ»Ð³Ð°Ð½.
  Evidence: src/Humanizer/Properties/Resources.uz-Cyrl-UZ.resx: TimeSpanHumanize_MultipleSeconds={0} ÑÐµÐºÑƒÐ½Ð´; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeSpanHumanizeTests.cs: Seconds(2) expects "2 ÑÐµÐºÑƒÐ½Ð´"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeUnitToSymbolTests.cs: TimeUnit.Second expects "ÑÐ¾Ð½Ð¸Ñ"
  Notes: Align with normalized temporal lexicon.
- [uz-Cyrl-UZ] `TimeSpanHumanize_SingleMinute`
  Current: 1 Ð¼Ð¸Ð½ÑƒÑ‚
  Proposed: 1 Ð´Ð°Ò›Ð¸Ò›Ð°
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Ð¡Ð¾Ð½Ð´Ð°Ð½ ÐºÐµÐ¹Ð¸Ð½ Ò³Ð°Ð¼ Ð²Ð°Ò›Ñ‚ Ð±Ð¸Ñ€Ð»Ð¸Ð³Ð¸ Ð¼ÐµÑŠÑ‘Ñ€Ð¸Ð¹ Ñ€Ð°Ð²Ð¸ÑˆÐ´Ð° "Ð´Ð°Ò›Ð¸Ò›Ð°" Ð±ÑžÐ»Ð¸ÑˆÐ¸ ÐºÐµÑ€Ð°Ðº. "1 Ð¼Ð¸Ð½ÑƒÑ‚" ÑƒÐ·Ð±ÐµÐºÑ‡Ð° Ð¸Ð½Ñ‚ÐµÑ€Ñ„ÐµÐ¹ÑÐ´Ð° ÑÑ‚Ð¸Ð»Ð¸ÑÑ‚Ð¸Ðº Ñ€ÑƒÑÐ¸Ð·Ð¼ Ð±ÑžÐ»Ð¸Ð± Ñ‚ÑƒÑŽÐ»Ð°Ð´Ð¸.
  Evidence: src/Humanizer/Properties/Resources.uz-Cyrl-UZ.resx: TimeSpanHumanize_SingleMinute=1 Ð¼Ð¸Ð½ÑƒÑ‚; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeSpanHumanizeTests.cs: Minutes(1) expects "1 Ð¼Ð¸Ð½ÑƒÑ‚"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeUnitToSymbolTests.cs: TimeUnit.Minute expects "Ð´Ð°Ò›Ð¸Ò›Ð°"
  Notes: Use same noun for singular/plural for Uzbek numeral constructions.
- [uz-Cyrl-UZ] `TimeSpanHumanize_SingleSecond`
  Current: 1 ÑÐµÐºÑƒÐ½Ð´
  Proposed: 1 ÑÐ¾Ð½Ð¸Ñ
  Status: defect / Severity: P2 / Confidence: high
  Rationale: "1 ÑÐµÐºÑƒÐ½Ð´" ÑžÑ€Ð½Ð¸Ð³Ð° "1 ÑÐ¾Ð½Ð¸Ñ" Ð°Ð´Ð°Ð±Ð¸Ð¹ Ð½Ð¾Ñ€Ð¼Ð°Ð³Ð° Ð¼Ð¾Ñ Ð²Ð° ÑˆÑƒ Ð»Ð¾ÐºÐ°Ð»Ð´Ð°Ð³Ð¸ Ð±Ð¾ÑˆÒ›Ð° Ò›Ð°Ñ‚Ð¾Ñ€Ð»Ð°Ñ€ Ð±Ð¸Ð»Ð°Ð½ ÑƒÐ¹Ò“ÑƒÐ½.
  Evidence: src/Humanizer/Properties/Resources.uz-Cyrl-UZ.resx: TimeSpanHumanize_SingleSecond=1 ÑÐµÐºÑƒÐ½Ð´; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeSpanHumanizeTests.cs: Seconds(1) expects "1 ÑÐµÐºÑƒÐ½Ð´"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeUnitToSymbolTests.cs: TimeUnit.Second expects "ÑÐ¾Ð½Ð¸Ñ"
  Notes: Keep numeral format; replace noun only.
- [uz-Latn-UZ] `DateHumanize_MultipleDaysFromNow`
  Current: {0} kundan so`ng
  Proposed: {0} kundan so'ng
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Uz-Latn matnida apostrof belgisi bir xil bo'lishi kerak; bu joylarda backtick ishlatilgani uchun imlo notekis ko'rinadi.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Ma'no saqlangan, lekin imlo va belgilar birxilligi buzilgan.
- [uz-Latn-UZ] `DateHumanize_MultipleDaysFromNow_Paucal`
  Current: {0} kundan so`ng
  Proposed: {0} kundan so'ng
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Uz-Latn matnida apostrof belgisi bir xil bo'lishi kerak; bu joylarda backtick ishlatilgani uchun imlo notekis ko'rinadi.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Ma'no saqlangan, lekin imlo va belgilar birxilligi buzilgan.
- [uz-Latn-UZ] `DateHumanize_MultipleHoursFromNow`
  Current: {0} soatdan so`ng
  Proposed: {0} soatdan so'ng
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Uz-Latn matnida apostrof belgisi bir xil bo'lishi kerak; bu joylarda backtick ishlatilgani uchun imlo notekis ko'rinadi.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Ma'no saqlangan, lekin imlo va belgilar birxilligi buzilgan.
- [uz-Latn-UZ] `DateHumanize_MultipleMinutesFromNow`
  Current: {0} minutdan so`ng
  Proposed: {0} minutdan so'ng
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Uz-Latn matnida apostrof belgisi bir xil bo'lishi kerak; bu joylarda backtick ishlatilgani uchun imlo notekis ko'rinadi.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Ma'no saqlangan, lekin imlo va belgilar birxilligi buzilgan.
- [uz-Latn-UZ] `DateHumanize_MultipleMonthsFromNow`
  Current: {0} oydan so`ng
  Proposed: {0} oydan so'ng
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Uz-Latn matnida apostrof belgisi bir xil bo'lishi kerak; bu joylarda backtick ishlatilgani uchun imlo notekis ko'rinadi.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Ma'no saqlangan, lekin imlo va belgilar birxilligi buzilgan.
- [uz-Latn-UZ] `DateHumanize_MultipleSecondsFromNow`
  Current: {0} sekunddan so`ng
  Proposed: {0} sekunddan so'ng
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Uz-Latn matnida apostrof belgisi bir xil bo'lishi kerak; bu joylarda backtick ishlatilgani uchun imlo notekis ko'rinadi.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Ma'no saqlangan, lekin imlo va belgilar birxilligi buzilgan.
- [uz-Latn-UZ] `DateHumanize_MultipleYearsFromNow`
  Current: {0} yildan so`ng
  Proposed: {0} yildan so'ng
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Uz-Latn matnida apostrof belgisi bir xil bo'lishi kerak; bu joylarda backtick ishlatilgani uchun imlo notekis ko'rinadi.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Ma'no saqlangan, lekin imlo va belgilar birxilligi buzilgan.
- [uz-Latn-UZ] `DateHumanize_SingleHourFromNow`
  Current: bir soatdan so`ng
  Proposed: bir soatdan so'ng
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Uz-Latn matnida apostrof belgisi bir xil bo'lishi kerak; bu joylarda backtick ishlatilgani uchun imlo notekis ko'rinadi.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Ma'no saqlangan, lekin imlo va belgilar birxilligi buzilgan.
- [uz-Latn-UZ] `DateHumanize_SingleMinuteFromNow`
  Current: bir daqiqadan so`ng
  Proposed: bir daqiqadan so'ng
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Uz-Latn matnida apostrof belgisi bir xil bo'lishi kerak; bu joylarda backtick ishlatilgani uchun imlo notekis ko'rinadi.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Ma'no saqlangan, lekin imlo va belgilar birxilligi buzilgan.
- [uz-Latn-UZ] `DateHumanize_SingleMonthFromNow`
  Current: bir oydan so`ng
  Proposed: bir oydan so'ng
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Uz-Latn matnida apostrof belgisi bir xil bo'lishi kerak; bu joylarda backtick ishlatilgani uchun imlo notekis ko'rinadi.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Ma'no saqlangan, lekin imlo va belgilar birxilligi buzilgan.
- [uz-Latn-UZ] `DateHumanize_SingleSecondFromNow`
  Current: bir soniyadan so`ng
  Proposed: bir soniyadan so'ng
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Uz-Latn matnida apostrof belgisi bir xil bo'lishi kerak; bu joylarda backtick ishlatilgani uchun imlo notekis ko'rinadi.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Ma'no saqlangan, lekin imlo va belgilar birxilligi buzilgan.
- [uz-Latn-UZ] `DateHumanize_SingleYearFromNow`
  Current: bir yildan so`ng
  Proposed: bir yildan so'ng
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Uz-Latn matnida apostrof belgisi bir xil bo'lishi kerak; bu joylarda backtick ishlatilgani uchun imlo notekis ko'rinadi.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Ma'no saqlangan, lekin imlo va belgilar birxilligi buzilgan.
- [uz-Latn-UZ] `DateHumanize_TwoDaysAgo`
  Current: o`tgan kun
  Proposed: 2 kun avval
  Status: defect / Severity: P1 / Confidence: high
  Rationale: Joriy ibora ikki kun oldin ma'nosini aniq bermaydi, odatda kechaga yaqin talqin qilinadi.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: DateHumanize_TwoDaysAgo qiymati semantik noaniq; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs: shu qiymat tekshirilgan
  Notes: Aniq vaqt ofseti uchun sonli variant afzal.
- [uz-Latn-UZ] `DateHumanize_TwoDaysFromNow`
  Current: indindan keyin
  Proposed: indinga
  Status: defect / Severity: P1 / Confidence: high
  Rationale: Joriy ibora tabiiy emas; indinga ikki kundan keyinni tabiiy va aniq ifodalaydi.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: DateHumanize_TwoDaysFromNow qiymati leksik noaniq; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs: shu qiymat tekshirilgan
  Notes: Leksik tabiiylik va aniqlik tuzatildi.
- [uz-Latn-UZ] `TimeSpanHumanize_Zero`
  Current: vaqt yo`q
  Proposed: vaqt yo'q
  Status: suspicious / Severity: P2 / Confidence: high
  Rationale: Uz-Latn matnida apostrof belgisi bir xil bo'lishi kerak; bu joylarda backtick ishlatilgani uchun imlo notekis ko'rinadi.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Ma'no saqlangan, lekin imlo va belgilar birxilligi buzilgan.
- [vi] `TimeSpanHumanize_MultipleMilliseconds`
  Current: {0} phần ngàn giây
  Proposed: {0} mili giây
  Status: defect / Severity: P2 / Confidence: high
  Rationale: "phần ngàn giây" đúng nghĩa đen nhưng gượng và không tự nhiên trong tiếng Việt hiện đại; cách dùng phổ biến và nhất quán trong giao diện là "mili giây".
  Evidence: src/Humanizer/Properties/Resources.vi.resx: TimeUnit_Millisecond = "mili giây" (chuẩn thuật ngữ đã có sẵn trong cùng locale).; tests/Humanizer.Tests/Localisation/vi/TimeUnitToSymbolTests.cs: xác nhận biểu diễn millisecond dùng "mili giây".
  Notes: Nên đồng bộ thuật ngữ millisecond giữa TimeSpanHumanize và TimeUnit.
- [vi] `TimeSpanHumanize_SingleMillisecond`
  Current: 1 phần ngàn giây
  Proposed: 1 mili giây
  Status: defect / Severity: P2 / Confidence: high
  Rationale: Bản hiện tại mang tính dịch sát nghĩa kỹ thuật, không tự nhiên với người Việt; "1 mili giây" là dạng chuẩn, dễ hiểu và thống nhất với các chuỗi liên quan.
  Evidence: src/Humanizer/Properties/Resources.vi.resx: TimeUnit_Millisecond = "mili giây".; tests/Humanizer.Tests/Localisation/vi/TimeSpanHumanizeTests.cs: đang khóa chuỗi cũ "1 phần ngàn giây".
  Notes: Cần cập nhật test mong đợi sau khi sửa chuỗi.
- [vi] `TimeSpanHumanize_Zero`
  Current: không giờ
  Proposed: không có thời gian
  Status: defect / Severity: P2 / Confidence: high
  Rationale: "không giờ" trong tiếng Việt thường được hiểu là mốc giờ (ví dụ 0h), không diễn đạt tự nhiên nghĩa "no time" của một khoảng thời lượng bằng 0.
  Evidence: src/Humanizer/Properties/Resources.vi.resx: TimeSpanHumanize_Zero có giá trị "không giờ".; tests/Humanizer.Tests/Localisation/vi/TimeSpanHumanizeTests.cs: NoTimeToWords hiện kỳ vọng "không giờ".
  Notes: Đề xuất ưu tiên tính tự nhiên ngữ nghĩa; có thể cân nhắc "không có thời lượng" nếu muốn văn phong kỹ thuật hơn.
- [zh-Hant] `TimeSpanHumanize_MultipleWeeks`
  Current: {0} 周
  Proposed: {0} 週
  Status: defect / Severity: P2 / Confidence: high
  Rationale: 「周」在繁中可讀，但 zh-Hant/zh-HK 的星期量詞慣用為「週」，目前譯法帶有簡轉繁痕跡，地區自然度不足。
  Evidence: src/Humanizer/Properties/Resources.zh-Hant.resx:213; tests/Humanizer.Tests/Localisation/zh-Hant/TimeSpanHumanizeTests.cs:28; tests/Humanizer.Tests/Localisation/zh-HK/TimeSpanHumanizeTests.cs:26
  Notes: zh-HK 目前回退同字形，語意可懂但不屬香港常用書寫。
- [zh-Hant] `TimeSpanHumanize_MultipleWeeks_Dual`
  Current: {0} 周
  Proposed: {0} 週
  Status: defect / Severity: P2 / Confidence: high
  Rationale: 複數變體沿用「周」，建議統一改為「週」以符合繁體中文地區用法。
  Evidence: src/Humanizer/Properties/Resources.zh-Hant.resx:606; tests/Humanizer.Tests/Localisation/zh-Hant/TimeSpanHumanizeTests.cs:29; tests/Humanizer.Tests/Localisation/zh-HK/TimeSpanHumanizeTests.cs:27
  Notes: 需與主要 weeks 字串同步修正，避免詞形不一致。
- [zh-Hant] `TimeSpanHumanize_MultipleWeeks_Paucal`
  Current: {0} 周
  Proposed: {0} 週
  Status: defect / Severity: P2 / Confidence: high
  Rationale: 此詞形與主詞條相同問題，繁中語境建議固定使用「週」。
  Evidence: src/Humanizer/Properties/Resources.zh-Hant.resx:249; tests/Humanizer.Tests/Localisation/zh-Hant/TimeSpanHumanizeTests.cs:28; tests/Humanizer.Tests/Localisation/zh-HK/TimeSpanHumanizeTests.cs:26
  Notes: zh-HK 回退可接受但不優。
- [zh-Hant] `TimeSpanHumanize_MultipleWeeks_Plural`
  Current: {0} 周
  Proposed: {0} 週
  Status: defect / Severity: P2 / Confidence: high
  Rationale: 多數型仍應與繁中慣例一致使用「週」，目前字形偏離本地寫法。
  Evidence: src/Humanizer/Properties/Resources.zh-Hant.resx:492; tests/Humanizer.Tests/Localisation/zh-Hant/TimeSpanHumanizeTests.cs:29; tests/Humanizer.Tests/Localisation/zh-HK/TimeSpanHumanizeTests.cs:27
  Notes: 建議與其他 week 詞條一起批次替換。
- [zh-Hant] `TimeSpanHumanize_MultipleWeeks_Singular`
  Current: {0} 周
  Proposed: {0} 週
  Status: defect / Severity: P2 / Confidence: high
  Rationale: 單數型用「周」在繁中仍可理解，但非最自然地區寫法。
  Evidence: src/Humanizer/Properties/Resources.zh-Hant.resx:369; tests/Humanizer.Tests/Localisation/zh-Hant/TimeSpanHumanizeTests.cs:28; tests/Humanizer.Tests/Localisation/zh-HK/TimeSpanHumanizeTests.cs:26
  Notes: 同系列詞條需一致改為「週」。
- [zh-Hant] `TimeSpanHumanize_SingleWeek`
  Current: 1 周
  Proposed: 1 週
  Status: defect / Severity: P2 / Confidence: high
  Rationale: 單位「week」在繁體中文常寫作「週」，「1 周」屬非首選本地用法。
  Evidence: src/Humanizer/Properties/Resources.zh-Hant.resx:237; tests/Humanizer.Tests/Localisation/zh-Hant/TimeSpanHumanizeTests.cs:28; tests/Humanizer.Tests/Localisation/zh-HK/TimeSpanHumanizeTests.cs:26
  Notes: 影響 zh-HK 直接回退結果。
- [zh-Hant] `TimeSpanHumanize_SingleWeeks_Words`
  Current: 1 周
  Proposed: 1 週
  Status: defect / Severity: P2 / Confidence: high
  Rationale: toWords 變體仍應沿用「週」；目前與繁中在地慣用字形不一致。
  Evidence: src/Humanizer/Properties/Resources.zh-Hant.resx:393; tests/Humanizer.Tests/Localisation/zh-Hant/TimeSpanHumanizeTests.cs:28; tests/Humanizer.Tests/Localisation/zh-HK/TimeSpanHumanizeTests.cs:26
  Notes: 建議與 SingleWeek 同步修正。
- [zh-Hant] `TimeUnit_Week`
  Current: 周
  Proposed: 週
  Status: defect / Severity: P2 / Confidence: high
  Rationale: 基礎時間單位建議使用「週」，可同時提升 zh-Hant 與 zh-HK 回退輸出的在地自然度。
  Evidence: src/Humanizer/Properties/Resources.zh-Hant.resx:483; tests/Humanizer.Tests/Localisation/zh-Hant/TimeUnitToSymbolTests.cs:12; tests/Humanizer.Tests/Localisation/zh-HK/TimeUnitToSymbolTests.cs:12
  Notes: 屬父文化字串，修正後可改善子文化 zh-HK fallback。

## Per-Locale Summary
- `af`: 4 defect, 0 suspicious, 88 ok
- `ar`: 7 defect, 1 suspicious, 114 ok
- `az`: 1 defect, 0 suspicious, 99 ok
- `bg`: 7 defect, 0 suspicious, 97 ok
- `bn`: 2 defect, 0 suspicious, 66 ok
- `ca`: 0 defect, 15 suspicious, 171 ok
- `cs`: 1 defect, 0 suspicious, 109 ok
- `da`: 0 defect, 1 suspicious, 93 ok
- `de`: 0 defect, 0 suspicious, 112 ok
- `el`: 7 defect, 0 suspicious, 67 ok
- `es`: 25 defect, 0 suspicious, 161 ok
- `fa`: 2 defect, 0 suspicious, 70 ok
- `fi`: 1 defect, 0 suspicious, 62 ok
- `fil`: 7 defect, 1 suspicious, 177 ok
- `fr`: 1 defect, 0 suspicious, 86 ok
- `he`: 6 defect, 0 suspicious, 108 ok
- `hr`: 2 defect, 0 suspicious, 107 ok
- `hu`: 2 defect, 0 suspicious, 184 ok
- `hy`: 1 defect, 0 suspicious, 73 ok
- `id`: 1 defect, 12 suspicious, 71 ok
- `is`: 2 defect, 0 suspicious, 142 ok
- `it`: 3 defect, 1 suspicious, 80 ok
- `ja`: 0 defect, 0 suspicious, 42 ok
- `ko`: 1 defect, 0 suspicious, 185 ok
- `ku`: 0 defect, 0 suspicious, 134 ok
- `lb`: 0 defect, 0 suspicious, 72 ok
- `lt`: 0 defect, 0 suspicious, 153 ok
- `lv`: 1 defect, 0 suspicious, 168 ok
- `ms`: 1 defect, 0 suspicious, 136 ok
- `mt`: 2 defect, 0 suspicious, 184 ok
- `nb`: 2 defect, 0 suspicious, 88 ok
- `nl`: 0 defect, 0 suspicious, 78 ok
- `pl`: 0 defect, 4 suspicious, 106 ok
- `pt`: 43 defect, 0 suspicious, 143 ok
- `pt-BR`: 8 defect, 0 suspicious, 177 ok
- `ro`: 0 defect, 0 suspicious, 76 ok
- `ru`: 0 defect, 0 suspicious, 186 ok
- `sk`: 0 defect, 0 suspicious, 110 ok
- `sl`: 0 defect, 0 suspicious, 156 ok
- `sr`: 4 defect, 0 suspicious, 126 ok
- `sr-Latn`: 0 defect, 0 suspicious, 130 ok
- `sv`: 2 defect, 0 suspicious, 82 ok
- `th`: 35 defect, 0 suspicious, 102 ok
- `tr`: 0 defect, 1 suspicious, 91 ok
- `uk`: 1 defect, 8 suspicious, 189 ok
- `uz-Cyrl-UZ`: 8 defect, 0 suspicious, 76 ok
- `uz-Latn-UZ`: 2 defect, 13 suspicious, 69 ok
- `vi`: 3 defect, 0 suspicious, 81 ok
- `zh-CN`: 0 defect, 0 suspicious, 186 ok
- `zh-Hans`: 0 defect, 0 suspicious, 186 ok
- `zh-Hant`: 8 defect, 0 suspicious, 178 ok
