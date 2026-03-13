# Locale Adversarial Review Second Pass

Generated: 2026-03-13

## Summary
- Locales adjudicated: 38
- Confirmed findings: 176
- Modified findings: 11
- Rejected findings: 73

## Adjudication
### Modified
- [bg] `DateHumanize_MultipleDaysAgo`
  Original: Ð¿Ñ€ÐµÐ´Ð¸ {0} Ð´ÐµÐ½Ð° -> Ð¿Ñ€ÐµÐ´Ð¸ {0} Ð´Ð½Ð¸
  Final: Ð¿Ñ€ÐµÐ´Ð¸ {0} Ð´Ð½Ð¸
  Decision: modified
  Rationale: As a native-speaker second pass, I consider this a register choice rather than a hard error. "дена" is colloquial but broadly understood; "дни" is the more formal default.
  Evidence: src/Humanizer/Properties/Resources.bg.resx keeps the day-plural strings in colloquial form for these keys.; tests/Humanizer.Tests/Localisation/bg/DateHumanizeTests.cs and ResourcesTests.cs intentionally assert the same day form.
  Notes: Downgraded from defect to style-level concern; replacement remains optional for formal-register standardization.
- [bg] `DateHumanize_MultipleDaysAgo_Paucal`
  Original: Ð¿Ñ€ÐµÐ´Ð¸ {0} Ð´ÐµÐ½Ð° -> Ð¿Ñ€ÐµÐ´Ð¸ {0} Ð´Ð½Ð¸
  Final: Ð¿Ñ€ÐµÐ´Ð¸ {0} Ð´Ð½Ð¸
  Decision: modified
  Rationale: As a native-speaker second pass, I consider this a register choice rather than a hard error. "дена" is colloquial but broadly understood; "дни" is the more formal default.
  Evidence: src/Humanizer/Properties/Resources.bg.resx keeps the day-plural strings in colloquial form for these keys.; tests/Humanizer.Tests/Localisation/bg/DateHumanizeTests.cs and ResourcesTests.cs intentionally assert the same day form.
  Notes: Downgraded from defect to style-level concern; replacement remains optional for formal-register standardization.
- [bg] `DateHumanize_MultipleDaysFromNow`
  Original: ÑÐ»ÐµÐ´ {0} Ð´ÐµÐ½Ð° -> ÑÐ»ÐµÐ´ {0} Ð´Ð½Ð¸
  Final: ÑÐ»ÐµÐ´ {0} Ð´Ð½Ð¸
  Decision: modified
  Rationale: As a native-speaker second pass, I consider this a register choice rather than a hard error. "дена" is colloquial but broadly understood; "дни" is the more formal default.
  Evidence: src/Humanizer/Properties/Resources.bg.resx keeps the day-plural strings in colloquial form for these keys.; tests/Humanizer.Tests/Localisation/bg/DateHumanizeTests.cs and ResourcesTests.cs intentionally assert the same day form.
  Notes: Downgraded from defect to style-level concern; replacement remains optional for formal-register standardization.
- [bg] `DateHumanize_MultipleDaysFromNow_Paucal`
  Original: ÑÐ»ÐµÐ´ {0} Ð´ÐµÐ½Ð° -> ÑÐ»ÐµÐ´ {0} Ð´Ð½Ð¸
  Final: ÑÐ»ÐµÐ´ {0} Ð´Ð½Ð¸
  Decision: modified
  Rationale: As a native-speaker second pass, I consider this a register choice rather than a hard error. "дена" is colloquial but broadly understood; "дни" is the more formal default.
  Evidence: src/Humanizer/Properties/Resources.bg.resx keeps the day-plural strings in colloquial form for these keys.; tests/Humanizer.Tests/Localisation/bg/DateHumanizeTests.cs and ResourcesTests.cs intentionally assert the same day form.
  Notes: Downgraded from defect to style-level concern; replacement remains optional for formal-register standardization.
- [bg] `DateHumanize_TwoDaysAgo`
  Original: Ð¿Ñ€ÐµÐ´Ð¸ 2 Ð´ÐµÐ½Ð° -> Ð¿Ñ€ÐµÐ´Ð¸ 2 Ð´Ð½Ð¸
  Final: Ð¿Ñ€ÐµÐ´Ð¸ 2 Ð´Ð½Ð¸
  Decision: modified
  Rationale: As a native-speaker second pass, I consider this a register choice rather than a hard error. "дена" is colloquial but broadly understood; "дни" is the more formal default.
  Evidence: src/Humanizer/Properties/Resources.bg.resx keeps the day-plural strings in colloquial form for these keys.; tests/Humanizer.Tests/Localisation/bg/DateHumanizeTests.cs and ResourcesTests.cs intentionally assert the same day form.
  Notes: Downgraded from defect to style-level concern; replacement remains optional for formal-register standardization.
- [bg] `DateHumanize_TwoDaysFromNow`
  Original: ÑÐ»ÐµÐ´ 2 Ð´ÐµÐ½Ð° -> ÑÐ»ÐµÐ´ 2 Ð´Ð½Ð¸
  Final: ÑÐ»ÐµÐ´ 2 Ð´Ð½Ð¸
  Decision: modified
  Rationale: As a native-speaker second pass, I consider this a register choice rather than a hard error. "дена" is colloquial but broadly understood; "дни" is the more formal default.
  Evidence: src/Humanizer/Properties/Resources.bg.resx keeps the day-plural strings in colloquial form for these keys.; tests/Humanizer.Tests/Localisation/bg/DateHumanizeTests.cs and ResourcesTests.cs intentionally assert the same day form.
  Notes: Downgraded from defect to style-level concern; replacement remains optional for formal-register standardization.
- [bg] `TimeSpanHumanize_MultipleDays`
  Original: {0} Ð´ÐµÐ½Ð° -> {0} Ð´Ð½Ð¸
  Final: {0} Ð´Ð½Ð¸
  Decision: modified
  Rationale: As a native-speaker second pass, I consider this a register choice rather than a hard error. "дена" is colloquial but broadly understood; "дни" is the more formal default.
  Evidence: src/Humanizer/Properties/Resources.bg.resx keeps the day-plural strings in colloquial form for these keys.; tests/Humanizer.Tests/Localisation/bg/TimeSpanHumanizeTests.cs intentionally asserts the same day form.
  Notes: Downgraded from defect to style-level concern; replacement remains optional for formal-register standardization.
- [fr] `TimeSpanHumanize_Zero`
  Original: temps nul -> aucune durÃ©e
  Final: durée nulle
  Decision: modified
  Rationale: 'temps nul' est compréhensible mais peu idiomatique pour une durée; en français standard, la formulation naturelle pour TimeSpan.Zero est 'durée nulle'.
  Evidence: src/Humanizer/Properties/Resources.fr.resx:355 contient la valeur actuelle "temps nul" pour TimeSpanHumanize_Zero.; tests/Humanizer.Tests/Localisation/fr/TimeSpanHumanizeTests.cs:213 valide actuellement "temps nul".; tests/Humanizer.Tests/Localisation/fr-BE/TimeSpanHumanizeTests.cs:99 valide aussi "temps nul" via fallback parent fr; il n'existe pas de Resources.fr-BE.resx/Resources.fr-CH.resx.
  Notes: Deuxième passe: défaut confirmé mais remplacement ajusté de "aucune durée" vers "durée nulle" pour une formulation plus idiomatique et concise.
- [id] `TimeSpanHumanize_Zero`
  Original: waktu kosong -> tanpa waktu
  Final: tidak ada waktu
  Decision: modified
  Rationale: "waktu kosong" dalam bahasa Indonesia lebih sering dimaknai sebagai waktu luang/slot kosong, bukan durasi nol. Terjemahan yang lebih tepat untuk "no time" adalah "tidak ada waktu".
  Evidence: src/Humanizer/Properties/Resources.id.resx (TimeSpanHumanize_Zero).; tests/Humanizer.Tests/Localisation/id/TimeSpanHumanizeTests.cs (NoTimeToWords saat ini mengharapkan "waktu kosong").
  Notes: Modified in second pass: replacement adjusted to a more idiomatic zero-duration phrase.
- [sr] `DateHumanize_SingleMinuteAgo`
  Original: пре минут -> пре минуте
  Final: пре минута
  Decision: modified
  Rationale: У српском је основни облик "минут" (м.р.). Уз предлог "пре" иде генитив, па је нормативни облик "пре минута". Први предлог "пре минуте" погрешно мења род именице.
  Evidence: src/Humanizer/Properties/Resources.sr.resx:207; src/Humanizer/Properties/Resources.sr.resx:288; tests/Humanizer.Tests/Localisation/sr/DateHumanizeTests.cs:19
  Notes: Задржан је статус defect, али је замена коригована у односу на први пас.
- [vi] `TimeSpanHumanize_Zero`
  Original: không giờ -> không có thời gian
  Final: không có thời lượng
  Decision: modified
  Rationale: Đồng ý có lỗi với "không giờ" vì dễ hiểu thành mốc giờ 0h, không diễn đạt đúng nghĩa duration = 0. Tôi điều chỉnh phương án thay thế từ "không có thời gian" sang "không có thời lượng" để giảm sắc thái hội thoại "không rảnh" và bám sát ngữ cảnh TimeSpan.
  Evidence: src/Humanizer/Properties/Resources.vi.resx: TimeSpanHumanize_Zero = "không giờ".; tests/Humanizer.Tests/Localisation/vi/TimeSpanHumanizeTests.cs: NoTimeToWords hiện kỳ vọng "không giờ".
  Notes: Sửa thay thế đề xuất ở pass 1 theo hướng thuật ngữ hóa nhẹ; cần cập nhật test NoTimeToWords tương ứng khi áp dụng.

### Rejected
- [ar] `DateHumanize_SingleYearAgo`
  Original: Ã˜Â§Ã™â€žÃ˜Â¹Ã˜Â§Ã™â€¦ Ã˜Â§Ã™â€žÃ˜Â³Ã˜Â§Ã˜Â¨Ã™â€š -> Ù…Ù†Ø° Ø³Ù†Ø© ÙˆØ§Ø­Ø¯Ø©
  Final: Ã˜Â§Ã™â€žÃ˜Â¹Ã˜Â§Ã™â€¦ Ã˜Â§Ã™â€žÃ˜Â³Ã˜Â§Ã˜Â¨Ã™â€š
  Decision: rejected
  Rationale: The current wording is acceptable as a natural equivalent of "last year" in date-humanization contexts, so this does not need a localization change.
  Evidence: src/Humanizer/Properties/Resources.ar.resx: DateHumanize_SingleYearAgo; tests/Humanizer.Tests/Localisation/ar/DateHumanizeTests.cs: YearsAgo(-1)
  Notes: Second-pass independent review: keep current wording.
- [da] `DateHumanize_TwoDaysAgo`
  Original: forgårs -> i forgårs
  Final: forgårs
  Decision: rejected
  Rationale: Rejected. "forgårs" is standard, idiomatic Danish for "two days ago" and is natural in UI text; "i forgårs" is an alternative phrasing, not a required correction.
  Evidence: src/Humanizer/Properties/Resources.da.resx: DateHumanize_TwoDaysAgo = "forgårs".; tests/Humanizer.Tests/Localisation/da/ResourcesTests.cs: asserts "forgårs" for DateHumanize_TwoDaysAgo.
  Notes: Second-pass independent review: keep current translation.
- [hr] `DateHumanize_SingleDayAgo`
  Original: juÄer -> jučer
  Final: jučer
  Decision: rejected
  Rationale: Nalaz odbacujem: u stvarnom lokalizacijskom resursu vrijednost je ispravno "jučer". Problem "juÄer" je artefakt kodiranja u review zapisu (mojibake), a ne greška hrvatskog prijevoda.
  Evidence: src/Humanizer/Properties/Resources.hr.resx: DateHumanize_SingleDayAgo = "jučer"; tests/Humanizer.Tests/Localisation/hr/DateHumanizeTests.cs: [InlineData(-1, "jučer")]
  Notes: Nije potrebna izmjena lokalizacije; potrebno je samo ispravno kodiranje artefakta pregleda.
- [id] `DateHumanize_MultipleDaysFromNow`
  Original: {0} hari dari sekarang -> dalam {0} hari
  Final: {0} hari dari sekarang
  Decision: rejected
  Rationale: Bentuk "{0} ... dari sekarang" tetap alami dan lazim dipakai dalam bahasa Indonesia standar untuk menyatakan waktu relatif ke masa depan. Ini perbedaan gaya, bukan cacat terjemahan.
  Evidence: src/Humanizer/Properties/Resources.id.resx (keluarga DateHumanize_*FromNow).; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs (ekspektasi current strings).
  Notes: No change required.
- [id] `DateHumanize_MultipleDaysFromNow_Paucal`
  Original: {0} hari dari sekarang -> dalam {0} hari
  Final: {0} hari dari sekarang
  Decision: rejected
  Rationale: Bentuk "{0} ... dari sekarang" tetap alami dan lazim dipakai dalam bahasa Indonesia standar untuk menyatakan waktu relatif ke masa depan. Ini perbedaan gaya, bukan cacat terjemahan.
  Evidence: src/Humanizer/Properties/Resources.id.resx (keluarga DateHumanize_*FromNow).; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs (ekspektasi current strings).
  Notes: No change required.
- [id] `DateHumanize_MultipleHoursFromNow`
  Original: {0} jam dari sekarang -> dalam {0} jam
  Final: {0} jam dari sekarang
  Decision: rejected
  Rationale: Bentuk "{0} ... dari sekarang" tetap alami dan lazim dipakai dalam bahasa Indonesia standar untuk menyatakan waktu relatif ke masa depan. Ini perbedaan gaya, bukan cacat terjemahan.
  Evidence: src/Humanizer/Properties/Resources.id.resx (keluarga DateHumanize_*FromNow).; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs (ekspektasi current strings).
  Notes: No change required.
- [id] `DateHumanize_MultipleMinutesFromNow`
  Original: {0} menit dari sekarang -> dalam {0} menit
  Final: {0} menit dari sekarang
  Decision: rejected
  Rationale: Bentuk "{0} ... dari sekarang" tetap alami dan lazim dipakai dalam bahasa Indonesia standar untuk menyatakan waktu relatif ke masa depan. Ini perbedaan gaya, bukan cacat terjemahan.
  Evidence: src/Humanizer/Properties/Resources.id.resx (keluarga DateHumanize_*FromNow).; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs (ekspektasi current strings).
  Notes: No change required.
- [id] `DateHumanize_MultipleMonthsFromNow`
  Original: {0} bulan dari sekarang -> dalam {0} bulan
  Final: {0} bulan dari sekarang
  Decision: rejected
  Rationale: Bentuk "{0} ... dari sekarang" tetap alami dan lazim dipakai dalam bahasa Indonesia standar untuk menyatakan waktu relatif ke masa depan. Ini perbedaan gaya, bukan cacat terjemahan.
  Evidence: src/Humanizer/Properties/Resources.id.resx (keluarga DateHumanize_*FromNow).; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs (ekspektasi current strings).
  Notes: No change required.
- [id] `DateHumanize_MultipleSecondsFromNow`
  Original: {0} detik dari sekarang -> dalam {0} detik
  Final: {0} detik dari sekarang
  Decision: rejected
  Rationale: Bentuk "{0} ... dari sekarang" tetap alami dan lazim dipakai dalam bahasa Indonesia standar untuk menyatakan waktu relatif ke masa depan. Ini perbedaan gaya, bukan cacat terjemahan.
  Evidence: src/Humanizer/Properties/Resources.id.resx (keluarga DateHumanize_*FromNow).; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs (ekspektasi current strings).
  Notes: No change required.
- [id] `DateHumanize_MultipleYearsFromNow`
  Original: {0} tahun dari sekarang -> dalam {0} tahun
  Final: {0} tahun dari sekarang
  Decision: rejected
  Rationale: Bentuk "{0} ... dari sekarang" tetap alami dan lazim dipakai dalam bahasa Indonesia standar untuk menyatakan waktu relatif ke masa depan. Ini perbedaan gaya, bukan cacat terjemahan.
  Evidence: src/Humanizer/Properties/Resources.id.resx (keluarga DateHumanize_*FromNow).; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs (ekspektasi current strings).
  Notes: No change required.
- [id] `DateHumanize_SingleHourFromNow`
  Original: sejam dari sekarang -> sejam lagi
  Final: sejam dari sekarang
  Decision: rejected
  Rationale: Bentuk "{0} ... dari sekarang" tetap alami dan lazim dipakai dalam bahasa Indonesia standar untuk menyatakan waktu relatif ke masa depan. Ini perbedaan gaya, bukan cacat terjemahan.
  Evidence: src/Humanizer/Properties/Resources.id.resx (keluarga DateHumanize_*FromNow).; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs (ekspektasi current strings).
  Notes: No change required.
- [id] `DateHumanize_SingleMinuteFromNow`
  Original: semenit dari sekarang -> semenit lagi
  Final: semenit dari sekarang
  Decision: rejected
  Rationale: Bentuk "{0} ... dari sekarang" tetap alami dan lazim dipakai dalam bahasa Indonesia standar untuk menyatakan waktu relatif ke masa depan. Ini perbedaan gaya, bukan cacat terjemahan.
  Evidence: src/Humanizer/Properties/Resources.id.resx (keluarga DateHumanize_*FromNow).; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs (ekspektasi current strings).
  Notes: No change required.
- [id] `DateHumanize_SingleMonthFromNow`
  Original: sebulan dari sekarang -> sebulan lagi
  Final: sebulan dari sekarang
  Decision: rejected
  Rationale: Bentuk "{0} ... dari sekarang" tetap alami dan lazim dipakai dalam bahasa Indonesia standar untuk menyatakan waktu relatif ke masa depan. Ini perbedaan gaya, bukan cacat terjemahan.
  Evidence: src/Humanizer/Properties/Resources.id.resx (keluarga DateHumanize_*FromNow).; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs (ekspektasi current strings).
  Notes: No change required.
- [id] `DateHumanize_SingleSecondFromNow`
  Original: sedetik dari sekarang -> sedetik lagi
  Final: sedetik dari sekarang
  Decision: rejected
  Rationale: Bentuk "{0} ... dari sekarang" tetap alami dan lazim dipakai dalam bahasa Indonesia standar untuk menyatakan waktu relatif ke masa depan. Ini perbedaan gaya, bukan cacat terjemahan.
  Evidence: src/Humanizer/Properties/Resources.id.resx (keluarga DateHumanize_*FromNow).; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs (ekspektasi current strings).
  Notes: No change required.
- [id] `DateHumanize_SingleYearFromNow`
  Original: setahun dari sekarang -> setahun lagi
  Final: setahun dari sekarang
  Decision: rejected
  Rationale: Bentuk "{0} ... dari sekarang" tetap alami dan lazim dipakai dalam bahasa Indonesia standar untuk menyatakan waktu relatif ke masa depan. Ini perbedaan gaya, bukan cacat terjemahan.
  Evidence: src/Humanizer/Properties/Resources.id.resx (keluarga DateHumanize_*FromNow).; tests/Humanizer.Tests/Localisation/id/DateHumanizeTests.cs (ekspektasi current strings).
  Notes: No change required.
- [pl] `TimeUnit_Day`
  Original: dzień -> d
  Final: dzień
  Decision: rejected
  Rationale: Odrzucam zgłoszenie: w polskim locale pełne formy „dzień/tydzień/miesiąc/rok” w ToSymbol są naturalne i spójne z istniejącą konwencją Humanizera, gdzie dla części języków te jednostki nie są skracane do jednoznakowych symboli.
  Evidence: src/Humanizer/Properties/Resources.pl.resx (TimeUnit_Day = "dzień").; tests/Humanizer.Tests/Localisation/pl/TimeUnitToSymbolTests.cs (oczekiwane Day -> "dzień").; tests/Humanizer.Tests/Localisation/pl/TimeSpanHumanizeTests.cs (spójna terminologia: „1 dzień”, „2 dni”).
  Notes: Brak zmian; to kwestia preferencji skrótów, nie błąd lokalizacji.
- [pl] `TimeUnit_Month`
  Original: miesiąc -> mies.
  Final: miesiąc
  Decision: rejected
  Rationale: Odrzucam zgłoszenie: „miesiąc” jako symbol jednostki w tym API jest akceptowalny i idiomatyczny dla polskiego użytkownika. Wymuszenie skrótu „mies.” byłoby zmianą stylu, nie korektą jakości tłumaczenia.
  Evidence: src/Humanizer/Properties/Resources.pl.resx (TimeUnit_Month = "miesiąc").; tests/Humanizer.Tests/Localisation/pl/TimeUnitToSymbolTests.cs (oczekiwane Month -> "miesiąc").; tests/Humanizer.Tests/Localisation/pl/DateHumanizeTests.cs (spójna terminologia miesięcy w locale pl).
  Notes: Brak zmian; propozycja pass 1 dotyczyła wariantu stylistycznego.
- [pl] `TimeUnit_Week`
  Original: tydzień -> tydz.
  Final: tydzień
  Decision: rejected
  Rationale: Odrzucam zgłoszenie: „tydzień” jest poprawnym i naturalnym polskim oznaczeniem jednostki czasu w kontekście Humanizera. Skrót „tydz.” może być używany, ale nie jest tu wymagany jako jedyna poprawna forma.
  Evidence: src/Humanizer/Properties/Resources.pl.resx (TimeUnit_Week = "tydzień").; tests/Humanizer.Tests/Localisation/pl/TimeUnitToSymbolTests.cs (oczekiwane Week -> "tydzień").; tests/Humanizer.Tests/Localisation/pl/TimeSpanHumanizeTests.cs (spójnie: „1 tydzień”, „2 tygodnie”).
  Notes: Brak zmian; obecna wartość jest poprawna i naturalna.
- [pl] `TimeUnit_Year`
  Original: rok -> r.
  Final: rok
  Decision: rejected
  Rationale: Odrzucam zgłoszenie: „rok” jest standardową, jednoznaczną i naturalną formą dla użytkownika polskiego. Zamiana na „r.” to opcjonalny skrót redakcyjny, nie konieczna poprawka lokalizacyjna.
  Evidence: src/Humanizer/Properties/Resources.pl.resx (TimeUnit_Year = "rok").; tests/Humanizer.Tests/Localisation/pl/TimeUnitToSymbolTests.cs (oczekiwane Year -> "rok").; tests/Humanizer.Tests/Localisation/pl/DateHumanizeTests.cs oraz TimeSpanHumanizeTests.cs (konsekwentne użycie leksemu „rok/lat(a)”).
  Notes: Brak zmian; wykrycie pass 1 oceniam jako preferencję skrótu, nie defekt.
- [pt] `DateHumanize_MultipleDaysAgo`
  Original: hÃƒÂ¡ {0} dias -> hÃ¡ {0} dias
  Final: hÃ¡ {0} dias
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleDaysAgo_Dual`
  Original: hÃƒÂ¡ {0} dias -> hÃ¡ {0} dias
  Final: hÃ¡ {0} dias
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleDaysAgo_Paucal`
  Original: hÃƒÂ¡ {0} dias -> hÃ¡ {0} dias
  Final: hÃ¡ {0} dias
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleDaysAgo_Plural`
  Original: hÃƒÂ¡ {0} dias -> hÃ¡ {0} dias
  Final: hÃ¡ {0} dias
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleDaysAgo_Singular`
  Original: hÃƒÂ¡ {0} dia -> hÃ¡ {0} dia
  Final: hÃ¡ {0} dia
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleHoursAgo`
  Original: hÃƒÂ¡ {0} horas -> hÃ¡ {0} horas
  Final: hÃ¡ {0} horas
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleHoursAgo_Dual`
  Original: hÃƒÂ¡ {0} horas -> hÃ¡ {0} horas
  Final: hÃ¡ {0} horas
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleHoursAgo_Paucal`
  Original: hÃƒÂ¡ {0} horas -> hÃ¡ {0} horas
  Final: hÃ¡ {0} horas
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleHoursAgo_Plural`
  Original: hÃƒÂ¡ {0} horas -> hÃ¡ {0} horas
  Final: hÃ¡ {0} horas
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleHoursAgo_Singular`
  Original: hÃƒÂ¡ {0} hora -> hÃ¡ {0} hora
  Final: hÃ¡ {0} hora
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleMinutesAgo`
  Original: hÃƒÂ¡ {0} minutos -> hÃ¡ {0} minutos
  Final: hÃ¡ {0} minutos
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleMinutesAgo_Dual`
  Original: hÃƒÂ¡ {0} minutos -> hÃ¡ {0} minutos
  Final: hÃ¡ {0} minutos
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleMinutesAgo_Paucal`
  Original: hÃƒÂ¡ {0} minutos -> hÃ¡ {0} minutos
  Final: hÃ¡ {0} minutos
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleMinutesAgo_Plural`
  Original: hÃƒÂ¡ {0} minutos -> hÃ¡ {0} minutos
  Final: hÃ¡ {0} minutos
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleMinutesAgo_Singular`
  Original: hÃƒÂ¡ {0} minuto -> hÃ¡ {0} minuto
  Final: hÃ¡ {0} minuto
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleMonthsAgo`
  Original: hÃƒÂ¡ {0} meses -> hÃ¡ {0} meses
  Final: hÃ¡ {0} meses
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleMonthsAgo_Dual`
  Original: hÃƒÂ¡ {0} meses -> hÃ¡ {0} meses
  Final: hÃ¡ {0} meses
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleMonthsAgo_Paucal`
  Original: hÃƒÂ¡ {0} meses -> hÃ¡ {0} meses
  Final: hÃ¡ {0} meses
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleMonthsAgo_Plural`
  Original: hÃƒÂ¡ {0} meses -> hÃ¡ {0} meses
  Final: hÃ¡ {0} meses
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleMonthsAgo_Singular`
  Original: hÃƒÂ¡ {0} mÃƒÂªs -> hÃ¡ {0} mÃªs
  Final: hÃ¡ {0} mÃªs
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleMonthsFromNow_Singular`
  Original: daqui a {0} mÃƒÂªs -> daqui a {0} mÃªs
  Final: daqui a {0} mÃªs
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleSecondsAgo`
  Original: hÃƒÂ¡ {0} segundos -> hÃ¡ {0} segundos
  Final: hÃ¡ {0} segundos
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleSecondsAgo_Dual`
  Original: hÃƒÂ¡ {0} segundos -> hÃ¡ {0} segundos
  Final: hÃ¡ {0} segundos
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleSecondsAgo_Paucal`
  Original: hÃƒÂ¡ {0} segundos -> hÃ¡ {0} segundos
  Final: hÃ¡ {0} segundos
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleSecondsAgo_Plural`
  Original: hÃƒÂ¡ {0} segundos -> hÃ¡ {0} segundos
  Final: hÃ¡ {0} segundos
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleSecondsAgo_Singular`
  Original: hÃƒÂ¡ {0} segundo -> hÃ¡ {0} segundo
  Final: hÃ¡ {0} segundo
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleYearsAgo`
  Original: hÃƒÂ¡ {0} anos -> hÃ¡ {0} anos
  Final: hÃ¡ {0} anos
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleYearsAgo_Dual`
  Original: hÃƒÂ¡ {0} anos -> hÃ¡ {0} anos
  Final: hÃ¡ {0} anos
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleYearsAgo_Paucal`
  Original: hÃƒÂ¡ {0} anos -> hÃ¡ {0} anos
  Final: hÃ¡ {0} anos
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleYearsAgo_Plural`
  Original: hÃƒÂ¡ {0} anos -> hÃ¡ {0} anos
  Final: hÃ¡ {0} anos
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_MultipleYearsAgo_Singular`
  Original: hÃƒÂ¡ {0} ano -> hÃ¡ {0} ano
  Final: hÃ¡ {0} ano
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_SingleDayFromNow`
  Original: amanhÃƒÂ£ -> amanhÃ£
  Final: amanhÃ£
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_SingleHourAgo`
  Original: hÃƒÂ¡ uma hora -> hÃ¡ uma hora
  Final: hÃ¡ uma hora
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_SingleMinuteAgo`
  Original: hÃƒÂ¡ um minuto -> hÃ¡ um minuto
  Final: hÃ¡ um minuto
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_SingleMonthAgo`
  Original: hÃƒÂ¡ um mÃƒÂªs -> hÃ¡ um mÃªs
  Final: hÃ¡ um mÃªs
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_SingleMonthFromNow`
  Original: daqui a um mÃƒÂªs -> daqui a um mÃªs
  Final: daqui a um mÃªs
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_SingleSecondAgo`
  Original: hÃƒÂ¡ um segundo -> hÃ¡ um segundo
  Final: hÃ¡ um segundo
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_SingleYearAgo`
  Original: hÃƒÂ¡ um ano -> hÃ¡ um ano
  Final: hÃ¡ um ano
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `DateHumanize_TwoDaysFromNow`
  Original: depois de amanhÃƒÂ£ -> depois de amanhÃ£
  Final: depois de amanhÃ£
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/DateHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `TimeSpanHumanize_MultipleMonths_Singular`
  Original: {0} mÃƒÂªs -> {0} mÃªs
  Final: {0} mÃªs
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `TimeSpanHumanize_SingleMonth`
  Original: 1 mÃƒÂªs -> 1 mÃªs
  Final: 1 mÃªs
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `TimeSpanHumanize_SingleMonth_Words`
  Original: um mÃƒÂªs -> um mÃªs
  Final: um mÃªs
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [pt] `TimeSpanHumanize_Zero`
  Original: sem horÃƒÂ¡rio -> sem horÃ¡rio
  Final: sem horÃ¡rio
  Decision: rejected
  Rationale: Rejeito o achado: nÃ£o hÃ¡ defeito de traduÃ§Ã£o no locale pt. O valor canÃ³nico em Resources.pt.resx jÃ¡ estÃ¡ com acentuaÃ§Ã£o correta; as sequÃªncias com "Ãƒ" refletem mojibake no artefacto de revisÃ£o da primeira passada.
  Evidence: src/Humanizer/Properties/Resources.pt.resx (valor da chave com acentuaÃ§Ã£o portuguesa correta).; tests/Humanizer.Tests/Localisation/pt/TimeSpanHumanizeTests.cs (asserÃ§Ãµes esperadas com acentuaÃ§Ã£o correta).
  Notes: Segunda passada independente: falso positivo de codificaÃ§Ã£o no ficheiro de revisÃ£o; nenhuma alteraÃ§Ã£o linguÃ­stica Ã© necessÃ¡ria no recurso pt.
- [sr] `DateHumanize_SingleMinuteFromNow`
  Original: за минут -> за минуту
  Final: за минут
  Decision: rejected
  Rationale: Облик "за минут" је природан и граматички исправан у српском јер је "минут" мушког рода; у акузативу једнине (неживо) облик остаје "минут". Предложено "за минуту" намеће други лексички образац.
  Evidence: src/Humanizer/Properties/Resources.sr.resx:210; src/Humanizer/Properties/Resources.sr.resx:288; tests/Humanizer.Tests/Localisation/sr/DateHumanizeTests.cs:26
  Notes: No change required.
- [tr] `DateHumanize_TwoDaysFromNow`
  Original: yarından sonra -> öbür gün
  Final: yarından sonra
  Decision: rejected
  Rationale: İkinci bağımsız ana dil değerlendirmesinde "yarından sonra" ifadesi Türkçede "two days from now" anlamını doğal ve açık biçimde karşılıyor. Önerilen "öbür gün" de mümkündür; ancak daha konuşma dili/bağlama açık kalabildiği için mevcut çeviriyi zorunlu olarak değiştirmeyi gerektiren bir kusur yoktur.
  Evidence: src/Humanizer/Properties/Resources.tr.resx: DateHumanize_TwoDaysFromNow = 'yarından sonra'; tests/Humanizer.Tests/Localisation/tr/DateHumanizeTests.cs: HasExplicitResidualDateResources asserts 'yarından sonra'
  Notes: No change required.
- [uk] `DataUnit_BitSymbol`
  Original: б -> b
  Final: б
  Decision: rejected
  Rationale: Для української локалізації скорочення «б» для біта є прийнятним і не створює неоднозначності з байтом («B»), тож обов’язкової заміни на латинське «b» не потрібно.
  Evidence: src/Humanizer/Properties/Resources.uk.resx:123; src/Humanizer/Properties/Resources.ru.resx:123
  Notes: No change required.
- [uk] `ENE`
  Original: схід-північний схід -> східно-північний схід
  Final: схід-північний схід
  Decision: rejected
  Rationale: Форма «схід-північний схід» є природною українською назвою румба ENE; заміна на «східно-північний схід» не є обов’язковою нормою.
  Evidence: src/Humanizer/Properties/Resources.uk.resx:636; tests/Humanizer.Tests/Localisation/uk-UA/HeadingTests.cs:30
  Notes: No change required.
- [uk] `ESE`
  Original: схід-південний схід -> східно-південний схід
  Final: схід-південний схід
  Decision: rejected
  Rationale: Форма «схід-південний схід» є прийнятною українською назвою румба ESE; підстав вважати чинний варіант дефектним немає.
  Evidence: src/Humanizer/Properties/Resources.uk.resx:648; tests/Humanizer.Tests/Localisation/uk-UA/HeadingTests.cs:32
  Notes: No change required.
- [uk] `NNE`
  Original: північ-північний схід -> північно-північний схід
  Final: північ-північний схід
  Decision: rejected
  Rationale: Форма «північ-північний схід» є усталеною для румба NNE в українському вжитку; варіант із «північно-» не має безумовної переваги.
  Evidence: src/Humanizer/Properties/Resources.uk.resx:624; tests/Humanizer.Tests/Localisation/uk-UA/HeadingTests.cs:28
  Notes: No change required.
- [uk] `NNW`
  Original: північ-північний захід -> північно-північний захід
  Final: північ-північний захід
  Decision: rejected
  Rationale: Форма «північ-північний захід» є природною назвою румба NNW українською; запропонована заміна не є необхідною.
  Evidence: src/Humanizer/Properties/Resources.uk.resx:708; tests/Humanizer.Tests/Localisation/uk-UA/HeadingTests.cs:42
  Notes: No change required.
- [uk] `SSE`
  Original: південь-південний схід -> південно-південний схід
  Final: південь-південний схід
  Decision: rejected
  Rationale: Форма «південь-південний схід» є коректною назвою румба SSE в цій номенклатурі; варіант із «південно-» не є обов’язковим.
  Evidence: src/Humanizer/Properties/Resources.uk.resx:660; tests/Humanizer.Tests/Localisation/uk-UA/HeadingTests.cs:34
  Notes: No change required.
- [uk] `SSW`
  Original: південь-південний захід -> південно-південний захід
  Final: південь-південний захід
  Decision: rejected
  Rationale: Форма «південь-південний захід» є нормативно прийнятною назвою румба SSW; підстав для обов’язкової зміни немає.
  Evidence: src/Humanizer/Properties/Resources.uk.resx:672; tests/Humanizer.Tests/Localisation/uk-UA/HeadingTests.cs:36
  Notes: No change required.
- [uk] `WNW`
  Original: захід-північний захід -> західно-північний захід
  Final: захід-північний захід
  Decision: rejected
  Rationale: Форма «захід-північний захід» є природною назвою румба WNW у чинній українській традиції позначень.
  Evidence: src/Humanizer/Properties/Resources.uk.resx:696; tests/Humanizer.Tests/Localisation/uk-UA/HeadingTests.cs:40
  Notes: No change required.
- [uk] `WSW`
  Original: захід-південний захід -> західно-південний захід
  Final: захід-південний захід
  Decision: rejected
  Rationale: Форма «захід-південний захід» є прийнятною назвою румба WSW; підстав для заміни на «західно-південний» недостатньо.
  Evidence: src/Humanizer/Properties/Resources.uk.resx:684; tests/Humanizer.Tests/Localisation/uk-UA/HeadingTests.cs:38
  Notes: No change required.

### Confirmed
- [af] `TimeSpanHumanize_SingleMillisecond`
  Original: 1 millisekond -> 1 millisekonde
  Final: 1 millisekonde
  Decision: confirmed
  Rationale: As onafhanklike tweede moedertaalbeoordelaar bevestig ek die bevinding: die standaard enkelvoud is "millisekonde". "Millisekond" is nie idiomatiese of ortografies standaard Afrikaans nie.
  Evidence: src/Humanizer/Properties/Resources.af.resx: DateHumanize_SingleMillisecondAgo en DateHumanize_SingleMillisecondFromNow gebruik reeds "1 millisekonde".; src/Humanizer/Properties/Resources.af.resx: TimeSpanHumanize_MultipleMilliseconds gebruik "{0} millisekondes", wat die stam "millisekonde" bevestig.
  Notes: Tweede-pass onafhanklike moedertaal-adjudikasie: eerste-pass voorstel onveranderd bevestig.
- [af] `TimeSpanHumanize_SingleMillisecond_Words`
  Original: een millisekond -> een millisekonde
  Final: een millisekonde
  Decision: confirmed
  Rationale: Hierdie woordvorm moet dieselfde korrekte enkelvoud gebruik as die syfervorm. "Een millisekonde" is die standaard Afrikaans; "een millisekond" is afwykend.
  Evidence: src/Humanizer/Properties/Resources.af.resx: DateHumanize_SingleMillisecondAgo gebruik "1 millisekonde terug".; src/Humanizer/Properties/Resources.af.resx: TimeSpanHumanize_MultipleMilliseconds gebruik "{0} millisekondes", dus stem "millisekonde" morfologies ooreen.
  Notes: Tweede-pass onafhanklike moedertaal-adjudikasie: eerste-pass voorstel onveranderd bevestig.
- [af] `TimeSpanHumanize_SingleSecond`
  Original: 1 sekond -> 1 sekonde
  Final: 1 sekonde
  Decision: confirmed
  Rationale: Ek bevestig die bevinding: in standaard geskrewe Afrikaans is die enkelvoud "sekonde". "Sekond" is nie die korrekte normvorm in hierdie konteks nie.
  Evidence: src/Humanizer/Properties/Resources.af.resx: DateHumanize_SingleSecondAgo en DateHumanize_SingleSecondFromNow gebruik albei "1 sekonde".; tests/Humanizer.Tests/Localisation/af/DateHumanizeTests.cs: verwagte enkelvoud vir sekondes is "1 sekonde terug" en "oor 1 sekonde".
  Notes: Tweede-pass onafhanklike moedertaal-adjudikasie: eerste-pass voorstel onveranderd bevestig.
- [af] `TimeSpanHumanize_SingleSecond_Words`
  Original: een sekond -> een sekonde
  Final: een sekonde
  Decision: confirmed
  Rationale: Die woordvorm moet die standaard enkelvoud behou: "sekonde". "Een sekond" is nie natuurlike standaard Afrikaans vir hierdie gebruik nie.
  Evidence: src/Humanizer/Properties/Resources.af.resx: TimeSpanHumanize_MultipleSeconds gebruik "{0} sekondes", wat by "sekonde" as enkelvoud aansluit.; src/Humanizer/Properties/Resources.af.resx en tests/Humanizer.Tests/Localisation/af/DateHumanizeTests.cs: enkelvoudige DateHumanize-vorme gebruik konsekwent "1 sekonde".
  Notes: Tweede-pass onafhanklike moedertaal-adjudikasie: eerste-pass voorstel onveranderd bevestig.
- [ar] `TimeSpanHumanize_MultipleMilliseconds`
  Original: {0} Ã˜Â¬Ã˜Â²Ã˜Â¡ Ã™â€¦Ã™â€  Ã˜Â§Ã™â€žÃ˜Â«Ã˜Â§Ã™â€ Ã™Å Ã˜Â© -> {0} Ù…Ù„Ù„ÙŠ Ø«Ø§Ù†ÙŠØ©
  Final: {0} Ù…Ù„Ù„ÙŠ Ø«Ø§Ù†ÙŠØ©
  Decision: confirmed
  Rationale: The current phrase means an unspecified fraction of a second, not the millisecond unit. The proposed technical unit term is correct.
  Evidence: src/Humanizer/Properties/Resources.ar.resx: TimeSpanHumanize_MultipleMilliseconds; src/Humanizer/Properties/Resources.ar.resx: TimeUnit_Millisecond
  Notes: Second-pass confirmed by independent native review.
- [ar] `TimeSpanHumanize_MultipleMilliseconds_Dual`
  Original: Ã˜Â¬Ã˜Â²Ã˜Â¦Ã™Å Ã™â€  Ã™â€¦Ã™â€  Ã˜Â§Ã™â€žÃ˜Â«Ã˜Â§Ã™â€ Ã™Å Ã˜Â© -> Ù…Ù„Ù„ÙŠ Ø«Ø§Ù†ÙŠØªÙŠÙ†
  Final: Ù…Ù„Ù„ÙŠ Ø«Ø§Ù†ÙŠØªÙŠÙ†
  Decision: confirmed
  Rationale: The existing dual form uses a generic "two parts of a second" expression and does not represent milliseconds accurately. The proposed dual unit form is acceptable.
  Evidence: src/Humanizer/Properties/Resources.ar.resx: TimeSpanHumanize_MultipleMilliseconds_Dual; tests/Humanizer.Tests/Localisation/ar/TimeSpanHumanizeTests.cs: Milliseconds
  Notes: Second-pass confirmed by independent native review.
- [ar] `TimeSpanHumanize_MultipleMilliseconds_Plural`
  Original: {0} Ã˜Â£Ã˜Â¬Ã˜Â²Ã˜Â§Ã˜Â¡ Ã™â€¦Ã™â€  Ã˜Â§Ã™â€žÃ˜Â«Ã˜Â§Ã™â€ Ã™Å Ã˜Â© -> {0} Ù…Ù„Ù„ÙŠ Ø«Ø§Ù†ÙŠØ©
  Final: {0} Ù…Ù„Ù„ÙŠ Ø«Ø§Ù†ÙŠØ©
  Decision: confirmed
  Rationale: The current plural phrase is non-terminological and can be interpreted as generic fractions. Using the explicit millisecond unit is the correct localization.
  Evidence: src/Humanizer/Properties/Resources.ar.resx: TimeSpanHumanize_MultipleMilliseconds_Plural; src/Humanizer/Properties/Resources.ar.resx: TimeUnit_Millisecond
  Notes: Second-pass confirmed by independent native review.
- [ar] `TimeSpanHumanize_MultipleMonths`
  Original: {0} Ã˜Â£Ã˜Â´Ã™â€¡Ã˜Â± -> {0} Ø´Ù‡Ø±
  Final: {0} Ø´Ù‡Ø±
  Decision: confirmed
  Rationale: For the many-number branch (e.g., 11), Arabic requires the singular unit form. Keeping "{0} أشهر" here causes incorrect output for that branch.
  Evidence: src/Humanizer/Properties/Resources.ar.resx: TimeSpanHumanize_MultipleMonths; tests/Humanizer.Tests/Localisation/ar/TimeSpanHumanizeTests.cs: Months(335)
  Notes: Second-pass confirmed by independent native review.
- [ar] `TimeSpanHumanize_MultipleYears_Plural`
  Original: {0} Ã˜Â³Ã™â€ Ã˜Â© -> {0} Ø³Ù†ÙˆØ§Øª
  Final: {0} Ø³Ù†ÙˆØ§Øª
  Decision: confirmed
  Rationale: The plural-specific key must carry a true plural noun form. The current value duplicates singular behavior and loses grammatical contrast.
  Evidence: src/Humanizer/Properties/Resources.ar.resx: TimeSpanHumanize_MultipleYears_Plural; src/Humanizer/Properties/Resources.ar.resx: DateHumanize_MultipleYearsFromNow_Plural
  Notes: Second-pass confirmed by independent native review.
- [ar] `TimeSpanHumanize_SingleMonth`
  Original: Ã˜Â´Ã™â€¡Ã˜Â± 1 -> Ø´Ù‡Ø± ÙˆØ§Ø­Ø¯
  Final: Ø´Ù‡Ø± ÙˆØ§Ø­Ø¯
  Decision: confirmed
  Rationale: The existing value is a machine-ordered numeral phrase and is not natural Arabic. The proposed phrase is the correct native singular expression.
  Evidence: src/Humanizer/Properties/Resources.ar.resx: TimeSpanHumanize_SingleMonth; tests/Humanizer.Tests/Localisation/ar/TimeSpanHumanizeTests.cs: Months(31)
  Notes: Second-pass confirmed by independent native review.
- [ar] `TimeSpanHumanize_SingleYear`
  Original: Ã˜Â§Ã™â€žÃ˜Â³Ã™â€ Ã˜Â© 1 -> Ø³Ù†Ø© ÙˆØ§Ø­Ø¯Ø©
  Final: Ø³Ù†Ø© ÙˆØ§Ø­Ø¯Ø©
  Decision: confirmed
  Rationale: The current form is not idiomatic Arabic for a singular duration. The proposed wording is the correct native singular year expression.
  Evidence: src/Humanizer/Properties/Resources.ar.resx: TimeSpanHumanize_SingleYear; tests/Humanizer.Tests/Localisation/ar/TimeSpanHumanizeTests.cs: Years(366)
  Notes: Second-pass confirmed by independent native review.
- [az] `TimeSpanHumanize_Zero`
  Original: zaman fÉ™rqi yoxdur -> vaxt yoxdur
  Final: vaxt yoxdur
  Decision: confirmed
  Rationale: "zaman fərqi yoxdur" daha çox iki zaman nöqtəsinin müqayisəsi kimi səslənir. TimeSpan.Zero üçün daha təbii və birbaşa qarşılıq "vaxt yoxdur"dur.
  Evidence: src/Humanizer/Properties/Resources.az.resx:244 -> <value>zaman fərqi yoxdur</value>; tests/Humanizer.Tests/Localisation/az/TimeSpanHumanizeTests.cs:104 -> Assert.Equal("zaman fərqi yoxdur", actual);
  Notes: Birinci keçid təklifi saxlanıldı; məna dəqiqliyi üçün dəyişiklik əsaslıdır.
- [bn] `TimeSpanHumanize_SingleMonth`
  Original: à¦à¦• à¦®à¦¾à¦¸à§‡à¦° -> à¦à¦• à¦®à¦¾à¦¸
  Final: à¦à¦• à¦®à¦¾à¦¸
  Decision: confirmed
  Rationale: à¦¦à§à¦¬à¦¿à¦¤à§€à§Ÿ à¦¸à§à¦¬à¦¾à¦§à§€à¦¨ à¦®à¦¾à¦¤à§ƒà¦­à¦¾à¦·à¦¿à¦• à¦ªà¦°à§à¦¯à¦¾à¦²à§‹à¦šà¦¨à¦¾à§Ÿ à¦ªà§à¦°à¦¥à¦®-à¦ªà¦¾à¦¸ à¦¸à¦¿à¦¦à§à¦§à¦¾à¦¨à§à¦¤ à¦¨à¦¿à¦¶à§à¦šà¦¿à¦¤ à¦•à¦°à¦¾ à¦¹à¦²à§‹à¥¤ Bengali-à¦¤à§‡ à¦à¦•à¦• à¦¸à¦®à§Ÿà¦•à¦¾à¦² à¦¬à§‹à¦à¦¾à¦¤à§‡ à¦¸à§à¦¬à¦¾à¦­à¦¾à¦¬à¦¿à¦• à¦°à§‚à¦ª â€˜à¦à¦• à¦®à¦¾à¦¸â€™; â€˜à¦à¦• à¦®à¦¾à¦¸à§‡à¦°â€™ genitive à¦¹à¦“à§Ÿà¦¾à§Ÿ à¦à¦Ÿà¦¿ à¦à¦•à¦¾ à¦¦à¦¾à¦à§œà¦¾à¦²à§‡ à¦…à¦¸à¦®à§à¦ªà§‚à¦°à§à¦£ à¦¶à§‹à¦¨à¦¾à§Ÿà¥¤
  Evidence: src/Humanizer/Properties/Resources.bn.resx: TimeSpanHumanize_SingleMonth à¦¬à¦°à§à¦¤à¦®à¦¾à¦¨à§‡ 'à¦à¦• à¦®à¦¾à¦¸à§‡à¦°', à¦¯à¦¾ TimeSpanHumanize_SingleDay/SingleWeek/SingleYear-à¦à¦° noun-phrase pattern-à¦à¦° à¦¸à¦™à§à¦—à§‡ à¦…à¦¸à¦¾à¦®à¦žà§à¦œà¦¸à§à¦¯à¦ªà§‚à¦°à§à¦£à¥¤; tests/Humanizer.Tests/Localisation/bn-BD/TimeSpanHumanizeTests.cs: Months test-à¦ [Trait("Translation", "Google")] à¦†à¦›à§‡ à¦à¦¬à¦‚ 31 days case-à¦ à¦¬à¦°à§à¦¤à¦®à¦¾à¦¨ output 'à¦à¦• à¦®à¦¾à¦¸à§‡à¦°' à¦ªà§à¦°à¦¤à§à¦¯à¦¾à¦¶à¦¿à¦¤, à¦¯à¦¾ à¦…à¦¨à§à¦¬à¦¾à¦¦-à¦‰à§Žà¦ªà¦¤à§à¦¤à¦¿ à¦¤à§à¦°à§à¦Ÿà¦¿à¦° à¦à§à¦à¦•à¦¿ à¦¨à¦¿à¦°à§à¦¦à§‡à¦¶ à¦•à¦°à§‡à¥¤; Native usage check: standalone duration à¦¹à¦¿à¦¸à§‡à¦¬à§‡ â€˜à¦à¦• à¦®à¦¾à¦¸â€™ à¦ªà§à¦°à¦®à¦¿à¦¤ à¦“ à¦ªà§à¦°à¦¾à¦•à§ƒà¦¤à¦¿à¦•; â€˜à¦à¦• à¦®à¦¾à¦¸à§‡à¦°â€™ à¦¸à¦¾à¦§à¦¾à¦°à¦£à¦¤ à¦ªà¦°à¦¬à¦°à§à¦¤à§€ noun à¦šà¦¾à§Ÿ (à¦¯à§‡à¦®à¦¨ â€˜à¦à¦• à¦®à¦¾à¦¸à§‡à¦° à¦›à§à¦Ÿà¦¿â€™)à¥¤
  Notes: à¦¦à§à¦¬à¦¿à¦¤à§€à§Ÿ-à¦ªà¦¾à¦¸ à¦°à¦¾à§Ÿ: first-pass à¦ªà§à¦°à¦¸à§à¦¤à¦¾à¦¬ à¦…à¦ªà¦°à¦¿à¦¬à¦°à§à¦¤à¦¿à¦¤à¦­à¦¾à¦¬à§‡ à¦¨à¦¿à¦¶à§à¦šà¦¿à¦¤à¥¤
- [bn] `TimeSpanHumanize_Zero`
  Original: à¦¶à§‚à¦¨à§à¦¯ à¦¸à¦®à§Ÿ -> à¦•à§‹à¦¨à§‹ à¦¸à¦®à§Ÿ à¦¨à§‡à¦‡
  Final: à¦•à§‹à¦¨à§‹ à¦¸à¦®à§Ÿ à¦¨à§‡à¦‡
  Decision: confirmed
  Rationale: à¦¦à§à¦¬à¦¿à¦¤à§€à§Ÿ à¦¸à§à¦¬à¦¾à¦§à§€à¦¨ à¦®à¦¾à¦¤à§ƒà¦­à¦¾à¦·à¦¿à¦• à¦¯à¦¾à¦šà¦¾à¦‡à§Ÿà§‡ à¦à¦Ÿà¦¿à¦“ à¦¨à¦¿à¦¶à§à¦šà¦¿à¦¤à¥¤ â€˜à¦¶à§‚à¦¨à§à¦¯ à¦¸à¦®à§Ÿâ€™ à¦¬à¦¾à¦‚à¦²à¦¾ à¦­à¦¾à¦·à¦¾à§Ÿ calque-à¦œà¦¾à¦¤à§€à§Ÿ à¦“ à¦…à¦¨à§ˆà¦¡à¦¿à¦“à¦®à§à¦¯à¦¾à¦Ÿà¦¿à¦•; â€˜no timeâ€™ à¦à¦° à¦ªà§à¦°à¦¾à¦•à§ƒà¦¤à¦¿à¦• à¦ªà§à¦°à¦•à¦¾à¦¶ â€˜à¦•à§‹à¦¨à§‹ à¦¸à¦®à§Ÿ à¦¨à§‡à¦‡â€™à¥¤
  Evidence: src/Humanizer/Properties/Resources.bn.resx: TimeSpanHumanize_Zero-à¦à¦° à¦¬à¦°à§à¦¤à¦®à¦¾à¦¨ à¦®à¦¾à¦¨ â€˜à¦¶à§‚à¦¨à§à¦¯ à¦¸à¦®à§Ÿâ€™; à¦à¦•à¦‡ à¦«à¦¾à¦‡à¦²à§‡ à¦…à¦¨à§à¦¯à¦¾à¦¨à§à¦¯ à¦®à¦¾à¦¨à¦—à§à¦²à§‹à¦“ à¦ªà§à¦°à¦¾à¦•à§ƒà¦¤à¦¿à¦• à¦•à¦¥à§à¦¯/à¦²à¦¿à¦–à¦¿à¦¤ à¦°à§‚à¦ªà§‡ à¦°à§Ÿà§‡à¦›à§‡, à¦«à¦²à§‡ à¦à¦Ÿà¦¿ à¦¶à§ˆà¦²à§€à¦¤à§‡ à¦¬à¦¿à¦šà§à¦›à¦¿à¦¨à§à¦¨à¥¤; tests/Humanizer.Tests/Localisation/bn-BD/TimeSpanHumanizeTests.cs: NoTimeToWords test-à¦ à¦¬à¦¿à¦¦à§à¦¯à¦®à¦¾à¦¨ à¦®à¦¨à§à¦¤à¦¬à§à¦¯ ('doesn't make a lot of sense') à¦¬à¦°à§à¦¤à¦®à¦¾à¦¨ phrasing-à¦à¦° à¦…à¦ªà§à¦°à¦¾à¦•à§ƒà¦¤à¦¿à¦•à¦¤à¦¾ corroborate à¦•à¦°à§‡à¥¤; Native usage check: à¦¸à¦®à§Ÿà§‡à¦° à¦…à¦¨à§à¦ªà¦¸à§à¦¥à¦¿à¦¤à¦¿ à¦¬à¦¾à¦‚à¦²à¦¾à§Ÿ à¦¸à¦¾à¦§à¦¾à¦°à¦£à¦¤ â€˜...à¦¨à§‡à¦‡â€™ à¦—à¦ à¦¨à§‡ à¦ªà§à¦°à¦•à¦¾à¦¶ à¦ªà¦¾à§Ÿ; â€˜à¦•à§‹à¦¨à§‹ à¦¸à¦®à§Ÿ à¦¨à§‡à¦‡â€™ à¦¸à§‡à¦‡ idiomatic formà¥¤
  Notes: à¦¦à§à¦¬à¦¿à¦¤à§€à§Ÿ-à¦ªà¦¾à¦¸ à¦°à¦¾à§Ÿ: first-pass à¦ªà§à¦°à¦¸à§à¦¤à¦¾à¦¬ à¦…à¦ªà¦°à¦¿à¦¬à¦°à§à¦¤à¦¿à¦¤à¦­à¦¾à¦¬à§‡ à¦¨à¦¿à¦¶à§à¦šà¦¿à¦¤à¥¤
- [ca] `DateHumanize_MultipleDaysFromNow_Dual`
  Original: {0} dies a partir d'ara -> d'aquÃ­ {0} dies
  Final: d'aquÃ­ {0} dies
  Decision: confirmed
  Rationale: Com a parlant nadiu de catalÃ , confirmo que la forma actual Ã©s correcta perÃ² menys idiomÃ tica i menys coherent internament que el patrÃ³ Â«d'aquÃ­ {0} ...Â» present a la localitzaciÃ³.
  Evidence: src/Humanizer/Properties/Resources.ca.resx: les formes de futur base i paucal d'aquests mateixos grups fan servir Â«d'aquÃ­ {0} ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament resultats de la forma Â«d'aquÃ­ N ...Â» per segons/minuts/dies/mesos/anys.
  Notes: Segona passada independent: proposta confirmada sense canvis.
- [ca] `DateHumanize_MultipleDaysFromNow_Plural`
  Original: {0} dies a partir d'ara -> d'aquÃ­ {0} dies
  Final: d'aquÃ­ {0} dies
  Decision: confirmed
  Rationale: Com a parlant nadiu de catalÃ , confirmo que la forma actual Ã©s correcta perÃ² menys idiomÃ tica i menys coherent internament que el patrÃ³ Â«d'aquÃ­ {0} ...Â» present a la localitzaciÃ³.
  Evidence: src/Humanizer/Properties/Resources.ca.resx: les formes de futur base i paucal d'aquests mateixos grups fan servir Â«d'aquÃ­ {0} ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament resultats de la forma Â«d'aquÃ­ N ...Â» per segons/minuts/dies/mesos/anys.
  Notes: Segona passada independent: proposta confirmada sense canvis.
- [ca] `DateHumanize_MultipleDaysFromNow_Singular`
  Original: {0} dia a partir d'ara -> d'aquÃ­ {0} dia
  Final: d'aquÃ­ {0} dia
  Decision: confirmed
  Rationale: Com a parlant nadiu de catalÃ , confirmo que la forma actual Ã©s correcta perÃ² menys idiomÃ tica i menys coherent internament que el patrÃ³ Â«d'aquÃ­ {0} ...Â» present a la localitzaciÃ³.
  Evidence: src/Humanizer/Properties/Resources.ca.resx: les formes de futur base i paucal d'aquests mateixos grups fan servir Â«d'aquÃ­ {0} ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament resultats de la forma Â«d'aquÃ­ N ...Â» per segons/minuts/dies/mesos/anys.
  Notes: Segona passada independent: proposta confirmada sense canvis.
- [ca] `DateHumanize_MultipleMinutesFromNow_Dual`
  Original: {0} minuts a partir d'ara -> d'aquÃ­ {0} minuts
  Final: d'aquÃ­ {0} minuts
  Decision: confirmed
  Rationale: Com a parlant nadiu de catalÃ , confirmo que la forma actual Ã©s correcta perÃ² menys idiomÃ tica i menys coherent internament que el patrÃ³ Â«d'aquÃ­ {0} ...Â» present a la localitzaciÃ³.
  Evidence: src/Humanizer/Properties/Resources.ca.resx: les formes de futur base i paucal d'aquests mateixos grups fan servir Â«d'aquÃ­ {0} ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament resultats de la forma Â«d'aquÃ­ N ...Â» per segons/minuts/dies/mesos/anys.
  Notes: Segona passada independent: proposta confirmada sense canvis.
- [ca] `DateHumanize_MultipleMinutesFromNow_Plural`
  Original: {0} minuts a partir d'ara -> d'aquÃ­ {0} minuts
  Final: d'aquÃ­ {0} minuts
  Decision: confirmed
  Rationale: Com a parlant nadiu de catalÃ , confirmo que la forma actual Ã©s correcta perÃ² menys idiomÃ tica i menys coherent internament que el patrÃ³ Â«d'aquÃ­ {0} ...Â» present a la localitzaciÃ³.
  Evidence: src/Humanizer/Properties/Resources.ca.resx: les formes de futur base i paucal d'aquests mateixos grups fan servir Â«d'aquÃ­ {0} ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament resultats de la forma Â«d'aquÃ­ N ...Â» per segons/minuts/dies/mesos/anys.
  Notes: Segona passada independent: proposta confirmada sense canvis.
- [ca] `DateHumanize_MultipleMinutesFromNow_Singular`
  Original: {0} minut a partir d'ara -> d'aquÃ­ {0} minut
  Final: d'aquÃ­ {0} minut
  Decision: confirmed
  Rationale: Com a parlant nadiu de catalÃ , confirmo que la forma actual Ã©s correcta perÃ² menys idiomÃ tica i menys coherent internament que el patrÃ³ Â«d'aquÃ­ {0} ...Â» present a la localitzaciÃ³.
  Evidence: src/Humanizer/Properties/Resources.ca.resx: les formes de futur base i paucal d'aquests mateixos grups fan servir Â«d'aquÃ­ {0} ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament resultats de la forma Â«d'aquÃ­ N ...Â» per segons/minuts/dies/mesos/anys.
  Notes: Segona passada independent: proposta confirmada sense canvis.
- [ca] `DateHumanize_MultipleMonthsFromNow_Dual`
  Original: {0} mesos a partir d'ara -> d'aquÃ­ {0} mesos
  Final: d'aquÃ­ {0} mesos
  Decision: confirmed
  Rationale: Com a parlant nadiu de catalÃ , confirmo que la forma actual Ã©s correcta perÃ² menys idiomÃ tica i menys coherent internament que el patrÃ³ Â«d'aquÃ­ {0} ...Â» present a la localitzaciÃ³.
  Evidence: src/Humanizer/Properties/Resources.ca.resx: les formes de futur base i paucal d'aquests mateixos grups fan servir Â«d'aquÃ­ {0} ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament resultats de la forma Â«d'aquÃ­ N ...Â» per segons/minuts/dies/mesos/anys.
  Notes: Segona passada independent: proposta confirmada sense canvis.
- [ca] `DateHumanize_MultipleMonthsFromNow_Plural`
  Original: {0} mesos a partir d'ara -> d'aquÃ­ {0} mesos
  Final: d'aquÃ­ {0} mesos
  Decision: confirmed
  Rationale: Com a parlant nadiu de catalÃ , confirmo que la forma actual Ã©s correcta perÃ² menys idiomÃ tica i menys coherent internament que el patrÃ³ Â«d'aquÃ­ {0} ...Â» present a la localitzaciÃ³.
  Evidence: src/Humanizer/Properties/Resources.ca.resx: les formes de futur base i paucal d'aquests mateixos grups fan servir Â«d'aquÃ­ {0} ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament resultats de la forma Â«d'aquÃ­ N ...Â» per segons/minuts/dies/mesos/anys.
  Notes: Segona passada independent: proposta confirmada sense canvis.
- [ca] `DateHumanize_MultipleMonthsFromNow_Singular`
  Original: {0} mes a partir d'ara -> d'aquÃ­ {0} mes
  Final: d'aquÃ­ {0} mes
  Decision: confirmed
  Rationale: Com a parlant nadiu de catalÃ , confirmo que la forma actual Ã©s correcta perÃ² menys idiomÃ tica i menys coherent internament que el patrÃ³ Â«d'aquÃ­ {0} ...Â» present a la localitzaciÃ³.
  Evidence: src/Humanizer/Properties/Resources.ca.resx: les formes de futur base i paucal d'aquests mateixos grups fan servir Â«d'aquÃ­ {0} ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament resultats de la forma Â«d'aquÃ­ N ...Â» per segons/minuts/dies/mesos/anys.
  Notes: Segona passada independent: proposta confirmada sense canvis.
- [ca] `DateHumanize_MultipleSecondsFromNow_Dual`
  Original: {0} segons a partir d'ara -> d'aquÃ­ {0} segons
  Final: d'aquÃ­ {0} segons
  Decision: confirmed
  Rationale: Com a parlant nadiu de catalÃ , confirmo que la forma actual Ã©s correcta perÃ² menys idiomÃ tica i menys coherent internament que el patrÃ³ Â«d'aquÃ­ {0} ...Â» present a la localitzaciÃ³.
  Evidence: src/Humanizer/Properties/Resources.ca.resx: les formes de futur base i paucal d'aquests mateixos grups fan servir Â«d'aquÃ­ {0} ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament resultats de la forma Â«d'aquÃ­ N ...Â» per segons/minuts/dies/mesos/anys.
  Notes: Segona passada independent: proposta confirmada sense canvis.
- [ca] `DateHumanize_MultipleSecondsFromNow_Plural`
  Original: {0} segons a partir d'ara -> d'aquÃ­ {0} segons
  Final: d'aquÃ­ {0} segons
  Decision: confirmed
  Rationale: Com a parlant nadiu de catalÃ , confirmo que la forma actual Ã©s correcta perÃ² menys idiomÃ tica i menys coherent internament que el patrÃ³ Â«d'aquÃ­ {0} ...Â» present a la localitzaciÃ³.
  Evidence: src/Humanizer/Properties/Resources.ca.resx: les formes de futur base i paucal d'aquests mateixos grups fan servir Â«d'aquÃ­ {0} ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament resultats de la forma Â«d'aquÃ­ N ...Â» per segons/minuts/dies/mesos/anys.
  Notes: Segona passada independent: proposta confirmada sense canvis.
- [ca] `DateHumanize_MultipleSecondsFromNow_Singular`
  Original: {0} segon a partir d'ara -> d'aquÃ­ {0} segon
  Final: d'aquÃ­ {0} segon
  Decision: confirmed
  Rationale: Com a parlant nadiu de catalÃ , confirmo que la forma actual Ã©s correcta perÃ² menys idiomÃ tica i menys coherent internament que el patrÃ³ Â«d'aquÃ­ {0} ...Â» present a la localitzaciÃ³.
  Evidence: src/Humanizer/Properties/Resources.ca.resx: les formes de futur base i paucal d'aquests mateixos grups fan servir Â«d'aquÃ­ {0} ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament resultats de la forma Â«d'aquÃ­ N ...Â» per segons/minuts/dies/mesos/anys.
  Notes: Segona passada independent: proposta confirmada sense canvis.
- [ca] `DateHumanize_MultipleYearsFromNow_Dual`
  Original: {0} anys a partir d'ara -> d'aquÃ­ {0} anys
  Final: d'aquÃ­ {0} anys
  Decision: confirmed
  Rationale: Com a parlant nadiu de catalÃ , confirmo que la forma actual Ã©s correcta perÃ² menys idiomÃ tica i menys coherent internament que el patrÃ³ Â«d'aquÃ­ {0} ...Â» present a la localitzaciÃ³.
  Evidence: src/Humanizer/Properties/Resources.ca.resx: les formes de futur base i paucal d'aquests mateixos grups fan servir Â«d'aquÃ­ {0} ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament resultats de la forma Â«d'aquÃ­ N ...Â» per segons/minuts/dies/mesos/anys.
  Notes: Segona passada independent: proposta confirmada sense canvis.
- [ca] `DateHumanize_MultipleYearsFromNow_Plural`
  Original: {0} anys a partir d'ara -> d'aquÃ­ {0} anys
  Final: d'aquÃ­ {0} anys
  Decision: confirmed
  Rationale: Com a parlant nadiu de catalÃ , confirmo que la forma actual Ã©s correcta perÃ² menys idiomÃ tica i menys coherent internament que el patrÃ³ Â«d'aquÃ­ {0} ...Â» present a la localitzaciÃ³.
  Evidence: src/Humanizer/Properties/Resources.ca.resx: les formes de futur base i paucal d'aquests mateixos grups fan servir Â«d'aquÃ­ {0} ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament resultats de la forma Â«d'aquÃ­ N ...Â» per segons/minuts/dies/mesos/anys.
  Notes: Segona passada independent: proposta confirmada sense canvis.
- [ca] `DateHumanize_MultipleYearsFromNow_Singular`
  Original: {0} any a partir d'ara -> d'aquÃ­ {0} any
  Final: d'aquÃ­ {0} any
  Decision: confirmed
  Rationale: Com a parlant nadiu de catalÃ , confirmo que la forma actual Ã©s correcta perÃ² menys idiomÃ tica i menys coherent internament que el patrÃ³ Â«d'aquÃ­ {0} ...Â» present a la localitzaciÃ³.
  Evidence: src/Humanizer/Properties/Resources.ca.resx: les formes de futur base i paucal d'aquests mateixos grups fan servir Â«d'aquÃ­ {0} ...Â».; tests/Humanizer.Tests/Localisation/ca/DateHumanizeTests.cs: els tests de futur validen explÃ­citament resultats de la forma Â«d'aquÃ­ N ...Â» per segons/minuts/dies/mesos/anys.
  Notes: Segona passada independent: proposta confirmada sense canvis.
- [cs] `TimeSpanHumanize_Zero`
  Original: není čas -> žádný čas
  Final: žádný čas
  Decision: confirmed
  Rationale: Jako druhý nezávislý rodilý mluvčí potvrzuji nález. Výraz „není čas“ v běžné češtině vyjadřuje nedostatek času (spěch), nikoli nulovou délku intervalu; pro TimeSpan.Zero je přirozenější a významově přesnější „žádný čas“.
  Evidence: src/Humanizer/Properties/Resources.cs.resx: klíč TimeSpanHumanize_Zero je aktuálně přeložen jako "není čas", což významově odpovídá spíš situaci "nemáme čas".; tests/Humanizer.Tests/Localisation/cs/TimeSpanHumanizeTests.cs: chybí aserce pro nulové trvání, takže stávající nevhodná formulace není testy zachycena.
  Notes: Druhé kolo adjudikace: první návrh potvrzen bez úprav.
- [el] `DateHumanize_SingleHourFromNow`
  Original: Ï€ÏÎ¯Î½ Î±Ï€ÏŒ Î¼Î¯Î± ÏŽÏÎ± Î±Ï€ÏŒ Ï„ÏŽÏÎ± -> Î¼Î¯Î± ÏŽÏÎ± Î±Ï€ÏŒ Ï„ÏŽÏÎ±
  Final: Î¼Î¯Î± ÏŽÏÎ± Î±Ï€ÏŒ Ï„ÏŽÏÎ±
  Decision: confirmed
  Rationale: Î•Ï€Î¹Î²ÎµÎ²Î±Î¹ÏŽÎ½ÎµÏ„Î±Î¹. Î¤Î¿ Â«Ï€ÏÎ¹Î½ Î±Ï€ÏŒÂ» Î´Î·Î»ÏŽÎ½ÎµÎ¹ Ï€Î±ÏÎµÎ»Î¸ÏŒÎ½, ÎµÎ½ÏŽ Ï„Î¿ ÎºÎ»ÎµÎ¹Î´Î¯ FromNow Î±Ï€Î±Î¹Ï„ÎµÎ¯ Î¼ÎµÎ»Î»Î¿Î½Ï„Î¹ÎºÎ® Î±Î½Î±Ï†Î¿ÏÎ¬. Î— Î¼Î¿ÏÏ†Î® Â«Î¼Î¯Î± ÏŽÏÎ± Î±Ï€ÏŒ Ï„ÏŽÏÎ±Â» ÎµÎ¯Î½Î±Î¹ Ï†Ï…ÏƒÎ¹ÎºÎ® ÎºÎ±Î¹ ÏƒÏ…Î½ÎµÏ€Î®Ï‚ Î¼Îµ Ï„Î± Î»Î¿Î¹Ï€Î¬ FromNow Î¼Î·Î½ÏÎ¼Î±Ï„Î±.
  Evidence: src/Humanizer/Properties/Resources.el.resx (DateHumanize_SingleHourFromNow = "Ï€ÏÎ¯Î½ Î±Ï€ÏŒ Î¼Î¯Î± ÏŽÏÎ± Î±Ï€ÏŒ Ï„ÏŽÏÎ±"); tests/Humanizer.Tests/Localisation/el/DateHumanizeTests.cs (HoursFromNow, InlineData(1, "Ï€ÏÎ¯Î½ Î±Ï€ÏŒ Î¼Î¯Î± ÏŽÏÎ± Î±Ï€ÏŒ Ï„ÏŽÏÎ±"))
  Notes: Confirmed in second pass without changing severity or replacement.
- [el] `DateHumanize_SingleMinuteFromNow`
  Original: Ï€ÏÎ¯Î½ Î±Ï€ÏŒ Î­Î½Î± Î»ÎµÏ€Ï„ÏŒ Î±Ï€ÏŒ Ï„ÏŽÏÎ± -> Î­Î½Î± Î»ÎµÏ€Ï„ÏŒ Î±Ï€ÏŒ Ï„ÏŽÏÎ±
  Final: Î­Î½Î± Î»ÎµÏ€Ï„ÏŒ Î±Ï€ÏŒ Ï„ÏŽÏÎ±
  Decision: confirmed
  Rationale: Î•Ï€Î¹Î²ÎµÎ²Î±Î¹ÏŽÎ½ÎµÏ„Î±Î¹. Î— Ï€Î±ÏÎ¿ÏÏƒÎ± Ï†ÏÎ¬ÏƒÎ· Î­Ï‡ÎµÎ¹ Î´ÎµÎ¯ÎºÏ„Î· Ï€Î±ÏÎµÎ»Î¸ÏŒÎ½Ï„Î¿Ï‚ ÏƒÎµ ÎºÎ»ÎµÎ¹Î´Î¯ Î¼Î­Î»Î»Î¿Î½Ï„Î¿Ï‚. Î¤Î¿ Â«Î­Î½Î± Î»ÎµÏ€Ï„ÏŒ Î±Ï€ÏŒ Ï„ÏŽÏÎ±Â» Î±Ï€Î¿Î´Î¯Î´ÎµÎ¹ ÏƒÏ‰ÏƒÏ„Î¬ Ï„Î· Ï‡ÏÎ¿Î½Î¹ÎºÎ® ÎºÎ±Ï„ÎµÏÎ¸Ï…Î½ÏƒÎ· ÎºÎ±Î¹ Ï„Î±Î¹ÏÎ¹Î¬Î¶ÎµÎ¹ Î¼Îµ Ï„Î¿ Î¼Î¿Ï„Î¯Î²Î¿ Ï„Ï‰Î½ Ï€Î¿Î»Î»Î±Ï€Î»ÏŽÎ½ Î»ÎµÏ€Ï„ÏŽÎ½.
  Evidence: src/Humanizer/Properties/Resources.el.resx (DateHumanize_SingleMinuteFromNow = "Ï€ÏÎ¯Î½ Î±Ï€ÏŒ Î­Î½Î± Î»ÎµÏ€Ï„ÏŒ Î±Ï€ÏŒ Ï„ÏŽÏÎ±"); tests/Humanizer.Tests/Localisation/el/DateHumanizeTests.cs (MinutesFromNow, InlineData(1, "Ï€ÏÎ¯Î½ Î±Ï€ÏŒ Î­Î½Î± Î»ÎµÏ€Ï„ÏŒ Î±Ï€ÏŒ Ï„ÏŽÏÎ±"))
  Notes: Confirmed in second pass without changing severity or replacement.
- [el] `DateHumanize_SingleMonthFromNow`
  Original: Ï€ÏÎ¹Î½ Î±Ï€ÏŒ Î­Î½Î±Î½ Î¼Î®Î½Î± Î±Ï€ÏŒ Ï„ÏŽÏÎ± -> Î­Î½Î±Î½ Î¼Î®Î½Î± Î±Ï€ÏŒ Ï„ÏŽÏÎ±
  Final: Î­Î½Î±Î½ Î¼Î®Î½Î± Î±Ï€ÏŒ Ï„ÏŽÏÎ±
  Decision: confirmed
  Rationale: Î•Ï€Î¹Î²ÎµÎ²Î±Î¹ÏŽÎ½ÎµÏ„Î±Î¹. Î— Ï…Ï†Î¹ÏƒÏ„Î¬Î¼ÎµÎ½Î· Î´Î¹Î±Ï„ÏÏ€Ï‰ÏƒÎ· Î±Î½Ï„Î¹Ï†Î¬ÏƒÎºÎµÎ¹ Î¼Îµ Ï„Î¿ FromNow Î»ÏŒÎ³Ï‰ Â«Ï€ÏÎ¹Î½ Î±Ï€ÏŒÂ». Î— Ï€ÏÎ¿Ï„ÎµÎ¹Î½ÏŒÎ¼ÎµÎ½Î· Î±Ï€ÏŒÎ´Î¿ÏƒÎ· ÎµÎ¯Î½Î±Î¹ Î³ÏÎ±Î¼Î¼Î±Ï„Î¹ÎºÎ¬ Î¿ÏÎ¸Î®, Ï†Ï…ÏƒÎ¹ÎºÎ® ÎºÎ±Î¹ ÎµÏ…Î¸Ï…Î³ÏÎ±Î¼Î¼Î¹ÏƒÎ¼Î­Î½Î· Î¼Îµ Ï„Î· Î¼Î¿ÏÏ†Î® Â«{0} Î¼Î®Î½ÎµÏ‚ Î±Ï€ÏŒ Ï„ÏŽÏÎ±Â».
  Evidence: src/Humanizer/Properties/Resources.el.resx (DateHumanize_SingleMonthFromNow = "Ï€ÏÎ¹Î½ Î±Ï€ÏŒ Î­Î½Î±Î½ Î¼Î®Î½Î± Î±Ï€ÏŒ Ï„ÏŽÏÎ±"); tests/Humanizer.Tests/Localisation/el/DateHumanizeTests.cs (MonthsFromNow, InlineData(1, "Ï€ÏÎ¹Î½ Î±Ï€ÏŒ Î­Î½Î±Î½ Î¼Î®Î½Î± Î±Ï€ÏŒ Ï„ÏŽÏÎ±"))
  Notes: Confirmed in second pass without changing severity or replacement.
- [el] `DateHumanize_SingleSecondFromNow`
  Original: Ï€ÏÎ¹Î½ Î±Ï€ÏŒ Î­Î½Î± Î´ÎµÏ…Ï„ÎµÏÏŒÎ»ÎµÏ€Ï„Î¿ Î±Ï€ÏŒ Ï„ÏŽÏÎ± -> Î­Î½Î± Î´ÎµÏ…Ï„ÎµÏÏŒÎ»ÎµÏ€Ï„Î¿ Î±Ï€ÏŒ Ï„ÏŽÏÎ±
  Final: Î­Î½Î± Î´ÎµÏ…Ï„ÎµÏÏŒÎ»ÎµÏ€Ï„Î¿ Î±Ï€ÏŒ Ï„ÏŽÏÎ±
  Decision: confirmed
  Rationale: Î•Ï€Î¹Î²ÎµÎ²Î±Î¹ÏŽÎ½ÎµÏ„Î±Î¹. Î— Ï€Î±ÏÎ¿ÏÏƒÎ± Î´Î¹Î±Ï„ÏÏ€Ï‰ÏƒÎ· ÏƒÎ·Î¼Î±Î¯Î½ÎµÎ¹ Ï€Î±ÏÎµÎ»Î¸ÏŒÎ½ ÎµÎ¾Î±Î¹Ï„Î¯Î±Ï‚ Ï„Î¿Ï… Â«Ï€ÏÎ¹Î½ Î±Ï€ÏŒÂ». Î¤Î¿ Â«Î­Î½Î± Î´ÎµÏ…Ï„ÎµÏÏŒÎ»ÎµÏ€Ï„Î¿ Î±Ï€ÏŒ Ï„ÏŽÏÎ±Â» ÎµÎ¯Î½Î±Î¹ Î· ÏƒÏ‰ÏƒÏ„Î® Î¼ÎµÎ»Î»Î¿Î½Ï„Î¹ÎºÎ® ÎµÎºÎ´Î¿Ï‡Î® ÎºÎ±Î¹ Î±ÎºÎ¿Î»Î¿Ï…Î¸ÎµÎ¯ Ï„Î¿ Ï…Ï€Î¬ÏÏ‡Î¿Î½ Î¼Î¿Ï„Î¯Î²Î¿ Ï„Î·Ï‚ Î³Î»ÏŽÏƒÏƒÎ±Ï‚.
  Evidence: src/Humanizer/Properties/Resources.el.resx (DateHumanize_SingleSecondFromNow = "Ï€ÏÎ¹Î½ Î±Ï€ÏŒ Î­Î½Î± Î´ÎµÏ…Ï„ÎµÏÏŒÎ»ÎµÏ€Ï„Î¿ Î±Ï€ÏŒ Ï„ÏŽÏÎ±"); tests/Humanizer.Tests/Localisation/el/DateHumanizeTests.cs (SecondsFromNow, InlineData(1, "Ï€ÏÎ¹Î½ Î±Ï€ÏŒ Î­Î½Î± Î´ÎµÏ…Ï„ÎµÏÏŒÎ»ÎµÏ€Ï„Î¿ Î±Ï€ÏŒ Ï„ÏŽÏÎ±"))
  Notes: Confirmed in second pass without changing severity or replacement.
- [el] `DateHumanize_SingleYearFromNow`
  Original: Ï€ÏÎ¹Î½ Î±Ï€ÏŒ Î­Î½Î±Î½ Ï‡ÏÏŒÎ½Î¿ Î±Ï€ÏŒ Ï„ÏŽÏÎ± -> Î­Î½Î±Î½ Ï‡ÏÏŒÎ½Î¿ Î±Ï€ÏŒ Ï„ÏŽÏÎ±
  Final: Î­Î½Î±Î½ Ï‡ÏÏŒÎ½Î¿ Î±Ï€ÏŒ Ï„ÏŽÏÎ±
  Decision: confirmed
  Rationale: Î•Ï€Î¹Î²ÎµÎ²Î±Î¹ÏŽÎ½ÎµÏ„Î±Î¹. Î¤Î¿ Â«Ï€ÏÎ¹Î½ Î±Ï€ÏŒÂ» Î¼ÎµÏ„Î±Ï„ÏÎ­Ï€ÎµÎ¹ Î»Î±Î½Î¸Î±ÏƒÎ¼Î­Î½Î± Ï„Î¿ Î¼Î®Î½Ï…Î¼Î± ÏƒÎµ Ï€Î±ÏÎµÎ»Î¸Î¿Î½Ï„Î¹ÎºÏŒ. Î— Î±Î½Ï„Î¹ÎºÎ±Ï„Î¬ÏƒÏ„Î±ÏƒÎ· Î¼Îµ Â«Î­Î½Î±Î½ Ï‡ÏÏŒÎ½Î¿ Î±Ï€ÏŒ Ï„ÏŽÏÎ±Â» ÎµÎ¯Î½Î±Î¹ Î· Ï†Ï…ÏƒÎ¹ÎºÎ® Î¼ÎµÎ»Î»Î¿Î½Ï„Î¹ÎºÎ® Î´Î¹Î±Ï„ÏÏ€Ï‰ÏƒÎ· Î³Î¹Î± Î¿Ï…Î´Î­Ï„ÎµÏÎ¿ UX ÎºÎµÎ¯Î¼ÎµÎ½Î¿.
  Evidence: src/Humanizer/Properties/Resources.el.resx (DateHumanize_SingleYearFromNow = "Ï€ÏÎ¹Î½ Î±Ï€ÏŒ Î­Î½Î±Î½ Ï‡ÏÏŒÎ½Î¿ Î±Ï€ÏŒ Ï„ÏŽÏÎ±"); tests/Humanizer.Tests/Localisation/el/DateHumanizeTests.cs (YearsFromNow, InlineData(1, "Ï€ÏÎ¹Î½ Î±Ï€ÏŒ Î­Î½Î±Î½ Ï‡ÏÏŒÎ½Î¿ Î±Ï€ÏŒ Ï„ÏŽÏÎ±"))
  Notes: Confirmed in second pass without changing severity or replacement.
- [el] `TimeSpanHumanize_SingleMillisecond`
  Original: 1 Ï‡Î¹Î»Î¹Î¿ÏƒtÏŒ Ï„Î¿Ï… Î´ÎµÏ…Ï„ÎµÏÎ¿Î»Î­Ï€Ï„Î¿Ï… -> 1 Ï‡Î¹Î»Î¹Î¿ÏƒÏ„ÏŒ Ï„Î¿Ï… Î´ÎµÏ…Ï„ÎµÏÎ¿Î»Î­Ï€Ï„Î¿Ï…
  Final: 1 Ï‡Î¹Î»Î¹Î¿ÏƒÏ„ÏŒ Ï„Î¿Ï… Î´ÎµÏ…Ï„ÎµÏÎ¿Î»Î­Ï€Ï„Î¿Ï…
  Decision: confirmed
  Rationale: Î•Ï€Î¹Î²ÎµÎ²Î±Î¹ÏŽÎ½ÎµÏ„Î±Î¹. Î ÏÏŒÎºÎµÎ¹Ï„Î±Î¹ Î³Î¹Î± ÏƒÎ±Ï†Î­Ï‚ Ï„Ï…Ï€Î¿Î³ÏÎ±Ï†Î¹ÎºÏŒ/ÎºÏ‰Î´Î¹ÎºÎ¿ÏƒÎµÎ»Î¯Î´Î±Ï‚ ÏƒÏ†Î¬Î»Î¼Î± (Î»Î±Ï„Î¹Î½Î¹ÎºÏŒ 't' Î±Î½Ï„Î¯ ÎµÎ»Î»Î·Î½Î¹ÎºÎ¿Ï 'Ï„') ÏƒÎµ ÎµÎ¼Ï†Î±Î½Î¹Î¶ÏŒÎ¼ÎµÎ½Î¿ ÎºÎµÎ¯Î¼ÎµÎ½Î¿.
  Evidence: src/Humanizer/Properties/Resources.el.resx (TimeSpanHumanize_SingleMillisecond = "1 Ï‡Î¹Î»Î¹Î¿ÏƒtÏŒ Ï„Î¿Ï… Î´ÎµÏ…Ï„ÎµÏÎ¿Î»Î­Ï€Ï„Î¿Ï…"); tests/Humanizer.Tests/Localisation/el/TimeSpanHumanizeTests.cs (Milliseconds, InlineData(1, "1 Ï‡Î¹Î»Î¹Î¿ÏƒtÏŒ Ï„Î¿Ï… Î´ÎµÏ…Ï„ÎµÏÎ¿Î»Î­Ï€Ï„Î¿Ï…"))
  Notes: Confirmed in second pass without changing severity or replacement.
- [el] `TimeSpanHumanize_Zero`
  Original: Î¼Î·Î´Î­Î½ Ï‡ÏÏŒÎ½Î¿Ï‚ -> Î¼Î·Î´ÎµÎ½Î¹ÎºÏŒÏ‚ Ï‡ÏÏŒÎ½Î¿Ï‚
  Final: Î¼Î·Î´ÎµÎ½Î¹ÎºÏŒÏ‚ Ï‡ÏÏŒÎ½Î¿Ï‚
  Decision: confirmed
  Rationale: Î•Ï€Î¹Î²ÎµÎ²Î±Î¹ÏŽÎ½ÎµÏ„Î±Î¹ Î¼Îµ Î¼Î­Ï„ÏÎ¹Î± Î²ÎµÎ²Î±Î¹ÏŒÏ„Î·Ï„Î±. Î¤Î¿ Â«Î¼Î·Î´Î­Î½ Ï‡ÏÏŒÎ½Î¿Ï‚Â» Î´ÎµÎ½ ÎµÎ¯Î½Î±Î¹ Ï†Ï…ÏƒÎ¹ÎºÎ® Î¿Î½Î¿Î¼Î±Ï„Î¹ÎºÎ® Î±Ï€ÏŒÎ´Î¿ÏƒÎ· Î³Î¹Î± UI ÎºÎµÎ¯Î¼ÎµÎ½Î¿Â· Î· Ï€ÏÎ¿Ï„ÎµÎ¹Î½ÏŒÎ¼ÎµÎ½Î· Î¼Î¿ÏÏ†Î® ÎµÎ¯Î½Î±Î¹ Î³ÏÎ±Î¼Î¼Î±Ï„Î¹ÎºÎ¬ Î¿Î¼Î±Î»ÏŒÏ„ÎµÏÎ·.
  Evidence: src/Humanizer/Properties/Resources.el.resx (TimeSpanHumanize_Zero = "Î¼Î·Î´Î­Î½ Ï‡ÏÏŒÎ½Î¿Ï‚"); tests/Humanizer.Tests/Localisation/el/TimeSpanHumanizeTests.cs (NoTimeToWords expects "Î¼Î·Î´Î­Î½ Ï‡ÏÏŒÎ½Î¿Ï‚")
  Notes: Confirmed in second pass; confidence remains medium because acceptable stylistic alternatives exist.
- [es] `DateHumanize_MultipleDaysFromNow_Dual`
  Original: hace {0} dÃ­as desde ahora -> en {0} dÃ­as
  Final: en {0} dÃ­as
  Decision: confirmed
  Rationale: La clave es de futuro (from now), pero la cadena actual usa una construcciÃ³n de pasado ('hace ...'). En espaÃ±ol natural debe expresarse con 'en ...'.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleDaysFromNow*).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (DaysFromNow usa 'en ...').
  Notes: Se alinea con la variante base DateHumanize_MultipleDaysFromNow. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleDaysFromNow_Plural`
  Original: hace {0} dÃ­as desde ahora -> en {0} dÃ­as
  Final: en {0} dÃ­as
  Decision: confirmed
  Rationale: La cadena mezcla pasado ('hace') con una clave de futuro ('from now'). La formulaciÃ³n idiomÃ¡tica es 'en {0} dÃ­as'.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleDaysFromNow*).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (DaysFromNow).
  Notes: CorrecciÃ³n de tiempo verbal. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleDaysFromNow_Singular`
  Original: {0} dÃ­a desde ahora -> en {0} dÃ­a
  Final: en {0} dÃ­a
  Decision: confirmed
  Rationale: La frase actual es forzada y poco natural en espaÃ±ol. Para futuro inmediato en Humanizer se usa el patrÃ³n 'en ...'.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleDaysFromNow_Singular).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (futuro con 'en').
  Notes: Se normaliza al mismo patrÃ³n de las demÃ¡s formas de futuro. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleHoursFromNow_Dual`
  Original: hace {0} horas desde ahora -> en {0} horas
  Final: en {0} horas
  Decision: confirmed
  Rationale: La expresiÃ³n actual contradice el sentido de futuro de la clave; 'hace' indica pasado.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleHoursFromNow_Dual).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (HoursFromNow).
  Notes: Se usa el patrÃ³n natural de futuro: 'en {0} horas'. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleHoursFromNow_Paucal`
  Original: hace {0} horas desde ahora -> en {0} horas
  Final: en {0} horas
  Decision: confirmed
  Rationale: La forma actual estÃ¡ en pasado y no corresponde a una clave de futuro.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleHoursFromNow_Paucal).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (HoursFromNow).
  Notes: CorrecciÃ³n de tiempo verbal. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleHoursFromNow_Plural`
  Original: hace {0} horas desde ahora -> en {0} horas
  Final: en {0} horas
  Decision: confirmed
  Rationale: La redacciÃ³n actual mezcla pasado con futuro y suena no nativa.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleHoursFromNow_Plural).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (HoursFromNow).
  Notes: Consistencia con DateHumanize_MultipleHoursFromNow. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleHoursFromNow_Singular`
  Original: hace {0} hora desde ahora -> en {0} hora
  Final: en {0} hora
  Decision: confirmed
  Rationale: La estructura 'hace ... desde ahora' es incorrecta para futuro en espaÃ±ol.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleHoursFromNow_Singular).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (HoursFromNow).
  Notes: Se sustituye por la fÃ³rmula idiomÃ¡tica 'en ...'. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleMinutesFromNow_Dual`
  Original: {0} minutos desde ahora -> en {0} minutos
  Final: en {0} minutos
  Decision: confirmed
  Rationale: Aunque entendible, no sigue la forma idiomÃ¡tica usada en espaÃ±ol para futuro relativo en Humanizer ('en ...').
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleMinutesFromNow_Dual).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (MinutesFromNow).
  Notes: Ajuste de estilo y consistencia. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleMinutesFromNow_Paucal`
  Original: hace {0} minutos desde ahora -> en {0} minutos
  Final: en {0} minutos
  Decision: confirmed
  Rationale: La cadena actual usa marcador de pasado ('hace') en una clave de futuro.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleMinutesFromNow_Paucal).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (MinutesFromNow).
  Notes: CorrecciÃ³n de tiempo verbal. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleMinutesFromNow_Plural`
  Original: hace {0} minutos desde ahora -> en {0} minutos
  Final: en {0} minutos
  Decision: confirmed
  Rationale: La redacciÃ³n es semÃ¡nticamente incorrecta para una referencia temporal futura.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleMinutesFromNow_Plural).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (MinutesFromNow).
  Notes: Se normaliza a 'en {0} minutos'. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleMinutesFromNow_Singular`
  Original: hace {0} minuto desde ahora -> en {0} minuto
  Final: en {0} minuto
  Decision: confirmed
  Rationale: La cadena combina pasado y futuro de forma antinatural; en espaÃ±ol estÃ¡ndar debe ir con 'en'.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleMinutesFromNow_Singular).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (MinutesFromNow).
  Notes: Consistencia con el resto de variantes FromNow. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleMonthsAgo_Dual`
  Original: hace {0} meses desde ahora -> hace {0} meses
  Final: hace {0} meses
  Decision: confirmed
  Rationale: La clave es de pasado ('ago') y no debe incluir 'desde ahora', que expresa futuro relativo.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleMonthsAgo_Dual).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (MonthsAgo).
  Notes: Se alinea con DateHumanize_MultipleMonthsAgo. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleMonthsAgo_Singular`
  Original: hace {0} meses -> hace {0} mes
  Final: hace {0} mes
  Decision: confirmed
  Rationale: La forma singular debe usar 'mes', no 'meses'.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleMonthsAgo_Singular).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (MonthsAgo singular usa 'un mes').
  Notes: CorrecciÃ³n de concordancia de nÃºmero. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleMonthsFromNow_Dual`
  Original: hace {0} meses desde ahora -> en {0} meses
  Final: en {0} meses
  Decision: confirmed
  Rationale: La cadena actual usa un giro de pasado para una clave de futuro; en espaÃ±ol debe ser 'en {0} meses'.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleMonthsFromNow_Dual).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (MonthsFromNow).
  Notes: Consistencia con DateHumanize_MultipleMonthsFromNow. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleMonthsFromNow_Paucal`
  Original: hace {0} meses desde ahora -> en {0} meses
  Final: en {0} meses
  Decision: confirmed
  Rationale: ExpresiÃ³n temporal incorrecta: marca pasado cuando la clave expresa futuro.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleMonthsFromNow_Paucal).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (MonthsFromNow).
  Notes: CorrecciÃ³n de tiempo verbal. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleMonthsFromNow_Plural`
  Original: hace {0} meses desde ahora -> en {0} meses
  Final: en {0} meses
  Decision: confirmed
  Rationale: La construcciÃ³n actual suena no nativa y contradice el sentido de 'from now'.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleMonthsFromNow_Plural).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (MonthsFromNow).
  Notes: Se normaliza a la forma idiomÃ¡tica de futuro. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleMonthsFromNow_Singular`
  Original: hace {0} mes desde ahora -> en {0} mes
  Final: en {0} mes
  Decision: confirmed
  Rationale: La forma actual mezcla pasado y futuro. Para espaÃ±ol natural se usa 'en {0} mes'.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleMonthsFromNow_Singular).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (MonthsFromNow).
  Notes: Consistencia con el resto de variantes FromNow. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleSecondsFromNow_Dual`
  Original: hace {0} segundos desde ahora -> en {0} segundos
  Final: en {0} segundos
  Decision: confirmed
  Rationale: La clave requiere futuro relativo y la cadena actual estÃ¡ en pasado.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleSecondsFromNow_Dual).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (SecondsFromNow).
  Notes: CorrecciÃ³n de tiempo verbal. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleSecondsFromNow_Paucal`
  Original: hace {0} segundos desde ahora -> en {0} segundos
  Final: en {0} segundos
  Decision: confirmed
  Rationale: La frase no es idiomÃ¡tica para futuro; 'hace' marca pasado.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleSecondsFromNow_Paucal).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (SecondsFromNow).
  Notes: Alineado con la variante principal de futuro. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleSecondsFromNow_Plural`
  Original: hace {0} segundos desde ahora -> en {0} segundos
  Final: en {0} segundos
  Decision: confirmed
  Rationale: El contenido expresa pasado y la clave expresa futuro.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleSecondsFromNow_Plural).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (SecondsFromNow).
  Notes: Se corrige al patrÃ³n estÃ¡ndar en espaÃ±ol. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleSecondsFromNow_Singular`
  Original: hace {0} segundo desde ahora -> en {0} segundo
  Final: en {0} segundo
  Decision: confirmed
  Rationale: ConstrucciÃ³n incorrecta para referencia futura; debe ser 'en {0} segundo'.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleSecondsFromNow_Singular).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (SecondsFromNow).
  Notes: Consistencia con el resto de variantes FromNow. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleYearsFromNow_Dual`
  Original: hace {0} aÃ±os desde ahora -> en {0} aÃ±os
  Final: en {0} aÃ±os
  Decision: confirmed
  Rationale: La forma actual contradice la semÃ¡ntica de futuro de la clave.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleYearsFromNow_Dual).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (YearsFromNow).
  Notes: CorrecciÃ³n de tiempo verbal. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleYearsFromNow_Paucal`
  Original: hace {0} aÃ±os desde ahora -> en {0} aÃ±os
  Final: en {0} aÃ±os
  Decision: confirmed
  Rationale: Usa 'hace' (pasado) en una variante de futuro.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleYearsFromNow_Paucal).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (YearsFromNow).
  Notes: Consistencia con DateHumanize_MultipleYearsFromNow. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleYearsFromNow_Plural`
  Original: hace {0} aÃ±os desde ahora -> en {0} aÃ±os
  Final: en {0} aÃ±os
  Decision: confirmed
  Rationale: La cadena no es natural en espaÃ±ol para futuro relativo.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleYearsFromNow_Plural).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (YearsFromNow).
  Notes: Se estandariza al patrÃ³n 'en ...'. Confirmado sin cambios en segunda pasada.
- [es] `DateHumanize_MultipleYearsFromNow_Singular`
  Original: hace {0} aÃ±o desde ahora -> en {0} aÃ±o
  Final: en {0} aÃ±o
  Decision: confirmed
  Rationale: ConstrucciÃ³n gramaticalmente forzada y en tiempo verbal incorrecto para una clave de futuro.
  Evidence: src/Humanizer/Properties/Resources.es.resx (DateHumanize_MultipleYearsFromNow_Singular).; tests/Humanizer.Tests/Localisation/es/DateHumanizeTests.cs (YearsFromNow).
  Notes: CorrecciÃ³n idiomÃ¡tica para espaÃ±ol nativo. Confirmado sin cambios en segunda pasada.
- [fa] `TimeSpanHumanize_MultipleMilliseconds`
  Original: {0} Ù…ÛŒÙ„ÛŒ Ø«Ø§Ù†ÛŒÙ‡ -> {0} Ù…ÛŒÙ„ÛŒâ€ŒØ«Ø§Ù†ÛŒÙ‡
  Final: {0} Ù…ÛŒÙ„ÛŒâ€ŒØ«Ø§Ù†ÛŒÙ‡
  Decision: confirmed
  Rationale: ØªØ£ÛŒÛŒØ¯ Ù…ÛŒâ€ŒØ´ÙˆØ¯. Ø¯Ø± ÙØ§Ø±Ø³ÛŒ Ù…Ø¹ÛŒØ§Ø± Ù†ÙˆØ´ØªØ§Ø± Ø¯Ø±Ø³Øª Ø§ÛŒÙ† ØªØ±Ú©ÛŒØ¨ Â«Ù…ÛŒÙ„ÛŒâ€ŒØ«Ø§Ù†ÛŒÙ‡Â» Ø§Ø³Øª Ùˆ Ø§Ø³ØªÙØ§Ø¯Ù‡ Ø§Ø² ÙØ§ØµÙ„Ù‡Ù” Ú©Ø§Ù…Ù„ Ø¯Ø± Ù‡Ù…ÛŒÙ† Ø¨Ø³ØªÙ‡Ù” Ø²Ø¨Ø§Ù†ÛŒ Ø¨Ø§ Ù…Ù‚Ø¯Ø§Ø± TimeUnit_Millisecond Ù†Ø§Ø³Ø§Ø²Ú¯Ø§Ø± Ø§Ø³Øª.
  Evidence: src/Humanizer/Properties/Resources.fa.resx: TimeSpanHumanize_MultipleMilliseconds={0} Ù…ÛŒÙ„ÛŒ Ø«Ø§Ù†ÛŒÙ‡; src/Humanizer/Properties/Resources.fa.resx: TimeSpanHumanize_SingleMillisecond=ÛŒÚ© Ù…ÛŒÙ„ÛŒ Ø«Ø§Ù†ÛŒÙ‡; src/Humanizer/Properties/Resources.fa.resx: TimeUnit_Millisecond=Ù…ÛŒÙ„ÛŒâ€ŒØ«Ø§Ù†ÛŒÙ‡; tests/Humanizer.Tests/Localisation/fa/TimeSpanHumanizeTests.cs: [InlineData(1, "ÛŒÚ© Ù…ÛŒÙ„ÛŒ Ø«Ø§Ù†ÛŒÙ‡")]; tests/Humanizer.Tests/Localisation/fa/TimeUnitToSymbolTests.cs: [InlineData(TimeUnit.Millisecond, "Ù…ÛŒÙ„ÛŒâ€ŒØ«Ø§Ù†ÛŒÙ‡")]
  Notes: Confirmed as an orthographic defect; any implementation change should align related tests in a separate code change.
- [fa] `TimeSpanHumanize_SingleMillisecond`
  Original: ÛŒÚ© Ù…ÛŒÙ„ÛŒ Ø«Ø§Ù†ÛŒÙ‡ -> ÛŒÚ© Ù…ÛŒÙ„ÛŒâ€ŒØ«Ø§Ù†ÛŒÙ‡
  Final: ÛŒÚ© Ù…ÛŒÙ„ÛŒâ€ŒØ«Ø§Ù†ÛŒÙ‡
  Decision: confirmed
  Rationale: ØªØ£ÛŒÛŒØ¯ Ù…ÛŒâ€ŒØ´ÙˆØ¯. Ù†ÙˆØ´ØªØ§Ø± Â«ÛŒÚ© Ù…ÛŒÙ„ÛŒâ€ŒØ«Ø§Ù†ÛŒÙ‡Â» Ø§Ø² Ù†Ø¸Ø± Ø§Ù…Ù„Ø§ÛŒ Ù…Ø¹ÛŒØ§Ø± ÙØ§Ø±Ø³ÛŒ Ø¯Ø±Ø³Øª Ø§Ø³Øª Ùˆ Ø¨Ø§ Ù…Ù‚Ø¯Ø§Ø± Ù…ÙˆØ¬ÙˆØ¯ Ø¨Ø±Ø§ÛŒ TimeUnit_Millisecond Ù†ÛŒØ² Ù‡Ù…â€ŒØ®ÙˆØ§Ù†ÛŒ Ø¯Ø§Ø±Ø¯.
  Evidence: src/Humanizer/Properties/Resources.fa.resx: TimeSpanHumanize_MultipleMilliseconds={0} Ù…ÛŒÙ„ÛŒ Ø«Ø§Ù†ÛŒÙ‡; src/Humanizer/Properties/Resources.fa.resx: TimeSpanHumanize_SingleMillisecond=ÛŒÚ© Ù…ÛŒÙ„ÛŒ Ø«Ø§Ù†ÛŒÙ‡; src/Humanizer/Properties/Resources.fa.resx: TimeUnit_Millisecond=Ù…ÛŒÙ„ÛŒâ€ŒØ«Ø§Ù†ÛŒÙ‡; tests/Humanizer.Tests/Localisation/fa/TimeSpanHumanizeTests.cs: [InlineData(1, "ÛŒÚ© Ù…ÛŒÙ„ÛŒ Ø«Ø§Ù†ÛŒÙ‡")]; tests/Humanizer.Tests/Localisation/fa/TimeUnitToSymbolTests.cs: [InlineData(TimeUnit.Millisecond, "Ù…ÛŒÙ„ÛŒâ€ŒØ«Ø§Ù†ÛŒÙ‡")]
  Notes: Confirmed as an orthographic defect; any implementation change should align related tests in a separate code change.
- [fi] `DateHumanize_SingleSecondAgo`
  Original: sekuntti sitten -> sekunti sitten
  Final: sekunti sitten
  Decision: confirmed
  Rationale: Vahvistettu. "Sekuntti" on kirjoitusvirhe suomessa; norminmukainen muoto on "sekunti". Aikailmauksessa oikea ja luonnollinen käännös on "sekunti sitten".
  Evidence: src/Humanizer/Properties/Resources.fi.resx:192; src/Humanizer/Properties/Resources.fi.resx:193; tests/Humanizer.Tests/Localisation/fi-FI/DateHumanizeTests.cs:33
  Notes: Korjaa resurssiarvo muotoon "sekunti sitten" ja päivitä fi-FI-testin odotusarvo vastaamaan korjausta.
- [fil] `TimeSpanHumanize_Age`
  Original: {0} gulang -> {0} na gulang
  Final: {0} na gulang
  Decision: confirmed
  Rationale: Kinukumpirma ko ang finding bilang suspicious: mas idiomatic sa Filipino ang may pang-angkop, kaya mas natural ang "{0} na gulang" kaysa "{0} gulang", lalo sa output na toWords (hal. "isang taon na gulang").
  Evidence: src/Humanizer/Properties/Resources.fil.resx:465; tests/Humanizer.Tests/Localisation/fil-PH/TimeSpanHumanizeTests.cs:80-81
  Notes: May style issue pero naiintindihan pa rin ang kasalukuyang output; dapat i-update nang sabay ang fil resource at fil-PH assertions.
- [fil] `TimeSpanHumanize_MultipleMilliseconds`
  Original: {0} milliseconds -> {0} milisegundo
  Final: {0} milisegundo
  Decision: confirmed
  Rationale: Kinukumpirma ko ang finding bilang defect: may hindi naisaling English unit ("milliseconds") sa fil locale. Dapat "{0} milisegundo" para sa natural at konsistent na Filipino output.
  Evidence: src/Humanizer/Properties/Resources.fil.resx:375-376; tests/Humanizer.Tests/Localisation/fil-PH/TimeSpanHumanizeTests.cs:63-66
  Notes: Apektado ang fil-PH fallback; kailangang i-align ang .resx at localization tests.
- [fil] `TimeSpanHumanize_MultipleMilliseconds_Dual`
  Original: {0} milliseconds -> {0} milisegundo
  Final: {0} milisegundo
  Decision: confirmed
  Rationale: Kinukumpirma ko ang finding bilang defect: may hindi naisaling English unit ("milliseconds") sa dual form. Sa Filipino, pareho ring "milisegundo" ang dapat gamitin.
  Evidence: src/Humanizer/Properties/Resources.fil.resx:378-379; tests/Humanizer.Tests/Localisation/fil-PH/TimeSpanHumanizeTests.cs:63-66
  Notes: Apektado ang fil-PH fallback; kailangang i-align ang .resx at localization tests.
- [fil] `TimeSpanHumanize_MultipleMilliseconds_Paucal`
  Original: {0} milliseconds -> {0} milisegundo
  Final: {0} milisegundo
  Decision: confirmed
  Rationale: Kinukumpirma ko ang finding bilang defect: nanatiling English ang "milliseconds" sa paucal form. Filipino unit term na "milisegundo" ang tama at konsistent.
  Evidence: src/Humanizer/Properties/Resources.fil.resx:381-382; tests/Humanizer.Tests/Localisation/fil-PH/TimeSpanHumanizeTests.cs:63-66
  Notes: Apektado ang fil-PH fallback; kailangang i-align ang .resx at localization tests.
- [fil] `TimeSpanHumanize_MultipleMilliseconds_Plural`
  Original: {0} milliseconds -> {0} milisegundo
  Final: {0} milisegundo
  Decision: confirmed
  Rationale: Kinukumpirma ko ang finding bilang defect: English leakage pa rin ang "milliseconds" sa plural form. Dapat itong mapalitan ng "milisegundo".
  Evidence: src/Humanizer/Properties/Resources.fil.resx:384-385; tests/Humanizer.Tests/Localisation/fil-PH/TimeSpanHumanizeTests.cs:63-66
  Notes: Apektado ang fil-PH fallback; kailangang i-align ang .resx at localization tests.
- [fil] `TimeSpanHumanize_MultipleMilliseconds_Singular`
  Original: {0} millisecond -> {0} milisegundo
  Final: {0} milisegundo
  Decision: confirmed
  Rationale: Kinukumpirma ko ang finding bilang defect: English singular na "millisecond" pa rin ang gamit sa singular-form key. Sa Filipino, "milisegundo" ang wastong anyo.
  Evidence: src/Humanizer/Properties/Resources.fil.resx:387-388; tests/Humanizer.Tests/Localisation/fil-PH/TimeSpanHumanizeTests.cs:63-66
  Notes: Apektado ang fil-PH fallback; kailangang i-align ang .resx at localization tests.
- [fil] `TimeSpanHumanize_SingleMillisecond`
  Original: 1 millisecond -> 1 milisegundo
  Final: 1 milisegundo
  Decision: confirmed
  Rationale: Kinukumpirma ko ang finding bilang defect: naka-English ang kasalukuyang string ("1 millisecond"). Filipino localization na "1 milisegundo" ang tamang replacement.
  Evidence: src/Humanizer/Properties/Resources.fil.resx:480-481; tests/Humanizer.Tests/Localisation/fil-PH/TimeSpanHumanizeTests.cs:63-66
  Notes: Apektado ang fil-PH fallback; kailangang i-align ang .resx at localization tests.
- [fil] `TimeSpanHumanize_SingleMillisecond_Words`
  Original: isang millisecond -> isang milisegundo
  Final: isang milisegundo
  Decision: confirmed
  Rationale: Kinukumpirma ko ang finding bilang defect: English term pa rin ang nasa word-form variant ("isang millisecond"). Ang natural na Filipino ay "isang milisegundo".
  Evidence: src/Humanizer/Properties/Resources.fil.resx:483-484; tests/Humanizer.Tests/Localisation/fil-PH/TimeSpanHumanizeTests.cs:89-90
  Notes: Apektado ang fil-PH fallback; kailangang i-align ang .resx at localization tests.
- [he] `E_Short`
  Original: ×ž -> ×ž×–
  Final: ×ž×–
  Decision: confirmed
  Rationale: ×ž××©×¨. ×©×™×ž×•×© ×‘××•×ª×” ××•×ª ×œ×ž×–×¨×— ×•×œ×ž×¢×¨×‘ ×™×•×¦×¨ ×”×ª× ×’×©×•×ª × ×™×•×•×˜×™×ª; "×ž×–" ×ž×•×œ "×ž×¢" ×”×•× ×§×™×¦×•×¨ ×‘×¨×•×¨ ×•×§×¨×™× ×œ×“×•×‘×¨ ×¢×‘×¨×™×ª.
  Evidence: src/Humanizer/Properties/Resources.he.resx: E_Short='×ž' and W_Short='×ž'; tests/Humanizer.Tests/Localisation/he/HeadingTests.cs: 90Â° and 270Â° both expect '×ž'
  Notes: ×ž××©×¨ ××ª ×”×¦×¢×ª ×”×ž×¢×‘×¨ ×œ×¡×˜ ×§×™×¦×•×¨×™× ×—×“-×ž×©×ž×¢×™.
- [he] `NE_Short`
  Original: ×¦×ž -> ×¦×ž×–
  Final: ×¦×ž×–
  Decision: confirmed
  Rationale: ×ž××©×¨. "×¦×ž" ×“×•-×ž×©×ž×¢×™ ×›×™×•×; "×¦×ž×–" ×©×•×ž×¨ ×¢×œ ×”×”×™×’×™×•×Ÿ ×”×ž×•×¨×¤×•×œ×•×’×™ (×¦×¤×•×Ÿ+×ž×–×¨×—) ×•×ž×‘×“×™×œ ×ž×¦×¤×•×Ÿ-×ž×¢×¨×‘.
  Evidence: src/Humanizer/Properties/Resources.he.resx: NE_Short='×¦×ž' and NW_Short='×¦×ž'; tests/Humanizer.Tests/Localisation/he/HeadingTests.cs: 45Â° and 315Â° both expect '×¦×ž'
  Notes: ×ž××©×¨ ×™×—×“ ×¢× NW_Short ×›×“×™ ×œ×©×ž×•×¨ ×¢×œ ×¢×§×‘×™×•×ª ×©×™×˜×”.
- [he] `NW_Short`
  Original: ×¦×ž -> ×¦×ž×¢
  Final: ×¦×ž×¢
  Decision: confirmed
  Rationale: ×ž××©×¨. ×”×§×™×¦×•×¨ ×”×ž×•×¦×¢ ×™×•×¦×¨ ×”×‘×—× ×” ×—×“-×ž×©×ž×¢×™×ª ×ž×•×œ "×¦×ž×–" ×•×ª×•×× ××ª ×ž×‘× ×” ×”×›×™× ×•×™×™× ×”×§×¦×¨×™× ×‘×¢×‘×¨×™×ª.
  Evidence: src/Humanizer/Properties/Resources.he.resx: NE_Short='×¦×ž' and NW_Short='×¦×ž'; tests/Humanizer.Tests/Localisation/he/HeadingTests.cs: 45Â° and 315Â° both expect '×¦×ž'
  Notes: ×ž××©×¨ ×›×—×œ×§ ×ž×–×•×’ ×ª×™×§×•× ×™× NE/NW.
- [he] `SE_Short`
  Original: ×“×ž -> ×“×ž×–
  Final: ×“×ž×–
  Decision: confirmed
  Rationale: ×ž××©×¨. ×”×‘×—× ×” ×‘×™×Ÿ ×“×¨×•×-×ž×–×¨×— ×œ×“×¨×•×-×ž×¢×¨×‘ ×—×™×•× ×™×ª, ×•"×“×ž×–" ×ž×‘×”×™×¨ ××ª ×¨×›×™×‘ ×”×ž×–×¨×— ×œ×œ× ×©×™× ×•×™ ×‘×§×™×¦×•×¨ ×©×œ ×“×¨×•×.
  Evidence: src/Humanizer/Properties/Resources.he.resx: SE_Short='×“×ž' and SW_Short='×“×ž'; tests/Humanizer.Tests/Localisation/he/HeadingTests.cs: 135Â° and 225Â° both expect '×“×ž'
  Notes: ×ž××©×¨ ×¢× SW_Short ×›×©×™× ×•×™ ×–×•×’×™.
- [he] `SW_Short`
  Original: ×“×ž -> ×“×ž×¢
  Final: ×“×ž×¢
  Decision: confirmed
  Rationale: ×ž××©×¨. "×“×ž×¢" ×™×•×¦×¨ × ×™×’×•×“ ×ª×§×™×Ÿ ×ž×•×œ "×“×ž×–" ×•×ž×¡×™×¨ ××ª ×”×¢×ž×™×ž×•×ª ×”×§×™×™×ž×ª ×‘×§×™×¦×•×¨×™ ×”××œ×›×¡×•× ×™× ×”×“×¨×•×ž×™×™×.
  Evidence: src/Humanizer/Properties/Resources.he.resx: SE_Short='×“×ž' and SW_Short='×“×ž'; tests/Humanizer.Tests/Localisation/he/HeadingTests.cs: 135Â° and 225Â° both expect '×“×ž'
  Notes: ×ž××©×¨ ×›×—×œ×§ ×ž×–×•×’ ×ª×™×§×•× ×™× SE/SW.
- [he] `W_Short`
  Original: ×ž -> ×ž×¢
  Final: ×ž×¢
  Decision: confirmed
  Rationale: ×ž××©×¨. ×”×§×™×¦×•×¨ "×ž×¢" ×˜×‘×¢×™ ×¢×‘×•×¨ ×ž×¢×¨×‘ ×•×ž×•× ×¢ ×”×ª× ×’×©×•×ª ×™×©×™×¨×” ×¢× ×§×™×¦×•×¨ ×ž×–×¨×—.
  Evidence: src/Humanizer/Properties/Resources.he.resx: E_Short='×ž' and W_Short='×ž'; tests/Humanizer.Tests/Localisation/he/HeadingTests.cs: 90Â° and 270Â° both expect '×ž'
  Notes: ×ž××©×¨ ××ª ×”×–×•×’ E/W ×›×‘×¡×™×¡ ×œ×›×œ ×§×™×¦×•×¨×™ ×”×›×™×•×•× ×™×.
- [hr] `TimeSpanHumanize_Zero`
  Original: bez podatka o vremenu -> bez vremena
  Final: bez vremena
  Decision: confirmed
  Rationale: Nalaz potvrđujem: "bez podatka o vremenu" označava nedostatak informacije, a ne nulto trajanje. Za "no time" prirodniji i semantički točniji izraz u ovom kontekstu je "bez vremena".
  Evidence: src/Humanizer/Properties/Resources.resx: TimeSpanHumanize_Zero = "no time"; src/Humanizer/Properties/Resources.hr.resx: TimeSpanHumanize_Zero = "bez podatka o vremenu"; Semantička usporedba: "bez podatka" = nedostajući podatak, ne nulta količina vremena.
  Notes: Drugi prolaz (neovisna procjena izvornog govornika): potvrđena potreba za terminološkim usklađenjem.
- [hu] `DateHumanize_TwoDaysAgo`
  Original: két éve -> tegnapelőtt
  Final: tegnapelőtt
  Decision: confirmed
  Rationale: The current value means "two years ago", which is a semantic error for the TwoDays key. "tegnapelőtt" is the standard native Hungarian form for "the day before yesterday" and is correct here.
  Evidence: src/Humanizer/Properties/Resources.hu.resx: DateHumanize_TwoDaysAgo = "két éve"; src/Humanizer/Properties/Resources.resx: DateHumanize_TwoDaysAgo = "two days ago"; tests/Humanizer.Tests/Localisation/hu/DateHumanizeTests.cs: DaysAgo covers 1 and 10 days, no explicit 2-day assertion
  Notes: Second-pass independent native review confirms a day/year unit swap defect.
- [hu] `DateHumanize_TwoDaysFromNow`
  Original: két év múlva -> holnapután
  Final: holnapután
  Decision: confirmed
  Rationale: The current value means "in two years", which is semantically wrong for the TwoDays key. "holnapután" is the natural native Hungarian equivalent of "the day after tomorrow".
  Evidence: src/Humanizer/Properties/Resources.hu.resx: DateHumanize_TwoDaysFromNow = "két év múlva"; src/Humanizer/Properties/Resources.resx: DateHumanize_TwoDaysFromNow = "two days from now"; tests/Humanizer.Tests/Localisation/hu/DateHumanizeTests.cs: DaysFromNow covers 1 and 10 days, no explicit 2-day assertion
  Notes: Second-pass independent native review confirms a day/year unit swap defect.
- [hy] `TimeSpanHumanize_Zero`
  Original: ÕªÕ¡Õ´Õ¡Õ¶Õ¡Õ¯Õ¨ Õ¢Õ¡ÖÕ¡Õ¯Õ¡ÕµÕ¸Ö‚Õ´ Õ§ -> ÕªÕ¡Õ´Õ¡Õ¶Õ¡Õ¯ Õ¹Õ¯Õ¡
  Final: ÕªÕ¡Õ´Õ¡Õ¶Õ¡Õ¯ Õ¹Õ¯Õ¡
  Decision: confirmed
  Rationale: ÔµÖ€Õ¯Ö€Õ¸Ö€Õ¤ Õ¡Õ¶Õ¯Õ¡Õ­ Õ´Õ¡ÕµÖ€Õ¥Õ¶Õ« Õ½Õ¿Õ¸Ö‚Õ£Õ´Õ¡Õ´Õ¢ Õ¡Õ¼Õ¡Õ»Õ«Õ¶-pass Õ£Õ¶Õ¡Õ°Õ¡Õ¿Õ¡Õ¯Õ¡Õ¶Õ¨ Õ°Õ¡Õ½Õ¿Õ¡Õ¿Õ¾Õ¸Ö‚Õ´ Õ§. Â«ÕªÕ¡Õ´Õ¡Õ¶Õ¡Õ¯Õ¨ Õ¢Õ¡ÖÕ¡Õ¯Õ¡ÕµÕ¸Ö‚Õ´ Õ§Â» Õ°Õ¡ÕµÕ¥Ö€Õ¥Õ¶Õ¸Ö‚Õ´ Õ¸Õ¹ Õ¢Õ¶Õ¡Õ¯Õ¡Õ¶, Õ¢Õ¡Õ¼Õ¡ÖÕ« Õ±Ö‡Õ¡Õ¯Õ¥Ö€ÕºÕ¸Ö‚Õ´ Õ§, Õ«Õ½Õ¯ Â«ÕªÕ¡Õ´Õ¡Õ¶Õ¡Õ¯ Õ¹Õ¯Õ¡Â»-Õ¶ idiomatic Ö‡ Ö…Õ£Õ¿Õ¡Õ£Õ¸Ö€Õ®Õ¡Õ¯Õ¡Õ¶ Õ³Õ«Õ·Õ¿ Õ¿Õ¡Ö€Õ¢Õ¥Ö€Õ¡Õ¯Õ¶ Õ§ TimeSpanHumanize_Zero-Õ« Õ°Õ¡Õ´Õ¡Ö€Ö‰
  Evidence: src/Humanizer/Properties/Resources.hy.resx:267-268 currently sets TimeSpanHumanize_Zero to "ÕªÕ¡Õ´Õ¡Õ¶Õ¡Õ¯Õ¨ Õ¢Õ¡ÖÕ¡Õ¯Õ¡ÕµÕ¸Ö‚Õ´ Õ§", which is a literal calque rather than natural Armenian usage.; tests/Humanizer.Tests/Localisation/hy/TimeSpanHumanizeTests.cs:128 expects the same phrase in NoTimeToWords, confirming the awkward wording is baked into current behavior.; Native usage check: Armenian expresses "no time" as absence with "Õ¹Õ¯Õ¡"; "ÕªÕ¡Õ´Õ¡Õ¶Õ¡Õ¯ Õ¹Õ¯Õ¡" is the standard natural form.
  Notes: Second-pass decision: first-pass finding and proposed replacement are confirmed without modification.
- [is] `DateHumanize_MultipleSecondsAgo_Plural`
  Original: fyrir {0} sekúndu -> fyrir {0} sekúndum
  Final: fyrir {0} sekúndum
  Decision: confirmed
  Rationale: Staðfest. Fyrir fleirtölu í framsetningu með forsetningunni "fyrir" þarf þágufall fleirtölu: "sekúndum". Núverandi strengur "sekúndu" er eintala og passar ekki við lykilinn _Plural né hegðun í prófum.
  Evidence: src/Humanizer/Properties/Resources.is.resx (DateHumanize_MultipleSecondsAgo = "fyrir {0} sekúndum").; tests/Humanizer.Tests/Localisation/is/DateHumanizeTests.cs (SecondsAgo staðfestir "fyrir 2 sekúndum").
  Notes: Engin breyting á tillögu fyrstu yfirferðar.
- [is] `TimeSpanHumanize_MultipleMonths_Singular`
  Original: {0} mánuðir -> {0} mánuður
  Final: {0} mánuður
  Decision: confirmed
  Rationale: Staðfest. Singular-lykillinn þarf að skila eintölu, "mánuður". Núverandi gildi "{0} mánuðir" er fleirtala og veldur röngu úttaki fyrir 1.
  Evidence: src/Humanizer/Properties/Resources.is.resx (TimeSpanHumanize_MultipleMonths_Singular er skráð sem "{0} mánuðir").; tests/Humanizer.Tests/Localisation/is/TimeSpanHumanizeTests.cs (Months fyrir 31 daga væntir "1 mánuður").
  Notes: Engin breyting á tillögu fyrstu yfirferðar.
- [it] `DataUnit_Byte`
  Original: bytes -> byte
  Final: byte
  Decision: confirmed
  Rationale: Confermato: in italiano d'uso tecnico corrente il prestito 'byte' e invariabile; il plurale inglese 'bytes' risulta innaturale in output localizzato.
  Evidence: src/Humanizer/Properties/Resources.it.resx (DataUnit_Byte): valore corrente 'bytes'.; tests/Humanizer.Tests/Localisation/it/Bytes/ToFullWordsTests.cs (ReturnsPluralBytes): aspettativa corrente '10 bytes'.
  Notes: Confermato senza modifiche: sostituire con 'byte' e mantenere coerenza con il singolare gia presente.
- [it] `DataUnit_Gigabyte`
  Original: gigabytes -> gigabyte
  Final: gigabyte
  Decision: confirmed
  Rationale: Confermato: nel registro tecnico italiano 'gigabyte' si usa normalmente invariabile; 'gigabytes' e un anglismo morfologico non necessario.
  Evidence: src/Humanizer/Properties/Resources.it.resx (DataUnit_Gigabyte): valore corrente 'gigabytes'.; tests/Humanizer.Tests/Localisation/it/Bytes/ToFullWordsTests.cs (ReturnsPluralGigabytes): aspettativa corrente '10 gigabytes'.
  Notes: Confermato senza modifiche: usare 'gigabyte' anche al plurale per output naturale e coerente.
- [it] `TimeSpanHumanize_Age`
  Original: {0} vecchio -> {0} anni
  Final: {0} anni
  Decision: confirmed
  Rationale: Confermato: '{0} vecchio' non esprime correttamente l'eta della persona e suona scorretto in italiano standard; la forma naturale per age humanization e '{0} anni'.
  Evidence: src/Humanizer/Properties/Resources.it.resx (TimeSpanHumanize_Age): valore corrente '{0} vecchio'.; tests/Humanizer.Tests/Localisation/it/ResourcesTests.cs (HasExplicitResidualResources): conferma dell'attuale valore residuo.
  Notes: Confermato senza modifiche: priorita alta perche tocca una stringa comune e percepibilmente non nativa.
- [it] `TimeSpanHumanize_SingleHour_Words`
  Original: una ora -> un'ora
  Final: un'ora
  Decision: confirmed
  Rationale: Confermato: la forma preferita e standard in italiano contemporaneo e 'un'ora'; 'una ora' resta comprensibile ma marcata e meno naturale.
  Evidence: src/Humanizer/Properties/Resources.it.resx (TimeSpanHumanize_SingleHour_Words): valore corrente 'una ora'.; tests/Humanizer.Tests/Localisation/it/TimeSpanHumanizeTests.cs (Hours con toWords=true): aspettativa corrente 'una ora'.
  Notes: Confermato senza modifiche di severita: miglioramento stilistico/ortografico consigliato.
- [ko] `DateHumanize_Never`
  Original: 사용 안 함 -> 없음
  Final: 없음
  Decision: confirmed
  Rationale: Second-pass native adjudication confirms this defect. "사용 안 함" means "disabled/turned off" and reads like a UI toggle state, not the temporal meaning of "never" used by nullable Date/Time humanization. "없음" is the most natural standalone Korean output for this API context and preserves expected null-date semantics.
  Evidence: src/Humanizer/Properties/Resources.ko.resx: DateHumanize_Never is currently "사용 안 함".; src/Humanizer/DateHumanizeExtensions.cs: nullable Humanize overloads return DateHumanize_Never when the value is null.; tests/Humanizer.Tests/Localisation/ko-KR/DateHumanizeTests.cs: no assertion currently covers DateHumanize_Never, so this mistranslation is not blocked by ko-KR tests.
  Notes: Independent second-pass Korean native review confirms first-pass replacement without modification.
- [lv] `DateHumanize_SingleMinuteFromNow`
  Original: pÄ“c minÅ«tÄ“s -> pēc minūtes
  Final: pēc minūtes
  Decision: confirmed
  Rationale: Confirmed. Latvian requires genitive singular after the preposition "pēc"; "minūtēs" is locative plural and ungrammatical in this construction. The correct fixed form is "pēc minūtes", consistent with the existing "pirms minūtes" pattern.
  Evidence: src/Humanizer/Properties/Resources.lv.resx:316 (DateHumanize_SingleMinuteFromNow is currently "pēc minūtēs"); src/Humanizer/Properties/Resources.lv.resx:313 (DateHumanize_SingleMinuteAgo uses "pirms minūtes", showing the expected singular genitive pattern); tests/Humanizer.Tests/Localisation/lv/DateHumanizeTests.cs:6 (Latvian date humanization relies on explicit locale resources)
  Notes: First-pass replacement is correct; no modification needed.
- [ms] `DateHumanize_MultipleYearsAgo_Paucal`
  Original: {0} tahun dari yang lalu -> {0} tahun yang lalu
  Final: {0} tahun yang lalu
  Decision: confirmed
  Rationale: Sebagai penutur jati Melayu, frasa "{0} tahun dari yang lalu" tidak gramatis dan tidak digunakan secara natural. Untuk makna "{0} years ago", bentuk baku yang tepat ialah "{0} tahun yang lalu".
  Evidence: src/Humanizer/Properties/Resources.ms.resx:306-319 menunjukkan siri DateHumanize_MultipleYearsAgo menggunakan pola "yang lalu"; hanya entri Paucal menyimpang kepada "dari yang lalu".; src/Humanizer/Properties/Resources.ms.resx:379 menggunakan "setahun yang lalu", mengesahkan pola tatabahasa lampau yang konsisten dalam locale ms.; tests/Humanizer.Tests/Localisation/ms-MY/DateHumanizeTests.cs menunjukkan ms-MY mengambil resource ms; isu dalam resource induk ms akan turut terwaris ke ms-MY.
  Notes: Keputusan first-pass dikekalkan tanpa perubahan pada status, keterukan, atau cadangan penggantian.
- [mt] `N`
  Original: XL -> tramuntana
  Final: tramuntana
  Decision: confirmed
  Rationale: Nikkonferma d-difett: fil-Malti, il-punt kardinali N gÄ§andu jkun "tramuntana". "XL" hija abbrevjazzjoni ta' "xlokk" (SE) u ma tistax tintuÅ¼a bÄ§ala forma sÄ§iÄ§a ta' N.
  Evidence: src/Humanizer/Properties/Resources.mt.resx: "N" huwa "XL" (linji 402-404) filwaqt li "N_Short" huwa "T" (linji 429-431), jiÄ¡ifieri l-key sÄ§iÄ§ qed juÅ¼a forma Ä§aÅ¼ina.; tests/Humanizer.Tests/Localisation/mt/HeadingTests.cs: fit-test ToHeading, 0Â° jistenna "XL" fil-full style, li juri propagazzjoni diretta tal-valur Å¼baljat tar-riÅ¼orsa.
  Notes: Second-pass independent native adjudication: confirmed.
- [mt] `TimeSpanHumanize_MultipleYears_Singular`
  Original: {0} snin -> {0} sena
  Final: {0} sena
  Decision: confirmed
  Rationale: Nikkonferma d-difett morfoloÄ¡iku: il-key singulari trid forma nominali singulari. "{0} snin" hija plural, waqt li l-forma korretta f'dan il-kuntest hija "{0} sena".
  Evidence: src/Humanizer/Properties/Resources.mt.resx: "TimeSpanHumanize_MultipleYears_Singular" hu "{0} snin" (linji 579-580), li hu plural minkejja l-kategorija singular.; src/Humanizer/Properties/Resources.mt.resx + tests/Humanizer.Tests/Localisation/mt/TimeSpanHumanizeTests.cs: "TimeSpanHumanize_SingleYear" hu "sena" u t-testijiet ta' sena waÄ§da jistennew "sena", li jappoÄ¡Ä¡ja "{0} sena" gÄ§as-singular numeriku.
  Notes: Second-pass independent native adjudication: confirmed.
- [nb] `DataUnit_Byte`
  Original: byte -> byte
  Final: byte
  Decision: confirmed
  Rationale: Bekreftet. Som bokmålsskribent er "byte" korrekt grunnform, men engelsk flertall "bytes" i norsk UI er unaturlig. Forventet norsk form er uten engelsk -s-ending.
  Evidence: src/Humanizer/Properties/Resources.nb.resx:330 sets DataUnit_Byte to "byte".; tests/Humanizer.Tests/Localisation/nb/Bytes/ToFullWordsTests.cs:11 expects "10 bytes".; tests/Humanizer.Tests/Localisation/nb/Bytes/ToFullWordsTests.cs:19 also expects "10 gigabytes", which indicates English pluralization path.
  Notes: Feilen ligger i pluraliseringslogikken for ByteSize.ToFullWords for nb, ikke i selve ressursverdien.
- [nb] `DataUnit_Gigabyte`
  Original: gigabyte -> gigabyte
  Final: gigabyte
  Decision: confirmed
  Rationale: Bekreftet. "Gigabyte" er riktig bokmålsform både i singular og som vanlig teknisk flertall i norsk; "gigabytes" med engelsk -s er ikke naturlig i nb.
  Evidence: src/Humanizer/Properties/Resources.nb.resx:336 sets DataUnit_Gigabyte to "gigabyte".; tests/Humanizer.Tests/Localisation/nb/Bytes/ToFullWordsTests.cs:19 expects "10 gigabytes".
  Notes: Samme underliggende problem som DataUnit_Byte: engelsk bøying i stedet for nb-tilpasset pluralisering.
- [pt-BR] `TimeSpanHumanize_MultipleMilliseconds`
  Original: {0} milisegundos -> {0} milissegundos
  Final: {0} milissegundos
  Decision: confirmed
  Rationale: Confirmado. A forma padrÃ£o em pt-BR Ã© â€œmilissegundo(s)â€; â€œmilisegundo(s)â€ Ã© erro ortogrÃ¡fico e aparece no output pÃºblico.
  Evidence: src/Humanizer/Properties/Resources.pt-BR.resx (TimeSpanHumanize_MultipleMilliseconds = "{0} milisegundos"); tests/Humanizer.Tests/Localisation/pt-BR/TimeSpanHumanizeTests.cs (assert atual: "2 milisegundos")
  Notes: CorreÃ§Ã£o ortogrÃ¡fica direta, sem impacto de formato/placeholders.
- [pt-BR] `TimeSpanHumanize_MultipleMilliseconds_Dual`
  Original: {0} milisegundos -> {0} milissegundos
  Final: {0} milissegundos
  Decision: confirmed
  Rationale: Confirmado. A variante dual deve manter a mesma ortografia correta usada nas demais formas: â€œmilissegundosâ€.
  Evidence: src/Humanizer/Properties/Resources.pt-BR.resx (TimeSpanHumanize_MultipleMilliseconds_Dual = "{0} milisegundos"); FamÃ­lia TimeSpanHumanize exige consistÃªncia lexical entre variantes gramaticais.
  Notes: Ajuste de consistÃªncia ortogrÃ¡fica dentro da famÃ­lia de chaves.
- [pt-BR] `TimeSpanHumanize_MultipleMilliseconds_Paucal`
  Original: {0} milisegundos -> {0} milissegundos
  Final: {0} milissegundos
  Decision: confirmed
  Rationale: Confirmado. â€œMilissegundosâ€ Ã© a Ãºnica grafia ortograficamente correta em pt-BR tambÃ©m para a forma paucal.
  Evidence: src/Humanizer/Properties/Resources.pt-BR.resx (TimeSpanHumanize_MultipleMilliseconds_Paucal = "{0} milisegundos"); Norma ortogrÃ¡fica do portuguÃªs brasileiro: prefixo + â€œsegundoâ€ resulta em â€œmilissegundoâ€.
  Notes: Sem mudanÃ§a semÃ¢ntica; apenas correÃ§Ã£o linguÃ­stica.
- [pt-BR] `TimeSpanHumanize_MultipleMilliseconds_Plural`
  Original: {0} milisegundos -> {0} milissegundos
  Final: {0} milissegundos
  Decision: confirmed
  Rationale: Confirmado. Forma plural atual contÃ©m erro ortogrÃ¡fico visÃ­vel ao usuÃ¡rio.
  Evidence: src/Humanizer/Properties/Resources.pt-BR.resx (TimeSpanHumanize_MultipleMilliseconds_Plural = "{0} milisegundos"); tests/Humanizer.Tests/Localisation/pt-BR/TimeSpanHumanizeTests.cs (assert atual de plural com â€œmilisegundosâ€)
  Notes: Deve acompanhar atualizaÃ§Ã£o dos testes de localizaÃ§Ã£o pt-BR.
- [pt-BR] `TimeSpanHumanize_MultipleMilliseconds_Singular`
  Original: {0} milisegundo -> {0} milissegundo
  Final: {0} milissegundo
  Decision: confirmed
  Rationale: Confirmado. A forma singular correta Ã© â€œmilissegundoâ€; a grafia atual com um sÃ³ â€œsâ€ estÃ¡ incorreta.
  Evidence: src/Humanizer/Properties/Resources.pt-BR.resx (TimeSpanHumanize_MultipleMilliseconds_Singular = "{0} milisegundo"); ConsistÃªncia com TimeUnit_Millisecond abreviado como â€œmsâ€ e termo lexical correspondente.
  Notes: CorreÃ§Ã£o pontual de ortografia na forma singular.
- [pt-BR] `TimeSpanHumanize_SingleMillisecond`
  Original: 1 milisegundo -> 1 milissegundo
  Final: 1 milissegundo
  Decision: confirmed
  Rationale: Confirmado. String de unidade no singular numÃ©rico deve usar â€œmilissegundoâ€.
  Evidence: src/Humanizer/Properties/Resources.pt-BR.resx (TimeSpanHumanize_SingleMillisecond = "1 milisegundo"); tests/Humanizer.Tests/Localisation/pt-BR/TimeSpanHumanizeTests.cs (assert atual: "1 milisegundo")
  Notes: Alinhar com a forma correta e com os demais recursos de milissegundo.
- [pt-BR] `TimeSpanHumanize_SingleMillisecond_Words`
  Original: um milisegundo -> um milissegundo
  Final: um milissegundo
  Decision: confirmed
  Rationale: Confirmado. Na forma por extenso, a ortografia correta permanece â€œmilissegundoâ€.
  Evidence: src/Humanizer/Properties/Resources.pt-BR.resx (TimeSpanHumanize_SingleMillisecond_Words = "um milisegundo"); PadronizaÃ§Ã£o ortogrÃ¡fica em todas as chaves TimeSpanHumanize relacionadas a millisecond.
  Notes: CorreÃ§Ã£o ortogrÃ¡fica sem alterar gÃªnero/nÃºmero.
- [pt-BR] `TimeSpanHumanize_Zero`
  Original: sem horÃ¡rio -> sem tempo
  Final: sem tempo
  Decision: confirmed
  Rationale: Confirmado. Para TimeSpan.Zero em `toWords`, â€œsem tempoâ€ Ã© natural e semanticamente correto; â€œsem horÃ¡rioâ€ remete a ausÃªncia de agenda/horÃ¡rio marcado.
  Evidence: src/Humanizer/Properties/Resources.pt-BR.resx (TimeSpanHumanize_Zero = "sem horÃ¡rio"); tests/Humanizer.Tests/Localisation/pt-BR/TimeSpanHumanizeTests.cs (NoTimeToWords espera "sem horÃ¡rio")
  Notes: Ajuste melhora adequaÃ§Ã£o semÃ¢ntica do texto exibido ao usuÃ¡rio.
- [sr] `DateHumanize_SingleSecondAgo`
  Original: пре секунд -> пре секунде
  Final: пре секунде
  Decision: confirmed
  Rationale: Потврђујем налаз из првог паса: у овом locale-у се једнина за ову јединицу доследно води као "секунда" (нпр. "1 секунда"), па је уз "пре" исправно "пре секунде". Тренутно "пре секунд" је неприродно и падежно неисправно.
  Evidence: src/Humanizer/Properties/Resources.sr.resx:219; src/Humanizer/Properties/Resources.sr.resx:294; tests/Humanizer.Tests/Localisation/sr/DateHumanizeTests.cs:7
  Notes: Задржана је предложена замена из првог паса.
- [sr] `DateHumanize_SingleSecondFromNow`
  Original: за секунд -> за секунду
  Final: за секунду
  Decision: confirmed
  Rationale: Потврђујем налаз из првог паса: у будућој конструкцији са једнином природан и нормативан облик је "за секунду". Тренутно "за секунд" одступа од обрасца који locale већ користи за "секунда".
  Evidence: src/Humanizer/Properties/Resources.sr.resx:222; src/Humanizer/Properties/Resources.sr.resx:294; tests/Humanizer.Tests/Localisation/sr/DateHumanizeTests.cs:13
  Notes: Задржана је предложена замена из првог паса.
- [sv] `DataUnit_Byte`
  Original: bytes -> byte
  Final: byte
  Decision: confirmed
  Rationale: Bekräftat. På idiomatisk svenska används enhetsnamnet utan engelsk plural-s i detta sammanhang: "1 byte", "10 byte". Formen "bytes" är inte naturlig i svensk löptext för datamängder.
  Evidence: src/Humanizer/Properties/Resources.sv.resx:306; src/Humanizer/Properties/Resources.sv.resx:309; tests/Humanizer.Tests/Localisation/sv/Bytes/ToFullWordsTests.cs:12
  Notes: Behåll singular och plural som "byte" i sv-resursen; uppdatera testförväntan från "10 bytes" till "10 byte" när fixen görs.
- [sv] `DataUnit_Gigabyte`
  Original: gigabytes -> gigabyte
  Final: gigabyte
  Decision: confirmed
  Rationale: Bekräftat. I svensk teknisk språkdräkt används normalt samma form för singular och plural här: "1 gigabyte", "10 gigabyte". "Gigabytes" är en direkt engelsk pluralisering.
  Evidence: src/Humanizer/Properties/Resources.sv.resx:315; src/Humanizer/Properties/Resources.sv.resx:318; tests/Humanizer.Tests/Localisation/sv/Bytes/ToFullWordsTests.cs:20
  Notes: Behåll singular och plural som "gigabyte" i sv-resursen; uppdatera testförväntan från "10 gigabytes" till "10 gigabyte" när fixen görs.
- [th] `DateHumanize_MultipleDaysFromNow`
  Original: {0} Ã Â¸Â§Ã Â¸Â±Ã Â¸â„¢Ã Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Â§Ã Â¸Â±Ã Â¸â„¢
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Â§Ã Â¸Â±Ã Â¸â„¢
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleDaysFromNow_Dual`
  Original: {0} Ã Â¸Â§Ã Â¸Â±Ã Â¸â„¢Ã Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Â§Ã Â¸Â±Ã Â¸â„¢
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Â§Ã Â¸Â±Ã Â¸â„¢
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleDaysFromNow_Paucal`
  Original: {0} Ã Â¸Â§Ã Â¸Â±Ã Â¸â„¢Ã Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Â§Ã Â¸Â±Ã Â¸â„¢
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Â§Ã Â¸Â±Ã Â¸â„¢
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleDaysFromNow_Plural`
  Original: {0} Ã Â¸Â§Ã Â¸Â±Ã Â¸â„¢Ã Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Â§Ã Â¸Â±Ã Â¸â„¢
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Â§Ã Â¸Â±Ã Â¸â„¢
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleDaysFromNow_Singular`
  Original: {0} Ã Â¸Â§Ã Â¸Â±Ã Â¸â„¢Ã Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Â§Ã Â¸Â±Ã Â¸â„¢
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Â§Ã Â¸Â±Ã Â¸â„¢
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleHoursFromNow`
  Original: {0} Ã Â¸Å Ã Â¸Â±Ã Â¹Ë†Ã Â¸Â§Ã Â¹â€šÃ Â¸Â¡Ã Â¸â€¡Ã Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Å Ã Â¸Â±Ã Â¹Ë†Ã Â¸Â§Ã Â¹â€šÃ Â¸Â¡Ã Â¸â€¡
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Å Ã Â¸Â±Ã Â¹Ë†Ã Â¸Â§Ã Â¹â€šÃ Â¸Â¡Ã Â¸â€¡
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleHoursFromNow_Dual`
  Original: {0} Ã Â¸Å Ã Â¸Â±Ã Â¹Ë†Ã Â¸Â§Ã Â¹â€šÃ Â¸Â¡Ã Â¸â€¡Ã Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Å Ã Â¸Â±Ã Â¹Ë†Ã Â¸Â§Ã Â¹â€šÃ Â¸Â¡Ã Â¸â€¡
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Å Ã Â¸Â±Ã Â¹Ë†Ã Â¸Â§Ã Â¹â€šÃ Â¸Â¡Ã Â¸â€¡
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleHoursFromNow_Paucal`
  Original: {0} Ã Â¸Å Ã Â¸Â±Ã Â¹Ë†Ã Â¸Â§Ã Â¹â€šÃ Â¸Â¡Ã Â¸â€¡Ã Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Å Ã Â¸Â±Ã Â¹Ë†Ã Â¸Â§Ã Â¹â€šÃ Â¸Â¡Ã Â¸â€¡
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Å Ã Â¸Â±Ã Â¹Ë†Ã Â¸Â§Ã Â¹â€šÃ Â¸Â¡Ã Â¸â€¡
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleHoursFromNow_Plural`
  Original: {0} Ã Â¸Å Ã Â¸Â±Ã Â¹Ë†Ã Â¸Â§Ã Â¹â€šÃ Â¸Â¡Ã Â¸â€¡Ã Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Å Ã Â¸Â±Ã Â¹Ë†Ã Â¸Â§Ã Â¹â€šÃ Â¸Â¡Ã Â¸â€¡
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Å Ã Â¸Â±Ã Â¹Ë†Ã Â¸Â§Ã Â¹â€šÃ Â¸Â¡Ã Â¸â€¡
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleHoursFromNow_Singular`
  Original: {0} Ã Â¸Å Ã Â¸Â±Ã Â¹Ë†Ã Â¸Â§Ã Â¹â€šÃ Â¸Â¡Ã Â¸â€¡Ã Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Å Ã Â¸Â±Ã Â¹Ë†Ã Â¸Â§Ã Â¹â€šÃ Â¸Â¡Ã Â¸â€¡
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Å Ã Â¸Â±Ã Â¹Ë†Ã Â¸Â§Ã Â¹â€šÃ Â¸Â¡Ã Â¸â€¡
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleMinutesFromNow`
  Original: {0} Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸ÂµÃ Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleMinutesFromNow_Dual`
  Original: {0} Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸ÂµÃ Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleMinutesFromNow_Paucal`
  Original: {0} Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸ÂµÃ Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleMinutesFromNow_Plural`
  Original: {0} Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸ÂµÃ Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleMinutesFromNow_Singular`
  Original: {0} Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸ÂµÃ Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleMonthsFromNow`
  Original: {0} Ã Â¹â‚¬Ã Â¸â€Ã Â¸Â·Ã Â¸Â­Ã Â¸â„¢Ã Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¹â‚¬Ã Â¸â€Ã Â¸Â·Ã Â¸Â­Ã Â¸â„¢
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¹â‚¬Ã Â¸â€Ã Â¸Â·Ã Â¸Â­Ã Â¸â„¢
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleMonthsFromNow_Dual`
  Original: {0} Ã Â¹â‚¬Ã Â¸â€Ã Â¸Â·Ã Â¸Â­Ã Â¸â„¢Ã Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¹â‚¬Ã Â¸â€Ã Â¸Â·Ã Â¸Â­Ã Â¸â„¢
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¹â‚¬Ã Â¸â€Ã Â¸Â·Ã Â¸Â­Ã Â¸â„¢
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleMonthsFromNow_Paucal`
  Original: {0} Ã Â¹â‚¬Ã Â¸â€Ã Â¸Â·Ã Â¸Â­Ã Â¸â„¢Ã Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¹â‚¬Ã Â¸â€Ã Â¸Â·Ã Â¸Â­Ã Â¸â„¢
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¹â‚¬Ã Â¸â€Ã Â¸Â·Ã Â¸Â­Ã Â¸â„¢
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleMonthsFromNow_Plural`
  Original: {0} Ã Â¹â‚¬Ã Â¸â€Ã Â¸Â·Ã Â¸Â­Ã Â¸â„¢Ã Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¹â‚¬Ã Â¸â€Ã Â¸Â·Ã Â¸Â­Ã Â¸â„¢
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¹â‚¬Ã Â¸â€Ã Â¸Â·Ã Â¸Â­Ã Â¸â„¢
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleMonthsFromNow_Singular`
  Original: {0} Ã Â¹â‚¬Ã Â¸â€Ã Â¸Â·Ã Â¸Â­Ã Â¸â„¢Ã Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¹â‚¬Ã Â¸â€Ã Â¸Â·Ã Â¸Â­Ã Â¸â„¢
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¹â‚¬Ã Â¸â€Ã Â¸Â·Ã Â¸Â­Ã Â¸â„¢
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleSecondsFromNow`
  Original: {0} Ã Â¸Â§Ã Â¸Â´Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸ÂµÃ Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Â§Ã Â¸Â´Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Â§Ã Â¸Â´Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleSecondsFromNow_Dual`
  Original: {0} Ã Â¸Â§Ã Â¸Â´Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸ÂµÃ Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Â§Ã Â¸Â´Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Â§Ã Â¸Â´Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleSecondsFromNow_Paucal`
  Original: {0} Ã Â¸Â§Ã Â¸Â´Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸ÂµÃ Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Â§Ã Â¸Â´Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Â§Ã Â¸Â´Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleSecondsFromNow_Plural`
  Original: {0} Ã Â¸Â§Ã Â¸Â´Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸ÂµÃ Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Â§Ã Â¸Â´Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Â§Ã Â¸Â´Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleSecondsFromNow_Singular`
  Original: {0} Ã Â¸Â§Ã Â¸Â´Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸ÂµÃ Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Â§Ã Â¸Â´Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸Â§Ã Â¸Â´Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleYearsFromNow`
  Original: {0} Ã Â¸â€ºÃ Â¸ÂµÃ Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸â€ºÃ Â¸Âµ
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸â€ºÃ Â¸Âµ
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleYearsFromNow_Dual`
  Original: {0} Ã Â¸â€ºÃ Â¸ÂµÃ Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸â€ºÃ Â¸Âµ
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸â€ºÃ Â¸Âµ
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleYearsFromNow_Paucal`
  Original: {0} Ã Â¸â€ºÃ Â¸ÂµÃ Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸â€ºÃ Â¸Âµ
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸â€ºÃ Â¸Âµ
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleYearsFromNow_Plural`
  Original: {0} Ã Â¸â€ºÃ Â¸ÂµÃ Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸â€ºÃ Â¸Âµ
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸â€ºÃ Â¸Âµ
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_MultipleYearsFromNow_Singular`
  Original: {0} Ã Â¸â€ºÃ Â¸ÂµÃ Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸â€ºÃ Â¸Âµ
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸Â {0} Ã Â¸â€ºÃ Â¸Âµ
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_SingleHourFromNow`
  Original: Ã Â¸Â«Ã Â¸â„¢Ã Â¸Â¶Ã Â¹Ë†Ã Â¸â€¡Ã Â¸Å Ã Â¸Â±Ã Â¹Ë†Ã Â¸Â§Ã Â¹â€šÃ Â¸Â¡Ã Â¸â€¡Ã Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸ÂÃ Â¸Â«Ã Â¸â„¢Ã Â¸Â¶Ã Â¹Ë†Ã Â¸â€¡Ã Â¸Å Ã Â¸Â±Ã Â¹Ë†Ã Â¸Â§Ã Â¹â€šÃ Â¸Â¡Ã Â¸â€¡
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸ÂÃ Â¸Â«Ã Â¸â„¢Ã Â¸Â¶Ã Â¹Ë†Ã Â¸â€¡Ã Â¸Å Ã Â¸Â±Ã Â¹Ë†Ã Â¸Â§Ã Â¹â€šÃ Â¸Â¡Ã Â¸â€¡
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_SingleMinuteFromNow`
  Original: Ã Â¸Â«Ã Â¸â„¢Ã Â¸Â¶Ã Â¹Ë†Ã Â¸â€¡Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸ÂµÃ Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸ÂÃ Â¸Â«Ã Â¸â„¢Ã Â¸Â¶Ã Â¹Ë†Ã Â¸â€¡Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸ÂÃ Â¸Â«Ã Â¸â„¢Ã Â¸Â¶Ã Â¹Ë†Ã Â¸â€¡Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_SingleMonthFromNow`
  Original: Ã Â¸Â«Ã Â¸â„¢Ã Â¸Â¶Ã Â¹Ë†Ã Â¸â€¡Ã Â¹â‚¬Ã Â¸â€Ã Â¸Â·Ã Â¸Â­Ã Â¸â„¢Ã Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸ÂÃ Â¸Â«Ã Â¸â„¢Ã Â¸Â¶Ã Â¹Ë†Ã Â¸â€¡Ã Â¹â‚¬Ã Â¸â€Ã Â¸Â·Ã Â¸Â­Ã Â¸â„¢
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸ÂÃ Â¸Â«Ã Â¸â„¢Ã Â¸Â¶Ã Â¹Ë†Ã Â¸â€¡Ã Â¹â‚¬Ã Â¸â€Ã Â¸Â·Ã Â¸Â­Ã Â¸â„¢
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_SingleSecondFromNow`
  Original: Ã Â¸Â«Ã Â¸â„¢Ã Â¸Â¶Ã Â¹Ë†Ã Â¸â€¡Ã Â¸Â§Ã Â¸Â´Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸ÂµÃ Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸ÂÃ Â¸Â«Ã Â¸â„¢Ã Â¸Â¶Ã Â¹Ë†Ã Â¸â€¡Ã Â¸Â§Ã Â¸Â´Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸ÂÃ Â¸Â«Ã Â¸â„¢Ã Â¸Â¶Ã Â¹Ë†Ã Â¸â€¡Ã Â¸Â§Ã Â¸Â´Ã Â¸â„¢Ã Â¸Â²Ã Â¸â€”Ã Â¸Âµ
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [th] `DateHumanize_SingleYearFromNow`
  Original: Ã Â¸Â«Ã Â¸â„¢Ã Â¸Â¶Ã Â¹Ë†Ã Â¸â€¡Ã Â¸â€ºÃ Â¸ÂµÃ Â¸Ë†Ã Â¸Â²Ã Â¸ÂÃ Â¸â„¢Ã Â¸ÂµÃ Â¹â€° -> Ã Â¸Â­Ã Â¸ÂµÃ Â¸ÂÃ Â¸Â«Ã Â¸â„¢Ã Â¸Â¶Ã Â¹Ë†Ã Â¸â€¡Ã Â¸â€ºÃ Â¸Âµ
  Final: Ã Â¸Â­Ã Â¸ÂµÃ Â¸ÂÃ Â¸Â«Ã Â¸â„¢Ã Â¸Â¶Ã Â¹Ë†Ã Â¸â€¡Ã Â¸â€ºÃ Â¸Âµ
  Decision: confirmed
  Rationale: à¸¢à¸·à¸™à¸¢à¸±à¸™à¸‚à¹‰à¸­à¸ªà¸£à¸¸à¸›à¸„à¸£à¸±à¹‰à¸‡à¹à¸£à¸: à¸ªà¸³à¸™à¸§à¸™ "...à¸ˆà¸²à¸à¸™à¸µà¹‰" à¹„à¸¡à¹ˆà¹€à¸›à¹‡à¸™à¸˜à¸£à¸£à¸¡à¸Šà¸²à¸•à¸´à¸ªà¸³à¸«à¸£à¸±à¸šà¸à¸²à¸£à¸šà¸­à¸à¹€à¸§à¸¥à¸²à¹€à¸Šà¸´à¸‡à¸ªà¸±à¸¡à¸žà¸±à¸—à¸˜à¹Œà¹ƒà¸™à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ à¹à¸¥à¸°à¸„à¸§à¸£à¹ƒà¸Šà¹‰à¹‚à¸„à¸£à¸‡à¸ªà¸£à¹‰à¸²à¸‡ "à¸­à¸µà¸ ..."
  Evidence: src/Humanizer/Properties/Resources.th.resx: à¸„à¹ˆà¸²à¸à¸¥à¸¸à¹ˆà¸¡ DateHumanize_*FromNow à¸›à¸±à¸ˆà¸ˆà¸¸à¸šà¸±à¸™à¹ƒà¸Šà¹‰à¸£à¸¹à¸› "...à¸ˆà¸²à¸à¸™à¸µà¹‰"; tests/Humanizer.Tests/Localisation/th-TH/DateHumanizeTests.cs: à¸¡à¸µà¸à¸²à¸£à¸—à¸”à¸ªà¸­à¸šà¸—à¸£à¸±à¸žà¸¢à¸²à¸à¸£à¹€à¸‰à¸žà¸²à¸° th-TH à¹à¸¥à¸°à¸žà¸¤à¸•à¸´à¸à¸£à¸£à¸¡ fallback à¸ˆà¸²à¸ parent locale
  Notes: Second-pass (native): confirmed first-pass defect and replacement.
- [uz-Cyrl-UZ] `DateHumanize_MultipleMinutesAgo`
  Original: {0} Ð¼Ð¸Ð½ÑƒÑ‚ Ð°Ð²Ð²Ð°Ð» -> {0} Ð´Ð°Ò›Ð¸Ò›Ð° Ð°Ð²Ð²Ð°Ð»
  Final: {0} Ð´Ð°Ò›Ð¸Ò›Ð° Ð°Ð²Ð²Ð°Ð»
  Decision: confirmed
  Rationale: Confirmed: in uz-Cyrl-UZ, the localeâ€™s own baseline for minute is "Ð´Ð°Ò›Ð¸Ò›Ð°" (including singular DateHumanize and TimeUnit). Keeping "Ð¼Ð¸Ð½ÑƒÑ‚" in this plural form is an avoidable register mismatch.
  Evidence: src/Humanizer/Properties/Resources.uz-Cyrl-UZ.resx: DateHumanize_MultipleMinutesAgo={0} Ð¼Ð¸Ð½ÑƒÑ‚ Ð°Ð²Ð²Ð°Ð»; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/DateHumanizeTests.cs: MinutesAgo(10) expects "10 Ð¼Ð¸Ð½ÑƒÑ‚ Ð°Ð²Ð²Ð°Ð»"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/DateHumanizeTests.cs: MinutesAgo(1) expects "Ð±Ð¸Ñ€ Ð´Ð°Ò›Ð¸Ò›Ð° Ð°Ð²Ð²Ð°Ð»"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeUnitToSymbolTests.cs: TimeUnit.Minute expects "Ð´Ð°Ò›Ð¸Ò›Ð°"
  Notes: Second-pass independent native review confirms first-pass defect and replacement.
- [uz-Cyrl-UZ] `DateHumanize_MultipleMinutesFromNow`
  Original: {0} Ð¼Ð¸Ð½ÑƒÑ‚Ð´Ð°Ð½ ÑÑžÐ½Ð³ -> {0} Ð´Ð°Ò›Ð¸Ò›Ð°Ð´Ð°Ð½ ÑÑžÐ½Ð³
  Final: {0} Ð´Ð°Ò›Ð¸Ò›Ð°Ð´Ð°Ð½ ÑÑžÐ½Ð³
  Decision: confirmed
  Rationale: Confirmed: this should use the same minute lexeme ("Ð´Ð°Ò›Ð¸Ò›Ð°") already used in the corresponding singular and TimeUnit entries; "Ð¼Ð¸Ð½ÑƒÑ‚Ð´Ð°Ð½" introduces inconsistent style within one locale.
  Evidence: src/Humanizer/Properties/Resources.uz-Cyrl-UZ.resx: DateHumanize_MultipleMinutesFromNow={0} Ð¼Ð¸Ð½ÑƒÑ‚Ð´Ð°Ð½ ÑÑžÐ½Ð³; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/DateHumanizeTests.cs: MinutesFromNow(10) expects "10 Ð¼Ð¸Ð½ÑƒÑ‚Ð´Ð°Ð½ ÑÑžÐ½Ð³"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/DateHumanizeTests.cs: MinutesFromNow(1) expects "Ð±Ð¸Ñ€ Ð´Ð°Ò›Ð¸Ò›Ð°Ð´Ð°Ð½ ÑÑžÐ½Ð³"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeUnitToSymbolTests.cs: TimeUnit.Minute expects "Ð´Ð°Ò›Ð¸Ò›Ð°"
  Notes: Second-pass independent native review confirms first-pass defect and replacement.
- [uz-Cyrl-UZ] `DateHumanize_MultipleSecondsAgo`
  Original: {0} ÑÐµÐºÑƒÐ½Ð´ Ð°Ð²Ð²Ð°Ð» -> {0} ÑÐ¾Ð½Ð¸Ñ Ð°Ð²Ð²Ð°Ð»
  Final: {0} ÑÐ¾Ð½Ð¸Ñ Ð°Ð²Ð²Ð°Ð»
  Decision: confirmed
  Rationale: Confirmed: the locale consistently uses "ÑÐ¾Ð½Ð¸Ñ" for the singular second and TimeUnit; plural past form should match that lexeme to avoid intra-locale terminology drift.
  Evidence: src/Humanizer/Properties/Resources.uz-Cyrl-UZ.resx: DateHumanize_MultipleSecondsAgo={0} ÑÐµÐºÑƒÐ½Ð´ Ð°Ð²Ð²Ð°Ð»; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/DateHumanizeTests.cs: SecondsAgo(10) expects "10 ÑÐµÐºÑƒÐ½Ð´ Ð°Ð²Ð²Ð°Ð»"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/DateHumanizeTests.cs: SecondsAgo(1) expects "Ð±Ð¸Ñ€ ÑÐ¾Ð½Ð¸Ñ Ð°Ð²Ð²Ð°Ð»"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeUnitToSymbolTests.cs: TimeUnit.Second expects "ÑÐ¾Ð½Ð¸Ñ"
  Notes: Second-pass independent native review confirms first-pass defect and replacement.
- [uz-Cyrl-UZ] `DateHumanize_MultipleSecondsFromNow`
  Original: {0} ÑÐµÐºÑƒÐ½Ð´Ð´Ð°Ð½ ÑÑžÐ½Ð³ -> {0} ÑÐ¾Ð½Ð¸ÑÐ´Ð°Ð½ ÑÑžÐ½Ð³
  Final: {0} ÑÐ¾Ð½Ð¸ÑÐ´Ð°Ð½ ÑÑžÐ½Ð³
  Decision: confirmed
  Rationale: Confirmed: "ÑÐ¾Ð½Ð¸ÑÐ´Ð°Ð½" is the internally consistent and natural continuation of the localeâ€™s second terminology; "ÑÐµÐºÑƒÐ½Ð´Ð´Ð°Ð½" is mixed register relative to the rest of this surface.
  Evidence: src/Humanizer/Properties/Resources.uz-Cyrl-UZ.resx: DateHumanize_MultipleSecondsFromNow={0} ÑÐµÐºÑƒÐ½Ð´Ð´Ð°Ð½ ÑÑžÐ½Ð³; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/DateHumanizeTests.cs: SecondsFromNow(10) expects "10 ÑÐµÐºÑƒÐ½Ð´Ð´Ð°Ð½ ÑÑžÐ½Ð³"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/DateHumanizeTests.cs: SecondsFromNow(1) expects "Ð±Ð¸Ñ€ ÑÐ¾Ð½Ð¸ÑÐ´Ð°Ð½ ÑÑžÐ½Ð³"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeUnitToSymbolTests.cs: TimeUnit.Second expects "ÑÐ¾Ð½Ð¸Ñ"
  Notes: Second-pass independent native review confirms first-pass defect and replacement.
- [uz-Cyrl-UZ] `TimeSpanHumanize_MultipleMinutes`
  Original: {0} Ð¼Ð¸Ð½ÑƒÑ‚ -> {0} Ð´Ð°Ò›Ð¸Ò›Ð°
  Final: {0} Ð´Ð°Ò›Ð¸Ò›Ð°
  Decision: confirmed
  Rationale: Confirmed: TimeSpan outputs should align with the localeâ€™s core time unit naming; using "Ð¼Ð¸Ð½ÑƒÑ‚" here conflicts with "Ð´Ð°Ò›Ð¸Ò›Ð°" in TimeUnit and in singular date phrasing.
  Evidence: src/Humanizer/Properties/Resources.uz-Cyrl-UZ.resx: TimeSpanHumanize_MultipleMinutes={0} Ð¼Ð¸Ð½ÑƒÑ‚; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeSpanHumanizeTests.cs: Minutes(2) expects "2 Ð¼Ð¸Ð½ÑƒÑ‚"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeUnitToSymbolTests.cs: TimeUnit.Minute expects "Ð´Ð°Ò›Ð¸Ò›Ð°"
  Notes: Second-pass independent native review confirms first-pass defect and replacement.
- [uz-Cyrl-UZ] `TimeSpanHumanize_MultipleSeconds`
  Original: {0} ÑÐµÐºÑƒÐ½Ð´ -> {0} ÑÐ¾Ð½Ð¸Ñ
  Final: {0} ÑÐ¾Ð½Ð¸Ñ
  Decision: confirmed
  Rationale: Confirmed: keeping "ÑÐµÐºÑƒÐ½Ð´" in TimeSpan while the locale uses "ÑÐ¾Ð½Ð¸Ñ" elsewhere fragments terminology; replacing with "ÑÐ¾Ð½Ð¸Ñ" restores consistency and naturalness.
  Evidence: src/Humanizer/Properties/Resources.uz-Cyrl-UZ.resx: TimeSpanHumanize_MultipleSeconds={0} ÑÐµÐºÑƒÐ½Ð´; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeSpanHumanizeTests.cs: Seconds(2) expects "2 ÑÐµÐºÑƒÐ½Ð´"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeUnitToSymbolTests.cs: TimeUnit.Second expects "ÑÐ¾Ð½Ð¸Ñ"
  Notes: Second-pass independent native review confirms first-pass defect and replacement.
- [uz-Cyrl-UZ] `TimeSpanHumanize_SingleMinute`
  Original: 1 Ð¼Ð¸Ð½ÑƒÑ‚ -> 1 Ð´Ð°Ò›Ð¸Ò›Ð°
  Final: 1 Ð´Ð°Ò›Ð¸Ò›Ð°
  Decision: confirmed
  Rationale: Confirmed: this singular numeric form should use "Ð´Ð°Ò›Ð¸Ò›Ð°" to match the localeâ€™s established minute lexeme and avoid mixing "1 Ð¼Ð¸Ð½ÑƒÑ‚" with other "Ð´Ð°Ò›Ð¸Ò›Ð°" strings in the same culture.
  Evidence: src/Humanizer/Properties/Resources.uz-Cyrl-UZ.resx: TimeSpanHumanize_SingleMinute=1 Ð¼Ð¸Ð½ÑƒÑ‚; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeSpanHumanizeTests.cs: Minutes(1) expects "1 Ð¼Ð¸Ð½ÑƒÑ‚"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeUnitToSymbolTests.cs: TimeUnit.Minute expects "Ð´Ð°Ò›Ð¸Ò›Ð°"
  Notes: Second-pass independent native review confirms first-pass defect and replacement.
- [uz-Cyrl-UZ] `TimeSpanHumanize_SingleSecond`
  Original: 1 ÑÐµÐºÑƒÐ½Ð´ -> 1 ÑÐ¾Ð½Ð¸Ñ
  Final: 1 ÑÐ¾Ð½Ð¸Ñ
  Decision: confirmed
  Rationale: Confirmed: the singular numeric second should follow the localeâ€™s consistent "ÑÐ¾Ð½Ð¸Ñ" usage rather than the mixed-form "ÑÐµÐºÑƒÐ½Ð´".
  Evidence: src/Humanizer/Properties/Resources.uz-Cyrl-UZ.resx: TimeSpanHumanize_SingleSecond=1 ÑÐµÐºÑƒÐ½Ð´; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeSpanHumanizeTests.cs: Seconds(1) expects "1 ÑÐµÐºÑƒÐ½Ð´"; tests/Humanizer.Tests/Localisation/uz-Cyrl-UZ/TimeUnitToSymbolTests.cs: TimeUnit.Second expects "ÑÐ¾Ð½Ð¸Ñ"
  Notes: Second-pass independent native review confirms first-pass defect and replacement.
- [uz-Latn-UZ] `DateHumanize_MultipleDaysFromNow`
  Original: {0} kundan so`ng -> {0} kundan so'ng
  Final: {0} kundan so'ng
  Decision: confirmed
  Rationale: Confirmed: replacing backtick with apostrophe in this locale improves Uzbek Latin orthographic consistency while preserving meaning and grammar.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Second-pass confirms the first-pass normalization without modification.
- [uz-Latn-UZ] `DateHumanize_MultipleDaysFromNow_Paucal`
  Original: {0} kundan so`ng -> {0} kundan so'ng
  Final: {0} kundan so'ng
  Decision: confirmed
  Rationale: Confirmed: replacing backtick with apostrophe in this locale improves Uzbek Latin orthographic consistency while preserving meaning and grammar.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Second-pass confirms the first-pass normalization without modification.
- [uz-Latn-UZ] `DateHumanize_MultipleHoursFromNow`
  Original: {0} soatdan so`ng -> {0} soatdan so'ng
  Final: {0} soatdan so'ng
  Decision: confirmed
  Rationale: Confirmed: replacing backtick with apostrophe in this locale improves Uzbek Latin orthographic consistency while preserving meaning and grammar.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Second-pass confirms the first-pass normalization without modification.
- [uz-Latn-UZ] `DateHumanize_MultipleMinutesFromNow`
  Original: {0} minutdan so`ng -> {0} minutdan so'ng
  Final: {0} minutdan so'ng
  Decision: confirmed
  Rationale: Confirmed: replacing backtick with apostrophe in this locale improves Uzbek Latin orthographic consistency while preserving meaning and grammar.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Second-pass confirms the first-pass normalization without modification.
- [uz-Latn-UZ] `DateHumanize_MultipleMonthsFromNow`
  Original: {0} oydan so`ng -> {0} oydan so'ng
  Final: {0} oydan so'ng
  Decision: confirmed
  Rationale: Confirmed: replacing backtick with apostrophe in this locale improves Uzbek Latin orthographic consistency while preserving meaning and grammar.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Second-pass confirms the first-pass normalization without modification.
- [uz-Latn-UZ] `DateHumanize_MultipleSecondsFromNow`
  Original: {0} sekunddan so`ng -> {0} sekunddan so'ng
  Final: {0} sekunddan so'ng
  Decision: confirmed
  Rationale: Confirmed: replacing backtick with apostrophe in this locale improves Uzbek Latin orthographic consistency while preserving meaning and grammar.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Second-pass confirms the first-pass normalization without modification.
- [uz-Latn-UZ] `DateHumanize_MultipleYearsFromNow`
  Original: {0} yildan so`ng -> {0} yildan so'ng
  Final: {0} yildan so'ng
  Decision: confirmed
  Rationale: Confirmed: replacing backtick with apostrophe in this locale improves Uzbek Latin orthographic consistency while preserving meaning and grammar.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Second-pass confirms the first-pass normalization without modification.
- [uz-Latn-UZ] `DateHumanize_SingleHourFromNow`
  Original: bir soatdan so`ng -> bir soatdan so'ng
  Final: bir soatdan so'ng
  Decision: confirmed
  Rationale: Confirmed: replacing backtick with apostrophe in this locale improves Uzbek Latin orthographic consistency while preserving meaning and grammar.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Second-pass confirms the first-pass normalization without modification.
- [uz-Latn-UZ] `DateHumanize_SingleMinuteFromNow`
  Original: bir daqiqadan so`ng -> bir daqiqadan so'ng
  Final: bir daqiqadan so'ng
  Decision: confirmed
  Rationale: Confirmed: replacing backtick with apostrophe in this locale improves Uzbek Latin orthographic consistency while preserving meaning and grammar.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Second-pass confirms the first-pass normalization without modification.
- [uz-Latn-UZ] `DateHumanize_SingleMonthFromNow`
  Original: bir oydan so`ng -> bir oydan so'ng
  Final: bir oydan so'ng
  Decision: confirmed
  Rationale: Confirmed: replacing backtick with apostrophe in this locale improves Uzbek Latin orthographic consistency while preserving meaning and grammar.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Second-pass confirms the first-pass normalization without modification.
- [uz-Latn-UZ] `DateHumanize_SingleSecondFromNow`
  Original: bir soniyadan so`ng -> bir soniyadan so'ng
  Final: bir soniyadan so'ng
  Decision: confirmed
  Rationale: Confirmed: replacing backtick with apostrophe in this locale improves Uzbek Latin orthographic consistency while preserving meaning and grammar.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Second-pass confirms the first-pass normalization without modification.
- [uz-Latn-UZ] `DateHumanize_SingleYearFromNow`
  Original: bir yildan so`ng -> bir yildan so'ng
  Final: bir yildan so'ng
  Decision: confirmed
  Rationale: Confirmed: replacing backtick with apostrophe in this locale improves Uzbek Latin orthographic consistency while preserving meaning and grammar.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Second-pass confirms the first-pass normalization without modification.
- [uz-Latn-UZ] `DateHumanize_TwoDaysAgo`
  Original: o`tgan kun -> 2 kun avval
  Final: 2 kun avval
  Decision: confirmed
  Rationale: Confirmed: "o`tgan kun" is semantically vague in Uzbek and does not reliably encode an exact two-day offset; "2 kun avval" is precise and natural for deterministic humanization.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: DateHumanize_TwoDaysAgo qiymati semantik noaniq; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs: shu qiymat tekshirilgan
  Notes: Second-pass confirms the first-pass fix without modification.
- [uz-Latn-UZ] `DateHumanize_TwoDaysFromNow`
  Original: indindan keyin -> indinga
  Final: indinga
  Decision: confirmed
  Rationale: Confirmed: "indindan keyin" is awkward and can imply a point after the day-after-tomorrow; "indinga" is the idiomatic exact form for two days from now.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: DateHumanize_TwoDaysFromNow qiymati leksik noaniq; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs: shu qiymat tekshirilgan
  Notes: Second-pass confirms the first-pass fix without modification.
- [uz-Latn-UZ] `TimeSpanHumanize_Zero`
  Original: vaqt yo`q -> vaqt yo'q
  Final: vaqt yo'q
  Decision: confirmed
  Rationale: Confirmed: replacing backtick with apostrophe in this locale improves Uzbek Latin orthographic consistency while preserving meaning and grammar.
  Evidence: src/Humanizer/Properties/Resources.uz-Latn-UZ.resx: backtick bilan yozilgan shakl; tests/Humanizer.Tests/Localisation/uz-Latn-UZ/DateHumanizeTests.cs va TimeSpanHumanizeTests.cs: shu shaklga bog'langan kutilmalar
  Notes: Second-pass confirms the first-pass normalization without modification.
- [vi] `TimeSpanHumanize_MultipleMilliseconds`
  Original: {0} phần ngàn giây -> {0} mili giây
  Final: {0} mili giây
  Decision: confirmed
  Rationale: Xác nhận nhận định pass 1: "phần ngàn giây" là cách dịch sát nghĩa nhưng gượng trong tiếng Việt tự nhiên; "mili giây" là thuật ngữ chuẩn và nhất quán với hệ thuật ngữ hiện có của locale.
  Evidence: src/Humanizer/Properties/Resources.vi.resx: TimeSpanHumanize_MultipleMilliseconds = "{0} phần ngàn giây" và TimeUnit_Millisecond = "mili giây".; tests/Humanizer.Tests/Localisation/vi/TimeUnitToSymbolTests.cs: millisecond được kiểm tra với kỳ vọng "mili giây".
  Notes: Giữ nguyên thay thế đề xuất ở pass 1 để đồng bộ thuật ngữ.
- [vi] `TimeSpanHumanize_SingleMillisecond`
  Original: 1 phần ngàn giây -> 1 mili giây
  Final: 1 mili giây
  Decision: confirmed
  Rationale: Xác nhận nhận định pass 1: "1 phần ngàn giây" không tự nhiên trong ngữ cảnh giao diện; "1 mili giây" là cách nói chuẩn, ngắn gọn và dễ hiểu hơn cho người dùng Việt.
  Evidence: src/Humanizer/Properties/Resources.vi.resx: TimeSpanHumanize_SingleMillisecond = "1 phần ngàn giây" trong khi TimeUnit_Millisecond = "mili giây".; tests/Humanizer.Tests/Localisation/vi/TimeSpanHumanizeTests.cs: test Milliseconds đang khóa chuỗi "1 phần ngàn giây".
  Notes: Giữ nguyên thay thế đề xuất ở pass 1; cần cập nhật test kỳ vọng khi sửa resource.
- [zh-Hant] `TimeSpanHumanize_MultipleWeeks`
  Original: {0} å‘¨ -> {0} é€±
  Final: {0} é€±
  Decision: confirmed
  Rationale: ç¬¬äºŒè¼ªæ¯èªžå¯©æ ¡ç¢ºèªï¼šæ­¤è™• time unitã€Œweekã€åœ¨ zh-Hant èˆ‡ zh-HK æ…£ç”¨å¯«æ³•æ‡‰ç‚ºã€Œé€±ã€ã€‚ç¾è¡Œã€Œå‘¨ã€é›–å¯ç†è§£ï¼Œä½†å±¬éžé¦–é¸åœ°å€å­—å½¢ï¼Œä¸”æœƒé€éŽçˆ¶æ–‡åŒ–å›žé€€å½±éŸ¿ zh-HK é¡¯ç¤ºè‡ªç„¶åº¦ã€‚
  Evidence: src/Humanizer/Properties/Resources.zh-Hant.resx:213; tests/Humanizer.Tests/Localisation/zh-Hant/TimeSpanHumanizeTests.cs:28; tests/Humanizer.Tests/Localisation/zh-HK/TimeSpanHumanizeTests.cs:26
  Notes: Second-pass (native, independent): confirmed first-pass defect and replacement; includes zh-HK fallback impact.
- [zh-Hant] `TimeSpanHumanize_MultipleWeeks_Dual`
  Original: {0} å‘¨ -> {0} é€±
  Final: {0} é€±
  Decision: confirmed
  Rationale: ç¬¬äºŒè¼ªæ¯èªžå¯©æ ¡ç¢ºèªï¼šæ­¤è™• time unitã€Œweekã€åœ¨ zh-Hant èˆ‡ zh-HK æ…£ç”¨å¯«æ³•æ‡‰ç‚ºã€Œé€±ã€ã€‚ç¾è¡Œã€Œå‘¨ã€é›–å¯ç†è§£ï¼Œä½†å±¬éžé¦–é¸åœ°å€å­—å½¢ï¼Œä¸”æœƒé€éŽçˆ¶æ–‡åŒ–å›žé€€å½±éŸ¿ zh-HK é¡¯ç¤ºè‡ªç„¶åº¦ã€‚
  Evidence: src/Humanizer/Properties/Resources.zh-Hant.resx:606; tests/Humanizer.Tests/Localisation/zh-Hant/TimeSpanHumanizeTests.cs:29; tests/Humanizer.Tests/Localisation/zh-HK/TimeSpanHumanizeTests.cs:27
  Notes: Second-pass (native, independent): confirmed first-pass defect and replacement; includes zh-HK fallback impact.
- [zh-Hant] `TimeSpanHumanize_MultipleWeeks_Paucal`
  Original: {0} å‘¨ -> {0} é€±
  Final: {0} é€±
  Decision: confirmed
  Rationale: ç¬¬äºŒè¼ªæ¯èªžå¯©æ ¡ç¢ºèªï¼šæ­¤è™• time unitã€Œweekã€åœ¨ zh-Hant èˆ‡ zh-HK æ…£ç”¨å¯«æ³•æ‡‰ç‚ºã€Œé€±ã€ã€‚ç¾è¡Œã€Œå‘¨ã€é›–å¯ç†è§£ï¼Œä½†å±¬éžé¦–é¸åœ°å€å­—å½¢ï¼Œä¸”æœƒé€éŽçˆ¶æ–‡åŒ–å›žé€€å½±éŸ¿ zh-HK é¡¯ç¤ºè‡ªç„¶åº¦ã€‚
  Evidence: src/Humanizer/Properties/Resources.zh-Hant.resx:249; tests/Humanizer.Tests/Localisation/zh-Hant/TimeSpanHumanizeTests.cs:28; tests/Humanizer.Tests/Localisation/zh-HK/TimeSpanHumanizeTests.cs:26
  Notes: Second-pass (native, independent): confirmed first-pass defect and replacement; includes zh-HK fallback impact.
- [zh-Hant] `TimeSpanHumanize_MultipleWeeks_Plural`
  Original: {0} å‘¨ -> {0} é€±
  Final: {0} é€±
  Decision: confirmed
  Rationale: ç¬¬äºŒè¼ªæ¯èªžå¯©æ ¡ç¢ºèªï¼šæ­¤è™• time unitã€Œweekã€åœ¨ zh-Hant èˆ‡ zh-HK æ…£ç”¨å¯«æ³•æ‡‰ç‚ºã€Œé€±ã€ã€‚ç¾è¡Œã€Œå‘¨ã€é›–å¯ç†è§£ï¼Œä½†å±¬éžé¦–é¸åœ°å€å­—å½¢ï¼Œä¸”æœƒé€éŽçˆ¶æ–‡åŒ–å›žé€€å½±éŸ¿ zh-HK é¡¯ç¤ºè‡ªç„¶åº¦ã€‚
  Evidence: src/Humanizer/Properties/Resources.zh-Hant.resx:492; tests/Humanizer.Tests/Localisation/zh-Hant/TimeSpanHumanizeTests.cs:29; tests/Humanizer.Tests/Localisation/zh-HK/TimeSpanHumanizeTests.cs:27
  Notes: Second-pass (native, independent): confirmed first-pass defect and replacement; includes zh-HK fallback impact.
- [zh-Hant] `TimeSpanHumanize_MultipleWeeks_Singular`
  Original: {0} å‘¨ -> {0} é€±
  Final: {0} é€±
  Decision: confirmed
  Rationale: ç¬¬äºŒè¼ªæ¯èªžå¯©æ ¡ç¢ºèªï¼šæ­¤è™• time unitã€Œweekã€åœ¨ zh-Hant èˆ‡ zh-HK æ…£ç”¨å¯«æ³•æ‡‰ç‚ºã€Œé€±ã€ã€‚ç¾è¡Œã€Œå‘¨ã€é›–å¯ç†è§£ï¼Œä½†å±¬éžé¦–é¸åœ°å€å­—å½¢ï¼Œä¸”æœƒé€éŽçˆ¶æ–‡åŒ–å›žé€€å½±éŸ¿ zh-HK é¡¯ç¤ºè‡ªç„¶åº¦ã€‚
  Evidence: src/Humanizer/Properties/Resources.zh-Hant.resx:369; tests/Humanizer.Tests/Localisation/zh-Hant/TimeSpanHumanizeTests.cs:28; tests/Humanizer.Tests/Localisation/zh-HK/TimeSpanHumanizeTests.cs:26
  Notes: Second-pass (native, independent): confirmed first-pass defect and replacement; includes zh-HK fallback impact.
- [zh-Hant] `TimeSpanHumanize_SingleWeek`
  Original: 1 å‘¨ -> 1 é€±
  Final: 1 é€±
  Decision: confirmed
  Rationale: ç¬¬äºŒè¼ªæ¯èªžå¯©æ ¡ç¢ºèªï¼šæ­¤è™• time unitã€Œweekã€åœ¨ zh-Hant èˆ‡ zh-HK æ…£ç”¨å¯«æ³•æ‡‰ç‚ºã€Œé€±ã€ã€‚ç¾è¡Œã€Œå‘¨ã€é›–å¯ç†è§£ï¼Œä½†å±¬éžé¦–é¸åœ°å€å­—å½¢ï¼Œä¸”æœƒé€éŽçˆ¶æ–‡åŒ–å›žé€€å½±éŸ¿ zh-HK é¡¯ç¤ºè‡ªç„¶åº¦ã€‚
  Evidence: src/Humanizer/Properties/Resources.zh-Hant.resx:237; tests/Humanizer.Tests/Localisation/zh-Hant/TimeSpanHumanizeTests.cs:28; tests/Humanizer.Tests/Localisation/zh-HK/TimeSpanHumanizeTests.cs:26
  Notes: Second-pass (native, independent): confirmed first-pass defect and replacement; includes zh-HK fallback impact.
- [zh-Hant] `TimeSpanHumanize_SingleWeeks_Words`
  Original: 1 å‘¨ -> 1 é€±
  Final: 1 é€±
  Decision: confirmed
  Rationale: ç¬¬äºŒè¼ªæ¯èªžå¯©æ ¡ç¢ºèªï¼šæ­¤è™• time unitã€Œweekã€åœ¨ zh-Hant èˆ‡ zh-HK æ…£ç”¨å¯«æ³•æ‡‰ç‚ºã€Œé€±ã€ã€‚ç¾è¡Œã€Œå‘¨ã€é›–å¯ç†è§£ï¼Œä½†å±¬éžé¦–é¸åœ°å€å­—å½¢ï¼Œä¸”æœƒé€éŽçˆ¶æ–‡åŒ–å›žé€€å½±éŸ¿ zh-HK é¡¯ç¤ºè‡ªç„¶åº¦ã€‚
  Evidence: src/Humanizer/Properties/Resources.zh-Hant.resx:393; tests/Humanizer.Tests/Localisation/zh-Hant/TimeSpanHumanizeTests.cs:28; tests/Humanizer.Tests/Localisation/zh-HK/TimeSpanHumanizeTests.cs:26
  Notes: Second-pass (native, independent): confirmed first-pass defect and replacement; includes zh-HK fallback impact.
- [zh-Hant] `TimeUnit_Week`
  Original: å‘¨ -> é€±
  Final: é€±
  Decision: confirmed
  Rationale: ç¬¬äºŒè¼ªæ¯èªžå¯©æ ¡ç¢ºèªï¼šæ­¤è™• time unitã€Œweekã€åœ¨ zh-Hant èˆ‡ zh-HK æ…£ç”¨å¯«æ³•æ‡‰ç‚ºã€Œé€±ã€ã€‚ç¾è¡Œã€Œå‘¨ã€é›–å¯ç†è§£ï¼Œä½†å±¬éžé¦–é¸åœ°å€å­—å½¢ï¼Œä¸”æœƒé€éŽçˆ¶æ–‡åŒ–å›žé€€å½±éŸ¿ zh-HK é¡¯ç¤ºè‡ªç„¶åº¦ã€‚
  Evidence: src/Humanizer/Properties/Resources.zh-Hant.resx:483; tests/Humanizer.Tests/Localisation/zh-Hant/TimeUnitToSymbolTests.cs:12; tests/Humanizer.Tests/Localisation/zh-HK/TimeUnitToSymbolTests.cs:12
  Notes: Second-pass (native, independent): confirmed first-pass defect and replacement; includes zh-HK fallback impact.

