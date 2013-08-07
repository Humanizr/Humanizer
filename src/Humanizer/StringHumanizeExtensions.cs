using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Humanizer
{
    public static class StringHumanizeExtensions
    {
        static readonly Func<string, string> FromUnderscoreDashSeparatedWords = methodName => string.Join(" ", methodName.Split(new[] { '_', '-' }));
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

        /// <summary>
        /// Humanizes the input string; e.g. Underscored_input_String_is_turned_INTO_sentence -> 'Underscored input String is turned INTO sentence'
        /// </summary>
        /// <param name="input">The string to be humanized</param>
        /// <returns></returns>
        public static string Humanize(this string input)
        {
            // if input is all capitals (e.g. an acronym) then return it without change
            if (!input.Any(Char.IsLower))
                return input;

            if (input.Contains('_') || input.Contains('-'))
                return FromUnderscoreDashSeparatedWords(input);

            return FromPascalCase(input);
        }

        /// <summary>
        /// Humanized the input string based on the provided casing
        /// </summary>
        /// <param name="input">The string to be humanized</param>
        /// <param name="casing">The desired casing for the output</param>
        /// <returns></returns>
        public static string Humanize(this string input, LetterCasing casing)
        {
            var humanizedString = input.Humanize();

            return ApplyCase(humanizedString, casing);
        }

        /// <summary>
        /// Changes the casing of the provided input
        /// </summary>
        /// <param name="input"></param>
        /// <param name="casing"></param>
        /// <returns></returns>
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
