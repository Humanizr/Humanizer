using System;
using System.Globalization;
using System.Linq;

using Humanizer.Localisation;

using Xunit;

namespace Humanizer.Tests.Localisation.es
{
    [UseCulture("es-ES")]
    public class TimeSpanHumanizeTests
    {
        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(366, "1 año")]
        [InlineData(731, "2 años")]
        [InlineData(1096, "3 años")]
        [InlineData(4018, "11 años")]
        public void Years(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Theory]
        [Trait("Translation", "Google")]
        [InlineData(31, "1 mes")]
        [InlineData(61, "2 meses")]
        [InlineData(92, "3 meses")]
        [InlineData(335, "11 meses")]
        public void Months(int days, string expected)
        {
            Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year));
        }

        [Fact]
        public void TwoWeeks()
        {
            Assert.Equal("2 semanas", TimeSpan.FromDays(14).Humanize());
        }

        [Fact]
        public void OneWeek()
        {
            Assert.Equal("1 semana", TimeSpan.FromDays(7).Humanize());
        }

        [Fact]
        public void SixDays()
        {
            Assert.Equal("6 días", TimeSpan.FromDays(6).Humanize());
        }

        [Fact]
        public void TwoDays()
        {
            Assert.Equal("2 días", TimeSpan.FromDays(2).Humanize());
        }

        [Fact]
        public void OneDay()
        {
            Assert.Equal("1 día", TimeSpan.FromDays(1).Humanize());
        }

        [Fact]
        public void TwoHours()
        {
            Assert.Equal("2 horas", TimeSpan.FromHours(2).Humanize());
        }

        [Fact]
        public void OneHour()
        {
            Assert.Equal("1 hora", TimeSpan.FromHours(1).Humanize());
        }

        [Fact]
        public void TwoMinutes()
        {
            Assert.Equal("2 minutos", TimeSpan.FromMinutes(2).Humanize());
        }

        [Fact]
        public void OneMinute()
        {
            Assert.Equal("1 minuto", TimeSpan.FromMinutes(1).Humanize());
        }

        [Fact]
        public void TwoSeconds()
        {
            Assert.Equal("2 segundos", TimeSpan.FromSeconds(2).Humanize());
        }

        [Fact]
        public void OneSecond()
        {
            Assert.Equal("1 segundo", TimeSpan.FromSeconds(1).Humanize());
        }

        [Fact]
        public void TwoMilliseconds()
        {
            Assert.Equal("2 milisegundos", TimeSpan.FromMilliseconds(2).Humanize());
        }

        [Fact]
        public void OneMillisecond()
        {
            Assert.Equal("1 milisegundo", TimeSpan.FromMilliseconds(1).Humanize());
        }

        [Theory]
        [InlineData(0, 0, 1, 1, 2, "un minuto, un segundo")]
        [InlineData(0, 0, 2, 2, 2, "dos minutos, dos segundos")]
        [InlineData(1, 2, 3, 4, 4, "un día, dos horas, tres minutos, cuatro segundos")]
        public void ComplexTimeSpan(int days, int hours, int minutes, int seconds, int precision, string expected)
        {
            var timeSpan = new TimeSpan(days, hours, minutes, seconds);
            Assert.Equal(expected, timeSpan.Humanize(precision, toWords: true));
        }

        [Fact]
        public void NoTime()
        {
            // This one doesn't make a lot of sense but ... w/e
            Assert.Equal("0 milisegundos", TimeSpan.Zero.Humanize());
        }

        [Fact]
        public void NoTimeToWords()
        {
            // This one doesn't make a lot of sense but ... w/e
            Assert.Equal("nada", TimeSpan.Zero.Humanize(toWords: true));
        }

        [Fact]
        public void AllTimeSpansMustBeUniqueForASequenceOfDays()
        {
            var culture = new CultureInfo("es-ES");
            var qry = from i in Enumerable.Range(0, 100000)
                      let ts = TimeSpan.FromDays(i)
                      let text = ts.Humanize(precision: 3, culture: culture, maxUnit: TimeUnit.Year)
                      select text;
            var grouping = from t in qry
                           group t by t into g
                           select new { g.Key, Count = g.Count() };
            var allUnique = grouping.All(g => g.Count == 1);
            Assert.True(allUnique);
        }

        [Theory]
        [InlineData(365, "11 meses, 30 días")]
        [InlineData(365 + 1, "1 año")]
        [InlineData(365 + 365, "1 año, 11 meses, 29 días")]
        [InlineData(365 + 365 + 1, "2 años")]
        [InlineData(365 + 365 + 365, "2 años, 11 meses, 29 días")]
        [InlineData(365 + 365 + 365 + 1, "3 años")]
        [InlineData(365 + 365 + 365 + 365, "3 años, 11 meses, 29 días")]
        [InlineData(365 + 365 + 365 + 365 + 1, "4 años")]
        [InlineData(365 + 365 + 365 + 365 + 366, "4 años, 11 meses, 30 días")]
        [InlineData(365 + 365 + 365 + 365 + 366 + 1, "5 años")]
        public void Year(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize(precision: 7, maxUnit: TimeUnit.Year);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(30, "4 semanas, 2 días")]
        [InlineData(30 + 1, "1 mes")]
        [InlineData(30 + 30, "1 mes, 29 días")]
        [InlineData(30 + 30 + 1, "2 meses")]
        [InlineData(30 + 30 + 31, "2 meses, 30 días")]
        [InlineData(30 + 30 + 31 + 1, "3 meses")]
        [InlineData(30 + 30 + 31 + 30, "3 meses, 29 días")]
        [InlineData(30 + 30 + 31 + 30 + 1, "4 meses")]
        [InlineData(30 + 30 + 31 + 30 + 31, "4 meses, 30 días")]
        [InlineData(30 + 30 + 31 + 30 + 31 + 1, "5 meses")]
        [InlineData(365, "11 meses, 30 días")]
        [InlineData(366, "1 año")]
        public void Month(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize(precision: 7, maxUnit: TimeUnit.Year);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(14, "2 semanas")]
        [InlineData(7, "1 semana")]
        [InlineData(-14, "2 semanas")]
        [InlineData(-7, "1 semana")]
        [InlineData(730, "104 semanas")]
        public void Weeks(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(6, "6 días")]
        [InlineData(2, "2 días")]
        [InlineData(1, "1 día")]
        [InlineData(-6, "6 días")]
        [InlineData(-2, "2 días")]
        [InlineData(-1, "1 día")]
        public void Days(int days, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 horas")]
        [InlineData(1, "1 hora")]
        [InlineData(-2, "2 horas")]
        [InlineData(-1, "1 hora")]
        public void Hours(int hours, string expected)
        {
            var actual = TimeSpan.FromHours(hours).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, "2 minutos")]
        [InlineData(1, "1 minuto")]
        [InlineData(-2, "2 minutos")]
        [InlineData(-1, "1 minuto")]
        public void Minutes(int minutes, string expected)
        {
            var actual = TimeSpan.FromMinutes(minutes).Humanize();
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(135, "2 minutos")]
        [InlineData(60, "1 minuto")]
        [InlineData(2, "2 segundos")]
        [InlineData(1, "1 segundo")]
        [InlineData(-135, "2 minutos")]
        [InlineData(-60, "1 minuto")]
        [InlineData(-2, "2 segundos")]
        [InlineData(-1, "1 segundo")]
        public void Seconds(int seconds, string expected)
        {
            var actual = TimeSpan.FromSeconds(seconds).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2500, "2 segundos")]
        [InlineData(1400, "1 segundo")]
        [InlineData(2, "2 milisegundos")]
        [InlineData(1, "1 milisegundo")]
        [InlineData(-2500, "2 segundos")]
        [InlineData(-1400, "1 segundo")]
        [InlineData(-2, "2 milisegundos")]
        [InlineData(-1, "1 milisegundo")]
        public void Milliseconds(int ms, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData((long)366 * 24 * 60 * 60 * 1000, "12 meses", TimeUnit.Month)]
        [InlineData((long)6 * 7 * 24 * 60 * 60 * 1000, "6 semanas", TimeUnit.Week)]
        [InlineData(7 * 24 * 60 * 60 * 1000, "7 días", TimeUnit.Day)]
        [InlineData(24 * 60 * 60 * 1000, "24 horas", TimeUnit.Hour)]
        [InlineData(60 * 60 * 1000, "60 minutos", TimeUnit.Minute)]
        [InlineData(60 * 1000, "60 segundos", TimeUnit.Second)]
        [InlineData(1000, "1000 milisegundos", TimeUnit.Millisecond)]
        public void TimeSpanWithMaxTimeUnit(long ms, string expected, TimeUnit maxUnit)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize(maxUnit: maxUnit);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(10, "10 milisegundos", TimeUnit.Millisecond)]
        [InlineData(10, "nada", TimeUnit.Second, true)]
        [InlineData(10, "nada", TimeUnit.Minute, true)]
        [InlineData(10, "nada", TimeUnit.Hour, true)]
        [InlineData(10, "nada", TimeUnit.Day, true)]
        [InlineData(10, "nada", TimeUnit.Week, true)]
        [InlineData(10, "0 segundos", TimeUnit.Second)]
        [InlineData(10, "0 minutos", TimeUnit.Minute)]
        [InlineData(10, "0 horas", TimeUnit.Hour)]
        [InlineData(10, "0 días", TimeUnit.Day)]
        [InlineData(10, "0 semanas", TimeUnit.Week)]
        [InlineData(2500, "2 segundos, 500 milisegundos", TimeUnit.Millisecond)]
        [InlineData(2500, "2 segundos", TimeUnit.Second)]
        [InlineData(2500, "nada", TimeUnit.Minute, true)]
        [InlineData(2500, "nada", TimeUnit.Hour, true)]
        [InlineData(2500, "nada", TimeUnit.Day, true)]
        [InlineData(2500, "nada", TimeUnit.Week, true)]
        [InlineData(2500, "0 minutos", TimeUnit.Minute)]
        [InlineData(2500, "0 horas", TimeUnit.Hour)]
        [InlineData(2500, "0 días", TimeUnit.Day)]
        [InlineData(2500, "0 semanas", TimeUnit.Week)]
        [InlineData(122500, "2 minutos, 2 segundos, 500 milisegundos", TimeUnit.Millisecond)]
        [InlineData(122500, "2 minutos, 2 segundos", TimeUnit.Second)]
        [InlineData(122500, "2 minutos", TimeUnit.Minute)]
        [InlineData(122500, "nada", TimeUnit.Hour, true)]
        [InlineData(122500, "nada", TimeUnit.Day, true)]
        [InlineData(122500, "nada", TimeUnit.Week, true)]
        [InlineData(122500, "0 horas", TimeUnit.Hour)]
        [InlineData(122500, "0 días", TimeUnit.Day)]
        [InlineData(122500, "0 semanas", TimeUnit.Week)]
        [InlineData(3722500, "1 hora, 2 minutos, 2 segundos, 500 milisegundos", TimeUnit.Millisecond)]
        [InlineData(3722500, "1 hora, 2 minutos, 2 segundos", TimeUnit.Second)]
        [InlineData(3722500, "1 hora, 2 minutos", TimeUnit.Minute)]
        [InlineData(3722500, "1 hora", TimeUnit.Hour)]
        [InlineData(3722500, "nada", TimeUnit.Day, true)]
        [InlineData(3722500, "nada", TimeUnit.Week, true)]
        [InlineData(3722500, "0 días", TimeUnit.Day)]
        [InlineData(3722500, "0 semanas", TimeUnit.Week)]
        [InlineData(90122500, "1 día, 1 hora, 2 minutos, 2 segundos, 500 milisegundos", TimeUnit.Millisecond)]
        [InlineData(90122500, "1 día, 1 hora, 2 minutos, 2 segundos", TimeUnit.Second)]
        [InlineData(90122500, "1 día, 1 hora, 2 minutos", TimeUnit.Minute)]
        [InlineData(90122500, "1 día, 1 hora", TimeUnit.Hour)]
        [InlineData(90122500, "1 día", TimeUnit.Day)]
        [InlineData(90122500, "nada", TimeUnit.Week, true)]
        [InlineData(90122500, "0 semanas", TimeUnit.Week)]
        [InlineData(694922500, "1 semana, 1 día, 1 hora, 2 minutos, 2 segundos, 500 milisegundos", TimeUnit.Millisecond)]
        [InlineData(694922500, "1 semana, 1 día, 1 hora, 2 minutos, 2 segundos", TimeUnit.Second)]
        [InlineData(694922500, "1 semana, 1 día, 1 hora, 2 minutos", TimeUnit.Minute)]
        [InlineData(694922500, "1 semana, 1 día, 1 hora", TimeUnit.Hour)]
        [InlineData(694922500, "1 semana, 1 día", TimeUnit.Day)]
        [InlineData(694922500, "1 semana", TimeUnit.Week)]
        [InlineData(2768462500, "1 mes, 1 día, 1 hora, 1 minuto, 2 segundos, 500 milisegundos", TimeUnit.Millisecond)]
        [InlineData(2768462500, "1 mes, 1 día, 1 hora, 1 minuto, 2 segundos", TimeUnit.Second)]
        [InlineData(2768462500, "1 mes, 1 día, 1 hora, 1 minuto", TimeUnit.Minute)]
        [InlineData(2768462500, "1 mes, 1 día, 1 hora", TimeUnit.Hour)]
        [InlineData(2768462500, "1 mes, 1 día", TimeUnit.Day)]
        [InlineData(2768462500, "1 mes", TimeUnit.Week)]
        [InlineData(2768462500, "1 mes", TimeUnit.Month)]
        [InlineData(2768462500, "nada", TimeUnit.Year, true)]
        [InlineData(2768462500, "0 años", TimeUnit.Year)]
        [InlineData(34390862500, "1 año, 1 mes, 2 días, 1 hora, 1 minuto, 2 segundos, 500 milisegundos", TimeUnit.Millisecond)]
        [InlineData(34390862500, "1 año, 1 mes, 2 días, 1 hora, 1 minuto, 2 segundos", TimeUnit.Second)]
        [InlineData(34390862500, "1 año, 1 mes, 2 días, 1 hora, 1 minuto", TimeUnit.Minute)]
        [InlineData(34390862500, "1 año, 1 mes, 2 días, 1 hora", TimeUnit.Hour)]
        [InlineData(34390862500, "1 año, 1 mes, 2 días", TimeUnit.Day)]
        [InlineData(34390862500, "1 año, 1 mes", TimeUnit.Week)]
        [InlineData(34390862500, "1 año, 1 mes", TimeUnit.Month)]
        [InlineData(34390862500, "1 año", TimeUnit.Year)]
        public void TimeSpanWithMinTimeUnit(long ms, string expected, TimeUnit minUnit, bool toWords = false)
        {
            var actual = TimeSpan.FromMilliseconds(ms).Humanize(minUnit: minUnit, precision: 7, maxUnit: TimeUnit.Year, toWords: toWords);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 3, "nada", true)]
        [InlineData(0, 2, "nada", true)]
        [InlineData(0, 3, "0 milisegundos")]
        [InlineData(0, 2, "0 milisegundos")]
        [InlineData(10, 2, "10 milisegundos")]
        [InlineData(1400, 2, "1 segundo, 400 milisegundos")]
        [InlineData(2500, 2, "2 segundos, 500 milisegundos")]
        [InlineData(120000, 2, "2 minutos")]
        [InlineData(62000, 2, "1 minuto, 2 segundos")]
        [InlineData(62020, 2, "1 minuto, 2 segundos")]
        [InlineData(62020, 3, "1 minuto, 2 segundos, 20 milisegundos")]
        [InlineData(3600020, 4, "1 hora, 20 milisegundos")]
        [InlineData(3600020, 3, "1 hora, 20 milisegundos")]
        [InlineData(3600020, 2, "1 hora, 20 milisegundos")]
        [InlineData(3600020, 1, "1 hora")]
        [InlineData(3603001, 2, "1 hora, 3 segundos")]
        [InlineData(3603001, 3, "1 hora, 3 segundos, 1 milisegundo")]
        [InlineData(86400000, 3, "1 día")]
        [InlineData(86400000, 2, "1 día")]
        [InlineData(86400000, 1, "1 día")]
        [InlineData(86401000, 1, "1 día")]
        [InlineData(86401000, 2, "1 día, 1 segundo")]
        [InlineData(86401200, 2, "1 día, 1 segundo")]
        [InlineData(86401200, 3, "1 día, 1 segundo, 200 milisegundos")]
        [InlineData(1296000000, 1, "2 semanas")]
        [InlineData(1296000000, 2, "2 semanas, 1 día")]
        [InlineData(1299600000, 2, "2 semanas, 1 día")]
        [InlineData(1299600000, 3, "2 semanas, 1 día, 1 hora")]
        [InlineData(1299630020, 3, "2 semanas, 1 día, 1 hora")]
        [InlineData(1299630020, 4, "2 semanas, 1 día, 1 hora, 30 segundos")]
        [InlineData(1299630020, 5, "2 semanas, 1 día, 1 hora, 30 segundos, 20 milisegundos")]
        [InlineData(2768462500, 6, "1 mes, 1 día, 1 hora, 1 minuto, 2 segundos, 500 milisegundos")]
        [InlineData(2768462500, 5, "1 mes, 1 día, 1 hora, 1 minuto, 2 segundos")]
        [InlineData(2768462500, 4, "1 mes, 1 día, 1 hora, 1 minuto")]
        [InlineData(2768462500, 3, "1 mes, 1 día, 1 hora")]
        [InlineData(2768462500, 2, "1 mes, 1 día")]
        [InlineData(2768462500, 1, "1 mes")]
        [InlineData(34390862500, 7, "1 año, 1 mes, 2 días, 1 hora, 1 minuto, 2 segundos, 500 milisegundos")]
        [InlineData(34390862500, 6, "1 año, 1 mes, 2 días, 1 hora, 1 minuto, 2 segundos")]
        [InlineData(34390862500, 5, "1 año, 1 mes, 2 días, 1 hora, 1 minuto")]
        [InlineData(34390862500, 4, "1 año, 1 mes, 2 días, 1 hora")]
        [InlineData(34390862500, 3, "1 año, 1 mes, 2 días")]
        [InlineData(34390862500, 2, "1 año, 1 mes")]
        [InlineData(34390862500, 1, "1 año")]
        public void TimeSpanWithPrecision(long milliseconds, int precision, string expected, bool toWords = false)
        {
            var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize(precision, maxUnit: TimeUnit.Year, toWords: toWords);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(3 * 7 + 4, 2, "3 semanas, 4 días")]
        [InlineData(6 * 7 + 3, 2, "6 semanas, 3 días")]
        [InlineData(72 * 7 + 6, 2, "72 semanas, 6 días")]
        public void DaysWithPrecision(int days, int precision, string expected)
        {
            var actual = TimeSpan.FromDays(days).Humanize(precision: precision);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(52)]
        public void TimeSpanWithMinAndMaxUnits_DoesNotReportExcessiveTime(int minutes)
        {
            var actual = TimeSpan.FromMinutes(minutes).Humanize(2, null, TimeUnit.Hour, TimeUnit.Minute);
            var expected = TimeSpan.FromMinutes(minutes).Humanize(2);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 3, "nada", true)]
        [InlineData(0, 2, "nada", true)]
        [InlineData(0, 3, "0 milisegundos")]
        [InlineData(0, 2, "0 milisegundos")]
        [InlineData(10, 2, "10 milisegundos")]
        [InlineData(1400, 2, "1 segundo, 400 milisegundos")]
        [InlineData(2500, 2, "2 segundos, 500 milisegundos")]
        [InlineData(60001, 1, "1 minuto")]
        [InlineData(60001, 2, "1 minuto")]
        [InlineData(60001, 3, "1 minuto, 1 milisegundo")]
        [InlineData(120000, 2, "2 minutos")]
        [InlineData(62000, 2, "1 minuto, 2 segundos")]
        [InlineData(62020, 2, "1 minuto, 2 segundos")]
        [InlineData(62020, 3, "1 minuto, 2 segundos, 20 milisegundos")]
        [InlineData(3600020, 4, "1 hora, 20 milisegundos")]
        [InlineData(3600020, 3, "1 hora")]
        [InlineData(3600020, 2, "1 hora")]
        [InlineData(3600020, 1, "1 hora")]
        [InlineData(3603001, 2, "1 hora")]
        [InlineData(3603001, 3, "1 hora, 3 segundos")]
        [InlineData(86400000, 3, "1 día")]
        [InlineData(86400000, 2, "1 día")]
        [InlineData(86400000, 1, "1 día")]
        [InlineData(86401000, 1, "1 día")]
        [InlineData(86401000, 2, "1 día")]
        [InlineData(86401000, 3, "1 día")]
        [InlineData(86401000, 4, "1 día, 1 segundo")]
        [InlineData(86401200, 4, "1 día, 1 segundo")]
        [InlineData(86401200, 5, "1 día, 1 segundo, 200 milisegundos")]
        [InlineData(1296000000, 1, "2 semanas")]
        [InlineData(1296000000, 2, "2 semanas, 1 día")]
        [InlineData(1299600000, 2, "2 semanas, 1 día")]
        [InlineData(1299600000, 3, "2 semanas, 1 día, 1 hora")]
        [InlineData(1299630020, 3, "2 semanas, 1 día, 1 hora")]
        [InlineData(1299630020, 4, "2 semanas, 1 día, 1 hora")]
        [InlineData(1299630020, 5, "2 semanas, 1 día, 1 hora, 30 segundos")]
        [InlineData(1299630020, 6, "2 semanas, 1 día, 1 hora, 30 segundos, 20 milisegundos")]
        public void TimeSpanWithPrecisionAndCountingEmptyUnits(int milliseconds, int precision, string expected, bool toWords = false)
        {
            var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize(precision: precision, countEmptyUnits: true, toWords: toWords);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 3, "nada", true)]
        [InlineData(0, 2, "nada", true)]
        [InlineData(0, 3, "0 milisegundos")]
        [InlineData(0, 2, "0 milisegundos")]
        [InlineData(10, 2, "10 milisegundos")]
        [InlineData(1400, 2, "1 segundo y 400 milisegundos")]
        [InlineData(2500, 2, "2 segundos y 500 milisegundos")]
        [InlineData(120000, 2, "2 minutos")]
        [InlineData(62000, 2, "1 minuto y 2 segundos")]
        [InlineData(62020, 2, "1 minuto y 2 segundos")]
        [InlineData(62020, 3, "1 minuto, 2 segundos y 20 milisegundos")]
        [InlineData(3600020, 4, "1 hora y 20 milisegundos")]
        [InlineData(3600020, 3, "1 hora y 20 milisegundos")]
        [InlineData(3600020, 2, "1 hora y 20 milisegundos")]
        [InlineData(3600020, 1, "1 hora")]
        [InlineData(3603001, 2, "1 hora y 3 segundos")]
        [InlineData(3603001, 3, "1 hora, 3 segundos y 1 milisegundo")]
        [InlineData(86400000, 3, "1 día")]
        [InlineData(86400000, 2, "1 día")]
        [InlineData(86400000, 1, "1 día")]
        [InlineData(86401000, 1, "1 día")]
        [InlineData(86401000, 2, "1 día y 1 segundo")]
        [InlineData(86401200, 2, "1 día y 1 segundo")]
        [InlineData(86401200, 3, "1 día, 1 segundo y 200 milisegundos")]
        [InlineData(1296000000, 1, "2 semanas")]
        [InlineData(1296000000, 2, "2 semanas y 1 día")]
        [InlineData(1299600000, 2, "2 semanas y 1 día")]
        [InlineData(1299600000, 3, "2 semanas, 1 día y 1 hora")]
        [InlineData(1299630020, 3, "2 semanas, 1 día y 1 hora")]
        [InlineData(1299630020, 4, "2 semanas, 1 día, 1 hora y 30 segundos")]
        [InlineData(1299630020, 5, "2 semanas, 1 día, 1 hora, 30 segundos y 20 milisegundos")]
        public void TimeSpanWithPrecisionAndAlternativeCollectionFormatter(int milliseconds, int precision,
            string expected, bool toWords = false)
        {
            var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize(precision, collectionSeparator: null, toWords: toWords);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 3, "nada")]
        [InlineData(0, 2, "nada")]
        [InlineData(10, 2, "diez milisegundos")]
        [InlineData(1400, 2, "un segundo, cuatrocientos milisegundos")]
        [InlineData(2500, 2, "dos segundos, quinientos milisegundos")]
        [InlineData(120000, 2, "dos minutos")]
        [InlineData(62000, 2, "un minuto, dos segundos")]
        [InlineData(62020, 2, "un minuto, dos segundos")]
        [InlineData(62020, 3, "un minuto, dos segundos, veinte milisegundos")]
        [InlineData(3600020, 4, "una hora, veinte milisegundos")]
        [InlineData(3600020, 3, "una hora, veinte milisegundos")]
        [InlineData(3600020, 2, "una hora, veinte milisegundos")]
        [InlineData(3600020, 1, "una hora")]
        [InlineData(3603001, 2, "una hora, tres segundos")]
        [InlineData(3603001, 3, "una hora, tres segundos, un milisegundo")]
        [InlineData(86400000, 3, "un día")]
        [InlineData(86400000, 2, "un día")]
        [InlineData(86400000, 1, "un día")]
        [InlineData(86401000, 1, "un día")]
        [InlineData(86401000, 2, "un día, un segundo")]
        [InlineData(86401200, 2, "un día, un segundo")]
        [InlineData(86401200, 3, "un día, un segundo, doscientos milisegundos")]
        [InlineData(1296000000, 1, "dos semanas")]
        [InlineData(1296000000, 2, "dos semanas, un día")]
        [InlineData(1299600000, 2, "dos semanas, un día")]
        [InlineData(1299600000, 3, "dos semanas, un día, una hora")]
        [InlineData(1299630020, 3, "dos semanas, un día, una hora")]
        [InlineData(1299630020, 4, "dos semanas, un día, una hora, treinta segundos")]
        [InlineData(1299630020, 5, "dos semanas, un día, una hora, treinta segundos, veinte milisegundos")]
        public void TimeSpanWithNumbersConvertedToWords(int milliseconds, int precision, string expected)
        {
            var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize(precision, toWords: true);
            Assert.Equal(expected, actual);
        }
    }
}
