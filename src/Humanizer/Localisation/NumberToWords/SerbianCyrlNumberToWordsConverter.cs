namespace Humanizer;

class SerbianCyrlNumberToWordsConverter(CultureInfo culture) :
    GenderlessNumberToWordsConverter
{
    static readonly string[] UnitsMap = ["нула", "један", "два", "три", "четири", "пет", "шест", "седам", "осам", "девет", "десет", "једанест", "дванаест", "тринаест", "четрнаест", "петнаест", "шеснаест", "седамнаест", "осамнаест", "деветнаест"];
    static readonly string[] TensMap = ["нула", "десет", "двадесет", "тридесет", "четрдесет", "петдесет", "шестдесет", "седамдесет", "осамдесет", "деветдесет"];

    public override string Convert(long input)
    {
        if (input is > int.MaxValue or < int.MinValue)
        {
            throw new NotImplementedException();
        }

        var number = (int)input;

        if (number == 0)
        {
            return "нула";
        }

        if (number < 0)
        {
            return $"- {Convert(-number)}";
        }

        var parts = new List<string>();
        var billions = number / 1000000000;

        if (billions > 0)
        {
            parts.Add(Part("милијарда", "две милијарде", "{0} милијарде", "{0} милијарда", billions));
            number %= 1000000000;

            if (number > 0)
            {
                parts.Add(" ");
            }
        }

        var millions = number / 1000000;

        if (millions > 0)
        {
            parts.Add(Part("милион", "два милиона", "{0} милиона", "{0} милиона", millions));
            number %= 1000000;

            if (number > 0)
            {
                parts.Add(" ");
            }
        }

        var thousands = number / 1000;

        if (thousands > 0)
        {
            parts.Add(Part("хиљаду", "две хиљаде", "{0} хиљаде", "{0} хиљада", thousands));
            number %= 1000;

            if (number > 0)
            {
                parts.Add(" ");
            }
        }

        var hundreds = number / 100;

        if (hundreds > 0)
        {
            parts.Add(Part("сто", "двесто", "{0}сто", "{0}сто", hundreds));
            number %= 100;

            if (number > 0)
            {
                parts.Add(" ");
            }
        }

        if (number > 0)
        {
            if (number < 20)
            {
                parts.Add(UnitsMap[number]);
            }
            else
            {
                parts.Add(TensMap[number / 10]);

                var units = number % 10;

                if (units > 0)
                {
                    parts.Add($" {UnitsMap[units]}");
                }
            }
        }

        return string.Concat(parts);
    }

    public override string ConvertToOrdinal(int number) =>
        //TODO: In progress
        number.ToString(culture);

    string Part(string singular, string dual, string trialQuadral, string plural, int number)
    {
        return number switch
        {
            1 => singular,
            2 => dual,
            3 or 4 => string.Format(trialQuadral, Convert(number)),
            _ => string.Format(plural, Convert(number)),
        };
    }
}