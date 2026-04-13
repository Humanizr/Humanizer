using System.Text;

namespace Humanizer.Tests.Localisation;

static class LocaleCoverageData
{
    static readonly string[] ShippedLocaleNames = FindShippedLocales();

#if NET8_0
    const string ZhHantShortTime = "1:23";
    const string ZhHantRoundedTime = "1:25";
    const string ZhHantMorningShortTime = "1:05";
#else
    const string ZhHantShortTime = "下午1:23";
    const string ZhHantRoundedTime = "下午1:25";
    const string ZhHantMorningShortTime = "上午1:05";
#endif

    public static IReadOnlyList<string> ShippedLocales => ShippedLocaleNames;

    // Representative non-first-of-month date sample.
    public static TheoryData<string, DateExpectationRow> DateToOrdinalWords2022January25ExpectationTheoryData { get; } = new()
    {
        { "af", new(2022, 1, 25, "25 Januarie 2022") },
        { "ar", new(2022, 1, 25, "25 يناير 2022") },
        { "az", new(2022, 1, 25, "25 yanvar 2022") },
        { "bg", new(2022, 1, 25, "25 януари 2022 г.") },
        { "bn", new(2022, 1, 25, "25 জানুয়ারী 2022") },
        { "ca", new(2022, 1, 25, "25 de gener de 2022") },
        { "cs", new(2022, 1, 25, "25. ledna 2022") },
        { "da", new(2022, 1, 25, "25. januar 2022") },
        { "de", new(2022, 1, 25, "25. Januar 2022") },
        { "de-CH", new(2022, 1, 25, "25. Januar 2022") },
        { "de-LI", new(2022, 1, 25, "25. Januar 2022") },
        { "el", new(2022, 1, 25, "25 Ιανουαρίου 2022") },
        { "en", new(2022, 1, 25, "25th January 2022") },
        { "en-GB", new(2022, 1, 25, "25th January 2022") },
        { "en-IN", new(2022, 1, 25, "25th January 2022") },
        { "en-US", new(2022, 1, 25, "January 25th, 2022") },
        { "es", new(2022, 1, 25, "25 de enero de 2022") },
        { "fa", new(2022, 1, 25, "25 ژانویهٔ 2022") },
        { "fi", new(2022, 1, 25, "25. tammikuuta 2022") },
        { "fil", new(2022, 1, 25, "Enero 25, 2022") },
        { "fr", new(2022, 1, 25, "25 janvier 2022") },
        { "fr-BE", new(2022, 1, 25, "25 janvier 2022") },
        { "fr-CH", new(2022, 1, 25, "25 janvier 2022") },
        { "he", new(2022, 1, 25, "25 בינואר 2022") },
        { "hr", new(2022, 1, 25, "25. siječnja 2022.") },
        { "hu", new(2022, 1, 25, "2022. január 25.") },
        { "hy", new(2022, 1, 25, "25 հունվարի 2022") },
        { "id", new(2022, 1, 25, "25 Januari 2022") },
        { "is", new(2022, 1, 25, "25. janúar 2022") },
        { "it", new(2022, 1, 25, "25 gennaio 2022") },
        { "ja", new(2022, 1, 25, "2022年1月25日") },
        { "ko", new(2022, 1, 25, "2022년 1월 25일") },
        { "ku", new(2022, 1, 25, "25 کانوونی دووەم 2022") },
        { "lb", new(2022, 1, 25, "25. Januar 2022") },
        { "lt", new(2022, 1, 25, "2022 m. sausio 25 d.") },
        { "lv", new(2022, 1, 25, "25. janvāris 2022") },
        { "ms", new(2022, 1, 25, "25 Januari 2022") },
        { "mt", new(2022, 1, 25, "25 ta’ Jannar 2022") },
        { "nb", new(2022, 1, 25, "25. januar 2022") },
        { "nl", new(2022, 1, 25, "25 januari 2022") },
        { "nn", new(2022, 1, 25, "25. januar 2022") },
        { "pl", new(2022, 1, 25, "25 stycznia 2022") },
        { "pt", new(2022, 1, 25, "25 de janeiro de 2022") },
        { "pt-BR", new(2022, 1, 25, "25 de janeiro de 2022") },
        { "ro", new(2022, 1, 25, "25 ianuarie 2022") },
        { "ru", new(2022, 1, 25, "25 января 2022") },
        { "sk", new(2022, 1, 25, "25. januára 2022") },
        { "sl", new(2022, 1, 25, "25. januar 2022") },
        { "sr", new(2022, 1, 25, "25. јануар 2022.") },
        { "sr-Latn", new(2022, 1, 25, "25. januar 2022.") },
        { "sv", new(2022, 1, 25, "25 januari 2022") },
        { "ta", new(2022, 1, 25, "25 ஜனவரி 2022") },
        { "th", new(2022, 1, 25, "25 มกราคม 2565") },
        { "tr", new(2022, 1, 25, "25 Ocak 2022") },
        { "uk", new(2022, 1, 25, "25 січня 2022") },
        { "ur", new(2022, 1, 25, "25 جنوری، 2022") },
        { "uz-Cyrl-UZ", new(2022, 1, 25, "25 январ 2022") },
        { "uz-Latn-UZ", new(2022, 1, 25, "25-yanvar 2022") },
        { "vi", new(2022, 1, 25, "25 tháng 1 năm 2022") },
        { "zh-CN", new(2022, 1, 25, "2022年1月25日") },
        { "zh-Hans", new(2022, 1, 25, "2022年1月25日") },
        { "zh-Hant", new(2022, 1, 25, "2022年1月25日") },
        { "zu-ZA", new(2022, 1, 25, "25 Januwari 2022") }
    };

    // First-of-month sample where many locales change the day form.
    public static TheoryData<string, DateExpectationRow> DateToOrdinalWords2015January1ExpectationTheoryData { get; } = new()
    {
        { "af", new(2015, 1, 1, "1 Januarie 2015") },
        { "ar", new(2015, 1, 1, "1 يناير 2015") },
        { "az", new(2015, 1, 1, "1 yanvar 2015") },
        { "bg", new(2015, 1, 1, "1 януари 2015 г.") },
        { "bn", new(2015, 1, 1, "1 জানুয়ারী 2015") },
        { "ca", new(2015, 1, 1, "1 de gener de 2015") },
        { "cs", new(2015, 1, 1, "1. ledna 2015") },
        { "da", new(2015, 1, 1, "1. januar 2015") },
        { "de", new(2015, 1, 1, "1. Januar 2015") },
        { "de-CH", new(2015, 1, 1, "1. Januar 2015") },
        { "de-LI", new(2015, 1, 1, "1. Januar 2015") },
        { "el", new(2015, 1, 1, "1 Ιανουαρίου 2015") },
        { "en", new(2015, 1, 1, "1st January 2015") },
        { "en-GB", new(2015, 1, 1, "1st January 2015") },
        { "en-IN", new(2015, 1, 1, "1st January 2015") },
        { "en-US", new(2015, 1, 1, "January 1st, 2015") },
        { "es", new(2015, 1, 1, "1 de enero de 2015") },
        { "fa", new(2015, 1, 1, "1 ژانویهٔ 2015") },
        { "fi", new(2015, 1, 1, "1. tammikuuta 2015") },
        { "fil", new(2015, 1, 1, "Enero 1, 2015") },
        { "fr", new(2015, 1, 1, "1er janvier 2015") },
        { "fr-BE", new(2015, 1, 1, "1er janvier 2015") },
        { "fr-CH", new(2015, 1, 1, "1er janvier 2015") },
        { "he", new(2015, 1, 1, "1 בינואר 2015") },
        { "hr", new(2015, 1, 1, "1. siječnja 2015.") },
        { "hu", new(2015, 1, 1, "2015. január 1.") },
        { "hy", new(2015, 1, 1, "1 հունվարի 2015") },
        { "id", new(2015, 1, 1, "1 Januari 2015") },
        { "is", new(2015, 1, 1, "1. janúar 2015") },
        { "it", new(2015, 1, 1, "1 gennaio 2015") },
        { "ja", new(2015, 1, 1, "2015年1月1日") },
        { "ko", new(2015, 1, 1, "2015년 1월 1일") },
        { "ku", new(2015, 1, 1, "1 کانوونی دووەم 2015") },
        { "lb", new(2015, 1, 1, "1. Januar 2015") },
        { "lt", new(2015, 1, 1, "2015 m. sausio 1 d.") },
        { "lv", new(2015, 1, 1, "1. janvāris 2015") },
        { "ms", new(2015, 1, 1, "1 Januari 2015") },
        { "mt", new(2015, 1, 1, "1 ta’ Jannar 2015") },
        { "nb", new(2015, 1, 1, "1. januar 2015") },
        { "nl", new(2015, 1, 1, "1 januari 2015") },
        { "nn", new(2015, 1, 1, "1. januar 2015") },
        { "pl", new(2015, 1, 1, "1 stycznia 2015") },
        { "pt", new(2015, 1, 1, "1 de janeiro de 2015") },
        { "pt-BR", new(2015, 1, 1, "1º de janeiro de 2015") },
        { "ro", new(2015, 1, 1, "1 ianuarie 2015") },
        { "ru", new(2015, 1, 1, "1 января 2015") },
        { "sk", new(2015, 1, 1, "1. januára 2015") },
        { "sl", new(2015, 1, 1, "1. januar 2015") },
        { "sr", new(2015, 1, 1, "1. јануар 2015.") },
        { "sr-Latn", new(2015, 1, 1, "1. januar 2015.") },
        { "sv", new(2015, 1, 1, "1 januari 2015") },
        { "ta", new(2015, 1, 1, "1 ஜனவரி 2015") },
        { "th", new(2015, 1, 1, "1 มกราคม 2558") },
        { "tr", new(2015, 1, 1, "1 Ocak 2015") },
        { "uk", new(2015, 1, 1, "1 січня 2015") },
        { "ur", new(2015, 1, 1, "1 جنوری، 2015") },
        { "uz-Cyrl-UZ", new(2015, 1, 1, "1 январ 2015") },
        { "uz-Latn-UZ", new(2015, 1, 1, "1-yanvar 2015") },
        { "vi", new(2015, 1, 1, "1 tháng 1 năm 2015") },
        { "zh-CN", new(2015, 1, 1, "2015年1月1日") },
        { "zh-Hans", new(2015, 1, 1, "2015年1月1日") },
        { "zh-Hant", new(2015, 1, 1, "2015年1月1日") },
        { "zu-ZA", new(2015, 1, 1, "1 Januwari 2015") }
    };

    // Early-month sample used to catch plain numeric-day formatting.
    public static TheoryData<string, DateExpectationRow> DateToOrdinalWords2015February3ExpectationTheoryData { get; } = new()
    {
        { "af", new(2015, 2, 3, "3 Februarie 2015") },
        { "ar", new(2015, 2, 3, "3 فبراير 2015") },
        { "az", new(2015, 2, 3, "3 fevral 2015") },
        { "bg", new(2015, 2, 3, "3 февруари 2015 г.") },
        { "bn", new(2015, 2, 3, "3 ফেব্রুয়ারী 2015") },
        { "ca", new(2015, 2, 3, "3 de febrer de 2015") },
        { "cs", new(2015, 2, 3, "3. února 2015") },
        { "da", new(2015, 2, 3, "3. februar 2015") },
        { "de", new(2015, 2, 3, "3. Februar 2015") },
        { "de-CH", new(2015, 2, 3, "3. Februar 2015") },
        { "de-LI", new(2015, 2, 3, "3. Februar 2015") },
        { "el", new(2015, 2, 3, "3 Φεβρουαρίου 2015") },
        { "en", new(2015, 2, 3, "3rd February 2015") },
        { "en-GB", new(2015, 2, 3, "3rd February 2015") },
        { "en-IN", new(2015, 2, 3, "3rd February 2015") },
        { "en-US", new(2015, 2, 3, "February 3rd, 2015") },
        { "es", new(2015, 2, 3, "3 de febrero de 2015") },
        { "fa", new(2015, 2, 3, "3 فوریهٔ 2015") },
        { "fi", new(2015, 2, 3, "3. helmikuuta 2015") },
        { "fil", new(2015, 2, 3, "Pebrero 3, 2015") },
        { "fr", new(2015, 2, 3, "3 février 2015") },
        { "fr-BE", new(2015, 2, 3, "3 février 2015") },
        { "fr-CH", new(2015, 2, 3, "3 février 2015") },
        { "he", new(2015, 2, 3, "3 בפברואר 2015") },
        { "hr", new(2015, 2, 3, "3. veljače 2015.") },
        { "hu", new(2015, 2, 3, "2015. február 3.") },
        { "hy", new(2015, 2, 3, "3 փետրվարի 2015") },
        { "id", new(2015, 2, 3, "3 Februari 2015") },
        { "is", new(2015, 2, 3, "3. febrúar 2015") },
        { "it", new(2015, 2, 3, "3 febbraio 2015") },
        { "ja", new(2015, 2, 3, "2015年2月3日") },
        { "ko", new(2015, 2, 3, "2015년 2월 3일") },
        { "ku", new(2015, 2, 3, "3 شوبات 2015") },
        { "lb", new(2015, 2, 3, "3. Februar 2015") },
        { "lt", new(2015, 2, 3, "2015 m. vasario 3 d.") },
        { "lv", new(2015, 2, 3, "3. februāris 2015") },
        { "ms", new(2015, 2, 3, "3 Februari 2015") },
        { "mt", new(2015, 2, 3, "3 ta’ Frar 2015") },
        { "nb", new(2015, 2, 3, "3. februar 2015") },
        { "nl", new(2015, 2, 3, "3 februari 2015") },
        { "nn", new(2015, 2, 3, "3. februar 2015") },
        { "pl", new(2015, 2, 3, "3 lutego 2015") },
        { "pt", new(2015, 2, 3, "3 de fevereiro de 2015") },
        { "pt-BR", new(2015, 2, 3, "3 de fevereiro de 2015") },
        { "ro", new(2015, 2, 3, "3 februarie 2015") },
        { "ru", new(2015, 2, 3, "3 февраля 2015") },
        { "sk", new(2015, 2, 3, "3. februára 2015") },
        { "sl", new(2015, 2, 3, "3. februar 2015") },
        { "sr", new(2015, 2, 3, "3. фебруар 2015.") },
        { "sr-Latn", new(2015, 2, 3, "3. februar 2015.") },
        { "sv", new(2015, 2, 3, "3 februari 2015") },
        { "ta", new(2015, 2, 3, "3 பிப்ரவரி 2015") },
        { "th", new(2015, 2, 3, "3 กุมภาพันธ์ 2558") },
        { "tr", new(2015, 2, 3, "3 Şubat 2015") },
        { "uk", new(2015, 2, 3, "3 лютого 2015") },
        { "ur", new(2015, 2, 3, "3 فروری، 2015") },
        { "uz-Cyrl-UZ", new(2015, 2, 3, "3 феврал 2015") },
        { "uz-Latn-UZ", new(2015, 2, 3, "3-fevral 2015") },
        { "vi", new(2015, 2, 3, "3 tháng 2 năm 2015") },
        { "zh-CN", new(2015, 2, 3, "2015年2月3日") },
        { "zh-Hans", new(2015, 2, 3, "2015年2月3日") },
        { "zh-Hant", new(2015, 2, 3, "2015年2月3日") },
        { "zu-ZA", new(2015, 2, 3, "3 Februwari 2015") }
    };

    public static TheoryData<string, DateExpectationRow> DateToOrdinalWords2020February29ExpectationTheoryData { get; } = new()
    {
        { "ca", new(2020, 2, 29, "29 de febrer de 2020") },
        { "es", new(2020, 2, 29, "29 de febrero de 2020") },
    };

    public static TheoryData<string, DateExpectationRow> DateToOrdinalWords2015September4ExpectationTheoryData { get; } = new()
    {
        { "ca", new(2015, 9, 4, "4 de setembre de 2015") },
        { "es", new(2015, 9, 4, "4 de septiembre de 2015") },
    };

    public static TheoryData<string, DateExpectationRow> DateToOrdinalWords1979November7ExpectationTheoryData { get; } = new()
    {
        { "ca", new(1979, 11, 7, "7 de novembre de 1979") },
        { "es", new(1979, 11, 7, "7 de noviembre de 1979") },
    };

    public static TheoryData<string, DateExpectationRow> DateToOrdinalWords2020March2ExpectationTheoryData { get; } = new()
    {
        { "fr", new(2020, 3, 2, "2 mars 2020") },
    };

    public static TheoryData<string, DateExpectationRow> DateToOrdinalWords2021October31ExpectationTheoryData { get; } = new()
    {
        { "fr", new(2021, 10, 31, "31 octobre 2021") },
    };

    public static TheoryData<string, DateExpectationRow> DateToOrdinalWords2024December31ExpectationTheoryData { get; } = new()
    {
        { "ar", new(2024, 12, 31, "31 ديسمبر 2024") },
        { "fa", new(2024, 12, 31, "31 دسامبر 2024") },
        { "ja", new(2024, 12, 31, "2024年12月31日") },
    };

#if NET6_0_OR_GREATER

    // Unrounded afternoon clock sample.
    public static TheoryData<string, ClockExpectationRow> TimeOnlyToClockNotation1323ExpectationTheoryData { get; } = new()
    {
        { "af", new(13, 23, "een uur drie en twintig") },
        { "ar", new(13, 23, "الواحدة وثلاث وعشرون دقيقة بعد الظهر") },
        { "az", new(13, 23, "on üç saat iyirmi üç dəqiqə") },
        { "bg", new(13, 23, "тринадесет часа и двадесет и три минути") },
        { "bn", new(13, 23, "দুপুর একটা তেইশ") },
        { "ca", new(13, 23, "la una i vint-i-tres de la tarda") },
        { "cs", new(13, 23, "třináct hodin dvacet tři minut") },
        { "da", new(13, 23, "tretten treogtyve") },
        { "de", new(13, 23, "eins Uhr dreiundzwanzig") },
        { "de-CH", new(13, 23, "eins Uhr dreiundzwanzig") },
        { "de-LI", new(13, 23, "eins Uhr dreiundzwanzig") },
        { "el", new(13, 23, "μία και είκοσι τρία το απόγευμα") },
        { "en", new(13, 23, "one twenty-three") },
        { "en-GB", new(13, 23, "one twenty-three") },
        { "en-IN", new(13, 23, "one twenty three") },
        { "en-US", new(13, 23, "one twenty-three") },
        { "es", new(13, 23, "la una y veintitrés de la tarde") },
        { "fa", new(13, 23, "یک و بیست و سه بعدازظهر") },
        { "fi", new(13, 23, "kolmetoista kaksikymmentäkolme") },
        { "fil", new(13, 23, "ala una beinte-tres ng hapon") },
        { "fr", new(13, 23, "treize heures vingt-trois") },
        { "fr-BE", new(13, 23, "treize heures vingt-trois") },
        { "fr-CH", new(13, 23, "treize heures vingt-trois") },
        { "he", new(13, 23, "אחת עשרים ושלוש") },
        { "hr", new(13, 23, "trinaest sati i dvadeset tri minute") },
        { "hu", new(13, 23, "tizenhárom óra huszonhárom perc") },
        { "hy", new(13, 23, "մեկն անց քսաներեք") },
        { "id", new(13, 23, "pukul satu lewat dua puluh tiga menit") },
        { "is", new(13, 23, "þrettán tuttugu og þrjú") },
        { "it", new(13, 23, "una e ventitré") },
        { "ja", new(13, 23, "13時23分") },
        { "ko", new(13, 23, "오후 한 시 이십삼 분") },
        { "ku", new(13, 23, "یەک و بیست و سێی ئێوارە") },
        { "lb", new(13, 23, "dräianzwanzeg Minutten op eng") },
        { "lt", new(13, 23, "trylika valandų dvidešimt trys minutės") },
        { "lv", new(13, 23, "trīspadsmit un divdesmit trīs") },
        { "ms", new(13, 23, "pukul satu dua puluh tiga petang") },
        { "mt", new(13, 23, "is-siegħa waħda u tlieta u għoxrin") },
        { "nb", new(13, 23, "tretten tjuetre") },
        { "nl", new(13, 23, "een uur drieëntwintig") },
        { "nn", new(13, 23, "tretten tjuetre") },
        { "pl", new(13, 23, "trzynasta dwadzieścia trzy") },
        { "pt", new(13, 23, "uma e vinte e três") },
        { "pt-BR", new(13, 23, "uma e vinte e três") },
        { "ro", new(13, 23, "ora unu și douăzeci și trei") },
        { "ru", new(13, 23, "час двадцать три") },
        { "sk", new(13, 23, "trinásť hodín dvadsaťtri minút") },
        { "sl", new(13, 23, "trinajst triindvajset") },
        { "sr", new(13, 23, "један и двадесет три") },
        { "sr-Latn", new(13, 23, "jedan i dvadeset tri") },
        { "sv", new(13, 23, "tretton tjugotre") },
        { "ta", new(13, 23, "பிற்பகல் ஒரு மணி இருபத்து மூன்று நிமிடம்") },
        { "th", new(13, 23, "บ่ายหนึ่งยี่สิบสามนาที") },
        { "tr", new(13, 23, "on üç yirmi üç") },
        { "uk", new(13, 23, "перша двадцять три") },
        { "ur", new(13, 23, "ایک بج کر تئیس منٹ دوپہر") },
        { "uz-Cyrl-UZ", new(13, 23, "бирдан йигирма уч ўтди") },
        { "uz-Latn-UZ", new(13, 23, "birdan yigirma uch o’tdi") },
        { "vi", new(13, 23, "một giờ hai mươi ba phút chiều") },
        { "zh-CN", new(13, 23, "下午一点二十三分") },
        { "zh-Hans", new(13, 23, "下午一点二十三分") },
        { "zh-Hant", new(13, 23, "下午一點二十三分") },
        { "zu-ZA", new(13, 23, "ihora lokuqala namashumi amabili nantathu ntambama") }
    };

    // Same afternoon clock sample rounded to the nearest five minutes.
    public static TheoryData<string, ClockExpectationRow> TimeOnlyToClockNotation1323RoundedExpectationTheoryData { get; } = new()
    {
        { "af", new(13, 23, "een uur vyf en twintig") },
        { "ar", new(13, 23, "الواحدة وخمس وعشرون دقيقة بعد الظهر") },
        { "az", new(13, 23, "on üç saat iyirmi beş dəqiqə") },
        { "bg", new(13, 23, "тринадесет часа и двадесет и пет минути") },
        { "bn", new(13, 23, "দুপুর একটা পঁচিশ") },
        { "ca", new(13, 23, "la una i vint-i-cinc de la tarda") },
        { "cs", new(13, 23, "třináct hodin dvacet pět minut") },
        { "da", new(13, 23, "tretten femogtyve") },
        { "de", new(13, 23, "fünf vor halb zwei") },
        { "de-CH", new(13, 23, "fünf vor halb zwei") },
        { "de-LI", new(13, 23, "fünf vor halb zwei") },
        { "el", new(13, 23, "μία και είκοσι πέντε το απόγευμα") },
        { "en", new(13, 23, "twenty-five past one") },
        { "en-GB", new(13, 23, "twenty-five past one") },
        { "en-IN", new(13, 23, "twenty-five past one") },
        { "en-US", new(13, 23, "twenty-five past one") },
        { "es", new(13, 23, "la una y veinticinco de la tarde") },
        { "fa", new(13, 23, "یک و بیست و پنج بعدازظهر") },
        { "fi", new(13, 23, "kolmetoista kaksikymmentäviisi") },
        { "fil", new(13, 23, "ala una beinte-singko ng hapon") },
        { "fr", new(13, 23, "treize heures vingt-cinq") },
        { "fr-BE", new(13, 23, "treize heures vingt-cinq") },
        { "fr-CH", new(13, 23, "treize heures vingt-cinq") },
        { "he", new(13, 23, "אחת עשרים וחמש") },
        { "hr", new(13, 23, "trinaest sati i dvadeset pet minuta") },
        { "hu", new(13, 23, "tizenhárom óra huszonöt perc") },
        { "hy", new(13, 23, "մեկն անց քսանհինգ") },
        { "id", new(13, 23, "pukul satu lewat dua puluh lima menit") },
        { "is", new(13, 23, "þrettán tuttugu og fimm") },
        { "it", new(13, 23, "una e venticinque") },
        { "ja", new(13, 23, "13時25分") },
        { "ko", new(13, 23, "오후 한 시 이십오 분") },
        { "ku", new(13, 23, "یەک و بیست و پێنجی ئێوارە") },
        { "lb", new(13, 23, "fënnef vir hallwer zwou") },
        { "lt", new(13, 23, "trylika valandų dvidešimt penkios minutės") },
        { "lv", new(13, 23, "trīspadsmit un divdesmit pieci") },
        { "ms", new(13, 23, "pukul satu dua puluh lima petang") },
        { "mt", new(13, 23, "is-siegħa waħda u ħamsa u għoxrin") },
        { "nb", new(13, 23, "tretten tjuefem") },
        { "nl", new(13, 23, "een uur vijfentwintig") },
        { "nn", new(13, 23, "tretten tjuefem") },
        { "pl", new(13, 23, "trzynasta dwadzieścia pięć") },
        { "pt", new(13, 23, "uma e vinte e cinco") },
        { "pt-BR", new(13, 23, "uma e vinte e cinco") },
        { "ro", new(13, 23, "ora unu și douăzeci și cinci") },
        { "ru", new(13, 23, "час двадцать пять") },
        { "sk", new(13, 23, "trinásť hodín dvadsaťpäť minút") },
        { "sl", new(13, 23, "trinajst petindvajset") },
        { "sr", new(13, 23, "један и двадесет пет") },
        { "sr-Latn", new(13, 23, "jedan i dvadeset pet") },
        { "sv", new(13, 23, "tretton tjugofem") },
        { "ta", new(13, 23, "பிற்பகல் ஒரு மணி இருபத்து ஐந்து நிமிடம்") },
        { "th", new(13, 23, "บ่ายหนึ่งยี่สิบห้านาที") },
        { "tr", new(13, 23, "on üç yirmi beş") },
        { "uk", new(13, 23, "перша двадцять п’ять") },
        { "ur", new(13, 23, "ایک بج کر پچیس منٹ دوپہر") },
        { "uz-Cyrl-UZ", new(13, 23, "бирдан йигирма беш ўтди") },
        { "uz-Latn-UZ", new(13, 23, "birdan yigirma besh o‘tdi") },
        { "vi", new(13, 23, "một giờ hai mươi lăm phút chiều") },
        { "zh-CN", new(13, 23, "下午一点二十五分") },
        { "zh-Hans", new(13, 23, "下午一点二十五分") },
        { "zh-Hant", new(13, 23, "下午一點二十五分") },
        { "zu-ZA", new(13, 23, "ihora lokuqala namashumi amabili nanhlanu ntambama") }
    };

    // Early-morning sample that catches one-o'clock phrasing and culture short-time output.
    public static TheoryData<string, ClockExpectationRow> TimeOnlyToClockNotation0105ExpectationTheoryData { get; } = new()
    {
        { "af", new(1, 5, "een uur vyf") },
        { "ar", new(1, 5, "الواحدة وخمس دقائق صباحًا") },
        { "az", new(1, 5, "bir saat beş dəqiqə") },
        { "bg", new(1, 5, "един час и пет минути") },
        { "bn", new(1, 5, "রাত একটা পাঁচ") },
        { "ca", new(1, 5, "la una i cinc de la matinada") },
        { "cs", new(1, 5, "jedna hodina pět minut") },
        { "da", new(1, 5, "en nul fem") },
        { "de", new(1, 5, "fünf nach eins") },
        { "de-CH", new(1, 5, "fünf nach eins") },
        { "de-LI", new(1, 5, "fünf nach eins") },
        { "el", new(1, 5, "μία και πέντε το πρωί") },
        { "en", new(1, 5, "five past one") },
        { "en-GB", new(1, 5, "five past one") },
        { "en-IN", new(1, 5, "five past one") },
        { "en-US", new(1, 5, "five past one") },
        { "es", new(1, 5, "la una y cinco de la madrugada") },
        { "fa", new(1, 5, "یک و پنج بامداد") },
        { "fi", new(1, 5, "yksi nolla viisi") },
        { "fil", new(1, 5, "ala una singko ng umaga") },
        { "fr", new(1, 5, "une heure cinq") },
        { "fr-BE", new(1, 5, "une heure cinq") },
        { "fr-CH", new(1, 5, "une heure cinq") },
        { "he", new(1, 5, "אחת וחמש") },
        { "hr", new(1, 5, "jedan sat i pet minuta") },
        { "hu", new(1, 5, "egy óra öt perc") },
        { "hy", new(1, 5, "մեկն անց հինգ") },
        { "id", new(1, 5, "pukul satu lewat lima menit") },
        { "is", new(1, 5, "eitt núll fimm") },
        { "it", new(1, 5, "una e cinque") },
        { "ja", new(1, 5, "1時5分") },
        { "ko", new(1, 5, "오전 한 시 오 분") },
        { "ku", new(1, 5, "یەکی بەیانی و پێنج خولەک") },
        { "lb", new(1, 5, "fënnef op eng") },
        { "lt", new(1, 5, "viena valanda penkios minutės") },
        { "lv", new(1, 5, "viens un piecas") },
        { "ms", new(1, 5, "pukul satu lima pagi") },
        { "mt", new(1, 5, "is-siegħa waħda u ħamsa") },
        { "nb", new(1, 5, "ett null fem") },
        { "nl", new(1, 5, "een uur vijf") },
        { "nn", new(1, 5, "eit null fem") },
        { "pl", new(1, 5, "pierwsza pięć") },
        { "pt", new(1, 5, "uma e cinco") },
        { "pt-BR", new(1, 5, "uma e cinco") },
        { "ro", new(1, 5, "ora unu și cinci") },
        { "ru", new(1, 5, "час пять") },
        { "sk", new(1, 5, "jedna hodina päť minút") },
        { "sl", new(1, 5, "ena pet") },
        { "sr", new(1, 5, "један и пет") },
        { "sr-Latn", new(1, 5, "jedan i pet") },
        { "sv", new(1, 5, "ett noll fem") },
        { "ta", new(1, 5, "அதிகாலை ஒரு மணி ஐந்து நிமிடம்") },
        { "th", new(1, 5, "ตีหนึ่งห้านาที") },
        { "tr", new(1, 5, "bir sıfır beş") },
        { "uk", new(1, 5, "перша п’ять") },
        { "ur", new(1, 5, "ایک بج کر پانچ منٹ صبح سویرے") },
        { "uz-Cyrl-UZ", new(1, 5, "бирдан беш ўтди") },
        { "uz-Latn-UZ", new(1, 5, "birdan besh o‘tdi") },
        { "vi", new(1, 5, "một giờ năm phút sáng") },
        { "zh-CN", new(1, 5, "凌晨一点零五分") },
        { "zh-Hans", new(1, 5, "凌晨一点零五分") },
        { "zh-Hant", new(1, 5, "凌晨一點零五分") },
        { "zu-ZA", new(1, 5, "ihora lokuqala nemizuzu emihlanu") }
    };

    public static TheoryData<string, ClockExpectationRow> TimeOnlyToClockNotationAdditionalExactExpectationTheoryData { get; } = new()
    {
        { "az", new(13, 0, "on üç saat") },
        { "ca", new(0, 0, "mitjanit") },
        { "ca", new(0, 7, "les dotze i set de la nit") },
        { "ca", new(1, 11, "la una i onze de la matinada") },
        { "ca", new(4, 0, "les quatre de la matinada") },
        { "ca", new(5, 1, "les cinc i un de la matinada") },
        { "ca", new(6, 0, "les sis del matí") },
        { "ca", new(6, 5, "les sis i cinc del matí") },
        { "ca", new(7, 10, "les set i deu del matí") },
        { "ca", new(8, 15, "les vuit i quart del matí") },
        { "ca", new(9, 20, "les nou i vint del matí") },
        { "ca", new(10, 25, "les deu i vint-i-cinc del matí") },
        { "ca", new(11, 30, "les onze i mitja del matí") },
        { "ca", new(12, 0, "migdia") },
        { "ca", new(12, 35, "la una menys vint-i-cinc de la tarda") },
        { "ca", new(12, 38, "les dotze i trenta-vuit de la tarda") },
        { "ca", new(15, 40, "les quatre menys vint de la tarda") },
        { "ca", new(17, 45, "les sis menys quart de la tarda") },
        { "ca", new(19, 50, "les vuit menys deu de la tarda") },
        { "ca", new(21, 0, "les nou de la nit") },
        { "ca", new(21, 55, "les deu menys cinc de la nit") },
        { "ca", new(22, 59, "les deu i cinquanta-nou de la nit") },
        { "ca", new(23, 43, "les onze i quaranta-tres de la nit") },
        { "de", new(0, 0, "zwölf Uhr nachts") },
        { "de", new(4, 0, "vier Uhr") },
        { "de", new(5, 1, "fünf Uhr eins") },
        { "de", new(6, 5, "fünf nach sechs") },
        { "de", new(7, 10, "zehn nach sieben") },
        { "de", new(8, 15, "viertel nach acht") },
        { "de", new(9, 20, "zwanzig nach neun") },
        { "de", new(10, 25, "fünf vor halb elf") },
        { "de", new(11, 30, "halb zwölf") },
        { "de", new(12, 0, "zwölf Uhr mittags") },
        { "de", new(15, 35, "fünf nach halb vier") },
        { "de", new(16, 40, "zwanzig vor fünf") },
        { "de", new(17, 45, "viertel vor sechs") },
        { "de", new(18, 50, "zehn vor sieben") },
        { "de", new(19, 55, "fünf vor acht") },
        { "de", new(20, 59, "acht Uhr neunundfünfzig") },
        { "el", new(13, 0, "μία το απόγευμα") },
        { "en", new(0, 0, "midnight") },
        { "en", new(4, 0, "four o'clock") },
        { "en", new(6, 5, "five past six") },
        { "en", new(7, 10, "ten past seven") },
        { "en", new(8, 15, "a quarter past eight") },
        { "en", new(9, 20, "twenty past nine") },
        { "en", new(10, 25, "twenty-five past ten") },
        { "en", new(11, 30, "half past eleven") },
        { "en", new(12, 0, "noon") },
        { "en", new(15, 35, "three thirty-five") },
        { "en", new(16, 40, "twenty to five") },
        { "en", new(17, 45, "a quarter to six") },
        { "en", new(18, 50, "ten to seven") },
        { "en", new(19, 55, "five to eight") },
        { "en", new(20, 59, "eight fifty-nine") },
        { "es", new(0, 0, "medianoche") },
        { "es", new(0, 7, "las doce y siete de la noche") },
        { "es", new(1, 11, "la una y once de la madrugada") },
        { "es", new(4, 0, "las cuatro de la madrugada") },
        { "es", new(5, 1, "las cinco y uno de la madrugada") },
        { "es", new(6, 0, "las seis de la mañana") },
        { "es", new(6, 5, "las seis y cinco de la mañana") },
        { "es", new(7, 10, "las siete y diez de la mañana") },
        { "es", new(8, 15, "las ocho y cuarto de la mañana") },
        { "es", new(9, 20, "las nueve y veinte de la mañana") },
        { "es", new(10, 25, "las diez y veinticinco de la mañana") },
        { "es", new(11, 30, "las once y media de la mañana") },
        { "es", new(12, 0, "mediodía") },
        { "es", new(12, 35, "la una menos veinticinco de la tarde") },
        { "es", new(12, 38, "las doce y treinta y ocho de la tarde") },
        { "es", new(15, 40, "las cuatro menos veinte de la tarde") },
        { "es", new(17, 45, "las seis menos cuarto de la tarde") },
        { "es", new(19, 50, "las ocho menos diez de la tarde") },
        { "es", new(21, 0, "las nueve de la noche") },
        { "es", new(21, 55, "las diez menos cinco de la noche") },
        { "es", new(22, 59, "las diez y cincuenta y nueve de la noche") },
        { "es", new(23, 43, "las once y cuarenta y tres de la noche") },
        { "fr", new(0, 0, "minuit") },
        { "fr", new(0, 7, "minuit sept") },
        { "fr", new(1, 11, "une heure onze") },
        { "fr", new(4, 0, "quatre heures") },
        { "fr", new(5, 1, "cinq heures une") },
        { "fr", new(6, 5, "six heures cinq") },
        { "fr", new(7, 10, "sept heures dix") },
        { "fr", new(8, 15, "huit heures quinze") },
        { "fr", new(9, 20, "neuf heures vingt") },
        { "fr", new(10, 25, "dix heures vingt-cinq") },
        { "fr", new(11, 30, "onze heures trente") },
        { "fr", new(12, 0, "midi") },
        { "fr", new(12, 38, "midi trente-huit") },
        { "fr", new(15, 35, "quinze heures trente-cinq") },
        { "fr", new(16, 40, "seize heures quarante") },
        { "fr", new(17, 45, "dix-sept heures quarante-cinq") },
        { "fr", new(18, 50, "dix-huit heures cinquante") },
        { "fr", new(19, 55, "dix-neuf heures cinquante-cinq") },
        { "fr", new(20, 59, "vingt heures cinquante-neuf") },
        { "hu", new(13, 0, "tizenhárom óra") },
        { "hy", new(13, 0, "մեկն") },
        { "ja", new(0, 0, "0時0分") },
        { "ja", new(15, 45, "15時45分") },
        { "lb", new(0, 0, "Mëtternuecht") },
        { "lb", new(0, 7, "siwe Minutten op zwielef") },
        { "lb", new(1, 11, "eelef Minutten op eng") },
        { "lb", new(4, 0, "véier Auer") },
        { "lb", new(5, 1, "eng Minutt op fënnef") },
        { "lb", new(6, 5, "fënnef op sechs") },
        { "lb", new(7, 10, "zéng op siwen") },
        { "lb", new(8, 15, "Véirel op aacht") },
        { "lb", new(9, 20, "zwanzeg op néng") },
        { "lb", new(10, 25, "fënnef vir hallwer eelef") },
        { "lb", new(11, 30, "hallwer zwielef") },
        { "lb", new(12, 0, "Mëtteg") },
        { "lb", new(12, 39, "eenanzwanzeg Minutten vir eng") },
        { "lb", new(14, 32, "zwou Minutten op hallwer dräi") },
        { "lb", new(15, 35, "fënnef op hallwer véier") },
        { "lb", new(16, 40, "zwanzeg vir fënnef") },
        { "lb", new(17, 45, "Véirel vir sechs") },
        { "lb", new(18, 50, "zéng vir siwen") },
        { "lb", new(19, 52, "aacht Minutten vir aacht") },
        { "lb", new(20, 55, "fënnef vir néng") },
        { "lb", new(21, 58, "zwou Minutten vir zéng") },
        { "lb", new(22, 59, "eng Minutt vir eelef") },
        { "pt", new(0, 0, "meia-noite") },
        { "pt", new(4, 0, "quatro horas") },
        { "pt", new(6, 5, "seis e cinco") },
        { "pt", new(7, 10, "sete e dez") },
        { "pt", new(8, 15, "oito e um quarto") },
        { "pt", new(9, 20, "nove e vinte") },
        { "pt", new(10, 25, "dez e vinte e cinco") },
        { "pt", new(11, 30, "onze e meia") },
        { "pt", new(12, 0, "meio-dia") },
        { "pt", new(15, 35, "três e trinta e cinco") },
        { "pt", new(16, 40, "cinco menos vinte") },
        { "pt", new(17, 45, "seis menos um quarto") },
        { "pt", new(18, 50, "sete menos dez") },
        { "pt", new(19, 55, "oito menos cinco") },
        { "pt", new(20, 59, "oito e cinquenta e nove") },
        { "pt-BR", new(0, 0, "meia-noite") },
        { "pt-BR", new(4, 0, "quatro em ponto") },
        { "pt-BR", new(6, 5, "seis e cinco") },
        { "pt-BR", new(7, 10, "sete e dez") },
        { "pt-BR", new(8, 15, "oito e quinze") },
        { "pt-BR", new(9, 20, "nove e vinte") },
        { "pt-BR", new(10, 25, "dez e vinte e cinco") },
        { "pt-BR", new(11, 30, "onze e meia") },
        { "pt-BR", new(12, 0, "meio-dia") },
        { "pt-BR", new(15, 35, "três e trinta e cinco") },
        { "pt-BR", new(16, 40, "vinte para as cinco") },
        { "pt-BR", new(17, 45, "quinze para as seis") },
        { "pt-BR", new(18, 50, "dez para as sete") },
        { "pt-BR", new(19, 55, "cinco para as oito") },
        { "pt-BR", new(20, 59, "oito e cinquenta e nove") }
    };

    public static TheoryData<string, ClockExpectationRow> TimeOnlyToClockNotationAdditionalRoundedExpectationTheoryData { get; } = new()
    {
        { "ca", new(0, 0, "mitjanit") },
        { "ca", new(0, 7, "les dotze i cinc de la nit") },
        { "ca", new(1, 11, "la una i deu de la matinada") },
        { "ca", new(4, 0, "les quatre de la matinada") },
        { "ca", new(5, 1, "les cinc de la matinada") },
        { "ca", new(6, 0, "les sis del matí") },
        { "ca", new(6, 5, "les sis i cinc del matí") },
        { "ca", new(7, 10, "les set i deu del matí") },
        { "ca", new(8, 15, "les vuit i quart del matí") },
        { "ca", new(9, 20, "les nou i vint del matí") },
        { "ca", new(10, 25, "les deu i vint-i-cinc del matí") },
        { "ca", new(11, 30, "les onze i mitja del matí") },
        { "ca", new(12, 0, "migdia") },
        { "ca", new(12, 35, "la una menys vint-i-cinc de la tarda") },
        { "ca", new(12, 38, "la una menys vint de la tarda") },
        { "ca", new(15, 40, "les quatre menys vint de la tarda") },
        { "ca", new(17, 45, "les sis menys quart de la tarda") },
        { "ca", new(19, 50, "les vuit menys deu de la tarda") },
        { "ca", new(21, 0, "les nou de la nit") },
        { "ca", new(21, 55, "les deu menys cinc de la nit") },
        { "ca", new(22, 59, "les onze de la nit") },
        { "ca", new(23, 43, "les dotze menys quart de la nit") },
        { "de", new(0, 0, "zwölf Uhr nachts") },
        { "de", new(4, 0, "vier Uhr") },
        { "de", new(5, 1, "fünf Uhr") },
        { "de", new(6, 5, "fünf nach sechs") },
        { "de", new(7, 10, "zehn nach sieben") },
        { "de", new(8, 15, "viertel nach acht") },
        { "de", new(9, 20, "zwanzig nach neun") },
        { "de", new(10, 25, "fünf vor halb elf") },
        { "de", new(11, 30, "halb zwölf") },
        { "de", new(12, 0, "zwölf Uhr mittags") },
        { "de", new(14, 32, "halb drei") },
        { "de", new(15, 35, "fünf nach halb vier") },
        { "de", new(16, 40, "zwanzig vor fünf") },
        { "de", new(17, 45, "viertel vor sechs") },
        { "de", new(18, 50, "zehn vor sieben") },
        { "de", new(19, 55, "fünf vor acht") },
        { "de", new(20, 59, "neun Uhr") },
        { "en", new(0, 0, "midnight") },
        { "en", new(4, 0, "four o'clock") },
        { "en", new(5, 1, "five o'clock") },
        { "en", new(6, 5, "five past six") },
        { "en", new(7, 10, "ten past seven") },
        { "en", new(8, 15, "a quarter past eight") },
        { "en", new(9, 20, "twenty past nine") },
        { "en", new(10, 25, "twenty-five past ten") },
        { "en", new(11, 30, "half past eleven") },
        { "en", new(12, 0, "noon") },
        { "en", new(14, 32, "half past two") },
        { "en", new(15, 35, "three thirty-five") },
        { "en", new(16, 40, "twenty to five") },
        { "en", new(17, 45, "a quarter to six") },
        { "en", new(18, 50, "ten to seven") },
        { "en", new(19, 55, "five to eight") },
        { "en", new(20, 59, "nine o'clock") },
        { "es", new(0, 0, "medianoche") },
        { "es", new(0, 7, "las doce y cinco de la noche") },
        { "es", new(1, 11, "la una y diez de la madrugada") },
        { "es", new(4, 0, "las cuatro de la madrugada") },
        { "es", new(5, 1, "las cinco de la madrugada") },
        { "es", new(6, 0, "las seis de la mañana") },
        { "es", new(6, 5, "las seis y cinco de la mañana") },
        { "es", new(7, 10, "las siete y diez de la mañana") },
        { "es", new(8, 15, "las ocho y cuarto de la mañana") },
        { "es", new(9, 20, "las nueve y veinte de la mañana") },
        { "es", new(10, 25, "las diez y veinticinco de la mañana") },
        { "es", new(11, 30, "las once y media de la mañana") },
        { "es", new(12, 0, "mediodía") },
        { "es", new(12, 35, "la una menos veinticinco de la tarde") },
        { "es", new(12, 38, "la una menos veinte de la tarde") },
        { "es", new(15, 40, "las cuatro menos veinte de la tarde") },
        { "es", new(17, 45, "las seis menos cuarto de la tarde") },
        { "es", new(19, 50, "las ocho menos diez de la tarde") },
        { "es", new(21, 0, "las nueve de la noche") },
        { "es", new(21, 55, "las diez menos cinco de la noche") },
        { "es", new(22, 59, "las once de la noche") },
        { "es", new(23, 43, "las doce menos cuarto de la noche") },
        { "fr", new(0, 0, "minuit") },
        { "fr", new(0, 7, "minuit cinq") },
        { "fr", new(1, 11, "une heure dix") },
        { "fr", new(4, 0, "quatre heures") },
        { "fr", new(5, 1, "cinq heures") },
        { "fr", new(6, 5, "six heures cinq") },
        { "fr", new(7, 10, "sept heures dix") },
        { "fr", new(8, 15, "huit heures quinze") },
        { "fr", new(9, 20, "neuf heures vingt") },
        { "fr", new(10, 25, "dix heures vingt-cinq") },
        { "fr", new(11, 30, "onze heures trente") },
        { "fr", new(12, 0, "midi") },
        { "fr", new(12, 38, "midi quarante") },
        { "fr", new(14, 32, "quatorze heures trente") },
        { "fr", new(15, 35, "quinze heures trente-cinq") },
        { "fr", new(16, 40, "seize heures quarante") },
        { "fr", new(17, 45, "dix-sept heures quarante-cinq") },
        { "fr", new(18, 50, "dix-huit heures cinquante") },
        { "fr", new(19, 55, "dix-neuf heures cinquante-cinq") },
        { "fr", new(20, 59, "vingt et une heures") },
        { "ja", new(23, 58, "0時0分") },
        { "lb", new(0, 0, "Mëtternuecht") },
        { "lb", new(0, 7, "fënnef op zwielef") },
        { "lb", new(1, 11, "zéng op eng") },
        { "lb", new(4, 0, "véier Auer") },
        { "lb", new(5, 1, "fënnef Auer") },
        { "lb", new(6, 5, "fënnef op sechs") },
        { "lb", new(7, 10, "zéng op siwen") },
        { "lb", new(8, 15, "Véirel op aacht") },
        { "lb", new(9, 20, "zwanzeg op néng") },
        { "lb", new(10, 25, "fënnef vir hallwer eelef") },
        { "lb", new(11, 30, "hallwer zwielef") },
        { "lb", new(12, 0, "Mëtteg") },
        { "lb", new(12, 39, "zwanzeg vir eng") },
        { "lb", new(14, 32, "hallwer dräi") },
        { "lb", new(15, 35, "fënnef op hallwer véier") },
        { "lb", new(16, 40, "zwanzeg vir fënnef") },
        { "lb", new(17, 45, "Véirel vir sechs") },
        { "lb", new(18, 50, "zéng vir siwen") },
        { "lb", new(19, 52, "zéng vir aacht") },
        { "lb", new(20, 55, "fënnef vir néng") },
        { "lb", new(21, 58, "zéng Auer") },
        { "lb", new(22, 59, "eelef Auer") },
        { "pt", new(0, 0, "meia-noite") },
        { "pt", new(4, 0, "quatro horas") },
        { "pt", new(5, 1, "cinco horas") },
        { "pt", new(6, 5, "seis e cinco") },
        { "pt", new(7, 10, "sete e dez") },
        { "pt", new(8, 15, "oito e um quarto") },
        { "pt", new(9, 20, "nove e vinte") },
        { "pt", new(10, 25, "dez e vinte e cinco") },
        { "pt", new(11, 30, "onze e meia") },
        { "pt", new(12, 0, "meio-dia") },
        { "pt", new(14, 32, "duas e meia") },
        { "pt", new(15, 35, "três e trinta e cinco") },
        { "pt", new(16, 40, "cinco menos vinte") },
        { "pt", new(17, 45, "seis menos um quarto") },
        { "pt", new(18, 50, "sete menos dez") },
        { "pt", new(19, 55, "oito menos cinco") },
        { "pt", new(20, 59, "nove horas") },
        { "pt-BR", new(0, 0, "meia-noite") },
        { "pt-BR", new(4, 0, "quatro em ponto") },
        { "pt-BR", new(5, 1, "cinco em ponto") },
        { "pt-BR", new(6, 5, "seis e cinco") },
        { "pt-BR", new(7, 10, "sete e dez") },
        { "pt-BR", new(8, 15, "oito e quinze") },
        { "pt-BR", new(9, 20, "nove e vinte") },
        { "pt-BR", new(10, 25, "dez e vinte e cinco") },
        { "pt-BR", new(11, 30, "onze e meia") },
        { "pt-BR", new(12, 0, "meio-dia") },
        { "pt-BR", new(14, 32, "duas e meia") },
        { "pt-BR", new(15, 35, "três e trinta e cinco") },
        { "pt-BR", new(16, 40, "vinte para as cinco") },
        { "pt-BR", new(17, 45, "quinze para as seis") },
        { "pt-BR", new(18, 50, "dez para as sete") },
        { "pt-BR", new(19, 55, "cinco para as oito") },
        { "pt-BR", new(20, 59, "nove em ponto") }
    };
#endif

    public static TheoryData<string, string, string> FormatterExpectationTheoryData =>
        new()
        {
            { "af", "gister", "2 dae" },
            { "ar", "أمس", "يومين" },
            { "az", "dünən", "2 gün" },
            { "bg", "вчера", "2 дни" },
            { "bn", "গতকাল", "2 দিন" },
            { "ca", "ahir", "2 dies" },
            { "cs", "včera", "2 dny" },
            { "da", "i går", "2 dage" },
            { "de", "gestern", "2 Tage" },
            { "de-CH", "gestern", "2 Tage" },
            { "de-LI", "gestern", "2 Tage" },
            { "el", "χθες", "2 μέρες" },
            { "en", "yesterday", "2 days" },
            { "en-GB", "yesterday", "2 days" },
            { "en-IN", "yesterday", "2 days" },
            { "en-US", "yesterday", "2 days" },
            { "es", "ayer", "2 días" },
            { "fa", "دیروز", "2 روز" },
            { "fi", "eilen", "2 päivää" },
            { "fil", "kahapon", "2 araw" },
            { "fr", "hier", "2 jours" },
            { "fr-BE", "hier", "2 jours" },
            { "fr-CH", "hier", "2 jours" },
            { "he", "אתמול", "יומיים" },
            { "hr", "jučer", "2 dana" },
            { "hu", "tegnap", "2 nap" },
            { "hy", "երեկ", "2 օր" },
            { "id", "kemarin", "2 hari" },
            { "is", "í gær", "2 dagar" },
            { "it", "ieri", "2 giorni" },
            { "ja", "昨日", "2 日" },
            { "ko", "어제", "2일" },
            { "ku", "دوێنێ", "2 ڕۆژ" },
            { "lb", "gëschter", "2 Deeg" },
            { "lt", "vakar", "2 dienos" },
            { "lv", "vakardien", "2 dienas" },
            { "ms", "semalam", "2 hari" },
            { "mt", "il-bieraħ", "jumejn" },
            { "nb", "i går", "2 dager" },
            { "nl", "gisteren", "2 dagen" },
            { "nn", "i går", "2 dager" },
            { "pl", "wczoraj", "2 dni" },
            { "pt", "ontem", "2 dias" },
            { "pt-BR", "ontem", "2 dias" },
            { "ro", "ieri", "2 zile" },
            { "ru", "вчера", "2 дня" },
            { "sk", "včera", "2 dni" },
            { "sl", "včeraj", "2 dneva" },
            { "sr", "јуче", "2 дана" },
            { "sr-Latn", "juče", "2 dana" },
            { "sv", "igår", "2 dagar" },
            { "ta", "நேற்று", "2 நாட்கள்" },
            { "th", "เมื่อวานนี้", "2 วัน" },
            { "tr", "dün", "2 gün" },
            { "uk", "вчора", "2 дні" },
            { "uz-Cyrl-UZ", "кеча", "2 кун" },
            { "uz-Latn-UZ", "kecha", "2 kun" },
            { "vi", "hôm qua", "2 ngày" },
            { "zh-CN", "昨天", "2 天" },
            { "zh-Hans", "昨天", "2 天" },
            { "zh-Hant", "昨天", "2 天" },
            { "zu-ZA", "izolo", "2 izinsuku" }
        };

    public static TheoryData<string, string, string> CollectionFormatterExpectationTheoryData =>
        new()
        {
            { "af", "1 en 2", "1, 2 en 3" },
            { "ar", "1 و2", "1, 2 و3" },
            { "az", "1 və 2", "1, 2 və 3" },
            { "bg", "1 и 2", "1, 2 и 3" },
            { "bn", "1 ও 2", "1, 2 ও 3" },
            { "ca", "1 i 2", "1, 2 i 3" },
            { "cs", "1 a 2", "1, 2 a 3" },
            { "da", "1 og 2", "1, 2 og 3" },
            { "de", "1 und 2", "1, 2 und 3" },
            { "de-CH", "1 und 2", "1, 2 und 3" },
            { "de-LI", "1 und 2", "1, 2 und 3" },
            { "el", "1 και 2", "1, 2 και 3" },
            { "en", "1 and 2", "1, 2, and 3" },
            { "en-GB", "1 and 2", "1, 2, and 3" },
            { "en-IN", "1 and 2", "1, 2, and 3" },
            { "en-US", "1 and 2", "1, 2, and 3" },
            { "es", "1 y 2", "1, 2 y 3" },
            { "fa", "1 و2", "1, 2 و3" },
            { "fi", "1 ja 2", "1, 2 ja 3" },
            { "fil", "1 at 2", "1, 2 at 3" },
            { "fr", "1 et 2", "1, 2 et 3" },
            { "fr-BE", "1 et 2", "1, 2 et 3" },
            { "fr-CH", "1 et 2", "1, 2 et 3" },
            { "he", "1 ו2", "1, 2 ו3" },
            { "hr", "1 i 2", "1, 2 i 3" },
            { "hu", "1 és 2", "1, 2 és 3" },
            { "hy", "1 և 2", "1, 2 և 3" },
            { "id", "1 dan 2", "1, 2 dan 3" },
            { "is", "1 og 2", "1, 2 og 3" },
            { "it", "1 e 2", "1, 2 e 3" },
            { "ja", "1、2", "1、2、3" },
            { "ko", "1 및 2", "1, 2 및 3" },
            { "ku", "1 û 2", "1, 2 û 3" },
            { "lb", "1 an 2", "1, 2 an 3" },
            { "lt", "1 ir 2", "1, 2 ir 3" },
            { "lv", "1 un 2", "1, 2 un 3" },
            { "ms", "1 dan 2", "1, 2 dan 3" },
            { "mt", "1 u 2", "1, 2 u 3" },
            { "nb", "1 og 2", "1, 2 og 3" },
            { "nl", "1 en 2", "1, 2 en 3" },
            { "nn", "1 og 2", "1, 2 og 3" },
            { "pl", "1 i 2", "1, 2 i 3" },
            { "pt", "1 e 2", "1, 2 e 3" },
            { "pt-BR", "1 e 2", "1, 2 e 3" },
            { "ro", "1 și 2", "1, 2 și 3" },
            { "ru", "1 и 2", "1, 2 и 3" },
            { "sk", "1 a 2", "1, 2 a 3" },
            { "sl", "1 in 2", "1, 2 in 3" },
            { "sr", "1 и 2", "1, 2 и 3" },
            { "sr-Latn", "1 i 2", "1, 2 i 3" },
            { "sv", "1 och 2", "1, 2 och 3" },
            { "ta", "1 மற்றும் 2", "1, 2 மற்றும் 3" },
            { "th", "1 และ 2", "1, 2 และ 3" },
            { "tr", "1 ve 2", "1, 2 ve 3" },
            { "uk", "1 і 2", "1, 2 і 3" },
            { "uz-Cyrl-UZ", "1 ва 2", "1, 2 ва 3" },
            { "uz-Latn-UZ", "1 va 2", "1, 2 va 3" },
            { "vi", "1 và 2", "1, 2 và 3" },
            { "zh-CN", "1、2", "1、2、3" },
            { "zh-Hans", "1、2", "1、2、3" },
            { "zh-Hant", "1、2", "1、2、3" },
            { "zu-ZA", "1 na 2", "1, 2 na 3" }
        };

    public static TheoryData<string, int, string> NumberToWordsOrdinalExpectationTheoryData =>
        new()
        {
            { "af", 1, "eerste" },
            { "ar", 1, "الأولى" },
            { "az", 1, "birinci" },
            { "bg", 1, "първа" },
            { "bn", 1, "প্রথম" },
            { "ca", 1, "primera" },
            { "cs", 1, "1" },
            { "da", 1, "første" },
            { "de", 1, "erste" },
            { "de-CH", 1, "erste" },
            { "de-LI", 1, "erste" },
            { "el", 1, "πρώτος" },
            { "en", 1, "first" },
            { "en-GB", 1, "first" },
            { "en-IN", 1, "one" },
            { "en-US", 1, "first" },
            { "es", 1, "primera" },
            { "fa", 1, "اول" },
            { "fi", 1, "ensimmäinen" },
            { "fil", 1, "una" },
            { "fr", 1, "première" },
            { "fr-BE", 1, "première" },
            { "fr-CH", 1, "première" },
            { "he", 1, "1" },
            { "hr", 1, "1" },
            { "hu", 1, "első" },
            { "hy", 1, "առաջին" },
            { "id", 1, "pertama" },
            { "is", 1, "fyrsta" },
            { "it", 1, "prima" },
            { "ja", 1, "一番目" },
            { "ko", 1, "첫번째" },
            { "ku", 1, "یەکەم" },
            { "lb", 1, "éischt" },
            { "lt", 1, "pirma" },
            { "lv", 1, "pirmā" },
            { "ms", 1, "pertama" },
            { "mt", 1, "l-ewwel" },
            { "nb", 1, "første" },
            { "nl", 1, "eerste" },
            { "nn", 1, "første" },
            { "pl", 1, "1" },
            { "pt", 1, "primeira" },
            { "pt-BR", 1, "primeira" },
            { "ro", 1, "prima" },
            { "ru", 1, "первая" },
            { "sk", 1, "prvá" },
            { "sl", 1, "1" },
            { "sr", 1, "1" },
            { "sr-Latn", 1, "1" },
            { "sv", 1, "första" },
            { "ta", 1, "முதலாவது" },
            { "th", 1, "หนึ่ง" },
            { "tr", 1, "birinci" },
            { "uk", 1, "перша" },
            { "ur", 1, "پہلی" },
            { "uz-Cyrl-UZ", 1, "биринчи" },
            { "uz-Latn-UZ", 1, "birinchi" },
            { "vi", 1, "thứ nhất" },
            { "zh-CN", 1, "第 一" },
            { "zh-Hans", 1, "第 一" },
            { "zh-Hant", 1, "第 一" },
            { "zu-ZA", 1, "okokuqala" }
        };

    public static TheoryData<string, long, string> NumberToWordsCardinalExpectationTheoryData =>
        new()
        {
            { "af", 1, "een" },
            { "ar", 1, "واحد" },
            { "az", 1, "bir" },
            { "bg", 1, "едно" },
            { "bn", 1, "এক" },
            { "ca", 1, "un" },
            { "cs", 1, "jeden" },
            { "da", 1, "en" },
            { "de", 1, "eins" },
            { "de-CH", 1, "eins" },
            { "de-LI", 1, "eins" },
            { "el", 1, "ένα" },
            { "en", 1, "one" },
            { "en-GB", 1, "one" },
            { "en-IN", 1, "one" },
            { "en-US", 1, "one" },
            { "es", 1, "uno" },
            { "fa", 1, "یک" },
            { "fi", 1, "yksi" },
            { "fil", 1, "isa" },
            { "fr", 1, "un" },
            { "fr-BE", 1, "un" },
            { "fr-CH", 1, "un" },
            { "he", 1, "אחת" },
            { "hr", 1, "jedan" },
            { "hu", 1, "egy" },
            { "hy", 1, "մեկ" },
            { "id", 1, "satu" },
            { "is", 1, "einn" },
            { "it", 1, "uno" },
            { "ja", 1, "一" },
            { "ko", 1, "일" },
            { "ku", 1, "یەک" },
            { "lb", 1, "een" },
            { "lt", 1, "vienas" },
            { "lv", 1, "viens" },
            { "ms", 1, "satu" },
            { "mt", 1, "wieħed" },
            { "nb", 1, "en" },
            { "nl", 1, "een" },
            { "nn", 1, "en" },
            { "pl", 1, "jeden" },
            { "pt", 1, "um" },
            { "pt-BR", 1, "um" },
            { "ro", 1, "unu" },
            { "ru", 1, "один" },
            { "sk", 1, "jeden" },
            { "sl", 1, "ena" },
            { "sr", 1, "један" },
            { "sr-Latn", 1, "jedan" },
            { "sv", 1, "ett" },
            { "ta", 1, "ஒன்று" },
            { "th", 1, "หนึ่ง" },
            { "tr", 1, "bir" },
            { "uk", 1, "один" },
            { "ur", 1, "ایک" },
            { "uz-Cyrl-UZ", 1, "бир" },
            { "uz-Latn-UZ", 1, "bir" },
            { "vi", 1, "một" },
            { "zh-CN", 1, "一" },
            { "zh-Hans", 1, "一" },
            { "zh-Hant", 1, "一" },
            { "zu-ZA", 1, "kanye" }
        };

    public static TheoryData<string, int, string> OrdinalizerExpectationTheoryData => LocaleOrdinalizerMatrixData.OrdinalizerExpectationTheoryData;

    public static TheoryData<string, int, GrammaticalGender, string> OrdinalizerGenderExpectationTheoryData => LocaleOrdinalizerMatrixData.OrdinalizerGenderExpectationTheoryData;

    public static TheoryData<string, string, string> OrdinalizerDefaultExpectationTheoryData => LocaleOrdinalizerMatrixData.OrdinalizerDefaultExpectationTheoryData;

    public static TheoryData<string, int, string> OrdinalizerNegativeExpectationTheoryData => LocaleOrdinalizerMatrixData.OrdinalizerNegativeExpectationTheoryData;

    public static TheoryData<string, int, WordForm, string> OrdinalizerWordFormExpectationTheoryData => LocaleOrdinalizerMatrixData.OrdinalizerWordFormExpectationTheoryData;

    public static TheoryData<string, int, GrammaticalGender, WordForm, string> OrdinalizerWordFormGenderExpectationTheoryData => LocaleOrdinalizerMatrixData.OrdinalizerWordFormGenderExpectationTheoryData;

    public static TheoryData<string, string, GrammaticalGender, string> OrdinalizerStringExactExpectationTheoryData => LocaleOrdinalizerMatrixData.OrdinalizerStringExactExpectationTheoryData;

    public static TheoryData<string, int, GrammaticalGender, string> OrdinalizerNumberExactExpectationTheoryData => LocaleOrdinalizerMatrixData.OrdinalizerNumberExactExpectationTheoryData;

#if NET6_0_OR_GREATER
    public static TheoryData<string, DateExpectationRow> DateOnlyToOrdinalWords2022January25ExpectationTheoryData => DateToOrdinalWords2022January25ExpectationTheoryData;

    public static TheoryData<string, DateExpectationRow> DateOnlyToOrdinalWords2015January1ExpectationTheoryData => DateToOrdinalWords2015January1ExpectationTheoryData;

    public static TheoryData<string, DateExpectationRow> DateOnlyToOrdinalWords2015February3ExpectationTheoryData => DateToOrdinalWords2015February3ExpectationTheoryData;

    public static TheoryData<string, DateExpectationRow> DateOnlyToOrdinalWords2020February29ExpectationTheoryData => DateToOrdinalWords2020February29ExpectationTheoryData;

    public static TheoryData<string, DateExpectationRow> DateOnlyToOrdinalWords2015September4ExpectationTheoryData => DateToOrdinalWords2015September4ExpectationTheoryData;

    public static TheoryData<string, DateExpectationRow> DateOnlyToOrdinalWords1979November7ExpectationTheoryData => DateToOrdinalWords1979November7ExpectationTheoryData;

    public static TheoryData<string, DateExpectationRow> DateOnlyToOrdinalWords2020March2ExpectationTheoryData => DateToOrdinalWords2020March2ExpectationTheoryData;

    public static TheoryData<string, DateExpectationRow> DateOnlyToOrdinalWords2021October31ExpectationTheoryData => DateToOrdinalWords2021October31ExpectationTheoryData;

    public static TheoryData<string, DateExpectationRow> DateOnlyToOrdinalWords2024December31ExpectationTheoryData => DateToOrdinalWords2024December31ExpectationTheoryData;
#endif

    public static TheoryData<string, long, string> WordsToNumberExpectationTheoryData =>
        new()
        {
            { "af", 21, "een en twintig" },
            { "ar", 21, "واحد و عشرون" },
            { "az", 21, "iyirmi bir" },
            { "bg", 21, "двадесет и едно" },
            { "bn", 21, "একুশ" },
            { "ca", 21, "vint-i-u" },
            { "cs", 21, "dvacet jeden" },
            { "da", 21, "enogtyve" },
            { "de", 21, "einundzwanzig" },
            { "de-CH", 21, "einundzwanzig" },
            { "de-LI", 21, "einundzwanzig" },
            { "el", 21, "είκοσι ένα" },
            { "en", 21, "twenty-one" },
            { "en-GB", 21, "twenty-one" },
            { "en-IN", 21, "twenty-one" },
            { "en-US", 21, "twenty-one" },
            { "es", 21, "veintiuno" },
            { "fa", 21, "بیست و یک" },
            { "fi", 21, "kaksikymmentäyksi" },
            { "fil", 21, "dalawampu't isa" },
            { "fr", 21, "vingt et un" },
            { "fr-BE", 21, "vingt et un" },
            { "fr-CH", 21, "vingt et un" },
            { "he", 21, "עשרים ואחת" },
            { "hr", 21, "dvadeset jedan" },
            { "hu", 21, "huszonegy" },
            { "hy", 21, "քսանմեկ" },
            { "id", 21, "dua puluh satu" },
            { "is", 21, "tuttugu og einn" },
            { "it", 21, "ventuno" },
            { "ja", 21, "二十一" },
            { "ko", 21, "이십일" },
            { "ku", 21, "بیست و یەک" },
            { "lb", 21, "eenanzwanzeg" },
            { "lt", 21, "dvidešimt vienas" },
            { "lv", 21, "divdesmit viens" },
            { "ms", 21, "dua puluh satu" },
            { "mt", 21, "wieħed u għoxrin" },
            { "nb", 21, "tjueen" },
            { "nl", 21, "eenentwintig" },
            { "nn", 21, "tjueen" },
            { "pl", 21, "dwadzieścia jeden" },
            { "pt", 21, "vinte e um" },
            { "pt-BR", 21, "vinte e um" },
            { "ro", 21, "douăzeci și unu" },
            { "ru", 21, "двадцать один" },
            { "sk", 21, "dvadsať jeden" },
            { "sl", 21, "enaindvajset" },
            { "sr", 21, "двадесет један" },
            { "sr-Latn", 21, "dvadeset jedan" },
            { "sv", 21, "tjugoett" },
            { "ta", 21, "இருபத்து ஒன்று" },
            { "th", 21, "ยี่สิบหนึ่ง" },
            { "tr", 21, "yirmi bir" },
            { "uk", 21, "двадцять один" },
            { "ur", 21, "اکیس" },
            { "uz-Cyrl-UZ", 21, "йигирма бир" },
            { "uz-Latn-UZ", 21, "yigirma bir" },
            { "vi", 21, "hai mươi mốt" },
            { "zh-CN", 21, "二十一" },
            { "zh-Hans", 21, "二十一" },
            { "zh-Hant", 21, "二十一" },
            { "zu-ZA", 21, "amashumi amabili kanye" }
        };

    static string[] FindShippedLocales()
    {
        foreach (var root in new[] { AppContext.BaseDirectory, Environment.CurrentDirectory })
        {
            var directory = new DirectoryInfo(root);
            while (directory is not null)
            {
                var localeRoot = Path.Combine(directory.FullName, "src", "Humanizer", "Locales");
                if (Directory.Exists(localeRoot))
                {
                    return Directory
                        .GetFiles(localeRoot, "*.yml", SearchOption.TopDirectoryOnly)
                        .Select(Path.GetFileNameWithoutExtension)
                        .Where(static locale => !string.IsNullOrEmpty(locale))
                        .OrderBy(static locale => locale, StringComparer.Ordinal)
                        .ToArray()!;
                }

                directory = directory.Parent;
            }
        }

        throw new Xunit.Sdk.XunitException("Could not locate src/Humanizer/Locales.");
    }
}

#if NET7_0_OR_GREATER
public readonly record struct DateExpectationRow(int Year, int Month, int Day, string Expected) : IFormattable, IParsable<DateExpectationRow>
{
    public static DateExpectationRow Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);

        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        throw new FormatException($"Could not parse {nameof(DateExpectationRow)} value.");
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out DateExpectationRow result)
    {
        result = default;
        if (string.IsNullOrEmpty(s))
        {
            return false;
        }

        var parts = s.Split(';');
        if (parts.Length != 4 ||
            !int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var year) ||
            !int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var month) ||
            !int.TryParse(parts[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out var day))
        {
            return false;
        }

        try
        {
            result = new(year, month, day, Encoding.UTF8.GetString(Convert.FromBase64String(parts[3])));
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public override string ToString() => ToString(null, CultureInfo.InvariantCulture);

    public string ToString(string? format, IFormatProvider? formatProvider) =>
        string.Join(
            ";",
            Year.ToString(CultureInfo.InvariantCulture),
            Month.ToString(CultureInfo.InvariantCulture),
            Day.ToString(CultureInfo.InvariantCulture),
            Convert.ToBase64String(Encoding.UTF8.GetBytes(Expected)));
}
#else
public readonly record struct DateExpectationRow(int Year, int Month, int Day, string Expected);
#endif

#if NET6_0_OR_GREATER
#if NET7_0_OR_GREATER
public readonly record struct ClockExpectationRow(int Hours, int Minutes, string Expected) : IFormattable, IParsable<ClockExpectationRow>
{
    public static ClockExpectationRow Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);

        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        throw new FormatException($"Could not parse {nameof(ClockExpectationRow)} value.");
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out ClockExpectationRow result)
    {
        result = default;
        if (string.IsNullOrEmpty(s))
        {
            return false;
        }

        var parts = s.Split(';');
        if (parts.Length != 3 ||
            !int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var hours) ||
            !int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var minutes))
        {
            return false;
        }

        try
        {
            result = new(hours, minutes, Encoding.UTF8.GetString(Convert.FromBase64String(parts[2])));
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public override string ToString() => ToString(null, CultureInfo.InvariantCulture);

    public string ToString(string? format, IFormatProvider? formatProvider) =>
        string.Join(
            ";",
            Hours.ToString(CultureInfo.InvariantCulture),
            Minutes.ToString(CultureInfo.InvariantCulture),
            Convert.ToBase64String(Encoding.UTF8.GetBytes(Expected)));
}
#else
public readonly record struct ClockExpectationRow(int Hours, int Minutes, string Expected);
#endif
#endif

sealed class CultureSwap : IDisposable
{
    readonly CultureInfo originalCulture = CultureInfo.CurrentCulture;
    readonly CultureInfo originalUICulture = CultureInfo.CurrentUICulture;

    public CultureSwap(CultureInfo culture)
    {
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
    }

    public void Dispose()
    {
        CultureInfo.CurrentCulture = originalCulture;
        CultureInfo.CurrentUICulture = originalUICulture;
    }
}