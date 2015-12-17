using System;
using Xunit;

namespace Humanizer.Tests
{
    public class DateToOrdinalWordsTests
    {
        [Fact]
        public void CanToOrdinalWords()
        {
            var date = new DateTime(2015, 1, 1);

            Assert.Equal("1st January 2015", date.ToOrdinalWords());
        }

        [UseCulture("pt-BR")]
        [Fact]
        public void CanToOrdinalWordsIncludingGender()
        {
            var date = new DateTime(2015, 1, 1);

            Assert.Equal("1º janeiro 2015", date.ToOrdinalWords(GrammaticalGender.Masculine));
        }
    }
}
