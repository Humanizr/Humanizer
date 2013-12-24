using System;

namespace Humanizer
{
    /// <summary>
    /// ApplyCase method to allow changing the case of a sentence easily
    /// </summary>
    public static class CasingExtensions
    {
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
<<<<<<< HEAD
                    //TODO: RWM: Fix this in Portable Class Libraries
                    //return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
                    System.Diagnostics.Debug.WriteLine("TitleCase is not supported in Portable Class Libraries.");
                    return input;
=======
                    return input.Transform(To.TitleCase);
>>>>>>> 0d285f39a5d6d56c869e3ea01743f81197b1415f

                case LetterCasing.LowerCase:
                    return input.Transform(To.LowerCase);

                case LetterCasing.AllCaps:
                    return input.Transform(To.UpperCase);

                case LetterCasing.Sentence:
                    return input.Transform(To.SentenceCase);

                default:
                    throw new ArgumentOutOfRangeException("casing");
            }
        }

        
    }
}