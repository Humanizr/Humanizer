using System.Collections.Generic;
using Xunit;

namespace Humanizer.Tests.Localisation.sv
{
    [UseCulture("sv-SE")]
    public class CollectionFormatterTests
    {
        [Fact]
        public void MoreThanTwoItems()
        {
            var collection = new List<int>(new[] { 1, 2, 3 });
            var humanized = "1, 2 och 3";
            Assert.Equal(humanized, collection.Humanize());
        }

        [Fact]
        public void OneItem()
        {
            var collection = new List<int>(new[] { 1 });
            var humanized = "1";
            Assert.Equal(humanized, collection.Humanize());
        }

        [Fact]
        public void TwoItems()
        {
            var collection = new List<int>(new[] { 1, 2 });
            var humanized = "1 och 2";
            Assert.Equal(humanized, collection.Humanize());
        }
    }
}