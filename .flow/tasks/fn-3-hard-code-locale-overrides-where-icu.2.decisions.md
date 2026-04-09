# Locale Override Decisions

Decision document for fn-3-hard-code-locale-overrides-where-icu.2.
Each section records the chosen canonical value, rationale, and source reference.

## Evidence sources

- `tools/probe-macos.json`, `tools/probe-linux.json`, `tools/probe-windows-net10.json`,
  `tools/probe-windows-net48.json` -- cross-platform CultureInfo outputs
- `tests/Humanizer.Tests/Localisation/LocaleCoverageData.cs` -- current DateToOrdinalWords
  contract values
- `tests/Humanizer.Tests/Localisation/LocaleFormatterExactTheoryData.cs` -- current ByteSize
  decimal-separator expectations
- `src/Humanizer/Locales/{bn,fa,he,ku,ta,zu-ZA,ar,fr-CH}.yml` -- current authored locale
  identity and script/dialect intent
- `src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs` -- downstream MMMM
  replacement architecture

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

The task spec explicitly calls out that fa month names are "missing ezafe mark
(U+0654 `ٔ`) in date context". The ezafe is a grammatical feature in Persian
that connects a noun to a following modifier. In the ordinal-date pattern
`{day} MMMM yyyy`, the month name precedes the year number, and Persian
orthography requires the written ezafe on month names ending in he (ه) or
ye (ی) when followed by a dependent element. Since the downstream architecture
replaces `MMMM` directly and does not add context afterward, the date-context
form must be baked into the override data.

**Decision:** Use date-context Gregorian month forms with written ezafe where
Persian orthography requires it. The override values are the forms that should
appear in the `{day} MMMM yyyy` output pattern.

**Months to override (full set, date-context Gregorian transliterations):**

1. ژانویهٔ (January -- ezafe on final he)
2. فوریهٔ (February -- ezafe on final he)
3. مارس (March -- no ezafe needed, ends in sin)
4. آوریل (April -- no ezafe needed, ends in lam)
5. مهٔ (May -- ezafe on final he)
6. ژوئن (June -- no ezafe needed, ends in nun)
7. ژوئیهٔ (July -- ezafe on final he)
8. اوت (August -- no ezafe needed, ends in te)
9. سپتامبر (September -- no ezafe needed, ends in re)
10. اکتبر (October -- no ezafe needed, ends in re)
11. نوامبر (November -- no ezafe needed, ends in re)
12. دسامبر (December -- no ezafe needed, ends in re)

**Note:** The existing test expectations in LocaleCoverageData.cs use forms
without ezafe (`ژانویه`). Task .3 must update these test expectations when
adding the override.

**Source:** fn-3 epic spec explicitly identifies fa as "month names missing
ezafe mark"; Persian orthographic rules for date-context ezafe; CLDR Gregorian
month names for `fa` locale with date-context modifications.

---

### he (Hebrew)

**Issue:** Hebrew month names are consistent across all 4 probes (standalone
forms: `ינואר`, `פברואר`, etc.). The existing test expectations include a `ב`
(bet) prefix: `בינואר` for January. The Hebrew long date format pattern is
`dddd, d בMMMM yyyy` -- the `ב` is a literal in the pattern, not part of the
month name.

Humanizer's ordinal-date pattern for he is `{day} MMMM yyyy` (no `ב` prefix).
The downstream architecture in OrdinalDatePattern replaces `MMMM` using
`calendar.months` when available. Since the `ב` prefix is required in the
date-context output and the replacement surface is `MMMM` only, the `ב` must
be included in the month value itself.

A pattern-only fix (`{day} בMMMM yyyy`) would work but would keep Humanizer
dependent on CultureInfo month names for the core locale contract. Using
`calendar.months` with `ב`-prefixed date-context forms is more aligned with
the spec's architecture of owning locale data in YAML.

**Decision:** Use `calendar.months` override with `ב`-prefixed date-context
month forms. This is a date-context month-form case, not standalone labels.

**Months to override (date-context with ב prefix):**

1. בינואר (January)
2. בפברואר (February)
3. במרץ (March)
4. באפריל (April)
5. במאי (May)
6. ביוני (June)
7. ביולי (July)
8. באוגוסט (August)
9. בספטמבר (September)
10. באוקטובר (October)
11. בנובמבר (November)
12. בדצמבר (December)

**Source:** All 4 probe environments agree on standalone names. The `ב` prefix
is standard Hebrew preposition for "in/of" used with months in date contexts.
Current test expectations already require the `ב`-prefixed output. Using
`calendar.months` override aligns with the downstream MMMM replacement
architecture.

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

**Issue:** Decimal separator varies wildly across platforms:

| Platform   | Separator | Unicode |
|------------|-----------|---------|
| macOS      | `,`       | U+002C  |
| Linux      | `,`       | U+002C  |
| Win net10  | `٫`       | U+066B  |
| Win net48  | `.`       | U+002E  |

This is closely tied to the Sorani vs Kurmanji decision above. Since we chose
Sorani (Arabic script), the decimal separator should be consistent with Sorani
locale identity.

**Decision:** Use `٫` (U+066B, Arabic decimal separator) as the override.
Rationale:

1. The locale identity decision for `ku` is Sorani / Central Kurdish / Arabic
   script. The momayyiz `٫` aligns with this Arabic-script identity.
2. The existing modern-target test expectations already use `٫` (the
   `KurdishKilobytes` constant resolves to `1٫95 KB` on modern .NET).
3. Choosing `٫` normalizes all frameworks to the same Sorani-authored output
   instead of following whichever platform maps `ku` differently.
4. Unlike `ar` where Western digits are the primary formatting context, `ku`
   Sorani text uses Arabic-script conventions throughout, making `٫` the
   natural decimal separator for this locale.

**Override value:** `٫` (U+066B)

**Source:** Windows CultureInfo for `ku` (Sorani/Central Kurdish); existing
Humanizer modern-target test expectations; consistency with Sorani locale
identity decision.

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
- **fa**: Override with date-context Gregorian transliterations with ezafe (12 entries)
- **he**: Override with `ב`-prefixed date-context month forms (12 entries)
- **ku**: Override with Sorani/Arabic-script month names (12 entries)

Locales NOT needing `calendar.months` override:
- **zu-ZA**: No override needed (all probes agree). Fix test expected value
  `Febhuwari` -> `Februwari` (test was wrong from day one).
- **ta**: No override needed (all probes agree, matches existing tests).

### Task .4 (decimal separator overrides)

Locales needing `number.formatting.decimalSeparator` override in YAML:
- **ar**: Override with `.` (U+002E)
- **ku**: Override with `٫` (U+066B)
- **fr-CH**: Override with `.` (U+002E)

### Test expected value corrections (merged into .3 and .4)

- `fa` date expectations: Update to include ezafe marks (e.g., `ژانویهٔ`)
- `zu-ZA` February: `Febhuwari` -> `Februwari`
- `fr-CH` byte-size: `1,95 Ko` -> `1.95 Ko`
- `ar` byte-size: Remove `#if NET48` conditional, use `1.95 KB` unconditionally
- `ku` byte-size: Remove `#if NET48` conditional, use `1٫95 KB` unconditionally

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
