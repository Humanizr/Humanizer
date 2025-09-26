namespace ca;

[UseCulture("ca")]
public class DateToOrdinalWordsTests
{
    [Fact]
    public void OrdinalizeString()
    {
        Assert.Equal("25 de gener de 2022", new DateTime(2022, 1, 25).ToOrdinalWords());
        Assert.Equal("29 de febrer de 2020", new DateTime(2020, 2, 29).ToOrdinalWords());
        Assert.Equal("4 de setembre de 2015", new DateTime(2015, 9, 4).ToOrdinalWords());
        Assert.Equal("7 de novembre de 1979", new DateTime(1979, 11, 7).ToOrdinalWords());
    }

#if NET6_0_OR_GREATER
    [Fact]
    public void OrdinalizeDateOnlyString()
    {
        Assert.Equal("25 de gener de 2022", new DateOnly(2022, 1, 25).ToOrdinalWords());
        Assert.Equal("29 de febrer de 2020", new DateOnly(2020, 2, 29).ToOrdinalWords());
        Assert.Equal("4 de setembre de 2015", new DateOnly(2015, 9, 4).ToOrdinalWords());
        Assert.Equal("7 de novembre de 1979", new DateOnly(1979, 11, 7).ToOrdinalWords());
    }
#endif
}