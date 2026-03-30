namespace Humanizer;

class UzbekFamilyNumberToWordsConverter(UzbekNumberToWordsProfile profile) : GenderlessNumberToWordsConverter
{
    readonly UzbekNumberToWordsProfile profile = profile;

    public override string Convert(long input)
    {
        if (input is > int.MaxValue or < int.MinValue)
        {
            throw new NotImplementedException();
        }

        var number = (int)input;
        if (number < 0)
        {
            return $"{profile.MinusWord} {Convert(-number, true)}";
        }

        return Convert(number, true);
    }

    string Convert(int number, bool checkForHundredRule)
    {
        if (number == 0)
        {
            return profile.UnitsMap[0];
        }

        if (checkForHundredRule && number == 100)
        {
            return profile.HundredWord;
        }

        var builder = new StringBuilder();

        AppendScale(builder, ref number, 1_000_000_000, profile.BillionWord, false);
        AppendScale(builder, ref number, 1_000_000, profile.MillionWord, true);
        AppendScale(builder, ref number, 1_000, profile.ThousandWord, true);
        AppendScale(builder, ref number, 100, profile.HundredWord, false);

        if (number / 10 > 0)
        {
            builder.Append(profile.TensMap[number / 10]);
            builder.Append(' ');
            number %= 10;
        }

        if (number > 0)
        {
            builder.Append(profile.UnitsMap[number]);
            builder.Append(' ');
        }

        return builder
            .ToString()
            .Trim();
    }

    void AppendScale(StringBuilder builder, ref int number, int divisor, string scaleWord, bool checkForHundredRule)
    {
        if (number / divisor <= 0)
        {
            return;
        }

        builder.AppendFormat("{0} {1} ", Convert(number / divisor, checkForHundredRule), scaleWord);
        number %= divisor;
    }

    public override string ConvertToOrdinal(int number)
    {
        var word = Convert(number);
        if (string.IsNullOrEmpty(word))
        {
            return string.Empty;
        }

        var suffixIndex = profile.SecondOrdinalSuffixCharacters.Contains(word[^1]) ? 1 : 0;
        return word + profile.OrdinalSuffixes[suffixIndex];
    }
}

sealed class UzbekNumberToWordsProfile(
    string minusWord,
    string hundredWord,
    string thousandWord,
    string millionWord,
    string billionWord,
    string secondOrdinalSuffixCharacters,
    string[] unitsMap,
    string[] tensMap,
    string[] ordinalSuffixes)
{
    public string MinusWord { get; } = minusWord;
    public string HundredWord { get; } = hundredWord;
    public string ThousandWord { get; } = thousandWord;
    public string MillionWord { get; } = millionWord;
    public string BillionWord { get; } = billionWord;
    public string SecondOrdinalSuffixCharacters { get; } = secondOrdinalSuffixCharacters;
    public string[] UnitsMap { get; } = unitsMap;
    public string[] TensMap { get; } = tensMap;
    public string[] OrdinalSuffixes { get; } = ordinalSuffixes;
}
