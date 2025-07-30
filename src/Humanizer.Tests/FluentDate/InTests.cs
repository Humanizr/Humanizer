public class InTests
{
    [Fact]
    public void InJanuary() =>
        Assert.Equal(new(DateTime.Now.Year, 1, 1), In.January);

    [Fact]
    public void InJanuaryOf2009() =>
        Assert.Equal(new(2009, 1, 1), In.JanuaryOf(2009));

    [Fact]
    public void InFebruary() =>
        Assert.Equal(new(DateTime.Now.Year, 2, 1), In.February);

    [Fact]
    public void InFebruaryOf2009() =>
        Assert.Equal(new(2009, 2, 1), In.FebruaryOf(2009));

    [Fact]
    public void InMarch() =>
        Assert.Equal(new(DateTime.Now.Year, 3, 1), In.March);

    [Fact]
    public void InMarchOf2009() =>
        Assert.Equal(new(2009, 3, 1), In.MarchOf(2009));

    [Fact]
    public void InApril() =>
        Assert.Equal(new(DateTime.Now.Year, 4, 1), In.April);

    [Fact]
    public void InAprilOf2009() =>
        Assert.Equal(new(2009, 4, 1), In.AprilOf(2009));

    [Fact]
    public void InMay() =>
        Assert.Equal(new(DateTime.Now.Year, 5, 1), In.May);

    [Fact]
    public void InMayOf2009() =>
        Assert.Equal(new(2009, 5, 1), In.MayOf(2009));

    [Fact]
    public void InJune() =>
        Assert.Equal(new(DateTime.Now.Year, 6, 1), In.June);

    [Fact]
    public void InJuneOf2009() =>
        Assert.Equal(new(2009, 6, 1), In.JuneOf(2009));

    [Fact]
    public void InJuly() =>
        Assert.Equal(new(DateTime.Now.Year, 7, 1), In.July);

    [Fact]
    public void InJulyOf2009() =>
        Assert.Equal(new(2009, 7, 1), In.JulyOf(2009));

    [Fact]
    public void InAugust() =>
        Assert.Equal(new(DateTime.Now.Year, 8, 1), In.August);

    [Fact]
    public void InAugustOf2009() =>
        Assert.Equal(new(2009, 8, 1), In.AugustOf(2009));

    [Fact]
    public void InSeptember() =>
        Assert.Equal(new(DateTime.Now.Year, 9, 1), In.September);

    [Fact]
    public void InSeptemberOf2009() =>
        Assert.Equal(new(2009, 9, 1), In.SeptemberOf(2009));

    [Fact]
    public void InOctober() =>
        Assert.Equal(new(DateTime.Now.Year, 10, 1), In.October);

    [Fact]
    public void InOctoberOfIn2009() =>
        Assert.Equal(new(2009, 10, 1), In.OctoberOf(2009));

    [Fact]
    public void InNovember() =>
        Assert.Equal(new(DateTime.Now.Year, 11, 1), In.November);

    [Fact]
    public void InNovemberOf2009() =>
        Assert.Equal(new(2009, 11, 1), In.NovemberOf(2009));

    [Fact]
    public void InDecember() =>
        Assert.Equal(new(DateTime.Now.Year, 12, 1), In.December);

    [Fact]
    public void InDecemberOf2009() =>
        Assert.Equal(new(2009, 12, 1), In.DecemberOf(2009));

    [Fact]
    public void InTheYear() =>
        Assert.Equal(new(2009, 1, 1), In.TheYear(2009));

    [Fact]
    public void InFiveDays()
    {
        var baseDate = On.January.The21st;
        var date = In.Five.DaysFrom(baseDate);
        Assert.Equal(baseDate.AddDays(5), date);
    }
}