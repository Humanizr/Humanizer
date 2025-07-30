namespace Humanizer;

class BrazilianPortugueseNumberToWordsConverter :
    GenderedNumberToWordsConverter
{
    static readonly string[] UnitsMap = ["zero", "um", "dois", "três", "quatro", "cinco", "seis", "sete", "oito", "nove", "dez", "onze", "doze", "treze", "quatorze", "quinze", "dezesseis", "dezessete", "dezoito", "dezenove"];
    static readonly string[] TensMap = ["zero", "dez", "vinte", "trinta", "quarenta", "cinquenta", "sessenta", "setenta", "oitenta", "noventa"];
    static readonly string[] HundredsMap = ["zero", "cento", "duzentos", "trezentos", "quatrocentos", "quinhentos", "seiscentos", "setecentos", "oitocentos", "novecentos"];

    static readonly string[] OrdinalUnitsMap = ["zero", "primeiro", "segundo", "terceiro", "quarto", "quinto", "sexto", "sétimo", "oitavo", "nono"];
    static readonly string[] OrdinalTensMap = ["zero", "décimo", "vigésimo", "trigésimo", "quadragésimo", "quinquagésimo", "sexagésimo", "septuagésimo", "octogésimo", "nonagésimo"];
    static readonly string[] OrdinalHundredsMap = ["zero", "centésimo", "ducentésimo", "trecentésimo", "quadringentésimo", "quingentésimo", "sexcentésimo", "septingentésimo", "octingentésimo", "noningentésimo"];

    public override string Convert(long input, GrammaticalGender gender, bool addAnd = true)
    {
        if (input is > 999999999999 or < -999999999999)
        {
            throw new NotImplementedException();
        }

        var number = input;

        if (number == 0)
        {
            return "zero";
        }

        if (number < 0)
        {
            return $"menos {Convert(Math.Abs(number), gender)}";
        }

        var parts = new List<string>();

        if (number / 1000000000 > 0)
        {
            // gender is not applied for billions
            parts.Add(number / 1000000000 >= 2
                ? $"{Convert(number / 1000000000, GrammaticalGender.Masculine)} bilhões"
                : $"{Convert(number / 1000000000, GrammaticalGender.Masculine)} bilhão");

            number %= 1000000000;
        }

        if (number / 1000000 > 0)
        {
            // gender is not applied for millions
            parts.Add(number / 1000000 >= 2
                ? $"{Convert(number / 1000000, GrammaticalGender.Masculine)} milhões"
                : $"{Convert(number / 1000000, GrammaticalGender.Masculine)} milhão");

            number %= 1000000;
        }

        if (number / 1000 > 0)
        {
            // gender is not applied for thousands
            parts.Add(number / 1000 == 1 ? "mil" : $"{Convert(number / 1000, GrammaticalGender.Masculine)} mil");
            number %= 1000;
        }

        if (number / 100 > 0)
        {
            if (number == 100)
            {
                parts.Add(parts.Count > 0 ? "e cem" : "cem");
            }
            else
            {
                // Gender is applied to hundreds starting from 200
                parts.Add(ApplyGender(HundredsMap[number / 100], gender));
            }

            number %= 100;
        }

        if (number > 0)
        {
            if (parts.Count != 0)
            {
                parts.Add("e");
            }

            if (number < 20)
            {
                parts.Add(ApplyGender(UnitsMap[number], gender));
            }
            else
            {
                var lastPart = TensMap[number / 10];
                if (number % 10 > 0)
                {
                    lastPart += $" e {ApplyGender(UnitsMap[number % 10], gender)}";
                }

                parts.Add(lastPart);
            }
        }

        return string.Join(" ", parts);
    }

    public override string ConvertToOrdinal(int number, GrammaticalGender gender)
    {
        // N/A in Portuguese ordinal
        if (number == 0)
        {
            return "zero";
        }

        var parts = new List<string>();

        if (number / 1000000000 > 0)
        {
            parts.Add(number / 1000000000 == 1
                ? ApplyOrdinalGender("bilionésimo", gender)
                : string.Format("{0} " + ApplyOrdinalGender("bilionésimo", gender), ConvertToOrdinal(number / 1000000000, gender)));

            number %= 1000000000;
        }

        if (number / 1000000 > 0)
        {
            parts.Add(number / 1000000 == 1
                ? ApplyOrdinalGender("milionésimo", gender)
                : string.Format("{0}" + ApplyOrdinalGender("milionésimo", gender), ConvertToOrdinal(number / 1000000000, gender)));

            number %= 1000000;
        }

        if (number / 1000 > 0)
        {
            parts.Add(number / 1000 == 1
                ? ApplyOrdinalGender("milésimo", gender)
                : string.Format("{0} " + ApplyOrdinalGender("milésimo", gender), ConvertToOrdinal(number / 1000, gender)));

            number %= 1000;
        }

        if (number / 100 > 0)
        {
            parts.Add(ApplyOrdinalGender(OrdinalHundredsMap[number / 100], gender));
            number %= 100;
        }

        if (number / 10 > 0)
        {
            parts.Add(ApplyOrdinalGender(OrdinalTensMap[number / 10], gender));
            number %= 10;
        }

        if (number > 0)
        {
            parts.Add(ApplyOrdinalGender(OrdinalUnitsMap[number], gender));
        }

        return string.Join(" ", parts);
    }

    static string ApplyGender(string toWords, GrammaticalGender gender)
    {
        if (gender != GrammaticalGender.Feminine)
        {
            return toWords;
        }

        if (toWords.EndsWith("os"))
        {
            return StringHumanizeExtensions.Concat(toWords.AsSpan(0, toWords.Length - 2), "as".AsSpan());
        }

        if (toWords.EndsWith("um"))
        {
            return StringHumanizeExtensions.Concat(toWords.AsSpan(0, toWords.Length - 2), "uma".AsSpan());
        }

        if (toWords.EndsWith("dois"))
        {
            return StringHumanizeExtensions.Concat(toWords.AsSpan(0, toWords.Length - 4), "duas".AsSpan());
        }

        return toWords;
    }

    static string ApplyOrdinalGender(string toWords, GrammaticalGender gender)
    {
        if (gender == GrammaticalGender.Feminine)
        {
            return StringHumanizeExtensions.Concat(
                toWords.AsSpan().TrimEnd('o'),
                'a');
        }

        return toWords;
    }
}