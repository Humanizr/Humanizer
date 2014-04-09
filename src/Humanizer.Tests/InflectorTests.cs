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

using System.Collections;
using System.Collections.Generic;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests
{
    public class InflectorTests 
    {
        public readonly IList<object[]> PluralTestData = new List<object[]>();

        [Theory]
        [ClassData(typeof(PluralTestSource))]
        public void Pluralize(string singular, string plural)
        {
            Assert.Equal(plural, singular.Pluralize());
        }

        [Theory]
        [ClassData(typeof(PluralTestSource))]
        public void PluralizeAlreadyPluralWord(string singular, string plural)
        {
            Assert.Equal(plural, plural.Pluralize(Plurality.Plural));
        }

        [Theory]
        [ClassData(typeof(PluralTestSource))]
        public void PluralizeWordsWithUnknownPlurality(string singular, string plural)
        {
            Assert.Equal(plural, plural.Pluralize(Plurality.CouldBeEither));
            Assert.Equal(plural, singular.Pluralize(Plurality.CouldBeEither));
        }

        [Theory]
        [ClassData(typeof(PluralTestSource))]
        public void Singularize(string singular, string plural)
        {
            Assert.Equal(singular, plural.Singularize());
        }

        [Theory]
        [ClassData(typeof(PluralTestSource))]
        public void SingularizeAlreadySingularWord(string singular, string plural)
        {
            Assert.Equal(singular, singular.Singularize(Plurality.Singular));
        }

        [Theory]
        [ClassData(typeof(PluralTestSource))]
        public void SingularizeWordsWithUnknownSingularity(string singular, string plural)
        {
            Assert.Equal(singular, singular.Singularize(Plurality.CouldBeEither));
            Assert.Equal(singular, plural.Singularize(Plurality.CouldBeEither));
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

    class PluralTestSource : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {"search", "searches"};
            yield return new object[] {"switch", "switches"};
            yield return new object[] {"fix", "fixes"};
            yield return new object[] {"box", "boxes"};
            yield return new object[] {"process", "processes"};
            yield return new object[] {"address", "addresses"};
            yield return new object[] {"case", "cases"};
            yield return new object[] {"stack", "stacks"};
            yield return new object[] {"wish", "wishes"};
            yield return new object[] {"fish", "fish"};

            yield return new object[] {"category", "categories"};
            yield return new object[] {"query", "queries"};
            yield return new object[] {"ability", "abilities"};
            yield return new object[] {"agency", "agencies"};
            yield return new object[] {"movie", "movies"};

            yield return new object[] {"archive", "archives"};

            yield return new object[] {"index", "indices"};

            yield return new object[] {"wife", "wives"};
            yield return new object[] {"safe", "saves"};
            yield return new object[] {"half", "halves"};

            yield return new object[] {"move", "moves"};

            yield return new object[] {"salesperson", "salespeople"};
            yield return new object[] {"person", "people"};

            yield return new object[] {"spokesman", "spokesmen"};
            yield return new object[] {"man", "men"};
            yield return new object[] {"woman", "women"};

            yield return new object[] {"basis", "bases"};
            yield return new object[] {"diagnosis", "diagnoses"};

            yield return new object[] {"datum", "data"};
            yield return new object[] {"medium", "media"};
            yield return new object[] {"analysis", "analyses"};

            yield return new object[] {"node_child", "node_children"};
            yield return new object[] {"child", "children"};

            yield return new object[] {"experience", "experiences"};
            yield return new object[] {"day", "days"};

            yield return new object[] {"comment", "comments"};
            yield return new object[] {"foobar", "foobars"};
            yield return new object[] {"newsletter", "newsletters"};

            yield return new object[] {"old_news", "old_news"};
            yield return new object[] {"news", "news"};

            yield return new object[] {"series", "series"};
            yield return new object[] {"species", "species"};

            yield return new object[] {"quiz", "quizzes"};

            yield return new object[] {"perspective", "perspectives"};

            yield return new object[] {"ox", "oxen"};
            yield return new object[] {"photo", "photos"};
            yield return new object[] {"buffalo", "buffaloes"};
            yield return new object[] {"tomato", "tomatoes"};
            yield return new object[] {"dwarf", "dwarves"};
            yield return new object[] {"elf", "elves"};
            yield return new object[] {"information", "information"};
            yield return new object[] {"equipment", "equipment"};
            yield return new object[] {"bus", "buses"};
            yield return new object[] {"status", "statuses"};
            yield return new object[] {"status_code", "status_codes"};
            yield return new object[] {"mouse", "mice"};

            yield return new object[] {"louse", "lice"};
            yield return new object[] {"house", "houses"};
            yield return new object[] {"octopus", "octopi"};
            yield return new object[] {"virus", "viri"};
            yield return new object[] {"alias", "aliases"};
            yield return new object[] {"portfolio", "portfolios"};

            yield return new object[] {"vertex", "vertices"};
            yield return new object[] {"matrix", "matrices"};

            yield return new object[] {"axis", "axes"};
            yield return new object[] {"testis", "testes"};
            yield return new object[] {"crisis", "crises"};

            yield return new object[] {"rice", "rice"};
            yield return new object[] {"shoe", "shoes"};

            yield return new object[] {"horse", "horses"};
            yield return new object[] {"prize", "prizes"};
            yield return new object[] {"edge", "edges"};

            /* Tests added by Bas Jansen */
            yield return new object[] {"goose", "geese"};
            yield return new object[] {"deer", "deer"};
            yield return new object[] {"sheep", "sheep"};
            yield return new object[] {"wolf", "wolves"};
            yield return new object[] {"volcano", "volcanoes"};
            yield return new object[] {"aircraft", "aircraft"};
            yield return new object[] {"alumna", "alumnae"};
            yield return new object[] {"alumnus", "alumni"};
            yield return new object[] {"fungus", "fungi"};
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
