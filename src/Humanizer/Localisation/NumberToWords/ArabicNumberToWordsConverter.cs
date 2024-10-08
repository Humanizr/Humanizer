namespace Humanizer;

class ArabicNumberToWordsConverter :
    GenderedNumberToWordsConverter
{
    static readonly string[] Groups = ["مئة", "ألف", "مليون", "مليار", "تريليون", "كوادريليون", "كوينتليون", "سكستيليون"];
    static readonly string[] AppendedGroups = ["", "ألفاً", "مليوناً", "ملياراً", "تريليوناً", "كوادريليوناً", "كوينتليوناً", "سكستيليوناً"];
    static readonly string[] PluralGroups = ["", "آلاف", "ملايين", "مليارات", "تريليونات", "كوادريليونات", "كوينتليونات", "سكستيليونات"];
    static readonly string[] OnesGroup = ["", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة", "عشرة", "أحد عشر", "اثنا عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر"];
    static readonly string[] TensGroup = ["", "عشرة", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون"];
    static readonly string[] HundredsGroup = ["", "مئة", "مئتان", "ثلاث مئة", "أربع مئة", "خمس مئة", "ست مئة", "سبع مئة", "ثمان مئة", "تسع مئة"];
    static readonly string[] AppendedTwos = ["مئتان", "ألفان", "مليونان", "ملياران", "تريليونان", "كوادريليونان", "كوينتليونان", "سكستيليونلن"];
    static readonly string[] Twos = ["مئتان", "ألفان", "مليونان", "ملياران", "تريليونان", "كوادريليونان", "كوينتليونان", "سكستيليونان"];

    static readonly string[] FeminineOnesGroup = ["", "واحدة", "اثنتان", "ثلاث", "أربع", "خمس", "ست", "سبع", "ثمان", "تسع", "عشر", "إحدى عشرة", "اثنتا عشرة", "ثلاث عشرة", "أربع عشرة", "خمس عشرة", "ست عشرة", "سبع عشرة", "ثمان عشرة", "تسع عشرة"];

    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true)
    {
        if (number == 0)
        {
            return "صفر";
        }

        if (number < 0)
        {
            return $"سالب {Convert(-number, gender)}";
        }

        var result = string.Empty;
        var groupLevel = 0;

        while (number >= 1)
        {
            var groupNumber = number % 1000;
            number /= 1000;

            var tens = groupNumber % 100;
            var hundreds = groupNumber / 100;
            var process = string.Empty;

            if (hundreds > 0)
            {
                if (tens == 0 && hundreds == 2)
                {
                    process = AppendedTwos[0];
                }
                else
                {
                    process = HundredsGroup[hundreds];
                }
            }

            if (tens > 0)
            {
                if (tens < 20)
                {
                    if (tens == 2 && hundreds == 0 && groupLevel > 0)
                    {
                        if (number is 2000 or 2000000 or 2000000000)
                        {
                            process = AppendedTwos[groupLevel];
                        }
                        else
                        {
                            process = Twos[groupLevel];
                        }
                    }
                    else
                    {
                        if (process != string.Empty)
                        {
                            process += " و ";
                        }

                        if (tens == 1 && groupLevel > 0 && hundreds == 0)
                        {
                            process += " ";
                        }
                        else
                        {
                            process += gender == GrammaticalGender.Feminine && groupLevel == 0 ? FeminineOnesGroup[tens] : OnesGroup[tens];
                        }
                    }
                }
                else
                {
                    var ones = tens % 10;
                    tens /= 10;

                    if (ones > 0)
                    {
                        if (process != string.Empty)
                        {
                            process += " و ";
                        }

                        process += gender == GrammaticalGender.Feminine ? FeminineOnesGroup[ones] : OnesGroup[ones];
                    }

                    if (process != string.Empty)
                    {
                        process += " و ";
                    }

                    process += TensGroup[tens];
                }
            }

            if (process != string.Empty)
            {
                if (groupLevel > 0)
                {
                    if (result != string.Empty)
                    {
                        result = $"و {result}";
                    }

                    if (groupNumber != 2)
                    {
                        if (groupNumber % 100 != 1)
                        {
                            if (groupNumber is >= 3 and <= 10)
                            {
                                result = $"{PluralGroups[groupLevel]} {result}";
                            }
                            else
                            {
                                result = $"{(result != string.Empty ? AppendedGroups[groupLevel] : Groups[groupLevel])} {result}";
                            }
                        }
                        else
                        {
                            result = $"{Groups[groupLevel]} {result}";
                        }
                    }
                }

                result = $"{process} {result}";
            }

            groupLevel++;
        }

        return result.Trim();
    }

    static readonly Dictionary<string, string> OrdinalExceptions = new()
    {
        {
            "واحد", "الحادي"
        },
        {
            "أحد", "الحادي"
        },
        {
            "اثنان", "الثاني"
        },
        {
            "اثنا", "الثاني"
        },
        {
            "ثلاثة", "الثالث"
        },
        {
            "أربعة", "الرابع"
        },
        {
            "خمسة", "الخامس"
        },
        {
            "ستة", "السادس"
        },
        {
            "سبعة", "السابع"
        },
        {
            "ثمانية", "الثامن"
        },
        {
            "تسعة", "التاسع"
        },
        {
            "عشرة", "العاشر"
        },
    };

    static readonly Dictionary<string, string> FeminineOrdinalExceptions = new()
    {
        {
            "واحدة", "الحادية"
        },
        {
            "إحدى", "الحادية"
        },
        {
            "اثنتان", "الثانية"
        },
        {
            "اثنتا", "الثانية"
        },
        {
            "ثلاث", "الثالثة"
        },
        {
            "أربع", "الرابعة"
        },
        {
            "خمس", "الخامسة"
        },
        {
            "ست", "السادسة"
        },
        {
            "سبع", "السابعة"
        },
        {
            "ثمان", "الثامنة"
        },
        {
            "تسع", "التاسعة"
        },
        {
            "عشر", "العاشرة"
        },
    };

    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        if (number == 0)
        {
            return "الصفر";
        }

        var beforeOneHundredNumber = number % 100;
        var overTensPart = number / 100 * 100;
        var beforeOneHundredWord = string.Empty;
        var overTensWord = string.Empty;

        if (beforeOneHundredNumber > 0)
        {
            beforeOneHundredWord = Convert(beforeOneHundredNumber, gender);
            beforeOneHundredWord = ParseNumber(beforeOneHundredWord, beforeOneHundredNumber, gender);
        }

        if (overTensPart > 0)
        {
            overTensWord = Convert(overTensPart);
            overTensWord = ParseNumber(overTensWord, overTensPart, gender);
        }

        var word = beforeOneHundredWord +
                   (overTensPart > 0
                       ? (string.IsNullOrWhiteSpace(beforeOneHundredWord) ? string.Empty : " بعد ") + overTensWord
                       : string.Empty);
        return word.Trim();
    }

    static string ParseNumber(string word, int number, GrammaticalGender gender)
    {
        if (number == 1)
        {
            return gender == GrammaticalGender.Feminine ? "الأولى" : "الأول";
        }

        if (number <= 10)
        {
            var ordinals = gender == GrammaticalGender.Feminine ? FeminineOrdinalExceptions : OrdinalExceptions;
            foreach (var kv in ordinals.Where(kv => word.EndsWith(kv.Key)))
            {
                // replace word with exception
                return StringHumanizeExtensions.Concat(
                    word.AsSpan(0, word.Length - kv.Key.Length),
                    kv.Value.AsSpan());
            }
        }
        else if (number is > 10 and < 100)
        {
            var parts = word.Split(' ');
            var newParts = new string[parts.Length];
            var count = 0;

            foreach (var part in parts)
            {
                var newPart = part;
                var oldPart = part;

                var ordinals = gender == GrammaticalGender.Feminine ? FeminineOrdinalExceptions : OrdinalExceptions;
                foreach (var kv in ordinals.Where(kv => oldPart.EndsWith(kv.Key)))
                {
                    // replace word with exception
                    newPart = StringHumanizeExtensions.Concat(
                        oldPart.AsSpan(0, oldPart.Length - kv.Key.Length),
                        kv.Value.AsSpan());
                }

                if (number > 19 && newPart == oldPart && oldPart.Length > 1)
                {
                    newPart = "ال" + oldPart;
                }

                newParts[count++] = newPart;
            }

            word = string.Join(" ", newParts);
        }
        else
        {
            word = "ال" + word;
        }

        return word;
    }
}
