using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.it
{
    public class OrdinalizerTests : AmbientCulture
    {
        public OrdinalizerTests() : base("it") { }

        [Theory]
        [InlineData(0, "0")]  // No ordinal for 0 in italian (neologism apart)
        [InlineData(1, "1°")]
        [InlineData(11, "11°")]
        [InlineData(111, "111°")]
        public void Genderless(int number, string expected)
        {
            Assert.Equal(expected, number.Ordinalize());
        }

        [Theory]
        [InlineData(0, "0")]  // No ordinal for 0 in italian (neologism apart)
        [InlineData(1, "1°")]
        [InlineData(11, "11°")]
        [InlineData(111, "111°")]
        public void Masculine(int number, string expected)
        {
            Assert.Equal(expected, number.Ordinalize(GrammaticalGender.Masculine));
        }

        [Theory]
        [InlineData(0, "0")]  // No ordinal for 0 in italian (neologism apart)
        [InlineData(1, "1ª")]
        [InlineData(11, "11ª")]
        [InlineData(111, "111ª")]
        public void Feminine(int number, string expected)
        {
            Assert.Equal(expected, number.Ordinalize(GrammaticalGender.Feminine));
        }
    }
}