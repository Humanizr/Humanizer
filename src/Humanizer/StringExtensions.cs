using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Humanizer
{
    public static class StringExtensions
    {
        static readonly Func<string, string> FromUnderscoreSeparatedWords = methodName => string.Join(" ", methodName.Split(new[] { '_' }));
        static string FromPascalCase(string name)
        {
            var chars = name.Aggregate(
                new List<char>(),
                (list, currentChar) =>
                {
                    if (currentChar == ' ')
                    {
                        list.Add(currentChar);
                        return list;
                    }

                    if (list.Count == 0)
                    {
                        list.Add(currentChar);
                        return list;
                    }

                    var lastCharacterInTheList = list[list.Count - 1];
                    if (lastCharacterInTheList != ' ')
                    {
                        if (char.IsDigit(lastCharacterInTheList))
                        {
                            if (char.IsLetter(currentChar))
                                list.Add(' ');
                        }
                        else if (!char.IsLower(currentChar))
                            list.Add(' ');
                    }

                    list.Add(char.ToLower(currentChar));

                    return list;
                });

            var result = new string(chars.ToArray());
            return result.Replace(" i ", " I "); // I is an exception
        }

        public static string Humanize(this string input)
        {
            // if input is all capitals (e.g. an acronym) then return it without change
            if (!input.Any(Char.IsLower))
                return input;

            if (input.Contains("_"))
                return FromUnderscoreSeparatedWords(input);

            return FromPascalCase(input);
        }

        public static string Humanize(this string input, LetterCasing casing)
        {
            var humanizedString = input.Humanize();

            return ApplyCase(humanizedString, casing);
        }

        public static string ApplyCase(this string input, LetterCasing casing)
        {
            switch (casing)
            {
                case LetterCasing.Title:
                    return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);

                case LetterCasing.LowerCase:
                    return input.ToLower();

                case LetterCasing.AllCaps:
                    return input.ToUpper();

                case LetterCasing.Sentence:
                    if (input.Length >= 1)
                        return string.Concat(input.Substring(0, 1).ToUpper(), input.Substring(1));

                    return input.ToUpper();

                default:
                    throw new ArgumentOutOfRangeException("casing");
            }
        }
    }
}
