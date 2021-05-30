using System;
using Xunit;

namespace Humanizer.Tests.Localisation.hy
{
    [UseCulture("hy")]
    public class TimeSpanHumanizeTests
    {

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(366, "մեկ տարի")]
        [InlineData(731, "2 տարի")]
        [InlineData(1096, "3 տարի")]
        [InlineData(4018, "11 տարի")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Native speaker")]
        [InlineData(31, "մեկ ամիս")]
        [InlineData(61, "2 ամիս")]
        [InlineData(92, "3 ամիս")]
        [InlineData(335, "11 ամիս")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [InlineData(7, "մեկ շաբաթ")]
        [InlineData(14, "2 շաբաթ")]
        [InlineData(21, "3 շաբաթ")]
        [InlineData(28, "4 շաբաթ")]
        [InlineData(35, "5 շաբաթ")]
        [InlineData(77, "11 շաբաթ")]
        public void Weeks(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "մեկ օր")]
        [InlineData(2, "2 օր")]
        [InlineData(3, "3 օր")]
        [InlineData(4, "4 օր")]
        [InlineData(5, "5 օր")]
        [InlineData(6, "6 օր")]
        public void Days(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize());
        }

        [Theory]
        [InlineData(1, "մեկ ժամ")]
        [InlineData(2, "2 ժամ")]
        [InlineData(3, "3 ժամ")]
        [InlineData(4, "4 ժամ")]
        [InlineData(5, "5 ժամ")]
        [InlineData(6, "6 ժամ")]
        [InlineData(10, "10 ժամ")]
        [InlineData(11, "11 ժամ")]
        [InlineData(19, "19 ժամ")]
        [InlineData(20, "20 ժամ")]
        [InlineData(21, "21 ժամ")]
        [InlineData(22, "22 ժամ")]
        [InlineData(23, "23 ժամ")]
        public void Hours(int hours, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromHours(hours).Humanize());
        }

        [Theory]
        [InlineData(1, "մեկ րոպե")]
        [InlineData(2, "2 րոպե")]
        [InlineData(3, "3 րոպե")]
        [InlineData(4, "4 րոպե")]
        [InlineData(5, "5 րոպե")]
        [InlineData(6, "6 րոպե")]
        [InlineData(10, "10 րոպե")]
        [InlineData(11, "11 րոպե")]
        [InlineData(19, "19 րոպե")]
        [InlineData(20, "20 րոպե")]
        [InlineData(21, "21 րոպե")]
        [InlineData(22, "22 րոպե")]
        [InlineData(23, "23 րոպե")]
        [InlineData(24, "24 րոպե")]
        [InlineData(25, "25 րոպե")]
        [InlineData(40, "40 րոպե")]
        public void Minutes(int minutes, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMinutes(minutes).Humanize());
        }

        [Theory]
        [InlineData(1, "մեկ վայրկյան")]
        [InlineData(2, "2 վայրկյան")]
        [InlineData(3, "3 վայրկյան")]
        [InlineData(4, "4 վայրկյան")]
        [InlineData(5, "5 վայրկյան")]
        [InlineData(6, "6 վայրկյան")]
        [InlineData(10, "10 վայրկյան")]
        [InlineData(11, "11 վայրկյան")]
        [InlineData(19, "19 վայրկյան")]
        [InlineData(20, "20 վայրկյան")]
        [InlineData(21, "21 վայրկյան")]
        [InlineData(22, "22 վայրկյան")]
        [InlineData(23, "23 վայրկյան")]
        [InlineData(24, "24 վայրկյան")]
        [InlineData(25, "25 վայրկյան")]
        [InlineData(40, "40 վայրկյան")]
        public void Seconds(int seconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize());
        }

        [Theory]
        [InlineData(1, "մեկ միլիվայրկյան")]
        [InlineData(2, "2 միլիվայրկյան")]
        [InlineData(3, "3 միլիվայրկյան")]
        [InlineData(4, "4 միլիվայրկյան")]
        [InlineData(5, "5 միլիվայրկյան")]
        [InlineData(6, "6 միլիվայրկյան")]
        [InlineData(10, "10 միլիվայրկյան")]
        [InlineData(11, "11 միլիվայրկյան")]
        [InlineData(19, "19 միլիվայրկյան")]
        [InlineData(20, "20 միլիվայրկյան")]
        [InlineData(21, "21 միլիվայրկյան")]
        [InlineData(22, "22 միլիվայրկյան")]
        [InlineData(23, "23 միլիվայրկյան")]
        [InlineData(24, "24 միլիվայրկյան")]
        [InlineData(25, "25 միլիվայրկյան")]
        [InlineData(40, "40 միլիվայրկյան")]
        public void Milliseconds(int milliseconds, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromMilliseconds(milliseconds).Humanize());
        }

        [Fact]
        public void NoTime()
        {
            Assert.Equal("0 միլիվայրկյան", TimeSpan.Zero.Humanize());
        }

        [Fact]
        public void NoTimeToWords()
        {
            // This one doesn't make a lot of sense but ... w/e
            Assert.Equal("ժամանակը բացակայում է", TimeSpan.Zero.Humanize(toWords: true));
        }
    }
}
