using System.Collections.Generic;
using System.Globalization;

namespace Humanizer.Localisation.NumberToWords
{
    internal class CroatianNumberToWordsConverter : GenderlessNumberToWordsConverter
    {
        private static readonly string[] UnitsMap =
        {
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
        };

        private static readonly string[] TensMap =
        {
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
        };

        private readonly CultureInfo _culture;

        public CroatianNumberToWordsConverter(CultureInfo culture)
        {
            _culture = culture;
        }

        public override string Convert(long number)
        {
            switch (number)
            {
                case 0: return "nula";
                case < 0:
                    return number != long.MinValue
                        ? string.Format("minus {0}", Convert(-number))
                        : "minus devet trilijuna dvjesto dvadeset tri bilijarde tristo sedamdeset dva bilijuna trideset šest milijardi osamsto pedeset četiri milijuna sedamsto sedamdeset pet tisuća osamsto osam";
            }

            var parts = new List<string>();

            var quintillions = number / 1000000000000000000;
            if (quintillions > 0)
            {
                string part;
                switch (quintillions)
                {
                    case 1:
                        part = "trilijun";
                        break;
                    default:
                        part = string.Format("{0} trilijuna", Convert(quintillions));
                        break;
                }

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
                        part = string.Format("{0} bilijarde", Convert(quadrillions));
                        break;
                    default:
                    {
                        if (quadrillions % 100 > 4 && quadrillions % 100 < 21)
                        {
                            part = string.Format("{0} bilijardi", Convert(quadrillions));
                            break;
                        }

                        switch (quadrillions % 10)
                        {
                            case 1:
                                part = string.Format("{0} jedna bilijarda", Convert(quadrillions - 1));
                                break;
                            case 2:
                                part = string.Format("{0} dvije bilijarde", Convert(quadrillions - 2));
                                break;
                            case 3:
                            case 4:
                                part = string.Format("{0} bilijarde", Convert(quadrillions));
                                break;
                            default:
                                part = string.Format("{0} bilijardi", Convert(quadrillions));
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
                            part = string.Format("{0} bilijuna", Convert(trillions));
                            break;
                        }

                        part = string.Format("{0} bilijun", Convert(trillions));
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
                        part = string.Format("{0} milijarde", Convert(billions));
                        break;
                    default:
                    {
                        if (billions % 100 > 4 && billions % 100 < 21)
                        {
                            part = string.Format("{0} milijardi", Convert(billions));
                            break;
                        }

                        switch (billions % 10)
                        {
                            case 1:
                                part = string.Format("{0} jedna milijarda", Convert(billions - 1));
                                break;
                            case 2:
                                part = string.Format("{0} dvije milijarde", Convert(billions - 2));
                                break;
                            case 3:
                            case 4:
                                part = string.Format("{0} milijarde", Convert(billions));
                                break;
                            default:
                                part = string.Format("{0} milijardi", Convert(billions));
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
                            part = string.Format("{0} milijuna", Convert(millions));
                            break;
                        }

                        part = string.Format("{0} milijun", Convert(millions));
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
                        part = string.Format("{0} tisuće", Convert(thousands));
                        break;
                    default:
                    {
                        if (thousands % 100 > 4 && thousands % 100 < 21)
                        {
                            part = string.Format("{0} tisuća", Convert(thousands));
                            break;
                        }

                        switch (thousands % 10)
                        {
                            case 1:
                                part = string.Format("{0} jedna tisuća", Convert(thousands - 1));
                                break;
                            case 2:
                                part = string.Format("{0} dvije tisuće", Convert(thousands - 2));
                                break;
                            case 3:
                            case 4:
                                part = string.Format("{0} tisuće", Convert(thousands));
                                break;
                            default:
                                part = string.Format("{0} tisuća", Convert(thousands));
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
                string part;
                switch (hundreds)
                {
                    case 1:
                        part = "sto";
                        break;
                    case 2:
                        part = "dvjesto";
                        break;
                    default:
                        part = string.Format("{0}sto", Convert(hundreds));
                        break;
                }

                parts.Add(part);
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
                        parts.Add(string.Format(" {0}", UnitsMap[units]));
                    }
                }
            }

            return string.Join("", parts);
        }

        public override string ConvertToOrdinal(int number)
        {
            //TODO: In progress
            return number.ToString(_culture);
        }
    }
}
