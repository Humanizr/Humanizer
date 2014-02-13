using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Humanizer.Pluralization;
using Xunit;
namespace Humanizer.Pluralization.Tests
{
    public class EnglishPluralizationServiceTests
    {
        EnglishPluralizationService x = new EnglishPluralizationService();

        [Fact]
        public void PluralizeTest()
        {
            Assert.Equal(x.Pluralize("dog"), "dogs");
            Assert.Equal(x.Pluralize("gift"), "gifts");
            Assert.Equal(x.Pluralize("game"), "games");
            Assert.Equal(x.Pluralize("developer"), "developers");
        }

        [Fact]
        public void SingularizeTest()
        {
            Assert.Equal(x.Singularize("players"), "player");
            Assert.Equal(x.Singularize("gamers"), "gamer");
            Assert.Equal(x.Singularize("makers"), "maker");
            Assert.Equal(x.Singularize("programmers"), "programmer");
        }
    }
}
