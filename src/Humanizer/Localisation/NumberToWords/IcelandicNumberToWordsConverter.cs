using System;
using System.Linq;
using System.Collections.Generic;

namespace Humanizer.Localisation.NumberToWords
{
    internal class IcelandicNumberToWordsConverter : GenderedNumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "núll", string.Empty, string.Empty, string.Empty, string.Empty, "fimm", "sex", "sjö", "átta", "níu", "tíu", "ellefu", "tólf", "þrettán", "fjórtán", "fimmtán", "sextán", "sautján", "átján", "nítján" };
        private static readonly string[] FeminineUnitsMap = { string.Empty, "ein", "tvær", "þrjár", "fjórar" };
        private static readonly string[] MasculineUnitsMap = { string.Empty, "einn", "tveir", "þrír", "fjórir" };
        private static readonly string[] NeuterUnitsMap = { string.Empty, "eitt", "tvö", "þrjú", "fjögur" };
        private static readonly string[] TensMap = { string.Empty, "tíu", "tuttugu", "þrjátíu", "fjörutíu", "fimmtíu", "sextíu", "sjötíu", "áttatíu", "níutíu" };
        private static readonly string[] UnitsOrdinalPrefixes = { "núllt", "fyrst", string.Empty, "þriðj", "fjórð", "fimmt", "sjött", "sjöund", "áttund", "níund", "tíund", "elleft", "tólft", "þrettánd", "fjórtánd", "fimmtánd", "sextánd", "sautjánd", "átjánd", "nítjánd" };
        private static readonly string[] TensOrdinalPrefixes = { string.Empty, "tíund", "tuttugast", "þrítugast", "fertugast", "fimmtugast", "sextugast", "sjötugast", "áttugast", "nítugast" };
        private const string AndSplit = "og";
        private class Fact
        {
            public long Power { get; set; }
            public GrammaticalGender Gender { get; set; }
            public string Plural { get; set; }
            public string Single { get; set; }
            public string OrdinalPrefix { get; set; }
        }
        private static readonly Dictionary<int, Fact> PowerOfTenMap = new()
        {
            {0,     new Fact{Power = 0                      , Single = string.Empty,        Plural = string.Empty,  OrdinalPrefix = string.Empty,   Gender = GrammaticalGender.Neuter    }},
            {2,     new Fact{Power = 2                      , Single = "hundrað",           Plural = "hundruð",     OrdinalPrefix = "hundruðast",   Gender = GrammaticalGender.Neuter    }},
            {3,     new Fact{Power = 1000                   , Single = "eitt þúsund",       Plural = "þúsund",      OrdinalPrefix = "þúsundast",    Gender = GrammaticalGender.Neuter    }},
            {6,     new Fact{Power = 1000000                , Single = "ein milljón",       Plural = "milljónir",   OrdinalPrefix = "milljónast",   Gender = GrammaticalGender.Feminine  }},
            {9,     new Fact{Power = 1000000000             , Single = "einn milljarður",   Plural = "milljarðar",  OrdinalPrefix = "milljarðast",  Gender = GrammaticalGender.Masculine }},
            {12,    new Fact{Power = 1000000000000          , Single = "ein billjón",       Plural = "billjónir",   OrdinalPrefix = "billjónast",   Gender = GrammaticalGender.Feminine  }},
            {15,    new Fact{Power = 1000000000000000       , Single = "einn billjarður",   Plural = "billjarðar",  OrdinalPrefix = "billjarðast",  Gender = GrammaticalGender.Masculine }},
            {18,    new Fact{Power = 1000000000000000000    , Single = "ein trilljón",      Plural = "trilljónir",  OrdinalPrefix = "trilljónast",  Gender = GrammaticalGender.Feminine  }}
        };
        private static bool IsAndSplitNeeded(int number) =>
            (((number <= 20) || (number % 10 == 0) && (number < 100)) || (number % 100 == 0));
        private static string GetOrdinalEnding(GrammaticalGender gender) =>
            gender == GrammaticalGender.Masculine ? "i" : "a";
        private static void GetUnits(ICollection<string> builder, long number, GrammaticalGender gender)
        {
            if (number is > 0 and < 5)
            {
                var genderedForm = gender switch
                {
                    GrammaticalGender.Masculine => MasculineUnitsMap[number],
                    GrammaticalGender.Neuter => NeuterUnitsMap[number],
                    GrammaticalGender.Feminine => FeminineUnitsMap[number],
                    _ => throw new ArgumentOutOfRangeException(nameof(gender))
                };
                builder.Add(genderedForm);
            }
            else
            {
                builder.Add(UnitsMap[number]);
            }
        }
        private static void CollectOrdinalParts(ICollection<string> builder, int threeDigitPart, Fact conversionRule, GrammaticalGender partGender, GrammaticalGender ordinalGender)
        {
            var hundreds = threeDigitPart / 100;
            var hundredRemainder = threeDigitPart % 100;
            var units = hundredRemainder % 10;
            var decade = hundredRemainder / 10 * 10;
            var hasThousand = conversionRule.Power > 100;

            if (hundreds != 0)
            {
                GetUnits(builder, hundreds, GrammaticalGender.Neuter);
                var hundredPrefix = hundreds == 1 ? PowerOfTenMap[2].Single : PowerOfTenMap[2].Plural;
                if (hundredRemainder < 20 && false == hasThousand)
                {
                    var genderedFormWithPostfix = partGender switch
                    {
                        GrammaticalGender.Masculine => hundredPrefix + "asti",
                        GrammaticalGender.Neuter => hundredPrefix + "asta",
                        GrammaticalGender.Feminine => hundredPrefix + "asta",
                        _ => throw new ArgumentOutOfRangeException(nameof(partGender))
                    };
                    builder.Add(genderedFormWithPostfix);
                }
                else
                {
                    builder.Add(hundredPrefix);
                }
            }
            if (decade >= 20)
            {
                if (units != 0)
                {
                    builder.Add(CollectOrdinalPartsUnderAHundred(decade, partGender));
                    builder.Add(AndSplit);
                    builder.Add(CollectOrdinalPartsUnderAHundred(units, partGender));
                }
                else
                {
                    if (hundreds != 0)
                    {
                        builder.Add(AndSplit);
                    }
                    builder.Add(CollectOrdinalPartsUnderAHundred(decade, partGender));
                }
            }
            else if (hundredRemainder != 0)
            {
                if (hundreds != 0)
                {
                    builder.Add(AndSplit);
                }
                if (hasThousand)
                {
                    GetUnits(builder, hundredRemainder, conversionRule.Gender);
                }
                else
                {
                    builder.Add(CollectOrdinalPartsUnderAHundred(hundredRemainder, partGender));
                }
            }
            if (hasThousand)
            {
                builder.Add(conversionRule.OrdinalPrefix + GetOrdinalEnding(ordinalGender));
            }
        }
        private static string CollectOrdinalPartsUnderAHundred(int number, GrammaticalGender gender)
        {
            string returnValue = null;
            if (number is >= 0 and < 20)
            {
                if (number == 2)
                {
                    returnValue = gender switch
                    {
                        GrammaticalGender.Masculine => "annar",
                        GrammaticalGender.Feminine => "önnur",
                        GrammaticalGender.Neuter => "annað",
                        _ => throw new ArgumentOutOfRangeException(nameof(gender))
                    };
                }
                else
                {
                    returnValue = UnitsOrdinalPrefixes[number] + GetOrdinalEnding(gender);
                }
            }
            else if (number < 100 && number % 10 == 0)
            {
                returnValue = TensOrdinalPrefixes[number / 10] + GetOrdinalEnding(gender);
            }
            return returnValue;
        }
        private static void CollectParts(IList<string> parts, ref long number, ref bool needsAnd, Fact rule)
        {
            var remainder = number / rule.Power;
            if (remainder > 0)
            {
                number %= rule.Power;
                var prevLen = parts.Count;
                CollectPart(parts, remainder, rule);
                if (number == 0 && needsAnd && false == parts.Skip(prevLen).Contains(AndSplit))
                {
                    parts.Insert(prevLen, AndSplit);
                }
                needsAnd = true;
            }
        }
        private static void CollectPart(ICollection<string> parts, long number, Fact rule)
        {
            if (number == 1)
            {
                parts.Add(rule.Single);
            }
            else
            {
                CollectPartUnderOneThousand(parts, number, rule.Gender);
                parts.Add(rule.Plural);
            }
        }
        private static void CollectPartUnderOneThousand(ICollection<string> builder, long number, GrammaticalGender gender)
        {
            var hundreds = number / 100;
            var hundredRemainder = number % 100;
            var units = hundredRemainder % 10;
            var tens = hundredRemainder / 10;

            if (hundreds != 0)
            {
                GetUnits(builder, hundreds, GrammaticalGender.Neuter);
                builder.Add(hundreds == 1 ? PowerOfTenMap[2].Single : PowerOfTenMap[2].Plural);
            }

            if (tens >= 2)
            {
                if (units != 0)
                {
                    builder.Add(TensMap[tens]);
                    builder.Add(AndSplit);
                    GetUnits(builder, units, gender);
                }
                else
                {
                    if (hundreds != 0)
                    {
                        builder.Add(AndSplit);
                    }
                    builder.Add(TensMap[tens]);

                }
            }
            else if (hundredRemainder != 0)
            {
                if (hundreds != 0)
                {
                    builder.Add(AndSplit);
                }
                GetUnits(builder, hundredRemainder, gender);
            }
        }
        private static void CollectOrdinal(IList<string> parts, ref int number, ref bool needsAnd, Fact rule, GrammaticalGender gender)
        {
            var remainder = number / rule.Power;
            if (remainder > 0)
            {
                number %= (int)rule.Power;

                if (number > 0 && (number > 19 || (number % 100 > 10 && number % 100 % 10 == 0))) // https://malfar.arnastofnun.is/grein/65658
                {
                    if (remainder == 1)
                    {
                        parts.Add(rule.Single);
                    }
                    else
                    {
                        CollectPartUnderOneThousand(parts, remainder, rule.Gender);
                        if (rule.Power > 0)
                        {
                            parts.Add(rule.Plural);
                        }
                    }
                }
                else
                {
                    var prevLen = parts.Count;
                    CollectOrdinalParts(parts, (int)remainder, rule, rule.Gender, gender);
                    if (number == 0 && needsAnd && false == parts.Skip(prevLen).Contains(AndSplit))
                    {
                        parts.Insert(prevLen, AndSplit);
                    }
                }
                needsAnd = true;
            }
        }
        public override string Convert(long number, GrammaticalGender gender, bool addAnd = true)
        {
            if (number == 0)
            {
                return UnitsMap[number];
            }

            var parts = new List<string>();
            if (number < 0)
            {
                parts.Add("mínus");
                number = -number;
            }

            var needsAnd = false;
            CollectParts(parts, ref number, ref needsAnd, PowerOfTenMap[18]);
            CollectParts(parts, ref number, ref needsAnd, PowerOfTenMap[15]);
            CollectParts(parts, ref number, ref needsAnd, PowerOfTenMap[12]);
            CollectParts(parts, ref number, ref needsAnd, PowerOfTenMap[9]);
            CollectParts(parts, ref number, ref needsAnd, PowerOfTenMap[6]);
            CollectParts(parts, ref number, ref needsAnd, PowerOfTenMap[3]);

            if (number > 0)
            {
                if (needsAnd && IsAndSplitNeeded((int)number))
                {
                    parts.Add(AndSplit);
                }
                CollectPartUnderOneThousand(parts, number, gender);
            }
            return string.Join(" ", parts);
        }
        public override string ConvertToOrdinal(int number, GrammaticalGender gender)
        {
            if (number == 0)
            {
                return UnitsOrdinalPrefixes[number] + GetOrdinalEnding(gender);
            }
            var parts = new List<string>();
            var needsAnd = false;

            CollectOrdinal(parts, ref number, ref needsAnd, PowerOfTenMap[12], gender);
            CollectOrdinal(parts, ref number, ref needsAnd, PowerOfTenMap[9], gender);
            CollectOrdinal(parts, ref number, ref needsAnd, PowerOfTenMap[6], gender);
            CollectOrdinal(parts, ref number, ref needsAnd, PowerOfTenMap[3], gender);

            if (number > 0)
            {
                if (needsAnd && IsAndSplitNeeded(number))
                {
                    parts.Add(AndSplit);
                }
                CollectOrdinalParts(parts, number, PowerOfTenMap[0], gender, gender);
            }
            return string.Join(" ", parts);
        }
    }
}
