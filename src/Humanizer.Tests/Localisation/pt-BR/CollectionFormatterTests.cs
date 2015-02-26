using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.ptBR
{
    public class CollectionFormatterTests : AmbientCulture
    {
        public CollectionFormatterTests()
            : base("pt-BR")
        {
        }

        [Fact]
        public void OneItem()
        {
            var collection = new List<int>(new int[] { 1 });
            string humanized = "1";
            Assert.Equal(humanized, collection.Humanize());
        }

        [Fact]
        public void TwoItems()
        {
            var collection = new List<int>(new int[] { 1, 2 });
            string humanized = "1 e 2";
            Assert.Equal(humanized, collection.Humanize());
        }

        [Fact]
        public void MoreThanTwoItems()
        {
            var collection = new List<int>(new int[] { 1, 2, 3 });
            string humanized = "1, 2 e 3";
            Assert.Equal(humanized, collection.Humanize());
        }

        [Fact]
        public void MoreThanTwoItemsWithCustomSeparator()
        {
            var collection = new List<int>(new int[] { 1, 2, 3 });
            string humanized = "1, 2 ou 3";
            Assert.Equal(humanized, collection.Humanize("ou"));
        }
    }
}