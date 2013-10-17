using System.Collections.Generic;
using Xunit;

namespace Humanizer.Tests
{
    public class InflectorTests 
    {
        public readonly Dictionary<string, string> TestData = new Dictionary<string, string>();

        [Fact]
        public void Pluralize()
        {
            foreach (var pair in TestData)
            {
                Assert.Equal(pair.Key.Pluralize(), pair.Value);
            }
        }

        [Fact]
        public void Singularize()
        {
            foreach (var pair in TestData)
            {
                Assert.Equal(pair.Value.Singularize(), pair.Key);
            }
        }

        public InflectorTests()
        {
            TestData.Add("search", "searches");
            TestData.Add("switch", "switches");
            TestData.Add("fix", "fixes");
            TestData.Add("box", "boxes");
            TestData.Add("process", "processes");
            TestData.Add("address", "addresses");
            TestData.Add("case", "cases");
            TestData.Add("stack", "stacks");
            TestData.Add("wish", "wishes");
            TestData.Add("fish", "fish");

            TestData.Add("category", "categories");
            TestData.Add("query", "queries");
            TestData.Add("ability", "abilities");
            TestData.Add("agency", "agencies");
            TestData.Add("movie", "movies");

            TestData.Add("archive", "archives");

            TestData.Add("index", "indices");

            TestData.Add("wife", "wives");
            TestData.Add("safe", "saves");
            TestData.Add("half", "halves");

            TestData.Add("move", "moves");

            TestData.Add("salesperson", "salespeople");
            TestData.Add("person", "people");

            TestData.Add("spokesman", "spokesmen");
            TestData.Add("man", "men");
            TestData.Add("woman", "women");

            TestData.Add("basis", "bases");
            TestData.Add("diagnosis", "diagnoses");

            TestData.Add("datum", "data");
            TestData.Add("medium", "media");
            TestData.Add("analysis", "analyses");

            TestData.Add("node_child", "node_children");
            TestData.Add("child", "children");

            TestData.Add("experience", "experiences");
            TestData.Add("day", "days");

            TestData.Add("comment", "comments");
            TestData.Add("foobar", "foobars");
            TestData.Add("newsletter", "newsletters");

            TestData.Add("old_news", "old_news");
            TestData.Add("news", "news");

            TestData.Add("series", "series");
            TestData.Add("species", "species");

            TestData.Add("quiz", "quizzes");

            TestData.Add("perspective", "perspectives");

            TestData.Add("ox", "oxen");
            TestData.Add("photo", "photos");
            TestData.Add("buffalo", "buffaloes");
            TestData.Add("tomato", "tomatoes");
            TestData.Add("dwarf", "dwarves");
            TestData.Add("elf", "elves");
            TestData.Add("information", "information");
            TestData.Add("equipment", "equipment");
            TestData.Add("bus", "buses");
            TestData.Add("status", "statuses");
            TestData.Add("status_code", "status_codes");
            TestData.Add("mouse", "mice");

            TestData.Add("louse", "lice");
            TestData.Add("house", "houses");
            TestData.Add("octopus", "octopi");
            TestData.Add("virus", "viri");
            TestData.Add("alias", "aliases");
            TestData.Add("portfolio", "portfolios");

            TestData.Add("vertex", "vertices");
            TestData.Add("matrix", "matrices");

            TestData.Add("axis", "axes");
            TestData.Add("testis", "testes");
            TestData.Add("crisis", "crises");

            TestData.Add("rice", "rice");
            TestData.Add("shoe", "shoes");

            TestData.Add("horse", "horses");
            TestData.Add("prize", "prizes");
            TestData.Add("edge", "edges");

            /* Tests added by Bas Jansen */
            TestData.Add("goose", "geese");
            TestData.Add("deer", "deer");
            TestData.Add("sheep", "sheep");
            TestData.Add("wolf", "wolves");
            TestData.Add("volcano", "volcanoes");
            TestData.Add("aircraft", "aircraft");
            TestData.Add("alumna", "alumnae");
            TestData.Add("alumnus", "alumni");
            TestData.Add("fungus", "fungi");
        }
    }
}
