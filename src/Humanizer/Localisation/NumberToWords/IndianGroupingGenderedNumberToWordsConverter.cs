namespace Humanizer;

class IndianGroupingGenderedNumberToWordsConverter(IndianGroupingGenderedNumberToWordsConverter.Profile profile) : GenderedNumberToWordsConverter
{
    public sealed record GenderOrdinalBlock(
        string DefaultSuffix,
        FrozenDictionary<int, string> ExactReplacements);

    public sealed record OrdinalProfile(
        GenderOrdinalBlock Masculine,
        GenderOrdinalBlock Feminine,
        string NeuterFallback);

    public sealed record Profile(
        string ZeroWord,
        string NegativeWord,
        string HundredWord,
        string ThousandWord,
        string LakhWord,
        string SingleLakhWord,
        string CroreWord,
        string ArabWord,
        string KharabWord,
        string[] DenseUnitsMap,
        OrdinalProfile Ordinal);

    readonly Profile profile = profile;

    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true) =>
        ConvertCardinal(number);

    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        var effectiveGender = gender == GrammaticalGender.Neuter
            ? GrammaticalGender.Masculine
            : gender;

        var genderBlock = effectiveGender == GrammaticalGender.Feminine
            ? profile.Ordinal.Feminine
            : profile.Ordinal.Masculine;

        if (genderBlock.ExactReplacements.TryGetValue(number, out var exact))
        {
            return exact;
        }

        return ConvertCardinal(number) + genderBlock.DefaultSuffix;
    }

    string ConvertCardinal(long number)
    {
        if (number == 0)
        {
            return profile.ZeroWord;
        }

        if (number < 0)
        {
            return profile.NegativeWord + " " + ConvertCardinal(-number);
        }

        var parts = new List<string>();

        AppendScale(parts, ref number, 100_000_000_000, profile.KharabWord);
        AppendScale(parts, ref number, 1_000_000_000, profile.ArabWord);
        AppendScale(parts, ref number, 10_000_000, profile.CroreWord);
        AppendScale(parts, ref number, 100_000, profile.LakhWord);

        if (number >= 1000)
        {
            var count = number / 1000;
            parts.Add(count == 1
                ? profile.SingleLakhWord + " " + profile.ThousandWord
                : ConvertCardinal(count) + " " + profile.ThousandWord);
            number %= 1000;
        }

        if (number >= 100)
        {
            parts.Add(profile.DenseUnitsMap[number / 100] + " " + profile.HundredWord);
            number %= 100;
        }

        if (number > 0)
        {
            parts.Add(profile.DenseUnitsMap[number]);
        }

        return string.Join(" ", parts);
    }

    void AppendScale(List<string> parts, ref long number, long scaleValue, string scaleWord)
    {
        if (number < scaleValue)
        {
            return;
        }

        var count = number / scaleValue;
        parts.Add(count == 1
            ? profile.SingleLakhWord + " " + scaleWord
            : ConvertCardinal(count) + " " + scaleWord);
        number %= scaleValue;
    }
}
