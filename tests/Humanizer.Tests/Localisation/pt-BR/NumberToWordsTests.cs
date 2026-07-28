namespace Humanizer.Tests.Localisation.pt_BR;

[UseCulture("pt-BR")]
public class NumberToWordsTests
{
    [Theory]
    [InlineData(15_200, true, "quinze mil e duzentos")]
    [InlineData(15_200, false, "quinze mil e duzentos")]
    [InlineData(1_520, true, "mil quinhentos e vinte")]
    [InlineData(152_000, true, "cento e cinquenta e dois mil")]
    public void ToWordsAddsAndBeforeExactHundredsAfterScale(int number, bool addAnd, string expected) =>
        Assert.Equal(expected, number.ToWords(addAnd));
}