using System;
using System.Collections.Generic;
using System.Globalization;
using Humanizer.Localisation.Ordinalizers;

namespace Humanizer
{
    /// <summary>
    /// Ordinalize extensions
    /// </summary>
    public static class OrdinalizeExtensions
    {
        private static readonly IDictionary<string, Func<DefaultOrdinalizer>> OrdinalizerFactories =
            new Dictionary<string, Func<DefaultOrdinalizer>>
            {
                {"en", () => new EnglishOrdinalizer()},
                {"es", () => new SpanishOrdinalizer()},
                {"pt-BR", () => new BrazilianPortugueseOrdinalizer()},
                {"ru", () => new RussianOrdinalizer()}
            };

        /// <summary>
        /// Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
        /// </summary>
        /// <param name="numberString">The number, in string, to be ordinalized</param>
        /// <returns></returns>
        public static string Ordinalize(this string numberString)
        {
            return Ordinalizer.Convert(int.Parse(numberString), numberString);
        }

        /// <summary>
        /// Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
        /// Gender for Brazilian Portuguese locale
        /// 1.Ordinalize(GrammaticalGender.Masculine) -> "1º"
        /// 1.Ordinalize(GrammaticalGender.Feminine) -> "1ª"
        /// </summary>
        /// <param name="numberString">The number, in string, to be ordinalized</param>
        /// <param name="gender">The grammatical gender to use for output words</param>
        /// <returns></returns>
        public static string Ordinalize(this string numberString, GrammaticalGender gender)
        {
            return Ordinalizer.Convert(int.Parse(numberString), numberString, gender);
        }

        /// <summary>
        /// Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
        /// </summary>
        /// <param name="number">The number to be ordinalized</param>
        /// <returns></returns>
        public static string Ordinalize(this int number)
        {
            return Ordinalizer.Convert(number, number.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th.
        /// Gender for Brazilian Portuguese locale
        /// 1.Ordinalize(GrammaticalGender.Masculine) -> "1º"
        /// 1.Ordinalize(GrammaticalGender.Feminine) -> "1ª"
        /// </summary>
        /// <param name="number">The number to be ordinalized</param>
        /// <param name="gender">The grammatical gender to use for output words</param>
        /// <returns></returns>
        public static string Ordinalize(this int number, GrammaticalGender gender)
        {
            return Ordinalizer.Convert(number, number.ToString(CultureInfo.InvariantCulture), gender);
        }

        private static DefaultOrdinalizer Ordinalizer
        {
            get
            {
                Func<DefaultOrdinalizer> ordinalizerFactory;

                if (OrdinalizerFactories.TryGetValue(CultureInfo.CurrentUICulture.Name, out ordinalizerFactory))
                    return ordinalizerFactory();

                if (OrdinalizerFactories.TryGetValue(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, out ordinalizerFactory))
                    return ordinalizerFactory();

                return new DefaultOrdinalizer();
            }
        }
    }
}