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
        public void PluralizeWordsWithUnknownPlurality(string singular, string plural)
        {
            Assert.Equal(plural, plural.Pluralize(false));
            Assert.Equal(plural, singular.Pluralize(false));
        }

        [Theory]
        [ClassData(typeof(PluralTestSource))]
        public void Singularize(string singular, string plural)
        {
            Assert.Equal(singular, plural.Singularize());
        }

        [Theory]
        [ClassData(typeof(PluralTestSource))]
        public void SingularizeWordsWithUnknownSingularity(string singular, string plural)
        {
            Assert.Equal(singular, singular.Singularize(false));
            Assert.Equal(singular, plural.Singularize(false));
        }

        [Theory]
        [InlineData("tires", "tires")]
        [InlineData("body", "bodies")]
        [InlineData("traxxas", "traxxas")]
        public void SingularizeSkipSimpleWords(string singular, string plural)
        {
            Assert.Equal(singular, plural.Singularize(skipSimpleWords: true));
        }

        [Theory]
        [InlineData("a")]
        [InlineData("A")]
        [InlineData("s")]
        [InlineData("S")]
        [InlineData("z")]
        [InlineData("Z")]
        [InlineData("1")]
        public void SingularizeSingleLetter(string input)
        {
            Assert.Equal(input, input.Singularize());
        }

        //Uppercases individual words and removes some characters 
        [Theory]
        [InlineData("some title", "Some Title")]
        [InlineData("some-title", "Some Title")]
        [InlineData("sometitle", "Sometitle")]
        [InlineData("some-title: The beginning", "Some Title: The Beginning")]
        [InlineData("some_title:_the_beginning", "Some Title: the Beginning")]
        [InlineData("some title: The_beginning", "Some Title: The Beginning")]
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
        [InlineData("customer_first_name goes here", "CustomerFirstNameGoesHere")]
        [InlineData("customer name", "CustomerName")]
        [InlineData("customer   name", "CustomerName")]
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
        [InlineData("customer_first_name goes here", "customerFirstNameGoesHere")]
        [InlineData("customer name", "customerName")]
        [InlineData("customer   name", "customerName")]
        [InlineData("", "")]
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
        [InlineData("SomeForeignWordsLikeÄgyptenÑu", "some_foreign_words_like_ägypten_ñu")]
        [InlineData("Some wordsTo be Underscored", "some_words_to_be_underscored")]
        public void Underscore(string input, string expectedOuput)
        {
            Assert.Equal(expectedOuput, input.Underscore());
        }

        // transform words into lowercase and separate with a -
        [Theory]
        [InlineData("SomeWords", "some-words")]
        [InlineData("SOME words TOGETHER", "some-words-together")]
        [InlineData("A spanish word EL niño", "a-spanish-word-el-niño")]
        [InlineData("SomeForeignWords ÆgÑuÄgypten", "some-foreign-words-æg-ñu-ägypten")]
        [InlineData("A VeryShortSENTENCE", "a-very-short-sentence")]
        public void Kebaberize(string input, string expectedOutput)
        {
            Assert.Equal(expectedOutput, input.Kebaberize());
        }
    }

    internal class PluralTestSource : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "search", "searches" };
            yield return new object[] { "switch", "switches" };
            yield return new object[] { "fix", "fixes" };
            yield return new object[] { "box", "boxes" };
            yield return new object[] { "process", "processes" };
            yield return new object[] { "address", "addresses" };
            yield return new object[] { "case", "cases" };
            yield return new object[] { "stack", "stacks" };
            yield return new object[] { "wish", "wishes" };
            yield return new object[] { "fish", "fish" };

            yield return new object[] { "category", "categories" };
            yield return new object[] { "query", "queries" };
            yield return new object[] { "ability", "abilities" };
            yield return new object[] { "agency", "agencies" };
            yield return new object[] { "movie", "movies" };

            yield return new object[] { "archive", "archives" };

            yield return new object[] { "index", "indices" };

            yield return new object[] { "wife", "wives" };
            yield return new object[] { "safe", "saves" };
            yield return new object[] { "half", "halves" };

            yield return new object[] { "glove", "gloves" };
            yield return new object[] { "move", "moves" };

            yield return new object[] { "salesperson", "salespeople" };
            yield return new object[] { "person", "people" };

            yield return new object[] { "spokesman", "spokesmen" };
            yield return new object[] { "man", "men" };
            yield return new object[] { "woman", "women" };
            yield return new object[] { "freshman", "freshmen" };
            yield return new object[] { "chairman", "chairmen" };
            yield return new object[] { "human", "humans" };
            yield return new object[] { "personnel", "personnel" };
            yield return new object[] { "staff", "staff" };
            yield return new object[] { "training", "training" };

            yield return new object[] { "basis", "bases" };
            yield return new object[] { "diagnosis", "diagnoses" };

            yield return new object[] { "datum", "data" };
            yield return new object[] { "medium", "media" };
            yield return new object[] { "analysis", "analyses" };

            yield return new object[] { "node_child", "node_children" };
            yield return new object[] { "child", "children" };

            yield return new object[] { "experience", "experiences" };
            yield return new object[] { "day", "days" };

            yield return new object[] { "comment", "comments" };
            yield return new object[] { "foobar", "foobars" };
            yield return new object[] { "newsletter", "newsletters" };

            yield return new object[] { "old_news", "old_news" };
            yield return new object[] { "news", "news" };

            yield return new object[] { "series", "series" };
            yield return new object[] { "species", "species" };

            yield return new object[] { "quiz", "quizzes" };

            yield return new object[] { "perspective", "perspectives" };

            yield return new object[] { "ox", "oxen" };
            yield return new object[] { "photo", "photos" };
            yield return new object[] { "buffalo", "buffaloes" };
            yield return new object[] { "tomato", "tomatoes" };
            yield return new object[] { "dwarf", "dwarves" };
            yield return new object[] { "elf", "elves" };
            yield return new object[] { "information", "information" };
            yield return new object[] { "equipment", "equipment" };
            yield return new object[] { "bus", "buses" };
            yield return new object[] { "status", "statuses" };
            yield return new object[] { "status_code", "status_codes" };
            yield return new object[] { "mouse", "mice" };

            yield return new object[] { "louse", "lice" };
            yield return new object[] { "house", "houses" };
            yield return new object[] { "octopus", "octopi" };
            yield return new object[] { "alias", "aliases" };
            yield return new object[] { "portfolio", "portfolios" };
            yield return new object[] { "criterion", "criteria" };

            yield return new object[] { "vertex", "vertices" };
            yield return new object[] { "matrix", "matrices" };

            yield return new object[] { "axis", "axes" };
            yield return new object[] { "testis", "testes" };
            yield return new object[] { "crisis", "crises" };

            yield return new object[] { "corn", "corn" };
            yield return new object[] { "milk", "milk" };
            yield return new object[] { "rice", "rice" };
            yield return new object[] { "shoe", "shoes" };

            yield return new object[] { "horse", "horses" };
            yield return new object[] { "prize", "prizes" };
            yield return new object[] { "edge", "edges" };

            /* Tests added by Bas Jansen */
            yield return new object[] { "goose", "geese" };
            yield return new object[] { "deer", "deer" };
            yield return new object[] { "sheep", "sheep" };
            yield return new object[] { "wolf", "wolves" };
            yield return new object[] { "volcano", "volcanoes" };
            yield return new object[] { "aircraft", "aircraft" };
            yield return new object[] { "alumna", "alumnae" };
            yield return new object[] { "alumnus", "alumni" };
            yield return new object[] { "fungus", "fungi" };

            yield return new object[] { "water", "water" };
            yield return new object[] { "waters", "waters" };
            yield return new object[] { "semen", "semen" };
            yield return new object[] { "sperm", "sperm" };

            yield return new object[] { "wave", "waves" };

            yield return new object[] { "campus", "campuses" };

            yield return new object[] { "is", "are" };

            yield return new object[] { "addendum", "addenda" };
            yield return new object[] { "alga", "algae" };
            yield return new object[] { "apparatus", "apparatuses" };
            yield return new object[] { "appendix", "appendices" };
            yield return new object[] { "bias", "biases" };
            yield return new object[] { "bison", "bison" };
            yield return new object[] { "blitz", "blitzes" };
            yield return new object[] { "buzz", "buzzes" };
            yield return new object[] { "cactus", "cacti" };
            yield return new object[] { "corps", "corps" };
            yield return new object[] { "curriculum", "curricula" };
            yield return new object[] { "database", "databases" };
            yield return new object[] { "die", "dice" };
            yield return new object[] { "echo", "echoes" };
            yield return new object[] { "ellipsis", "ellipses" };
            yield return new object[] { "elk", "elk" };
            yield return new object[] { "emphasis", "emphases" };
            yield return new object[] { "embargo", "embargoes" };
            yield return new object[] { "focus", "foci" };
            yield return new object[] { "foot", "feet" };
            yield return new object[] { "fuse", "fuses" };
            yield return new object[] { "grass", "grass" };
            yield return new object[] { "hair", "hair" };
            yield return new object[] { "hero", "heroes" };
            yield return new object[] { "hippopotamus", "hippopotami" };
            yield return new object[] { "hoof", "hooves" };
            yield return new object[] { "iris", "irises" };
            yield return new object[] { "larva", "larvae" };
            yield return new object[] { "leaf", "leaves" };
            yield return new object[] { "loaf", "loaves" };
            yield return new object[] { "luggage", "luggage" };
            yield return new object[] { "means", "means" };
            yield return new object[] { "mail", "mail" };
            yield return new object[] { "millennium", "millennia" };
            yield return new object[] { "moose", "moose" };
            yield return new object[] { "mosquito", "mosquitoes" };
            yield return new object[] { "mud", "mud" };
            yield return new object[] { "nucleus", "nuclei" };
            yield return new object[] { "neurosis", "neuroses" };
            yield return new object[] { "oasis", "oases" };
            yield return new object[] { "offspring", "offspring" };
            yield return new object[] { "paralysis", "paralyses" };
            yield return new object[] { "phenomenon", "phenomena" };
            yield return new object[] { "potato", "potatoes" };
            yield return new object[] { "radius", "radii" };
            yield return new object[] { "salmon", "salmon" };
            yield return new object[] { "scissors", "scissors" };
            yield return new object[] { "shrimp", "shrimp" };
            yield return new object[] { "someone", "someone" };
            yield return new object[] { "stimulus", "stimuli" };
            yield return new object[] { "swine", "swine" };
            yield return new object[] { "syllabus", "syllabi" };
            yield return new object[] { "that", "those" };
            yield return new object[] { "thief", "thieves" };
            yield return new object[] { "this", "these" };
            yield return new object[] { "tie", "ties" };
            yield return new object[] { "tooth", "teeth" };
            yield return new object[] { "torpedo", "torpedoes" };
            yield return new object[] { "trellis", "trellises" };
            yield return new object[] { "trout", "trout" };
            yield return new object[] { "tuna", "tuna" };
            yield return new object[] { "vertebra", "vertebrae" };
            yield return new object[] { "veto", "vetoes" };
            yield return new object[] { "virus", "viruses" };
            yield return new object[] { "walrus", "walruses" };
            yield return new object[] { "waltz", "waltzes" };
            yield return new object[] { "zombie", "zombies" };

            yield return new object[] { "cookie", "cookies" };
            yield return new object[] { "bookie", "bookies" };
            yield return new object[] { "rookie", "rookies" };
            yield return new object[] { "roomie", "roomies" };
            yield return new object[] { "smoothie", "smoothies" };

            //Issue #789
            yield return new object[] { "cache", "caches" };

            //Issue #975, added by Alex Boutin
            yield return new object[] { "ex", "exes" };
            yield return new object[] { "", "" };

            //Issue #1100
            yield return new object[] { "doe", "does" };
            yield return new object[] { "hoe", "hoes" };
            yield return new object[] { "toe", "toes" };
            yield return new object[] { "woe", "woes" };

            //Issue #1132
            yield return new object[] { "metadata", "metadata" };

            //Issue #1154
            yield return new object[] { "a", "as" };
            yield return new object[] { "A", "As" };
            yield return new object[] { "s", "ss" };
            yield return new object[] { "S", "Ss" };
            yield return new object[] { "z", "zs" };
            yield return new object[] { "Z", "Zs" };
            yield return new object[] { "1", "1s" };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
