namespace Humanizer;

internal enum InflectionUnicodeCase : byte
{
    None,
    Lower,
    Upper,
    Title
}
[Flags]
internal enum InflectionUnicodeScripts : uint
{
    None = 0,
    Arab = 1 << 0,
    Armn = 1 << 1,
    Beng = 1 << 2,
    Cyrl = 1 << 3,
    Deva = 1 << 4,
    Geor = 1 << 5,
    Grek = 1 << 6,
    Gujr = 1 << 7,
    Guru = 1 << 8,
    Hani = 1 << 9,
    Hebr = 1 << 10,
    Jpan = 1 << 11,
    Khmr = 1 << 12,
    Knda = 1 << 13,
    Kore = 1 << 14,
    Laoo = 1 << 15,
    Latn = 1 << 16,
    Mlym = 1 << 17,
    Mymr = 1 << 18,
    Orya = 1 << 19,
    Taml = 1 << 20,
    Telu = 1 << 21,
    Thai = 1 << 22,
    Ethi = 1 << 23,
    Mong = 1 << 24,
    Sinh = 1 << 25,
    Han = Hani | Jpan | Kore,
    All = 0x03FFFFFF
}

/// <summary>
/// Provides the pinned Unicode data used by localized inflection validation.
/// </summary>
internal static class InflectionUnicodeData
{
    // <generated-unicode-letter-mark>
    // Unicode 16.0.0 letter and mark categories, compressed into 998 ranges.
    // UnicodeData.txt SHA-256:
    // ff58e5823bd095166564a006e47d111130813dcf8bf234ef79fa51a870edb48f.
    const int UnicodeLetterMarkRangeCount = 998;
    const string UnicodeLetterMarkData = """
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
""";

    static readonly int[] UnicodeLetterMarkRanges = DecodeUnicodeLetterMarkRanges();
    // </generated-unicode-letter-mark>

    // <generated-unicode-case-fold>
    // Unicode 16.0.0 CaseFolding.txt C+S mappings, compressed into 697
    // source ranges from 1,484 mappings. Source SHA-256:
    // 6f1f9c588eb4a5c718d9e8f93b782685e5c7fec872cf05e8e6878053599e09bb.
    const int SimpleCaseFoldRangeCount = 697;
    const string SimpleCaseFoldData =
        "QRlAdACODAsWQBgGQCgAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA" +
        "AgIAAgIAAgIAAgQAAgIAAgIAAgMAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgMAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA" +
        "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA8QEBAAICAAICAAICAJcEAgCkAwEAAgIAAgIAnAMBAAICAZoD" +
        "AgACAwCeAQEAlAMBAJYDAQACAgCaAwEAngMCAKYDAQCiAwEAAgQApgMBAKoDAgCsAwEAAgIAAgIAAgIAtAMBAAICALQDAwACAgC0" +
        "AwEAAgIBsgMCAAICAAICALYDAQACBAACCAAEAQACAgAEAQACAgAEAQACAgACAgACAgACAgACAgACAgACAgACAgACAwACAgACAgAC" +
        "AgACAgACAgACAgACAgACAgACAwAEAQACAgACAgDBAQEAbwEAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA" +
        "AgIAAgIAAgIAAgIAAgIAAgIAAgIAgwICAAICAAICAAICAAICAAICAAICAAICAAICAAIIANaoAQEAAgIAxQIBANCoAQMAAgIAhQMB" +
        "AIoBAQCOAQEAAgIAAgIAAgIAAgIAAvcBAOgBKwACAgACBAACCQDoAQcATAICSgQAgAECAX4DEEASCEAfAAINABABADsBADEEAB0B" +
        "ACsCAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAGsBAF8DAHcBAH8CAAICAA0BAAIDAoMCAw+gARAfQFAAAgIA" +
        "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgoAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA" +
        "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAHgEAAgIAAgIAAgIAAgIAAgIAAgIA" +
        "AgMAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA" +
        "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgMlYO8W" +
        "JcBxJwDAcQYAwHGrBgUPiBEAm2EBAJlhAQCHYQEBg2ECAIVhAQD3YAEAx2ABAIanBAEAAgcq/y4tAv8uwwIAAgIAAgIAAgIAAgIA" +
        "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA" +
        "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA" +
        "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgcAcwMA/XYCAAICAAIC" +
        "AAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAIC" +
        "AAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAAIKBw8QBQ8QBw8QBw8Q" +
        "BQ8RAA8CAA8CAA8CAA8JBw8gBw8QBw8QBw8QAQ8CAZMBAgARAgCJcAoDqwEEABEHAIVxBQEPAgHHAQkA5XAFAQ8CAd8BAgANDAH/" +
        "AQIB+wECABGqAgC5dQQA/YIBAQCLgQEHADguDyAjAAKzBhk0yg4vYGAAAgIA7acBAQDLOwEAzacBAwACAgACAgACAgC3qAEBAPmn" +
        "AQEAvagBAQC7qAECAAIDAAIJAf2oAQIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA" +
        "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA" +
        "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgkAAgIAAgUAAs7yAQACAgACAgACAgACAgACAgACAgACAgACAgACAgACAgACAgACAgACAgAC" +
        "AgACAgACAgACAgACAgACAgACAgACAgACAgACFAACAgACAgACAgACAgACAgACAgACAgACAgACAgACAgACAgACAgACAgACiAEAAgIA" +
        "AgIAAgIAAgIAAgIAAgIAAgQAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIA" +
        "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAAgsAAgIAAgIAh6gEAQACAgACAgACAgACAgACBQACAgDPlAUDAAICAAIE" +
        "AAICAAICAAICAAICAAICAAICAAICAAICAAICAAICAIeVBQEAnZUFAQCVlQUBAIGVBQEAh5UFAgCjlAUBANOUBQEAqZQFAQDADgEA" +
        "AgIAAgIAAgIAAgIAAgIAAgIAAgIAAgIAXwEAhZUFAQDvqAQBAAICAAICAM2VBQEAAgQAAgYAAgIAAgIAAgIAgZkFGQAC+wZPn98E" +
        "lZ8BAAKcCBlA3wknULABI1DAAQpODA5OEAZOCAFO7A0ygAHQARVA0BYfQKCrAR9AwPUBIUQ=";

    static readonly int[] SimpleCaseFoldRanges = DecodeMappingRanges(
        SimpleCaseFoldData,
        SimpleCaseFoldRangeCount);
    // </generated-unicode-case-fold>

    // <generated-unicode-uppercase>
    // Unicode 16.0.0 UnicodeData.txt simple uppercase mappings, compressed
    // into 690 source ranges from 1,477 mappings. Source SHA-256:
    // ff58e5823bd095166564a006e47d111130813dcf8bf234ef79fa51a870edb48f.
    const int SimpleUppercaseRangeCount = 690;
    const string SimpleUppercaseData =
        "YRk/VADOCysWPxgGPwcA8gECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAEC" +
        "AAECAAECAAECAAECAAECAM8DAgABAgABAgABAwABAgABAgABAgABAgABAgABAgABAgABAwABAgABAgABAgABAgABAgABAgABAgAB" +
        "AgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAwABAgABAgABAQDXBAEAhgMDAAECAAEDAAEEAAEG" +
        "AAEDAMIBBAABAQDGAgEAgpkFAwCEAgMAAQIAAQIAAQMAAQUAAQMAAQQAAQIAAQMAAQQAAQIAcAYAAQEAAwIAAQEAAwIAAQEAAwIA" +
        "AQIAAQIAAQIAAQIAAQIAAQIAAQIAAQEAnQECAAECAAECAAECAAECAAECAAECAAECAAECAAEDAAEBAAMCAAEEAAECAAECAAECAAEC" +
        "AAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAEEAAECAAECAAECAAECAAECAAECAAECAAECAAEJ" +
        "AAEDAf6oAQMAAQUAAQIAAQIAAQIAAQIAAQEAvqgBAQC4qAEBALyoAQEAowMBAJsDAgGZAwMAkwMCAJUDAQCelQUEAJkDAQCWlQUC" +
        "AJ0DAQDOlQUBANCUBQEAiJUFAgChAwEApQMBAIiVBQEA7qcBAQCClQUDAKUDAgD6pwEBAKkDAwCrAwgAzqcBAwCzAwIAhpUFAQCz" +
        "AwQA1JQFAQCzAwEAiQEBAbEDAgCNAQYAtQMLAKqUBQEApJQFpwEAqAEsAAECAAEEAAEEAoQCMQBLAQJJBBA/EQA9AQg/CQB/AQF9" +
        "AwB7AQBxBABdAQBrAQAPAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAQCrAQEAnwEBAA4BAOcBAgC/AQMAAQMA" +
        "ATUfPyAPnwERAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAEKAAECAAECAAECAAECAAEC" +
        "AAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAEDAAECAAECAAEC" +
        "AAECAAECAAECAAEBAB0CAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAEC" +
        "AAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAEC" +
        "AAECAAECAAEyJV/vFiqALy0CgC/7BQUPiBEA22EBANlhAQDHYQEBw2ECAMVhAQC3YQEAyWABAISnBAIAAe8BAIioBAQAzDsRAPCo" +
        "BHMAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIA" +
        "AQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIA" +
        "AQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIA" +
        "AQYAdQYAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIA" +
        "AQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQIAAQEH" +
        "EBAFEBAHEBAHEBAFEBEAEAIAEAIAEAIAEAkHEBABlAECA6wBBAHIAQIBgAICAeABAgH8AQQHEBAHEBAHEBABEAMAEgsAyXAFABIN" +
        "ARAQARAFAA4OABLbAgA3Ig8fFAABzAYZM+AOL18xAAEEANWoAQEAz6gBAgABAgABAgABBwABAwABCwABAgABAgABAgABAgABAgAB" +
        "AgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgAB" +
        "AgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABCQABAgABBQABDSW/cScAv3EG" +
        "AL9xlPIBAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAEU" +
        "AAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAECAAGIAQABAgABAgABAgABAgABAgABAgABBAABAgABAgABAgAB" +
        "AgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgABAgAB" +
        "AgABAgABCwABAgABAwABAgABAgABAgABAgABBQABBQABAgABAQBgAwABAgABAgABAgABAgABAgABAgABAgABAgABAgABDAABAgAB" +
        "AgABAgABAgABAgABAgABAgABBQABAgABAwABBAABBgABAgABAgABGwAB3QYAvw4dT5/fBNGnARk/5wknT7ABI0+/AQpNDA5NEAZN" +
        "CAFNhQ4yf7ABFT/QFh8/oKsBHz/C9QEhQw==";

    static readonly int[] SimpleUppercaseRanges = DecodeMappingRanges(
        SimpleUppercaseData,
        SimpleUppercaseRangeCount);
    // </generated-unicode-uppercase>

    // <generated-unicode-case-categories>
    // Unicode 16.0.0 lowercase, uppercase, and titlecase categories, compressed
    // into 1,323 ranges. UnicodeData.txt SHA-256:
    // ff58e5823bd095166564a006e47d111130813dcf8bf234ef79fa51a870edb48f.
    const int UnicodeCaseRangeCount = 1323;
    const string UnicodeCaseData = """
QRkCIBkBVAABCxYCGAYCBxcBGQcBCAACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQAB
AQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQAC
AQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQEBAgACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQAB
AQACAQABAQACAQEBAgACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQAC
AQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQAB
AQECAgABAQACAQABAQACAQIBAwECAgABAQACAQABAQECAgABAQICAwEBAgMCBAABAQECAgABAQICAwIBAwECAgABAQECAgABAQAC
AQABAQACAQABAQECAgABAQACAQEBAgACAQABAQECAgABAQICAwABAQACAQABAQECAgEBAwACAQIBBwACAQADAQABAQACAQADAQAB
AQACAQADAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQEBAgACAQABAQACAQABAQACAQAB
AQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQEBAgACAQADAQABAQACAQABAQICAwABAQACAQABAQACAQABAQACAQAB
AQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQAC
AQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQAB
AQACAQYBBwECAgABAQECAgEBAgACAQABAQMCBAABAQACAQABAQACAQABAQACAQABAQACAUQBRhoB2wEAAgEAAQEAAgEAAQMAAgEA
AQQCAQQAAgcAAgICAgQAAgIBAgIAAQEQAhIIAgkiASMAAgEBAQICAgMCAQMAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEA
AgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEEAQUAAgEAAQIAAgEAAQEBAgIBAQIyAjMvATAAAgEAAQEAAgEA
AQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEA
AgEAAQEAAgEAAQEAAgEAAQkAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEA
AQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEA
AgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEBAgIAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEBAQIAAgEA
AQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEA
AgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEA
AQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEA
AgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQIlAi8oAcAWJQInAAIGAAID
KgEtAgGjBVUCWAUBiBEIAQkAAgEAAQYqAi0CAkMrAWsMAQ4hAYcBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEB
AAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIB
AAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEB
AAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIB
AAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEB
AAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIB
AAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBCAEJAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEB
AAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIB
AAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEB
AAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIB
AAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBAAEBAAIBCAEJBwIIBQEIBQIIBwEIBwIIBwEIBwIIBQEIBQIIBwEJAAICAAICAAICAAIB
BwEIBwIIDQEQBwEIBwMIBwEIBwMIBwEIBwMIBAEGAQECAwIEAAMCAAEEAgEEAQECAwIEAAMEAwEGAQECAwIIBwEIBAIKAgEEAQEC
AwIEAAOGAgACBQACAwABAQICAwEBAgICAwABAgACBAQCCwACAgACAgACAgMCBQABAQMCBAABBQABAwEBAgECBwACAQMBCAABNQAC
AQAB/BQvAjAvATAAAgEAAQECAgMBAQIAAgEAAQEAAgEAAQEAAgEAAQEDAgQAAQEAAgEBAQIAAgEFAQgCAgMAAQEAAgEAAQEAAgEA
AQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEA
AgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEA
AQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEA
AgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEBAQgAAgEAAQEAAgEAAQQAAgEA
AQ0lAScAAQYAAZPyAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQAC
AQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQAB
EwACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQACAQABAQAC
AQABAQACAQABhwEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgECAQMAAgEAAQEAAgEAAQEAAgEAAQEAAgEA
AQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEA
AgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEA
AQEAAgEAAQEAAgEAAQIHAQgAAgEAAQEAAgEAAQEBAgIAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQQAAgEAAQEAAgEAAQIAAgEA
AQEAAgECAQMAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEEAgUAAQEE
AgUAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEAAgEAAQEDAgQAAQEAAgEAAQEBAgIAAQMAAgEAAQIAAQIA
AQEAAgEAAQEAAgEAAQEAAgEAAQEAAhkAAgEAAQQAAbYGKgEwCAEQTwGQnwEGARMEAY4IGQIgGQG/CScCKCcBiAEjAigjAZgBCgIM
DgIQBgIIAQIDCgEMDgEQBgEIAQHFDTICQDIBkAEVAiAVAbAWHwIgHwGAqwEfAiAfAaDLARkCGhkBGhkCGgYBCBEBEhkCGhkBGgAC
AgECBAACAwECBAMCBQcCCAMBBQABAgYBCAoBCxkCGhkBGgECAwMCBgcCCQYCCBkBGgECAwMCBQQCBgACBAYCCBkBGhkCGhkBGhkC
GhkBGhkCGhkBGhkCGhkBGhkCGhkBGhkCGhsBHhgCGhgBGgUBBhgCGhgBGgUBBhgCGhgBGgUBBhgCGhgBGgUBBhgCGhgBGgUBBgAC
AQABtQ4JAQsTARoFAdsTIQIiIQE=
""";

    static readonly int[] UnicodeCaseRanges = DecodeUnicodeCaseRanges();
    // </generated-unicode-case-categories>

    internal static InflectionUnicodeCase GetCase(int scalar)
    {
        var low = 0;
        var high = UnicodeCaseRangeCount - 1;
        while (low <= high)
        {
            var middle = low + ((high - low) / 2);
            var offset = middle * 3;
            if (scalar < UnicodeCaseRanges[offset])
            {
                high = middle - 1;
            }
            else if (scalar > UnicodeCaseRanges[offset + 1])
            {
                low = middle + 1;
            }
            else
            {
                return (InflectionUnicodeCase)UnicodeCaseRanges[offset + 2];
            }
        }

        return InflectionUnicodeCase.None;
    }

    static int[] DecodeUnicodeCaseRanges()
    {
        var bytes = Convert.FromBase64String(UnicodeCaseData);
        var ranges = new int[UnicodeCaseRangeCount * 3];
        var byteIndex = 0;
        var previousFirst = 0;
        for (var rangeIndex = 0; rangeIndex < UnicodeCaseRangeCount; rangeIndex++)
        {
            var offset = rangeIndex * 3;
            var first = previousFirst + ReadVarUInt(bytes, ref byteIndex);
            ranges[offset] = first;
            ranges[offset + 1] = first + ReadVarUInt(bytes, ref byteIndex);
            ranges[offset + 2] = ReadVarUInt(bytes, ref byteIndex);
            previousFirst = first;
        }

        return ranges;
    }

    internal static int FoldSimpleCase(int scalar) =>
        MapSimple(scalar, SimpleCaseFoldRanges, SimpleCaseFoldRangeCount);

    internal static int ToUpperSimple(int scalar) =>
        MapSimple(scalar, SimpleUppercaseRanges, SimpleUppercaseRangeCount);

    internal static int CompareSimpleCase(string left, string right)
    {
        var leftIndex = 0;
        var rightIndex = 0;
        while (leftIndex < left.Length && rightIndex < right.Length)
        {
            var leftScalar = FoldSimpleCase(ReadScalar(left, ref leftIndex));
            var rightScalar = FoldSimpleCase(ReadScalar(right, ref rightIndex));
            if (leftScalar != rightScalar)
            {
                return leftScalar.CompareTo(rightScalar);
            }
        }

        return leftIndex < left.Length
            ? 1
            : rightIndex < right.Length
                ? -1
                : 0;
    }

    internal sealed class SimpleCaseComparer : StringComparer
    {
        internal static readonly SimpleCaseComparer Instance = new();

        public override int Compare(string? left, string? right)
        {
            if (ReferenceEquals(left, right))
            {
                return 0;
            }

            if (left is null)
            {
                return -1;
            }

            return right is null
                ? 1
                : CompareSimpleCase(left, right);
        }

        public override bool Equals(string? left, string? right) =>
            Compare(left, right) == 0;

        public override int GetHashCode(string value)
        {
            var hash = 17;
            for (var index = 0; index < value.Length;)
            {
                hash = unchecked(
                    (hash * 31) +
                    FoldSimpleCase(ReadScalar(value, ref index)));
            }

            return hash;
        }
    }

    static int MapSimple(int scalar, int[] ranges, int rangeCount)
    {
        var low = 0;
        var high = rangeCount - 1;
        while (low <= high)
        {
            var middle = low + ((high - low) / 2);
            var offset = middle * 3;
            var first = ranges[offset];
            var last = ranges[offset + 1];
            if (scalar < first)
            {
                high = middle - 1;
            }
            else if (scalar > last)
            {
                low = middle + 1;
            }
            else
            {
                return scalar + ranges[offset + 2];
            }
        }

        return scalar;
    }

    static int[] DecodeMappingRanges(string data, int rangeCount)
    {
        var bytes = Convert.FromBase64String(data);
        var ranges = new int[rangeCount * 3];
        var byteIndex = 0;
        var previousFirst = 0;
        for (var rangeIndex = 0; rangeIndex < rangeCount; rangeIndex++)
        {
            var offset = rangeIndex * 3;
            var first = previousFirst + ReadVarUInt(bytes, ref byteIndex);
            var length = ReadVarUInt(bytes, ref byteIndex);
            var encodedDelta = ReadVarUInt(bytes, ref byteIndex);
            ranges[offset] = first;
            ranges[offset + 1] = first + length;
            ranges[offset + 2] = (encodedDelta & 1) == 0
                ? encodedDelta / 2
                : -((encodedDelta + 1) / 2);
            previousFirst = first;
        }

        return ranges;
    }

    static int ReadScalar(string value, ref int index)
    {
        var first = value[index++];
        if (char.IsHighSurrogate(first) &&
            index < value.Length &&
            char.IsLowSurrogate(value[index]))
        {
            return char.ConvertToUtf32(first, value[index++]);
        }

        return first;
    }

    internal static bool TryGetScript(
        string script,
        out InflectionUnicodeScripts value)
    {
        value = script switch
        {
            "Arab" => InflectionUnicodeScripts.Arab,
            "Armn" => InflectionUnicodeScripts.Armn,
            "Beng" => InflectionUnicodeScripts.Beng,
            "Cyrl" => InflectionUnicodeScripts.Cyrl,
            "Deva" => InflectionUnicodeScripts.Deva,
            "Ethi" => InflectionUnicodeScripts.Ethi,
            "Geor" => InflectionUnicodeScripts.Geor,
            "Grek" => InflectionUnicodeScripts.Grek,
            "Gujr" => InflectionUnicodeScripts.Gujr,
            "Guru" => InflectionUnicodeScripts.Guru,
            "Hani" => InflectionUnicodeScripts.Hani,
            "Hebr" => InflectionUnicodeScripts.Hebr,
            "Jpan" => InflectionUnicodeScripts.Jpan,
            "Khmr" => InflectionUnicodeScripts.Khmr,
            "Knda" => InflectionUnicodeScripts.Knda,
            "Kore" => InflectionUnicodeScripts.Kore,
            "Laoo" => InflectionUnicodeScripts.Laoo,
            "Latn" => InflectionUnicodeScripts.Latn,
            "Mlym" => InflectionUnicodeScripts.Mlym,
            "Mong" => InflectionUnicodeScripts.Mong,
            "Mymr" => InflectionUnicodeScripts.Mymr,
            "Orya" => InflectionUnicodeScripts.Orya,
            "Sinh" => InflectionUnicodeScripts.Sinh,
            "Taml" => InflectionUnicodeScripts.Taml,
            "Telu" => InflectionUnicodeScripts.Telu,
            "Thai" => InflectionUnicodeScripts.Thai,
            _ => InflectionUnicodeScripts.None
        };
        return value != InflectionUnicodeScripts.None;
    }

    internal static InflectionUnicodeScripts GetScripts(int scalar)
    {
        var low = 0;
        var high = UnicodeScriptRanges.Length - 1;
        while (low <= high)
        {
            var middle = low + ((high - low) / 2);
            var range = UnicodeScriptRanges[middle];
            if (scalar < range.First)
            {
                high = middle - 1;
            }
            else if (scalar > range.Last)
            {
                low = middle + 1;
            }
            else
            {
                return range.Scripts;
            }
        }

        return InflectionUnicodeScripts.None;
    }

    // <generated-unicode-scripts>
    // Checked generated table from Unicode 16.0.0 Scripts.txt
    // (SHA-256 9e88f0a677df47311106340be8ede2ecdacd9c1c931831218d2be6d5508e0039)
    // and ScriptExtensions.txt
    // (SHA-256 049117ce26b9769fe2749b06eef51a50a89faef4a97764dd2d81daa715980700).
    // Common scalars without an exact supported Script_Extensions value are omitted;
    // unlisted Inherited scalars inherit the surrounding declared script.
    static readonly InflectionUnicodeScriptRange[] UnicodeScriptRanges =
    [
        new(0x0041, 0x005A, InflectionUnicodeScripts.Latn),
        new(0x0061, 0x007A, InflectionUnicodeScripts.Latn),
        new(0x00AA, 0x00AA, InflectionUnicodeScripts.Latn),
        new(0x00B7, 0x00B7, InflectionUnicodeScripts.Geor | InflectionUnicodeScripts.Grek | InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore | InflectionUnicodeScripts.Latn),
        new(0x00BA, 0x00BA, InflectionUnicodeScripts.Latn),
        new(0x00C0, 0x00D6, InflectionUnicodeScripts.Latn),
        new(0x00D8, 0x00F6, InflectionUnicodeScripts.Latn),
        new(0x00F8, 0x02B8, InflectionUnicodeScripts.Latn),
        new(0x02BC, 0x02BC, InflectionUnicodeScripts.Beng | InflectionUnicodeScripts.Cyrl | InflectionUnicodeScripts.Deva | InflectionUnicodeScripts.Latn | InflectionUnicodeScripts.Thai),
        new(0x02C7, 0x02C7, InflectionUnicodeScripts.Latn),
        new(0x02C9, 0x02CB, InflectionUnicodeScripts.Latn),
        new(0x02CD, 0x02CD, InflectionUnicodeScripts.Latn),
        new(0x02D7, 0x02D7, InflectionUnicodeScripts.Latn | InflectionUnicodeScripts.Thai),
        new(0x02D9, 0x02D9, InflectionUnicodeScripts.Latn),
        new(0x02E0, 0x02E4, InflectionUnicodeScripts.Latn),
        new(0x0300, 0x0301, InflectionUnicodeScripts.Cyrl | InflectionUnicodeScripts.Grek | InflectionUnicodeScripts.Latn),
        new(0x0302, 0x0302, InflectionUnicodeScripts.Cyrl | InflectionUnicodeScripts.Latn),
        new(0x0303, 0x0303, InflectionUnicodeScripts.Latn | InflectionUnicodeScripts.Thai),
        new(0x0304, 0x0304, InflectionUnicodeScripts.Cyrl | InflectionUnicodeScripts.Grek | InflectionUnicodeScripts.Latn),
        new(0x0305, 0x0305, InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Latn),
        new(0x0306, 0x0306, InflectionUnicodeScripts.Cyrl | InflectionUnicodeScripts.Grek | InflectionUnicodeScripts.Latn),
        new(0x0307, 0x0307, InflectionUnicodeScripts.Hebr | InflectionUnicodeScripts.Latn),
        new(0x0308, 0x0308, InflectionUnicodeScripts.Armn | InflectionUnicodeScripts.Cyrl | InflectionUnicodeScripts.Grek | InflectionUnicodeScripts.Hebr | InflectionUnicodeScripts.Latn),
        new(0x0309, 0x030A, InflectionUnicodeScripts.Latn),
        new(0x030B, 0x030B, InflectionUnicodeScripts.Cyrl | InflectionUnicodeScripts.Latn),
        new(0x030C, 0x030D, InflectionUnicodeScripts.Latn),
        new(0x030E, 0x030E, InflectionUnicodeScripts.Ethi | InflectionUnicodeScripts.Latn),
        new(0x030F, 0x030F, InflectionUnicodeScripts.All),
        new(0x0310, 0x0310, InflectionUnicodeScripts.Latn),
        new(0x0311, 0x0311, InflectionUnicodeScripts.Cyrl | InflectionUnicodeScripts.Latn),
        new(0x0312, 0x0312, InflectionUnicodeScripts.All),
        new(0x0313, 0x0313, InflectionUnicodeScripts.Grek | InflectionUnicodeScripts.Latn),
        new(0x0314, 0x031F, InflectionUnicodeScripts.All),
        new(0x0320, 0x0320, InflectionUnicodeScripts.Latn),
        new(0x0321, 0x0322, InflectionUnicodeScripts.All),
        new(0x0323, 0x0323, InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Latn),
        new(0x0324, 0x0325, InflectionUnicodeScripts.Latn),
        new(0x0326, 0x032C, InflectionUnicodeScripts.All),
        new(0x032D, 0x032E, InflectionUnicodeScripts.Latn),
        new(0x032F, 0x032F, InflectionUnicodeScripts.All),
        new(0x0330, 0x0330, InflectionUnicodeScripts.Latn),
        new(0x0331, 0x0331, InflectionUnicodeScripts.Latn | InflectionUnicodeScripts.Thai),
        new(0x0332, 0x0341, InflectionUnicodeScripts.All),
        new(0x0342, 0x0342, InflectionUnicodeScripts.Grek),
        new(0x0343, 0x0344, InflectionUnicodeScripts.All),
        new(0x0345, 0x0345, InflectionUnicodeScripts.Grek),
        new(0x0346, 0x0357, InflectionUnicodeScripts.All),
        new(0x0358, 0x0358, InflectionUnicodeScripts.Latn),
        new(0x0359, 0x035D, InflectionUnicodeScripts.All),
        new(0x035E, 0x035E, InflectionUnicodeScripts.Latn),
        new(0x035F, 0x0362, InflectionUnicodeScripts.All),
        new(0x0363, 0x036F, InflectionUnicodeScripts.Latn),
        new(0x0370, 0x0377, InflectionUnicodeScripts.Grek),
        new(0x037A, 0x037D, InflectionUnicodeScripts.Grek),
        new(0x037F, 0x037F, InflectionUnicodeScripts.Grek),
        new(0x0384, 0x0384, InflectionUnicodeScripts.Grek),
        new(0x0386, 0x0386, InflectionUnicodeScripts.Grek),
        new(0x0388, 0x038A, InflectionUnicodeScripts.Grek),
        new(0x038C, 0x038C, InflectionUnicodeScripts.Grek),
        new(0x038E, 0x03A1, InflectionUnicodeScripts.Grek),
        new(0x03A3, 0x03E1, InflectionUnicodeScripts.Grek),
        new(0x03F0, 0x03FF, InflectionUnicodeScripts.Grek),
        new(0x0400, 0x0484, InflectionUnicodeScripts.Cyrl),
        new(0x0485, 0x0486, InflectionUnicodeScripts.Cyrl | InflectionUnicodeScripts.Latn),
        new(0x0487, 0x052F, InflectionUnicodeScripts.Cyrl),
        new(0x0531, 0x0556, InflectionUnicodeScripts.Armn),
        new(0x0559, 0x0588, InflectionUnicodeScripts.Armn),
        new(0x0589, 0x0589, InflectionUnicodeScripts.Armn | InflectionUnicodeScripts.Geor),
        new(0x058A, 0x058A, InflectionUnicodeScripts.Armn),
        new(0x058D, 0x058F, InflectionUnicodeScripts.Armn),
        new(0x0591, 0x05C7, InflectionUnicodeScripts.Hebr),
        new(0x05D0, 0x05EA, InflectionUnicodeScripts.Hebr),
        new(0x05EF, 0x05F4, InflectionUnicodeScripts.Hebr),
        new(0x0600, 0x0604, InflectionUnicodeScripts.Arab),
        new(0x0606, 0x06DC, InflectionUnicodeScripts.Arab),
        new(0x06DE, 0x06FF, InflectionUnicodeScripts.Arab),
        new(0x0750, 0x077F, InflectionUnicodeScripts.Arab),
        new(0x0870, 0x088E, InflectionUnicodeScripts.Arab),
        new(0x0890, 0x0891, InflectionUnicodeScripts.Arab),
        new(0x0897, 0x08E1, InflectionUnicodeScripts.Arab),
        new(0x08E3, 0x08FF, InflectionUnicodeScripts.Arab),
        new(0x0900, 0x0950, InflectionUnicodeScripts.Deva),
        new(0x0951, 0x0952, InflectionUnicodeScripts.Beng | InflectionUnicodeScripts.Deva | InflectionUnicodeScripts.Gujr | InflectionUnicodeScripts.Guru | InflectionUnicodeScripts.Knda | InflectionUnicodeScripts.Latn | InflectionUnicodeScripts.Mlym | InflectionUnicodeScripts.Orya | InflectionUnicodeScripts.Taml | InflectionUnicodeScripts.Telu),
        new(0x0953, 0x0954, InflectionUnicodeScripts.All),
        new(0x0955, 0x0963, InflectionUnicodeScripts.Deva),
        new(0x0964, 0x0965, InflectionUnicodeScripts.Beng | InflectionUnicodeScripts.Deva | InflectionUnicodeScripts.Gujr | InflectionUnicodeScripts.Guru | InflectionUnicodeScripts.Knda | InflectionUnicodeScripts.Mlym | InflectionUnicodeScripts.Orya | InflectionUnicodeScripts.Sinh | InflectionUnicodeScripts.Taml | InflectionUnicodeScripts.Telu),
        new(0x0966, 0x097F, InflectionUnicodeScripts.Deva),
        new(0x0980, 0x0983, InflectionUnicodeScripts.Beng),
        new(0x0985, 0x098C, InflectionUnicodeScripts.Beng),
        new(0x098F, 0x0990, InflectionUnicodeScripts.Beng),
        new(0x0993, 0x09A8, InflectionUnicodeScripts.Beng),
        new(0x09AA, 0x09B0, InflectionUnicodeScripts.Beng),
        new(0x09B2, 0x09B2, InflectionUnicodeScripts.Beng),
        new(0x09B6, 0x09B9, InflectionUnicodeScripts.Beng),
        new(0x09BC, 0x09C4, InflectionUnicodeScripts.Beng),
        new(0x09C7, 0x09C8, InflectionUnicodeScripts.Beng),
        new(0x09CB, 0x09CE, InflectionUnicodeScripts.Beng),
        new(0x09D7, 0x09D7, InflectionUnicodeScripts.Beng),
        new(0x09DC, 0x09DD, InflectionUnicodeScripts.Beng),
        new(0x09DF, 0x09E3, InflectionUnicodeScripts.Beng),
        new(0x09E6, 0x09FE, InflectionUnicodeScripts.Beng),
        new(0x0A01, 0x0A03, InflectionUnicodeScripts.Guru),
        new(0x0A05, 0x0A0A, InflectionUnicodeScripts.Guru),
        new(0x0A0F, 0x0A10, InflectionUnicodeScripts.Guru),
        new(0x0A13, 0x0A28, InflectionUnicodeScripts.Guru),
        new(0x0A2A, 0x0A30, InflectionUnicodeScripts.Guru),
        new(0x0A32, 0x0A33, InflectionUnicodeScripts.Guru),
        new(0x0A35, 0x0A36, InflectionUnicodeScripts.Guru),
        new(0x0A38, 0x0A39, InflectionUnicodeScripts.Guru),
        new(0x0A3C, 0x0A3C, InflectionUnicodeScripts.Guru),
        new(0x0A3E, 0x0A42, InflectionUnicodeScripts.Guru),
        new(0x0A47, 0x0A48, InflectionUnicodeScripts.Guru),
        new(0x0A4B, 0x0A4D, InflectionUnicodeScripts.Guru),
        new(0x0A51, 0x0A51, InflectionUnicodeScripts.Guru),
        new(0x0A59, 0x0A5C, InflectionUnicodeScripts.Guru),
        new(0x0A5E, 0x0A5E, InflectionUnicodeScripts.Guru),
        new(0x0A66, 0x0A76, InflectionUnicodeScripts.Guru),
        new(0x0A81, 0x0A83, InflectionUnicodeScripts.Gujr),
        new(0x0A85, 0x0A8D, InflectionUnicodeScripts.Gujr),
        new(0x0A8F, 0x0A91, InflectionUnicodeScripts.Gujr),
        new(0x0A93, 0x0AA8, InflectionUnicodeScripts.Gujr),
        new(0x0AAA, 0x0AB0, InflectionUnicodeScripts.Gujr),
        new(0x0AB2, 0x0AB3, InflectionUnicodeScripts.Gujr),
        new(0x0AB5, 0x0AB9, InflectionUnicodeScripts.Gujr),
        new(0x0ABC, 0x0AC5, InflectionUnicodeScripts.Gujr),
        new(0x0AC7, 0x0AC9, InflectionUnicodeScripts.Gujr),
        new(0x0ACB, 0x0ACD, InflectionUnicodeScripts.Gujr),
        new(0x0AD0, 0x0AD0, InflectionUnicodeScripts.Gujr),
        new(0x0AE0, 0x0AE3, InflectionUnicodeScripts.Gujr),
        new(0x0AE6, 0x0AF1, InflectionUnicodeScripts.Gujr),
        new(0x0AF9, 0x0AFF, InflectionUnicodeScripts.Gujr),
        new(0x0B01, 0x0B03, InflectionUnicodeScripts.Orya),
        new(0x0B05, 0x0B0C, InflectionUnicodeScripts.Orya),
        new(0x0B0F, 0x0B10, InflectionUnicodeScripts.Orya),
        new(0x0B13, 0x0B28, InflectionUnicodeScripts.Orya),
        new(0x0B2A, 0x0B30, InflectionUnicodeScripts.Orya),
        new(0x0B32, 0x0B33, InflectionUnicodeScripts.Orya),
        new(0x0B35, 0x0B39, InflectionUnicodeScripts.Orya),
        new(0x0B3C, 0x0B44, InflectionUnicodeScripts.Orya),
        new(0x0B47, 0x0B48, InflectionUnicodeScripts.Orya),
        new(0x0B4B, 0x0B4D, InflectionUnicodeScripts.Orya),
        new(0x0B55, 0x0B57, InflectionUnicodeScripts.Orya),
        new(0x0B5C, 0x0B5D, InflectionUnicodeScripts.Orya),
        new(0x0B5F, 0x0B63, InflectionUnicodeScripts.Orya),
        new(0x0B66, 0x0B77, InflectionUnicodeScripts.Orya),
        new(0x0B82, 0x0B83, InflectionUnicodeScripts.Taml),
        new(0x0B85, 0x0B8A, InflectionUnicodeScripts.Taml),
        new(0x0B8E, 0x0B90, InflectionUnicodeScripts.Taml),
        new(0x0B92, 0x0B95, InflectionUnicodeScripts.Taml),
        new(0x0B99, 0x0B9A, InflectionUnicodeScripts.Taml),
        new(0x0B9C, 0x0B9C, InflectionUnicodeScripts.Taml),
        new(0x0B9E, 0x0B9F, InflectionUnicodeScripts.Taml),
        new(0x0BA3, 0x0BA4, InflectionUnicodeScripts.Taml),
        new(0x0BA8, 0x0BAA, InflectionUnicodeScripts.Taml),
        new(0x0BAE, 0x0BB9, InflectionUnicodeScripts.Taml),
        new(0x0BBE, 0x0BC2, InflectionUnicodeScripts.Taml),
        new(0x0BC6, 0x0BC8, InflectionUnicodeScripts.Taml),
        new(0x0BCA, 0x0BCD, InflectionUnicodeScripts.Taml),
        new(0x0BD0, 0x0BD0, InflectionUnicodeScripts.Taml),
        new(0x0BD7, 0x0BD7, InflectionUnicodeScripts.Taml),
        new(0x0BE6, 0x0BFA, InflectionUnicodeScripts.Taml),
        new(0x0C00, 0x0C0C, InflectionUnicodeScripts.Telu),
        new(0x0C0E, 0x0C10, InflectionUnicodeScripts.Telu),
        new(0x0C12, 0x0C28, InflectionUnicodeScripts.Telu),
        new(0x0C2A, 0x0C39, InflectionUnicodeScripts.Telu),
        new(0x0C3C, 0x0C44, InflectionUnicodeScripts.Telu),
        new(0x0C46, 0x0C48, InflectionUnicodeScripts.Telu),
        new(0x0C4A, 0x0C4D, InflectionUnicodeScripts.Telu),
        new(0x0C55, 0x0C56, InflectionUnicodeScripts.Telu),
        new(0x0C58, 0x0C5A, InflectionUnicodeScripts.Telu),
        new(0x0C5D, 0x0C5D, InflectionUnicodeScripts.Telu),
        new(0x0C60, 0x0C63, InflectionUnicodeScripts.Telu),
        new(0x0C66, 0x0C6F, InflectionUnicodeScripts.Telu),
        new(0x0C77, 0x0C7F, InflectionUnicodeScripts.Telu),
        new(0x0C80, 0x0C8C, InflectionUnicodeScripts.Knda),
        new(0x0C8E, 0x0C90, InflectionUnicodeScripts.Knda),
        new(0x0C92, 0x0CA8, InflectionUnicodeScripts.Knda),
        new(0x0CAA, 0x0CB3, InflectionUnicodeScripts.Knda),
        new(0x0CB5, 0x0CB9, InflectionUnicodeScripts.Knda),
        new(0x0CBC, 0x0CC4, InflectionUnicodeScripts.Knda),
        new(0x0CC6, 0x0CC8, InflectionUnicodeScripts.Knda),
        new(0x0CCA, 0x0CCD, InflectionUnicodeScripts.Knda),
        new(0x0CD5, 0x0CD6, InflectionUnicodeScripts.Knda),
        new(0x0CDD, 0x0CDE, InflectionUnicodeScripts.Knda),
        new(0x0CE0, 0x0CE3, InflectionUnicodeScripts.Knda),
        new(0x0CE6, 0x0CEF, InflectionUnicodeScripts.Knda),
        new(0x0CF1, 0x0CF3, InflectionUnicodeScripts.Knda),
        new(0x0D00, 0x0D0C, InflectionUnicodeScripts.Mlym),
        new(0x0D0E, 0x0D10, InflectionUnicodeScripts.Mlym),
        new(0x0D12, 0x0D44, InflectionUnicodeScripts.Mlym),
        new(0x0D46, 0x0D48, InflectionUnicodeScripts.Mlym),
        new(0x0D4A, 0x0D4F, InflectionUnicodeScripts.Mlym),
        new(0x0D54, 0x0D63, InflectionUnicodeScripts.Mlym),
        new(0x0D66, 0x0D7F, InflectionUnicodeScripts.Mlym),
        new(0x0D81, 0x0D83, InflectionUnicodeScripts.Sinh),
        new(0x0D85, 0x0D96, InflectionUnicodeScripts.Sinh),
        new(0x0D9A, 0x0DB1, InflectionUnicodeScripts.Sinh),
        new(0x0DB3, 0x0DBB, InflectionUnicodeScripts.Sinh),
        new(0x0DBD, 0x0DBD, InflectionUnicodeScripts.Sinh),
        new(0x0DC0, 0x0DC6, InflectionUnicodeScripts.Sinh),
        new(0x0DCA, 0x0DCA, InflectionUnicodeScripts.Sinh),
        new(0x0DCF, 0x0DD4, InflectionUnicodeScripts.Sinh),
        new(0x0DD6, 0x0DD6, InflectionUnicodeScripts.Sinh),
        new(0x0DD8, 0x0DDF, InflectionUnicodeScripts.Sinh),
        new(0x0DE6, 0x0DEF, InflectionUnicodeScripts.Sinh),
        new(0x0DF2, 0x0DF4, InflectionUnicodeScripts.Sinh),
        new(0x0E01, 0x0E3A, InflectionUnicodeScripts.Thai),
        new(0x0E40, 0x0E5B, InflectionUnicodeScripts.Thai),
        new(0x0E81, 0x0E82, InflectionUnicodeScripts.Laoo),
        new(0x0E84, 0x0E84, InflectionUnicodeScripts.Laoo),
        new(0x0E86, 0x0E8A, InflectionUnicodeScripts.Laoo),
        new(0x0E8C, 0x0EA3, InflectionUnicodeScripts.Laoo),
        new(0x0EA5, 0x0EA5, InflectionUnicodeScripts.Laoo),
        new(0x0EA7, 0x0EBD, InflectionUnicodeScripts.Laoo),
        new(0x0EC0, 0x0EC4, InflectionUnicodeScripts.Laoo),
        new(0x0EC6, 0x0EC6, InflectionUnicodeScripts.Laoo),
        new(0x0EC8, 0x0ECE, InflectionUnicodeScripts.Laoo),
        new(0x0ED0, 0x0ED9, InflectionUnicodeScripts.Laoo),
        new(0x0EDC, 0x0EDF, InflectionUnicodeScripts.Laoo),
        new(0x1000, 0x109F, InflectionUnicodeScripts.Mymr),
        new(0x10A0, 0x10C5, InflectionUnicodeScripts.Geor),
        new(0x10C7, 0x10C7, InflectionUnicodeScripts.Geor),
        new(0x10CD, 0x10CD, InflectionUnicodeScripts.Geor),
        new(0x10D0, 0x10FA, InflectionUnicodeScripts.Geor),
        new(0x10FB, 0x10FB, InflectionUnicodeScripts.Geor | InflectionUnicodeScripts.Latn),
        new(0x10FC, 0x10FF, InflectionUnicodeScripts.Geor),
        new(0x1100, 0x11FF, InflectionUnicodeScripts.Kore),
        new(0x1200, 0x1248, InflectionUnicodeScripts.Ethi),
        new(0x124A, 0x124D, InflectionUnicodeScripts.Ethi),
        new(0x1250, 0x1256, InflectionUnicodeScripts.Ethi),
        new(0x1258, 0x1258, InflectionUnicodeScripts.Ethi),
        new(0x125A, 0x125D, InflectionUnicodeScripts.Ethi),
        new(0x1260, 0x1288, InflectionUnicodeScripts.Ethi),
        new(0x128A, 0x128D, InflectionUnicodeScripts.Ethi),
        new(0x1290, 0x12B0, InflectionUnicodeScripts.Ethi),
        new(0x12B2, 0x12B5, InflectionUnicodeScripts.Ethi),
        new(0x12B8, 0x12BE, InflectionUnicodeScripts.Ethi),
        new(0x12C0, 0x12C0, InflectionUnicodeScripts.Ethi),
        new(0x12C2, 0x12C5, InflectionUnicodeScripts.Ethi),
        new(0x12C8, 0x12D6, InflectionUnicodeScripts.Ethi),
        new(0x12D8, 0x1310, InflectionUnicodeScripts.Ethi),
        new(0x1312, 0x1315, InflectionUnicodeScripts.Ethi),
        new(0x1318, 0x135A, InflectionUnicodeScripts.Ethi),
        new(0x135D, 0x137C, InflectionUnicodeScripts.Ethi),
        new(0x1380, 0x1399, InflectionUnicodeScripts.Ethi),
        new(0x1780, 0x17DD, InflectionUnicodeScripts.Khmr),
        new(0x17E0, 0x17E9, InflectionUnicodeScripts.Khmr),
        new(0x17F0, 0x17F9, InflectionUnicodeScripts.Khmr),
        new(0x1800, 0x1819, InflectionUnicodeScripts.Mong),
        new(0x1820, 0x1878, InflectionUnicodeScripts.Mong),
        new(0x1880, 0x18AA, InflectionUnicodeScripts.Mong),
        new(0x19E0, 0x19FF, InflectionUnicodeScripts.Khmr),
        new(0x1AB0, 0x1ACE, InflectionUnicodeScripts.All),
        new(0x1C80, 0x1C8A, InflectionUnicodeScripts.Cyrl),
        new(0x1C90, 0x1CBA, InflectionUnicodeScripts.Geor),
        new(0x1CBD, 0x1CBF, InflectionUnicodeScripts.Geor),
        new(0x1CD0, 0x1CD0, InflectionUnicodeScripts.Beng | InflectionUnicodeScripts.Deva | InflectionUnicodeScripts.Knda),
        new(0x1CD1, 0x1CD1, InflectionUnicodeScripts.Deva),
        new(0x1CD2, 0x1CD2, InflectionUnicodeScripts.Beng | InflectionUnicodeScripts.Deva | InflectionUnicodeScripts.Knda),
        new(0x1CD3, 0x1CD3, InflectionUnicodeScripts.Deva | InflectionUnicodeScripts.Knda),
        new(0x1CD4, 0x1CD4, InflectionUnicodeScripts.Deva),
        new(0x1CD5, 0x1CD6, InflectionUnicodeScripts.Beng | InflectionUnicodeScripts.Deva),
        new(0x1CD7, 0x1CD7, InflectionUnicodeScripts.Deva),
        new(0x1CD8, 0x1CD8, InflectionUnicodeScripts.Beng | InflectionUnicodeScripts.Deva),
        new(0x1CD9, 0x1CD9, InflectionUnicodeScripts.Deva),
        new(0x1CDA, 0x1CDA, InflectionUnicodeScripts.Deva | InflectionUnicodeScripts.Knda | InflectionUnicodeScripts.Mlym | InflectionUnicodeScripts.Orya | InflectionUnicodeScripts.Taml | InflectionUnicodeScripts.Telu),
        new(0x1CDB, 0x1CE0, InflectionUnicodeScripts.Deva),
        new(0x1CE1, 0x1CE1, InflectionUnicodeScripts.Beng | InflectionUnicodeScripts.Deva),
        new(0x1CE2, 0x1CE9, InflectionUnicodeScripts.Deva),
        new(0x1CEA, 0x1CEA, InflectionUnicodeScripts.Beng | InflectionUnicodeScripts.Deva),
        new(0x1CEB, 0x1CEC, InflectionUnicodeScripts.Deva),
        new(0x1CED, 0x1CED, InflectionUnicodeScripts.Beng | InflectionUnicodeScripts.Deva),
        new(0x1CEE, 0x1CF1, InflectionUnicodeScripts.Deva),
        new(0x1CF2, 0x1CF2, InflectionUnicodeScripts.Beng | InflectionUnicodeScripts.Deva | InflectionUnicodeScripts.Knda | InflectionUnicodeScripts.Mlym | InflectionUnicodeScripts.Orya | InflectionUnicodeScripts.Sinh | InflectionUnicodeScripts.Telu),
        new(0x1CF3, 0x1CF3, InflectionUnicodeScripts.Deva),
        new(0x1CF4, 0x1CF4, InflectionUnicodeScripts.Deva | InflectionUnicodeScripts.Knda),
        new(0x1CF5, 0x1CF6, InflectionUnicodeScripts.Beng | InflectionUnicodeScripts.Deva),
        new(0x1CF7, 0x1CF7, InflectionUnicodeScripts.Beng),
        new(0x1CF8, 0x1CF9, InflectionUnicodeScripts.Deva),
        new(0x1D00, 0x1D25, InflectionUnicodeScripts.Latn),
        new(0x1D26, 0x1D2A, InflectionUnicodeScripts.Grek),
        new(0x1D2B, 0x1D2B, InflectionUnicodeScripts.Cyrl),
        new(0x1D2C, 0x1D5C, InflectionUnicodeScripts.Latn),
        new(0x1D5D, 0x1D61, InflectionUnicodeScripts.Grek),
        new(0x1D62, 0x1D65, InflectionUnicodeScripts.Latn),
        new(0x1D66, 0x1D6A, InflectionUnicodeScripts.Grek),
        new(0x1D6B, 0x1D77, InflectionUnicodeScripts.Latn),
        new(0x1D78, 0x1D78, InflectionUnicodeScripts.Cyrl),
        new(0x1D79, 0x1DBE, InflectionUnicodeScripts.Latn),
        new(0x1DBF, 0x1DC1, InflectionUnicodeScripts.Grek),
        new(0x1DC2, 0x1DF7, InflectionUnicodeScripts.All),
        new(0x1DF8, 0x1DF8, InflectionUnicodeScripts.Cyrl | InflectionUnicodeScripts.Latn),
        new(0x1DF9, 0x1DF9, InflectionUnicodeScripts.All),
        new(0x1DFB, 0x1DFF, InflectionUnicodeScripts.All),
        new(0x1E00, 0x1EFF, InflectionUnicodeScripts.Latn),
        new(0x1F00, 0x1F15, InflectionUnicodeScripts.Grek),
        new(0x1F18, 0x1F1D, InflectionUnicodeScripts.Grek),
        new(0x1F20, 0x1F45, InflectionUnicodeScripts.Grek),
        new(0x1F48, 0x1F4D, InflectionUnicodeScripts.Grek),
        new(0x1F50, 0x1F57, InflectionUnicodeScripts.Grek),
        new(0x1F59, 0x1F59, InflectionUnicodeScripts.Grek),
        new(0x1F5B, 0x1F5B, InflectionUnicodeScripts.Grek),
        new(0x1F5D, 0x1F5D, InflectionUnicodeScripts.Grek),
        new(0x1F5F, 0x1F7D, InflectionUnicodeScripts.Grek),
        new(0x1F80, 0x1FB4, InflectionUnicodeScripts.Grek),
        new(0x1FB6, 0x1FC4, InflectionUnicodeScripts.Grek),
        new(0x1FC6, 0x1FD3, InflectionUnicodeScripts.Grek),
        new(0x1FD6, 0x1FDB, InflectionUnicodeScripts.Grek),
        new(0x1FDD, 0x1FEF, InflectionUnicodeScripts.Grek),
        new(0x1FF2, 0x1FF4, InflectionUnicodeScripts.Grek),
        new(0x1FF6, 0x1FFE, InflectionUnicodeScripts.Grek),
        new(0x200C, 0x200D, InflectionUnicodeScripts.All),
        new(0x202F, 0x202F, InflectionUnicodeScripts.Latn | InflectionUnicodeScripts.Mong),
        new(0x204F, 0x204F, InflectionUnicodeScripts.Arab),
        new(0x205A, 0x205A, InflectionUnicodeScripts.Geor),
        new(0x205D, 0x205D, InflectionUnicodeScripts.Grek),
        new(0x2071, 0x2071, InflectionUnicodeScripts.Latn),
        new(0x207F, 0x207F, InflectionUnicodeScripts.Latn),
        new(0x2090, 0x209C, InflectionUnicodeScripts.Latn),
        new(0x20D0, 0x20EF, InflectionUnicodeScripts.All),
        new(0x20F0, 0x20F0, InflectionUnicodeScripts.Deva | InflectionUnicodeScripts.Latn),
        new(0x2126, 0x2126, InflectionUnicodeScripts.Grek),
        new(0x212A, 0x212B, InflectionUnicodeScripts.Latn),
        new(0x2132, 0x2132, InflectionUnicodeScripts.Latn),
        new(0x214E, 0x214E, InflectionUnicodeScripts.Latn),
        new(0x2160, 0x2188, InflectionUnicodeScripts.Latn),
        new(0x2C60, 0x2C7F, InflectionUnicodeScripts.Latn),
        new(0x2D00, 0x2D25, InflectionUnicodeScripts.Geor),
        new(0x2D27, 0x2D27, InflectionUnicodeScripts.Geor),
        new(0x2D2D, 0x2D2D, InflectionUnicodeScripts.Geor),
        new(0x2D80, 0x2D96, InflectionUnicodeScripts.Ethi),
        new(0x2DA0, 0x2DA6, InflectionUnicodeScripts.Ethi),
        new(0x2DA8, 0x2DAE, InflectionUnicodeScripts.Ethi),
        new(0x2DB0, 0x2DB6, InflectionUnicodeScripts.Ethi),
        new(0x2DB8, 0x2DBE, InflectionUnicodeScripts.Ethi),
        new(0x2DC0, 0x2DC6, InflectionUnicodeScripts.Ethi),
        new(0x2DC8, 0x2DCE, InflectionUnicodeScripts.Ethi),
        new(0x2DD0, 0x2DD6, InflectionUnicodeScripts.Ethi),
        new(0x2DD8, 0x2DDE, InflectionUnicodeScripts.Ethi),
        new(0x2DE0, 0x2DFF, InflectionUnicodeScripts.Cyrl),
        new(0x2E17, 0x2E17, InflectionUnicodeScripts.Latn),
        new(0x2E31, 0x2E31, InflectionUnicodeScripts.Geor),
        new(0x2E41, 0x2E41, InflectionUnicodeScripts.Arab),
        new(0x2E43, 0x2E43, InflectionUnicodeScripts.Cyrl),
        new(0x2E80, 0x2E99, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x2E9B, 0x2EF3, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x2F00, 0x2FD5, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x2FF0, 0x2FFF, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x3001, 0x3002, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore | InflectionUnicodeScripts.Mong),
        new(0x3003, 0x3003, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x3005, 0x3007, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x3008, 0x300B, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore | InflectionUnicodeScripts.Mong),
        new(0x300C, 0x3011, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x3013, 0x301F, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x3021, 0x302D, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x302E, 0x302F, InflectionUnicodeScripts.Kore),
        new(0x3030, 0x3030, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x3031, 0x3035, InflectionUnicodeScripts.Jpan),
        new(0x3037, 0x303F, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x3041, 0x3096, InflectionUnicodeScripts.Jpan),
        new(0x3099, 0x30FA, InflectionUnicodeScripts.Jpan),
        new(0x30FB, 0x30FB, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x30FC, 0x30FF, InflectionUnicodeScripts.Jpan),
        new(0x3131, 0x318E, InflectionUnicodeScripts.Kore),
        new(0x3190, 0x319F, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x31C0, 0x31E5, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x31EF, 0x31EF, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x31F0, 0x31FF, InflectionUnicodeScripts.Jpan),
        new(0x3200, 0x321E, InflectionUnicodeScripts.Kore),
        new(0x3220, 0x3247, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x3260, 0x327E, InflectionUnicodeScripts.Kore),
        new(0x3280, 0x32B0, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x32C0, 0x32CB, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x32D0, 0x32FE, InflectionUnicodeScripts.Jpan),
        new(0x32FF, 0x32FF, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x3300, 0x3357, InflectionUnicodeScripts.Jpan),
        new(0x3358, 0x3370, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x337B, 0x337F, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x33E0, 0x33FE, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x3400, 0x4DBF, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x4E00, 0x9FFF, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0xA640, 0xA69F, InflectionUnicodeScripts.Cyrl),
        new(0xA700, 0xA707, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore | InflectionUnicodeScripts.Latn),
        new(0xA722, 0xA787, InflectionUnicodeScripts.Latn),
        new(0xA78B, 0xA7CD, InflectionUnicodeScripts.Latn),
        new(0xA7D0, 0xA7D1, InflectionUnicodeScripts.Latn),
        new(0xA7D3, 0xA7D3, InflectionUnicodeScripts.Latn),
        new(0xA7D5, 0xA7DC, InflectionUnicodeScripts.Latn),
        new(0xA7F2, 0xA7FF, InflectionUnicodeScripts.Latn),
        new(0xA830, 0xA832, InflectionUnicodeScripts.Deva | InflectionUnicodeScripts.Gujr | InflectionUnicodeScripts.Guru | InflectionUnicodeScripts.Knda | InflectionUnicodeScripts.Mlym),
        new(0xA833, 0xA835, InflectionUnicodeScripts.Deva | InflectionUnicodeScripts.Gujr | InflectionUnicodeScripts.Guru | InflectionUnicodeScripts.Knda),
        new(0xA836, 0xA839, InflectionUnicodeScripts.Deva | InflectionUnicodeScripts.Gujr | InflectionUnicodeScripts.Guru),
        new(0xA8E0, 0xA8F0, InflectionUnicodeScripts.Deva),
        new(0xA8F1, 0xA8F1, InflectionUnicodeScripts.Beng | InflectionUnicodeScripts.Deva),
        new(0xA8F2, 0xA8F2, InflectionUnicodeScripts.Deva),
        new(0xA8F3, 0xA8F3, InflectionUnicodeScripts.Deva | InflectionUnicodeScripts.Taml),
        new(0xA8F4, 0xA8FF, InflectionUnicodeScripts.Deva),
        new(0xA92E, 0xA92E, InflectionUnicodeScripts.Latn | InflectionUnicodeScripts.Mymr),
        new(0xA960, 0xA97C, InflectionUnicodeScripts.Kore),
        new(0xA9E0, 0xA9FE, InflectionUnicodeScripts.Mymr),
        new(0xAA60, 0xAA7F, InflectionUnicodeScripts.Mymr),
        new(0xAB01, 0xAB06, InflectionUnicodeScripts.Ethi),
        new(0xAB09, 0xAB0E, InflectionUnicodeScripts.Ethi),
        new(0xAB11, 0xAB16, InflectionUnicodeScripts.Ethi),
        new(0xAB20, 0xAB26, InflectionUnicodeScripts.Ethi),
        new(0xAB28, 0xAB2E, InflectionUnicodeScripts.Ethi),
        new(0xAB30, 0xAB5A, InflectionUnicodeScripts.Latn),
        new(0xAB5C, 0xAB64, InflectionUnicodeScripts.Latn),
        new(0xAB65, 0xAB65, InflectionUnicodeScripts.Grek),
        new(0xAB66, 0xAB69, InflectionUnicodeScripts.Latn),
        new(0xAC00, 0xD7A3, InflectionUnicodeScripts.Kore),
        new(0xD7B0, 0xD7C6, InflectionUnicodeScripts.Kore),
        new(0xD7CB, 0xD7FB, InflectionUnicodeScripts.Kore),
        new(0xF900, 0xFA6D, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0xFA70, 0xFAD9, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0xFB00, 0xFB06, InflectionUnicodeScripts.Latn),
        new(0xFB13, 0xFB17, InflectionUnicodeScripts.Armn),
        new(0xFB1D, 0xFB36, InflectionUnicodeScripts.Hebr),
        new(0xFB38, 0xFB3C, InflectionUnicodeScripts.Hebr),
        new(0xFB3E, 0xFB3E, InflectionUnicodeScripts.Hebr),
        new(0xFB40, 0xFB41, InflectionUnicodeScripts.Hebr),
        new(0xFB43, 0xFB44, InflectionUnicodeScripts.Hebr),
        new(0xFB46, 0xFB4F, InflectionUnicodeScripts.Hebr),
        new(0xFB50, 0xFBC2, InflectionUnicodeScripts.Arab),
        new(0xFBD3, 0xFD8F, InflectionUnicodeScripts.Arab),
        new(0xFD92, 0xFDC7, InflectionUnicodeScripts.Arab),
        new(0xFDCF, 0xFDCF, InflectionUnicodeScripts.Arab),
        new(0xFDF0, 0xFDFF, InflectionUnicodeScripts.Arab),
        new(0xFE00, 0xFE0F, InflectionUnicodeScripts.All),
        new(0xFE20, 0xFE2D, InflectionUnicodeScripts.All),
        new(0xFE2E, 0xFE2F, InflectionUnicodeScripts.Cyrl),
        new(0xFE45, 0xFE46, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0xFE70, 0xFE74, InflectionUnicodeScripts.Arab),
        new(0xFE76, 0xFEFC, InflectionUnicodeScripts.Arab),
        new(0xFF21, 0xFF3A, InflectionUnicodeScripts.Latn),
        new(0xFF41, 0xFF5A, InflectionUnicodeScripts.Latn),
        new(0xFF61, 0xFF65, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0xFF66, 0xFF9F, InflectionUnicodeScripts.Jpan),
        new(0xFFA0, 0xFFBE, InflectionUnicodeScripts.Kore),
        new(0xFFC2, 0xFFC7, InflectionUnicodeScripts.Kore),
        new(0xFFCA, 0xFFCF, InflectionUnicodeScripts.Kore),
        new(0xFFD2, 0xFFD7, InflectionUnicodeScripts.Kore),
        new(0xFFDA, 0xFFDC, InflectionUnicodeScripts.Kore),
        new(0x10140, 0x1018E, InflectionUnicodeScripts.Grek),
        new(0x101A0, 0x101A0, InflectionUnicodeScripts.Grek),
        new(0x101FD, 0x101FD, InflectionUnicodeScripts.All),
        new(0x102E0, 0x102FB, InflectionUnicodeScripts.Arab),
        new(0x10780, 0x10785, InflectionUnicodeScripts.Latn),
        new(0x10787, 0x107B0, InflectionUnicodeScripts.Latn),
        new(0x107B2, 0x107BA, InflectionUnicodeScripts.Latn),
        new(0x10E60, 0x10E7E, InflectionUnicodeScripts.Arab),
        new(0x10EC2, 0x10EC4, InflectionUnicodeScripts.Arab),
        new(0x10EFC, 0x10EFF, InflectionUnicodeScripts.Arab),
        new(0x111E1, 0x111F4, InflectionUnicodeScripts.Sinh),
        new(0x11301, 0x11301, InflectionUnicodeScripts.Taml),
        new(0x11303, 0x11303, InflectionUnicodeScripts.Taml),
        new(0x1133B, 0x1133C, InflectionUnicodeScripts.Taml),
        new(0x11660, 0x1166C, InflectionUnicodeScripts.Mong),
        new(0x116D0, 0x116E3, InflectionUnicodeScripts.Mymr),
        new(0x11B00, 0x11B09, InflectionUnicodeScripts.Deva),
        new(0x11FC0, 0x11FF1, InflectionUnicodeScripts.Taml),
        new(0x11FFF, 0x11FFF, InflectionUnicodeScripts.Taml),
        new(0x16FE2, 0x16FE3, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x16FF0, 0x16FF1, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x1AFF0, 0x1AFF3, InflectionUnicodeScripts.Jpan),
        new(0x1AFF5, 0x1AFFB, InflectionUnicodeScripts.Jpan),
        new(0x1AFFD, 0x1AFFE, InflectionUnicodeScripts.Jpan),
        new(0x1B000, 0x1B122, InflectionUnicodeScripts.Jpan),
        new(0x1B132, 0x1B132, InflectionUnicodeScripts.Jpan),
        new(0x1B150, 0x1B152, InflectionUnicodeScripts.Jpan),
        new(0x1B155, 0x1B155, InflectionUnicodeScripts.Jpan),
        new(0x1B164, 0x1B167, InflectionUnicodeScripts.Jpan),
        new(0x1CF00, 0x1CF2D, InflectionUnicodeScripts.All),
        new(0x1CF30, 0x1CF46, InflectionUnicodeScripts.All),
        new(0x1D167, 0x1D169, InflectionUnicodeScripts.All),
        new(0x1D17B, 0x1D182, InflectionUnicodeScripts.All),
        new(0x1D185, 0x1D18B, InflectionUnicodeScripts.All),
        new(0x1D1AA, 0x1D1AD, InflectionUnicodeScripts.All),
        new(0x1D200, 0x1D245, InflectionUnicodeScripts.Grek),
        new(0x1D360, 0x1D371, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x1DF00, 0x1DF1E, InflectionUnicodeScripts.Latn),
        new(0x1DF25, 0x1DF2A, InflectionUnicodeScripts.Latn),
        new(0x1E030, 0x1E06D, InflectionUnicodeScripts.Cyrl),
        new(0x1E08F, 0x1E08F, InflectionUnicodeScripts.Cyrl),
        new(0x1E7E0, 0x1E7E6, InflectionUnicodeScripts.Ethi),
        new(0x1E7E8, 0x1E7EB, InflectionUnicodeScripts.Ethi),
        new(0x1E7ED, 0x1E7EE, InflectionUnicodeScripts.Ethi),
        new(0x1E7F0, 0x1E7FE, InflectionUnicodeScripts.Ethi),
        new(0x1EE00, 0x1EE03, InflectionUnicodeScripts.Arab),
        new(0x1EE05, 0x1EE1F, InflectionUnicodeScripts.Arab),
        new(0x1EE21, 0x1EE22, InflectionUnicodeScripts.Arab),
        new(0x1EE24, 0x1EE24, InflectionUnicodeScripts.Arab),
        new(0x1EE27, 0x1EE27, InflectionUnicodeScripts.Arab),
        new(0x1EE29, 0x1EE32, InflectionUnicodeScripts.Arab),
        new(0x1EE34, 0x1EE37, InflectionUnicodeScripts.Arab),
        new(0x1EE39, 0x1EE39, InflectionUnicodeScripts.Arab),
        new(0x1EE3B, 0x1EE3B, InflectionUnicodeScripts.Arab),
        new(0x1EE42, 0x1EE42, InflectionUnicodeScripts.Arab),
        new(0x1EE47, 0x1EE47, InflectionUnicodeScripts.Arab),
        new(0x1EE49, 0x1EE49, InflectionUnicodeScripts.Arab),
        new(0x1EE4B, 0x1EE4B, InflectionUnicodeScripts.Arab),
        new(0x1EE4D, 0x1EE4F, InflectionUnicodeScripts.Arab),
        new(0x1EE51, 0x1EE52, InflectionUnicodeScripts.Arab),
        new(0x1EE54, 0x1EE54, InflectionUnicodeScripts.Arab),
        new(0x1EE57, 0x1EE57, InflectionUnicodeScripts.Arab),
        new(0x1EE59, 0x1EE59, InflectionUnicodeScripts.Arab),
        new(0x1EE5B, 0x1EE5B, InflectionUnicodeScripts.Arab),
        new(0x1EE5D, 0x1EE5D, InflectionUnicodeScripts.Arab),
        new(0x1EE5F, 0x1EE5F, InflectionUnicodeScripts.Arab),
        new(0x1EE61, 0x1EE62, InflectionUnicodeScripts.Arab),
        new(0x1EE64, 0x1EE64, InflectionUnicodeScripts.Arab),
        new(0x1EE67, 0x1EE6A, InflectionUnicodeScripts.Arab),
        new(0x1EE6C, 0x1EE72, InflectionUnicodeScripts.Arab),
        new(0x1EE74, 0x1EE77, InflectionUnicodeScripts.Arab),
        new(0x1EE79, 0x1EE7C, InflectionUnicodeScripts.Arab),
        new(0x1EE7E, 0x1EE7E, InflectionUnicodeScripts.Arab),
        new(0x1EE80, 0x1EE89, InflectionUnicodeScripts.Arab),
        new(0x1EE8B, 0x1EE9B, InflectionUnicodeScripts.Arab),
        new(0x1EEA1, 0x1EEA3, InflectionUnicodeScripts.Arab),
        new(0x1EEA5, 0x1EEA9, InflectionUnicodeScripts.Arab),
        new(0x1EEAB, 0x1EEBB, InflectionUnicodeScripts.Arab),
        new(0x1EEF0, 0x1EEF1, InflectionUnicodeScripts.Arab),
        new(0x1F200, 0x1F200, InflectionUnicodeScripts.Jpan),
        new(0x1F250, 0x1F251, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x20000, 0x2A6DF, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x2A700, 0x2B739, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x2B740, 0x2B81D, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x2B820, 0x2CEA1, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x2CEB0, 0x2EBE0, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x2EBF0, 0x2EE5D, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x2F800, 0x2FA1D, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x30000, 0x3134A, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0x31350, 0x323AF, InflectionUnicodeScripts.Hani | InflectionUnicodeScripts.Jpan | InflectionUnicodeScripts.Kore),
        new(0xE0100, 0xE01EF, InflectionUnicodeScripts.All),
    ];
    // </generated-unicode-scripts>

    readonly struct InflectionUnicodeScriptRange(
        int first,
        int last,
        InflectionUnicodeScripts scripts)
    {
        public int First { get; } = first;
        public int Last { get; } = last;
        public InflectionUnicodeScripts Scripts { get; } = scripts;
    }
    internal static bool TryGetPinnedLetterOrMark(
        int scalar,
        out bool isLetter,
        out bool isMark)
    {
        var low = 0;
        var high = UnicodeLetterMarkRangeCount - 1;
        while (low <= high)
        {
            var middle = low + ((high - low) / 2);
            var offset = middle * 3;
            if (scalar < UnicodeLetterMarkRanges[offset])
            {
                high = middle - 1;
            }
            else if (scalar > UnicodeLetterMarkRanges[offset + 1])
            {
                low = middle + 1;
            }
            else
            {
                isMark = UnicodeLetterMarkRanges[offset + 2] != 0;
                isLetter = !isMark;
                return true;
            }
        }

        isLetter = false;
        isMark = false;
        return false;
    }

    static int[] DecodeUnicodeLetterMarkRanges()
    {
        var bytes = Convert.FromBase64String(UnicodeLetterMarkData);
        var ranges = new int[UnicodeLetterMarkRangeCount * 3];
        var byteIndex = 0;
        var previousFirst = 0;
        for (var rangeIndex = 0; rangeIndex < UnicodeLetterMarkRangeCount; rangeIndex++)
        {
            var offset = rangeIndex * 3;
            var first = previousFirst + ReadVarUInt(bytes, ref byteIndex);
            var encodedLengthAndKind = ReadVarUInt(bytes, ref byteIndex);
            ranges[offset] = first;
            ranges[offset + 1] = first + (encodedLengthAndKind / 2);
            ranges[offset + 2] = encodedLengthAndKind & 1;
            previousFirst = first;
        }

        return ranges;
    }

    static int ReadVarUInt(byte[] bytes, ref int index)
    {
        var value = 0;
        var shift = 0;
        byte current;
        do
        {
            current = bytes[index++];
            value |= (current & 0x7F) << shift;
            shift += 7;
        }
        while ((current & 0x80) != 0);

        return value;
    }
}