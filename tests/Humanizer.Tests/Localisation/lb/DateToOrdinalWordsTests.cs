namespace lb;

[UseCulture("lb-LU")]
public class DateToOrdinalWordsTests
{
    [Fact]
    public void OrdinalizeString() =>
        Assert.Equal("1. Abrëll 2015", new DateTime(2015, 4, 1).ToOrdinalWords());

#if NET6_0_OR_GREATER
    [Fact]
    public void OrdinalizeDateOnlyString() =>
        Assert.Equal("1. Abrëll 2015", new DateOnly(2015, 4, 1).ToOrdinalWords());
#endif
}
