using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace Humanizer.Tests.Localisation.el
{
  
        [UseCulture("el")]
        public class NumberToOrdinalWordsTests
        {
        [InlineData(-1, "")]
        [InlineData(0, "")]
        [InlineData(1, "πρώτος")]
        [InlineData(10, "δέκατος")]
        [InlineData(11, "ενδέκατος")]
        [InlineData(12, "δωδέκατος")]
        [InlineData(20, "εικοστός")]
        [InlineData(31, "τριακοστός πρώτος")]
        [InlineData(100, "εκατοστός")]
        [InlineData(105, "εκατοστός πέμπτος")]
        [InlineData(286, "διακοσιοστός ογδοηκοστός έκτος")]
        [InlineData(530, "πεντακοσιοστός τριακοστός")]
        [InlineData(912, "εννιακοσιοστός δωδέκατος")]
        [InlineData(1203, "χιλιοστός διακοσιοστός τρίτος")]
        [InlineData(1596, "χιλιοστός πεντακοσιοστός ενενηκοστός έκτος")]
        [InlineData(1061, "χιλιοστός εξηκοστός πρώτος")]
        [InlineData(1008, "χιλιοστός όγδοος")]
        [InlineData(1211, "χιλιοστός διακοσιοστός ενδέκατος")]
        [InlineData(1999, "χιλιοστός εννιακοσιοστός ενενηκοστός ένατος")]
        [InlineData(2000, "")]

        [Theory]
        public void ToOrdinalWordsInt(int number, string expected)
            {
                Assert.Equal(expected, number.ToOrdinalWords());
            }
    }
}
