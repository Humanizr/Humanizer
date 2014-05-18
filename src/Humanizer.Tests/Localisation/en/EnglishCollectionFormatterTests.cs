using System.Collections.Generic;
using Xunit;

namespace Humanizer.Tests.Localisation.en
{
    public class EnglishCollectionFormatterTests : AmbientCulture
    {
        public EnglishCollectionFormatterTests() : base("en") { }

        [Fact]
        public void HumanizeReturnsOnlyNameWhenCollectionContainsOneItem()
        {
            var collection = new List<string> { "A String" };

            Assert.Equal("A String", collection.Humanize());
        }

        [Fact]
        public void HumanizeUsesSeparatorWhenMoreThanOneItemIsInCollection()
        {
            var collection = new List<string>
            {
                "A String",
                "Another String",
            };

            Assert.Equal("A String or Another String", collection.Humanize("or"));
        }

        [Fact]
        public void HumanizeDefaultsSeparatorToAnd()
        {
            var collection = new List<string>
            {
                "A String",
                "Another String",
            };

            Assert.Equal("A String and Another String", collection.Humanize());
        }

        [Fact]
        public void HumanizeUsesOxfordComma()
        {
            var collection = new List<string>
            {
                "A String",
                "Another String",
                "A Third String",
            };

            Assert.Equal("A String, Another String, or A Third String", collection.Humanize("or"));
        }
    }
}
