using System.Linq;

namespace Humanizer
{
    public static class StringDehumanizeExtensions
    {
        public static string Duhumanize(this string input)
        {
            var titlizedWords = 
                (from word in input.Split(' ')
                select word.Humanize(LetterCasing.Title)).ToArray(); // ToArrayed to support .Net 3.5

            return string.Join("", titlizedWords);
        }
    }
}
