namespace Humanizer;

class BrazilianPortugueseNumberToWordsConverter :
    GenderedNumberToWordsConverter
{
    static readonly string[] MasculineUnitsMap = ["zero", "um", "dois", "três", "quatro", "cinco", "seis", "sete", "oito", "nove", "dez", "onze", "doze", "treze", "quatorze", "quinze", "dezesseis", "dezessete", "dezoito", "dezenove"];
    static readonly string[] FeminineUnitsMap = ["zera", "uma", "duas", "três", "quatra", "cinca", "seis", "sete", "oito", "nove", "dez", "onze", "doze", "treze", "quatorze", "quinze", "dezesseis", "dezessete", "dezoita", "dezenove"];
    static readonly string[] MasculineTensMap = ["zero", "dez", "vinte", "trinta", "quarenta", "cinquenta", "sessenta", "setenta", "oitenta", "noventa"];
    static readonly string[] FeminineTensMap = ["zera", "dez", "vinte", "trinta", "quarenta", "cinquenta", "sessenta", "setenta", "oitenta", "noventa"];
    static readonly string[] MasculineHundredsMap = ["zero", "cento", "duzentos", "trezentos", "quatrocentos", "quinhentos", "seiscentos", "setecentos", "oitocentos", "novecentos"];
    static readonly string[] FeminineHundredsMap = ["zera", "centa", "duzentas", "trezentas", "quatrocentas", "quinhentas", "seiscentas", "setecentas", "oitocentas", "novecentas"];
    static readonly string[] MasculineOrdinalUnitsMap = ["zero", "primeiro", "segundo", "terceiro", "quarto", "quinto", "sexto", "sétimo", "oitavo", "nono"];
    static readonly string[] FeminineOrdinalUnitsMap = ["zera", "primeira", "segunda", "terceira", "quarta", "quinta", "sexta", "sétima", "oitava", "nona"];
    static readonly string[] MasculineOrdinalTensMap = ["zero", "décimo", "vigésimo", "trigésimo", "quadragésimo", "quinquagésimo", "sexagésimo", "septuagésimo", "octogésimo", "nonagésimo"];
    static readonly string[] FeminineOrdinalTensMap = ["zera", "décima", "vigésima", "trigésima", "quadragésima", "quinquagésima", "sexagésima", "septuagésima", "octogésima", "nonagésima"];
    static readonly string[] MasculineOrdinalHundredsMap = ["zero", "centésimo", "ducentésimo", "trecentésimo", "quadringentésimo", "quingentésimo", "sexcentésimo", "septingentésimo", "octingentésimo", "noningentésimo"];
    static readonly string[] FeminineOrdinalHundredsMap = ["zera", "centésima", "ducentésima", "trecentésima", "quadringentésima", "quingentésima", "sexcentésima", "septingentésima", "octingentésima", "noningentésima"];

    public override string Convert(long number, GrammaticalGender gender, bool addAnd = true)
    {
        if (number is > 999999999999 or < -999999999999)
        {
            throw new NotImplementedException();
        }

        if (gender == GrammaticalGender.Feminine)
        {
            return ConvertFeminine(number);
        }

        return ConvertMasculine(number);
    }

    string ConvertMasculine(long number)
    {
        if (number == 0)
        {
            return "zero";
        }

        if (number < 0)
        {
            return $"menos {ConvertMasculine(Math.Abs(number))}";
        }

        var parts = new List<string>();

        if (number / 1000000000 > 0)
        {
            // gender is not applied for billions
            parts.Add(number / 1000000000 >= 2
                ? $"{ConvertMasculine(number / 1000000000)} bilhões"
                : $"{ConvertMasculine(number / 1000000000)} bilhão");

            number %= 1000000000;
        }

        if (number / 1000000 > 0)
        {
            // gender is not applied for millions
            parts.Add(number / 1000000 >= 2
                ? $"{ConvertMasculine(number / 1000000)} milhões"
                : $"{ConvertMasculine(number / 1000000)} milhão");

            number %= 1000000;
        }

        if (number / 1000 > 0)
        {
            // gender is not applied for thousands
            parts.Add(number / 1000 == 1 ? "mil" : $"{ConvertMasculine(number / 1000)} mil");
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
                parts.Add(MasculineHundredsMap[number / 100]);
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
                parts.Add(MasculineUnitsMap[number]);
            }
            else
            {
                var lastPart = MasculineTensMap[number / 10];
                if (number % 10 > 0)
                {
                    lastPart += $" e {MasculineUnitsMap[number % 10]}";
                }

                parts.Add(lastPart);
            }
        }

        return string.Join(" ", parts);
    }

    string ConvertFeminine(long number)
    {
        if (number == 0)
        {
            return "zero";
        }

        if (number < 0)
        {
            return $"menos {ConvertFeminine(Math.Abs(number))}";
        }

        var parts = new List<string>();

        if (number / 1000000000 > 0)
        {
            // gender is not applied for billions
            parts.Add(number / 1000000000 >= 2
                ? $"{ConvertMasculine(number / 1000000000)} bilhões"
                : $"{ConvertMasculine(number / 1000000000)} bilhão");

            number %= 1000000000;
        }

        if (number / 1000000 > 0)
        {
            // gender is not applied for millions
            parts.Add(number / 1000000 >= 2
                ? $"{ConvertMasculine(number / 1000000)} milhões"
                : $"{ConvertMasculine(number / 1000000)} milhão");

            number %= 1000000;
        }

        if (number / 1000 > 0)
        {
            if (number / 1000 == 1)
            {
                parts.Add("mil");
            }
            else
            {
                // gender is not applied for thousands
                parts.Add($"{ConvertMasculine(number / 1000)} mil");
            }

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
                parts.Add(FeminineHundredsMap[number / 100]);
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
                parts.Add(FeminineUnitsMap[number]);
            }
            else
            {
                var lastPart = FeminineTensMap[number / 10];
                if (number % 10 > 0)
                {
                    lastPart += $" e {FeminineUnitsMap[number % 10]}";
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

        if (gender == GrammaticalGender.Feminine)
        {
            return ConvertToOrdinalFeminine(number);
        }

        return ConvertToOrdinalMasculine(number);
    }

    static string ConvertToOrdinalFeminine(int number)
    {
        var parts = new List<string>();

        if (number / 1000000000 > 0)
        {
            if (number / 1000000000 == 1)
            {
                parts.Add("bilionésima");
            }
            else
            {
                parts.Add($"{ConvertToOrdinalFeminine(number / 1000000000)} bilionésima");
            }

            number %= 1000000000;
        }

        if (number / 1000000 > 0)
        {
            if (number / 1000000 == 1)
            {
                parts.Add("milionésima");
            }
            else
            {
                parts.Add($"{ConvertToOrdinalFeminine(number / 1000000000)} milionésima");
            }

            number %= 1000000;
        }

        if (number / 1000 > 0)
        {
            if (number / 1000 == 1)
            {
                parts.Add( "milésima");
            }
            else
            {
                parts.Add($"{ConvertToOrdinalFeminine(number / 1000)} milésima");
            }
            number %= 1000;
        }

        if (number / 100 > 0)
        {
            parts.Add(FeminineOrdinalHundredsMap[number / 100]);
            number %= 100;
        }

        if (number / 10 > 0)
        {
            parts.Add(FeminineOrdinalTensMap[number / 10]);
            number %= 10;
        }

        if (number > 0)
        {
            parts.Add(FeminineOrdinalUnitsMap[number]);
        }

        return string.Join(" ", parts);
    }

    static string ConvertToOrdinalMasculine(int number)
    {
        var parts = new List<string>();

        if (number / 1000000000 > 0)
        {
            if (number / 1000000000 == 1)
            {
                parts.Add("bilionésimo");
            }
            else
            {
                parts.Add($"{ConvertToOrdinalMasculine(number / 1000000000)} bilionésimo");
            }

            number %= 1000000000;
        }

        if (number / 1000000 > 0)
        {
            parts.Add(number / 1000000 == 1
                ? "milionésimo"
                : $"{ConvertToOrdinalMasculine(number / 1000000000)} milionésimo");

            number %= 1000000;
        }

        if (number / 1000 > 0)
        {
            parts.Add(number / 1000 == 1
                ? "milésimo"
                : $"{ConvertToOrdinalMasculine(number / 1000)} milésimo");

            number %= 1000;
        }

        if (number / 100 > 0)
        {
            parts.Add(MasculineOrdinalHundredsMap[number / 100]);

            number %= 100;
        }

        if (number / 10 > 0)
        {
            parts.Add(MasculineOrdinalTensMap[number / 10]);

            number %= 10;
        }

        if (number > 0)
        {
            parts.Add(MasculineOrdinalUnitsMap[number]);
        }

        return string.Join(" ", parts);
    }
}