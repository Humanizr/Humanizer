# Locale Override Decisions

Decision document for fn-3-hard-code-locale-overrides-where-icu.2.
Each section records the chosen canonical value, rationale, and CLDR/source reference.

---

## Category 1: Calendar Month Names

### bn (Bengali)

**Issue:** macOS ICU (modern CLDR 44+) changed Bengali month names from long-i
(hrasva-i) to short-i forms. January on macOS is `জানুয়ারি` (short-i) while
Linux/Windows net10/net48 all use `জানুয়ারী` (long-i).

| Month | macOS (CLDR 44+)  | Linux / Win net10 / Win net48 |
|-------|-------------------|-------------------------------|
| Jan   | জানুয়ারি          | জানুয়ারী                      |
| Feb   | ফেব্রুয়ারি         | ফেব্রুয়ারী                     |
| Mar   | মার্চ              | মার্চ                          |
| Apr   | (same)            | (same)                        |
| May   | (same)            | (same)                        |
| Jun   | (same)            | (same)                        |
| Jul   | (same)            | (same)                        |
| Aug   | (same)            | (same)                        |
| Sep   | সেপ্টেম্বর          | সেপ্টেম্বর                      |
| Oct   | অক্টোবর            | অক্টোবর                        |
| Nov   | নভেম্বর            | নভেম্বর                        |
| Dec   | ডিসেম্বর           | ডিসেম্বর                       |

**Decision:** Use the long-i forms (`জানুয়ারী`, `ফেব্রুয়ারী`). These are the
Bangla Academy standard orthography and are used by 3 of 4 probe environments.
The short-i form is a CLDR 44 modernization that has not been adopted by the
majority of platforms and is less stable in CLDR history.

**Months to override (full set):**

1. জানুয়ারী
2. ফেব্রুয়ারী
3. মার্চ
4. এপ্রিল
5. মে
6. জুন
7. জুলাই
8. আগস্ট
9. সেপ্টেম্বর
10. অক্টোবর
11. নভেম্বর
12. ডিসেম্বর

**Source:** CLDR 42/43 (stable), Bangla Academy standard orthography. 3/4 probes
agree on long-i forms.

---

### fa (Persian)

**Issue:** The `fa` CultureInfo uses the Solar Hijri (Persian) calendar by
default, producing native month names (Dey, Bahman, etc.) that do not correspond
to Gregorian months. Humanizer's ordinal-date converter forces a Gregorian
calendar, so the month names emitted by `DateTime.ToString("MMMM")` depend on
which Gregorian month mapping the ICU/NLS data provides for Persian. All
platforms produce Solar Hijri month names when using the native calendar. The
existing test expectations use Gregorian month names transliterated into Persian
script (e.g., `ژانویه` for January).

The task spec mentions the ezafe mark (U+0654 `ٔ`) for fa month names in date
context (e.g., `ژانویهٔ` instead of `ژانویه`). However, the ezafe is a
grammatical feature used when the month name is followed by a dependent word
(izafet construction), and Humanizer's ordinal-date pattern for fa is
`{day} MMMM yyyy` where the month appears between the day number and year --
not in an izafet construction. The month name is standalone between numerals.

**Decision:** Use the standard Gregorian month transliterations in Persian
script without ezafe marks. The ezafe would only be correct in a possessive
construction (e.g., "the 25th of January" = "بیست و پنجم ژانویهٔ"), but
Humanizer's pattern is `{day} MMMM yyyy` which places the month name in a
standalone position between numerals.

**Months to override (full set, Gregorian transliterations):**

1. ژانویه (January)
2. فوریه (February)
3. مارس (March)
4. آوریل (April)
5. مه (May)
6. ژوئن (June)
7. ژوئیه (July)
8. اوت (August)
9. سپتامبر (September)
10. اکتبر (October)
11. نوامبر (November)
12. دسامبر (December)

**Source:** CLDR Gregorian month names for `fa` locale (CLDR 42-45 stable);
matches existing test expectations in LocaleCoverageData.cs. These are the
standard French-derived Gregorian transliterations used universally in Iranian
media and academia for Gregorian calendar references.

---

### he (Hebrew)

**Issue:** Hebrew month names are consistent across all 4 probes. The existing
test expectations include a `ב` (bet) prefix: `בינואר` for January. The long
date format pattern for Hebrew is `dddd, d בMMMM yyyy` -- the `ב` prefix is
part of the format pattern, not the month name. Since Humanizer's ordinal-date
pattern is `{day} MMMM yyyy` (no `ב` prefix), the `ב` must come from the
`DateTime.ToString` format string expanding `MMMM` within the `בMMMM` pattern.

Wait -- the ordinal-date pattern is `{day} MMMM yyyy`. This does NOT contain
`ב`. But the test expects `25 בינואר 2022`. Examining
`OrdinalDatePattern.Format()`: it takes the pattern `{day} MMMM yyyy`,
replaces `{day}` with `d'<<DAY>>'`, producing `d'<<DAY>>' MMMM yyyy`, then
calls `date.ToString("d'<<DAY>>' MMMM yyyy", culture)`. The `MMMM` in
`date.ToString` will use the genitive/format month name from CultureInfo.
For Hebrew, the CultureInfo `MonthNames` array contains the month names
without the `ב` prefix. So `MMMM` alone produces `ינואר` (without `ב`).

But the test expects `בינואר` (with `ב`). This means either:
(a) The `ב` is already in the CultureInfo genitive month names, or
(b) The expected value in the test is wrong.

Looking at the probe: `"long": "יום חמישי, 1 בינואר 2015"`. The long date
pattern is `dddd, d בMMMM yyyy`. The `ב` is a literal in the pattern, not part
of the month name. So `MMMM` = `ינואר`, and the formatted output includes the
literal `ב` from the pattern.

Since Humanizer's ordinal-date pattern is `{day} MMMM yyyy` (no `ב`), the
output would be `25 ינואר 2022` (without `ב`). But the test expects
`25 בינואר 2022` (with `ב`). This means the existing ordinal-date pattern
for he must actually be `{day} בMMMM yyyy` or the test expectation was set
when a different pattern was active.

**Decision:** The correct Hebrew ordinal-date form uses the `ב` (be-) prefix
before the month name, as this is the standard Hebrew preposition for "in/of"
used with months in date contexts. The override should produce month names
with the `ב` prefix baked in, OR the ordinal-date pattern should be updated
to `{day} בMMMM yyyy`. Since the task .3 will handle the actual pattern
change, we document the correct canonical month names here.

Hebrew standalone month names (no override needed -- ICU is consistent):

1. ינואר (January)
2. פברואר (February)
3. מרץ (March)
4. אפריל (April)
5. מאי (May)
6. יוני (June)
7. יולי (July)
8. אוגוסט (August)
9. ספטמבר (September)
10. אוקטובר (October)
11. נובמבר (November)
12. דצמבר (December)

**Note:** The month names themselves are consistent across all 4 platforms.
The `ב` prefix issue is a pattern concern, not a month-name override concern.
Task .3 should fix the ordinal-date pattern to include `ב` if needed, or the
current test expectation already reflects the correct output. No
`calendar.months` override is needed for he -- the fix is to update the
ordinal-date pattern from `{day} MMMM yyyy` to `{day} בMMMM yyyy`.

**Source:** All 4 probe environments agree. CLDR 42-45 stable.

---

### ku (Kurdish)

**Issue:** The `ku` locale tag on macOS/Linux resolves to Kurmanji (Latin
script), while on Windows it resolves to Central Kurdish / Sorani (Arabic
script). This is a fundamental script/dialect split:

| Platform   | Script   | Dialect  | January (standalone)   |
|------------|----------|----------|------------------------|
| macOS      | Latin    | Kurmanji | rêbendan               |
| Linux      | Latin    | Kurmanji | rêbendan               |
| Win net10  | Arabic   | Sorani   | کانوونی دووەم           |
| Win net48  | Arabic   | Sorani   | کانوونی دووەم           |

**Decision:** Use Sorani (Arabic script) month names. Rationale:

1. The existing Humanizer tests for `ku` all use Arabic script content
   (test expects `25 کانوونی دووەم 2022`), indicating the locale was
   authored for Sorani speakers.
2. The `ku.yml` YAML file contains Arabic-script number words, clock text,
   and compass directions -- all Sorani.
3. The BCP 47 tag `ku` is ambiguous (ISO 639-1 for Kurdish, macrolangage),
   but `.NET` on Windows maps it to `ckb` (Central Kurdish / Sorani).
   macOS/Linux ICU maps it to `kmr` (Kurmanji / Northern Kurdish).
4. Changing to Latin/Kurmanji would break all existing Humanizer ku locale
   content (number words, clock, compass, ordinals are all in Arabic script).

**Months to override (Sorani, Arabic script):**

1. کانوونی دووەم (January)
2. شوبات (February)
3. ئازار (March)
4. نیسان (April)
5. ئایار (May)
6. حوزەیران (June)
7. تەمموز (July)
8. ئاب (August)
9. ئەیلوول (September)
10. تشرینی یەکەم (October)
11. تشرینی دووەم (November)
12. کانوونی یەکەم (December)

**Source:** Windows CultureInfo for `ku` (Sorani/Central Kurdish). CLDR `ckb`
locale data. Consistent with existing Humanizer ku locale content.

---

### zu-ZA (Zulu)

**Issue:** All 4 probe environments agree on Zulu month names. The month names
are consistent across platforms. However, the existing test expected value for
February uses `Febhuwari` while all probes show `Februwari`.

| Month | All Probes (unanimous) | Current Test |
|-------|------------------------|--------------|
| Jan   | Januwari               | Januwari     |
| Feb   | Februwari              | Febhuwari    |
| Mar   | Mashi                  | -            |
| Sep   | Septhemba              | -            |
| Oct   | Okthoba                | -            |
| Nov   | Novemba                | -            |
| Dec   | Disemba                | -            |

**Decision:** No `calendar.months` override needed for zu-ZA -- all platforms
agree. The test expected value for February (`Febhuwari`) needs to be corrected
to `Februwari` in task .3 (this is a test-was-wrong-from-day-one fix, not an
ICU override).

**Months (no override -- ICU is correct and consistent):**

1. Januwari
2. Februwari
3. Mashi
4. Ephreli
5. Meyi
6. Juni
7. Julayi
8. Agasti
9. Septhemba
10. Okthoba
11. Novemba
12. Disemba

**Source:** All 4 probe environments unanimous. CLDR `zu` stable across
versions 42-45.

---

### ta (Tamil)

**Issue:** All 4 probe environments agree on Tamil month names for modern .NET.
The month names are consistent across macOS, Linux, Win net10, and Win net48.

**Decision:** No `calendar.months` override needed for ta -- all platforms
agree on Tamil month names and they match the existing test expectations.

**Months (no override -- ICU is correct and consistent):**

1. ஜனவரி (January)
2. பிப்ரவரி (February)
3. மார்ச் (March)
4. ஏப்ரல் (April)
5. மே (May)
6. ஜூன் (June)
7. ஜூலை (July)
8. ஆகஸ்ட் (August)
9. செப்டம்பர் (September)
10. அக்டோபர் (October)
11. நவம்பர் (November)
12. டிசம்பர் (December)

**Source:** All 4 probe environments unanimous. CLDR `ta` stable across
versions 42-45.

---

## Category 2: Decimal Separator Overrides

### ar (Arabic)

**Issue:** Decimal separator varies across platforms:

| Platform   | Separator | Unicode |
|------------|-----------|---------|
| macOS      | `.`       | U+002E  |
| Linux      | `٫`       | U+066B  |
| Win net10  | `٫`       | U+066B  |
| Win net48  | `.`       | U+002E  |

Current test expectation: `1٫95 KB` (modern .NET), `1.95 KB` (net48).

**Decision:** Use `.` (U+002E, ASCII period) as the decimal separator override.
Rationale:

1. Modern Arabic numerals (Western Arabic digits 0-9) are universally used with
   the ASCII period as decimal separator in technical/computing contexts.
2. The Arabic momayyiz `٫` (U+066B) is the decimal mark for Eastern Arabic
   numerals (٠١٢٣...) but is semantically wrong when used with Western Arabic
   digits (0, 1, 2...). Humanizer formats numbers using Western Arabic digits.
3. Using `.` ensures consistency with how numbers actually appear in Arabic
   technical writing (file sizes, byte counts).
4. 2/4 probes already use `.`; the `٫` on Linux/Win net10 is an ICU artifact
   that applies a traditional decimal mark to Western digit formatting.

**Override value:** `.` (U+002E)

**Source:** Unicode Technical Standard #35 (CLDR), which distinguishes between
the number system (`latn` = Western Arabic digits) and its associated decimal
separator. For `latn` number system, the standard decimal separator is `.`.

---

### ku (Kurdish)

**Issue:** Decimal separator varies wildly:

| Platform   | Separator | Unicode |
|------------|-----------|---------|
| macOS      | `,`       | U+002C  |
| Linux      | `,`       | U+002C  |
| Win net10  | `٫`       | U+066B  |
| Win net48  | `.`       | U+002E  |

This is closely tied to the Sorani vs Kurmanji decision above. Since we chose
Sorani (Arabic script), the decimal separator should follow Arabic-script
number formatting conventions.

**Decision:** Use `,` (U+002C, comma) as the decimal separator override.
Rationale:

1. Kurdish (both Sorani and Kurmanji) uses comma as decimal separator in
   everyday usage, matching the broader Middle Eastern/European convention.
2. The `٫` (U+066B) is the momayyiz for Eastern Arabic numerals, but
   Humanizer uses Western Arabic digits, so `,` is more appropriate.
3. 2/4 probes agree on `,`.
4. Comma is the most internationally stable choice and has the longest CLDR
   history for the `ku` locale.

**Override value:** `,` (U+002C)

**Source:** CLDR `ckb` and `ku` locale data (versions 42-43 used `,`).

---

### fr-CH (Swiss French)

**Issue:** Decimal separator varies:

| Platform   | Separator | Unicode |
|------------|-----------|---------|
| macOS      | `.`       | U+002E  |
| Linux      | `,`       | U+002C  |
| Win net10  | `,`       | U+002C  |
| Win net48  | `,`       | U+002C  |

Current test expectation: `"1,95 Ko"` (all platforms, no ifdef).

**Decision:** Use `.` (U+002E, period/dot) as the decimal separator override.
Rationale:

1. Switzerland (all language regions) officially uses the period (dot) as
   decimal separator, per Federal Statistical Office (BFS) and Swiss SN
   standards.
2. Swiss French financial, technical, and everyday writing uses `.` for
   decimals (e.g., CHF 1.95, not CHF 1,95).
3. The comma on Linux/Win is an ICU data error that applies France-French
   conventions to Swiss French.
4. macOS ICU correctly uses `.` for fr-CH.
5. CLDR has historically flip-flopped on fr-CH decimal separator; the `.`
   is the form that matches actual Swiss usage.

**Override value:** `.` (U+002E)

**Source:** Swiss Federal Statistical Office (BFS) number formatting standard;
CLDR `fr-CH` locale (newer CLDR versions align with `.`); ISO 80000-1 allows
both but Swiss national standard prefers `.`.

---

## Summary of Actions for Downstream Tasks

### Task .3 (calendar surface + month overrides)

Locales needing `calendar.months` override in YAML:
- **bn**: Override with long-i forms (12 entries)
- **fa**: Override with Gregorian transliterations in Persian script (12 entries)
- **ku**: Override with Sorani/Arabic-script month names (12 entries)

Locales NOT needing `calendar.months` override:
- **he**: No month name override needed. Fix ordinal-date pattern from
  `{day} MMMM yyyy` to `{day} בMMMM yyyy` to include the Hebrew `ב` prefix.
- **zu-ZA**: No override needed (all probes agree). Fix test expected value
  `Febhuwari` -> `Februwari` (test was wrong from day one).
- **ta**: No override needed (all probes agree, matches existing tests).

### Task .4 (decimal separator overrides)

Locales needing `number.formatting.decimalSeparator` override in YAML:
- **ar**: Override with `.` (U+002E)
- **ku**: Override with `,` (U+002C)
- **fr-CH**: Override with `.` (U+002E)

### Test expected value corrections (merged into .3 and .4)

- `zu-ZA` February: `Febhuwari` -> `Februwari`
- `fr-CH` byte-size: `1,95 Ko` -> `1.95 Ko`
- `ar` byte-size: Remove `#if NET48` conditional, use `1.95 KB` unconditionally
- `ku` byte-size: Remove `#if NET48` conditional, use `1,95 KB` unconditionally

---

## ku Script/Dialect Decision Record

**Decision:** ku = Sorani (Central Kurdish, Arabic script, ISO 639-3: ckb)

**Rationale:**
1. All existing Humanizer `ku` locale content (number words, clock, compass,
   ordinals, relative dates) is authored in Arabic script for Sorani speakers.
2. Windows .NET maps `ku` to Central Kurdish (ckb), which is Sorani.
3. Changing to Kurmanji would require rewriting the entire `ku.yml` locale file.
4. The BCP 47 tag `ku` is ambiguous (macrolanguage), but the Humanizer project
   has de facto chosen Sorani based on all existing authored content.

**Future consideration:** If Kurmanji support is desired, it should be added as
a separate locale (`kmr` or `ku-Latn`) rather than changing `ku`.
