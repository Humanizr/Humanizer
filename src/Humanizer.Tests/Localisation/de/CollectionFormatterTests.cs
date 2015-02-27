using System.Collections.Generic;
using Xunit;

namespace Humanizer.Tests.Localisation.de
{
    public class CollectionFormatterTests : AmbientCulture
    {
        public CollectionFormatterTests() 
            : base("de") 
        {
        }

        [Fact]
        public void OneItem()
        {
            var collection = new List<int>(new int[] {1});
            string humanized = "1";
            Assert.Equal(humanized, collection.Humanize());
        }

        [Fact]
        public void TwoItems()
        {
            var collection = new List<int>(new int[] {1, 2});
            string humanized = "1 und 2";
            Assert.Equal(humanized, collection.Humanize());
        }

        [Fact]
        public void MoreThanTwoItems()
        {
            var collection = new List<int>(new int[] {1, 2, 3});
            string humanized = "1, 2 und 3";
            Assert.Equal(humanized, collection.Humanize());
        }
    }
}