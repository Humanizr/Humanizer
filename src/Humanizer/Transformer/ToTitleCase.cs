using System;
using System.Collections.Generic;
using System.Linq;

namespace Humanizer
{
    class ToTitleCase : IStringTransformer
    {
        public string Transform(string input)
        {
            var words = input.Split(' ');
            var result = new List<string>();
            foreach (var word in words)
            {
                if (word.Length == 0 || AllCapitals(word))
                    result.Add(word);
                else if(word.Length == 1)
                    result.Add(word.ToUpper());
                else 
                    result.Add(Char.ToUpper(word[0]) + word.Remove(0, 1).ToLower());
            }

            return string.Join(" ", result);
        }

        static bool AllCapitals(string input)
        {
            return input.ToCharArray().All(Char.IsUpper);
        }
    }
}