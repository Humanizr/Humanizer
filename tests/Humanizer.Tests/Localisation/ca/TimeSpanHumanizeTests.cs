namespace ca;

[UseCulture("ca")]
public class TimeSpanHumanizeTests
{
    [Theory]
    [Trait("Translation", "Google")]
    [InlineData(366, "1 any")]
    [InlineData(731, "2 anys")]
    [InlineData(1096, "3 anys")]
    [InlineData(4018, "11 anys")]
    public void Years(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year));

    [Theory]
    [Trait("Translation", "Google")]
    [InlineData(31, "1 mes")]
    [InlineData(61, "2 mesos")]
    [InlineData(92, "3 mesos")]
    [InlineData(335, "11 mesos")]
    public void Months(int days, string expected) =>
        Assert.Equal(expected, TimeSpan.FromDays(days).Humanize(maxUnit: TimeUnit.Year));

    [Fact]
    public void TwoWeeks() =>
        Assert.Equal("2 setmanes", TimeSpan.FromDays(14).Humanize());

    [Fact]
    public void OneWeek() =>
        Assert.Equal("1 setmana", TimeSpan.FromDays(7).Humanize());

    [Fact]
    public void SixDays() =>
        Assert.Equal("6 dies", TimeSpan.FromDays(6).Humanize());

    [Fact]
    public void TwoDays() =>
        Assert.Equal("2 dies", TimeSpan.FromDays(2).Humanize());

    [Fact]
    public void OneDay() =>
        Assert.Equal("1 dia", TimeSpan.FromDays(1).Humanize());

    [Fact]
    public void TwoHours() =>
        Assert.Equal("2 hores", TimeSpan.FromHours(2).Humanize());

    [Fact]
    public void OneHour() =>
        Assert.Equal("1 hora", TimeSpan.FromHours(1).Humanize());

    [Fact]
    public void TwoMinutes() =>
        Assert.Equal("2 minuts", TimeSpan.FromMinutes(2).Humanize());

    [Fact]
    public void OneMinute() =>
        Assert.Equal("1 minut", TimeSpan.FromMinutes(1).Humanize());

    [Fact]
    public void TwoSeconds() =>
        Assert.Equal("2 segons", TimeSpan.FromSeconds(2).Humanize());

    [Fact]
    public void OneSecond() =>
        Assert.Equal("1 segon", TimeSpan.FromSeconds(1).Humanize());

    [Fact]
    public void TwoMilliseconds() =>
        Assert.Equal("2 mil·lisegons", TimeSpan.FromMilliseconds(2).Humanize());

    [Fact]
    public void OneMillisecond() =>
        Assert.Equal("1 mil·lisegon", TimeSpan.FromMilliseconds(1).Humanize());

    [Theory]
    [InlineData(0, 0, 1, 1, 2, "un minut, un segon")]
    [InlineData(0, 0, 2, 2, 2, "dos minuts, dos segons")]
    [InlineData(1, 2, 3, 4, 4, "un dia, dues hores, tres minuts, quatre segons")]
    public void ComplexTimeSpan(int days, int hours, int minutes, int seconds, int precision, string expected)
    {
        var timeSpan = new TimeSpan(days, hours, minutes, seconds);
        Assert.Equal(expected, timeSpan.Humanize(precision, toWords: true));
    }

    [Fact]
    public void NoTime() =>
        // This one doesn't make a lot of sense but ... w/e
        Assert.Equal("0 mil·lisegons", TimeSpan.Zero.Humanize());

    [Fact]
    public void NoTimeToWords() =>
        // This one doesn't make a lot of sense but ... w/e
        Assert.Equal("res", TimeSpan.Zero.Humanize(toWords: true));

    [Fact]
    public void AllTimeSpansMustBeUniqueForASequenceOfDays()
    {
        var culture = new CultureInfo("ca");
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
    [InlineData(365, "11 mesos, 30 dies")]
    [InlineData(365 + 1, "1 any")]
    [InlineData(365 + 365, "1 any, 11 mesos, 29 dies")]
    [InlineData(365 + 365 + 1, "2 anys")]
    [InlineData(365 + 365 + 365, "2 anys, 11 mesos, 29 dies")]
    [InlineData(365 + 365 + 365 + 1, "3 anys")]
    [InlineData(365 + 365 + 365 + 365, "3 anys, 11 mesos, 29 dies")]
    [InlineData(365 + 365 + 365 + 365 + 1, "4 anys")]
    [InlineData(365 + 365 + 365 + 365 + 366, "4 anys, 11 mesos, 30 dies")]
    [InlineData(365 + 365 + 365 + 365 + 366 + 1, "5 anys")]
    public void Year(int days, string expected)
    {
        var actual = TimeSpan.FromDays(days).Humanize(precision: 7, maxUnit: TimeUnit.Year);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(30, "4 setmanes, 2 dies")]
    [InlineData(30 + 1, "1 mes")]
    [InlineData(30 + 30, "1 mes, 29 dies")]
    [InlineData(30 + 30 + 1, "2 mesos")]
    [InlineData(30 + 30 + 31, "2 mesos, 30 dies")]
    [InlineData(30 + 30 + 31 + 1, "3 mesos")]
    [InlineData(30 + 30 + 31 + 30, "3 mesos, 29 dies")]
    [InlineData(30 + 30 + 31 + 30 + 1, "4 mesos")]
    [InlineData(30 + 30 + 31 + 30 + 31, "4 mesos, 30 dies")]
    [InlineData(30 + 30 + 31 + 30 + 31 + 1, "5 mesos")]
    [InlineData(365, "11 mesos, 30 dies")]
    [InlineData(366, "1 any")]
    public void Month(int days, string expected)
    {
        var actual = TimeSpan.FromDays(days).Humanize(precision: 7, maxUnit: TimeUnit.Year);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(14, "2 setmanes")]
    [InlineData(7, "1 setmana")]
    [InlineData(-14, "2 setmanes")]
    [InlineData(-7, "1 setmana")]
    [InlineData(730, "104 setmanes")]
    public void Weeks(int days, string expected)
    {
        var actual = TimeSpan.FromDays(days).Humanize();
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(6, "6 dies")]
    [InlineData(2, "2 dies")]
    [InlineData(1, "1 dia")]
    [InlineData(-6, "6 dies")]
    [InlineData(-2, "2 dies")]
    [InlineData(-1, "1 dia")]
    public void Days(int days, string expected)
    {
        var actual = TimeSpan.FromDays(days).Humanize();
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(2, "2 hores")]
    [InlineData(1, "1 hora")]
    [InlineData(-2, "2 hores")]
    [InlineData(-1, "1 hora")]
    public void Hours(int hours, string expected)
    {
        var actual = TimeSpan.FromHours(hours).Humanize();
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(2, "2 minuts")]
    [InlineData(1, "1 minut")]
    [InlineData(-2, "2 minuts")]
    [InlineData(-1, "1 minut")]
    public void Minutes(int minutes, string expected)
    {
        var actual = TimeSpan.FromMinutes(minutes).Humanize();
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(135, "2 minuts")]
    [InlineData(60, "1 minut")]
    [InlineData(2, "2 segons")]
    [InlineData(1, "1 segon")]
    [InlineData(-135, "2 minuts")]
    [InlineData(-60, "1 minut")]
    [InlineData(-2, "2 segons")]
    [InlineData(-1, "1 segon")]
    public void Seconds(int seconds, string expected)
    {
        var actual = TimeSpan.FromSeconds(seconds).Humanize();
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(2500, "2 segons")]
    [InlineData(1400, "1 segon")]
    [InlineData(2, "2 mil·lisegons")]
    [InlineData(1, "1 mil·lisegon")]
    [InlineData(-2500, "2 segons")]
    [InlineData(-1400, "1 segon")]
    [InlineData(-2, "2 mil·lisegons")]
    [InlineData(-1, "1 mil·lisegon")]
    public void Milliseconds(int ms, string expected)
    {
        var actual = TimeSpan.FromMilliseconds(ms).Humanize();
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData((long)366 * 24 * 60 * 60 * 1000, "12 mesos", TimeUnit.Month)]
    [InlineData((long)6 * 7 * 24 * 60 * 60 * 1000, "6 setmanes", TimeUnit.Week)]
    [InlineData(7 * 24 * 60 * 60 * 1000, "7 dies", TimeUnit.Day)]
    [InlineData(24 * 60 * 60 * 1000, "24 hores", TimeUnit.Hour)]
    [InlineData(60 * 60 * 1000, "60 minuts", TimeUnit.Minute)]
    [InlineData(60 * 1000, "60 segons", TimeUnit.Second)]
    [InlineData(1000, "1000 mil·lisegons", TimeUnit.Millisecond)]
    public void TimeSpanWithMaxTimeUnit(long ms, string expected, TimeUnit maxUnit)
    {
        var actual = TimeSpan.FromMilliseconds(ms).Humanize(maxUnit: maxUnit);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(10, "10 mil·lisegons", TimeUnit.Millisecond)]
    [InlineData(10, "res", TimeUnit.Second, true)]
    [InlineData(10, "res", TimeUnit.Minute, true)]
    [InlineData(10, "res", TimeUnit.Hour, true)]
    [InlineData(10, "res", TimeUnit.Day, true)]
    [InlineData(10, "res", TimeUnit.Week, true)]
    [InlineData(10, "0 segons", TimeUnit.Second)]
    [InlineData(10, "0 minuts", TimeUnit.Minute)]
    [InlineData(10, "0 hores", TimeUnit.Hour)]
    [InlineData(10, "0 dies", TimeUnit.Day)]
    [InlineData(10, "0 setmanes", TimeUnit.Week)]
    [InlineData(2500, "2 segons, 500 mil·lisegons", TimeUnit.Millisecond)]
    [InlineData(2500, "2 segons", TimeUnit.Second)]
    [InlineData(2500, "res", TimeUnit.Minute, true)]
    [InlineData(2500, "res", TimeUnit.Hour, true)]
    [InlineData(2500, "res", TimeUnit.Day, true)]
    [InlineData(2500, "res", TimeUnit.Week, true)]
    [InlineData(2500, "0 minuts", TimeUnit.Minute)]
    [InlineData(2500, "0 hores", TimeUnit.Hour)]
    [InlineData(2500, "0 dies", TimeUnit.Day)]
    [InlineData(2500, "0 setmanes", TimeUnit.Week)]
    [InlineData(122500, "2 minuts, 2 segons, 500 mil·lisegons", TimeUnit.Millisecond)]
    [InlineData(122500, "2 minuts, 2 segons", TimeUnit.Second)]
    [InlineData(122500, "2 minuts", TimeUnit.Minute)]
    [InlineData(122500, "res", TimeUnit.Hour, true)]
    [InlineData(122500, "res", TimeUnit.Day, true)]
    [InlineData(122500, "res", TimeUnit.Week, true)]
    [InlineData(122500, "0 hores", TimeUnit.Hour)]
    [InlineData(122500, "0 dies", TimeUnit.Day)]
    [InlineData(122500, "0 setmanes", TimeUnit.Week)]
    [InlineData(3722500, "1 hora, 2 minuts, 2 segons, 500 mil·lisegons", TimeUnit.Millisecond)]
    [InlineData(3722500, "1 hora, 2 minuts, 2 segons", TimeUnit.Second)]
    [InlineData(3722500, "1 hora, 2 minuts", TimeUnit.Minute)]
    [InlineData(3722500, "1 hora", TimeUnit.Hour)]
    [InlineData(3722500, "res", TimeUnit.Day, true)]
    [InlineData(3722500, "res", TimeUnit.Week, true)]
    [InlineData(3722500, "0 dies", TimeUnit.Day)]
    [InlineData(3722500, "0 setmanes", TimeUnit.Week)]
    [InlineData(90122500, "1 dia, 1 hora, 2 minuts, 2 segons, 500 mil·lisegons", TimeUnit.Millisecond)]
    [InlineData(90122500, "1 dia, 1 hora, 2 minuts, 2 segons", TimeUnit.Second)]
    [InlineData(90122500, "1 dia, 1 hora, 2 minuts", TimeUnit.Minute)]
    [InlineData(90122500, "1 dia, 1 hora", TimeUnit.Hour)]
    [InlineData(90122500, "1 dia", TimeUnit.Day)]
    [InlineData(90122500, "res", TimeUnit.Week, true)]
    [InlineData(90122500, "0 setmanes", TimeUnit.Week)]
    [InlineData(694922500, "1 setmana, 1 dia, 1 hora, 2 minuts, 2 segons, 500 mil·lisegons", TimeUnit.Millisecond)]
    [InlineData(694922500, "1 setmana, 1 dia, 1 hora, 2 minuts, 2 segons", TimeUnit.Second)]
    [InlineData(694922500, "1 setmana, 1 dia, 1 hora, 2 minuts", TimeUnit.Minute)]
    [InlineData(694922500, "1 setmana, 1 dia, 1 hora", TimeUnit.Hour)]
    [InlineData(694922500, "1 setmana, 1 dia", TimeUnit.Day)]
    [InlineData(694922500, "1 setmana", TimeUnit.Week)]
    [InlineData(2768462500, "1 mes, 1 dia, 1 hora, 1 minut, 2 segons, 500 mil·lisegons", TimeUnit.Millisecond)]
    [InlineData(2768462500, "1 mes, 1 dia, 1 hora, 1 minut, 2 segons", TimeUnit.Second)]
    [InlineData(2768462500, "1 mes, 1 dia, 1 hora, 1 minut", TimeUnit.Minute)]
    [InlineData(2768462500, "1 mes, 1 dia, 1 hora", TimeUnit.Hour)]
    [InlineData(2768462500, "1 mes, 1 dia", TimeUnit.Day)]
    [InlineData(2768462500, "1 mes", TimeUnit.Week)]
    [InlineData(2768462500, "1 mes", TimeUnit.Month)]
    [InlineData(2768462500, "res", TimeUnit.Year, true)]
    [InlineData(2768462500, "0 anys", TimeUnit.Year)]
    [InlineData(34390862500, "1 any, 1 mes, 2 dies, 1 hora, 1 minut, 2 segons, 500 mil·lisegons", TimeUnit.Millisecond)]
    [InlineData(34390862500, "1 any, 1 mes, 2 dies, 1 hora, 1 minut, 2 segons", TimeUnit.Second)]
    [InlineData(34390862500, "1 any, 1 mes, 2 dies, 1 hora, 1 minut", TimeUnit.Minute)]
    [InlineData(34390862500, "1 any, 1 mes, 2 dies, 1 hora", TimeUnit.Hour)]
    [InlineData(34390862500, "1 any, 1 mes, 2 dies", TimeUnit.Day)]
    [InlineData(34390862500, "1 any, 1 mes", TimeUnit.Week)]
    [InlineData(34390862500, "1 any, 1 mes", TimeUnit.Month)]
    [InlineData(34390862500, "1 any", TimeUnit.Year)]
    public void TimeSpanWithMinTimeUnit(long ms, string expected, TimeUnit minUnit, bool toWords = false)
    {
        var actual = TimeSpan.FromMilliseconds(ms).Humanize(minUnit: minUnit, precision: 7, maxUnit: TimeUnit.Year, toWords: toWords);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 3, "res", true)]
    [InlineData(0, 2, "res", true)]
    [InlineData(0, 3, "0 mil·lisegons")]
    [InlineData(0, 2, "0 mil·lisegons")]
    [InlineData(10, 2, "10 mil·lisegons")]
    [InlineData(1400, 2, "1 segon, 400 mil·lisegons")]
    [InlineData(2500, 2, "2 segons, 500 mil·lisegons")]
    [InlineData(120000, 2, "2 minuts")]
    [InlineData(62000, 2, "1 minut, 2 segons")]
    [InlineData(62020, 2, "1 minut, 2 segons")]
    [InlineData(62020, 3, "1 minut, 2 segons, 20 mil·lisegons")]
    [InlineData(3600020, 4, "1 hora, 20 mil·lisegons")]
    [InlineData(3600020, 3, "1 hora, 20 mil·lisegons")]
    [InlineData(3600020, 2, "1 hora, 20 mil·lisegons")]
    [InlineData(3600020, 1, "1 hora")]
    [InlineData(3603001, 2, "1 hora, 3 segons")]
    [InlineData(3603001, 3, "1 hora, 3 segons, 1 mil·lisegon")]
    [InlineData(86400000, 3, "1 dia")]
    [InlineData(86400000, 2, "1 dia")]
    [InlineData(86400000, 1, "1 dia")]
    [InlineData(86401000, 1, "1 dia")]
    [InlineData(86401000, 2, "1 dia, 1 segon")]
    [InlineData(86401200, 2, "1 dia, 1 segon")]
    [InlineData(86401200, 3, "1 dia, 1 segon, 200 mil·lisegons")]
    [InlineData(1296000000, 1, "2 setmanes")]
    [InlineData(1296000000, 2, "2 setmanes, 1 dia")]
    [InlineData(1299600000, 2, "2 setmanes, 1 dia")]
    [InlineData(1299600000, 3, "2 setmanes, 1 dia, 1 hora")]
    [InlineData(1299630020, 3, "2 setmanes, 1 dia, 1 hora")]
    [InlineData(1299630020, 4, "2 setmanes, 1 dia, 1 hora, 30 segons")]
    [InlineData(1299630020, 5, "2 setmanes, 1 dia, 1 hora, 30 segons, 20 mil·lisegons")]
    [InlineData(2768462500, 6, "1 mes, 1 dia, 1 hora, 1 minut, 2 segons, 500 mil·lisegons")]
    [InlineData(2768462500, 5, "1 mes, 1 dia, 1 hora, 1 minut, 2 segons")]
    [InlineData(2768462500, 4, "1 mes, 1 dia, 1 hora, 1 minut")]
    [InlineData(2768462500, 3, "1 mes, 1 dia, 1 hora")]
    [InlineData(2768462500, 2, "1 mes, 1 dia")]
    [InlineData(2768462500, 1, "1 mes")]
    [InlineData(34390862500, 7, "1 any, 1 mes, 2 dies, 1 hora, 1 minut, 2 segons, 500 mil·lisegons")]
    [InlineData(34390862500, 6, "1 any, 1 mes, 2 dies, 1 hora, 1 minut, 2 segons")]
    [InlineData(34390862500, 5, "1 any, 1 mes, 2 dies, 1 hora, 1 minut")]
    [InlineData(34390862500, 4, "1 any, 1 mes, 2 dies, 1 hora")]
    [InlineData(34390862500, 3, "1 any, 1 mes, 2 dies")]
    [InlineData(34390862500, 2, "1 any, 1 mes")]
    [InlineData(34390862500, 1, "1 any")]
    public void TimeSpanWithPrecision(long milliseconds, int precision, string expected, bool toWords = false)
    {
        var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize(precision, maxUnit: TimeUnit.Year, toWords: toWords);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(3 * 7 + 4, 2, "3 setmanes, 4 dies")]
    [InlineData(6 * 7 + 3, 2, "6 setmanes, 3 dies")]
    [InlineData(72 * 7 + 6, 2, "72 setmanes, 6 dies")]
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
    [InlineData(0, 3, "res", true)]
    [InlineData(0, 2, "res", true)]
    [InlineData(0, 3, "0 mil·lisegons")]
    [InlineData(0, 2, "0 mil·lisegons")]
    [InlineData(10, 2, "10 mil·lisegons")]
    [InlineData(1400, 2, "1 segon, 400 mil·lisegons")]
    [InlineData(2500, 2, "2 segons, 500 mil·lisegons")]
    [InlineData(60001, 1, "1 minut")]
    [InlineData(60001, 2, "1 minut")]
    [InlineData(60001, 3, "1 minut, 1 mil·lisegon")]
    [InlineData(120000, 2, "2 minuts")]
    [InlineData(62000, 2, "1 minut, 2 segons")]
    [InlineData(62020, 2, "1 minut, 2 segons")]
    [InlineData(62020, 3, "1 minut, 2 segons, 20 mil·lisegons")]
    [InlineData(3600020, 4, "1 hora, 20 mil·lisegons")]
    [InlineData(3600020, 3, "1 hora")]
    [InlineData(3600020, 2, "1 hora")]
    [InlineData(3600020, 1, "1 hora")]
    [InlineData(3603001, 2, "1 hora")]
    [InlineData(3603001, 3, "1 hora, 3 segons")]
    [InlineData(86400000, 3, "1 dia")]
    [InlineData(86400000, 2, "1 dia")]
    [InlineData(86400000, 1, "1 dia")]
    [InlineData(86401000, 1, "1 dia")]
    [InlineData(86401000, 2, "1 dia")]
    [InlineData(86401000, 3, "1 dia")]
    [InlineData(86401000, 4, "1 dia, 1 segon")]
    [InlineData(86401200, 4, "1 dia, 1 segon")]
    [InlineData(86401200, 5, "1 dia, 1 segon, 200 mil·lisegons")]
    [InlineData(1296000000, 1, "2 setmanes")]
    [InlineData(1296000000, 2, "2 setmanes, 1 dia")]
    [InlineData(1299600000, 2, "2 setmanes, 1 dia")]
    [InlineData(1299600000, 3, "2 setmanes, 1 dia, 1 hora")]
    [InlineData(1299630020, 3, "2 setmanes, 1 dia, 1 hora")]
    [InlineData(1299630020, 4, "2 setmanes, 1 dia, 1 hora")]
    [InlineData(1299630020, 5, "2 setmanes, 1 dia, 1 hora, 30 segons")]
    [InlineData(1299630020, 6, "2 setmanes, 1 dia, 1 hora, 30 segons, 20 mil·lisegons")]
    public void TimeSpanWithPrecisionAndCountingEmptyUnits(int milliseconds, int precision, string expected, bool toWords = false)
    {
        var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize(precision: precision, countEmptyUnits: true, toWords: toWords);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 3, "res", true)]
    [InlineData(0, 2, "res", true)]
    [InlineData(0, 3, "0 mil·lisegons")]
    [InlineData(0, 2, "0 mil·lisegons")]
    [InlineData(10, 2, "10 mil·lisegons")]
    [InlineData(1400, 2, "1 segon i 400 mil·lisegons")]
    [InlineData(2500, 2, "2 segons i 500 mil·lisegons")]
    [InlineData(120000, 2, "2 minuts")]
    [InlineData(62000, 2, "1 minut i 2 segons")]
    [InlineData(62020, 2, "1 minut i 2 segons")]
    [InlineData(62020, 3, "1 minut, 2 segons i 20 mil·lisegons")]
    [InlineData(3600020, 4, "1 hora i 20 mil·lisegons")]
    [InlineData(3600020, 3, "1 hora i 20 mil·lisegons")]
    [InlineData(3600020, 2, "1 hora i 20 mil·lisegons")]
    [InlineData(3600020, 1, "1 hora")]
    [InlineData(3603001, 2, "1 hora i 3 segons")]
    [InlineData(3603001, 3, "1 hora, 3 segons i 1 mil·lisegon")]
    [InlineData(86400000, 3, "1 dia")]
    [InlineData(86400000, 2, "1 dia")]
    [InlineData(86400000, 1, "1 dia")]
    [InlineData(86401000, 1, "1 dia")]
    [InlineData(86401000, 2, "1 dia i 1 segon")]
    [InlineData(86401200, 2, "1 dia i 1 segon")]
    [InlineData(86401200, 3, "1 dia, 1 segon i 200 mil·lisegons")]
    [InlineData(1296000000, 1, "2 setmanes")]
    [InlineData(1296000000, 2, "2 setmanes i 1 dia")]
    [InlineData(1299600000, 2, "2 setmanes i 1 dia")]
    [InlineData(1299600000, 3, "2 setmanes, 1 dia i 1 hora")]
    [InlineData(1299630020, 3, "2 setmanes, 1 dia i 1 hora")]
    [InlineData(1299630020, 4, "2 setmanes, 1 dia, 1 hora i 30 segons")]
    [InlineData(1299630020, 5, "2 setmanes, 1 dia, 1 hora, 30 segons i 20 mil·lisegons")]
    public void TimeSpanWithPrecisionAndAlternativeCollectionFormatter(int milliseconds, int precision,
        string expected, bool toWords = false)
    {
        var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize(precision, collectionSeparator: null, toWords: toWords);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, 3, "res")]
    [InlineData(0, 2, "res")]
    [InlineData(10, 2, "deu mil·lisegons")]
    [InlineData(1400, 2, "un segon, quatre-cents mil·lisegons")]
    [InlineData(2500, 2, "dos segons, cinc-cents mil·lisegons")]
    [InlineData(120000, 2, "dos minuts")]
    [InlineData(62000, 2, "un minut, dos segons")]
    [InlineData(62020, 2, "un minut, dos segons")]
    [InlineData(62020, 3, "un minut, dos segons, vint mil·lisegons")]
    [InlineData(3600020, 4, "una hora, vint mil·lisegons")]
    [InlineData(3600020, 3, "una hora, vint mil·lisegons")]
    [InlineData(3600020, 2, "una hora, vint mil·lisegons")]
    [InlineData(3600020, 1, "una hora")]
    [InlineData(3603001, 2, "una hora, tres segons")]
    [InlineData(3603001, 3, "una hora, tres segons, un mil·lisegon")]
    [InlineData(86400000, 3, "un dia")]
    [InlineData(86400000, 2, "un dia")]
    [InlineData(86400000, 1, "un dia")]
    [InlineData(86401000, 1, "un dia")]
    [InlineData(86401000, 2, "un dia, un segon")]
    [InlineData(86401200, 2, "un dia, un segon")]
    [InlineData(86401200, 3, "un dia, un segon, dos-cents mil·lisegons")]
    [InlineData(1296000000, 1, "dues setmanes")]
    [InlineData(1296000000, 2, "dues setmanes, un dia")]
    [InlineData(1299600000, 2, "dues setmanes, un dia")]
    [InlineData(1299600000, 3, "dues setmanes, un dia, una hora")]
    [InlineData(1299630020, 3, "dues setmanes, un dia, una hora")]
    [InlineData(1299630020, 4, "dues setmanes, un dia, una hora, trenta segons")]
    [InlineData(1299630020, 5, "dues setmanes, un dia, una hora, trenta segons, vint mil·lisegons")]
    public void TimeSpanWithNumbersConvertedToWords(int milliseconds, int precision, string expected)
    {
        var actual = TimeSpan.FromMilliseconds(milliseconds).Humanize(precision, toWords: true);
        Assert.Equal(expected, actual);
    }
}