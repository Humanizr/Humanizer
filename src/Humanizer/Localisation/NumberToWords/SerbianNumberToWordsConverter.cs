namespace Humanizer;

class SerbianNumberToWordsConverter(CultureInfo culture) :
    GenderlessNumberToWordsConverter
{
    static readonly string[] UnitsMap = ["nula", "jedan", "dva", "tri", "četiri", "pet", "šest", "sedam", "osam", "devet", "deset", "jedanaest", "dvanaest", "trinaest", "četrnaest", "petnaest", "šestnaest", "sedemnaest", "osemnaest", "devetnaest"];
    static readonly string[] TensMap = ["nula", "deset", "dvadeset", "trideset", "četrdeset", "petdeset", "šestdeset", "sedamdeset", "osamdeset", "devetdeset"];

    public override string Convert(long input)
    {
        if (input is > int.MaxValue or < int.MinValue)
        {
            throw new NotImplementedException();
        }

        var number = (int)input;
        if (number == 0)
        {
            return "nula";
        }

        if (number < 0)
        {
            return $"- {Convert(-number)}";
        }

        var parts = new List<string>();
        var billions = number / 1000000000;

        if (billions > 0)
        {
            parts.Add(Part("milijarda", "dve milijarde", "{0} milijarde", "{0} milijarda", billions));
            number %= 1000000000;

            if (number > 0)
            {
                parts.Add(" ");
            }
        }

        var millions = number / 1000000;

        if (millions > 0)
        {
            parts.Add(Part("milion", "dva miliona", "{0} miliona", "{0} miliona", millions));
            number %= 1000000;

            if (number > 0)
            {
                parts.Add(" ");
            }
        }

        var thousands = number / 1000;

        if (thousands > 0)
        {
            parts.Add(Part("hiljadu", "dve hiljade", "{0} hiljade", "{0} hiljada", thousands));
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