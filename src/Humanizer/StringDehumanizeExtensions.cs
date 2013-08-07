using System.Linq;

namespace Humanizer
{
    public static class StringDehumanizeExtensions
    {
        /// <summary>
        /// Dehumanizes a string; e.g. 'some string', 'Some String', 'Some string' -> 'SomeString'
        /// </summary>
        /// <param name="input">The string to be dehumanized</param>
        /// <returns></returns>
        public static string Dehumanize(this string input)
        {
            var titlizedWords = 
                (from word in input.Split(' ')
                select word.Humanize(LetterCasing.Title)).ToArray(); // ToArrayed to support .Net 3.5

            return string.Join("", titlizedWords);
        }
    }
}
