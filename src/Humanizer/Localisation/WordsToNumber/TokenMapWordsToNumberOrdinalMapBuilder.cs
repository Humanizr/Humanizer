using System.Collections.Frozen;
using System.Collections.Generic;
using System.Globalization;

namespace Humanizer;

enum TokenMapOrdinalGenderVariant
{
    None,
    MasculineAndFeminine,
    All
}

static class TokenMapWordsToNumberOrdinalMapBuilder
{
    public static FrozenDictionary<string, int> Build(
        string numberToWordsKind,
        TokenMapNormalizationProfile normalizationProfile,
        TokenMapOrdinalGenderVariant genderVariant) =>
        Build(
            NumberToWordsProfileCatalog.Resolve(numberToWordsKind, CultureInfo.InvariantCulture),
            normalizationProfile,
            genderVariant);

    public static FrozenDictionary<string, int> Build(
        INumberToWordsConverter converter,
        TokenMapNormalizationProfile normalizationProfile,
        TokenMapOrdinalGenderVariant genderVariant)
    {
        var ordinals = new Dictionary<string, int>(StringComparer.Ordinal);

        for (var number = 1; number <= 200; number++)
        {
            switch (genderVariant)
            {
                case TokenMapOrdinalGenderVariant.None:
                    ordinals[Normalize(converter.ConvertToOrdinal(number), normalizationProfile)] = number;
                    break;
                case TokenMapOrdinalGenderVariant.MasculineAndFeminine:
                    ordinals[Normalize(converter.ConvertToOrdinal(number, GrammaticalGender.Masculine), normalizationProfile)] = number;
                    ordinals[Normalize(converter.ConvertToOrdinal(number, GrammaticalGender.Feminine), normalizationProfile)] = number;
                    break;
                case TokenMapOrdinalGenderVariant.All:
                    ordinals[Normalize(converter.ConvertToOrdinal(number), normalizationProfile)] = number;
                    ordinals[Normalize(converter.ConvertToOrdinal(number, GrammaticalGender.Feminine), normalizationProfile)] = number;
                    ordinals[Normalize(converter.ConvertToOrdinal(number, GrammaticalGender.Neuter), normalizationProfile)] = number;
                    break;
            }
        }

        return ordinals.ToFrozenDictionary(StringComparer.Ordinal);
    }

    static string Normalize(string words, TokenMapNormalizationProfile normalizationProfile) =>
        TokenMapWordsToNumberNormalizer.Normalize(words, normalizationProfile);
}
