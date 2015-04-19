﻿//The Inflector class was cloned from Inflector (https://github.com/srkirkland/Inflector)

//The MIT License (MIT)

//Copyright (c) 2013 Scott Kirkland

//Permission is hereby granted, free of charge, to any person obtaining a copy of
//this software and associated documentation files (the "Software"), to deal in
//the Software without restriction, including without limitation the rights to
//use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//the Software, and to permit persons to whom the Software is furnished to do so,
//subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Text.RegularExpressions;
using Humanizer.Inflections;

namespace Humanizer
{
    /// <summary>
    /// Inflector extensions
    /// </summary>
    public static class InflectorExtensions
    {
        /// <summary>
        /// Pluralizes the provided input considering irregular words
        /// </summary>
        /// <param name="word">Word to be pluralized</param>
        /// <param name="plurality">Normally you call Pluralize on singular words; but if you're unsure call it with Plurality.CouldBeEither</param>
        /// <returns></returns>
        [Obsolete("Use string.Pluralize(bool) instead. This method will be removed in next major release.")]
        public static string Pluralize(this string word, Plurality plurality)
        {
            return plurality == Plurality.Plural ? word : word.Pluralize(inputIsKnownToBeSingular: false);
        }

        /// <summary>
        /// Pluralizes the provided input considering irregular words
        /// </summary>
        /// <param name="word">Word to be pluralized</param>
        /// <param name="inputIsKnownToBeSingular">Normally you call Pluralize on singular words; but if you're unsure call it with false</param>
        /// <returns></returns>
        public static string Pluralize(this string word, bool inputIsKnownToBeSingular = true)
        {
            return Vocabularies.Default.Pluralize(word, inputIsKnownToBeSingular);
        }

        /// <summary>
        /// Singularizes the provided input considering irregular words
        /// </summary>
        /// <param name="word">Word to be singularized</param>
        /// <param name="plurality">Normally you call Singularize on plural words; but if you're unsure call it with Plurality.CouldBeEither</param>
        /// <returns></returns>
        [Obsolete("Use string.Singularize(bool) instead. This method will be removed in next major release.")]
        public static string Singularize(this string word, Plurality plurality)
        {
            return plurality == Plurality.Singular ? word : word.Singularize(inputIsKnownToBePlural: false);
        }

        /// <summary>
        /// Singularizes the provided input considering irregular words
        /// </summary>
        /// <param name="word">Word to be singularized</param>
        /// <param name="inputIsKnownToBePlural">Normally you call Singularize on plural words; but if you're unsure call it with false</param>
        /// <returns></returns>
        public static string Singularize(this string word, bool inputIsKnownToBePlural = true)
        {
            return Vocabularies.Default.Singularize(word, inputIsKnownToBePlural);
        }

        /// <summary>
        /// Humanizes the input with Title casing
        /// </summary>
        /// <param name="input">The string to be titleized</param>
        /// <returns></returns>
        public static string Titleize(this string input)
        {
            return input.Humanize(LetterCasing.Title);
        }

        /// <summary>
        /// By default, pascalize converts strings to UpperCamelCase also removing underscores
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Pascalize(this string input)
        {
            return Regex.Replace(input, "(?:^|_)(.)", match => match.Groups[1].Value.ToUpper());
        }

        /// <summary>
        /// Same as Pascalize except that the first character is lower case
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Camelize(this string input)
        {
            string word = Pascalize(input);
            return word.Substring(0, 1).ToLower() + word.Substring(1);
        }

        /// <summary>
        /// Separates the input words with underscore
        /// </summary>
        /// <param name="input">The string to be underscored</param>
        /// <returns></returns>
        public static string Underscore(this string input)
        {
            return Regex.Replace(
                Regex.Replace(
                    Regex.Replace(input, @"([A-Z]+)([A-Z][a-z])", "$1_$2"), @"([a-z\d])([A-Z])", "$1_$2"), @"[-\s]", "_").ToLower();
        }

        /// <summary>
        /// Replaces underscores with dashes in the string
        /// </summary>
        /// <param name="underscoredWord"></param>
        /// <returns></returns>
        public static string Dasherize(this string underscoredWord)
        {
            return underscoredWord.Replace('_', '-');
        }
		
        /// <summary>
        /// Replaces underscores with hyphens in the string
        /// </summary>
        /// <param name="underscoredWord"></param>
        /// <returns></returns>
        public static string Hyphenate(this string underscoredWord)
        {
            return Dasherize(underscoredWord);
        }
    }
}