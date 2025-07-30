﻿namespace Humanizer;

class CroatianNumberToWordsConverter(CultureInfo culture)
    : GenderlessNumberToWordsConverter
{
    static readonly string[] UnitsMap =
    [
        "nula",
        "jedan",
        "dva",
        "tri",
        "četiri",
        "pet",
        "šest",
        "sedam",
        "osam",
        "devet",
        "deset",
        "jedanaest",
        "dvanaest",
        "trinaest",
        "četrnaest",
        "petnaest",
        "šesnaest",
        "sedamnaest",
        "osamnaest",
        "devetnaest"
    ];

    static readonly string[] TensMap =
    [
        "nula",
        "deset",
        "dvadeset",
        "trideset",
        "četrdeset",
        "pedeset",
        "šezdeset",
        "sedamdeset",
        "osamdeset",
        "devedeset"
    ];

    public override string Convert(long number)
    {
        switch (number)
        {
            case 0: return "nula";
            case < 0:
                return number != long.MinValue
                    ? $"minus {Convert(-number)}"
                    : "minus devet trilijuna dvjesto dvadeset tri bilijarde tristo sedamdeset dva bilijuna trideset šest milijardi osamsto pedeset četiri milijuna sedamsto sedamdeset pet tisuća osamsto osam";
        }

        var parts = new List<string>();

        var quintillions = number / 1000000000000000000;
        if (quintillions > 0)
        {
            var part = quintillions switch
            {
                1 => "trilijun",
                _ => $"{Convert(quintillions)} trilijuna"
            };

            parts.Add(part);
            number %= 1000000000000000000;

            if (number > 0)
            {
                parts.Add(" ");
            }
        }

        var quadrillions = number / 1000000000000000;
        if (quadrillions > 0)
        {
            string part;
            switch (quadrillions)
            {
                case 1:
                    part = "bilijarda";
                    break;
                case 2:
                    part = "dvije bilijarde";
                    break;
                case 3:
                case 4:
                    part = $"{Convert(quadrillions)} bilijarde";
                    break;
                default:
                {
                    if (quadrillions % 100 > 4 && quadrillions % 100 < 21)
                    {
                        part = $"{Convert(quadrillions)} bilijardi";
                        break;
                    }

                    switch (quadrillions % 10)
                    {
                        case 1:
                            part = $"{Convert(quadrillions - 1)} jedna bilijarda";
                            break;
                        case 2:
                            part = $"{Convert(quadrillions - 2)} dvije bilijarde";
                            break;
                        case 3:
                        case 4:
                            part = $"{Convert(quadrillions)} bilijarde";
                            break;
                        default:
                            part = $"{Convert(quadrillions)} bilijardi";
                            break;
                    }

                    break;
                }
            }

            parts.Add(part);
            number %= 1000000000000000;

            if (number > 0)
            {
                parts.Add(" ");
            }
        }

        var trillions = number / 1000000000000;
        if (trillions > 0)
        {
            string part;
            switch (trillions)
            {
                case 1:
                    part = "bilijun";
                    break;
                default:
                {
                    if (trillions % 100 == 11 || trillions % 10 != 1)
                    {
                        part = $"{Convert(trillions)} bilijuna";
                        break;
                    }

                    part = $"{Convert(trillions)} bilijun";
                    break;
                }
            }

            parts.Add(part);
            number %= 1000000000000;

            if (number > 0)
            {
                parts.Add(" ");
            }
        }

        var billions = number / 1000000000;
        if (billions > 0)
        {
            string part;
            switch (billions)
            {
                case 1:
                    part = "milijarda";
                    break;
                case 2:
                    part = "dvije milijarde";
                    break;
                case 3:
                case 4:
                    part = $"{Convert(billions)} milijarde";
                    break;
                default:
                {
                    if (billions % 100 > 4 && billions % 100 < 21)
                    {
                        part = $"{Convert(billions)} milijardi";
                        break;
                    }

                    switch (billions % 10)
                    {
                        case 1:
                            part = $"{Convert(billions - 1)} jedna milijarda";
                            break;
                        case 2:
                            part = $"{Convert(billions - 2)} dvije milijarde";
                            break;
                        case 3:
                        case 4:
                            part = $"{Convert(billions)} milijarde";
                            break;
                        default:
                            part = $"{Convert(billions)} milijardi";
                            break;
                    }

                    break;
                }
            }

            parts.Add(part);
            number %= 1000000000;

            if (number > 0)
            {
                parts.Add(" ");
            }
        }

        var millions = number / 1000000;
        if (millions > 0)
        {
            string part;
            switch (millions)
            {
                case 1:
                    part = "milijun";
                    break;
                default:
                {
                    if (millions % 100 == 11 || millions % 10 != 1)
                    {
                        part = $"{Convert(millions)} milijuna";
                        break;
                    }

                    part = $"{Convert(millions)} milijun";
                    break;
                }
            }

            parts.Add(part);
            number %= 1000000;

            if (number > 0)
            {
                parts.Add(" ");
            }
        }

        var thousands = number / 1000;
        if (thousands > 0)
        {
            string part;
            switch (thousands)
            {
                case 1:
                    part = "tisuću";
                    break;
                case 2:
                    part = "dvije tisuće";
                    break;
                case 3:
                case 4:
                    part = $"{Convert(thousands)} tisuće";
                    break;
                default:
                {
                    if (thousands % 100 > 4 && thousands % 100 < 21)
                    {
                        part = $"{Convert(thousands)} tisuća";
                        break;
                    }

                    switch (thousands % 10)
                    {
                        case 1:
                            part = $"{Convert(thousands - 1)} jedna tisuća";
                            break;
                        case 2:
                            part = $"{Convert(thousands - 2)} dvije tisuće";
                            break;
                        case 3:
                        case 4:
                            part = $"{Convert(thousands)} tisuće";
                            break;
                        default:
                            part = $"{Convert(thousands)} tisuća";
                            break;
                    }

                    break;
                }
            }

            parts.Add(part);
            number %= 1000;

            if (number > 0)
            {
                parts.Add(" ");
            }
        }

        var hundreds = number / 100;
        if (hundreds > 0)
        {
            var part = hundreds switch
            {
                1 => "sto",
                2 => "dvjesto",
                _ => $"{Convert(hundreds)}sto"
            };

            parts.Add(part);
            number %= 100;

            if (number > 0)
            {
                parts.Add(" ");
            }
        }

        switch (number)
        {
            case <= 0:
                return string.Concat(parts);
            case < 20:
                parts.Add(UnitsMap[number]);
                break;
            default:
            {
                parts.Add(TensMap[number / 10]);
                var units = number % 10;

                if (units > 0)
                {
                    parts.Add($" {UnitsMap[units]}");
                }

                break;
            }
        }

        return string.Concat(parts);
    }

    public override string ConvertToOrdinal(int number) =>
        //TODO: In progress
        number.ToString(culture);
}