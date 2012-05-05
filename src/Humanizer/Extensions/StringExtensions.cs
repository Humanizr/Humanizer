// Copyright (C) 2012, Mehdi Khalili
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of the <organization> nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.using System;

using System.Collections.Generic;
using System.Linq;

namespace Humanizer.Extensions
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
            if (input.Contains("_"))
                return FromUnderscoreSeparatedWords(input);

            return FromPascalCase(input);
        }
    }
}
