namespace Humanizer;

class SlovenianNumberToWordsConverter(CultureInfo culture) :
    GenderlessNumberToWordsConverter
{
    static readonly string[] UnitsMap = ["nič", "ena", "dva", "tri", "štiri", "pet", "šest", "sedem", "osem", "devet", "deset", "enajst", "dvanajst", "trinajst", "štirinajst", "petnajst", "šestnajst", "sedemnajst", "osemnajst", "devetnajst"];
    static readonly string[] TensMap = ["nič", "deset", "dvajset", "trideset", "štirideset", "petdeset", "šestdeset", "sedemdeset", "osemdeset", "devetdeset"];

    public override string Convert(long input)
    {
        if (input is > int.MaxValue or < int.MinValue)
        {
            throw new NotImplementedException();
        }

        var number = (int)input;
        if (number == 0)
        {
            return "nič";
        }

        if (number < 0)
        {
            return $"minus {Convert(-number)}";
        }

        var parts = new List<string>();

        var billions = number / 1000000000;
        if (billions > 0)
        {
            parts.Add(Part("milijarda", "dve milijardi", "{0} milijarde", "{0} milijard", billions));
            number %= 1000000000;
            if (number > 0)
            {
                parts.Add(" ");
            }
        }

        var millions = number / 1000000;
        if (millions > 0)
        {
            parts.Add(Part("milijon", "dva milijona", "{0} milijone", "{0} milijonov", millions));
            number %= 1000000;
            if (number > 0)
            {
                parts.Add(" ");
            }
        }

        var thousands = number / 1000;
        if (thousands > 0)
        {
            parts.Add(Part("tisoč", "dva tisoč", "{0} tisoč", "{0} tisoč", thousands));
            number %= 1000;
            if (number > 0)
            {
                parts.Add(" ");
            }
        }

        var hundreds = number / 100;
        if (hundreds > 0)
        {
            parts.Add(Part("sto", "dvesto", "{0}sto", "{0}sto", hundreds));
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
                if (number > 1)
                {
                    parts.Add(UnitsMap[number]);
                }
                else
                {
                    parts.Add("ena");
                }
            }
            else
            {
                var units = number % 10;
                if (units > 0)
                {
                    parts.Add($"{UnitsMap[units]}in");
                }

                parts.Add(TensMap[number / 10]);
            }
        }

        return string.Concat(parts);
    }

    public override string ConvertToOrdinal(int number) =>
        number.ToString(culture);

    string Part(string singular, string dual, string trialQuadral, string plural, int number)
    {
        if (number == 1)
        {
            return singular;
        }

        if (number == 2)
        {
            return dual;
        }

        if (number is 3 or 4)
        {
            return string.Format(trialQuadral, Convert(number));
        }

        return string.Format(plural, Convert(number));
    }
}