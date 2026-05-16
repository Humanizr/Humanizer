namespace Humanizer.Tests.Localisation.or;

[UseCulture("or")]
public class OdiaLocaleTests
{
    static readonly CultureInfo Odia = new("or");
    static readonly int[] Pair = [1, 2];
    static readonly int[] Triple = [1, 2, 3];

    [Fact]
    public void ListHumanize_UsesOdiaConjunction()
    {
        Assert.Equal("1 ଏବଂ 2", Pair.Humanize());
        Assert.Equal("1, 2 ଏବଂ 3", Triple.Humanize());
    }

    [Theory]
    [InlineData(1, TimeUnit.Second, Tense.Past, "ଏକ ସେକେଣ୍ଡ ପୂର୍ବରୁ")]
    [InlineData(2, TimeUnit.Second, Tense.Future, "2 ସେକେଣ୍ଡ ପରେ")]
    [InlineData(1, TimeUnit.Day, Tense.Past, "ଗତକାଲି")]
    [InlineData(1, TimeUnit.Day, Tense.Future, "ଆସନ୍ତାକାଲି")]
    [InlineData(2, TimeUnit.Day, Tense.Past, "2 ଦିନ ପୂର୍ବରୁ")]
    [InlineData(2, TimeUnit.Day, Tense.Future, "2 ଦିନ ପରେ")]
    [InlineData(0, TimeUnit.Second, Tense.Future, "ବର୍ତ୍ତମାନ")]
    public void RelativeDate_UsesOdiaPhrases(int count, TimeUnit unit, Tense tense, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Odia);
        Assert.Equal(expected, formatter.DateHumanize(unit, tense, count));
    }

    [Fact]
    public void NullableDateHumanize_NullDateUsesOdiaNeverPhrase()
    {
        DateTime? date = null;
        Assert.Equal("କେବେ ନୁହେଁ", date.Humanize(culture: Odia));
    }

    [Theory]
    [InlineData(1, "1 ସେକେଣ୍ଡ")]
    [InlineData(2, "2 ସେକେଣ୍ଡ")]
    public void Duration_UsesOdiaUnitForms(int seconds, string expected)
    {
        Assert.Equal(expected, TimeSpan.FromSeconds(seconds).Humanize(culture: Odia));
    }

    [Fact]
    public void Duration_ToWordsUsesOdiaNumberWords()
    {
        Assert.Equal("ଏକ ଘଣ୍ଟା", TimeSpan.FromHours(1).Humanize(toWords: true, culture: Odia));
    }

    [Theory]
    [InlineData(DataUnit.Bit, 1, "ବିଟ")]
    [InlineData(DataUnit.Byte, 2, "ବାଇଟ୍")]
    [InlineData(DataUnit.Megabyte, 2, "ମେଗାବାଇଟ୍")]
    public void DataUnit_FullWordForms(DataUnit unit, int count, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Odia);
        Assert.Equal(expected, formatter.DataUnitHumanize(unit, count, toSymbol: false));
    }

    [Theory]
    [InlineData(TimeUnit.Second, "ସେକେଣ୍ଡ")]
    [InlineData(TimeUnit.Minute, "ମିନିଟ୍")]
    [InlineData(TimeUnit.Hour, "ଘଣ୍ଟା")]
    [InlineData(TimeUnit.Day, "ଦିନ")]
    public void TimeUnitSymbols(TimeUnit unit, string expected)
    {
        var formatter = Configurator.Formatters.ResolveForCulture(Odia);
        Assert.Equal(expected, formatter.TimeUnitHumanize(unit));
    }

    [Theory]
    [InlineData(0, "ଶୂନ୍ୟ")]
    [InlineData(21, "ଏକୋଇଶ")]
    [InlineData(68, "ଅଠଷଠି")]
    [InlineData(100, "ଏକ ଶହ")]
    [InlineData(101, "ଏକ ଶହ ଏକ")]
    [InlineData(999, "ନଅ ଶହ ଅନେଶତ")]
    [InlineData(1000, "ଏକ ହଜାର")]
    [InlineData(100000, "ଏକ ଲକ୍ଷ")]
    [InlineData(12345678, "ଏକ କୋଟି ତେଇଶି ଲକ୍ଷ ପଇଞ୍ଚାଳିଶ ହଜାର ଛଅ ଶହ ଅଠସ୍ତରି")]
    public void NumberToWords_ProducesExpectedOutput(long number, string expected)
    {
        Assert.Equal(expected, number.ToWords(Odia));
    }

    [Theory]
    [InlineData("କୋଡିଏ", 20)]
    [InlineData("ଅଠଷଠି", 68)]
    [InlineData("ଏକୋଇଶ", 21)]
    [InlineData("ଏକ ଶହ ଏକ", 101)]
    [InlineData("ଏକ କୋଟି ତେଇଶି ଲକ୍ଷ ପଇଞ୍ଚାଳିଶ ହଜାର ଛଅ ଶହ ଅଠସ୍ତରି", 12345678)]
    public void WordsToNumber_ParsesOdiaCardinals(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Odia));
    }

    [Theory]
    [InlineData(1, "ପ୍ରଥମ")]
    [InlineData(2, "ଦ୍ୱିତୀୟ")]
    [InlineData(21, "ଏକୋଇଶତମ")]
    [InlineData(27, "ସତାଇଶତମ")]
    public void Ordinalize_UsesOdiaOrdinalWords(int number, string expected)
    {
        Assert.Equal(expected, number.Ordinalize(Odia));
        Assert.Equal(expected, number.ToOrdinalWords(Odia));
    }

    [Theory]
    [InlineData("ପ୍ରଥମ", 1)]
    [InlineData("ଏକୋଇଶତମ", 21)]
    [InlineData("ସତାଇଶତମ", 27)]
    public void WordsToNumber_ParsesOdiaOrdinalWords(string words, long expected)
    {
        Assert.Equal(expected, words.ToNumber(Odia));
    }

    [Theory]
    [InlineData(2022, 1, 25, "25 ଜାନୁଆରୀ 2022")]
    [InlineData(2015, 2, 3, "3 ଫେବୃଆରୀ 2015")]
    [InlineData(2024, 12, 31, "31 ଡିସେମ୍ବର 2024")]
    public void DateTime_ToOrdinalWords_ExactOutput(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateTime(year, month, day).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Theory]
    [InlineData(2022, 1, 25, "25 ଜାନୁଆରୀ 2022")]
    [InlineData(2015, 2, 3, "3 ଫେବୃଆରୀ 2015")]
    public void DateOnly_ToOrdinalWords_ExactOutput(int year, int month, int day, string expected)
    {
        Assert.Equal(expected, new DateOnly(year, month, day).ToOrdinalWords());
    }

    [Theory]
    [InlineData(1, 0, "ସକାଳ ଏକଟା")]
    [InlineData(1, 5, "ସକାଳ ଏକଟା ପାଞ୍ଚ ମିନିଟ୍")]
    [InlineData(13, 23, "ଦ୍ୱିପ୍ରହର ଏକଟା ତେଇଶି ମିନିଟ୍")]
    [InlineData(18, 0, "ସନ୍ଧ୍ୟା ଛଅଟା")]
    [InlineData(21, 0, "ରାତି ନଅଟା")]
    public void ClockNotation_ExactOutput(int hours, int minutes, string expected)
    {
        Assert.Equal(expected, new TimeOnly(hours, minutes).ToClockNotation());
    }

    [Fact]
    public void ClockNotation_Rounded_ExactOutput()
    {
        Assert.Equal("ଦ୍ୱିପ୍ରହର ଏକଟା ପଚିଶି ମିନିଟ୍", new TimeOnly(13, 23).ToClockNotation(ClockNotationRounding.NearestFiveMinutes));
    }
#endif

    [Theory]
    [InlineData(0.0, "ଉତ୍ତର")]
    [InlineData(45.0, "ଉତ୍ତର-ପୂର୍ବ")]
    [InlineData(90.0, "ପୂର୍ବ")]
    [InlineData(180.0, "ଦକ୍ଷିଣ")]
    [InlineData(270.0, "ପଶ୍ଚିମ")]
    public void Compass_FullDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Full));
    }

    [Theory]
    [InlineData(0.0, "ଉ")]
    [InlineData(45.0, "ଉ-ପୂ")]
    [InlineData(90.0, "ପୂ")]
    public void Compass_AbbreviatedDirections(double angle, string expected)
    {
        Assert.Equal(expected, angle.ToHeading(HeadingStyle.Abbreviated));
    }
}