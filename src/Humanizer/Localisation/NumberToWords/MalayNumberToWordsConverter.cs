namespace Humanizer;

class MalayNumberToWordsConverter : GenderlessNumberToWordsConverter
{
    static readonly string[] UnitsMap = ["kosong", "satu", "dua", "tiga", "empat", "lima", "enam", "tujuh", "lapan", "sembilan"];

    public override string Convert(long number)
    {
        if (number < 0)
        {
            return $"minus {Convert(-number)}";
        }

        if (number < 10)
        {
            return UnitsMap[number];
        }

        if (number == 10)
        {
            return "sepuluh";
        }

        if (number == 11)
        {
            return "sebelas";
        }

        if (number < 20)
        {
            return $"{Convert(number - 10)} belas";
        }

        if (number < 100)
        {
            var remainder = number % 10;
            var words = $"{Convert(number / 10)} puluh";
            return remainder == 0 ? words : $"{words} {Convert(remainder)}";
        }

        if (number < 200)
        {
            var remainder = number - 100;
            return remainder == 0 ? "seratus" : $"seratus {Convert(remainder)}";
        }

        if (number < 1000)
        {
            var remainder = number % 100;
            var words = $"{Convert(number / 100)} ratus";
            return remainder == 0 ? words : $"{words} {Convert(remainder)}";
        }

        if (number < 2000)
        {
            var remainder = number - 1000;
            return remainder == 0 ? "seribu" : $"seribu {Convert(remainder)}";
        }

        if (number < 1_000_000)
        {
            var remainder = number % 1000;
            var words = $"{Convert(number / 1000)} ribu";
            return remainder == 0 ? words : $"{words} {Convert(remainder)}";
        }

        if (number < 1_000_000_000)
        {
            var remainder = number % 1_000_000;
            var words = $"{Convert(number / 1_000_000)} juta";
            return remainder == 0 ? words : $"{words} {Convert(remainder)}";
        }

        if (number < 1_000_000_000_000)
        {
            var remainder = number % 1_000_000_000;
            var words = $"{Convert(number / 1_000_000_000)} bilion";
            return remainder == 0 ? words : $"{words} {Convert(remainder)}";
        }

        var trillionRemainder = number % 1_000_000_000_000;
        var trillionWords = $"{Convert(number / 1_000_000_000_000)} trilion";
        return trillionRemainder == 0 ? trillionWords : $"{trillionWords} {Convert(trillionRemainder)}";
    }

    public override string ConvertToOrdinal(int number)
    {
        if (number < 0)
        {
            return $"minus {ConvertToOrdinal(-number)}";
        }

        return number == 1 ? "pertama" : $"ke{Convert(number)}";
    }
}
