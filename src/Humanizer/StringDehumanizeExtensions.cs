using System.Linq;

namespace Humanizer
{
    public static class StringDehumanizeExtensions
    {
        public static string Duhumanize(this string input)
        {
            var titlizedWords = from word in input.Split(' ')
                           select word.Humanize(LetterCasing.Title);

            return string.Join("", titlizedWords);
        }
    }
}
