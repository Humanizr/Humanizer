namespace lt;

[UseCulture("lt")]
public class DateToOrdinalWordsTests
{
    [Fact]
    public void OrdinalizeString() =>
        Assert.Equal("2015 m. sausio 1 d.", new DateTime(2015, 1, 1).ToOrdinalWords());

#if NET6_0_OR_GREATER
    [Fact]
    public void OrdinalizeDateOnlyString() =>
        Assert.Equal("2015 m. sausio 1 d.", new DateOnly(2015, 1, 1).ToOrdinalWords());
#endif
}