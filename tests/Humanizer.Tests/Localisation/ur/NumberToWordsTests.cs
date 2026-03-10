namespace ur;

[UseCulture("ur")]
public class NumberToWordsTests
{
    [Theory]
    [InlineData(0, "صفر")]
    [InlineData(1, "ایک")]
    [InlineData(2, "دو")]
    [InlineData(3, "تین")]
    [InlineData(4, "چار")]
    [InlineData(5, "پانچ")]
    [InlineData(10, "دس")]
    [InlineData(11, "گیارہ")]
    [InlineData(15, "پندرہ")]
    [InlineData(19, "انیس")]
    [InlineData(20, "بیس")]
    [InlineData(25, "پچیس")]
    [InlineData(30, "تیس")]
    [InlineData(40, "چالیس")]
    [InlineData(50, "پچاس")]
    [InlineData(60, "ساٹھ")]
    [InlineData(70, "ستر")]
    [InlineData(80, "اسی")]
    [InlineData(90, "نوے")]
    [InlineData(99, "ننانوے")]
    public void ToWordsUrdu(int number, string expected) =>
        Assert.Equal(expected, number.ToWords());

    [Theory]
    [InlineData(100, "ایک سو")]
    [InlineData(200, "دو سو")]
    [InlineData(300, "تین سو")]
    [InlineData(111, "ایک سو گیارہ")]
    [InlineData(150, "ایک سو پچاس")]
    [InlineData(999, "نو سو ننانوے")]
    public void ToWordsUrduHundreds(int number, string expected) =>
        Assert.Equal(expected, number.ToWords());

    [Theory]
    [InlineData(1000, "ایک ہزار")]
    [InlineData(2000, "دو ہزار")]
    [InlineData(5000, "پانچ ہزار")]
    [InlineData(1500, "ایک ہزار پانچ سو")]
    [InlineData(3501, "تین ہزار پانچ سو ایک")]
    [InlineData(10000, "دس ہزار")]
    [InlineData(99999, "ننانوے ہزار نو سو ننانوے")]
    public void ToWordsUrduThousands(int number, string expected) =>
        Assert.Equal(expected, number.ToWords());

    [Theory]
    [InlineData(100000, "ایک لاکھ")]
    [InlineData(200000, "دو لاکھ")]
    [InlineData(500000, "پانچ لاکھ")]
    [InlineData(125000, "ایک لاکھ پچیس ہزار")]
    public void ToWordsUrduLakhs(int number, string expected) =>
        Assert.Equal(expected, number.ToWords());

    [Theory]
    [InlineData(10000000, "ایک کروڑ")]
    [InlineData(50000000, "پانچ کروڑ")]
    [InlineData(12500000, "ایک کروڑ پچیس لاکھ")]
    public void ToWordsUrduCrores(int number, string expected) =>
        Assert.Equal(expected, number.ToWords());

    [Theory]
    [InlineData(1000000000L, "ایک ارب")]
    [InlineData(5000000000L, "پانچ ارب")]
    public void ToWordsUrduArabs(long number, string expected) =>
        Assert.Equal(expected, number.ToWords());

    [Theory]
    [InlineData(-1, "منفی ایک")]
    [InlineData(-25, "منفی پچیس")]
    [InlineData(-1000, "منفی ایک ہزار")]
    public void ToWordsUrduNegative(int number, string expected) =>
        Assert.Equal(expected, number.ToWords());

    [Theory]
    [InlineData(1, "پہلا")]
    [InlineData(2, "دوسرا")]
    [InlineData(3, "تیسرا")]
    [InlineData(4, "چوتھا")]
    [InlineData(5, "پانچواں")]
    [InlineData(6, "چھٹا")]
    [InlineData(7, "ساتواں")]
    [InlineData(8, "آٹھواں")]
    [InlineData(9, "نواں")]
    [InlineData(10, "دسواں")]
    public void ToOrdinalWords(int number, string expected) =>
        Assert.Equal(expected, number.ToOrdinalWords());

    [Theory]
    [InlineData(11, "گیارہواں")]
    [InlineData(20, "بیسواں")]
    [InlineData(50, "پچاسواں")]
    [InlineData(100, "ایک سوواں")]
    public void ToOrdinalWordsLarger(int number, string expected) =>
        Assert.Equal(expected, number.ToOrdinalWords());
}
