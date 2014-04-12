using Xunit;
using Xunit.Extensions;

namespace Humanizer.Tests.Localisation.he
{
    public class NumberToWordsTests : AmbientCulture
    {
        public NumberToWordsTests() : base("he") { }

        [Theory]
        [InlineData(0, "אפס")]
        [InlineData(1, "אחת")]
        [InlineData(2, "שתיים")]
        [InlineData(3, "שלוש")]
        [InlineData(4, "ארבע")]
        [InlineData(5, "חמש")]
        [InlineData(6, "שש")]
        [InlineData(7, "שבע")]
        [InlineData(8, "שמונה")]
        [InlineData(9, "תשע")]
        [InlineData(10, "עשר")]
        [InlineData(11, "אחת עשרה")]
        [InlineData(12, "שתים עשרה")]
        [InlineData(19, "תשע עשרה")]
        [InlineData(20, "עשרים")]
        [InlineData(22, "עשרים ושתיים")]
        [InlineData(50, "חמישים")]
        [InlineData(99, "תשעים ותשע")]
        [InlineData(100, "מאה")]
        [InlineData(101, "מאה ואחת")]
        [InlineData(111, "מאה ואחת עשרה")]
        [InlineData(200, "מאתיים")]
        [InlineData(241, "מאתיים ארבעים ואחת")]
        [InlineData(500, "חמש מאות")]
        [InlineData(505, "חמש מאות וחמש")]
        [InlineData(725, "שבע מאות עשרים וחמש")]
        [InlineData(1000, "אלף")]
        [InlineData(1009, "אלף ותשע")]
        [InlineData(1011, "אלף ואחת עשרה")]
        [InlineData(1024, "אלף עשרים וארבע")]
        [InlineData(1040, "אלף ארבעים")]
        [InlineData(2000, "אלפיים")]
        [InlineData(7021, "שבעת אלפים עשרים ואחת")]
        [InlineData(20000, "עשרים אלף")]
        [InlineData(28123, "עשרים ושמונה אלף מאה עשרים ושלוש")]
        [InlineData(500000, "חמש מאות אלף")]
        [InlineData(500001, "חמש מאות אלף ואחת")]
        [InlineData(1000000, "מיליון")]
        [InlineData(1000001, "מיליון ואחת")]
        [InlineData(2000408, "שני מיליון ארבע מאות ושמונה")]
        [InlineData(1000000000, "מיליארד")]
        [InlineData(1000000001, "מיליארד ואחת")]
        [InlineData(int.MaxValue /* 2147483647 */, "שני מיליארד מאה ארבעים ושבעה מיליון ארבע מאות שמונים ושלוש אלף שש מאות ארבעים ושבע")]
        public void ToWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData(-2, "מינוס שתיים")]
        public void NegativeToWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }
    }
}