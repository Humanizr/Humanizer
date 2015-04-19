//The Inflector class was cloned from Inflector (https://github.com/srkirkland/Inflector)

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

using System.Collections.Generic;
using Humanizer.Inflections;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests
{
    public class InflectorTests
    {
        public readonly IList<object[]> PluralTestData = new List<object[]>();

        // All singular/pluralize tests just need to demonstrate that they're 
        // delegating to the default vocab.
        [Theory]
        [InlineData("mouse", "mice")]
        public void Pluralize(string singular, string plural)
        {
            Assert.Equal(plural, Vocabularies.Default.Pluralize(singular));
        }

        [Theory]
        [InlineData("mouse", "mice")]
        public void PluralizeWordsWithUnknownPlurality(string singular, string plural)
        {
            Assert.Equal(plural, Vocabularies.Default.Pluralize(singular, false));
            Assert.Equal(plural, Vocabularies.Default.Pluralize(plural, false));
        }

        [Theory]
        [InlineData("mouse", "mice")]
        public void Singularize(string singular, string plural)
        {
            Assert.Equal(singular, Vocabularies.Default.Singularize(plural));
        }

        [Theory]
        [InlineData("mouse", "mice")]
        public void SingularizeWordsWithUnknownSingularity(string singular, string plural)
        {
            Assert.Equal(singular, Vocabularies.Default.Singularize(singular, false));
            Assert.Equal(singular, Vocabularies.Default.Singularize(plural, false));
        }

        //Uppercases individual words and removes some characters 
        [Theory]
        [InlineData("some title", "Some Title")]
        [InlineData("some-title", "Some Title")]
        [InlineData("sometitle", "Sometitle")]
        [InlineData("some-title: The begining", "Some Title: The Begining")]
        [InlineData("some_title:_the_begining", "Some Title: The Begining")]
        [InlineData("some title: The_begining", "Some Title: The Begining")]
        public void Titleize(string input, string expectedOuput)
        {
            Assert.Equal(expectedOuput, input.Titleize());
        }

        [InlineData("some_title", "some-title")]
        [InlineData("some-title", "some-title")]
        [InlineData("some_title_goes_here", "some-title-goes-here")]
        [InlineData("some_title and_another", "some-title and-another")]
        [Theory]
        public void Dasherize(string input, string expectedOutput)
        {
            Assert.Equal(input.Dasherize(), expectedOutput);
        }

        [InlineData("some_title", "some-title")]
        [InlineData("some-title", "some-title")]
        [InlineData("some_title_goes_here", "some-title-goes-here")]
        [InlineData("some_title and_another", "some-title and-another")]
        [Theory]
        public void Hyphenate(string input, string expectedOutput)
        {
            Assert.Equal(input.Hyphenate(), expectedOutput);
        }

        [Theory]
        [InlineData("customer", "Customer")]
        [InlineData("CUSTOMER", "CUSTOMER")]
        [InlineData("CUStomer", "CUStomer")]
        [InlineData("customer_name", "CustomerName")]
        [InlineData("customer_first_name", "CustomerFirstName")]
        [InlineData("customer_first_name_goes_here", "CustomerFirstNameGoesHere")]
        [InlineData("customer name", "Customer name")]
        public void Pascalize(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Pascalize());
        }

        // Same as pascalize, except first char is lowercase
        [Theory]
        [InlineData("customer", "customer")]
        [InlineData("CUSTOMER", "cUSTOMER")]
        [InlineData("CUStomer", "cUStomer")]
        [InlineData("customer_name", "customerName")]
        [InlineData("customer_first_name", "customerFirstName")]
        [InlineData("customer_first_name_goes_here", "customerFirstNameGoesHere")]
        [InlineData("customer name", "customer name")]
        public void Camelize(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Camelize());
        }


        //Makes an underscored lowercase string
        [Theory]
        [InlineData("SomeTitle", "some_title")]
        [InlineData("someTitle", "some_title")]
        [InlineData("some title", "some_title")]
        [InlineData("some title that will be underscored", "some_title_that_will_be_underscored")]
        [InlineData("SomeTitleThatWillBeUnderscored", "some_title_that_will_be_underscored")]
        public void Underscore(string input, string expectedOuput)
        {
            Assert.Equal(expectedOuput, input.Underscore());
        }
    }
}
