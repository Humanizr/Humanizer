using System.Collections.Generic;
using Xunit;

namespace Humanizer.Tests
{
    public class SomeClass
    {
        public string SomeString;
        public int SomeInt;
        public override string ToString()
        {
            return "ToString";
        }
    }

    public class CollectionHumanizeTests : AmbientCulture
    {
        public CollectionHumanizeTests() : base("en") { }

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

        private readonly List<SomeClass> _testCollection = new List<SomeClass>
            {
                new SomeClass { SomeInt = 1, SomeString = "One" },
                new SomeClass { SomeInt = 2, SomeString = "Two" },
                new SomeClass { SomeInt = 3, SomeString = "Three" }
            };

        [Fact]
        public void HumanizeDefaultsToToString()
        {
            Assert.Equal("ToString, ToString, or ToString", _testCollection.Humanize("or"));
        }

        [Fact]
        public void HumanizeUsesObjectFormatter()
        {
            var humanized = _testCollection.Humanize(sc => string.Format("SomeObject #{0} - {1}", sc.SomeInt, sc.SomeString));
            Assert.Equal("SomeObject #1 - One, SomeObject #2 - Two, and SomeObject #3 - Three", humanized);
        }

        [Fact]
        public void HumanizeUsesObjectFormatterWhenSeparatorIsProvided()
        {
            var humanized = _testCollection.Humanize(sc => string.Format("SomeObject #{0} - {1}", sc.SomeInt, sc.SomeString), "or");
            Assert.Equal("SomeObject #1 - One, SomeObject #2 - Two, or SomeObject #3 - Three", humanized);
        }
    }
}
