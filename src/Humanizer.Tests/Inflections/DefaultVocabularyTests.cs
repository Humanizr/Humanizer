using System.Collections;
using System.Collections.Generic;
using Humanizer.Inflections;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Inflections
{
    public class DefaultVocabularyTests
    {
        [Theory]
        [ClassData(typeof(DefaultVocabularyTestSource))]
        public void Pluralize(string singular, string plural)
        {
            Assert.Equal(plural, Vocabularies.Default.Pluralize(singular));
        }

        [Theory]
        [ClassData(typeof(DefaultVocabularyTestSource))]
        public void PluralizeWordsWithUnknownPlurality(string singular, string plural)
        {
            Assert.Equal(plural, Vocabularies.Default.Pluralize(plural, false));
            Assert.Equal(plural, Vocabularies.Default.Pluralize(singular, false));
        }

        [Theory]
        [ClassData(typeof(DefaultVocabularyTestSource))]
        public void Singularize(string singular, string plural)
        {
            Assert.Equal(singular, Vocabularies.Default.Singularize(plural));
        }

        [Theory]
        [ClassData(typeof(DefaultVocabularyTestSource))]
        public void SingularizeWordsWithUnknownSingularity(string singular, string plural)
        {
            Assert.Equal(singular, Vocabularies.Default.Singularize(singular, false));
            Assert.Equal(singular, Vocabularies.Default.Singularize(plural, false));
        }
    }

    class DefaultVocabularyTestSource : IEnumerable<object[]>
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

            yield return new object[] { "move", "moves" };

            yield return new object[] { "salesperson", "salespeople" };
            yield return new object[] { "person", "people" };

            yield return new object[] { "spokesman", "spokesmen" };
            yield return new object[] { "man", "men" };
            yield return new object[] { "woman", "women" };

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
            yield return new object[] { "virus", "viri" };
            yield return new object[] { "alias", "aliases" };
            yield return new object[] { "portfolio", "portfolios" };
            yield return new object[] { "criterion", "criteria" };

            yield return new object[] { "vertex", "vertices" };
            yield return new object[] { "matrix", "matrices" };

            yield return new object[] { "axis", "axes" };
            yield return new object[] { "testis", "testes" };
            yield return new object[] { "crisis", "crises" };

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

            yield return new object[] { "wave", "waves" };

            yield return new object[] { "campus", "campuses" };

            yield return new object[] { "is", "are" };

            // Units of measurement:
            yield return new object[] { "oz", "oz" };
            yield return new object[] { "tsp", "tsp" };
            yield return new object[] { "ml", "ml" };
            yield return new object[] { "l", "l" };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}