namespace Humanizer;

class VietnameseNumberToWordsConverter : GenderlessNumberToWordsConverter
{
    const int OneBillion = 1000000000;
    const int OneMillion = 1000000;

    static readonly string[] NumberVerbalPairs =
    [
        "", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín"
    ];

    public override string Convert(long number) =>
        number == 0
            ? "không"
            : ConvertImpl(number);

    public override string ConvertToOrdinal(int number) =>
        $"thứ {ConvertToOrdinalImpl(number)}";

    string ConvertToOrdinalImpl(int number) =>
        number switch
        {
            1 => "nhất",
            2 => "nhì",
            4 => "tư",
            _ => Convert(number)
        };

    static string ConvertImpl(long number, bool hasTens = false, bool isGreaterThanOneHundred = false)
    {
        if (number < 0)
        {
            return $"trừ {ConvertImpl(-number, hasTens, isGreaterThanOneHundred)}";
        }

        if (number >= OneBillion)
        {
            return string.Format(
                    "{0} tỉ {1}",
                    ConvertImpl(number / OneBillion),
                    ConvertImpl(number % OneBillion, isGreaterThanOneHundred: true)
                )
                .TrimEnd();
        }

        if (number >= OneMillion)
        {
            return string.Format(
                    "{0} triệu {1}",
                    ConvertImpl(number / OneMillion),
                    ConvertImpl(number % OneMillion, isGreaterThanOneHundred: true)
                )
                .TrimEnd();
        }

        if (number >= 1000)
        {
            return string.Format(
                    "{0} nghìn {1}",
                    ConvertImpl(number / 1000),
                    ConvertImpl(number % 1000, isGreaterThanOneHundred: true)
                )
                .TrimEnd();
        }

        if (number >= 100)
        {
            return string.Format(
                    "{0} trăm {1}",
                    NumberVerbalPairs[number / 100],
                    ConvertImpl(number % 100, isGreaterThanOneHundred: true)
                )
                .TrimEnd();
        }

        if (number >= 20)
        {
            return string.Format(
                    "{0} mươi {1}",
                    NumberVerbalPairs[number / 10],
                    ConvertImpl(number % 10, hasTens: true)
                )
                .TrimEnd();
        }

        if (number == 14)
        {
            return "mười bốn";
        }

        if (number == 11)
        {
            return "mười một";
        }

        if (number >= 10)
        {
            return $"mười {ConvertImpl(number % 10, hasTens: true)}".TrimEnd();
        }

        if (number == 5 && hasTens)
        {
            return "lăm";
        }

        if (number == 4 && hasTens)
        {
            return "tư";
        }

        if (number == 1 && hasTens)
        {
            return "mốt";
        }

        if (number > 0 && isGreaterThanOneHundred && !hasTens)
        {
            return $"linh {NumberVerbalPairs[number]}";
        }

        return NumberVerbalPairs[number];
    }
}